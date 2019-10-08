using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mvz.Fwk.UI {
    public class SyncApiController : ApiController {
        protected DateTime GetDatetime(string lastSynchronizationDate) {
            var datetime = DateTime.MinValue;

            if (!String.IsNullOrEmpty(lastSynchronizationDate)) {
                //lastSynchronizationDate = lastSynchronizationDate.Replace('!', ':');
                DateTime date = Convert.ToDateTime(lastSynchronizationDate).ToUniversalTime();
                datetime = date;
            }

            return datetime;
        }
    }
}
