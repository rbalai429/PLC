using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static HPPlc.Models.HttpClientServices.ResultAndStatusModel;

namespace HPPlc.Models.HttpClientServices
{
    public class ApiCallServices
    {
        public string token;
        public ApiCallServices(string tokentype = "")
        {
            GetAPIToken(tokentype);
        }
        public Responce GetAPIToken(string type)
        {
            Responce response = new Responce();
            try
            {
                using (var client = new HttpClient())
                {
                    TokenData tokenData = new TokenData();
                    TokenInput input;
                    if (type == "unsubscribe")
                    {
                        input = new TokenInput()
                        {
                            client_id = System.Configuration.ConfigurationManager.AppSettings["client_id_UnSubscribe"],
                            client_secret = System.Configuration.ConfigurationManager.AppSettings["client_secret_Unsubscribe"],
                        };
                       
                    }
                    else if (type == "registrationInviteUser")
                    {
                        input = new TokenInput()
                        {
                            client_id = System.Configuration.ConfigurationManager.AppSettings["client_id_registrationUserInvite"],
                            client_secret = System.Configuration.ConfigurationManager.AppSettings["client_secret_registrationUserInvite"],
                        };

                    }
                    else
                    {
                        input = new TokenInput()
                        {
                            client_id = System.Configuration.ConfigurationManager.AppSettings["client_id"],
                            client_secret = System.Configuration.ConfigurationManager.AppSettings["client_secret"],
                        };
                    }

                    var json = JsonConvert.SerializeObject(input);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync(ConstantUrl.GetTokenUrl, data);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<TokenData>();
                        readTask.Wait();
                        tokenData = readTask.Result;
                        response.Result = tokenData;
                        token = tokenData.access_token;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Message = "Server error. Please contact administrator.";
                    }

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public Responce PostRegistartionData(RegistrationPostModel input, string dataSource)
        {
            Responce response = new Responce();
            try
            {
                if (input != null && input.Data != null)
                {
                    if (token != null && !string.IsNullOrWhiteSpace(token))
                    {
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var json = JsonConvert.SerializeObject(input);
                            var data = new StringContent(json, Encoding.UTF8, "application/json");


                            var responseTask = client.PostAsync(ConstantUrl.PostRegistarationUrl, data);
                            responseTask.Wait();
                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                string requestId = String.Empty;
                                var readTask = result?.Content?.ReadAsAsync<PostResult>();

                                try
                                {
                                    readTask?.Wait();
                                    if (readTask != null && readTask?.Result != null)
                                    {
                                        response.Result = readTask?.Result;
                                        requestId = readTask?.Result?.requestId;
                                    }
                                }
                                catch { }

                                var user = input.Data;
                                if (user != null)
                                {
                                    bool IsSave = SaveRequestId(requestId, user.UserId.Value, user.TransationId, dataSource, result.IsSuccessStatusCode);
                                    if (IsSave)
                                    {
                                        response.StatusCode = HttpStatusCode.OK;
                                        response.Message = "Request Id Save..";
                                    }
                                    else
                                    {
                                        response.StatusCode = HttpStatusCode.InternalServerError;
                                        response.Message = "Request Id Not Save..";
                                    }
                                }
                            }
                            else //web api sent error response 
                            {
                                //log response status here..
                                response.StatusCode = HttpStatusCode.InternalServerError;
                                response.Message = "Server error. Please contact administrator.";
                            }
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Message = "Please provide token..";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        public Responce BonusSubscriptionTracker(List<BonusSubscriptionData> input, string dataSource)
        {
            Responce response = new Responce();
            try
            {
                if (input != null && input.Count > 0 && input.FirstOrDefault().values != null && input.FirstOrDefault().keys != null)
                {
                    if (token != null && !string.IsNullOrWhiteSpace(token))
                    {
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var json = JsonConvert.SerializeObject(input);
                            var data = new StringContent(json, Encoding.UTF8, "application/json");


                            var responseTask = client.PostAsync(ConstantUrl.SFMCBonusSubscription, data);
                            responseTask.Wait();
                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                string requestId = String.Empty;
                                var readTask = result?.Content?.ReadAsAsync<PostResult>();

                                try
                                {
                                    readTask?.Wait();
                                    if (readTask != null && readTask?.Result != null)
                                    {
                                        response.Result = readTask?.Result;
                                        requestId = readTask?.Result?.requestId;
                                    }
                                }
                                catch { }

                                var user = input.FirstOrDefault().values;
                                if (user != null)
                                {
                                    bool IsSave = SaveRequestId(requestId, user.userId, user.TransationId, dataSource, result.IsSuccessStatusCode);
                                    if (IsSave)
                                    {
                                        response.StatusCode = HttpStatusCode.OK;
                                        response.Message = "Request Id Save..";
                                    }
                                    else
                                    {
                                        response.StatusCode = HttpStatusCode.InternalServerError;
                                        response.Message = "Request Id Not Save..";
                                    }
                                }
                            }
                            else //web api sent error response 
                            {
                                //log response status here..
                                response.StatusCode = HttpStatusCode.InternalServerError;
                                response.Message = "Server error. Please contact administrator.";
                            }
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Message = "Please provide token..";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        public Responce PostRegistartionDataRegistrationUserInvite(List<UpdatePostedModel> input, string dataSource)

        {

            Responce response = new Responce();

            try

            {

                if (input != null && input.Count > 0 && input.FirstOrDefault().values != null && input.FirstOrDefault().keys != null)

                {

                    if (token != null && !string.IsNullOrWhiteSpace(token))

                    {

                        using (var client = new HttpClient())

                        {

                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            var json = JsonConvert.SerializeObject(input);

                            var data = new StringContent(json, Encoding.UTF8, "application/json");


                            var responseTask = client.PostAsync(ConstantUrl.PostRegistarationUrlInviteUser, data);

                            responseTask.Wait();

                            var result = responseTask.Result;

                            if (result.IsSuccessStatusCode)

                            {

                                string requestId = String.Empty;

                                var readTask = result?.Content?.ReadAsAsync<PostResult>();

                                try

                                {

                                    readTask?.Wait();

                                    if (readTask != null && readTask?.Result != null)

                                    {

                                        response.Result = readTask?.Result;

                                        requestId = readTask?.Result?.requestId;

                                    }

                                }

                                catch { }

                                var user = input?.FirstOrDefault()?.values;

                                if (user != null)

                                {

                                    bool IsSave = SaveRequestId(requestId, user.UserId.Value, user.TransationId, dataSource, result.IsSuccessStatusCode);

                                    if (IsSave)

                                    {

                                        response.StatusCode = HttpStatusCode.OK;

                                        response.Message = "Request Id Save..";

                                    }

                                    else

                                    {

                                        response.StatusCode = HttpStatusCode.InternalServerError;

                                        response.Message = "Request Id Not Save..";

                                    }

                                }

                            }

                            else //web api sent error response 

                            {

                                //log response status here..

                                response.StatusCode = HttpStatusCode.InternalServerError;

                                response.Message = "Server error. Please contact administrator.";

                            }

                        }

                    }

                    else

                    {

                        response.StatusCode = HttpStatusCode.BadRequest;

                        response.Message = "Please provide token..";

                        return response;

                    }

                }

            }

            catch (Exception ex)

            {

                response.StatusCode = HttpStatusCode.InternalServerError;

                response.Message = ex.Message;

                return response;

            }

            return response;

        }

        public Responce UpdateRegistartionData(List<UpdatePostedModel> input, string dataSource)
        {
            Responce response = new Responce();
            try
            {
                if (input != null && input.Count > 0 && input.FirstOrDefault().values != null && input.FirstOrDefault().keys != null)
                {
                    if (token != null && !string.IsNullOrWhiteSpace(token))
                    {
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var json = JsonConvert.SerializeObject(input);
                            var data = new StringContent(json, Encoding.UTF8, "application/json");


                            var responseTask = client.PostAsync(ConstantUrl.SFMCPostUpdationUrl, data);
                            responseTask.Wait();
                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                string requestId = String.Empty;
                                var readTask = result?.Content?.ReadAsAsync<PostResult>();

                                try
                                {
                                    readTask?.Wait();
                                    if (readTask != null && readTask?.Result != null)
                                    {
                                        response.Result = readTask?.Result;
                                        requestId = readTask?.Result?.requestId;
                                    }
                                }
                                catch { }

                                var user = input.FirstOrDefault();
                                if (user != null)
                                {
                                    bool IsSave = SaveRequestId(requestId, (int)user.values.UserId, user.values.TransationId, dataSource, result.IsSuccessStatusCode);
                                    if (IsSave)
                                    {
                                        response.StatusCode = HttpStatusCode.OK;
                                        response.Message = "Request Id Save..";
                                    }
                                    else
                                    {
                                        response.StatusCode = HttpStatusCode.InternalServerError;
                                        response.Message = "Request Id Not Save..";
                                    }
                                }
                            }
                            else //web api sent error response 
                            {
                                //log response status here..
                                response.StatusCode = HttpStatusCode.InternalServerError;
                                response.Message = "Server error. Please contact administrator.";
                            }
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Message = "Please provide token..";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        //public Responce UpdateRegistartionData(List<UpdatePostedModel> input, string dataSource)
        //{
        //    Responce response = new Responce();
        //    try
        //    {
        //        if (input != null && input.Count > 0 && input.FirstOrDefault().values != null && input.FirstOrDefault().keys != null)
        //        {
        //            if (token != null && !string.IsNullOrWhiteSpace(token))
        //            {
        //                using (var client = new HttpClient())
        //                {
        //                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //                    var json = JsonConvert.SerializeObject(input);
        //                    var data = new StringContent(json, Encoding.UTF8, "application/json");


        //                    var responseTask = client.PostAsync(ConstantUrl.SFMCPostUpdationUrl, data);
        //                    responseTask.Wait();
        //                    var result = responseTask.Result;
        //                    if (result.IsSuccessStatusCode)
        //                    {
        //                        string requestId = String.Empty;
        //                        var readTask = result?.Content?.ReadAsAsync<PostResult>();

        //                        try
        //                        {
        //                            readTask?.Wait();
        //                            if (readTask != null && readTask?.Result != null)
        //                            {
        //                                response.Result = readTask?.Result;
        //                                requestId = readTask?.Result?.requestId;
        //                            }
        //                        }
        //                        catch { }

        //                        var user = input?.FirstOrDefault()?.values;

        //                        if (user != null)
        //                        {
        //                            bool IsSave = SaveRequestId(requestId, user.UserId.Value, user.TransationId, dataSource, result.IsSuccessStatusCode);
        //                            if (IsSave)
        //                            {
        //                                response.StatusCode = HttpStatusCode.OK;
        //                                response.Message = "Request Id Save..";
        //                            }
        //                            else
        //                            {
        //                                response.StatusCode = HttpStatusCode.InternalServerError;
        //                                response.Message = "Request Id Not Save..";
        //                            }
        //                        }
        //                    }
        //                    else //web api sent error response 
        //                    {
        //                        //log response status here..
        //                        response.StatusCode = HttpStatusCode.InternalServerError;
        //                        response.Message = "Server error. Please contact administrator.";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                response.StatusCode = HttpStatusCode.BadRequest;
        //                response.Message = "Please provide token..";
        //                return response;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Message = ex.Message;
        //        return response;
        //    }

        //    return response;
        //}

        public Responce UnsubscribeUser(UnSubscribePostModel input)
        {
            Responce response = new Responce();
            try
            {
                if (token != null && !string.IsNullOrWhiteSpace(token))
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var json = JsonConvert.SerializeObject(input);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        var responseTask = client.PostAsync(ConstantUrl.PostRegistarationUrl, data);
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            string requestId = String.Empty;
                            var readTask = result?.Content?.ReadAsAsync<PostResult>();

                            try
                            {
                                readTask?.Wait();
                                if (readTask != null && readTask?.Result != null)
                                {
                                    response.Result = readTask?.Result;
                                    requestId = readTask?.Result?.requestId;
                                }
                            }
                            catch { }

                            var user = input.Data;
                            if (readTask.Result.requestId != null && user != null)
                            {

                                bool IsSave = SaveRequestId(requestId, user.UserId.Value, "", user.TransactionType, result.IsSuccessStatusCode);
                                if (IsSave)
                                {
                                    response.StatusCode = HttpStatusCode.OK;
                                    response.Message = "Request Id Save..";
                                }
                                else
                                {
                                    response.StatusCode = HttpStatusCode.InternalServerError;
                                    response.Message = "Request Id Not Save..";
                                }
                            }
                        }
                        else //web api sent error response 
                        {
                            //log response status here..
                            response.StatusCode = HttpStatusCode.InternalServerError;
                            response.Message = "Server error. Please contact administrator.";
                        }

                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Please provide token..";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        //public Responce PostRegistartionData(RegistrationPostModel input,string datSource)
        //{
        //    Responce response = new Responce();
        //    try
        //    {
        //        if (input != null && input.Data != null)
        //        {
        //            if (token != null && !string.IsNullOrWhiteSpace(token))
        //            {
        //                //var client = new HttpClient();
        //                //var request = new HttpRequestMessage(HttpMethod.Post, ConstantUrl.PostRegistarationUrl);
        //                //request.Headers.Add("Authorization", "Bearer " + token);
        //                //var json = JsonConvert.SerializeObject(input);
        //                //var data = new StringContent(json, Encoding.UTF8, "application/json");
        //                //var content = new StringContent(json, null, "application/json");
        //                //request.Content = content;
        //                //var responses = client.SendAsync(request);
        //                //responses.EnsureSuccessStatusCode();
        //                //Console.WriteLine(responses.Content.ReadAsStringAsync());

        //                using (var client = new HttpClient())
        //                {
        //                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //                    var json = JsonConvert.SerializeObject(input);
        //                    var data = new StringContent(json, Encoding.UTF8, "application/json");


        //                    var responseTask = client.PostAsync(ConstantUrl.PostRegistarationUrl, data);
        //                    responseTask.Wait();
        //                    var result = responseTask.Result;
        //                    if (result.IsSuccessStatusCode)
        //                    {
        //                        var readTask = result.Content.ReadAsAsync<PostResult>();
        //                        readTask.Wait();
        //                        response.Result = readTask.Result;

        //                        var user = input.Data;
        //                        if (readTask.Result.requestId != null && user != null)
        //                        {
        //                            bool IsSave = SaveRequestId(readTask?.Result?.requestId, user.UserId.Value, user.TransationId, datSource);
        //                            if (IsSave)
        //                            {
        //                                response.StatusCode = HttpStatusCode.OK;
        //                                response.Message = "Request Id Save..";
        //                            }
        //                            else
        //                            {
        //                                response.StatusCode = HttpStatusCode.InternalServerError;
        //                                response.Message = "Request Id Not Save..";
        //                            }
        //                        }
        //                    }
        //                    else //web api sent error response 
        //                    {
        //                        //log response status here..
        //                        response.StatusCode = HttpStatusCode.InternalServerError;
        //                        response.Message = "Server error. Please contact administrator.";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                response.StatusCode = HttpStatusCode.BadRequest;
        //                response.Message = "Please provide token..";
        //                return response;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Message = ex.Message;
        //        return response;
        //    }

        //    return response;
        //}

        //public Responce UnsubscribeUser(UnSubscribePostModel input)
        //{
        //    Responce response = new Responce();
        //    try
        //    {
        //        if (token != null && !string.IsNullOrWhiteSpace(token))
        //        {
        //            using (var client = new HttpClient())
        //            {
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //                var json = JsonConvert.SerializeObject(input);
        //                var data = new StringContent(json, Encoding.UTF8, "application/json");

        //                var responseTask = client.PostAsync(ConstantUrl.PostRegistarationUrl, data);
        //                responseTask.Wait();
        //                var result = responseTask.Result;
        //                if (result.IsSuccessStatusCode)
        //                {
        //                    var readTask = result.Content.ReadAsAsync<PostResult>();
        //                    readTask.Wait();
        //                    response.Result = readTask.Result;

        //                    var user = input.Data;
        //                    if (readTask.Result.requestId != null && user != null)
        //                    {

        //                        bool IsSave = SaveRequestId(readTask?.Result?.requestId, user.UserId.Value,"",user.TransactionType);
        //                        if (IsSave)
        //                        {
        //                            response.StatusCode = HttpStatusCode.OK;
        //                            response.Message = "Request Id Save..";
        //                        }
        //                        else
        //                        {
        //                            response.StatusCode = HttpStatusCode.InternalServerError;
        //                            response.Message = "Request Id Not Save..";
        //                        }
        //                    }
        //                }
        //                else //web api sent error response 
        //                {
        //                    //log response status here..
        //                    response.StatusCode = HttpStatusCode.InternalServerError;
        //                    response.Message = "Server error. Please contact administrator.";
        //                }

        //            }
        //        }
        //        else
        //        {
        //            response.StatusCode = HttpStatusCode.BadRequest;
        //            response.Message = "Please provide token..";
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Message = ex.Message;
        //        return response;
        //    }

        //    return response;
        //}
        public Responce GetStatusByRequestId(string RequestId)
        {
            Responce response = new Responce();
            try
            {
                if (RequestId == null || string.IsNullOrWhiteSpace(RequestId))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Please Enter RequestId";
                    return response;
                }
                //if (token != null && !string.IsNullOrWhiteSpace(token))
                //{
                //    using (var client = new HttpClient())
                //    {
                //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //        string Url = ConstantUrl.GetStatusByRequestId.Replace("{requestId}", RequestId);
                //        var responseTask = client.GetAsync(Url);
                //        responseTask.Wait();
                //        var result = responseTask.Result;
                //        if (result.IsSuccessStatusCode)
                //        {
                //            var readTask = result.Content.ReadAsAsync<RequestStatus>();
                //            readTask.Wait();
                //            response.Result = readTask.Result;
                //            response.StatusCode = HttpStatusCode.OK;
                //        }
                //        else //web api sent error response 
                //        {
                //            //log response status here..
                //            response.StatusCode = HttpStatusCode.InternalServerError;
                //            response.Message = "Server error. Please contact administrator.";
                //        }

                //    }
                //}
                //else
                //{
                //    response.StatusCode = HttpStatusCode.BadRequest;
                //    response.Message = "Please provide token..";
                //    return response;
                //}
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        //public Responce GetResultByRequestId(string RequestId)
        //{
        //    Responce response = new Responce();
        //    try
        //    {
        //        if (RequestId == null || string.IsNullOrWhiteSpace(RequestId))
        //        {
        //            response.StatusCode = HttpStatusCode.BadRequest;
        //            response.Message = "Please Enter RequestId";
        //            return response;
        //        }
        //        if (token != null && !string.IsNullOrWhiteSpace(token))
        //        {
        //            using (var client = new HttpClient())
        //            {
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //                string Url = ConstantUrl.GetResultByRequestId.Replace("{requestId}", RequestId);
        //                var responseTask = client.GetAsync(Url);
        //                responseTask.Wait();
        //                var result = responseTask.Result;
        //                if (result.IsSuccessStatusCode)
        //                {
        //                    var readTask = result.Content.ReadAsAsync<Result>();
        //                    readTask.Wait();
        //                    response.Result = readTask.Result;
        //                    response.StatusCode = HttpStatusCode.OK;
        //                }
        //                else //web api sent error response 
        //                {
        //                    //log response status here..
        //                    response.StatusCode = HttpStatusCode.InternalServerError;
        //                    response.Message = "Server error. Please contact administrator.";
        //                }

        //            }
        //        }
        //        else
        //        {
        //            response.StatusCode = HttpStatusCode.BadRequest;
        //            response.Message = "Please provide token..";
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Message = ex.Message;
        //        return response;
        //    }
        //    return response;
        //}
        public Boolean SaveRequestId(string RequestId, int UserId, string TransationId,string datSource, bool IsSuccessStatusCode)
        {

            dbProxy _db = new dbProxy();
            GetStatus insertStatus = new GetStatus();
            try
            {
                List<SetParameters> spinsert = new List<SetParameters>()
                             {
                                new SetParameters{ ParameterName = "@QType", Value = "1" },
                                new SetParameters{ ParameterName = "@UserId", Value=UserId.ToString()},
                                new SetParameters{ ParameterName = "@RequestId", Value=RequestId == null ? "" : RequestId},
                                new SetParameters{ ParameterName = "@RequestStatus", Value=IsSuccessStatusCode.ToString() == null ? "" :IsSuccessStatusCode.ToString()},
                                new SetParameters{ ParameterName = "@TransationId", Value=TransationId == null ? "" :TransationId},
                                new SetParameters{ ParameterName = "@TransactionType", Value=datSource == null ? "" :datSource}
                              };

                insertStatus = _db.StoreData("InsertSyncRequestId", spinsert);
                if (insertStatus != null && insertStatus.returnStatus == "Success")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}