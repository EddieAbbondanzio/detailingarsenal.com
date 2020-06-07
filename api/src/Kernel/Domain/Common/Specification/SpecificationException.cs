[System.Serializable]
public class SpecificationException : System.Exception {
    public SpecificationResult Result { get; }
    public SpecificationException(SpecificationResult result) {
        Result = result;
    }
}