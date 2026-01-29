using QuotationApp.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuotationApp
{

    public static class PrintHelper
    {
        private static string GetSafeDateString(SqlDataReader rdr, int columnIndex)
        {
            if (rdr.IsDBNull(columnIndex))
                return "";

            var value = rdr.GetValue(columnIndex);

            if (value is DateTime dt)
                return dt.ToString("dd/MM/yyyy");

            return value.ToString();
        }

        /// <summary>
        /// SqlDataReader में मौजूद columnIndex पर date या string safe रूप में DateTime में बदलकर लौटाता है.
        /// </summary>
        private static DateTime GetSafeDateTime(SqlDataReader rdr, int columnIndex)
        {
            if (rdr.IsDBNull(columnIndex))
                return DateTime.Now;

            var value = rdr.GetValue(columnIndex);

            if (value is DateTime dt)
                return dt;

            if (DateTime.TryParse(value.ToString(), out DateTime parsed))
                return parsed;

            return DateTime.Now;
        }
        // Multi-page state
        private static int _currentIndex = 0;
        private static Quotation _cachedQuotation = null;
        private static int _currentQuotationId = -1;
        private static int _pageCount = 0;
        private const int MaxPages = 60; // safety guard to avoid infinite loops
        private static readonly object _printLock = new object();
        // Global variable add karo:
        private static int _currentPage = 0;
        private static int _totalPages = 0;

        private static string BuildSuggestedFileName(int id)
        {
            int qno = 0;
            string customer = "";

            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT QuotationNo, CustomerName FROM Quotations WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            if (!rdr.IsDBNull(0)) qno = rdr.GetInt32(0);
                            if (!rdr.IsDBNull(1)) customer = rdr.GetString(1) ?? "";
                        }
                    }
                }
            }

            if (qno <= 0) qno = id;
            if (string.IsNullOrWhiteSpace(customer)) customer = "Customer";

            string raw = $"Quotation_{qno}_{customer}".Trim();
            foreach (var ch in Path.GetInvalidFileNameChars()) raw = raw.Replace(ch, '_');
            raw = string.Join(" ", raw.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            if (raw.Length > 120) raw = raw.Substring(0, 120);
            return raw; // no extension
        }
        public static void PrintQuotationById(int id)
        {
            var doc = new PrintDocument();
            doc.DocumentName = BuildSuggestedFileName(id); // initial set

            // ✅ Force-set on every print pass (preview/actual)
            doc.BeginPrint += (s, e) =>
            {
                var pd = (PrintDocument)s;
                pd.DocumentName = BuildSuggestedFileName(_currentQuotationId > 0 ? _currentQuotationId : id);
            };

            doc.DocumentName = BuildSuggestedFileName(id);
            doc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            doc.DefaultPageSettings.Landscape = false;
            doc.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            _currentQuotationId = id;

            bool measuring = true;

            PrintEventHandler begin = (s, e) =>
            {
                lock (_printLock)
                {
                    _pageCount = 0;
                    _currentIndex = 0;
                    _cachedQuotation = null;
                    _currentPage = 0;
                }
            };

            PrintEventHandler end = (s, e) =>
            {
                if (!measuring)
                {
                    lock (_printLock)
                    {
                        _pageCount = 0;
                        _currentIndex = 0;
                        _cachedQuotation = null;
                        _currentQuotationId = -1;
                        _currentPage = 0;
                        _totalPages = 0;
                    }
                }
            };

            doc.BeginPrint += begin;
            doc.PrintPage += PrintPage;
            doc.EndPrint += end;

            try
            {
                // 1st pass (measure)
                doc.PrintController = new PreviewPrintController();
                doc.Print();

                _totalPages = _currentPage;

                // 2nd pass (actual)
                measuring = false;
                _currentPage = 0;
                doc.PrintController = new StandardPrintController();

                using (var pd = new PrintDialog())
                {
                    pd.Document = doc;
                    if (pd.ShowDialog() == DialogResult.OK)
                    {
                        doc.Print();
                    }
                }
            }
            finally
            {
                doc.BeginPrint -= begin;
                doc.EndPrint -= end;
                doc.PrintPage -= PrintPage;
            }
        }

        public static void PreviewQuotationById(int id)
        {
            var doc = new PrintDocument();
            doc.DocumentName = BuildSuggestedFileName(id);

            // ✅ Ensure name persists when user clicks Print inside preview
            doc.BeginPrint += (s, e) =>
            {
                var pd = (PrintDocument)s;
                pd.DocumentName = BuildSuggestedFileName(_currentQuotationId > 0 ? _currentQuotationId : id);
            };

            doc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            doc.DefaultPageSettings.Landscape = false;
            doc.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            _currentQuotationId = id;

            bool measuring = true;

            PrintEventHandler begin = (s, e) =>
            {
                lock (_printLock)
                {
                    _pageCount = 0;
                    _currentIndex = 0;
                    _cachedQuotation = null;
                    _currentPage = 0;
                }
            };

            PrintEventHandler end = (s, e) =>
            {
                if (!measuring)
                {
                    lock (_printLock)
                    {
                        _pageCount = 0;
                        _currentIndex = 0;
                        _cachedQuotation = null;
                        _currentQuotationId = -1;
                        _currentPage = 0;
                        _totalPages = 0;
                    }
                }
            };

            doc.BeginPrint += begin;
            doc.PrintPage += PrintPage;
            doc.EndPrint += end;

            try
            {
                // Measure
                doc.PrintController = new PreviewPrintController();
                doc.Print();
                _totalPages = _currentPage;

                // Show preview (user may click Print here)
                measuring = false;
                _currentPage = 0;
                doc.PrintController = new StandardPrintController();

                using (var preview = new PrintPreviewDialog())
                {
                    preview.Document = doc;
                    preview.WindowState = FormWindowState.Maximized;
                    preview.ShowDialog();
                }
            }
            finally
            {
                doc.BeginPrint -= begin;
                doc.EndPrint -= end;
                doc.PrintPage -= PrintPage;
            }
        }
        private static void DrawPageNumber(Graphics g, Rectangle bounds)
        {
            var totalStr = _totalPages > 0 ? _totalPages.ToString() : "?";
            string pageText = $"Page {_currentPage} of {totalStr}";
            using (var fontPage = new Font("Arial", 8, FontStyle.Regular))
            {
                SizeF sz = g.MeasureString(pageText, fontPage);
                float px = bounds.Right - sz.Width - 10;
                float py = bounds.Bottom - sz.Height - 4;
                g.DrawString(pageText, fontPage, Brushes.Black, px, py);
            }
        }
        private static string _lastSavedFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static void SaveToPdfById(int id)
        {
            try
            {
                // Build suggested file name using existing method
                string suggestedFileName = BuildSuggestedFileName(id);

                // Create SaveFileDialog
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Save Quotation as PDF";
                    saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveDialog.DefaultExt = ".pdf";
                    saveDialog.AddExtension = true;
                    saveDialog.FileName = suggestedFileName; // Pre-fill with suggested name
                                                             // Use the last saved folder path here
                    saveDialog.InitialDirectory = _lastSavedFolder;

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = saveDialog.FileName;

                        // Update last saved folder path
                        _lastSavedFolder = Path.GetDirectoryName(selectedPath) ?? _lastSavedFolder;

                        // Create PrintDocument for PDF generation
                        var doc = new PrintDocument();
                        doc.DocumentName = Path.GetFileNameWithoutExtension(selectedPath);
                        doc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
                        doc.DefaultPageSettings.Landscape = false;
                        doc.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

                        _currentQuotationId = id;

                        bool measuring = true;

                        PrintEventHandler begin = (s, e) =>
                        {
                            lock (_printLock)
                            {
                                _pageCount = 0;
                                _currentIndex = 0;
                                _cachedQuotation = null;
                                _currentPage = 0;
                            }
                        };

                        PrintEventHandler end = (s, e) =>
                        {
                            if (!measuring)
                            {
                                lock (_printLock)
                                {
                                    _pageCount = 0;
                                    _currentIndex = 0;
                                    _cachedQuotation = null;
                                    _currentQuotationId = -1;
                                    _currentPage = 0;
                                    _totalPages = 0;
                                }
                            }
                        };

                        doc.BeginPrint += begin;
                        doc.PrintPage += PrintPage;
                        doc.EndPrint += end;

                        // Set printer to "Microsoft Print to PDF"
                        doc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                        doc.PrinterSettings.PrintToFile = true;
                        doc.PrinterSettings.PrintFileName = selectedPath;

                        try
                        {
                            // First pass - measure pages
                            doc.PrintController = new PreviewPrintController();
                            doc.Print();
                            _totalPages = _currentPage;

                            // Second pass - actual print to PDF
                            measuring = false;
                            _currentPage = 0;
                            doc.PrintController = new StandardPrintController();
                            doc.Print();

                            MessageBox.Show($"PDF saved successfully!\n\nLocation: {selectedPath}",
                                          "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Optional: Ask user if they want to open the saved PDF
                            if (MessageBox.Show("Do you want to open the saved PDF file?",
                                              "Open PDF", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                                {
                                    FileName = selectedPath,
                                    UseShellExecute = true
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error saving PDF: {ex.Message}",
                                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            doc.BeginPrint -= begin;
                            doc.EndPrint -= end;
                            doc.PrintPage -= PrintPage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void DrawImageFit(Graphics g, Image img, Rectangle destRect)
        {
            // Calculate aspect ratio fit
            float ratioX = (float)destRect.Width / img.Width;
            float ratioY = (float)destRect.Height / img.Height;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(img.Width * ratio);
            int newHeight = (int)(img.Height * ratio);

            int posX = destRect.X + (destRect.Width - newWidth) / 2;
            int posY = destRect.Y + (destRect.Height - newHeight) / 2;

            g.DrawImage(img, posX, posY, newWidth, newHeight);
        }
        private static Image NormalizeImage(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            bmp.SetResolution(96, 96); // force standard DPI
            return bmp;
        }


        // Helper to use the same lock (keeps code consistent)
        private static object _print_lock_reflection() => _printLock;

        private static void PrintPage(object sender, PrintPageEventArgs e)
        {
            lock (_print_lock_reflection())
            {
                _pageCount++;
                _currentPage++;

                if (_pageCount > MaxPages)
                {
                    e.HasMorePages = false;
                    return;
                }

                if (_cachedQuotation == null || _cachedQuotation.Id != _currentQuotationId)
                    _cachedQuotation = LoadQuotationFromDb(_currentQuotationId);

                var q = _cachedQuotation ?? new Quotation();

                Graphics g = e.Graphics;
                var bounds = e.MarginBounds;
                int left = bounds.Left;
                int top = bounds.Top;
                int pageWidth = bounds.Width;
                int pageHeight = bounds.Height;

                // Fonts tuned to look like sample
                using (var fontTitle = new Font("Adobe Arabic", 55, FontStyle.Bold))
                using (var fontSubTitle = new Font("Arial", 11, FontStyle.Bold))
                using (var fontNormal = new Font("Arial", 10))
                using (var fontBold = new Font("Arial", 8, FontStyle.Bold))
                {
                    float itemFontSize = 9f;
                    using (var fontSmall = new Font("Arial", itemFontSize))
                    {
                        // measure lines
                        var terms = (q.Terms ?? "").Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        int lineH_normal = (int)Math.Ceiling(fontNormal.GetHeight(g)) + 2;
                        int lineH_small = (int)Math.Ceiling(fontSmall.GetHeight(g)) + 4;
                        int footerHeight = Math.Max(160, terms.Length * lineH_small + 120);

                        // CURSOR Y start
                        int curY = top;

                        // Decide whether to draw the full header (only on first page)
                        bool fullHeader = _currentPage == 1;

                        if (fullHeader)
                        {
                            // ------------------- LOGO & TITLE (only first page) -------------------
                            string logoPath = DatabaseHelper.GetSetting("LogoPath");
                            if (!string.IsNullOrEmpty(logoPath) && System.IO.File.Exists(logoPath))
                            {
                                try
                                {
                                    using (var raw = Image.FromFile(logoPath))
                                    using (var img = NormalizeImage(raw))
                                    {
                                        Rectangle logoRect = new Rectangle(left + 10, curY + 8, 167, 145);
                                        g.DrawImage(img, logoRect); // direct stretch, no crop
                                    }
                                }
                                catch { }
                            }

                            int logoW = 200, logoH = 140; // slightly smaller than before to fit well
                            int logoX = left + 10;
                            int logoY = curY + 8;
                            string company = " J.P. Electricals";
                            var titleSize = g.MeasureString(company, fontTitle);
                            float titleX = left + (pageWidth - titleSize.Width) / 2f;
                            if (titleX < logoX + logoW + 8) titleX = logoX + logoW + 8;

                            // 🟡 Main company name
                            g.DrawString(company, fontTitle, Brushes.DarkGoldenrod, titleX, curY + 46);

                            // 🟢 NEW LINE: tagline just below company name (center aligned)
                            string tagline = "                              Accurate - Efficient - Quality";
                            Font fontTagline = new Font("Adobe Arabic", 15, FontStyle.Italic); // aap chahe to fontTitle se chhota rakho
                            var taglineSize = g.MeasureString(tagline, fontTagline);
                            float taglineX = left + (pageWidth - taglineSize.Width) / 2f;
                            float taglineY = curY + 46 + titleSize.Height - 25; // -5 se thoda chipka hua lagega
                            g.DrawString(tagline, fontTagline, Brushes.Gray, taglineX, taglineY);

                            int usedHeaderH = Math.Max(logoY + logoH, curY + (int)Math.Ceiling(titleSize.Height) - 10);
                            curY = usedHeaderH + 6;


                            // ------------------- DESCRIPTION (centered) -------------------
                            string descLine1 = "We Undertake Repairing & Maintenance Work Of All Types O PLC Control, Automation, DC Work,";
                            string descLine2 = "Alternator Work, Electrical Equipments, Panel Work & Commissioning.";
                            var sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
                            RectangleF descRect = new RectangleF(left, curY, pageWidth, lineH_small + 2);
                            g.DrawString(descLine1, fontBold, Brushes.Black, descRect, sfCenter);
                            curY += lineH_small + 2;
                            descRect = new RectangleF(left, curY, pageWidth, lineH_small + 2);
                            g.DrawString(descLine2, fontBold, Brushes.Black, descRect, sfCenter);
                            curY += lineH_small + 10; // gap after description

                            // ------------------- ADDRESS BOX -------------------
                            int addressBoxH = 24;
                            int boxWidth = Math.Min(720, pageWidth);
                            g.DrawRectangle(Pens.Black, left, curY, boxWidth, addressBoxH);
                            g.DrawString("Address:- J.P.Electricas, 10, Radhakrishna Nagar, Near Rajiv Nagar Bus Stop, Hingna Road, Nagpur. Mob.9822699075,",
                                         fontBold, Brushes.Black, left + 6, curY + 5);
                            curY += addressBoxH + 14; // gap after address

                            // ------------------- QUOTATION HEADER BAR -------------------
                            int qBoxW = boxWidth;
                            int qBoxH = 24;
                            g.FillRectangle(Brushes.LawnGreen, left, curY, qBoxW, qBoxH);
                            g.DrawRectangle(Pens.Black, left, curY, qBoxW, qBoxH);
                            g.DrawString("QUOTATION", fontSubTitle, Brushes.Black, left + (qBoxW / 2) - 36, curY + 5);
                            curY += qBoxH + 14; // gap after quotation header

                            // ------------------- CUSTOMER & QUOTE DETAILS -------------------
                            int custBoxWidth = Math.Min(400, (int)(pageWidth * 0.58));
                            int rightBoxWidth = Math.Min(pageWidth - custBoxWidth - 12, 260);
                            int rowHeight = 22;

                            // Row 1 (To)
                            var custRow1 = new Rectangle(left, curY, custBoxWidth, rowHeight);
                            var qtRow1 = new Rectangle(custRow1.Right + 57, curY, rightBoxWidth, rowHeight);
                            g.DrawRectangle(Pens.Black, custRow1);
                            g.DrawRectangle(Pens.Black, qtRow1);
                            g.DrawString("To,", fontNormal, Brushes.Black, custRow1.Left + 6, custRow1.Top + 4);
                            g.DrawString($"Quotation No.: {q.QuotationNo}", fontNormal, Brushes.Black, qtRow1.Left + 6, qtRow1.Top + 4);
                            curY += rowHeight;

                            // Row 2 (CustomerName + Date)
                            var custRow2 = new Rectangle(left, curY, custBoxWidth, rowHeight);
                            var qtRow2 = new Rectangle(custRow2.Right + 57, curY, rightBoxWidth, rowHeight);
                            g.DrawRectangle(Pens.Black, custRow2);
                            g.DrawRectangle(Pens.Black, qtRow2);
                            g.DrawString(q.CustomerName ?? "", fontNormal, Brushes.Black, custRow2.Left + 6, custRow2.Top + 4);
                            g.DrawString($"Date: {q.Date:dd.MM.yyyy}", fontNormal, Brushes.Black, qtRow2.Left + 6, qtRow2.Top + 4);
                            curY += rowHeight;

                            // Row 3 (Address + Enquiry) – can expand
                            string addrText = q.Address ?? "";
                            int addrBoxWidth = custBoxWidth - 12;
                            StringFormat fmtWrapAddr = new StringFormat
                            {
                                Alignment = StringAlignment.Near,
                                LineAlignment = StringAlignment.Near,
                                FormatFlags = StringFormatFlags.LineLimit,
                                Trimming = StringTrimming.Word
                            };

                            // Measure first row height for address
                            SizeF addrSize = g.MeasureString(addrText, fontNormal, addrBoxWidth, fmtWrapAddr);
                            int addrNeeded = (int)Math.Ceiling(addrSize.Height);

                            // First row (fixed height)
                            var custRow3 = new Rectangle(left, curY, custBoxWidth, rowHeight);
                            var qtRow3 = new Rectangle(custRow3.Right + 57, curY, rightBoxWidth, rowHeight);
                            g.DrawRectangle(Pens.Black, custRow3);
                            g.DrawRectangle(Pens.Black, qtRow3);

                            // Draw part of address that fits in first row
                            int charsFitted, linesFilled;
                            g.MeasureString(addrText, fontNormal, new SizeF(addrBoxWidth, rowHeight), fmtWrapAddr,
                                            out charsFitted, out linesFilled);
                            string visible = addrText.Substring(0, charsFitted);
                            string overflow = (charsFitted < addrText.Length) ? addrText.Substring(charsFitted).Trim() : "";
                            g.DrawString(visible, fontNormal, Brushes.Black,
                                         new RectangleF(custRow3.Left + 6, custRow3.Top + 4, addrBoxWidth, rowHeight - 4), fmtWrapAddr);
                            g.DrawString($"Enquiry: {q.Enquiry}", fontNormal, Brushes.Black, qtRow3.Left + 6, qtRow3.Top + 4);
                            curY += rowHeight;

                            // Extra rows if address overflow
                            while (!string.IsNullOrEmpty(overflow))
                            {
                                g.MeasureString(overflow, fontNormal, new SizeF(addrBoxWidth, rowHeight), fmtWrapAddr,
                                                out charsFitted, out linesFilled);
                                string lineText = overflow.Substring(0, charsFitted);
                                overflow = (charsFitted < overflow.Length) ? overflow.Substring(charsFitted).Trim() : "";

                                var custExtra = new Rectangle(left, curY, custBoxWidth, rowHeight);
                                var qtExtra = new Rectangle(custExtra.Right + 57, curY, rightBoxWidth, rowHeight);
                                g.DrawRectangle(Pens.Black, custExtra);
                                g.DrawRectangle(Pens.Black, qtExtra);

                                g.DrawString(lineText, fontNormal, Brushes.Black,
                                             new RectangleF(custExtra.Left + 6, custExtra.Top + 4, addrBoxWidth, rowHeight - 4), fmtWrapAddr);

                                curY += rowHeight;
                            }

                            // Gap after blocks
                            curY += 14;


                            // ------------------- KIND ATTN BAR -------------------
                            int kindH = 24;
                            int kindW = qBoxW;
                            g.FillRectangle(Brushes.LawnGreen, left, curY, kindW, kindH);
                            g.DrawRectangle(Pens.Black, left, curY, kindW, kindH);
                            string kindText = $"Kind Attn.: {q.KindAttn}";
                            SizeF kindSize = g.MeasureString(kindText, fontBold);
                            float kindX = left + (kindW - kindSize.Width) / 2f;
                            float kindY = curY + (kindH - kindSize.Height) / 2f;
                            g.DrawString(kindText, fontSubTitle, Brushes.Black, kindX, kindY);

                            curY += kindH + 14; // gap before table
                        }
                        else
                        {
                            // Subsequent pages: keep a small top gap and optionally show a "Continued" hint.
                            g.DrawString("Continued...", fontBold, Brushes.Black, left, curY);
                            curY += lineH_small + 6;
                        }

                        // ------------------- TABLE AREA -------------------
                        int tableLeft = left;
                        int tableWidth = Math.Min(720, pageWidth);
                        int tableTop = curY;
                        int tableRight = tableLeft + tableWidth;
                        int tableBottom = bounds.Bottom - footerHeight - 10;

                        if (tableBottom <= tableTop + 10)
                        {
                            e.HasMorePages = false;
                            return;
                        }

                        StringFormat fmtCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                        // Columns: Sr.No | Particulars | Unit | Qty | Rate | Amount
                        int col1 = tableLeft;                      // Sr.No narrow col
                        int col2 = col1 + 45;                      // Particulars (wide)
                        int col3 = col2 + 415;                     // Unit
                        int col4 = col3 + 50;                      // Qty
                        int col5 = col4 + 50;                      // Rate
                        int col6 = col5 + 65;                      // Amount (rest)
                        int[] cols = { col1, col2, col3, col4, col5, col6, tableRight };

                        // header (column headings) — always draw on every page
                        int headerH = 24;
                        var headerRect = new Rectangle(tableLeft, tableTop, tableWidth, headerH);
                        g.FillRectangle(Brushes.White, headerRect);
                        g.DrawRectangle(Pens.Black, headerRect);

                        // vertical column lines
                        for (int i = 0; i < cols.Length; i++)
                            g.DrawLine(Pens.Black, cols[i], tableTop, cols[i], tableBottom);

                        // Draw the column headings
                        g.DrawString("Sr.No", fontBold, Brushes.Black,
                            new RectangleF(col1, tableTop, col2 - col1, headerH), fmtCenter);
                        g.DrawString("Particulars", fontBold, Brushes.Black,
                            new RectangleF(col2, tableTop, col3 - col2, headerH), fmtCenter);
                        g.DrawString("Unit", fontBold, Brushes.Black,
                            new RectangleF(col3, tableTop, col4 - col3, headerH), fmtCenter);
                        g.DrawString("Qty", fontBold, Brushes.Black,
                           new RectangleF(col4, tableTop, col5 - col4, headerH), fmtCenter);
                        g.DrawString("Rate", fontBold, Brushes.Black,
                            new RectangleF(col5, tableTop, col6 - col5, headerH), fmtCenter);
                        g.DrawString("Amount", fontBold, Brushes.Black,
                            new RectangleF(col6, tableTop, tableRight - col6, headerH), fmtCenter);

                        // wrapping format for Particulars
                        var fmtWrap = new StringFormat
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Near,
                            FormatFlags = StringFormatFlags.LineLimit,
                            Trimming = StringTrimming.Word
                        };

                        // Compute how many rows fit (rough)
                        int rowH = (int)Math.Ceiling(fontSmall.GetHeight(g)) + 8;
                        int rowsCanFit = (tableBottom - (tableTop + headerH)) / rowH;
                        if (rowsCanFit <= 0)
                        {
                            e.HasMorePages = false;
                            return;
                        }

                        // Draw rows with dynamic height & wrapping
                        int y = tableTop + headerH;
                        while (_currentIndex < q.Items.Count)
                        {
                            var it = q.Items[_currentIndex];

                            // particular column width inside padding
                            float col2Width = (col3 - col2) - 10;
                            SizeF partSize = g.MeasureString(it.Particulars ?? "", fontSmall, (int)col2Width, fmtWrap);
                            int rowHCurrent = (int)Math.Ceiling(partSize.Height) + 8;

                            // If doesn't fit on this page => new page
                            if (y + rowHCurrent > tableBottom)
                            {
                                // draw bottom separator for the table on this page
                                g.DrawLine(Pens.Black, tableLeft, tableBottom, tableRight, tableBottom);
                                DrawPageNumber(g, bounds);

                                e.HasMorePages = true;
                                return;
                            }

                            // Draw row contents
                            g.DrawString(it.SrNo.ToString(), fontSmall, Brushes.Black, col1 + 6, y + 4);
                            g.DrawString(it.Particulars ?? "", fontSmall, Brushes.Black,
                                         new RectangleF(col2 + 6, y + 4, col2Width, rowHCurrent), fmtWrap);
                            g.DrawString(it.Unit ?? "", fontSmall, Brushes.Black, col3 + 6, y + 4);
                            g.DrawString(it.Qty.ToString("0"), fontSmall, Brushes.Black, col4 + 6, y + 4);
                            g.DrawString(it.Rate.ToString("0.00"), fontSmall, Brushes.Black, col5 + 6, y + 4);
                            g.DrawString(it.Amount.ToString("0.00"), fontSmall, Brushes.Black, col6 + 6, y + 4);

                            // Advance
                            y += rowHCurrent;
                            _currentIndex++;
                        }

                        // All items printed on this (final) page: draw totals + footer
                        decimal total = q.Items.Sum(it => it.Amount);


                        // total row + footer
                        int totalRowH = (int)Math.Ceiling(fontSmall.GetHeight(g)) + 8;

                        // ✅ पहले RGP या Challan block reserve करो (जो भी available हो)
                        string rgpLine1 = "";
                        string rgpLine2 = "";

                        int lineH = (int)Math.Ceiling(fontNormal.GetHeight(g)) + 4;

                        // ✅ अगर RGP data hai — use karo
                        if (!string.IsNullOrWhiteSpace(q.RGPNo) || !string.IsNullOrWhiteSpace(q.RGPDate))
                        {
                            rgpLine1 = !string.IsNullOrWhiteSpace(q.RGPNo) ? "RGP No.: " + q.RGPNo : "";
                            rgpLine2 = !string.IsNullOrWhiteSpace(q.RGPDate) ? "RGP Date: " + q.RGPDate : "";
                        }
                        // ✅ अगर RGP blank hai aur Challan available hai — use karo
                        else if (!string.IsNullOrWhiteSpace(q.ChallanNo) || !string.IsNullOrWhiteSpace(q.ChallanDate))
                        {
                            rgpLine1 = !string.IsNullOrWhiteSpace(q.ChallanNo) ? "Challan No.: " + q.ChallanNo : "";
                            rgpLine2 = !string.IsNullOrWhiteSpace(q.ChallanDate) ? "Challan Date: " + q.ChallanDate : "";
                        }

                        // ✅ Height calculate karo (sirf agar kuch print hona hai to)
                        int rgpBlockH = 0;
                        if (!string.IsNullOrWhiteSpace(rgpLine1) || !string.IsNullOrWhiteSpace(rgpLine2))
                        {
                            rgpBlockH = string.IsNullOrWhiteSpace(rgpLine2) ? lineH : lineH * 2;
                        }

                        // ✅ RGP/Challan block position
                        int totalRowTop = tableBottom - totalRowH;
                        int rgpRowTop = totalRowTop - rgpBlockH; // total row ke upar

                        // ✅ Print only if kuch content hai
                        if (rgpBlockH > 0)
                        {
                            float textX = col2 + 6;
                            float textY = rgpRowTop + 4;

                            if (!string.IsNullOrWhiteSpace(rgpLine1))
                            {
                                g.DrawString(rgpLine1, fontNormal, Brushes.Black, textX, textY);
                                textY += lineH;
                            }
                            if (!string.IsNullOrWhiteSpace(rgpLine2))
                            {
                                g.DrawString(rgpLine2, fontNormal, Brushes.Black, textX, textY);
                            }
                        }


                        // Draw total row
                        int totalLeft = col5;
                        int totalRight = tableRight;
                        g.FillRectangle(Brushes.White,
                            new Rectangle(totalLeft, totalRowTop, totalRight - totalLeft, totalRowH));
                        g.DrawRectangle(Pens.Black,
                            totalLeft, totalRowTop, totalRight - totalLeft, totalRowH);
                        g.DrawLine(Pens.Black, col6, totalRowTop, col6, totalRowTop + totalRowH);

                        g.DrawString("Total :", fontBold, Brushes.Black, col5 + 12, totalRowTop + 6);
                        string totText = total.ToString("0.00");
                        SizeF totSize = g.MeasureString(totText, fontBold);
                        g.DrawString(totText, fontBold, Brushes.Black,
                            totalRight - totSize.Width - 8, totalRowTop + 6);



                        // Border line
                        g.DrawLine(Pens.Black, tableLeft, tableBottom, tableRight, tableBottom);



                        // Footer (draw on every page)
                        DrawFooter(g, q, bounds, footerHeight, fontNormal, fontSmall, fontBold);

                        DrawPageNumber(g, bounds);

                        // STOP: no more pages remaining
                        e.HasMorePages = false;
                    } // using fontSmall
                } // using fonts
            } // lock
        }

        // Footer draws at bottom; keeps spacing safe
        private static void DrawFooter(Graphics g, Quotation q, Rectangle bounds, int footerHeight, Font fontNormal, Font fontSmall, Font fontBold)
        {
            int left = bounds.Left;
            int bottom = bounds.Bottom;
            int footerTop = bottom - footerHeight + 8;

            // Left side (GST + Terms)
            int gstBoxW = 300;
            int gstBoxH = 22;

            // --- GST Box ---
            g.FillRectangle(Brushes.LawnGreen, left, footerTop, gstBoxW, gstBoxH);
            g.DrawRectangle(Pens.Black, left, footerTop, gstBoxW, gstBoxH);
            g.DrawString($"GSTN No.: {q.GSTNo}", fontBold, Brushes.Black, left + 6, footerTop + 3);

            // --- Terms & Conditions ---
            int termsTop = footerTop + gstBoxH + 6;
            g.DrawString("Terms & Conditions:", fontBold, Brushes.Black, left, termsTop);

            termsTop += (int)Math.Ceiling(fontNormal.GetHeight(g)) + 4;
            var terms = (q.Terms ?? "").Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var t in terms)
            {
                g.DrawString(t.Trim(), fontSmall, Brushes.Black, left + 10, termsTop);
                termsTop += (int)Math.Ceiling(fontSmall.GetHeight(g)) + 2;
            }

            // Right side (Signature block)
            int blockWidth = 160;
            int baseX = bounds.Right - blockWidth;
            int sigTop = footerTop;

            g.DrawString("For J.P. ELECTRICALS", fontBold, Brushes.Black, baseX, sigTop);

            string sig = DatabaseHelper.GetSetting("SignaturePath");
            if (!string.IsNullOrEmpty(sig) && System.IO.File.Exists(sig))
            {
                try
                {
                    using (var raw = Image.FromFile(sig))
                    using (var img = NormalizeImage(raw))
                    {
                        Rectangle sigRect = new Rectangle(baseX, sigTop + 18, 125, 100);
                        g.DrawImage(img, sigRect);
                    }
                }
                catch { }
            }

            g.DrawString("Authorized Signatory", fontBold, Brushes.Black, baseX, sigTop + 115);


        }


        // unchanged - load from DB (kept exactly as before)
        private static Quotation LoadQuotationFromDb(int id)
        {
            var q = new Quotation();
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand()) // ✅ SqlCommand explicitly use करें
                {
                    cmd.Connection = con;
                    cmd.CommandText = @"SELECT QuotationNo, Date, Enquiry, CustomerName, Address, KindAttn, GSTNo, Terms, Subtotal, RGPNo, RGPDate, ChallanNo, ChallanDate 
                FROM Quotations 
                WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            q.Id = id;
                            q.QuotationNo = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);

                            // ✅ MAIN FIX: DateTime column को properly handle करें
                            q.Date = GetSafeDateTime(rdr, 1);

                            q.Enquiry = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                            q.CustomerName = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                            q.Address = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                            q.KindAttn = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                            q.GSTNo = rdr.IsDBNull(6) ? "" : rdr.GetString(6);
                            q.Terms = rdr.IsDBNull(7) ? "" : rdr.GetString(7);

                            q.Subtotal = 0;
                            if (!rdr.IsDBNull(8))
                            {
                                object val = rdr.GetValue(8);
                                q.Subtotal = Convert.ToDecimal(val);
                            }

                            q.RGPNo = rdr.IsDBNull(9) ? "" : rdr.GetString(9);

                            q.RGPDate = GetSafeDateString(rdr, 10);

                            q.ChallanNo = rdr.IsDBNull(11) ? "" : rdr.GetString(11);


                            q.ChallanDate = GetSafeDateString(rdr, 12);


                        }
                    }
                }

            
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;
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
                                Qty = rdr.IsDBNull(3) ? 0 : Convert.ToDecimal(rdr.GetValue(3)),
                                Rate = rdr.IsDBNull(4) ? 0 : Convert.ToDecimal(rdr.GetValue(4)),
                                Amount = rdr.IsDBNull(5) ? 0 : Convert.ToDecimal(rdr.GetValue(5)),
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
