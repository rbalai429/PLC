
using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using System.Configuration;

namespace HPPlc.Models
{
    public class GenerateBitlyLink
    {
        //string baseUrl = "https://api.ssl.bitly.com/v4/shorten";
        //string accessToken = "e771186b1e31a274f12aa4c463c66b1741874ddc";
        string baseUrl = ConfigurationManager.AppSettings["bitlyBaseUrl"].ToString();
        string accessToken = ConfigurationManager.AppSettings["bitlyAccessToken"].ToString();

        public string Generate(string longUrl)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            string bitlyUrl = "";
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
               
                string requestUrl = string.Format(baseUrl + "?access_token=" + accessToken + "&longUrl=" + longUrl +"&format=txt",baseUrl,accessToken,Uri.EscapeDataString(longUrl));
                HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    bitlyUrl = response.Content.ReadAsStringAsync().Result;
                }
            }

            //dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(bitlyUrl);
            //return jsonResponse["link"];
            return bitlyUrl;
        }

        public string Shorten(string longUrl)
        {
            //string post = "{\"group_guid\": \"" + groupId + "\", \"long_url\": \"" + longUrl + "\"}";
            string post = "{ \"long_url\": \"" + longUrl + "\"}";// If you've a free account.
            string shortUrl = "";

			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
				   | SecurityProtocolType.Tls11
				   | SecurityProtocolType.Tls12
				   | SecurityProtocolType.Ssl3;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);

            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                //request.ServicePoint.Expect100Continue = true;
                request.Method = "POST";
                request.ContentLength = post.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Cache-Control", "no-cache");
                request.Host = "api-ssl.bitly.com";
                request.Headers.Add("Authorization", "Bearer " + accessToken);
                request.UseDefaultCredentials = true;
                request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                    requestStream.Write(postBuffer, 0, postBuffer.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(responseStream))
                        {
                            string json = responseReader.ReadToEnd();

                            var objlink = new BitlyLinkJson();
                            objlink = JsonConvert.DeserializeObject<BitlyLinkJson>(json);
                            shortUrl = objlink.Link.AbsoluteUri;
                            //shortUrl = Regex.Match(json, @"""link"": ?""(?[^,;]+)""").Groups["link"].Value;
                            // shortUrl = Regex.Match(json, @"""link"": ?""(?[^,;]+)""").Groups["link"].Value;

                            // return Regex.Match(json, @"{""short_url"":""([^""]*)""[,}]").Groups[1].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // LogManager.WriteLog(ex.Message);
            }

            if (shortUrl.Length > 0) // this check is if your bit.ly rate exceeded
                return shortUrl;
            else
                return longUrl;
        }
    }
}

public partial class BitlyLinkJson
{
    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("link")]
    public Uri Link { get; set; }

    [JsonProperty("custom_bitlinks")]
    public List<object> CustomBitlinks { get; set; }

    [JsonProperty("long_url")]
    public Uri LongUrl { get; set; }

    [JsonProperty("archived")]
    public bool Archived { get; set; }

    [JsonProperty("tags")]
    public List<object> Tags { get; set; }

    [JsonProperty("deeplinks")]
    public List<object> Deeplinks { get; set; }

    [JsonProperty("references")]
    public References References { get; set; }
}



public partial class References
{
    [JsonProperty("group")]
    public Uri Group { get; set; }
}