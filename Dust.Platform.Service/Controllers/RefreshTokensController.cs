using System.Threading.Tasks;
using System.Web.Http;
using Dust.Platform.Service.DataBase;

namespace Dust.Platform.Service.Controllers
{
    public class RefreshTokensController : ApiController
    {
        private readonly AuthRepository _repo;

        public RefreshTokensController()
        {
            _repo = new AuthRepository();
        }

        [Authorize(Users = "Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_repo.GetAllRefreshTokens());
        }

        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await _repo.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Token Id des not exist.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}