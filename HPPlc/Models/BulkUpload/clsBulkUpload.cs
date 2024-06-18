using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.BulkUpload
{
    public class clsBulkUpload
    {
        public List <clsLessionPlan> LessionPlans { get; set; }
        public List<clsworksheetPlan> WorksheetPlans { get; set;}
        public List<clsTeacherPlan> TeacherPlans { get; set; } = new List<clsTeacherPlan>();
    }

    public class clsLessionPlan
    {
        public string NodeID { get; set; }
        public string IsPublished { get; set; }
        public string languageKey { get; set; }
        public string languageName { get; set; }
        public string AgeGroup { get; set; }
        public string SubjectID { get; set; }
        public string WeekID { get; set; }
        public string TopicID { get; set; }
        public string SubjectName { get; set; }
        public string WorksheetTitle { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ShareContent { get; set; }
        public string SharingText { get; set; }
        public string Description { get; set; }
        public string IsGuestUserSheet { get; set; }
        public string CBSEContentIncluded { get; set; }
        public string IsQuizWorksheet { get; set; }
        public string IsEnableforDetailsPage { get; set; }
        public string DescriptionPageContent { get; set; }
        public string DesktopImageID { get; set; }
        public string DesktopImageWebpID { get; set; }
        public string MobileImageID { get; set; }
        public string MobileIamgeWebpID { get; set; }
        public string WhatsAppBannerID { get; set; }
        public string Subscription { get; set; }
        public string UploadPDF { get; set; }
        public string UploadPreviewPDF { get; set; }
       

    }

    public class clsworksheetPlan
    {
        public string NodeID { get; set; }
        public string IsPublished { get; set; }
        public string languageKey { get; set; }
        public string languageName { get; set; }
        public string AgeGroup { get; set; }
        
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string TopicID { get; set; }
        public string WorksheetTitle { get; set; }
        public string umbracoUrlAlias { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string IsGuestUserSheet { get; set; }
        public string IsEnableForDetailsPage { get; set; }
        public string Paid { get; set; }
        public string RankingIndex { get; set; }
        public string DesktopImageID { get; set; }
        public string DesktopImageWebpID { get; set; }
        public string MobileImageID { get; set; }
        public string MobileIamgeWebpID { get; set; }
        public string UploadPDF { get; set; }
        public string UploadPreviewPDF { get; set; }
       
    }

    public class clsTeacherPlan
    {
        public string NodeID { get; set; }
        public string IsPublished { get; set; }
        public string languageKey { get; set; }
        public string languageName { get; set; }
        public string AgeGroup { get; set; }
        
        public string WorksheetTitle { get; set; }
        public string umbracoUrlAlias { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string NoofDays { get; set; }
        public string IsGuestUserSheet { get; set; }
        public string IsEnableForDetailsPage { get; set; }
        public string Paid { get; set; }
        public string RankingIndex { get; set; }
        public string DesktopImageID { get; set; }
        public string DesktopImageWebpID { get; set; }
        public string MobileImageID { get; set; }
        public string MobileIamgeWebpID { get; set; }
        public string UploadPDF { get; set; }
        public string UploadPreviewPDF { get; set; }
        
    }

    public class clsSpecialOffer
    {
        public string WorksheetTitle { get; set; }
        public string umbracoUrlAlias { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string IsEnableForDetailsPage { get; set; }
        public string Paid { get; set; }
        public string DesktopImageID { get; set; }
        public string DesktopImageWebpID { get; set; }
        public string MobileImageID { get; set; }
        public string MobileIamgeWebpID { get; set; }
        public string UploadPDF { get; set; }
        public string UploadPreviewPDF { get; set; }

    }
}