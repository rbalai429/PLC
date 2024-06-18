using HPPlc.Models.BulkUpload;
using HPPlc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;
using Umbraco.Core.Models.PublishedContent;

namespace HPPlc.Controllers
{
    public class CMSBulkUploadController : SurfaceController
    {
        private readonly ApplicationContext _context;
        private readonly IVariationContextAccessor _variationContextAccessor;
        // GET: CMSBulkUpload
        public CMSBulkUploadController()
        {
        }

        public CMSBulkUploadController(ApplicationContext context)
        {
            _context = context;
        }

        public CMSBulkUploadController(IVariationContextAccessor variationContextAccessor)
        {
            _variationContextAccessor = variationContextAccessor;
        }
        public string querycheck()
        {
            
                    var classRoot_worksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey("en-us"))?.FirstOrDefault()?.DescendantsOrSelf()?
                        .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                        .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>();
           
                    var classRoot_lesson = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey("en-us"))?.FirstOrDefault()?.DescendantsOrSelf()?
                               .Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault().DescendantsOrSelf()?
                               .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>();


            return "done";
        }

        public string BulkUpload_WS(string language, string planType, string agegroupe, DataTable dt)
        {
            
            string returnVal = "";
            try
            {
                if (planType.ToLower() == "1")// Lesson
                {
                    returnVal =  Lessonplansave(language, agegroupe, dt);
                }
                else if (planType.ToLower() == "2") // worksheet
                {
                    returnVal = Worksheetplansave(language, agegroupe, dt);
                }

                else if (planType.ToLower() == "3") //teachers
                {
                    returnVal = teachersplansave(language, agegroupe, dt);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "BulkUpload_WS");
            }

            return returnVal;
        }
        //public string Teachersplansave_old(string parentnodeid, string doctypeName, string language, DataTable dt)
        //{
        //    bool excel_ok = true;

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        if (row["SheetName"].ToString().Trim() == "")
        //        {
        //            excel_ok = false;
        //        }
        //    }

        //    var data_0 = Umbraco.Media(29020);
        //    var data_1 = Umbraco.Media(29021);
        //    var data_2 = Umbraco.Media(5828);

        //    var qw11 = data_1.ContentType.Alias;
        //    //var aaa = Umbraco.MediaAtRoot().Where(m=>m.Path =)
        //    var aaa = Umbraco.MediaAtRoot();
        //    var data1 = Umbraco.Content(9867);
        //    var home = Umbraco.Content(1067);
        //    // Create a new child item of type 'Product'

        //    var datadd = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
        //                            .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
        //                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>()?.Where(c => c.AgeGroup.Name == "4-5")?.FirstOrDefault()?.DescendantsOrSelf()?
        //                            .Where(x => x.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>();
        //    //.Where(x => x.IsActive == true && Umbraco?.Content(x?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == 16929).FirstOrDefault();

        //    var sheetproduct = Services.ContentService.Create("Week 6 (3-4 Years) by KP", 9867, "worksheetRoot"); // lessonplan
        //    var sheetproduct_ws = Services.ContentService.Create("Week 6 (3-4 Years) by KP", 9867, "structureProgramItems"); // Worksheet
        //    var sheetproduct_teacher = Services.ContentService.Create("Week 6 (3-4 Years) by KP", 9867, "teacherProgramItems"); // teacherProgramItems

        //    //var ss = Services.MediaTypeService.CreateContainer()

        //    //var mid = Umbraco.
        //    //var ids= ((Umbraco.Core.Models.PublishedContent.PublishedContentWrapped)(new System.Linq.SystemCore_EnumerableDebugView<Umbraco.Core.Models.PublishedContent.IPublishedContent>(((Umbraco.Core.Models.PublishedContent.PublishedContentWrapped)(new System.Linq.SystemCore_EnumerableDebugView<Umbraco.Core.Models.PublishedContent.IPublishedContent>(((Umbraco.Core.Models.PublishedContent.PublishedContentWrapped)(new System.Linq.SystemCore_EnumerableDebugView<Umbraco.Core.Models.PublishedContent.IPublishedContent>(((Umbraco.Core.Models.PublishedContent.PublishedContentWrapped)home).Children).Items[8])).Children).Items[6])).Children).Items[0])).Id

        //    //var aa = (((Umbraco.Core.Models.PublishedContent.PublishedContentWrapped)home)).Children.IndexOf(data1);


        //    sheetproduct.SetValue("title", "Hello India title", "en-US");
        //    sheetproduct.SetValue("subTitle", "Hello India subTitle", "en-US");
        //    sheetproduct.SetValue("shareContent", "Hello India shareContent", "en-US");
        //    sheetproduct.SetValue("isGuestUserSheet", false, "en-US");
        //    sheetproduct.SetValue("cBSEContentIncluded", false, "en-US");
        //    sheetproduct.SetValue("isQuizWorksheet", false, "en-US");
        //    sheetproduct.SetValue("isActive", true, "en-US");
        //    sheetproduct.SetValue("isEnableForDetailsPage", false, "en-US");
        //    sheetproduct.SetValue("sharingText", "Hello India sharingText");
        //    sheetproduct.SetValue("minAge", "3", "en-US");
        //    sheetproduct.SetValue("maxAge", "4", "en-US");
        //    sheetproduct.SetValue("description", "Encourage free imagination by drawing favorite things - test by page", "en-US");
        //    sheetproduct.SetValue("descriptionPageContent", "Description Page Content - test by page", "en-US");

        //    sheetproduct.SetValue("isSubscriptionWiseDocument", false, "en-US");
        //    sheetproduct.SetValue("uploadThumbnail", 5828, "en-US");
        //    sheetproduct.SetValue("uploadMobileThumbnail", 13577, "en-US");
        //    sheetproduct.SetValue("whatsAppBanner", 13472, "en-US");
        //    sheetproduct.SetValue("nextGenImage", 6202, "en-US");
        //    sheetproduct.SetValue("mobileNextGenImage", 2828, "en-US");
        //    //content.SetValue("uploadPDF", "/media/ozbkxsly/brain-games-4.pdf", "en-US");

        //    sheetproduct.SetValue("uploadPDF", "/media/umiblofp/nursery-art-and-craft-imaginary-drawing-worksheet.pdf", "en-US");
        //    sheetproduct.SetValue("uploadPreviewPDF", "/media/ozbkxsly/brain-games-4.pdf", "en-US");
        //    sheetproduct.SetValue("subscription", "", "en-US");

        //    //// Save and publish the child item
        //    sheetproduct.SetCultureName("Week 6 (3-4 Years) by KP", "en-us");


        //    //var val = Services.ContentService.SaveAndPublish(sheetproduct);
        //    //var val1 = Services.ContentService.Save(sheetproduct);

        //    return "";
        //}

        public string Lessonplansave(string language, string agegroupe, DataTable dt)
        {
            string message = "ok";
            try
            {
                if (!String.IsNullOrEmpty(language) && !String.IsNullOrEmpty(agegroupe) && dt != null)
                {
                    HomeController home = new HomeController();

                    string doctypeName = "worksheetRoot";
                    string planName = "lesson";
                    bool excel_ok = true;
                    int row_num = 0;
                    int paretnid = 0;
                    int ageGroupNodeid = home.GetAgeGroup_lang(language).Where(a => a.ItemValue == agegroupe).FirstOrDefault().Id;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row_num = row_num + 1;
                            if (

                                (String.IsNullOrEmpty(row["SubjectID"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["WorksheetTitle"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["Title"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["ShareContent"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["SharingText"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["WeekID"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["TopicID"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["Subscription"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["UploadPDF"].ToString().Trim()))
                                )
                            {
                                excel_ok = false;
                                message = "Invalid data in row number : " + row_num.ToString();
                                break;
                            }

                            if (check_media(row["DesktopImageID"].ToString().Trim(), "image") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid DesktopImageID in row number : " + row_num.ToString();
                                break;
                            }
                            if (check_media(row["MobileImageID"].ToString().Trim(), "image") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid MobileImageID in row number : " + row_num.ToString();
                                break;
                            }
                            if (row["DesktopImageWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["DesktopImageWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["MobileIamgeWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (row["WhatsAppBannerID"].ToString().Trim() != "")
                            {
                                if (check_media(row["WhatsAppBannerID"].ToString().Trim(), "image") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid WhatsAppBannerID in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (check_media(row["UploadPDF"].ToString().Trim(), "pdf") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid UploadPDF in row number : " + row_num.ToString();
                                break;
                            }
                            if (!String.IsNullOrEmpty(row["Subscription"].ToString().Trim()))
                            {
                                string[] subscriptions = row["Subscription"].ToString().Split(',');
                                foreach (var sbs in subscriptions)
                                {
                                    if (sbs == null || sbs.Any(char.IsLetter) == true)
                                    {
                                        excel_ok = false;
                                        message = "Invalid SubscriptionId in row number : " + row_num.ToString();
                                        break;
                                    }
                                }
                            }
                            if (row["UploadPreviewPDF"].ToString().Trim() != "")
                            {
                                if (check_media(row["UploadPreviewPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPreviewPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }


                            if (getparent_nodeid(planName, agegroupe, Convert.ToInt32(row["SubjectID"].ToString().Trim()),language).ToString() == "0")
                            {
                                excel_ok = false;
                                message = "Invalid SubjectID in row number : " + row_num.ToString();
                                break;
                            }

                            var subjectNodeidValidate = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>()?.ToList()?
                                                    .Where(t => t.SubjectValue == Convert.ToInt32(row["SubjectID"].ToString().Trim()))?.FirstOrDefault()?.Id;

                            if (subjectNodeidValidate == null)
                            {
                                excel_ok = false;
                                message = "Invalid SubjectID in row number : " + row_num.ToString();
                                break;
                            }

                            var topicNodeidValidate = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.OfType<Topics>()?.ToList()?
                                                    .Where(t => t.TopicValue == Convert.ToInt32(row["TopicID"].ToString().Trim()))?.FirstOrDefault()?.Id;

                            if (topicNodeidValidate == null)
                            {
                                excel_ok = false;
                                message = "Invalid TopicID in row number : " + row_num.ToString();
                                break;
                            }

                            var weekidValidate = GetWeekMaster(language).Where(a => a.ItemValue == row["WeekID"].ToString())?.FirstOrDefault().Id;

                            if (weekidValidate == null)
                            {
                                excel_ok = false;
                                message = "Invalid WeekID in row number : " + row_num.ToString();
                                break;
                            }
                        }
                    }
                    else
                    {
                        excel_ok = false;
                    }

                    if (excel_ok == true)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {

                            var subjectNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>()?.ToList()?
                                                    .Where(t => t.SubjectValue == Convert.ToInt32(row1["SubjectID"].ToString().Trim()))?.FirstOrDefault()?.Id;


                            var topicNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.OfType<Topics>()?.ToList()?
                                                    .Where(t => t.TopicValue == Convert.ToInt32(row1["TopicID"].ToString().Trim()))?.FirstOrDefault()?.Id;

                            var weekid = GetWeekMaster(language).Where(a => a.ItemValue == row1["WeekID"].ToString())?.FirstOrDefault().Id;

                            paretnid = getparent_nodeid(planName, agegroupe, Convert.ToInt32(row1["SubjectID"].ToString().Trim()),language);

                            var sheetproduct = Services.ContentService.Create(row1["WorksheetTitle"].ToString(), paretnid, doctypeName);


                            sheetproduct.SetCultureName(row1["WorksheetTitle"].ToString(), language);
                            sheetproduct.SetValue("title", row1["Title"].ToString(), language);
                            sheetproduct.SetValue("subTitle", row1["SubTitle"].ToString(), language);
                            sheetproduct.SetValue("shareContent", row1["ShareContent"].ToString(), language);
                            //sheetproduct.SetValue("sharingText", row1["SharingText"].ToString(), language);
                            sheetproduct.SetValue("sharingText", "1");
                            sheetproduct.SetValue("Description", row1["Description"].ToString(), language);
                            sheetproduct.SetValue("isGuestUserSheet", row1["IsGuestUserSheet"].ToString() == "Y" ? true : false, language);
                            sheetproduct.SetValue("cBSEContentIncluded", row1["CBSEContentIncluded"].ToString() == "Y" ? true : false, language);
                            sheetproduct.SetValue("isQuizWorksheet", row1["IsQuizWorksheet"].ToString() == "Y" ? true : false, language);
                            sheetproduct.SetValue("isEnableforDetailsPage", row1["IsEnableforDetailsPage"].ToString() == "Y" ? true : false, language);
                            sheetproduct.SetValue("isActive", true, language);
                            //sheetproduct.SetValue("rankingIndex", row1["RankingIndex"].ToString(), language);
                            sheetproduct.SetValue("descriptionPageContent", row1["DescriptionPageContent"].ToString(), language);

                            sheetproduct.SetValue("ageTitle", getlink(ageGroupNodeid.ToString()), language);
                            sheetproduct.SetValue("selectSubject", getlink(subjectNodeid.ToString()), language);
                            sheetproduct.SetValue("selectWeek", getlink(weekid.ToString()), language);
                            sheetproduct.SetValue("topic", getlink(topicNodeid.ToString()), language);


                            sheetproduct.SetValue("uploadThumbnail", check_media(row1["DesktopImageID"].ToString(), "image"), language);
                            if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("nextGenImage", check_media(row1["DesktopImageWebpID"].ToString(), "webp"), language);
                            }
                            sheetproduct.SetValue("uploadMobileThumbnail", check_media(row1["MobileImageID"].ToString(), "image"), language);
                            if (!String.IsNullOrEmpty(row1["MobileIamgeWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("mobileNextGenImage", check_media(row1["MobileIamgeWebpID"].ToString(), "webp"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["WhatsAppBannerID"].ToString()))
                            {
                                sheetproduct.SetValue("whatsAppBanner", check_media(row1["WhatsAppBannerID"].ToString(), "image"), language);
                            }
                            sheetproduct.SetValue("Subscription", getsubscription(row1["Subscription"].ToString(),language), language);

                            sheetproduct.SetValue("UploadPDF", check_media(row1["UploadPDF"].ToString(), "pdf"), language);
                            if (!String.IsNullOrEmpty(row1["UploadPreviewPDF"].ToString()))
                            {
                                sheetproduct.SetValue("UploadPreviewPDF", check_media(row1["UploadPreviewPDF"].ToString(), "pdf"), language);
                            }
                            var val = Services.ContentService.Save(sheetproduct);
                        }

                    }
                }
                else
                {
                    return "Data is not in correct format!!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "BulkUpload_WS");
                message = "Error Message!! " + ex.StackTrace;
            }
            return message.ToString();
        }
        public string Worksheetplansave(string language, string agegroupe, DataTable dt)
        {
            string message = "ok";
            try
            {
                if (!String.IsNullOrEmpty(language) && !String.IsNullOrEmpty(agegroupe) && dt != null)
                {
                    HomeController home = new HomeController();

                    string doctypeName = "structureProgramItems";
                    string planName = "worksheet";
                    bool excel_ok = true;
                    int row_num = 0;
                    int paretnid = 0;
                    int ageGroupNodeid = home.GetAgeGroup_lang(language).Where(a => a.ItemValue == agegroupe).FirstOrDefault().Id;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row_num = row_num + 1;
                            if (

                                (String.IsNullOrEmpty(row["SubjectID"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["WorksheetTitle"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["Title"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["TopicID"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["UploadPDF"].ToString().Trim()))
                                )
                            {
                                excel_ok = false;
                                message = "Invalid data in row number : " + row_num.ToString();
                                break;
                            }

                            if (check_media(row["DesktopImageID"].ToString().Trim(), "image") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid DesktopImageID in row number : " + row_num.ToString();
                                break;
                            }
                            if (!String.IsNullOrEmpty(row["MobileIamgeWebpID"].ToString().Trim()))
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (!String.IsNullOrEmpty(row["DesktopImageWebpID"].ToString().Trim()))
                            {
                                if (check_media(row["DesktopImageWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (!String.IsNullOrEmpty(row["MobileIamgeWebpID"].ToString().Trim()))
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (check_media(row["UploadPDF"].ToString().Trim(), "pdf") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid UploadPDF in row number : " + row_num.ToString();
                                break;
                            }

                            if (!String.IsNullOrEmpty(row["UploadPreviewPDF"].ToString().Trim()))
                            {
                                if (check_media(row["UploadPreviewPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPreviewPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (getparent_nodeid(planName, agegroupe, Convert.ToInt32(row["SubjectID"].ToString().Trim()),language).ToString() == "0")
                            {
                                excel_ok = false;
                                message = "Invalid SubjectID in row number : " + row_num.ToString();
                                break;
                            }

                            var subjectNodeidValidate = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                   .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                   .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?
                                                   .Children?.OfType<Subjects>()?.ToList().Where(t => t.SubjectValue == Convert.ToInt32(row["SubjectID"].ToString().Trim())).FirstOrDefault().Id;

                            if (subjectNodeidValidate == null)
                            {
                                excel_ok = false;
                                message = "Invalid SubjectID in row number : " + row_num.ToString();
                                break;
                            }

                            var topicNodeidValidate = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?
                                                    .Children?.OfType<Topics>()?.ToList().Where(t => t.TopicValue == Convert.ToInt32(row["TopicID"].ToString().Trim())).FirstOrDefault().Id;

                            if (topicNodeidValidate == null)
                            {
                                excel_ok = false;
                                message = "Invalid TopicID in row number : " + row_num.ToString();
                                break;
                            }
                        }
                    }
                    else
                    {
                        excel_ok = false;
                    }

                    if (excel_ok == true)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {

                            var subjectNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>()?.ToList()?
                                                    .Where(t => t.SubjectValue == Convert.ToInt32(row1["SubjectID"].ToString().Trim())).FirstOrDefault().Id;

                            var topicNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.OfType<Topics>()?.ToList()?
                                                    .Where(t => t.TopicValue == Convert.ToInt32(row1["TopicID"].ToString().Trim())).FirstOrDefault().Id;


                            paretnid = getparent_nodeid(planName, agegroupe, Convert.ToInt32(row1["SubjectID"].ToString().Trim()), language);

                            var sheetproduct = Services.ContentService.Create(row1["WorksheetTitle"].ToString(), paretnid, doctypeName);

                            sheetproduct.SetCultureName(row1["WorksheetTitle"].ToString(), language);
                            sheetproduct.SetValue("umbracoUrlAlias", row1["umbracoUrlAlias"].ToString(), language);
                            sheetproduct.SetValue("title", row1["Title"].ToString(), language);
                            sheetproduct.SetValue("subTitle", row1["SubTitle"].ToString(), language);
                            sheetproduct.SetValue("Description", row1["Description"].ToString(), language);
                            sheetproduct.SetValue("isGuestUserSheet", row1["IsGuestUserSheet"].ToString() == "Y" ? true : false, language);
                            sheetproduct.SetValue("IsEnableForDetailsPage", row1["IsEnableForDetailsPage"].ToString() == "Y" ? true : false, language);
                            sheetproduct.SetValue("isPaid", row1["Paid"].ToString() == "Y" ? true : false, language);
                            sheetproduct.SetValue("isActive", true, language);
                            sheetproduct.SetValue("rankingIndex", row1["RankingIndex"].ToString(), language);

                            sheetproduct.SetValue("selectAgeGroup", getlink(ageGroupNodeid.ToString()), language);
                            sheetproduct.SetValue("SelectSubject", getlink(subjectNodeid.ToString()), language);
                            sheetproduct.SetValue("selectTopic", getlink(topicNodeid.ToString()), language);


                            sheetproduct.SetValue("DesktopImage", check_media(row1["DesktopImageID"].ToString(), "image"), language);
                            if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("DesktopNextGenImage", check_media(row1["DesktopImageWebpID"].ToString(), "webp"), language);
                            }
                            sheetproduct.SetValue("mobileImage", check_media(row1["MobileImageID"].ToString(), "image"), language);

                            if (!String.IsNullOrEmpty(row1["MobileIamgeWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("MobileNextGenImage", check_media(row1["MobileIamgeWebpID"].ToString(), "webp"), language);
                            }

                            sheetproduct.SetValue("UploadPDF", check_media(row1["UploadPDF"].ToString(), "pdf"), language);
                            if (!String.IsNullOrEmpty(row1["UploadPreviewPDF"].ToString()))
                            {
                                sheetproduct.SetValue("UploadPreviewPDF", check_media(row1["UploadPreviewPDF"].ToString(), "pdf"), language);
                            }
                            var val = Services.ContentService.Save(sheetproduct);
                        }


                    }
                }
                else
                {
                    message = "Invalid Data!!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "Worksheetplansave");
                message = "Error Message!! " + ex.StackTrace;
            }
            return message.ToString();
        }

        public string teachersplansave(string language, string agegroupe, DataTable dt)
        {
            HomeController home = new HomeController();
            string message = "ok";
            try
            {
                if (!String.IsNullOrEmpty(language) && !String.IsNullOrEmpty(agegroupe) && dt != null)
                {
                    string doctypeName = "teacherProgramItems";
                    string planName = "teachers";
                    bool excel_ok = true;
                    int row_num = 0;
                    int paretnid = 0;
                    //int ageGroupNodeid = home.GetAgeGroup().Where(a => a.ItemValue == agegroupe).FirstOrDefault().Id;
                    //int ageGroupNodeid = GetdaysMaster().Where(a => a.ItemValue == agegroupe).FirstOrDefault().Id;
                    int? No_of_days;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row_num = row_num + 1;

                            if ((String.IsNullOrEmpty(row["NoofDays"].ToString().Trim()))
                               || (String.IsNullOrEmpty(row["WorksheetTitle"].ToString().Trim()))
                               || (String.IsNullOrEmpty(row["Title"].ToString().Trim()))
                               || (String.IsNullOrEmpty(row["UploadPDF"].ToString().Trim()))
                                    )

                            {
                                excel_ok = false;
                                message = "Invalid WorksheetTitle in row number : " + row_num.ToString();
                                break;
                            }



                            if (check_media(row["DesktopImageID"].ToString().Trim(), "image") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid DesktopImageID in row number : " + row_num.ToString();
                                break;
                            }
                            if (check_media(row["MobileImageID"].ToString().Trim(), "image") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid MobileImageID in row number : " + row_num.ToString();
                                break;
                            }
                            if (row["DesktopImageWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["DesktopImageWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["MobileIamgeWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (check_media(row["UploadPDF"].ToString().Trim(), "pdf") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid UploadPDF in row number : " + row_num.ToString();
                                break;
                            }
                            if (row["UploadPreviewPDF"].ToString().Trim() != "")
                            {
                                if (check_media(row["UploadPreviewPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPreviewPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (getparent_nodeid(planName, agegroupe, 0,language) == 0)
                            {
                                excel_ok = false;
                                message = "Invalid SubjectID in row number : " + row_num.ToString();
                                break;
                            }
                        }
                    }
                    else
                    {
                        excel_ok = false;
                    }

                    if (excel_ok == true)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {

                            //var subjectNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                            //                        .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                            //                        .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>()?.ToList().Where(t => t.SubjectValue == Convert.ToInt32(row1["SubjectID"].ToString().Trim())).FirstOrDefault().Id;

                            //var topicNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home")?.FirstOrDefault()?.DescendantsOrSelf()?
                            //                        .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                            //                        .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.OfType<Topics>()?.ToList().Where(t => t.TopicValue == Convert.ToInt32(row1["Topic"].ToString().Trim())).FirstOrDefault().Id;

                            No_of_days = GetdaysMaster(language)?.Where(a => a.ItemValue == row1["NoofDays"].ToString())?.FirstOrDefault()?.Id;

                            if (No_of_days > 0)
                            {
                                paretnid = getparent_nodeid(planName, agegroupe, 0, language);



                                var sheetproduct = Services.ContentService.Create(row1["WorksheetTitle"].ToString(), paretnid, doctypeName);

                                sheetproduct.SetCultureName(row1["WorksheetTitle"].ToString(), language);

                                sheetproduct.SetValue("umbracoUrlAlias", row1["umbracoUrlAlias"].ToString(), language);
                                sheetproduct.SetValue("title", row1["Title"].ToString(), language);
                                sheetproduct.SetValue("subTitle", row1["SubTitle"].ToString(), language);
                                sheetproduct.SetValue("Description", row1["Description"].ToString(), language);
                                sheetproduct.SetValue("isGuestUserSheet", row1["IsGuestUserSheet"].ToString() == "Y" ? true : false, language);
                                sheetproduct.SetValue("IsEnableForDetailsPage", row1["IsEnableForDetailsPage"].ToString() == "Y" ? true : false, language);
                                sheetproduct.SetValue("isPaid", row1["Paid"].ToString() == "Y" ? true : false, language);
                                sheetproduct.SetValue("isActive", true, language);
                                sheetproduct.SetValue("rankingIndex", row1["RankingIndex"].ToString(), language);

                                sheetproduct.SetValue("noOfDays", getlink(No_of_days.ToString()), language);

                                sheetproduct.SetValue("DesktopImage", check_media(row1["DesktopImageID"].ToString(), "image"), language);
                                if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                                {
                                    sheetproduct.SetValue("DesktopNextGenImage", check_media(row1["DesktopImageWebpID"].ToString(), "webp"), language);
                                }
                                sheetproduct.SetValue("mobileImage", check_media(row1["MobileImageID"].ToString(), "image"), language);
                                if (!String.IsNullOrEmpty(row1["MobileIamgeWebpID"].ToString()))
                                {
                                    sheetproduct.SetValue("MobileNextGenImage", check_media(row1["MobileIamgeWebpID"].ToString(), "webp"), language);
                                }

                                sheetproduct.SetValue("UploadPDF", check_media(row1["UploadPDF"].ToString(), "pdf"), language);
                                if (!String.IsNullOrEmpty(row1["UploadPreviewPDF"].ToString()))
                                {
                                    sheetproduct.SetValue("UploadPreviewPDF", check_media(row1["UploadPreviewPDF"].ToString(), "pdf"), language);
                                }

                                var val = Services.ContentService.Save(sheetproduct);
                            }
                            else
                            {
                                message = "Invalid Data!! - No of days is invalid";
                            }
                        }

                    }
                }
                else
                {
                    message = "Invalid Data!!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "BulkUpload_WS");
                message = "Error Message!! " + ex.StackTrace;
            }
            return message.ToString();
        }

        public string check_media(string id, string type)
        {
            string return_value = "0";

            var media_data = Umbraco.Media(id);

            if (media_data == null)
            {
                return_value = "0";
                return return_value;
            }
            var fExtension = media_data.Value("umbracoExtension").ToString();
            if (media_data != null)
            {
                if (type == "pdf")
                {
                    if (fExtension == "pdf")
                        return_value = media_data.Url().ToString();
                    else
                        return_value = "0";
                }
                else if (type == "webp")
                {
                    if (fExtension == "webp")
                        return_value = id.ToString();
                    else
                        return_value = "0";
                }
                else if (type == "image")
                {
                    if (!String.IsNullOrEmpty(fExtension) && (fExtension == "jpeg" || fExtension == "jpg" || fExtension == "png"))
                        return_value = id.ToString();
                    else
                        return_value = "0";
                }
                else
                {
                    return_value = id.ToString();
                }
            }
            else
            {
                return_value = "0";
            }

            return return_value;
        }

        public int getparent_nodeid(string ws_type, string agegroupe, int subject_id,string language)
        {
            int return_value = 0;
            try
            {
                if (ws_type == "worksheet")
                {

                    var classRoot_worksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>()?.Where(c => c.AgeGroup.Name == agegroupe)?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
                                            .Where(s => Umbraco?.Content(s?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == subject_id).FirstOrDefault();

                    return_value = Convert.ToInt32(classRoot_worksheet.Id);
                }

                if (ws_type == "lesson")
                {
                    var classRoot_lesson = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                       .Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault().DescendantsOrSelf()?
                                       .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>()?.Where(c => c.AgeGroup.Name == agegroupe)?.FirstOrDefault()?.DescendantsOrSelf()?
                                       .Where(x => x.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
                                       .Where(s => Umbraco?.Content(s?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == subject_id).FirstOrDefault();

                    return_value = Convert.ToInt32(classRoot_lesson.Id);
                }

                if (ws_type == "teachers")
                {

                    var classRoot_teachers = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                           .Where(x => x.ContentType.Alias == "teacherRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                           .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>()?.Where(c => c.AgeGroup.Name == agegroupe)?.FirstOrDefault();//?
                                                                                                                                                                                                                            //.Where(x => x.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>();//?
                                                                                                                                                                                                                            //.Where(s => Umbraco?.Content(s?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == 1).FirstOrDefault();
                    return_value = Convert.ToInt32(classRoot_teachers.Id);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "getparent_nodeid");
            }
            return return_value;
        }

        public string getlink(string id)
        {
            string return_value = "";
            var contentPage = Umbraco.Content(id);

            // Create an Udi of the Content
            var contentPageUdi = Udi.Create(Constants.UdiEntityType.Document, contentPage.Key);
            var externalLink = new List<Link>
            {
				// External Link
				new Link
                {
                    Target = "_self",
                    Name = contentPage.Name,
                    Url = contentPage.Url(),
                    Type = LinkType.Content,
                    Udi = contentPageUdi
                },

            };

            // Serialize the list with links to JSON
            var links = JsonConvert.SerializeObject(externalLink);
            return_value = links;
            return return_value;
        }


        public string getsubscription(string subsLinks,string language)
        {
            string return_value = string.Empty;
            //SubscriptionBinding
            //subsLinks = "1,2,3";
            if (!String.IsNullOrWhiteSpace(subsLinks))
            {
                List<string> subscriptionId = subsLinks.Split(',').ToList();

                if (subscriptionId != null)
                {
                    var subscriptionLink = new List<Link>();
                    foreach (var slinks in subscriptionId)
                    {
                        if (slinks != null)
                        {
                            // Get the content you want to assign to the footer links property 
                            var subscriptionDtls = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                    .Where(x => x.ContentType.Alias == "subscriptionList")?.FirstOrDefault()?.DescendantsOrSelf()?.OfType<Subscriptions>()?
                                    .Where(x => x.IsActive == true && x.Ranking == slinks)?.FirstOrDefault();

                            // Create an Udi of the Content
                            var subscriptionDtlsUdi = Udi.Create(Constants.UdiEntityType.Document, subscriptionDtls.Key);

                            subscriptionLink.Add(new Link { Target = "_self", Name = subscriptionDtls.Name, Url = subscriptionDtls.Url(), Type = LinkType.Content, Udi = subscriptionDtlsUdi });
                        }
                    }

                    // Serialize the list with links to JSON
                    var linksubs = JsonConvert.SerializeObject(subscriptionLink);
                    // Set the value of the property with alias 'footerLinks'. 
                    //content.SetValue("subscription", linksubs, "en-US");
                    return_value = linksubs.ToString();
                }
            }
            return return_value;
        }

        public object getSheetDetails(string ws_type,string agegroupe)
        {
            var content = Services.ContentService.GetById(31374);

            ws_type = "teachers";
            agegroupe = "3-4";

            string languagekey = "";
            string languageName = "";
            ILocalizationService ls = Services.LocalizationService;
            IEnumerable<ILanguage> languages = ls.GetAllLanguages();
            List<clsLessionPlan> objclsLessionPlan = new List<clsLessionPlan>();
            List<clsworksheetPlan> objclsworksheetPlan = new List<clsworksheetPlan>();
            List<clsTeacherPlan> objclsTeacherPlan = new List<clsTeacherPlan>();
            var sheetproduct = Services.ContentService.GetById(Convert.ToInt32("7254"));
            Responce responce = new Responce();
            // Iterate over the collection
            string node_calss;
            string node_subject;
            string node_ws;

            try
            {
                foreach (ILanguage language in languages)
                {
                    // Get the .NET culture info
                    CultureInfo cultureInfo = language.CultureInfo;
                    //languagekey = language.IsoCode.ToString();
                    languagekey = cultureInfo.Name.ToString();
                    languageName = cultureInfo.DisplayName.ToString();

                    if (ws_type.ToLower() == "worksheet")
                    {
                        // var root = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey.ToLower()))?.FirstOrDefault();

                        _variationContextAccessor.VariationContext = new VariationContext(languagekey.ToLower());

                        var rootOfClass = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey))?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && (x.IsPublished() == true || x.IsPublished() == false))?.OfType<WorksheetListingAgeWise>()?
                                            .Where(c => c.AgeGroup.Name == agegroupe)
                                            .FirstOrDefault()?.Children();

                            if (rootOfClass != null)
                            {
                                node_calss = agegroupe;
                                foreach (var subjects in rootOfClass)
                                {
                                    node_subject = subjects?.Name;
                                    var worksheetNode = subjects?.Children().Where(x => x.ContentType.Alias == "structureProgramItems" && x.Cultures.ContainsKey(languagekey))?.OfType<StructureProgramItems>();
                                    foreach (var item in worksheetNode)
                                    {
                                        node_calss = agegroupe;
                                        node_subject = subjects.Name;

                                        clsworksheetPlan objlist = new clsworksheetPlan();
                                        node_ws = item.Name;
                                        var pub = item.IsPublished();
                                        var classId = Umbraco.Content(item.SelectAgeGroup.FirstOrDefault()?.Udi)?.DescendantsOrSelf().OfType<NameListItem>().FirstOrDefault()?.ItemValue.ToString();
                                        var SubjectValue = Umbraco.Content(item.SelectSubject.FirstOrDefault()?.Udi)?.DescendantsOrSelf().OfType<Subjects>().FirstOrDefault()?.SubjectValue;
                                        var topicValue = Umbraco.Content(item.SelectTopic.FirstOrDefault()?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue;
                                        var isGuestUserSheet = item.IsGuestUserSheet.ToString();
                                        var IsEnableForDetailsPage = item.IsEnableForDetailsPage.ToString();
                                        var IsPaid = item.IsPaid.ToString();
                                        var RankingIndex = item.RankingIndex.ToString();

                                        objlist.NodeID = item.Id.ToString();
                                        objlist.WorksheetTitle = item.Name.ToString();
                                        objlist.umbracoUrlAlias = item.Value("umbracoUrlAlias").ToString();
                                        objlist.Title = item.Value("title").ToString();
                                        objlist.SubTitle = item.Value("subtitle").ToString();
                                        objlist.SubjectID = SubjectValue.ToString();
                                        objlist.SubjectName = node_subject;
                                        //objlist.Description = item3.Value("Description").ToString();
                                        objlist.TopicID = topicValue.ToString();
                                        objlist.AgeGroup = node_calss + "(" + classId + ")";
                                        objlist.IsGuestUserSheet = isGuestUserSheet.ToString().ToLower() == "true" ? "Y" : "N";
                                        objlist.IsEnableForDetailsPage = IsEnableForDetailsPage.ToString().ToLower() == "true" ? "Y" : "N";
                                        objlist.Paid = IsPaid.ToString().ToLower() == "true" ? "Y" : "N";
                                        objlist.languageKey = languagekey;
                                        objlist.languageName = languageName;
                                        objlist.IsPublished = pub == true ? "Y" : "N";
                                        objclsworksheetPlan.Add(objlist);
                                    }
                                }
                            }
                        
                        responce.Result = objclsworksheetPlan;
                    }

                    if (ws_type.ToLower() == "lesson")
                    {

                        _variationContextAccessor.VariationContext = new VariationContext(languagekey.ToLower());

                        var SubjectsForTheClass = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey))?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && (x.IsPublished() == true || x.IsPublished() == false))?.OfType<WorksheetListingAgeWise>()?
                                            .Where(c => c?.AgeGroup?.Name == agegroupe)
                                            .FirstOrDefault().Children;

                        if (SubjectsForTheClass != null)
                        {
                            node_calss = agegroupe;
                            foreach (var subjects in SubjectsForTheClass)
                            {
                                var worksheetNode = subjects?.Children().Where(x => x.ContentType.Alias == "worksheetRoot" && x.Cultures.ContainsKey(languagekey))?.OfType<WorksheetRoot>();

                                foreach (var item3 in worksheetNode)
                                {
                                    clsLessionPlan objlist = new clsLessionPlan();
                                    node_subject = item3?.Name;
                                    node_ws = item3.Name;
                                    var pub = item3.IsPublished();
                                    var classId = Umbraco.Content(item3.AgeTitle?.Udi)?.DescendantsOrSelf().OfType<NameListItem>().FirstOrDefault().ItemValue;
                                    var SubjectValue = Umbraco.Content(item3.SelectSubject?.Udi)?.DescendantsOrSelf().OfType<Subjects>().FirstOrDefault().SubjectValue;
                                    var weekValue = Umbraco.Content(item3.SelectWeek?.Udi)?.DescendantsOrSelf().OfType<NameListItem>().FirstOrDefault().ItemValue;
                                    var topicValue = Umbraco.Content(item3.Topic?.Udi)?.DescendantsOrSelf()?.OfType<Topics>()?.FirstOrDefault()?.TopicValue;

                                    //var uploadThumbnail = item3.UploadThumbnail == null ? 0 : item3.UploadThumbnail.Id;
                                    //var nextGenImage = item3.NextGenImage == null ? 0 : item3.NextGenImage.Id;
                                    //var uploadMobileThumbnail = item3.UploadMobileThumbnail == null ? 0 : item3.UploadMobileThumbnail.Id;
                                    //var mobileNextGenImage = item3.MobileNextGenImage == null ? 0 : item3.MobileNextGenImage.Id;
                                    //var WhatsAppBannerID = item3.WhatsAppBanner == null ? 0 : item3.WhatsAppBanner.Id;

                                    var isGuestUserSheet = item3.IsGuestUserSheet.ToString();
                                    var cBSEContentIncluded = item3.CBsecontentIncluded.ToString();
                                    var isQuizWorksheet = item3.IsQuizWorksheet.ToString();
                                    var isEnableforDetailsPage = item3.IsEnableForDetailsPage.ToString();


                                    objlist.NodeID = item3.Id.ToString();
                                    objlist.WorksheetTitle = item3.Name.ToString();
                                    objlist.Title = item3.Value("title").ToString();
                                    objlist.SubTitle = item3.Value("subtitle").ToString();
                                    objlist.SubjectName = node_subject;
                                    //objlist.AgeGroup = node_calss + "(" + classId + ")";
                                    objlist.AgeGroup = node_calss;
                                    //objlist.DesktopImageID = uploadThumbnail.ToString();
                                    //objlist.DesktopImageWebpID = nextGenImage.ToString();
                                    //objlist.MobileImageID = uploadMobileThumbnail.ToString();
                                    //objlist.MobileIamgeWebpID = mobileNextGenImage.ToString();
                                    objlist.languageKey = languagekey;
                                    objlist.languageName = languageName;
                                    objlist.IsPublished = pub == true ? "Y" : "N";
                                    objclsLessionPlan.Add(objlist);
                                    //foreach (var item3 in item.Children().Where(x => x.ContentType.Alias == "worksheetRoot").OfType<WorksheetRoot>())
                                    //{

                                    //}
                                }
                            }
                            responce.Result = objclsLessionPlan;
                        }
                    }

                    if (ws_type.ToLower() == "teachers")
                    {
                        var classRoot_teachers = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                               .Where(x => x.ContentType.Alias == "teacherRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                               .Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && (x.IsPublished() == true || x.IsPublished() == false))?
                                               .OfType<WorksheetListingAgeWise>()?.Where(c => c.AgeGroup.Name == agegroupe);

                        _variationContextAccessor.VariationContext = new VariationContext(languagekey.ToLower());

                        var SubjectsForTheClass = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(languagekey))?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise" && (x.IsPublished() == true || x.IsPublished() == false))?.OfType<WorksheetListingAgeWise>()?
                                            .Where(c => c?.AgeGroup?.Name == agegroupe)
                                            .FirstOrDefault().Children;
                        
                        foreach (var item in classRoot_teachers)
                        {
                            node_calss = item.Name;
                            foreach (var item2 in item.Children().Where(x => x.ContentType.Alias == "teacherProgramItems").OfType<TeacherProgramItems>())
                            {
                                //var item2 = item;
                                node_subject = item2.Name;
                                clsTeacherPlan objlist = new clsTeacherPlan();
                                node_ws = item2.Name;
                                var pub = item2.IsPublished();

                                var Noofday = Umbraco.Content(item2.NoOfDays?.Udi)?.DescendantsOrSelf().OfType<NameListItem>().FirstOrDefault().ItemValue;
                                var isGuestUserSheet = item2.IsGuestUserSheet.ToString();
                                var IsEnableForDetailsPage = item2.IsEnableForDetailsPage.ToString();
                                var IsPaid = item2.IsPaid.ToString();
                                var RankingIndex = item2.RankingIndex.ToString();


                                objlist.NodeID = item2.Id.ToString();
                                objlist.WorksheetTitle = item2.Name.ToString();
                                objlist.umbracoUrlAlias = item2.Value("umbracoUrlAlias").ToString();
                                objlist.Title = item2.Value("title").ToString();
                                objlist.SubTitle = item2.Value("subtitle").ToString();
                                objlist.RankingIndex = item2.Value("rankingIndex").ToString();

                                objlist.AgeGroup = node_calss;

                                objlist.NoofDays = Noofday.ToString();
                                objlist.IsGuestUserSheet = isGuestUserSheet.ToString().ToLower() == "true" ? "Y" : "N";
                                objlist.IsEnableForDetailsPage = IsEnableForDetailsPage.ToString().ToLower() == "true" ? "Y" : "N";
                                objlist.Paid = IsPaid.ToString().ToLower() == "true" ? "Y" : "N";
                                objlist.languageKey = languagekey;
                                objlist.languageName = languageName;
                                objlist.IsPublished = pub == true ? "Y" : "N";
                                objclsTeacherPlan.Add(objlist);
                                //}
                            }
                        }
                        responce.Result = objclsTeacherPlan;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            var json = JsonConvert.SerializeObject(responce.Result);
            return responce.Result;
            //return json;

        }

        public List<NameListItem> GetdaysMaster(string language)
        {
            List<NameListItem> nameListItems = new List<NameListItem>();
            nameListItems = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()
                            ?.Where(x => x.ContentType.Alias == "daysMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>()?.ToList();
            return nameListItems;
        }
        public List<NameListItem> GetWeekMaster(string language)
        {
            List<NameListItem> nameListItems = new List<NameListItem>();
            nameListItems = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()
                            ?.Where(x => x.ContentType.Alias == "volumeMaster")?.FirstOrDefault()?.Children?.OfType<NameListItem>()?.ToList();
            return nameListItems;
        }

        #region Worksheet Bulk Update

        public string BulkUpDate_WS(string language, string planType, string agegroupe, DataTable dt)
        {

            string returnVal = "";
            try
            {
                if (planType.ToLower() == "1")// Lesson
                {
                    returnVal = Lessonplan_update(language, agegroupe, dt);
                }
                else if (planType.ToLower() == "2") // worksheet
                {
                    returnVal = Worksheetplan_update(language, agegroupe, dt);
                }

                else if (planType.ToLower() == "3") //teachers
                {
                    returnVal = teachersplan_update(language, agegroupe, dt);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "BulkUpload_WS");
            }

            return returnVal;
        }

        public string Lessonplan_update(string language, string agegroupe, DataTable dt)
        {
            string message = "ok";
            try
            {
                if (!String.IsNullOrEmpty(language) && !String.IsNullOrEmpty(agegroupe) && dt != null)
                {
                    HomeController home = new HomeController();
                    bool excel_ok = true;
                    int row_num = 0;
                    int ageGroupNodeid = home.GetAgeGroup_lang(language).Where(a => a.ItemValue == agegroupe).FirstOrDefault().Id;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row_num = row_num + 1;
                            if (
                                (String.IsNullOrEmpty(row["NodeID"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["Title"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["ShareContent"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["SharingText"].ToString().Trim()))
                                 || (String.IsNullOrEmpty(row["WorksheetTitle"].ToString().Trim()))
                                 || (String.IsNullOrEmpty(row["languageKey"].ToString().Trim()))
                                )
                            {
                                excel_ok = false;
                                message = "Invalid data in row number : " + row_num.ToString();
                                break;
                            }

                            if (row["DesktopImageID"].ToString().Trim() != "")
                            {
                                if (check_media(row["DesktopImageID"].ToString().Trim(), "image") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["MobileImageID"].ToString().Trim() != "")
                            {
                                if (check_media(row["MobileImageID"].ToString().Trim(), "image") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileImageID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["DesktopImageWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["DesktopImageWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["MobileIamgeWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (row["WhatsAppBannerID"].ToString().Trim() != "")
                            {
                                if (check_media(row["WhatsAppBannerID"].ToString().Trim(), "image") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid WhatsAppBannerID in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (row["UploadPDF"].ToString().Trim() != "")
                            {
                                if (check_media(row["UploadPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["UploadPreviewPDF"].ToString().Trim() != "")
                            {
                                if (row["UploadPreviewPDF"].ToString().Trim() != "")
                                {
                                    if (check_media(row["UploadPreviewPDF"].ToString().Trim(), "pdf") == "0")
                                    {
                                        excel_ok = false;
                                        message = "Invalid UploadPreviewPDF in row number : " + row_num.ToString();
                                        break;
                                    }
                                }
                            }
                            if (!String.IsNullOrEmpty(row["Subscription"].ToString().Trim()))
                            {
                                string[] subscriptions = row["Subscription"].ToString().Split(',');
                                foreach (var sbs in subscriptions)
                                {
                                    if (sbs == null || sbs.Any(char.IsLetter) == true)
                                    {
                                        excel_ok = false;
                                        message = "Invalid SubscriptionId in row number : " + row_num.ToString();
                                        break;
                                    }
                                }
                            }

                            //if (Check_nodeid(planName, row["NodeID"].ToString().Trim(),language) ==0)
                            //{
                            //    excel_ok = false;
                            //    message = "Invalid SubjectID in row number : " + row_num.ToString();
                            //    break;
                            //}

                            if (!String.IsNullOrEmpty(row["SubjectID"].ToString().Trim()))
                            {
                                var subjectNodeidValidate = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                        .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                        .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>()?.ToList()?
                                                        .Where(t => t.SubjectValue == Convert.ToInt32(row["SubjectID"].ToString().Trim()))?.FirstOrDefault()?.Id;

                                if (subjectNodeidValidate == null)
                                {
                                    excel_ok = false;
                                    message = "Invalid SubjectID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (!String.IsNullOrEmpty(row["TopicID"].ToString().Trim()))
                            {
                                var topicNodeidValidate = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.OfType<Topics>()?.ToList()?
                                                    .Where(t => t.TopicValue == Convert.ToInt32(row["TopicID"].ToString().Trim()))?.FirstOrDefault()?.Id;

                            
                                if (topicNodeidValidate == null)
                                {
                                    excel_ok = false;
                                    message = "Invalid TopicID in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (!String.IsNullOrEmpty(row["WeekID"].ToString().Trim()))
                            {
                                var weekidValidate = GetWeekMaster(language).Where(a => a.ItemValue == row["WeekID"].ToString())?.FirstOrDefault().Id;

                            
                                if (weekidValidate == null)
                                {
                                    excel_ok = false;
                                    message = "Invalid WeekID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        excel_ok = false;
                    }

                    if (excel_ok == true)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {
                            language = row1["languageKey"].ToString();

                            //var subjectNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                            //                        .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                            //                        .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>()?.ToList()?
                            //                        .Where(t => t.SubjectValue == Convert.ToInt32(row1["SubjectID"].ToString().Trim()))?.FirstOrDefault()?.Id;




                            //paretnid = getparent_nodeid(planName, agegroupe, Convert.ToInt32(row1["SubjectID"].ToString().Trim()), language);

                            //var sheetproduct = Services.ContentService.Create(row1["WorksheetTitle"].ToString(), paretnid, doctypeName);

                            var sheetproduct = Services.ContentService.GetById(Convert.ToInt32(row1["NodeID"].ToString()));

                            if (!String.IsNullOrEmpty(row1["Title"].ToString()))
                            {
                                sheetproduct.SetValue("title", row1["Title"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["SubTitle"].ToString()))
                            {
                                sheetproduct.SetValue("subTitle", row1["SubTitle"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["ShareContent"].ToString()))
                            {
                                sheetproduct.SetValue("shareContent", row1["ShareContent"].ToString(), language);
                            }
                            
                            if (!String.IsNullOrEmpty(row1["Description"].ToString()))
                            {
                                sheetproduct.SetValue("Description", row1["Description"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["IsGuestUserSheet"].ToString()))
                            {
                                sheetproduct.SetValue("isGuestUserSheet", row1["IsGuestUserSheet"].ToString() == "Y" ? true : false, language);
                            }
                            if (!String.IsNullOrEmpty(row1["CBSEContentIncluded"].ToString()))
                            {
                                sheetproduct.SetValue("cBSEContentIncluded", row1["CBSEContentIncluded"].ToString() == "Y" ? true : false, language);
                            }
                            if (!String.IsNullOrEmpty(row1["IsQuizWorksheet"].ToString()))
                            {
                                sheetproduct.SetValue("isQuizWorksheet", row1["IsQuizWorksheet"].ToString() == "Y" ? true : false, language);
                            }
                            if (!String.IsNullOrEmpty(row1["IsEnableforDetailsPage"].ToString()))
                            {
                                sheetproduct.SetValue("isEnableforDetailsPage", row1["IsEnableforDetailsPage"].ToString() == "Y" ? true : false, language);
                            }

                            //sheetproduct.SetValue("rankingIndex", row1["RankingIndex"].ToString(), language);
                            if (!String.IsNullOrEmpty(row1["DescriptionPageContent"].ToString()))
                            {
                                sheetproduct.SetValue("descriptionPageContent", row1["DescriptionPageContent"].ToString(), language);
                            }

                            sheetproduct.SetValue("ageTitle", getlink(ageGroupNodeid.ToString()), language);

                            if (!String.IsNullOrEmpty(row1["SubjectID"].ToString()))
                            {
                                var subjectNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                         .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                         .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>()?.ToList()?
                         .Where(t => t.SubjectValue == Convert.ToInt32(row1["SubjectID"].ToString().Trim()))?.FirstOrDefault()?.Id;

                                sheetproduct.SetValue("selectSubject", getlink(subjectNodeid.ToString()), language);
                            }

                            if (!String.IsNullOrEmpty(row1["WeekID"].ToString()))
                            {
                                var weekid = GetWeekMaster(language).Where(a => a.ItemValue == row1["WeekID"].ToString())?.FirstOrDefault().Id;
                                sheetproduct.SetValue("selectWeek", getlink(weekid.ToString()), language);
                            }
                            if (!String.IsNullOrEmpty(row1["TopicID"].ToString()))
                            {
                                var topicNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.OfType<Topics>()?.ToList()?
                                                    .Where(t => t.TopicValue == Convert.ToInt32(row1["TopicID"].ToString().Trim()))?.FirstOrDefault()?.Id;

                                sheetproduct.SetValue("topic", getlink(topicNodeid.ToString()), language);
                            }

                            if (!String.IsNullOrEmpty(row1["DesktopImageID"].ToString()))
                            {
                                sheetproduct.SetValue("uploadThumbnail", check_media(row1["DesktopImageID"].ToString(), "image"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("nextGenImage", check_media(row1["DesktopImageWebpID"].ToString(), "webp"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["MobileImageID"].ToString()))
                            {
                                sheetproduct.SetValue("uploadMobileThumbnail", check_media(row1["MobileImageID"].ToString(), "image"), language);
                            }
                            
                            if (!String.IsNullOrEmpty(row1["MobileIamgeWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("mobileNextGenImage", check_media(row1["MobileIamgeWebpID"].ToString(), "webp"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["WhatsAppBannerID"].ToString()))
                            {
                                sheetproduct.SetValue("whatsAppBanner", check_media(row1["WhatsAppBannerID"].ToString(), "image"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["Subscription"].ToString()))
                            {
                                sheetproduct.SetValue("Subscription", getsubscription(row1["Subscription"].ToString(), language), language);
                            }
                            if (!String.IsNullOrEmpty(row1["UploadPDF"].ToString()))
                            {
                                sheetproduct.SetValue("UploadPDF", check_media(row1["UploadPDF"].ToString(), "pdf"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["UploadPreviewPDF"].ToString()))
                            {
                                sheetproduct.SetValue("UploadPreviewPDF", check_media(row1["UploadPreviewPDF"].ToString(), "pdf"), language);
                            }
                            sheetproduct.SetCultureName(row1["WorksheetTitle"].ToString(), language);

                            var val = Services.ContentService.Save(sheetproduct);
                        }
                    }
                }
                else
                {
                    return "Data is not in correct format!!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "Lessonplan_update");
                message = "Error Message!! " + ex.StackTrace;
            }
            return message.ToString();
        }
        
        public string Worksheetplan_update(string language, string agegroupe, DataTable dt)
        {
            string message = "ok";
            try
            {
                language = "bn";
                if (!String.IsNullOrEmpty(language) && !String.IsNullOrEmpty(agegroupe) && dt != null)
                {
                    HomeController home = new HomeController();

                    bool excel_ok = true;
                    int row_num = 0;
                    int ageGroupNodeid = home.GetAgeGroup_lang(language).Where(a => a.ItemValue == agegroupe).FirstOrDefault().Id;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row_num = row_num + 1;
                            if (

                                   //(String.IsNullOrEmpty(row["SubjectID"].ToString().Trim()))
                                  
                                   //|| (String.IsNullOrEmpty(row["Title"].ToString().Trim()))
                                   //|| (String.IsNullOrEmpty(row["TopicID"].ToString().Trim()))
                                   //|| (String.IsNullOrEmpty(row["UploadPDF"].ToString().Trim()))
                                   (String.IsNullOrEmpty(row["NodeID"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["Title"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["languageKey"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["WorksheetTitle"].ToString().Trim()))
                                )
                            {
                                excel_ok = false;
                                message = "Invalid data in row number : " + row_num.ToString();
                                break;
                            }

                            if (!String.IsNullOrEmpty(row["DesktopImageID"].ToString().Trim()))
                            {
                                if (check_media(row["DesktopImageID"].ToString().Trim(), "image") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageID in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (!String.IsNullOrEmpty(row["MobileIamgeWebpID"].ToString().Trim()))
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (!String.IsNullOrEmpty(row["DesktopImageWebpID"].ToString().Trim()))
                            {
                                if (check_media(row["DesktopImageWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (!String.IsNullOrEmpty(row["MobileIamgeWebpID"].ToString().Trim()))
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (!String.IsNullOrEmpty(row["UploadPDF"].ToString().Trim()))
                            {
                                if (check_media(row["UploadPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (!String.IsNullOrEmpty(row["UploadPreviewPDF"].ToString().Trim()))
                            {
                                if (check_media(row["UploadPreviewPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPreviewPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            //if (getparent_nodeid(planName, agegroupe, Convert.ToInt32(row["SubjectID"].ToString().Trim()), language).ToString() == "0")
                            //{
                            //    excel_ok = false;
                            //    message = "Invalid SubjectID in row number : " + row_num.ToString();
                            //    break;
                            //}

                            //var subjectNodeidValidate = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                            //                       .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                            //                       .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?
                            //                       .Children?.OfType<Subjects>()?.ToList().Where(t => t.SubjectValue == Convert.ToInt32(row["SubjectID"].ToString().Trim())).FirstOrDefault().Id;

                            //if (subjectNodeidValidate == null)
                            //{
                            //    excel_ok = false;
                            //    message = "Invalid SubjectID in row number : " + row_num.ToString();
                            //    break;
                            //}

                            var topicNodeidValidate = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?
                                                    .Children?.OfType<Topics>()?.ToList().Where(t => t.TopicValue == Convert.ToInt32(row["TopicID"].ToString().Trim())).FirstOrDefault().Id;

                            if (!String.IsNullOrEmpty(row["TopicID"].ToString().Trim()))
                            {
                                if (topicNodeidValidate == null)
                                {
                                    excel_ok = false;
                                    message = "Invalid TopicID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        excel_ok = false;
                    }

                    if (excel_ok == true)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {
                            language = row1["languageKey"].ToString();
                            
                            //paretnid = getparent_nodeid(planName, agegroupe, Convert.ToInt32(row1["SubjectID"].ToString().Trim()), language);

                            //var sheetproduct = Services.ContentService.Create(row1["WorksheetTitle"].ToString(), paretnid, doctypeName);
                            
                            var sheetproduct = Services.ContentService.GetById(Convert.ToInt32(row1["NodeID"].ToString()));

                            //sheetproduct.SetCultureName(row1["WorksheetTitle"].ToString(), language);

                            if (!String.IsNullOrEmpty(row1["umbracoUrlAlias"].ToString()))
                            {
                                sheetproduct.SetValue("umbracoUrlAlias", row1["umbracoUrlAlias"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["Title"].ToString()))
                            {
                                sheetproduct.SetValue("title", row1["Title"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["SubTitle"].ToString()))
                            {
                                sheetproduct.SetValue("subTitle", row1["SubTitle"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["Description"].ToString()))
                            {
                                sheetproduct.SetValue("Description", row1["Description"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["IsGuestUserSheet"].ToString()))
                            {
                                sheetproduct.SetValue("isGuestUserSheet", row1["IsGuestUserSheet"].ToString() == "Y" ? true : false, language);
                            }
                            if (!String.IsNullOrEmpty(row1["IsEnableForDetailsPage"].ToString()))
                            {
                                sheetproduct.SetValue("IsEnableForDetailsPage", row1["IsEnableForDetailsPage"].ToString() == "Y" ? true : false, language);
                            }
                            if (!String.IsNullOrEmpty(row1["Paid"].ToString()))
                            {
                                sheetproduct.SetValue("isPaid", row1["Paid"].ToString() == "Y" ? true : false, language);
                            }
                            
                            if (!String.IsNullOrEmpty(row1["RankingIndex"].ToString()))
                            {
                                sheetproduct.SetValue("rankingIndex", row1["RankingIndex"].ToString(), language);
                            }

                            if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("DesktopImage", check_media(row1["DesktopImageID"].ToString(), "image"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("DesktopNextGenImage", check_media(row1["DesktopImageWebpID"].ToString(), "webp"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("mobileImage", check_media(row1["MobileImageID"].ToString(), "image"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["MobileIamgeWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("MobileNextGenImage", check_media(row1["MobileIamgeWebpID"].ToString(), "webp"), language);
                            }

                            if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("UploadPDF", check_media(row1["UploadPDF"].ToString(), "pdf"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["UploadPreviewPDF"].ToString()))
                            {
                                sheetproduct.SetValue("UploadPreviewPDF", check_media(row1["UploadPreviewPDF"].ToString(), "pdf"), language);
                            }
                            sheetproduct.SetValue("selectAgeGroup", getlink(ageGroupNodeid.ToString()), language);

                            if (!String.IsNullOrEmpty(row1["SubjectID"].ToString()))
                            {
                                var subjectNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "subjectsRoot")?.FirstOrDefault()?.Children?.OfType<Subjects>()?.ToList()?
                                                    .Where(t => t.SubjectValue == Convert.ToInt32(row1["SubjectID"].ToString().Trim())).FirstOrDefault().Id;
                                sheetproduct.SetValue("SelectSubject", getlink(subjectNodeid.ToString()), language);

                            }
                            if (!String.IsNullOrEmpty(row1["TopicID"].ToString()))
                            {
                                var topicNodeid = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                                    .Where(x => x.ContentType.Alias == "topicsRoot")?.FirstOrDefault()?.Children?.OfType<Topics>()?.ToList()?
                                                    .Where(t => t.TopicValue == Convert.ToInt32(row1["TopicID"].ToString().Trim())).FirstOrDefault().Id;

                                sheetproduct.SetValue("selectTopic", getlink(topicNodeid.ToString()), language);
                            }

                            sheetproduct.SetCultureName(row1["WorksheetTitle"].ToString(), language);
                            var val = Services.ContentService.Save(sheetproduct);
                        }
                    }
                }
                else
                {
                    message = "Invalid Data!!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "Worksheetplansave");
                message = "Error Message!! " + ex.StackTrace;
            }
            return message.ToString();
        }

        public string teachersplan_update(string language, string agegroupe, DataTable dt)
        {
            HomeController home = new HomeController();
            string message = "ok";
            try
            {
                if (!String.IsNullOrEmpty(language) && !String.IsNullOrEmpty(agegroupe) && dt != null)
                {
                    bool excel_ok = true;
                    int row_num = 0;
                    int? No_of_days;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row_num = row_num + 1;

                            if ((String.IsNullOrEmpty(row["NodeID"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["Title"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["languageKey"].ToString().Trim()))
                                || (String.IsNullOrEmpty(row["WorksheetTitle"].ToString().Trim()))
                                )
                            {
                                excel_ok = false;
                                message = "Invalid data in row number : " + row_num.ToString();
                                break;
                            }

                            if (row["DesktopImageID"].ToString().Trim() != "")
                            {
                                if (check_media(row["DesktopImageID"].ToString().Trim(), "image") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["MobileImageID"].ToString().Trim() != "")
                            {
                                if (check_media(row["MobileImageID"].ToString().Trim(), "image") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileImageID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["DesktopImageWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["DesktopImageWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["MobileIamgeWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["UploadPDF"].ToString().Trim() != "")
                            {
                                if (check_media(row["UploadPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["UploadPreviewPDF"].ToString().Trim() != "")
                            {
                                if (check_media(row["UploadPreviewPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPreviewPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        excel_ok = false;
                    }
                    if (excel_ok == true)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {
                            language = row1["languageKey"].ToString();
                            var sheetproduct = Services.ContentService.GetById(Convert.ToInt32(row1["NodeID"].ToString()));
                            
                            if (!String.IsNullOrEmpty(row1["umbracoUrlAlias"].ToString()))
                            {
                                sheetproduct.SetValue("umbracoUrlAlias", row1["umbracoUrlAlias"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["Title"].ToString()))
                            {
                                sheetproduct.SetValue("title", row1["Title"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["SubTitle"].ToString()))
                            {
                                sheetproduct.SetValue("subTitle", row1["SubTitle"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["Description"].ToString()))
                            {
                                sheetproduct.SetValue("Description", row1["Description"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["IsGuestUserSheet"].ToString()))
                            {
                                sheetproduct.SetValue("isGuestUserSheet", row1["IsGuestUserSheet"].ToString() == "Y" ? true : false, language);
                            }
                            if (!String.IsNullOrEmpty(row1["IsEnableForDetailsPage"].ToString()))
                            {
                                sheetproduct.SetValue("IsEnableForDetailsPage", row1["IsEnableForDetailsPage"].ToString() == "Y" ? true : false, language);
                            }
                            if (!String.IsNullOrEmpty(row1["Paid"].ToString()))
                            {
                                sheetproduct.SetValue("isPaid", row1["Paid"].ToString() == "Y" ? true : false, language);
                            }
                            
                            if (!String.IsNullOrEmpty(row1["RankingIndex"].ToString()))
                            {
                                sheetproduct.SetValue("rankingIndex", row1["RankingIndex"].ToString(), language);
                            }
                            if (!String.IsNullOrEmpty(row1["NoofDays"].ToString()))
                            {
                                No_of_days = GetdaysMaster(language)?.Where(a => a.ItemValue == row1["NoofDays"].ToString())?.FirstOrDefault()?.Id;
                                sheetproduct.SetValue("noOfDays", getlink(No_of_days.ToString()), language);
                            }
                            if (!String.IsNullOrEmpty(row1["DesktopImageID"].ToString()))
                            {
                                sheetproduct.SetValue("DesktopImage", check_media(row1["DesktopImageID"].ToString(), "image"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("DesktopNextGenImage", check_media(row1["DesktopImageWebpID"].ToString(), "webp"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["MobileImageID"].ToString()))
                            {
                                sheetproduct.SetValue("mobileImage", check_media(row1["MobileImageID"].ToString(), "image"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["MobileIamgeWebpID"].ToString()))
                            {
                                sheetproduct.SetValue("MobileNextGenImage", check_media(row1["MobileIamgeWebpID"].ToString(), "webp"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["UploadPDF"].ToString()))
                            {
                                sheetproduct.SetValue("UploadPDF", check_media(row1["UploadPDF"].ToString(), "pdf"), language);
                            }
                            if (!String.IsNullOrEmpty(row1["UploadPreviewPDF"].ToString()))
                            {
                                sheetproduct.SetValue("UploadPreviewPDF", check_media(row1["UploadPreviewPDF"].ToString(), "pdf"), language);
                            }
                            sheetproduct.SetCultureName(row1["WorksheetTitle"].ToString(), language);
                            var val = Services.ContentService.Save(sheetproduct);
                        }

                    }
                }
                else
                {
                    message = "Invalid Data!!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "teachersplan_update");
                message = "Error Message!! " + ex.StackTrace;
            }
            return message.ToString();
        }


        public int Check_nodeid(string ws_type, string nodeid, string language)
        {
             ws_type = "lesson";
             nodeid = "7254";
             language = "en-US";
            string agegroupe = "";
            int subject_id =0;

            var sheetproduct = Services.ContentService.GetById(Convert.ToInt32(nodeid));


            int return_value = 0;
            return_value = Convert.ToInt32(nodeid);
            try
            {
                if (ws_type == "worksheet")
                {

                    var classRoot_worksheet = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "structureProgramRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                            .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>()
                                            ?.Where(c => c.AgeGroup.Name == agegroupe)?.FirstOrDefault()?.DescendantsOrSelf().Where(a=>a.Id == Convert.ToInt32(nodeid));

                                            //?.Where(x => x.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
                                            //.Where(s => Umbraco?.Content(s?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == subject_id).FirstOrDefault();

                    //return_value = Convert.ToInt32(classRoot_worksheet.Id);
                }

                if (ws_type == "lesson")
                {
                    var classRoot_lesson = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                       .Where(x => x.ContentType.Alias == "worksheetNode")?.FirstOrDefault().DescendantsOrSelf()?
                                       .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>().Where(a => a.Id == Convert.ToInt32(nodeid));
                    //?.Where(c => c.AgeGroup.Name == agegroupe)?.FirstOrDefault()?.DescendantsOrSelf()?
                    //                   .Where(x => x.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>()?
                    //                   .Where(s => Umbraco?.Content(s?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == subject_id).FirstOrDefault();

                    //return_value = Convert.ToInt32(classRoot_lesson.Id);
                }

                if (ws_type == "teachers")
                {

                    var classRoot_teachers = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                           .Where(x => x.ContentType.Alias == "teacherRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                           .Where(x => x.ContentType.Alias == "worksheetListingAgeWise").OfType<WorksheetListingAgeWise>();
                    //?.Where(c => c.AgeGroup.Name == agegroupe)?.FirstOrDefault();//?
                                                                                                                                                                                                       //.Where(x => x.ContentType.Alias == "worksheetCategory").OfType<WorksheetCategory>();//?
                                                                                                                                                                                                       //.Where(s => Umbraco?.Content(s?.CategoryName?.Udi)?.DescendantsOrSelf()?.OfType<Subjects>()?.FirstOrDefault().SubjectValue == 1).FirstOrDefault();
                    //return_value = Convert.ToInt32(classRoot_teachers.Id);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "Check_nodeid");
            }
            return return_value;
        }

        #endregion


        #region special offer Bulk upload

        public string Specialofferplansave(string language, string offername, string agegroupe, DataTable dt)
        {
            //agegroupe = "4-5";
            //language = "en-US";
            ////DataTable dt1 = new DataTable();
            //dt.Clear();
            //dt.Columns.Add("WorksheetTitle");
            //dt.Columns.Add("umbracoUrlAlias");
            //dt.Columns.Add("Title");
            //dt.Columns.Add("SubTitle");
            //dt.Columns.Add("Description");
            //dt.Columns.Add("IsEnableForDetailsPage");
            //dt.Columns.Add("Paid");
            //dt.Columns.Add("DesktopImageID");
            //dt.Columns.Add("DesktopImageWebpID");
            //dt.Columns.Add("MobileImageID");
            //dt.Columns.Add("MobileIamgeWebpID");
            //dt.Columns.Add("UploadPDF");
            //dt.Columns.Add("UploadPreviewPDF");

            //DataRow _row= dt.NewRow();
            //_row["umbracoUrlAlias"] = "umbracoUrlAlias";
            //_row["WorksheetTitle"] = "WorksheetTitle";
            //_row["Title"] = "Title";
            //_row["SubTitle"] = "SubTitle";
            //_row["Description"] = "Description";
            //_row["IsEnableForDetailsPage"] = "Y";
            //_row["Paid"] = "Y";
            //_row["DesktopImageID"] = "6607";
            //_row["DesktopImageWebpID"] = "";
            //_row["MobileImageID"] = "";
            //_row["MobileIamgeWebpID"] = "";
            //_row["UploadPDF"] = "29021";
            //_row["UploadPreviewPDF"] = "29021";

            //dt.Rows.Add(_row);

            HomeController home = new HomeController();
            string message = "ok";
            try
            {
                if (!String.IsNullOrEmpty(language) && !String.IsNullOrEmpty(agegroupe) && dt != null)
                {
                    string doctypeName = "specialOfferItems";
                    offername = "summercamp";
                    bool excel_ok = true;
                    int row_num = 0;
                    int paretnid = 0;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            row_num = row_num + 1;

                            if (
                                (String.IsNullOrEmpty(row["WorksheetTitle"].ToString().Trim()))
                               || (String.IsNullOrEmpty(row["Title"].ToString().Trim()))
                               || (String.IsNullOrEmpty(row["DesktopImageID"].ToString().Trim()))
                               || (String.IsNullOrEmpty(row["UploadPDF"].ToString().Trim()))
                                    )

                            {
                                excel_ok = false;
                                message = "Invalid data in row number : " + row_num.ToString();
                                break;
                            }

                            if (check_media(row["DesktopImageID"].ToString().Trim(), "image") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid DesktopImageID in row number : " + row_num.ToString();
                                break;
                            }
                            if (row["MobileImageID"].ToString().Trim() != "")
                            {
                                if (check_media(row["MobileImageID"].ToString().Trim(), "image") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileImageID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["DesktopImageWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["DesktopImageWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid DesktopImageWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (row["MobileIamgeWebpID"].ToString().Trim() != "")
                            {
                                if (check_media(row["MobileIamgeWebpID"].ToString().Trim(), "webp") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid MobileIamgeWebpID in row number : " + row_num.ToString();
                                    break;
                                }
                            }
                            if (check_media(row["UploadPDF"].ToString().Trim(), "pdf") == "0")
                            {
                                excel_ok = false;
                                message = "Invalid UploadPDF in row number : " + row_num.ToString();
                                break;
                            }
                            if (row["UploadPreviewPDF"].ToString().Trim() != "")
                            {
                                if (check_media(row["UploadPreviewPDF"].ToString().Trim(), "pdf") == "0")
                                {
                                    excel_ok = false;
                                    message = "Invalid UploadPreviewPDF in row number : " + row_num.ToString();
                                    break;
                                }
                            }

                            if (getparent_specialoffer_nodeid(offername, agegroupe, language) == 0)
                            {
                                excel_ok = false;
                                message = "Invalid OfferID in row number : " + row_num.ToString();
                                break;
                            }
                        }
                    }
                    else
                    {
                        excel_ok = false;
                    }

                    if (excel_ok == true)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {
                                paretnid = getparent_specialoffer_nodeid(offername, agegroupe, language);

                                var sheetproduct = Services.ContentService.Create(row1["WorksheetTitle"].ToString(), paretnid, doctypeName);

                                sheetproduct.SetCultureName(row1["WorksheetTitle"].ToString(), language);

                                sheetproduct.SetValue("umbracoUrlAlias", row1["umbracoUrlAlias"].ToString(), language);
                                sheetproduct.SetValue("title", row1["Title"].ToString(), language);
                                sheetproduct.SetValue("subTitle", row1["SubTitle"].ToString(), language);
                                sheetproduct.SetValue("Description", row1["Description"].ToString(), language);
                                sheetproduct.SetValue("IsEnableForDetailsPage", row1["IsEnableForDetailsPage"].ToString() == "Y" ? true : false, language);
                                sheetproduct.SetValue("paid", row1["Paid"].ToString() == "Y" ? true : false, language);
                                sheetproduct.SetValue("isActive", true, language);

                                sheetproduct.SetValue("DesktopImage", check_media(row1["DesktopImageID"].ToString(), "image"), language);
                                if (!String.IsNullOrEmpty(row1["DesktopImageWebpID"].ToString()))
                                {
                                    sheetproduct.SetValue("DesktopNextGenImage", check_media(row1["DesktopImageWebpID"].ToString(), "webp"), language);
                                }
                                sheetproduct.SetValue("mobileImage", check_media(row1["MobileImageID"].ToString(), "image"), language);
                                if (!String.IsNullOrEmpty(row1["MobileIamgeWebpID"].ToString()))
                                {
                                    sheetproduct.SetValue("MobileNextGenImage", check_media(row1["MobileIamgeWebpID"].ToString(), "webp"), language);
                                }

                                sheetproduct.SetValue("UploadPDF", check_media(row1["UploadPDF"].ToString(), "pdf"), language);
                                if (!String.IsNullOrEmpty(row1["UploadPreviewPDF"].ToString()))
                                {
                                    sheetproduct.SetValue("UploadPreviewPDF", check_media(row1["UploadPreviewPDF"].ToString(), "pdf"), language);
                                }
                                var val = Services.ContentService.Save(sheetproduct);
                        }
                    }
                }
                else
                {
                    message = "Invalid Data!!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "Specialofferplansave");
                message = "Error Message!! " + ex.StackTrace;
            }
            return message.ToString();
        }


        public int getparent_specialoffer_nodeid(string offername, string agegroupe, string language)
        {
            int return_value = 0;
            try
            {
                if (offername == "summercamp")
                {
                    var classRoot_teachers = Umbraco?.ContentAtRoot()?.Where(x => x.ContentType.Alias == "home" && x.Cultures.ContainsKey(language.ToLower()))?.FirstOrDefault()?.DescendantsOrSelf()?
                                           .Where(x => x.ContentType.Alias == "offerRoot")?.FirstOrDefault().DescendantsOrSelf()?
                                           .Where(x => x.ContentType.Alias == "specialOfferAge").OfType<SpecialOfferAge>()?.Where(c => c.ClassName.Name == agegroupe)?.FirstOrDefault();//?
                    return_value = Convert.ToInt32(classRoot_teachers.Id);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(reporting: typeof(CMSBulkUploadController), ex, message: "getparent_specialoffer_nodeid");
            }
            return return_value;
        }
        #endregion
    }
}