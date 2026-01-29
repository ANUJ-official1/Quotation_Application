using iTextSharp.text;
using iTextSharp.text.pdf;
using QuotationApp;
using QuotationApp.Helpers;
using System;
using System.IO;

public class QuotationFooter : iTextSharp.text.pdf.PdfPageEventHelper
{
    private string gstNo;
    private string terms;
    private Font small;
    private Font smallBold;

    public QuotationFooter(string gstNo, string terms)
    {
        this.gstNo = gstNo;
        this.terms = terms;

        this.small = FontFactory.GetFont(FontFactory.HELVETICA, 9);
        this.smallBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
    }

    public override void OnEndPage(PdfWriter writer, Document doc)
    {
        PdfPTable footerTbl = new PdfPTable(2); // 2 columns: Left (GST + Terms), Right (Signature)
        footerTbl.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
        footerTbl.SetWidths(new float[] { 60f, 40f }); // left/right width ratio

        // ----------------- LEFT SIDE (GST + Terms) -----------------
        PdfPTable leftTbl = new PdfPTable(1);

        // GST Box
        Phrase gstPhrase = new Phrase("GSTN No.: " + gstNo, smallBold);
        PdfPCell gstCell = new PdfPCell(gstPhrase);
        gstCell.BackgroundColor = new BaseColor(200, 255, 200);
        gstCell.Border = Rectangle.NO_BORDER;
        leftTbl.AddCell(gstCell);

        // Terms
        PdfPCell termsCell = new PdfPCell();
        termsCell.Border = Rectangle.NO_BORDER;
        termsCell.PaddingTop = 5;
        termsCell.AddElement(new Paragraph("Terms & Conditions:", smallBold));
        termsCell.AddElement(new Paragraph(terms ?? "", small));
        leftTbl.AddCell(termsCell);

        PdfPCell leftWrap = new PdfPCell(leftTbl);
        leftWrap.Border = Rectangle.NO_BORDER;
        footerTbl.AddCell(leftWrap);

        // ----------------- RIGHT SIDE (Signature) -----------------
        PdfPTable sigTbl = new PdfPTable(1);
        sigTbl.DefaultCell.Border = Rectangle.NO_BORDER;

        // Top text
        PdfPCell sigTop = new PdfPCell(new Phrase("For J.P. ELECTRICALS", smallBold));
        sigTop.Border = Rectangle.NO_BORDER;
        sigTop.HorizontalAlignment = Element.ALIGN_RIGHT;
        sigTbl.AddCell(sigTop);

        // Image
        string sigPath = DatabaseHelper.GetSetting("SignaturePath");
        if (!string.IsNullOrEmpty(sigPath) && File.Exists(sigPath))
        {
            var img = iTextSharp.text.Image.GetInstance(sigPath);
            img.ScaleToFit(150f, 70f);   // bigger

            PdfPCell imgCell = new PdfPCell(img, false);
            imgCell.Border = Rectangle.NO_BORDER;
            imgCell.HorizontalAlignment = Element.ALIGN_CENTER;  // ✅ Center
            imgCell.PaddingTop = 10;
            sigTbl.AddCell(imgCell);
        }


        // Bottom text
        PdfPCell sigBottom = new PdfPCell(new Phrase("(Authorized Signatory)", smallBold));
        sigBottom.Border = Rectangle.NO_BORDER;
        sigBottom.HorizontalAlignment = Element.ALIGN_RIGHT;
        sigTbl.AddCell(sigBottom);

        PdfPCell sigWrap = new PdfPCell(sigTbl);
        sigWrap.Border = Rectangle.NO_BORDER;
        footerTbl.AddCell(sigWrap);

        // ----------------- Render Footer -----------------
        footerTbl.WriteSelectedRows(
    0, -1,
    doc.LeftMargin,
    doc.BottomMargin + footerTbl.TotalHeight, // proper bottom position
    writer.DirectContent
);

    }
}
