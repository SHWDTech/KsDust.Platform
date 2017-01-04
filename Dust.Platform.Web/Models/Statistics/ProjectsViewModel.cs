using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Dust.Platform.Web.Models.Statistics
{
    public class ProjectsViewModel
    {
        public Guid? DistrictId { get; set; }

        public List<SelectListItem> Districts { get; set; }

        public Guid? EnterpriseId { get; set; }

        public List<SelectListItem> Enterprises { get; set; }
    }
}