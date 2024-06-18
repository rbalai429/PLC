using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using HP_PLC_Doc.Models;
using System.Text;
using iTextSharp.text.html.simpleparser;

namespace HP_PLC_Doc.Controllers
{
	public class PdfGeneratorController : Controller
	{
		iTextSharp.text.Image jpg;
		public PdfGeneratorController()
		{
		}

		// GET: PdfGenerator
		public byte[] PdfGenerateAndSave(InvoiceModel invoiceModel, string TransactionId,string specialplan = "")
		{
			MemoryStream ms = new MemoryStream();
			//Create document
			Document doc = new Document(iTextSharp.text.PageSize.A4, 15, 20, 20, 10);

			//Create PDF Table
			PdfPTable tableLayout = new PdfPTable(9);
			PdfPTable tableLayout_Title = new PdfPTable(1);
			PdfPTable tableLayout_Paragraph = new PdfPTable(1);
			PdfPTable tableLayout_ParagraphBottom = new PdfPTable(1);
			PdfPTable tableLayout_ParagraphBtm = new PdfPTable(1);
			PdfPTable tableLayout_TwoColumnParagraph = new PdfPTable(2);

			tableLayout_Title.WidthPercentage = 100;
			tableLayout_Paragraph.WidthPercentage = 100;
			tableLayout_ParagraphBottom.WidthPercentage = 100;
			tableLayout_ParagraphBtm.WidthPercentage = 100;

			float[] twocolmheaders = { 22, 78 };  //Header Widths
			tableLayout_TwoColumnParagraph.SetWidths(twocolmheaders);
			tableLayout_TwoColumnParagraph.WidthPercentage = 100;

			//Create a PDF file in specific path
			//var fileName = DateTime.Now.Ticks.ToString() + ".pdf";
			//string path = System.Web.Hosting.HostingEnvironment.MapPath("/Sample-PDF-File.pdf");
			//PdfWriter.GetInstance(doc, new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("/Sample-PDF-File.pdf"), FileMode.Create));
			PdfWriter.GetInstance(doc, ms);

			//HTMLWorker htmlparser = new HTMLWorker(doc);
			//Open the PDF document
			doc.Open();

			//string imageURL = "https://d1o0e2ejzaj3cn.cloudfront.net/media/bhthgr2b/hp-logo.png";
			if (invoiceModel.Logo != null && !string.IsNullOrWhiteSpace(invoiceModel.Logo))
			{
				try
				{
					jpg = iTextSharp.text.Image.GetInstance(invoiceModel.Logo);
				}
				catch { }
				//Resize image depend upon your need
				//jpg.ScaleToFit(140f, 120f);
				if (jpg != null)
				{
					//Give space before image
					jpg.SpacingBefore = 5f;

					//Give some space after the image
					jpg.SpacingAfter = 70f;

					jpg.Alignment = Element.ALIGN_LEFT;

					doc.Add(jpg);
				}
			}
			//iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
			

			AddCellToHeaderTitle(tableLayout_Title, invoiceModel.InvoiceTitle);
			doc.Add(tableLayout_Title);

			AddCellToParagraph(tableLayout_Paragraph, invoiceModel.Address);
			if (invoiceModel.HasGSTNo)
			{
				AddCellToParagraphPadding(tableLayout_Paragraph, "GST NO: " + invoiceModel.GSTNo);
			}
			AddCellToParagraphPadding(tableLayout_Paragraph, "Customer email: " + invoiceModel.UserEmailId);
			if (invoiceModel?.InvoiceList != null && invoiceModel.InvoiceList.Any())
			{
				AddCellToParagraphPadding(tableLayout_Paragraph, "Invoice no: " + invoiceModel?.InvoiceList?.First()?.InvoiceNo);
			}
			AddCellToParagraphPadding(tableLayout_Paragraph, "Place of supply: " + invoiceModel.PlaceOfSupply);
			doc.Add(tableLayout_Paragraph);

			Paragraph p = new Paragraph();
			p.Add("  ");
			p.SpacingAfter = 6f;
			doc.Add(p);

			//Add Content to PDF
			doc.Add(Add_Content_To_PDF(tableLayout, invoiceModel,specialplan));

			Paragraph p1 = new Paragraph();
			p1.Add("  ");
			p1.SpacingBefore = 6f;
			doc.Add(p1);

			AddCellToParagraphPadding(tableLayout_ParagraphBottom, "Reference id: " + TransactionId);

			if (invoiceModel?.InvoiceList != null && invoiceModel.InvoiceList.Any())
			{
				AddCellToParagraphPadding(tableLayout_ParagraphBottom, "Payment date: " + invoiceModel?.InvoiceList?.First()?.PaymentDate);
			}
			doc.Add(tableLayout_ParagraphBottom);

			Paragraph p3 = new Paragraph();
			p3.Add(" ");
			p3.SpacingAfter = 10f;
			doc.Add(p3);

			AddCellToParagraphPaddingCtrAlign(tableLayout_ParagraphBtm, invoiceModel.ComputerGeneratedText);
			AddCellToInBold(tableLayout_ParagraphBtm, "Registered Address");
			AddCellToParagraphPaddingCtrAlign(tableLayout_ParagraphBtm, invoiceModel.BelowAddress);

			doc.Add(tableLayout_ParagraphBtm);
			//doc.Add(tableLayout_TwoColumnParagraph);

			//int totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
			//htmlparser.Parse(sr);
			//Paragraph Title = new Paragraph();
			//Title.Add(invoiceModel.InvoiceTitle);
			//Title.Alignment = Element.ALIGN_CENTER;
			////Font arial = FontFactory.GetFont("Arial", 28, BaseColor.BLACK);
			//BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
			//Font times = new Font(bfTimes, 40f, Font.ITALIC, BaseColor.BLACK);

			////var titleFont = new Font(Font.FontFamily.COURIER, 18, Font.BOLD, BaseColor.WHITE);// FontFactory.GetFont("Arial", 18f, BaseColor.BLACK);
			//Title.Font = times;
			//Title.SpacingBefore = 20;
			//Title.SpacingAfter = 20;
			//doc.Add(Title);

			//if (invoiceModel.HasAddress)
			//{
			//	Paragraph paragraphAddress = new Paragraph();
			//	paragraphAddress.Add(invoiceModel.Address);
			//	paragraphAddress.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//	doc.Add(paragraphAddress);
			//}
			//if (invoiceModel.HasGSTNo)
			//{
			//	Paragraph paragraphGSTNo = new Paragraph();
			//	paragraphGSTNo.Add("GST NO: " + invoiceModel.GSTNo.ToString());
			//	paragraphGSTNo.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//	paragraphGSTNo.SpacingAfter = 15;
			//	doc.Add(paragraphGSTNo);
			//}
			//if (!String.IsNullOrWhiteSpace(invoiceModel.UserEmailId))
			//{
			//	Paragraph paragraphEmail = new Paragraph();
			//	paragraphEmail.Add("Customer Email : " + invoiceModel.UserEmailId.ToString());
			//	paragraphEmail.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//	paragraphEmail.SpacingAfter = 15;
			//	doc.Add(paragraphEmail);
			//}

			//Paragraph paragraphPlaceOfSupply = new Paragraph();
			//paragraphPlaceOfSupply.Add("Place of supply : " + invoiceModel.PlaceOfSupply);
			//paragraphPlaceOfSupply.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//paragraphPlaceOfSupply.SpacingAfter = 15;
			//doc.Add(paragraphPlaceOfSupply);

			//Paragraph paragraphInvoice = new Paragraph();
			//paragraphInvoice.Add("Invoice no : " + invoiceModel.InvoiceNo);
			//paragraphInvoice.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//paragraphInvoice.SpacingAfter = 15;
			//doc.Add(paragraphInvoice);



			//Paragraph paragrathReference = new Paragraph();
			//paragrathReference.Add("Reference Id: " + TransactionId);
			//paragrathReference.Alignment = Element.ALIGN_LEFT;
			//paragrathReference.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//paragraphInvoice.SpacingBefore = 20;
			//paragraphInvoice.SpacingAfter = 20;
			//doc.Add(paragrathReference);

			//Paragraph paragrathDate = new Paragraph();
			//paragrathDate.Add("Payment date : " + (invoiceModel.PaymentDate?.ToString("dd/MM/yyyy")));
			//paragrathDate.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//paragrathDate.Alignment = Element.ALIGN_LEFT;
			//paragrathDate.SpacingAfter = 30;
			//doc.Add(paragrathDate);

			//Paragraph paragrathBottom = new Paragraph();
			//paragrathBottom.Add(invoiceModel.ComputerGeneratedText);
			//paragrathBottom.Alignment = Element.ALIGN_CENTER;
			//paragrathBottom.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//doc.Add(paragrathBottom);

			//Paragraph paragrathBottomAddress = new Paragraph();
			//paragrathBottomAddress.Add(invoiceModel.BelowAddress);
			//paragrathBottomAddress.Font = FontFactory.GetFont(invoiceModel.InvoiceFontFamily, Convert.ToInt32(invoiceModel.InvoiceFontSize == 0 ? 9 : invoiceModel.InvoiceFontSize), BaseColor.BLACK);
			//paragrathBottomAddress.Alignment = Element.ALIGN_CENTER;
			//doc.Add(paragrathBottomAddress);

			// Closing the document
			doc.Close();
			byte[] bytes = ms.ToArray();
			ms.Close();

			return bytes;
		}

		private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, InvoiceModel invoive,string specialplan)
		{
			decimal FinalTotal = 0;
			decimal DiscountAmt = 0;
			float[] headers = { 10, 20, 8, 17, 10, 9, 9, 8, 8 };  //Header Widths
			tableLayout.SetWidths(headers);        //Set the pdf headers
			tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
			tableLayout.PaddingTop = 50;

			decimal payableAmount = 0;
			decimal couponDiscountAmt = invoive.InvoiceList.FirstOrDefault().CouponDiscountAmt;

			//decimal ExistingUserDiscountedAmount = 0;
			//decimal CouponDiscountAmount = 0;
			//decimal SubscriptionAmount = 0;

			//HPPlc.Models.SubscriptionAmountCalc subscriptionAmountCalc = new HPPlc.Models.SubscriptionAmountCalc();
			//payableAmount = subscriptionAmountCalc.GetPayableAmount();
			//ExistingUserDiscountedAmount = subscriptionAmountCalc.GetExistingUserDiscountAmount();
			//CouponDiscountAmount = subscriptionAmountCalc.GetCouponDiscountAmount();
			//SubscriptionAmount = subscriptionAmountCalc.GetSubscriptionAmount();

			//Add Title to the PDF file at the top
			//tableLayout.AddCell(new PdfPCell(new Phrase("Creating PDF file using iTextsharp", new Font(Font.HELVETICA, 13, 1, new iTextSharp.text.Color(153, 51, 0)))) { Colspan = 4, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER });

			//Add header
			AddCellToHeader(tableLayout, "Date");
			AddCellToHeader(tableLayout, "Description");
			AddCellToHeader(tableLayout, "Part Code");
			AddCellToHeader(tableLayout, "Subscription Period");
			AddCellToHeader(tableLayout, "SAC");
			if (invoive.HasSGST)
				AddCellToHeader(tableLayout, "SGST @ " + invoive?.SGST + "%");
			if (invoive.HasCGST)
				AddCellToHeader(tableLayout, "CGST @ " + invoive?.CGST + "%");
			//if (invoive.HasDiscount)
			//	AddCellToHeader(tableLayout, "Discount @" + invoive.Discount);
			AddCellToHeader(tableLayout, "Amount");
			AddCellToHeader(tableLayout, "Sub Total");

			//Add body
			if (invoive.InvoiceList != null && invoive.InvoiceList.Any())
			{
				foreach (var invoicedtls in invoive.InvoiceList)
				{
					decimal? calSGCT = 0;
					decimal? calCGST = 0;
					if (invoive.HasSGST && invoive.SGST > 0 && invoicedtls.SubscriptionPrice > 0)
					{
						calSGCT = ((invoicedtls.SubscriptionPrice * invoive.SGST) / 100);
					}
					if (invoive.HasCGST && invoive.CGST > 0 && invoicedtls.SubscriptionPrice > 0)
					{
						calCGST = ((invoicedtls.SubscriptionPrice * invoive.CGST) / 100);
					}

					AddCellToBody(tableLayout, invoicedtls.PaymentDate);
					AddCellToBody(tableLayout, invoicedtls.SubscriptionDtls);
					AddCellToBody(tableLayout, invoicedtls.PartCode);
					AddCellToBody(tableLayout, invoicedtls.SubscriptionStartDate + " - " + invoicedtls.SubscriptionEndDate);
					AddCellToBody(tableLayout, invoive.SAC);

					if (invoive.HasSGST)
					{
						if (invoive.HasSGST && invoive.SGST > 0 && invoicedtls.SubscriptionPrice > 0)
						{
							AddCellToBody(tableLayout, calSGCT.ToString());
						}
						else
						{
							AddCellToBody(tableLayout, "--");
						}
					}
					if (invoive.HasCGST)
					{
						if (invoive.HasCGST && invoive.CGST > 0 && invoicedtls.SubscriptionPrice > 0)
						{
							AddCellToBody(tableLayout, calCGST.ToString());
						}
						else
						{
							AddCellToBody(tableLayout, "--");
						}
					}
					if(!String.IsNullOrWhiteSpace(specialplan) && specialplan == "sp")
						AddCellToBody(tableLayout, Convert.ToDecimal(invoive.InvoiceList.FirstOrDefault().MaxPrice).ToString());
					else
						AddCellToBody(tableLayout, Convert.ToDecimal(invoicedtls.SubscriptionPrice).ToString());
					//var subTotal = (invoicedtls.SubscriptionPrice + calSGCT + calCGST);

					FinalTotal += invoicedtls.SubscriptionPrice;
					DiscountAmt += invoicedtls.Discount;

					if (!String.IsNullOrWhiteSpace(specialplan) && specialplan == "sp")
						AddCellToBody(tableLayout, Convert.ToDecimal(invoive.InvoiceList.FirstOrDefault().MaxPrice).ToString());
					else
						AddCellToBody(tableLayout, Convert.ToDecimal(invoicedtls.SubscriptionPrice).ToString());
				}
			}

			if (!String.IsNullOrWhiteSpace(specialplan) && specialplan == "sp")
			{
				//Add Body Footer Discount
				AddCellToBodyFooter(tableLayout, "", 4);
				AddCellToBodyFooter(tableLayout, "Discount Amount", 4);
				AddCellToBody(tableLayout, invoive.InvoiceList.FirstOrDefault().DiscountPrice.ToString());
			}
			else {
				if (FinalTotal > 0)
				{
					//Add Body Footer Discount
					AddCellToBodyFooter(tableLayout, "", 4);
					AddCellToBodyFooter(tableLayout, "Subscription Total Amt.", 4);
					AddCellToBody(tableLayout, FinalTotal.ToString());
				}
				if (DiscountAmt > 0)
				{
					//Add Body Footer Discount
					AddCellToBodyFooter(tableLayout, "", 4);
					AddCellToBodyFooter(tableLayout, "Existing Member Discount Amt.", 4);
					AddCellToBody(tableLayout, DiscountAmt.ToString());
				}

				if (invoive?.InvoiceList?.FirstOrDefault()?.CouponDiscountAmt > 0)
				{
					//Add Coupon Discount
					AddCellToBodyFooter(tableLayout, "", 4);
					AddCellToBodyFooter(tableLayout, "Coupon Discount Amt.", 4);
					AddCellToBody(tableLayout, couponDiscountAmt.ToString());
				}
			}

			if (!String.IsNullOrWhiteSpace(specialplan) && specialplan == "sp")
			{
				payableAmount = invoive.InvoiceList.FirstOrDefault().SubscriptionPrice;
			}
			else
			{
				payableAmount = (FinalTotal - DiscountAmt - couponDiscountAmt);
				if (payableAmount <= 0)
				{
					payableAmount = 0;
				}
			}
			//Add Body Footer
			AddCellToBodyFooter(tableLayout, "", 4);
			AddCellToBodyFooter(tableLayout, "Total Paid Amt.", 4);
			AddCellToBody(tableLayout, Math.Floor(payableAmount).ToString());

			return tableLayout;
		}

		// Method to add single cell to the header
		private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 9f, Font.BOLD, BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(1, 151, 214) });
		}

		// Method to add single cell to the body
		private static void AddCellToBody(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.WHITE });
		}
		private static void AddCellToBodyFooter(PdfPTable tableLayout, string cellText, int colSpan)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.WHITE, Colspan = colSpan });
		}

		private static void AddCellToHeaderTitle(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 20f, Font.BOLD, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, FixedHeight = 50, Padding = 5, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.WHITE });
		}
		private static void AddCellToInBold(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 30, Padding = 5, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.WHITE });
		}
		private static void AddCellToParagraph(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.WHITE });
		}

		private static void AddCellToParagraphPadding(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingBottom = 20, Padding = 5, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.WHITE });
		}

		private static void AddCellToParagraphPaddingCtrAlign(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingBottom = 20, Padding = 5, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.WHITE });
		}
		private static void AddCellToParagraphPaddingLeftAlign(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingBottom = 20, Padding = 5, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.WHITE });
		}
	}
}