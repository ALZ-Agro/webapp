using ALZAGRO.AppRendicionGastos.Fwk.UI;
using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Application.Contracts {
    public interface IDataService {
        List<T> ReadCsv<T>(string FileName);
        string SaveCsv<T, E>(SearchResultViewModel<T> data, E criteria, string FileName, bool ShowFilterParams);
        string SaveCsv<T>(IEnumerable<T> data, string FileName);
    }
}
