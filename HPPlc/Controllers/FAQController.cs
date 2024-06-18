using HPPlc.Models.FAQ;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace HPPlc.Controllers
{
    public class FAQController : SurfaceController
    {
        // GET: FAQ
        [HttpPost]
        public ActionResult SaveRequestDetails(FAQRequestModel fAQRequest)
        {
            return Json(FAQHelper.SaveRequest(fAQRequest), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetHolidayList()
        {
            return Json(FAQHelper.GetHolidayList(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetTimeList(string SelectedDate)
        {
            return Json(FAQHelper.GetTimeList(SelectedDate), JsonRequestBehavior.AllowGet);
        }
    }
}