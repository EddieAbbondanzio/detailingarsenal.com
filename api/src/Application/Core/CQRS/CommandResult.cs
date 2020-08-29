using System.Collections.Generic;
using System.Dynamic;

namespace DetailingArsenal.Application {
    /// <summary>
    /// Action result command handlers can return 
    /// </summary>
    public class CommandResult {
        public dynamic Data { get; }
        public bool WasSuccessful { get; }

        public CommandResult(bool wasSuccessful, dynamic? data = null) {
            WasSuccessful = wasSuccessful;
            Data = data ?? new ExpandoObject();

        }

        public static CommandResult Success(dynamic? d = null) {
            return new CommandResult(true, d);
        }

        public static CommandResult Failure(dynamic? d = null) {
            return new CommandResult(false, d);
        }
    }
}