using System.Collections.Generic;

public class SpecificationResult {
    public bool IsSatisfied { get; }
    public List<string> Messages { get; }

    public SpecificationResult(bool isSatisfiedBy, string? message = null) {
        IsSatisfied = isSatisfiedBy;
        Messages = new List<string>();

        if (message != null) {
            Messages.Add(message);
        }
    }

    public SpecificationResult(bool isSatisfiedBy, List<string> messages) {
        IsSatisfied = isSatisfiedBy;
        Messages = messages;
    }
}