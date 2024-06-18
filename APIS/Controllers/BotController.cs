using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIS.Controllers
{
    public class BotController : Controller
    {
		public string BotCheck()
		{
			//Result ObjResult = new Result();
			//ObjResult.Status = 0;
			//ObjResult.Message = "Email Id can not be blank!!";
			//response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);

			return "Hello India";
		}
	}
}