using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Application.Contracts
{
    public interface IUserAppService : IEntityBaseAppService<User, UserDto>
    {

        SearchResultViewModel<UserDto> Search(UserListViewCriteria criteria);

        UserDto GetByUserName(string userName);

        UserDto GetByEmail(string email);

        IEnumerable<UserDto> GetMatchesUser(string username);

        void ChangePassword(ChangePasswordDto changePasswordDto);
        void UpdateProfile(UserDto userDto);
        void UpdateLastActivityDateTime(long id);

        void ResetPassword(UserDto userDto);

        UserDto GetUser(long UserId);

        List<SearchUserDto> GetForFilter();
    }
}