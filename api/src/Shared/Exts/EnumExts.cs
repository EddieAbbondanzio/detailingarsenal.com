using System;
using System.Collections.Generic;

namespace DetailingArsenal {
    public static class EnumExts {
        /// <summary>
        /// Get a custom attribute via it's type on an enum value.
        /// </summary>
        /// <param name="enumVal">The value to retrieve the attribute of.</param>
        /// <typeparam name="TAttribute">The attribute type to retrieve.</typeparam>
        /// <returns>The attribute if it exists.</returns>
        public static TAttribute? GetCustomAttribute<TAttribute>(this Enum enumVal) where TAttribute : Attribute {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());

            var attributes = memInfo[0].GetCustomAttributes(typeof(TAttribute), false);
            return (attributes.Length > 0) ? (TAttribute)attributes[0] : null;
        }
    }
}