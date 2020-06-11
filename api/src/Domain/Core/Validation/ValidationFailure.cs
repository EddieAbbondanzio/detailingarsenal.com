namespace DetailingArsenal.Domain {
    /// <summary>
    /// Property and error for a resource that failed validation.
    /// </summary>
    public sealed class ValidationFailure {
        #region Properties
        /// <summary>
        /// Name of the property on the resource that was validated.
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// Human readable text of why the error is occuring.
        /// </summary>
        public string Message { get; }
        #endregion

        #region Constructor(s)
        public ValidationFailure(string propertyName, string errorMessage) {
            Field = propertyName;
            Message = errorMessage;
        }
        #endregion
    }
}