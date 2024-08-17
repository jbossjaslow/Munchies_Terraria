using System;
using System.Collections.Generic;
using System.Linq;

namespace Munchies.Utils {
	public static class EnumUtil {
		public static IEnumerable<T> AllCases<T>() {
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
}
