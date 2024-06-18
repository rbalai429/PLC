
using HPPlc.Models.Mailer;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Mvc;

namespace HPPlc.Models.HtmlRenderHelper
{
    public class HtmlRenderHelper
    {
        // GET: Mailer
       
        public Responce GetRegistartionHtml(RegistrationMailerModel model)
        {
            Responce responce = new Responce();
            try
            {
                string html = RenderViewToString(model.ViewName, model);
                if (html != null)
                {
                    html = html.Replace("$Name", model.Name);
                    html = html.Replace("#Link", model.Link);
                    responce.StatusCode = HttpStatusCode.OK;
                    responce.Result = html;
                }
            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.Message;
            }
            return responce;

        }
        public Responce GetSubscriptionHtml(SubscriptionMailerModel model)
        {
            Responce responce = new Responce();
            try
            {
                string html = RenderViewToString(model.ViewName, model);
                if (html != null)
                {
                    responce.StatusCode = HttpStatusCode.OK;
                    responce.Result = html;
                }
            }
            catch (Exception ex)
            {
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = ex.Message;
            }
            return responce;

        }

        public string RenderViewToString(string viewPath, object model, bool partial = false, ViewDataDictionary viewDataDictionary = null)
        {
            try
            {
                // first find the ViewEngine for this view
                ViewEngineResult viewEngineResult = null;
                if (partial)
                    viewEngineResult = ViewEngines.Engines.FindPartialView(FakeControllerContext, viewPath);
                else
                    viewEngineResult = ViewEngines.Engines.FindView(FakeControllerContext, viewPath, null);

                if (viewEngineResult == null)
                    throw new FileNotFoundException("View cannot be found.");

                if (viewDataDictionary == null)
                    viewDataDictionary = new ViewDataDictionary();

                // get the view and attach the model to view data
                var view = viewEngineResult.View;
                viewDataDictionary.Model = model;

                using (var sw = new StringWriter())
                {
                    var ctx = new ViewContext(FakeControllerContext, view, viewDataDictionary, FakeControllerContext.Controller.TempData, sw);
                    view.Render(ctx, sw);
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static HttpContext FakeHttpContext()
        {
            //create fake request
            var httpRequest = new HttpRequest("", ConfigurationManager.AppSettings["SiteUrl"], "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(), 10, true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null, CallingConventions.Standard,
                    new[] { typeof(HttpSessionStateContainer) },
                    null)
                .Invoke(new object[] { sessionContainer });

            return httpContext;
        }

        private static HttpContext Ctx => HttpContext.Current ?? FakeHttpContext();


        private static ControllerContext _fakeControllerContext;
        private static ControllerContext FakeControllerContext
        {
            get
            {
                try
                {
                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Fake");
                    _fakeControllerContext = new ControllerContext(new HttpContextWrapper(Ctx), routeData, new FakeController());
                }
                catch (Exception ex)
                {
                    throw;
                }
               
                return _fakeControllerContext;
            }
        }
        private class FakeController : ControllerBase
        {
            protected override void ExecuteCore()
            {
            }
        }

    }
}