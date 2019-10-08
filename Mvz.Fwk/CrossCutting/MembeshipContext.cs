using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System.Security.Principal;

namespace ALZAGRO.AppRendicionGastos.Fwk.CrossCutting {

    public class MembershipContext {

        public IPrincipal Principal { get; set; }

        public IUser User { get; set; }
        
        public bool IsValid() {
            return Principal != null;
        }
    }
}