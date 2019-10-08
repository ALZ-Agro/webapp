using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ALZAGRO.AppRendicionGastos.Fwk.Domain {
    public class CustomMapper {

        public static void CopyTo(Object sourceEnttiy, Object targetEnttiy, Boolean deepCopy) {

            Type targetType = targetEnttiy.GetType();

            foreach (PropertyInfo pi in sourceEnttiy.GetType().GetProperties()) {
                if (deepCopy ||
                    (!deepCopy && !IsIEnumerable(pi))) {
                    object valueToCopy = pi.GetValue(sourceEnttiy, null);

                    var targetPI = targetType.GetProperty(pi.Name);
                    if (targetPI != null) {
                        MethodInfo setMethod = targetPI.GetSetMethod();
                        if (setMethod != null) {
                            targetPI.SetValue(targetEnttiy, valueToCopy, null);
                        }
                    }
                }
            }
        }

        private static Boolean IsIEnumerable(PropertyInfo pi) {
            return (pi.PropertyType != typeof(string) &&
                   pi.PropertyType.GetInterface(typeof(IEnumerable).Name) != null &&
                   pi.PropertyType.GetInterface(typeof(IEnumerable<>).Name) != null);
        }
    }
}