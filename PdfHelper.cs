using iTextSharp.text;
using iTextSharp.text.pdf;
using QuotationApp.Helpers;
using System;
using System.IO;
using System.Windows.Forms;

namespace QuotationApp
{
    public static class PdfHelper
    {
        public static void ExportQuotationToPdf(int id, string outputFile)
        {
            var q = LoadQuotationFromDb(id);

            var doc = new Document(PageSize.A4, 36, 36, 36, 36);
            using (var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var writer = PdfWriter.GetInstance(doc, fs);

                // Footer attach karo
                writer.PageEvent = new QuotationFooter(q.GSTNo, q.Terms);

                doc.Open();

                // Colors
                var orange = new BaseColor(226, 74, 14);
                var greenBar = new BaseColor(200, 255, 200);
                var lightGray = new BaseColor(245, 245, 245);

                // Fonts
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 30, orange);
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normal = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var small = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                var smallBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);

                // --- Company Header with Logo + Name side by side
                PdfPTable headerTbl = new PdfPTable(2) { WidthPercentage = 100 };
                headerTbl.SetWidths(new float[] { 1f, 3f }); // left=logo, right=text

                // --- Logo cell
                string logo = DatabaseHelper.GetSetting("LogoPath");
                if (!string.IsNullOrEmpty(logo) && File.Exists(logo))
                {
                    var img = iTextSharp.text.Image.GetInstance(logo);
                    img.ScaleToFit(100f, 100f); // ⬅ Bigger & proportional
                    img.Alignment = Element.ALIGN_LEFT;

                    PdfPCell logoCell = new PdfPCell(img)
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 5
                    };
                    headerTbl.AddCell(logoCell);
                }
                else
                {
                    // Empty placeholder if no logo
                    PdfPCell emptyLogo = new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER };
                    headerTbl.AddCell(emptyLogo);
                }

                // --- Company Name + Details in right cell
                PdfPTable textBlock = new PdfPTable(1) { WidthPercentage = 100 };

                // Line 1 - Company Name
                Paragraph compName = new Paragraph("J.P. Electricals", titleFont);
                compName.Alignment = Element.ALIGN_LEFT;
                PdfPCell nameCell = new PdfPCell(compName)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingBottom = 3
                };
                textBlock.AddCell(nameCell);

                // Line 2 - Description
                Paragraph compDesc = new Paragraph(
                    "We Undertake Repairing & Maintenance Work Of All Types O PLC Control, Automation, DC Work,Alternator Work, Electrical Equipments, Panel Work & Commissioning.", normal);

                compDesc.Alignment = Element.ALIGN_LEFT;
                PdfPCell descCell = new PdfPCell(compDesc)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingBottom = 2
                };
                textBlock.AddCell(descCell);

                // Line 3 - Address
                Paragraph compAddr = new Paragraph(
                    "# 10, Radhakrishna Nagari, Near Rajiv Nagar Bus Stop, Hingna Road, Nagpur. Mob.9822699075", small);
                compAddr.Alignment = Element.ALIGN_LEFT;
                PdfPCell addrCell = new PdfPCell(compAddr)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                textBlock.AddCell(addrCell);

                PdfPCell textCell = new PdfPCell(textBlock)
                {
                    Border = Rectangle.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                headerTbl.AddCell(textCell);

                doc.Add(headerTbl);

                doc.Add(new Paragraph("\n"));


                // --- Quotation Green Bar
                PdfPTable quotationBar = new PdfPTable(1) { WidthPercentage = 100 };
                PdfPCell qCell = new PdfPCell(new Phrase("QUOTATION", headerFont))
                {
                    BackgroundColor = greenBar,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                };
                quotationBar.AddCell(qCell);
                doc.Add(quotationBar);

                doc.Add(new Paragraph("\n"));
                // --- Quotation Header Table with Borders ---
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 2f, 1f });

                // Left Cell Content
                PdfPTable leftInnerTable = new PdfPTable(1);
                leftInnerTable.WidthPercentage = 50;

                PdfPCell toCellLeft = new PdfPCell(new Phrase("To,", smallBold));
                PdfPCell customerNameCell = new PdfPCell(new Phrase(q.CustomerName, smallBold));
                PdfPCell customerAddressCell = new PdfPCell(new Phrase(q.Address, smallBold));

                // Apply borders to each cell
                toCellLeft.Border = Rectangle.BOX;
                customerNameCell.Border = Rectangle.BOX;
                customerAddressCell.Border = Rectangle.BOX;

                leftInnerTable.AddCell(toCellLeft);
                leftInnerTable.AddCell(customerNameCell);
                leftInnerTable.AddCell(customerAddressCell);

                PdfPCell leftOuterCell = new PdfPCell(leftInnerTable);
                leftOuterCell.Border = Rectangle.BOX;
                leftOuterCell.Padding = 5;
                headerTable.AddCell(leftOuterCell);

                // Right Cell Content
                PdfPTable rightInnerTable = new PdfPTable(1);
                rightInnerTable.WidthPercentage = 50;

                PdfPCell quotationNoCell = new PdfPCell(new Phrase("Quotation No: " + q.QuotationNo, smallBold));
                PdfPCell quotationDateCell = new PdfPCell(new Phrase("Date: " + q.Date.ToString("dd-MM-yyyy"), smallBold));

                // Apply borders to each cell
                quotationNoCell.Border = Rectangle.BOX;
                quotationDateCell.Border = Rectangle.BOX;

                rightInnerTable.AddCell(quotationNoCell);
                rightInnerTable.AddCell(quotationDateCell);

                PdfPCell rightOuterCell = new PdfPCell(rightInnerTable);
                rightOuterCell.Border = Rectangle.BOX;
                rightOuterCell.Padding = 5;
                headerTable.AddCell(rightOuterCell);

                // Add to document
                doc.Add(headerTable);
                doc.Add(new Paragraph("\n"));



                // --- Kind Attn
                PdfPTable kindbar = new PdfPTable(1) { WidthPercentage = 100 };
                PdfPCell kCell = new PdfPCell(new Paragraph($"Kind Attn.: {q.KindAttn}", headerFont))
                {
                    BackgroundColor = greenBar,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                };
                kindbar.AddCell(kCell);
                doc.Add(kindbar);

                doc.Add(new Paragraph("\n"));
                // --- Items Table
                PdfPTable table = new PdfPTable(new float[] { 0.6f, 3f, 1f, 0.8f, 1f, 1.2f }) { WidthPercentage = 100 };
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                string[] headers = { "Sr.No", "Particulars", "Unit", "Qty", "Rate", "Amount" };
                foreach (var h in headers)
                {
                    PdfPCell hc = new PdfPCell(new Phrase(h, smallBold));
                    hc.BackgroundColor = lightGray;
                    hc.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(hc);
                }

                // --- Add items
                int rowIndex = 0;
                foreach (var it in q.Items)
                {
                    rowIndex++;
                    bool isLastRow = rowIndex == q.Items.Count; // ✅ check last row

                    table.AddCell(new PdfPCell(new Phrase(it.SrNo.ToString(), small))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Border = isLastRow ? Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER
                                           : Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1f
                    });
                    table.AddCell(new PdfPCell(new Phrase(it.Particulars ?? "", small))
                    {
                        Border = isLastRow ? Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER
                                           : Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1f
                    });
                    table.AddCell(new PdfPCell(new Phrase(it.Unit ?? "", small))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Border = isLastRow ? Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER
                                           : Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1f
                    });
                    table.AddCell(new PdfPCell(new Phrase(it.Qty.ToString("0.##"), small))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Border = isLastRow ? Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER
                                           : Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1f
                    });
                    table.AddCell(new PdfPCell(new Phrase(it.Rate.ToString("0.00"), small))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        Border = isLastRow ? Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER
                                           : Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1f
                    });
                    table.AddCell(new PdfPCell(new Phrase(it.Amount.ToString("0.00"), small))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        Border = isLastRow ? Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER
                                           : Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1f
                    });
                }

                // --- Fix row count to fill page
                int maxRows = 20;   // 👈 yaha set karo kitne rows ek page pe dikhni chahiye
                int currentRows = q.Items.Count;
                int emptyRows = maxRows - currentRows;

                // Empty rows fill karne ke liye
                for (int i = 0; i < emptyRows; i++)
                {
                    int borderStyle = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;  // ❌ bottom border नहीं
                    table.AddCell(new PdfPCell(new Phrase("")) { FixedHeight = 18, Border = borderStyle });
                    table.AddCell(new PdfPCell(new Phrase("")) { Border = borderStyle });
                    table.AddCell(new PdfPCell(new Phrase("")) { Border = borderStyle });
                    table.AddCell(new PdfPCell(new Phrase("")) { Border = borderStyle });
                    table.AddCell(new PdfPCell(new Phrase("")) { Border = borderStyle });
                    table.AddCell(new PdfPCell(new Phrase("")) { Border = borderStyle });
                }



                doc.Add(table);


                doc.Add(new Paragraph("\n"));

                // --- Totals Table (below items table)
                PdfPTable totalTbl = new PdfPTable(2);
                totalTbl.WidthPercentage = 40;
                totalTbl.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalTbl.SetWidths(new float[] { 1f, 1f });

                PdfPCell totalLbl = new PdfPCell(new Phrase("Total", smallBold));
                totalLbl.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalLbl.Padding = 6;
                totalLbl.BorderWidth = 1f;
                totalTbl.AddCell(totalLbl);

                PdfPCell totalVal = new PdfPCell(new Phrase(q.Subtotal.ToString("0.00"), smallBold));
                totalVal.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalVal.Padding = 6;
                totalVal.BorderWidth = 1f;
                totalTbl.AddCell(totalVal);

                doc.Add(totalTbl);


                doc.Close();
            }
        }

        // keep your LoadQuotationFromDb method same as before
       private static Quotation LoadQuotationFromDb(int id)
{
    var q = new Quotation();
    using (var con = DatabaseHelper.GetConnection())
    {
        con.Open();
        using (var cmd = con.CreateCommand())
        {
            cmd.CommandText = "SELECT QuotationNo, Date, CustomerName, Address, KindAttn, GSTNo, Terms, GSTPercent, Subtotal, GSTAmount, Total FROM Quotations WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            using (var rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    q.Id = id;
                    q.QuotationNo = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                    q.Date = rdr.IsDBNull(1) ? DateTime.Now : DateTime.Parse(rdr.GetString(1));
                    q.CustomerName = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                    q.Address = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                    q.KindAttn = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                    q.GSTNo = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                    q.Terms = rdr.IsDBNull(6) ? "" : rdr.GetString(6);
                 
                    q.Subtotal = (decimal)(rdr.IsDBNull(8) ? 0 : rdr.GetDouble(8));
                    
                }
            }
        }
        using (var cmd = con.CreateCommand())
        {
            cmd.CommandText = "SELECT SrNo, Particulars, Unit, Qty, Rate, Amount FROM QuotationItems WHERE QuotationId=@id ORDER BY SrNo";
            cmd.Parameters.AddWithValue("@id", id);
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var it = new QuotationItem()
                    {
                        SrNo = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0),
                        Particulars = rdr.IsDBNull(1) ? "" : rdr.GetString(1),
                        Unit = rdr.IsDBNull(2) ? "" : rdr.GetString(2),
                        Qty = (decimal)(rdr.IsDBNull(3) ? 0 : rdr.GetDouble(3)),
                        Rate = (decimal)(rdr.IsDBNull(4) ? 0 : rdr.GetDouble(4)),
                        Amount = (decimal)(rdr.IsDBNull(5) ? 0 : rdr.GetDouble(5))
                    };
                    q.Items.Add(it);
                }
            }
        }
    }
    return q;
}

    }
}
