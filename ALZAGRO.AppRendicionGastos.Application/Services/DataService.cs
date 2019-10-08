using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Mappings;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace ALZAGRO.AppRendicionGastos.Application.Services {
    public class DataService : IDataService {
        public DataService() { }

        public string SaveCsv<T, E>(SearchResultViewModel<T> data, E criteria, string FileName, bool ShowFilterParams) {
            var fileUrl = "/Resources/Export/" + FileName + ".csv";
            var path = HttpContext.Current.Server.MapPath("~" + fileUrl);
            if (File.Exists(path)) {
                throw new Exception("Ya existe un archivo con este nombre, por favor especifique otro.");
            }
            using (TextWriter sw = new StreamWriter(path)) {
                var csv = new CsvWriter(sw);
                //Register mappers
                //csv.Configuration.RegisterClassMap<ExpenseListViewCriteriaMap>();
                //csv.Configuration.RegisterClassMap<ExpenseMap>();
                //csv.Configuration.RegisterClassMap<ProviderListViewCriteriaMap>();
                csv.Configuration.RegisterClassMap<ProviderMap>();

                //Filtros
                if (ShowFilterParams) {
                    csv.WriteComment("Parámetros");
                    csv.NextRecord();

                    csv.WriteHeader<E>();
                    csv.NextRecord();
                    csv.WriteRecord<E>(criteria);

                    csv.NextRecord();
                    csv.NextRecord();
                }
                //Datos
                csv.WriteHeader<T>();
                csv.NextRecord();
                csv.WriteRecords<T>(data.Results);
                csv.Flush();
            }

            return ConfigurationManager.AppSettings["Host"].ToString() + fileUrl;
        }

        public string SaveCsv<T>(IEnumerable<T> data, string FileName) {
            var fileUrl = "/Resources/Export/" + FileName + ".csv";
            var path = HttpContext.Current.Server.MapPath("~" + fileUrl);
            if (File.Exists(path)) {
                throw new Exception("Ya existe un archivo con este nombre, por favor especifique otro.");
            }
            using (TextWriter sw = new StreamWriter(path)) {
                var csv = new CsvWriter(sw);
                //Register mappers
                csv.Configuration.RegisterClassMap<ImportProviderMap>();
                //Datos
                csv.WriteHeader<T>();
                csv.NextRecord();
                csv.WriteRecords<T>(data);
                csv.Flush();
            }

            return ConfigurationManager.AppSettings["Host"].ToString() + fileUrl;
        }

        public List<T> ReadCsv<T>(string FileName) {
            var fileUrl = "/Resources/Import/" + FileName + ".csv";
            var path = HttpContext.Current.Server.MapPath("~" + fileUrl);
            if (!File.Exists(path)) {
                throw new Exception("No se encontró el archivo a importar.");
            }
            if (path == null) return new List<T>();
            if (path == "") return new List<T>();



            List<T> results = new List<T>();


            using (TextReader reader = File.OpenText(path)) {
                try {

                    var csv = new CsvReader(reader);

                    csv.Configuration.RegisterClassMap<ImportProviderMap>();

                    results = csv.GetRecords<T>().ToList();

                }

                catch (Exception ex) {
                    Console.WriteLine(ex.Data["CsvHelper"]);
                    Console.WriteLine(ex.StackTrace);
                }
            }

            return results;
        }
    }
}
