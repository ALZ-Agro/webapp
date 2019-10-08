using System;

namespace ALZAGRO.AppRendicionGastos.Fwk.Exceptions {
    public class ExceptionInfo <T> where T:class {

        public T Info { get; set; }

        public Boolean CustomException { get; set; }
    }
}
