using HPPlc.Models;
using HPPlc.Models.Assessment;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
//using Microsoft.AspNetCore.Mvc;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
//using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace HPPlc.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/plcassessmt")]
    public class AssessmentModuleController : UmbracoApiController
    {
        //WMSEntities wmsEN = new WMSEntities();
        private readonly IVariationContextAccessor _variationContextAccessor;
        public AssessmentModuleController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }

        [Route("UserLogin")]
        [HttpPost]
        public ResponseVM UserLogin(LoginVM objVM)
        {
            if (objVM.UserName == "plcassessment" && objVM.Password == "plcassessment@#12")
                return new ResponseVM { Status = "Success", Message = TokenManager.GenerateToken(objVM.UserName) };
            else
                return new ResponseVM { Status = "Invalid", Message = "Invalid User." };

            //var objlst = wmsEN.Usp_Login(objVM.UserName, UtilityVM.Encryptdata(objVM.Passward), "").ToList<Usp_Login_Result>().FirstOrDefault();
            //if (objlst.Status == -1)
            //    return new ResponseVM { Status = "Invalid", Message = "Invalid User." };
            ////if (objlst.Status == 2)
            ////    return new ResponseVM { Status = "Invalid", Message = "Already Logged In." };
            //if (objlst.Status == 0)
            //    return new ResponseVM { Status = "Inactive", Message = "User Inactive." };
            //else
            //    return new ResponseVM { Status = "Success", Message = TokenManager.GenerateToken(objVM.UserName) };
        }

        [Route("Validate")]
        [HttpGet]
        public ResponseVM Validate(string token, string username = "plcassessment")
        {
            //int UserId = new UserRepository().GetUser(username);
            //if (UserId == 0) return new ResponseVM { Status = "Invalid", Message = "Invalid User." };
            string tokenUsername = TokenManager.ValidateToken(token);
            if (username.Equals(tokenUsername))
            {
                return new ResponseVM
                {
                    Status = "Success",
                    Message = "OK",
                };
            }
            return new ResponseVM { Status = "Invalid", Message = "Invalid Token." };
        }

        [Route("getworksheets")]
        [HttpPost]
        public HttpResponseMessage GetWorksheets(AssessmtFilterParameter assessmtFilterParameter)
        {
            HttpResponseMessage worksheetResponse = new HttpResponseMessage();
            Response response = new Response();

            //string token = Request.Headers.GetValues("token").FirstOrDefault().ToString();

            //if (!String.IsNullOrWhiteSpace(token))
            //{
            ResponseVM responseVM = new ResponseVM();
            //responseVM = Validate(token);

            //if (responseVM.Status == "Success")
            //{
            if (!String.IsNullOrWhiteSpace(assessmtFilterParameter.CultureName))
            {
                _variationContextAccessor.VariationContext = new VariationContext(assessmtFilterParameter.CultureName);
                List<WorksheetList> worksheetAllforAssessment = new List<WorksheetList>();
                var worksheetsAgeListing = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.Children?
                                            .Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise")?
                                            .OfType<WorksheetListingAgeWise>().ToList();

                if (worksheetsAgeListing != null && worksheetsAgeListing.Any())
                {
                    foreach (var age in worksheetsAgeListing)
                    {
                        if (!String.IsNullOrWhiteSpace(age?.AgeGroup.Name))
                        {
                            var subjects = age?.Children.Where(k => k.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
                                             .Where(x => x.IsActive == true).ToList();

                            if (subjects != null && subjects.Any())
                            {
                                WorksheetList worksheetList = new WorksheetList();
                                //List<WorksheetRoot> worksheetTemp = new List<WorksheetRoot>();
                                foreach (var subject in subjects)
                                {
                                    try
                                    {
                                        if (!String.IsNullOrWhiteSpace(subject?.CategoryName?.Name))
                                        {
                                            List<WorksheetRoot> worksheetTemp;
                                            if (!String.IsNullOrWhiteSpace(assessmtFilterParameter.Subject) && !String.IsNullOrWhiteSpace(assessmtFilterParameter.AgeGroup))
                                            {
                                                worksheetTemp = subject?.Children.OfType<WorksheetRoot>()?
                                                            .Where(x => x.IsActive == true && x.SelectSubject?.Name == subject?.CategoryName?.Name)?
                                                            .OfType<WorksheetRoot>()?.Where(x => x.IsQuizWorksheet == true && x.SelectSubject.Name == assessmtFilterParameter.Subject && x.AgeTitle.Name == assessmtFilterParameter.AgeGroup).ToList();
                                            }
                                            else if (!String.IsNullOrWhiteSpace(assessmtFilterParameter.Subject))
                                            {
                                                worksheetTemp = subject?.Children.OfType<WorksheetRoot>()?
                                                            .Where(x => x.IsActive == true && x.SelectSubject?.Name == subject?.CategoryName?.Name)?
                                                            .OfType<WorksheetRoot>()?.Where(x => x.IsQuizWorksheet == true && x.SelectSubject.Name == assessmtFilterParameter.Subject).ToList();
                                            }
                                            else if (!String.IsNullOrWhiteSpace(assessmtFilterParameter.AgeGroup))
                                            {
                                                worksheetTemp = subject?.Children.OfType<WorksheetRoot>()?
                                                            .Where(x => x.IsActive == true && x.SelectSubject?.Name == subject?.CategoryName?.Name)?
                                                            .OfType<WorksheetRoot>()?.Where(x => x.IsQuizWorksheet == true && x.AgeTitle.Name == assessmtFilterParameter.AgeGroup).ToList();
                                            }
                                            else
                                            {
                                                worksheetTemp = subject?.Children.OfType<WorksheetRoot>()?
                                                               .Where(x => x.IsActive == true && x.SelectSubject?.Name == subject?.CategoryName?.Name)?
                                                               .OfType<WorksheetRoot>().Where(x => x.IsQuizWorksheet == true).ToList();
                                            }

                                            if (worksheetTemp != null && worksheetTemp.Count > 0)
                                            {
                                                foreach (var items in worksheetTemp)
                                                {
                                                    if (items != null)
                                                    {
                                                        worksheetList.WorksheetId = items.Id;
                                                        worksheetList.Title = items.Title;
                                                        if (items.AgeTitle != null)
                                                            worksheetList.AgeGroup = items.AgeTitle.Name;

                                                        if (items.SelectSubject != null)
                                                            worksheetList.Subject = items.SelectSubject.Name;

                                                        if (items.SelectWeek != null)
                                                            worksheetList.Week = items.SelectWeek.Name;

                                                        worksheetList.Description = items.Description;
                                                        worksheetList.has_quiz = items.IsQuizWorksheet;

                                                        if (items.UploadThumbnail != null)
                                                            worksheetList.DesktopThumb = items.UploadThumbnail.Url();

                                                        if (items.UploadMobileThumbnail != null)
                                                            worksheetList.MobileThumb = items.UploadMobileThumbnail.Url();

                                                        worksheetAllforAssessment.Add(worksheetList);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }

                if (worksheetAllforAssessment != null && worksheetAllforAssessment.Any())
                {
                    response.StatusCode = 1;
                    response.StatusMessage = "Done";
                    response.Result = worksheetAllforAssessment;

                    worksheetResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
                }
                else
                {
                    response.StatusCode = 0;
                    response.StatusMessage = "Workshet not found.";
                    response.Result = null;

                    worksheetResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
                }
            }
            //}
            //else
            //{
            //    response.StatusCode = 0;
            //    response.StatusMessage = responseVM.Status;
            //    response.Result = responseVM.Message;

            //    worksheetResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
            //}
            //}

            return worksheetResponse;
        }


        [Route("getquizauth")]
        [HttpPost]
        public HttpResponseMessage GetQuizAuth(string uniquekey)
        {
            HttpResponseMessage authResponse = new HttpResponseMessage();
            Response response = new Response();
            QuizAuthResponse quizAuthResponse = new QuizAuthResponse();

            if (!String.IsNullOrWhiteSpace(uniquekey))
            {
                dbProxy _db = new dbProxy();
                QuizValidate quizValidate = new QuizValidate();
                List<SetParameters> sp = new List<SetParameters>()
                      {
                        new SetParameters { ParameterName = "@UniqueId", Value = uniquekey}
                      };

                quizValidate = _db.GetData("USP_AutheticationByQuizPlatform_Quiz", quizValidate, sp);

                if (quizValidate != null && quizValidate.status == 1)
                {
                    //ReturnMessage returnMessage = new ReturnMessage();
                    //returnMessage = GetAutheticateByQuizPlatform("0", uniquekey, quizValidate.WeekId.ToString());

                    quizAuthResponse.UserId = quizValidate.UserId;
                    quizAuthResponse.Culture = "en-US";
                    quizAuthResponse.QuizType = quizValidate.QuizType;
                    quizAuthResponse.WorksheetId = quizValidate.WorksheetId;
                    quizAuthResponse.QuizForDemo = quizValidate.QuizForDemo;

                    response.StatusCode = 1;
                    response.StatusMessage = "Done";
                    response.Result = quizAuthResponse;
                }
                else
                {
                    response.StatusCode = 0;
                    response.StatusMessage = "Wrong Key";
                    response.Result = null;
                }

                authResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
            }
            else
            {
                response.StatusCode = 0;
                response.StatusMessage = "Key can not be empty";
                response.Result = null;

                authResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
            }

            return authResponse;
        }

        [Route("authuserid")]
        [HttpPost]
        public HttpResponseMessage AuthUserId(string uniquekey, string userid, string WeekId)
        {
            HttpResponseMessage authResponse = new HttpResponseMessage();
            Response response = new Response();

            if (!String.IsNullOrWhiteSpace(userid) && !String.IsNullOrWhiteSpace(uniquekey))
            {
                ReturnMessage returnMessage = new ReturnMessage();
                returnMessage = GetAutheticateByQuizPlatform(userid, uniquekey, WeekId);

                if (returnMessage != null && returnMessage.status == "1")
                {
                    response.StatusCode = 1;
                    response.StatusMessage = "Done";
                    response.Result = null;
                }
                else
                {
                    response.StatusCode = 0;
                    response.StatusMessage = "Wrong UserId";
                    response.Result = null;
                }

                authResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
            }
            else
            {
                response.StatusCode = 0;
                response.StatusMessage = "UserId can not be empty";
                response.Result = null;

                authResponse = Request.CreateResponse<Response>(HttpStatusCode.OK, response);
            }

            return authResponse;
        }

        public ReturnMessage GetAutheticateByQuizPlatform(string UserId, string UniqueCode, string WeekId)
        {
            dbProxy _db = new dbProxy();
            ReturnMessage returnMessage = new ReturnMessage();

            List<SetParameters> sp = new List<SetParameters>()
                      {
                        new SetParameters { ParameterName = "@UserId", Value = UserId},
                        new SetParameters { ParameterName = "@WeekId", Value = WeekId},
                        new SetParameters { ParameterName = "@UniqueCode", Value = UniqueCode}
                      };

            returnMessage = _db.GetData("USP_AutheticationByQuizPlatform_Quiz", returnMessage, sp);

            return returnMessage;

        }
    }
}