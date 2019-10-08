using System;

namespace ALZAGRO.Reports.Data {
    public class DataEquivalences {
        public class CIRCOM {
            public static String IVA { get { return "0430"; } }
            public static String SIN_IVA { get { return "0431"; } }
        }
        public static string GetCODDIM(Int64 NROITEM) {
            switch (NROITEM) {
                case 1: return "PRODUC";
                case 2: return "REND";
            }
            return "";
        }

        public static string GetCODDIS(Int64 NROITEM, Int64 CompanyID, String UserCode) {
           if(NROITEM != 0 && NROITEM != 1) {
                return $"AL0{CompanyID}|{UserCode}";
            }
            return "";
        }

        public static string GetCODPT(string IVA_VALUE) {
            switch(IVA_VALUE) {
                case "21": return "IVA001";
                case "10.5": return "IVA002";
                case "27": return "IVA003";
            }
            return "";
        }
        
    }
}
