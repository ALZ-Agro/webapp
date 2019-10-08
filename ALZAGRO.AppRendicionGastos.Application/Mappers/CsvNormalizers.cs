using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Infrastructure;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Repositories;
using ALZAGRO.AppRendicionGastos.Data;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using CsvHelper.TypeConversion;
using CsvHelper;
using CsvHelper.Configuration;
using Autofac;
using Autofac.Integration.WebApi;

namespace ALZAGRO.AppRendicionGastos.Application.Mappers {
    public class DateNormalizer : ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(DateTime)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null) {
                return "Sin especificar";
            }
            else {
                var formatted = ((DateTime)value).ToString("dd-MM-yyyy");
                return formatted;
            }
        }
    }

    public class DateAndHourNormalizer : ITypeConverter {
        public bool CanConvertFrom(Type type)
        {
            return type == typeof(DateTime);
        }
        public bool CanConvertTo(Type type)
        {
            return type == typeof(string);
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null) {
                return "Sin especificar";
            }

            var formatted = ((DateTime)value).ToString("dd-MM-yyyy HH:mm");
            return formatted;
        }
    }



    public class TimeNormalizer : ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(System.TimeSpan)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null) {
                return "Sin especificar";
            }
            else {
                return ((System.TimeSpan)value).ToString(@"hh\:mm");
            }
        }

    }

    public class StringNormalizer : ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
        if(value == null) {
                return "<vacío>";
            }
            else if (string.IsNullOrEmpty(value.ToString())) {
                return "<vacío>";
            }
            else {
                return value.ToString();
            }
        }

    }

    public class DoubleNormalizer : ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            var stringifiedValue = value.ToString();
            if (value == null || string.IsNullOrEmpty(stringifiedValue)) {
                return "<vacío>";
            }
            else {
                var parseable = double.TryParse(stringifiedValue, out double doubleParsed);
                if (parseable) {
                    return Math.Round(doubleParsed, 2).ToString();
                } else {
                    return "<error>";
                }
               
            }
        }

    }


    public class LegalConditionNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || (long)value == 0) {
                return "Todas";
            }
            else {
                var container = Build();
                var repository = container.Resolve<Fwk.Domain.IEntityBaseRepository<LegalCondition>>();
                var status = repository.GetSingle((long)value);
                return status.Description;
            }
        }

    }

    public class SyncStatusNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || string.IsNullOrEmpty(value.ToString()) || (long)value == 0) {
                return "Todos";
            }
            else {
                var container = Build();
                var repository = container.Resolve<Fwk.Domain.IEntityBaseRepository<SyncStatus>>();
                var status = repository.GetSingle((long)value);
                return status.Description;
            }
        }

    }

    public class UserNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(long)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || (long)value == 0) {
                return "Todos";
            }
            else {
                var container = Build();
                var repository = container.Resolve<Fwk.Domain.IEntityBaseRepository<User>>();
                var user = repository.GetSingle((long)value);
                return user.FirstName + " " + user.LastName;
            }
        }

    }

    public class CompanyNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(long)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || (long)value == 0) {
                return "Todas";
            }
            else {
                return "AL0" + (long)value;
            }
        }

    }

    public class CategoryNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(long)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || (long)value == 0) {
                return "Todas";
            }
            else {
                var container = Build();
                var repository = container.Resolve<Fwk.Domain.IEntityBaseRepository<Category>>();
                var category = repository.GetAll().Where(x => x.Id == (long)value).FirstOrDefault();
                return category.Description.ToUpper();
            }
        }

    }

    public class ProviderNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(long)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || (long)value == 0) {
                return "Todos";
            }
            else {
                var container = Build();
                var repository = container.Resolve<Fwk.Domain.IEntityBaseRepository<Provider>>();
                var provider = repository.GetAll().Where(x => x.Id == (long)value).FirstOrDefault();
                return provider.Cuit.ToString();
            }
        }

    }

    public class AliquotNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(long)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || (long)value == 0) {
                return "-";
            }
            else {
                var container = Build();
                var repository = container.Resolve<Fwk.Domain.IEntityBaseRepository<Aliquot>>();
                var aliquot = repository.GetAll().Where(x => x.Id == (long)value).FirstOrDefault();
                return aliquot.Description;
            }
        }

    }

    public class PaymentTypeNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(long)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || (long)value == 0) {
                return "-";
            }
            else {
                var container = Build();
                var repository = container.Resolve<Fwk.Domain.IEntityBaseRepository<Aliquot>>();
                var description = repository.GetSingle((long)value).Description;
                if (description != "SIN IVA") {
                    return "Con IVA";
                } else {
                    return description;
                }
            }
        }

    }

    public class PaymentNormalizer : RepositoryNormalizer, ITypeConverter {
        public bool CanConvertFrom(Type type) {
            if (type == typeof(long)) return true;
            return false;
        }
        public bool CanConvertTo(Type type) {
            if (type == typeof(string)) return true;
            return false;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
            if (value == null || (long)value == 0) {
                return "Todas";
            }
            else {
                var container = Build();
                var repository = container.Resolve<Fwk.Domain.IEntityBaseRepository<Payment>>();
                return repository.GetSingle((long)value).Description;
            }
        }

    }

    public class RepositoryNormalizer {
        public Autofac.IContainer Build() {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<GeneralContext>()
                .As<IDbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Fwk.Data.Repositories.IEntityBaseRepository<>))
                   .As(typeof(Fwk.Domain.IEntityBaseRepository<>))
                   .InstancePerLifetimeScope();

            return builder.Build();
        }
    }

}
