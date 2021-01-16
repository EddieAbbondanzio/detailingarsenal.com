using System;
using System.Collections.Generic;
using System.Linq;

namespace DetailingArsenal {
    public static class EnumUtils {
        public static IEnumerable<T> GetValues<T>() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static IEnumerable<(TEnum Value, TAttribute? Attribute)> GetAllAttributesByValues<TEnum, TAttribute>() where TEnum : struct, Enum where TAttribute : Attribute {
            List<(TEnum value, TAttribute? attribute)> all = new();
            var values = EnumUtils.GetValues<TEnum>();

            foreach (var value in values) {
                all.Add((value, value.GetCustomAttribute<TAttribute>()));
            }

            return all;
        }
    }
}