using System;
using System.Collections.Generic;
using System.Linq;

namespace ALZAGRO.AppRendicionGastos.WebUI.Infrastructure.Core {

    public class PaginationSet<T> {

        public Int32 Page { get; set; }

        public Int32 Count {
            get {
                return (null != this.Items) ? this.Items.Count() : 0;
            }
        }

        public Int32 TotalPages { get; set; }

        public Int32 TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}