using DidiSoft.Pgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace HPPlc.Controllers
{
    public class GeneratePGPLibController : SurfaceController
    {
		public ActionResult PGPConvertReport()
		{
			string dataFile = Server.MapPath("/SFMC_PLC_ExtractReport/Sample_Records.csv");
			//string publicKeyFile = Server.MapPath("/Key/IN_Digitas_PublicKey.asc");
			string publicKeyFile = Server.MapPath("/Key/kleorsapub.asc");
			string encryptedFile = Server.MapPath("/SFMC_PLC_ExtractReport/Sample_Records.csv.gpg");
			bool asciiArmour = false;

			PGPLibAsync pgp = new PGPLibAsync();
			pgp.EncryptFileAsync(dataFile, publicKeyFile, encryptedFile, asciiArmour);

			return View();
		}
	}
}