using System;
using System.Collections.Generic;
using System.Dynamic;

namespace DetailingArsenal.Application {
    /// <summary>
    /// Action result command handlers can return 
    /// </summary>
    public class CommandResult {
        public dynamic Data { get; }
        public bool WasSuccessful { get; }

        public CommandResult(bool wasSuccessful, ExpandoObject? data = null) {
            WasSuccessful = wasSuccessful;
            Data = data ?? new ExpandoObject();
        }

        public static CommandResult Insert(Guid id) {
            var data = new ExpandoObject();

            var dynamo = (dynamic)data;
            dynamo.Id = id;

            return Success(data);
        }

        public static CommandResult Success(ExpandoObject? d = null) {
            return new CommandResult(true, d);
        }

        public static CommandResult Failure(ExpandoObject? d = null) {
            return new CommandResult(false, d);
        }
    }
}