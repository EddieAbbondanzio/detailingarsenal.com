[System.Serializable]
public class ValidationException : System.Exception {
    public ValidationResult Result { get; }
    public ValidationException(ValidationResult result) {
        Result = result;
    }
}