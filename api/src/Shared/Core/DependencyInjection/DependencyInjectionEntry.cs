using System;

namespace DetailingArsenal {
    public class DependencyInjectionEntry {
        public Type Type { get; set; }
        public DependencyInjectionAttribute Attribute { get; set; }

        public DependencyInjectionEntry(Type type, DependencyInjectionAttribute attribute) {
            Type = type;
            Attribute = attribute;
        }
    }
}