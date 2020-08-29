using System.Dynamic;

namespace DetailingArsenal.Application {
    /// <summary>
    /// Action result command handlers can return 
    /// </summary>
    public class ActionResult {
        public dynamic Data { get; }
        public bool Success { get; }

        public ActionResult(bool success) {
            Data = new ExpandoObject();
            Success = success;
        }
    }
}