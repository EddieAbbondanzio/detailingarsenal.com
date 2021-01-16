using System;

namespace DetailingArsenal {
    public class JsonValueAttribute : Attribute {
        public string Value { get; }

        public JsonValueAttribute(string value) {
            Value = value;
        }
    }
}