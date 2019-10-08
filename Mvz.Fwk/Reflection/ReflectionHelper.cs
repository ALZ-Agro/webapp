using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALZAGRO.AppRendicionGastos.Fwk.Reflection {
	public class ReflectionHelper {
		public static IEnumerable<IEnumerable> GetCollections<T>(object obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			var type = obj.GetType();
			var res = new List<IEnumerable>();
			foreach (var prop in type.GetProperties()) {

				if (prop.PropertyType.Name.Contains("ICollection")) {
					var collection = (IEnumerable)prop.GetValue(obj, null);
					res.Add(collection);
				}
			}
			return res;
		}
	}
}
