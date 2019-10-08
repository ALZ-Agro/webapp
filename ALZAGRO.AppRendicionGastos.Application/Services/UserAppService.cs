using AutoMapper;
using ALZAGRO.AppRendicionGastos.Fwk;
using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Culture;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Email;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace ALZAGRO.AppRendicionGastos.Application.Services
{
    public class UserAppService : EntityBaseAppService<User, UserDto>, IUserAppService
    {

        #region Variables

        private readonly IEncryptionService encryptionService;
        private readonly IMembershipService membershipService;
        private readonly IEntityBaseRepository<UserCompanyGroup> userCompanyGroupRepository;

        private readonly ITimeService timeService;
        #endregion

        public UserAppService(IEntityBaseRepository<User> usersRepository,
                              IEntityBaseRepository<Error> errorsRepository,
                              IEntityBaseRepository<UserCompanyGroup> userCompanyGroupRepository,
                              IUnitOfWork unitOfWork,
                              IEncryptionService encryptionService,
                              IMembershipService membershipService,
                              ITimeService timeService) :
            base(errorsRepository, unitOfWork, usersRepository)
        {

            this.encryptionService = encryptionService;
            this.userCompanyGroupRepository = userCompanyGroupRepository;
            this.membershipService = membershipService;
            this.timeService = timeService;
        }

        public UserDto GetUser(long UserId) {
            return Mapper.Map<User,UserDto>(this.entityRepository.GetAllIncluding(x => x.Role,
                                                                                 x => x.UserCompanyGroups,
                                                                                 x => x.UserCompanyGroups.Select(y => y.Company),
                                                                                 x => x.UserCompanyGroups.Select(y => y.UserGroup)).
                                                                  Where(x => x.Id == UserId).
                                                                  FirstOrDefault());
        }

        public UserDto GetByUserName(string userName)
        {
            var userDto = this.GetAll().Where(x => x.Username == userName).FirstOrDefault();
            return userDto;
        }

        public UserDto GetByEmail(string email)
        {
            return Mapper.Map<User, UserDto>(this.entityRepository.GetAllIncluding(x => x.Role,
                                                                                 x => x.UserCompanyGroups,
                                                                                 x => x.UserCompanyGroups.Select(y => y.Company),
                                                                                 x => x.UserCompanyGroups.Select(y => y.UserGroup)).
                                                                  Where(x => x.Email == email).
                                                                  FirstOrDefault());
        }

        public List<SearchUserDto> GetForFilter() {
            return Mapper.Map<List<User>, List<SearchUserDto>>(this.entityRepository.GetAll().ToList());
        }


        public SearchResultViewModel<UserDto> Search(UserListViewCriteria criteria)
        {

            var query = this.entityRepository.GetAllIncluding(x => x.UserCompanyGroups,
                                                              x => x.UserCompanyGroups.Select(y => y.Company),
                                                              x => x.UserCompanyGroups.Select(y => y.UserGroup));

            if (!string.IsNullOrEmpty(criteria.PartialDescription))
            {
                query = this.MatchInFields<User>(query, criteria.PartialDescription, true, c => new[] {
                    c.Email.ToLower(),
                    c.Username.ToLower(),
                    c.FirstName.ToLower(),
                    c.LastName.ToLower()
                });
            }
            if (criteria.RoleId != 0) {
                query = query.Where(x => x.RoleId == criteria.RoleId);
            }


            return this.CreateResult<User, UserDto>(query, criteria, "Username");
        }

        public override UserDto Save(UserDto userDto)
        {

            var sendMail = false;
            if (userDto.Id == 0)
            {
                if (userDto.Password == null) {
                    userDto.Password = this.membershipService.CreateRandomPassword();
                }


                var user = Mapper.Map<UserDto, User>(userDto);

                var passwordSalt = this.encryptionService.CreateSalt();

                user.Salt = passwordSalt;
                user.HashedPassword = this.encryptionService.EncryptPassword(userDto.Password, passwordSalt);
                user.Status = 1;
                this.entityRepository.Add(user);

                this.unitOfWork.Commit();

                userDto.Id = user.Id;
                sendMail = true;
            }
            else
            {
                var user = this.entityRepository.GetSingle(userDto.Id);
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.RoleId = userDto.Role.Id;
                if(user.Email != userDto.Email) {
                    sendMail = true;
                }
                if(!string.IsNullOrEmpty(userDto.Password)) {
                    var passwordSalt = this.encryptionService.CreateSalt();
                    user.HashedPassword = this.encryptionService.EncryptPassword(userDto.Password, passwordSalt);
                }
                user.Email = userDto.Email;
                user.Id_Erp = userDto.Id_Erp; 
                user.Username = userDto.Username;
                user.IsLocked = userDto.IsLocked;

                this.entityRepository.Edit(user);
                this.unitOfWork.Commit();
            }

            var userGroupCompanies = this.userCompanyGroupRepository.GetAll().Where(x => x.UserId == userDto.Id).ToList();

            if(userGroupCompanies.Count > 0) {
                foreach (var userGroupCompany in userGroupCompanies) {
                    var entity = this.userCompanyGroupRepository.GetSingle(userGroupCompany.Id);
                    this.userCompanyGroupRepository.Delete(entity);
                }

                this.unitOfWork.Commit();
            }
           

            foreach (var userGroupCompanyDto in userDto.UserCompanyGroups) {
                userGroupCompanyDto.UserId = userDto.Id;
                var ugcentity = Mapper.Map<UserCompanyGroupDto, UserCompanyGroup>(userGroupCompanyDto);
                    this.userCompanyGroupRepository.Add(ugcentity);
            }

            this.unitOfWork.Commit();


            if (sendMail)
            {
                Mailing.SendGenericMail("Solicitud de usuario", userDto.Email, userDto,
                                        "~/views/EmailTemplate/Index.cshtml");
            }
            return userDto;
        }

        public void ChangePassword(ChangePasswordDto changePasswordDto)
        {

            var user = this.entityRepository.GetSingle(changePasswordDto.UserId);
            var currentPassword = this.encryptionService.EncryptPassword(changePasswordDto.CurrentPassword, user.Salt);

            if (user.HashedPassword == currentPassword)
            {
                this.membershipService.UpdateUser(user.Username, user.Email, changePasswordDto.NewPassword);

                UserDto userDto = new UserDto();
                userDto.Username = user.Username;
                userDto.Password = changePasswordDto.CurrentPassword;

                Mailing.SendGenericMail("Solicitud de usuario", user.Email, userDto,
                                        "~/views/EmailTemplate/Password.cshtml");
            }
            else
            {
                throw new System.InvalidOperationException("La contraseña actual ingresada es incorrecta.");
            }
        }

        public void ResetPassword(UserDto userDto)
        {
            userDto.Password = Guid.NewGuid().ToString().Substring(0, 8);

            this.membershipService.UpdateUser(userDto.Username,
                                              userDto.Email,
                                              userDto.Password);

            Mailing.SendGenericMail("Solicitud de reseteo de contraseña", userDto.Email, userDto,
                                     "~/views/EmailTemplate/Password.cshtml");
        }
        public void UpdateProfile(UserDto userDto)
        {
            var user = this.entityRepository.GetSingle(userDto.Id);
            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.IsLocked = userDto.IsLocked;
            user.LastName = userDto.LastName;
            user.FirstName = userDto.FirstName;
            if(userDto.Role != null) {
                user.RoleId = userDto.Role.Id;
            }
            this.unitOfWork.Commit();
        }

        public void UpdateLastActivityDateTime(long id)
        {
            var user = this.entityRepository.GetSingle(id);
            user.LastActivityDateTime = this.timeService.LocalDateTimeNow;
            this.unitOfWork.Commit();
        }

        public IEnumerable<UserDto> GetMatchesUser(string username)
        {
            var users = this.entityRepository.GetAll().Where(x => x.Username.Contains(username));
            var usersDto = Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            return usersDto;
        }
    }
}