using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Culture;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;
using System.Linq;
using System.Security.Principal;

namespace ALZAGRO.AppRendicionGastos.CrossCutting {

    public class MembershipService : IMembershipService
    {

        #region Variables

        private readonly IEntityBaseRepository<User> userRepository;
        private readonly IEncryptionService encryptionService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITimeService timeService;

        #endregion

        public MembershipService(IEntityBaseRepository<User> userRepository,
                                 IEncryptionService encryptionService,
                                 ITimeService timeService,
                                 IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.encryptionService = encryptionService;
            this.unitOfWork = unitOfWork;
            this.timeService = timeService;
        }

        #region IMembershipService Implementation

        public MembershipContext ValidateUser(String username, String password)
        {
            var membershipCtx = new MembershipContext();

            var user = userRepository.GetAll().Where(x => x.Email == username || x.Username == username).FirstOrDefault();

            if (user != null && IsUserValid(user, password))
            {
                membershipCtx.User = user;
                //var identity = new GenericIdentity(user.Username);
                membershipCtx.Principal = new CustomPrincipal(user.Username, user.Id, user.FirstName, user.LastName);
            }

            return membershipCtx;
        }

        public IUser UpdateUser(String username, String email, String password)
        {

            var existingUser = userRepository.GetAll().Where(x => x.Username == username).FirstOrDefault();

            if (existingUser == null)
            {
                throw new Exception(String.Format("El usuario {0} no está registrado.", username));
            }

            var passwordSalt = encryptionService.CreateSalt();

            existingUser.Salt = passwordSalt;
            existingUser.Email = email;
            existingUser.HashedPassword = encryptionService.EncryptPassword(password, passwordSalt);

            unitOfWork.Commit();

            return existingUser;
        }

        public IUser UpdateUser(String username, String email)
        {

            var existingUser = userRepository.GetAll().Where(x => x.Username == username).FirstOrDefault();

            if (existingUser == null)
            {
                throw new Exception(String.Format("El usuario {0} no est&aacute; registrado.", username));
            }

            existingUser.Email = email;

            unitOfWork.Commit();

            return existingUser;
        }
        public IUser CreateUser(String username, String email, String password, Int64[] roles)
        {
            var existingUser = userRepository.GetAll().Where(x => x.Username == username).FirstOrDefault();

            if (existingUser != null)
            {
                throw new Exception(String.Format("El nombre de usuario {0} ya est&aacute; asignado.", username));
            }

            var passwordSalt = encryptionService.CreateSalt();

            var user = new User()
            {
                Username = username,
                Salt = passwordSalt,
                Email = email,
                HashedPassword = encryptionService.EncryptPassword(password, passwordSalt)
                //CreatedDate = timeService.DateTimeNow
            };

            userRepository.Add(user);

            unitOfWork.Commit();

            return user;
        }

        public IUser GetUser(Int64 userId)
        {
            return userRepository.GetSingle(userId);
        }

        public Boolean UserExist(Int64 userId, String username)
        {
            var existingUser = userRepository.GetAll().Where(x => x.Username == username && x.Id != userId).FirstOrDefault();
            return (existingUser != null);
        }

        public Boolean UserEmailExist(Int64 userId, String email)
        {
            var existingUser = userRepository.GetAll().Where(x => x.Email == email && x.Id != userId).FirstOrDefault();
            return (existingUser != null);
        }

        public String CreateRandomPassword()
        {
            String randomPassword = Guid.NewGuid().ToString().Substring(0, 8);
            return randomPassword;
        }

        #endregion

        #region Helper methods

        private bool IsPasswordValid(User user, String password)
        {
            return String.Equals(encryptionService.EncryptPassword(password, user.Salt), user.HashedPassword);
        }

        private bool IsUserValid(User user, String password)
        {
            if (IsPasswordValid(user, password))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}