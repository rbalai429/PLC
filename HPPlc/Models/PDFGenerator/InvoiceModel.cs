using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.PDFGenerator
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
            get;set;
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
		public string Logo
		{
			get; set;
		}
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
        public DateTime? PaymentDate
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public DateTime? SubscriptionStartDate
        {
            get; set;
        }
        public DateTime? SubscriptionEndDate
        {
            get; set;
        }
        public string PlaseOfSupply
        {
            get; set;
        }
        public long SAC
        {
            get; set;
        }
        public decimal? SubscriptionPrice
        {
            get; set;
        }
        public decimal? Discount
        {
            get;set;
        }
		public string Mode
		{
			get; set;
		}

		public string Name { get; set; }
		public string Email { get; set; }
		public string WhatsAppNoPrefix { get; set; }
		public string WhatsAppNo { get; set; }
		
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
    public class ReferralFromExcelUser
    {
        public int UserId
        {
            get; set;
        }
        public string SubscriptionId
        {
            get; set;
        }
        public string Amount
        {
            get; set;
        }
        public string PaymentId
        {
            get; set;
        }
        public string Mode
        {
            get; set;
        }
    }

}