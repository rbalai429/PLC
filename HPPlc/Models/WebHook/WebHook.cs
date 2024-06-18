using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WebHook
{
	public class JsonHookData
	{
		[JsonProperty("entry")]
		public string Entry { get; set; }

	}

	public class JsonData
	{
		[JsonProperty("entry")]
		public List<Entry> Entry { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }
	}

	public class Entry
	{
		[JsonProperty("changes")]
		public List<Change> changes { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("time")]
		public int Time { get; set; }
	}

	public class Change
	{
		[JsonProperty("field")]
		public string Field { get; set; }

		[JsonProperty("value")]
		public Value Value { get; set; }
	}

	public class Value
	{
		[JsonProperty("ad_id")]
		public string AdId { get; set; }

		[JsonProperty("form_id")]
		public string FormId { get; set; }

		[JsonProperty("leadgen_id")]
		public string LeadGenId { get; set; }

		[JsonProperty("created_time")]
		public int CreatedTime { get; set; }

		[JsonProperty("page_id")]
		public string PageId { get; set; }

		[JsonProperty("adgroup_id")]
		public string AdGroupId { get; set; }
	}

	public class LeadData
	{
		[JsonProperty("created_time")]
		public string CreatedTime { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("field_data")]
		public List<FieldData> FieldData { get; set; }
	}

	public class LeadData_Consent
	{
		[JsonProperty("custom_disclaimer_responses")]
		public List<FieldData_Consent> custom_disclaimer_responses { get; set; }

	}
	public class FieldData
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("values")]
		public List<string> Values { get; set; }
	}

	public class FieldData_Consent
	{
		public string checkbox_key { get; set; }
		public string is_checked { get; set; }
		//[JsonProperty("whatsapp")]
		//public string whatsapp { get; set; }

		//[JsonProperty("phone")]
		//public string phone { get; set; }

		//[JsonProperty("email_id")]
		//public string email_id { get; set; }

		//[JsonProperty("hp_may_contact_me_with_personalized_offers,_support_updates_and_events_beyond_hp_print_learn_center")]
		//public string hp_may_contact_me_with_personalized_offers { get; set; }
	}
	public class LeadFormData
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("leadgen_export_csv_url")]
		public string CsvExportUrl { get; set; }

		[JsonProperty("locale")]
		public string Locale { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

	}

	public class Feed
	{
		public string field { get; set; }
		public value value { get; set; }
	}

	public class value
	{
		public string item { get; set; }
		public string post_id { get; set; }
		public string verb { get; set; }
		public int published { get; set; }
		public string message { get; set; }
	}
}