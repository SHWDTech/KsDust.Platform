using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dust.Platform.Service.Entities;
using Dust.Platform.Storage.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dust.Platform.Storage.Repository
{

    public class AuthRepository : IDisposable
    {
        private readonly AuthContext _ctx;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IList<string>> GetUserRoles(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user.Id);

            return roles;
        }

        public async Task<IList<Claim>> GetUserClaims(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user.Id);

            return claims;
        }


        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _ctx.RefreshTokens.SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                await RemoveRefreshToken(existingToken);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _ctx.RefreshTokens.ToList();
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            var user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public bool UserInRole(string userId, string roleName)
        {
            return _userManager.IsInRoleAsync(userId, roleName).Result;
        }

        public IdentityUser FindByName(string name)
        {
            return _userManager.FindByName(name);
        }

        public Guid FindVendorId(IdentityUser usr)
        {
            var claims = _userManager.GetClaimsAsync(usr.Id).Result;
            return Guid.Parse(claims.First(c => c.Type == "VendorId").Value);
        }

        public List<Module> FindModuleByParentName(string userId, string pModule, bool includeNonMenu = true)
        {
            var userPermission = GetUserPermissions(userId);
            var parModule = _ctx.Modules.FirstOrDefault(m => m.ModuleName == pModule);
            if(parModule == null) return new List<Module>();

            var query = _ctx.Modules.Where(m => m.ParentModuleId == parModule.Id &&
                                                userPermission.Contains(m.PermissionId));
            if (!includeNonMenu)
            {
                query = query.Where(q => q.IsMenu);
            }
            return query.OrderBy(o => o.ModuleIndex).ToList();
        }

        public List<Guid> GetUserPermissions(string userId)
        {
            var user = _userManager.FindById(userId);
            return GetRolePermissions(user);
        }

        public List<Guid> GetRolePermissions(IdentityUser user)
        {
            var roles = GetUserRoles(user).Result;
            var pers = new List<Guid>();
            foreach (var role in roles)
            {
                var rolePers = GetRolePermissions(role);
                pers.AddRange(rolePers);
            }

            pers = pers.Distinct().ToList();
            return pers;
        }

        public List<Guid> GetRolePermissions(string role)
        {
            var roleId = Guid.Parse(_roleManager.FindByName(role).Id);
            return _ctx.RolePermissions.Where(rp => rp.RoleId == roleId).Select(o => o.PermissionId).ToList();
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}