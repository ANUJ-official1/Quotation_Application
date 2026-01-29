using System;
using System.Collections.Generic;

namespace QuotationApp
{
    public class Quotation
    {
        public int Id { get; set; }
        public int QuotationNo { get; set; }
        public DateTime Date { get; set; }
        public string Enquiry { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string KindAttn { get; set; }
        public string GSTNo { get; set; }
        public string Terms { get; set; }

        public decimal Subtotal { get; set; }
        public string RGPNo { get; set; }
        public string RGPDate { get; set; }
        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }


        public List<QuotationItem> Items { get; set; } = new List<QuotationItem>();
    }

    public class QuotationItem
    {
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public int SrNo { get; set; }
        public string Particulars { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
}
