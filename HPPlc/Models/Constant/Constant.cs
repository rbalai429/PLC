using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Constant
{
	//   public class Constant
	//   {
	//       public static int SMTFForUser => 3289;
	//       public static int SMTFForAdmin => 4521;
	//       public static int SMTFForFAQRequestHandler => 4522;
	//       public static int InvoiceDetails => 4532;
	//       public static int ExcelUserDiscountAmount => 200;
	//       public static int RegistrationMailer => 5797;

	//	public static int RegistrationMailerForExcel => 5848;
	//}
	public class Constant
	{
		public static int RegistrationMailer => 5797;
	}
	public class ConstantUserType
	{
		public static string Admin => "Admin";
		public static string User => "User";
		public static string FAQ => "FAQ";
		public static string Reports => "Reports";

		public static string Forget => "Forget";
		public static string CouponCode => "CouponCode";
	}
	public class ConstantRegistrationType
	{
		public static string User => "User";
		public static string Excel => "Excel";
	}
	public class ConstantDocType
	{
		public static string InvoiceDocType => "invoiceDetails";
	}
}