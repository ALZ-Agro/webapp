using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Fwk.UI {
    public class SearchCompositeResultViewModel<T,R> : SearchResultViewModel<T> {

        public R Resume { get; set; }
    }
}
