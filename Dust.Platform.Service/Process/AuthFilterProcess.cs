using System;
using System.Linq;
using System.Linq.Expressions;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;

namespace Dust.Platform.Service.Process
{
    public class AuthFilterProcess
    {
        private readonly IOwinContext _owinContext;

        private readonly KsDustDbContext _dbContext;

        public AuthFilterProcess(IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _dbContext = new KsDustDbContext();
        }

        public IQueryable<KsDustDevice> GetAuthedDevices(Expression<Func<KsDustDevice, bool>> exp)
        {
            var user = _owinContext.Authentication.User;
            IQueryable<KsDustDevice> authedQuery = _dbContext.KsDustDevices;

            var userId = Guid.Parse(user.Identity.GetUserId());
            var userEntities =
                    _dbContext.UserRelatedEntities.Where(ent => ent.User == userId).Select(id => id.Entity).ToList();
            if (user.IsInRole("DistrictManager"))
            {
                authedQuery = authedQuery.Where(dev => userEntities.Contains(dev.Project.DistrictId));
            }
            else if (user.IsInRole("VendorManager"))
            {
                authedQuery = authedQuery.Where(dev => userEntities.Contains(dev.VendorId));
            }
            else if (user.IsInRole("ProjectManager"))
            {
                authedQuery = authedQuery.Where(dev => userEntities.Contains(dev.ProjectId.Value));
            }

            if (exp != null)
            {
                authedQuery = authedQuery.Where(exp);
            }
            return authedQuery;
        }

        public IQueryable<KsDustProject> GetAuthedProjects(Expression<Func<KsDustProject, bool>> exp)
        {
            var user = _owinContext.Authentication.User;
            IQueryable<KsDustProject> authedQuery = _dbContext.KsDustProjects.Where(obj => obj.Id != Guid.Empty);

            var userId = Guid.Parse(user.Identity.GetUserId());
            var userEntities =
                    _dbContext.UserRelatedEntities.Where(ent => ent.User == userId).Select(id => id.Entity).ToList();

            if (user.IsInRole("DistrictManager"))
            {

                authedQuery = authedQuery.Where(prj => userEntities.Contains(prj.DistrictId));
            }
            else if (user.IsInRole("VendorManager"))
            {
                var devPrjs = _dbContext.KsDustDevices.Where(dev => userEntities.Contains(dev.VendorId))
                    .Select(d => d.ProjectId).Distinct();
                authedQuery = authedQuery.Where(prj => devPrjs.Contains(prj.Id));
            }
            else if (user.IsInRole("ProjectManager"))
            {
                authedQuery = authedQuery.Where(prj => userEntities.Contains(prj.Id));
            }

            if (exp != null)
            {
                authedQuery = authedQuery.Where(exp);
            }
            return authedQuery;
        }

        public IQueryable<District> GetAuthedDistricts(Expression<Func<District, bool>> exp)
        {
            var user = _owinContext.Authentication.User;
            IQueryable<District> authedQuery = _dbContext.Districts.Where(obj => obj.Id != Guid.Empty);
            var userId = Guid.Parse(user.Identity.GetUserId());
            var userEntities =
                _dbContext.UserRelatedEntities.Where(ent => ent.User == userId).Select(id => id.Entity).ToList();
            if (user.IsInRole("DistrictManager"))
            {

                authedQuery = authedQuery.Where(dis => userEntities.Contains(dis.Id));
            }
            else if (user.IsInRole("VendorManager"))
            {
                var authedPrjs = (from dev in _dbContext.KsDustDevices
                    let prjs = _dbContext.KsDustProjects.AsQueryable()
                    where userEntities.Contains(dev.VendorId)
                    select dev.Project.DistrictId).Distinct();

                authedQuery = authedQuery.Where(dis => authedPrjs.Contains(dis.Id));
            }
            else if (user.IsInRole("ProjectManager"))
            {
                var prjDis = _dbContext.KsDustProjects.Where(prj => userEntities.Contains(prj.Id))
                    .Select(p => p.DistrictId).Distinct();
                authedQuery = authedQuery.Where(dis => prjDis.Contains(dis.Id));
            }

            if (exp != null)
            {
                authedQuery = authedQuery.Where(exp);
            }
            return authedQuery;
        }
    }
}