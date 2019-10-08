﻿using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using Mvz.Fwk.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ALZAGRO.AppRendicionGastos.Fwk.ExtensionMethods {
    public static class EntityExtensionMethods {

        public static void CopyToOnlyPrimitiveValues(this EntityBase source, EntityBase target) {
            if (source != null && target != null) {
                foreach (PropertyInfo pi in source.GetType().GetProperties()) {
                    if (pi.PropertyType.IsPrimitive ||
                        pi.PropertyType == typeof(String) ||
                        pi.PropertyType == typeof(bool) ||
                        pi.PropertyType == typeof(bool?) ||
                        pi.PropertyType == typeof(Int16) ||
                        pi.PropertyType == typeof(Int16?) ||
                        pi.PropertyType == typeof(Int32) ||
                        pi.PropertyType == typeof(Int32?) ||
                        pi.PropertyType == typeof(Int64) ||
                        pi.PropertyType == typeof(Int64?) ||
                        pi.PropertyType == typeof(Decimal) ||
                        pi.PropertyType == typeof(Decimal?) ||
                        pi.PropertyType == typeof(DateTime) ||
                        pi.PropertyType == typeof(DateTime?)) {
                        object valueToCopy = pi.GetValue(source, null);

                        MethodInfo setMethod = pi.GetSetMethod();

                        if (setMethod != null) {
                            pi.SetValue(target, valueToCopy, null);
                        }
                    }
                }
            }
        }
    }
}
