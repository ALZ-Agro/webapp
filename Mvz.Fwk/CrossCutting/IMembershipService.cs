using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Fwk.CrossCutting {

    public interface IMembershipService {

        MembershipContext ValidateUser(String username, String password);

        IUser CreateUser(String username, String email, String password, Int64[] roles);

        IUser UpdateUser(String username, String email, String password);

        IUser UpdateUser(String username, String email);

        IUser GetUser(Int64 userId);

        Boolean UserExist(Int64 userId, String username);

        Boolean UserEmailExist(Int64 userId, String email);

        String CreateRandomPassword();
    }
}