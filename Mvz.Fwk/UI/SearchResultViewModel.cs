using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Fwk.UI {
    public class SearchResultViewModel<T> {

        public IEnumerable<T> Results { get; set; }

        public Int32 TotalItems { get; set; }
        
    }
}
