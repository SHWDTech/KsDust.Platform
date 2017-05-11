using System;
using System.Collections.Generic;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Models.Admin
{
    public class GeneralComparsionResultViewModel
    {
        public List<Guid> TargetObjects { get; set; } = new List<Guid>();

        public AverageType DataType { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}