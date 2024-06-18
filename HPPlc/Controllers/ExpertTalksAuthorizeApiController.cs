using HPPlc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.WebApi;

namespace HPPlc.Controllers
{
    public class ExpertTalksAuthorizeApiController : UmbracoAuthorizedApiController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;
        public ExpertTalksAuthorizeApiController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }

      

        [System.Web.Http.HttpGet]
        public virtual HttpResponseMessage ExportToExcel()
        {
            List<ExpertTalk> List = new List<ExpertTalk>();
            dbProxy _db = new dbProxy();

            List<SetParameters> sp = new List<SetParameters>()
            {
                new SetParameters{ ParameterName = "@QType", Value = "1" },
            };
            List = _db.GetDataMultiple("GetExportTalk", List, sp);
            if (List != null && List.Count > 0)
            {

                foreach (var item in List)
                {
                    item.Name = clsCommon.Decrypt(item.Name);
                    item.Email = clsCommon.Decrypt(item.Email);
                    item.Contact = clsCommon.Decrypt(item.Contact);
                }
            }
            clsExpertHelper clsExpertHelper = new clsExpertHelper();
            byte[] bytes = clsExpertHelper.ListToExcel(List, "Reprts");
           
            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "WebinarReports.xlsx";
            return result;

        }
    }
}