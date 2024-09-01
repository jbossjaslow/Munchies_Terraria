using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader;
using System.Text.RegularExpressions;

namespace Munchies.Utilities {
	public class EnumUtil : ModSystem {
		private static Dictionary<Type, Dictionary<string, LocalizedText>> EnumTypeToLocalizationMapping { get; set; }

		public override void OnModUnload() {
			EnumTypeToLocalizationMapping = null;
		}

		//Needs to be loaded as early as possible, do so in Mod
		public static void LoadEnumText(Mod mod) {
			EnumTypeToLocalizationMapping = new Dictionary<Type, Dictionary<string, LocalizedText>>();

			foreach (var type in AssemblyManager.GetLoadableTypes(mod.Code)
				.Where(t => t.IsEnum && t.IsDefined(typeof(LocalizeEnumAttribute), false))) {
				var attr = (LocalizeEnumAttribute)Attribute.GetCustomAttribute(type, typeof(LocalizeEnumAttribute));
				var category = attr.Category ?? type.Name;
				var dict = EnumTypeToLocalizationMapping[type] = new Dictionary<string, LocalizedText>();
				foreach (var name in Enum.GetNames(type)) {
					dict[name] = RegisterEnumText(mod, category, name);
				}
			}
		}

		private static LocalizedText RegisterEnumText(Mod mod, string category, string suffix) {
			string commonKey = $"{category}.";
			return mod.GetLocalization($"{commonKey}{suffix}", () => Regex.Replace(suffix, "([A-Z])", " $1").Trim());
		}

		public static LocalizedText GetEnumText<T>(T enumValue) where T : Enum {
			return EnumTypeToLocalizationMapping[typeof(T)][enumValue.ToString()];
		}

		public static IEnumerable<T> AllCases<T>() {
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}

	/// <summary>
	/// Marker for localizing enums, automatically registered (<see cref="EnumUtil.RegisterEnumText"/>) and accessible (<see cref="EnumUtil.GetEnumText{T}(T)"/>)
	/// </summary>
	[AttributeUsage(AttributeTargets.Enum)]
	public class LocalizeEnumAttribute: Attribute {
		/// <summary>
		/// Can be null
		/// </summary>
		public string Category { get; init; } = null;
	}
}
