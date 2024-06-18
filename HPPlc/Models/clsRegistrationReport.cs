using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class clsRegistrationReport
	{
		public int userId { get; set; }
		public string userUniqueId { get; set; }
		public string u_name { get; set; }
		public string u_email { get; set; }
		public string u_whatsappno_prefix { get; set; }
		public string u_whatsappno { get; set; }
		//public string[] ageGroup { get; set; }
		public string ComWithEmail { get; set; }
		public string ComWithWhatsApp { get; set; }
		public string ComWithPhone { get; set; }
		public string CheckedTAndC { get; set; }
		public DateTime DOC { get; set; }
		public string referralCode { get; set; }
		public int IsActive { get; set; }
        public string Mode
        {
            get; set;
        }
		public string DataSource
		{
			get; set;
		}
		public string AgeGroup
		{
			get; set;
		}
	}
	public class RegistrationExcelReports
	{
      
        public string Name
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string WhatAppContact
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public string Reason
        {
            get; set;
        }


    }


    public class clsNotificationReport
    {
        public string DateOfAction { get; set; }
        public string WhatsAppTotalDownloaded { get; set; }
        public string WhatsAppUniqueDownloaded { get; set; }
        public string SFMCDownloaded { get; set; }
        public string LessionPlanDownloadWorksheet { get; set; }
        public string LessionPlanPrint { get; set; }
        public string BonusPlanDownloadWorksheet { get; set; }
        public string BonusPlanDownloadPrint { get; set; }
        public string BonusPlanDownloadVideo { get; set; }
    }
    public class clsOTPReport
    {
        public string GetOtpTotal { get; set; }
        public string InvalidOtpTotal { get; set; }
        public string UniqueGetOtpEmail { get; set; }
        public string UniqueGetOtpMobile { get; set; }
        public string SuccessfulVerifiedUniqueOtp { get; set; }
        public string SuccessfullRegistration { get; set; }
    }
    public class clsUserDownloadData
    {
        public string UserId { get; set; }
        public string UserUniqueId { get; set; }
        public string UserName { get; set; }
        public string TotalDownloaded { get; set; }
    }
    public class clsUserDownloadDataByUser
    {
        public string UserId { get; set; }
        public string UserUniqueId { get; set; }
        public string UserName { get; set; }
        public string CultureInfo { get; set; }
        public string Age { get; set; }
        public string WorkSheetId { get; set; }
        public string WorkshhetPDFUrl { get; set; }
        public string FromDestination { get; set; }
        public string BonusWorksheetType { get; set; }
    }

    public class clsWorksheetDownloadDataByUser
    {
        public string UserId { get; set; }
        public string UserUniqueId { get; set; }
        public string Age { get; set; }
        public string CultureInfo { get; set; }
        public string WorkSheetId { get; set; }
        public string WorkshhetPDFUrl { get; set; }
        public string FromDestination { get; set; }
        public string StructuredProgramIsPaid { get; set; }
        public string InsertedOn { get; set; }
    }
       public class clsReferralReport
    {
        public string UserId { get; set; }
        public string UserUniqueId { get; set; }
        public string UserName { get; set; }
        public string ReferralCode { get; set; }
        public string RefToUserId { get; set; }
        public string RefToUserUniqueId { get; set; }
        public string RefToUserName { get; set; }
        public string PlanType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class clsURLManipulationRequest
    {
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }
    }
    public class clsURLManipulation
    {
        public string RowId { get; set; }
    }
    public class clsAgeGroupeMaster
    {
        public string id { get; set; }
        public string AgeGroup { get; set; }
    }

    public class clsLanguageMaster
    {
        public string id { get; set; }
        public string LanguageDetails { get; set; }
    }
    public class clsProgramTypeMaster
    {
        public string id { get; set; }
        public string ProgramType { get; set; }
    }

    public class clsWorksheetLession
    {
        public string SubjectID { get; set; }
        public string WorksheetTitle { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ShareContent { get; set; }
        public string SharingText { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string WeekID { get; set; }
        public string TopicID { get; set; }
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
    public class clsWorksheetStructure
    {
        public string SubjectID { get; set; }
        public string WorksheetTitle { get; set; }
        public string umbracoUrlAlias { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string AgeGroup { get; set; }
        public string Subject { get; set; }
        public string TopicID { get; set; }
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
    public class clsWorksheetTeacher
    {
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
    public class clsUploadWorksheetExcel
    {
        public string ProgramType { get; set; }
        public string AgeGroupe { get; set; }
        public string Language { get; set; }
        public string JsonData { get; set; }
    }
    public class MediaReferenceRequest
    {
        public string NodeId { get; set; }
    }
    public class MediaReference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extention { get; set; }
    }

    public class clsSendWhatsAppNotification
    {
        public string UserUniqueId { get; set; }
        public string Mode { get; set; }
        public string iStatus { get; set; }
        public string IsTestId { get; set; }
    }
    public class clsAgeGroupeAdvancedSearch
    {
        public string ClassName { get; set; }
        public string SynonymsName { get; set; }
    }
    public class clsAgeGroupeAdvancedSearchResponse
    {
        public string RowId { get; set; }
    }
    public class clsAgeGroupeAdvancedSearchList
    {
        public string RowId { get; set; }
        public string ClassName { get; set; }
        public string SynonymsName { get; set; }
    }
    public class clsNoRecordFoundReport
    {
        public string Message { get; set; }
        public string SearchText { get; set; }
        public string DateOfCreation { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }

    public class clsUploadWorksheetExcelUpdate
    {
        public string ProgramType { get; set; }
        public string AgeGroupe { get; set; }
        public string Language { get; set; }
        public string JsonData { get; set; }
    }

    public class clsUserTransaction
    {
        public string UserId { get; set; }
        public string UserUniqueId { get; set; }
        public string UserName { get; set; }
        public string Class { get; set; }
        public string DateOfRegistration { get; set; }
        public string WhatsAppNo { get; set; }
        public string Emailid { get; set; }
        public string MobileConsent { get; set; }
        public string EmailConsent { get; set; }
        public string WhatsAppConsent { get; set; }
        public string LessionPlanAllSubscriptions { get; set; }
        public string WorksheetPlanAllSubscriptions { get; set; }
        public string TeachersPlanAllSubscriptions { get; set; }
        public string LessionPlanActiveSubscriptions { get; set; }
        public string WorksheetPlanActiveSubscriptions { get; set; }
        public string TeachersPlanActiveSubscriptions { get; set; }
        public string LessionLastSubscriptionDate { get; set; }
        public string WorksheetLastSubscriptionDate { get; set; }
        public string TeachersLastSubscriptionDate { get; set; }
        public string SystemTotalDownloadNumber { get; set; }
        public string UniqueTotalDownloadNumber { get; set; }
        public string AreYouStudentOrParent { get; set; }
        public string ReferralCode { get; set; }
        public string ReferredByCode { get; set; }

    }
}