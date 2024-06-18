using APIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.WebApi;

namespace APIS.Controllers
{
    [RoutePrefix("api/plc")]
    public class ReactFrontController : UmbracoApiController
    {
        private readonly IVariationContextAccessor _variationContextAccessor;
        public ReactFrontController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }

        [Route("header")]
        [HttpPost]
        public HttpResponseMessage header()
        {
            HttpResponseMessage headerResponse = new HttpResponseMessage();
            Response response = new Response();

            response.StatusCode = 1;
            response.StatusMessage = "Done";
            response.Result = "";

            headerResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);

            return headerResponse;
        }
    }
}