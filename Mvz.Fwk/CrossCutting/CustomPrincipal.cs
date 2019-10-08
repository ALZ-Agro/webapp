using System;
using System.Security.Principal;

namespace ALZAGRO.AppRendicionGastos.Fwk.CrossCutting {

    public class CustomPrincipal : ICustomPrincipal {

        public IIdentity Identity { get; private set; }

        public bool IsInRole(String role) { return false; }

        public CustomPrincipal(String userName) {
            this.Identity = new GenericIdentity(userName);
        }

        public CustomPrincipal(String userName, Int64 userId, String firstName, String lastName) {
            this.Identity = new GenericIdentity(userName);
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;

        }

        public Int64 UserId { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}