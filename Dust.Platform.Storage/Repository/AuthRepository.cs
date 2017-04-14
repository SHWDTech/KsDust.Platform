using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                UserName = userModel.UserName,
                PhoneNumber = userModel.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityUser> FindById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }

        public DustRole FindDustRoleById(string id)
        {
            var roleId = Guid.Parse(id);
            return _ctx.DustRoles.First(r => r.Id == roleId);
        }

        public List<DustPermission> FindRolePermissions(DustRole role)
        {
            var rolePermissoins = _ctx.RolePermissions.Where(rp => rp.RoleId == role.Id).Select(obj => obj.PermissionId).ToList();
            return _ctx.DustPermissions.Where(p => rolePermissoins.Contains(p.Id)).ToList();
        }

        public async Task<IList<string>> GetUserRoles(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user.Id);

            return roles;
        }

        public List<DustRole> GetDustRoles(Expression<Func<DustRole, bool>> exp)
        {
            var query = _ctx.DustRoles.AsQueryable();
            if (exp != null)
            {
                query = query.Where(exp);
            }

            return query.ToList();
        }

        public IdentityResult DeleteUser(string userId)
        {
            var user = _userManager.FindById(userId);
            return _userManager.Delete(user);
        }

        public DustRole GetDustRole(IdentityUser user)
        {
            if (user.Roles == null || user.Roles.Count <= 0)
            {
                return new DustRole();
            }
            
            var roleId = Guid.Parse(user.Roles.First().RoleId);
            var dustRole = _ctx.DustRoles.First(r => r.Id == roleId);
            return dustRole;
        }

        public async Task<IList<Claim>> GetUserClaims(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user.Id);

            return claims;
        }

        public void UpdateRolePermissions(string roleId, List<Guid> permissions)
        {
            var roleGuid = Guid.Parse(roleId);
            _ctx.RolePermissions.RemoveRange(_ctx.RolePermissions.Where(rp => rp.RoleId == roleGuid));
            if (permissions != null)
            {
                foreach (var dustPermission in permissions)
                {
                    _ctx.RolePermissions.Add(new RolePermission
                    {
                        RoleId = roleGuid,
                        PermissionId = dustPermission
                    });
                }
            }

            _ctx.SaveChanges();
        }

        public IdentityResult ChangePassword(string userId, string currentPassword, string newPassword)
        {
            return _userManager.ChangePassword(userId, currentPassword, newPassword);
        }

        public List<DustPermission> GetDustPermissions(Expression<Func<DustPermission, bool>> exp)
        {
            return exp == null ? _ctx.DustPermissions.ToList() : _ctx.DustPermissions.Where(exp).ToList();
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

        public int GetUserCount(Expression<Func<IdentityUser, bool>> exp)
        {
            return exp == null ? _userManager.Users.Count() : _userManager.Users.Count(exp);
        }

        public int GetRoleCount(Expression<Func<IdentityRole, bool>> exp)
        {
            return exp == null ? _roleManager.Roles.Count() : _roleManager.Roles.Count(exp);
        }

        public List<IdentityUser> GetUserTable(int offset, int limit)
        {
            return _userManager.Users.OrderBy(u => u.Id).Skip(offset).Take(limit).ToList();
        }

        public List<DustRole> GetDustRoleTable(int offset, int limit)
        {
            return _ctx.DustRoles.OrderBy(r => r.Id).Skip(offset).Take(limit).ToList();
        }

        public async Task<IdentityResult> UpdateAsync(IdentityUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public IdentityResult Update(IdentityUser user)
        {
            return _userManager.Update(user);
        }

        public void UserAddRole(IdentityUser user, string roleId)
        {
            var role = _roleManager.FindById(roleId);
            _userManager.AddToRole(user.Id, role.Name);
        }

        public void UserRemoveFromRoles(IdentityUser user, string[] roleIds)
        {
            var roles = roleIds.Select(r => _roleManager.FindById(r).Name).ToArray();
            _userManager.RemoveFromRoles(user.Id, roles);
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}