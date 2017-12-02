using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace Dust.Platform.Web.Models
{
    public class OuterPlatformExecuteResult
    {
        public OuterPlatformExecuteResult()
        {
            
        }

        public OuterPlatformExecuteResult(ModelStateDictionary modelState) : this()
        {
            Message = "无效的请求。";
            foreach (var stateError in modelState)
            {
                InvalidProperties.Add(new InvalidProperty(stateError.Key.Replace("model.", string.Empty), stateError.Value.Errors[0].ErrorMessage));
            }
        }

        public OuterPlatformExecuteResult(string message) : this()
        {
            Message = message;
        }

        public string Result { get; set; } = "failed";

        public string Message { get; set; }

        public List<InvalidProperty> InvalidProperties { get; set; } = new List<InvalidProperty>();

        public OuterPlatformExecuteResult Success()
        {
            Result = "success";
            return this;
        }
    }

    public class InvalidProperty
    {
        public string PropertyName { get; set; }

        public string Error { get; set; }

        public InvalidProperty(string propertyName, string error)
        {
            PropertyName = propertyName;
            Error = error;
        }
    }
}