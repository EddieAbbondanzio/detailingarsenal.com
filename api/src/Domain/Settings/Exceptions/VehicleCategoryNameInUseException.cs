namespace DetailingArsenal.Domain.Settings {
    [System.Serializable]
    public class VehicleCategoryNameInUseException : System.Exception {
        public VehicleCategoryNameInUseException() { }
        public VehicleCategoryNameInUseException(string message) : base(message) { }
        public VehicleCategoryNameInUseException(string message, System.Exception inner) : base(message, inner) { }
        protected VehicleCategoryNameInUseException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}