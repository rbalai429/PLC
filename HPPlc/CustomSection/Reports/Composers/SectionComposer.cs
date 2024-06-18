using HPPlc.CustomSection.Reports.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Composing;
using Umbraco.Web;
namespace HPPlc.CustomSection.Reports.Composers
{
    public class SectionComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Sections().Append<ReportsSection>();
        }
    }
}