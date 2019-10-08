using System;
using System.Reflection;

namespace ALZAGRO.AppRendicionGastos.Fwk.Reflection {
    public class AssemblyHelper {

        public static Assembly GetAssemblyByName(String assembleName) {
            foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies()) {
                if (currentassembly.FullName.Contains(assembleName)) {
                    return currentassembly;
                }
            }         

            return null;
        }

        public static String GetAssemblyNameContainingType(String typeName) {
            foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies()) {
                Type t = currentassembly.GetType(typeName, false, true);
                if (t != null) { return currentassembly.FullName; }
            }

            return "not found";
        }
    }
}
