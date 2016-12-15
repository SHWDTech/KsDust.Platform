using System.Threading.Tasks;
using System.Web.Http;
using Dust.Platform.Service.DataBase;
using Dust.Platform.Service.Models;
using Microsoft.AspNet.Identity;

namespace Dust.Platform.Service.Controllers
{
    public class AccountController : ApiController
    {
        private readonly AuthRepository _repo;

        public AccountController()
        {
            _repo = new AuthRepository();
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(User userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.RegisterUser(userModel);

            var errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}