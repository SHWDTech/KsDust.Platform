using System.Net.Http;
using System.Web.Http;

namespace Dust.Platform.Service.Process
{
    public static class ApiControllerExtensionMethod
    {
        public static AuthFilterProcess CreateFilterProcess(this ApiController controller)
        {
            var ctx = controller.Request.GetOwinContext();
            return new AuthFilterProcess(ctx);
        }
    }
}