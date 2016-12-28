using System;
using System.Collections.Generic;
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

        public List<KsDustDevice> GetAuthedDevices(Expression<Func<KsDustDevice, bool>> exp)
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
                authedQuery = authedQuery.Where(dev => userEntities.Contains(dev.Id));
            }
            else if (user.IsInRole("ProjectManager"))
            {
                authedQuery = authedQuery.Where(dev => userEntities.Contains(dev.ProjectId));
            }

            if (exp != null)
            {
                authedQuery = authedQuery.Where(exp);
            }
            return authedQuery.ToList();
        }

        public List<KsDustProject> GetAuthedProjects(Expression<Func<KsDustProject, bool>> exp)
        {
            var user = _owinContext.Authentication.User;
            IQueryable<KsDustProject> authedQuery = _dbContext.KsDustProjects;

            var userId = Guid.Parse(user.Identity.GetUserId());
            var userEntities =
                    _dbContext.UserRelatedEntities.Where(ent => ent.User == userId).Select(id => id.Entity).ToList();

            if (user.IsInRole("DistrictManager"))
            {

                authedQuery = authedQuery.Where(prj => userEntities.Contains(prj.DistrictId));
            }

            if (user.IsInRole("ProjectManager"))
            {
                authedQuery = authedQuery.Where(prj => userEntities.Contains(prj.Id));
            }

            if (exp != null)
            {
                authedQuery = authedQuery.Where(exp);
            }
            return authedQuery.ToList();
        }
    }
}