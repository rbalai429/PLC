using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.Sections;

namespace HPPlc.CustomSection.Reports.Sections
{
    public class ReportsSection : ISection
    {
        /// <inheritdoc />
        public string Alias => "Reports";

        /// <inheritdoc />
        public string Name => "Reports";
    }
}