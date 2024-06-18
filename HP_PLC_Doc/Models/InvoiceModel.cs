using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HP_PLC_Doc.Models
{
	public class InvoiceModel
	{
		public string Name
		{
			get; set;
		}
		public string Address
		{
			get; set;
		}
		public Boolean HasAddress
		{
			get; set;
		} = false;
		public string GSTNo
		{
			get; set;
		}
		public Boolean HasGSTNo
		{
			get; set;
		} = false;
		public decimal SGST
		{
			get; set;
		}
		public Boolean HasSGST
		{
			get; set;
		} = false;
		public decimal CGST
		{
			get; set;
		}
		public Boolean HasCGST
		{
			get; set;
		} = false;
		public decimal Discount
		{
			get; set;
		}
		public Boolean HasDiscount
		{
			get; set;
		} = false;
		public string UserEmailId
		{
			get; set;
		}
		public string InvoiceNo
		{
			get; set;
		}
		public List<InvoiceData> InvoiceList
		{
			get; set;
		}
		public string PaymentMethod
		{
			get; set;
		}
		public DateTime? PaymentDate
		{
			get; set;
		}
		public string ReferenceId
		{
			get; set;
		}

		public string SAC
		{
			get; set;
		}
		public string Logo
		{
			get; set;
		}

		public string ComputerGeneratedText
		{
			get; set;
		}
		public string BelowAddress
		{
			get; set;
		}

		public string InvoiceTitle
		{
			get; set;
		}

		public string InvoiceFontFamily
		{
			get; set;
		}
		public int? InvoiceFontSize
		{
			get; set;
		}

		public string PlaceOfSupply
		{
			get; set;
		}
		public int MaxPrice
		{
			get; set;
		} = 0;

		public int DiscountPrice
		{
			get; set;
		} = 0;
	}

	public class InvoiceData
	{
		public int? UserId
		{
			get; set;
		}
		public string SubscriptionId
		{
			get; set;
		}
		public string SubscriptionName
		{
			get; set;
		}
		public string PaymentDate
		{
			get; set;
		}
		public string SubscriptionDtls
		{
			get; set;
		}
		public string SubscriptionStartDate
		{
			get; set;
		}
		public string SubscriptionEndDate
		{
			get; set;
		}
		public string PlaseOfSupply
		{
			get; set;
		}
		public string SAC
		{
			get; set;
		}
		public decimal SubscriptionPrice
		{
			get; set;
		}
		public decimal Discount
		{
			get; set;
		} = 0;

		public int MaxPrice
		{
			get; set;
		} = 0;

		public int DiscountPrice
		{
			get; set;
		} = 0;
		public string Mode
		{
			get; set;
		}
		public string InvoiceNo { get; set; }
		public string PartCode { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string WhatsAppNoPrefix { get; set; }
		public string WhatsAppNo { get; set; }
		public int Ranking { get; set; } = 0;
		public string AgeGroup { get; set; }
		public string TransactionId
		{
			get; set;
		}
		public string Tax
		{
			get; set;
		}
		public string Category
		{
			get; set;
		}
		public int? SubscriptionCount
		{
			get; set;
		}
		public string Coupon
		{
			get; set;
		}
		public string CouponSource
		{
			get; set;
		} = "";
		public decimal CouponDiscountAmt
		{
			get; set;
		}
		public decimal CouponDiscountAmtItemWise
		{
			get; set;
		}
		public string CouponNameDiscountItemWise
		{
			get; set;
		} = "";
		
	}
	public class InvoiceDetails
	{

		public int? InvoiceId
		{
			get; set;
		}
		public string SubscriptionId
		{
			get; set;
		}
		public string InvoicePDFUrl
		{
			get; set;
		}
		public string PaymentId
		{
			get; set;
		}
		public int? UserId
		{
			get; set;
		}
	}
}