using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class PaymentStatus
	{
		public string PaymentId { get; set; }
		public string txn_status { get; set; }
		public string txn_msg { get; set; }
		public string txn_err_msg { get; set; }
		public string clnt_txn_ref { get; set; }
		public string tpsl_bank_cd { get; set; }
		public string tpsl_txn_id { get; set; }
		public string txn_amt { get; set; }
		public string clnt_rqst_meta { get; set; }
		public string tpsl_txn_time { get; set; }
		public string tpsl_rfnd_id { get; set; }
		public string bal_amt { get; set; }
		public string rqst_token { get; set; }
		public string PaymentMode { get; set; }
	}
}