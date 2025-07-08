using System.ComponentModel.DataAnnotations;

namespace WIZWEB.Models
{
    public class app
    {
        public DateTime voudate { get; set; }
        public string officllocation { get; set; }
        public string invoice_number { get; set; }
        public string voutype { get; set; }
        public int refno { get; set; }
    }

    public class Estimatesapp
    {

        public string customer_id { get; set; }
        public string customer_type { get; set; }
        public string officllocation { get; set; }
        [Required]
        public string vou_number { get; set; }
        [Required]
        public string voutype { get; set; }

        public string reign_id { get; set; }
        public string Ref_invoice_number { get; set; }
        public string Ref_voutype { get; set; }
        public string place_of_supply { get; set; }
        public string vat_treatment { get; set; }
        public string tax_treatment { get; set; }
        public string gst_treatment { get; set; }
        public string Wiz_gst_no { get; set; }
        [Required]
        public string Blno { get; set; }

        public string reference_number { get; set; }
        public DateTime WizBookingdate { get; set; }
        public long template_id { get; set; }
        public string billtocustomerid { get; set; }
        [Required]
        public string BillToName { get; set; }

        public string BillToAddress { get; set; }
        public string BilltoGST { get; set; }
        public string Billtolocation { get; set; }
        public string supplytocustomerid { get; set; }
        [Required]
        public string SupplyToName { get; set; }

        public string SupplyToAddress { get; set; }
        public string SupplytoGST { get; set; }
        public string SupplyTolocation { get; set; }
        public string pol { get; set; }
        public string pod { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime date { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        public string payment_terms { get; set; }
        public string Product { get; set; }
        public string? discount1 { get; set; }
        public string is_discount_before_tax { get; set; }
        public string is_inclusive_tax { get; set; }
        public string exchange_rate { get; set; }
        public string notes { get; set; }
        public string terms { get; set; }





        public List<line_items1> line_items { get; set; }


        public string MBL { get; set; }
        public string FreightTerms { get; set; }
        public string VesselFlight { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Line { get; set; }
        public string IGM { get; set; }
        public string Againt_Vou_No { get; set; }
        public DateTime Againt_Voudate { get; set; }
        public string Againt_voutype { get; set; }
        public string Againt_officllocation { get; set; }
        public string Containerid { get; set; }
        //public string  Billtolocation { get; set; }
        //public string SupplyTolocation { get; set; }

        //public List<line_items> item_id { get; set; }
        //public List<line_items> product_type { get; set; }
        //public List<line_items> hsn_or_sac { get; set; }
        //public List<line_items> name { get; set; }
        //public List<line_items> description { get; set; }
        //public List<line_items> item_order { get; set; }
        //public List<line_items> bcy_rate { get; set; }
        //public List<line_items> Currency { get; set; }
        //public List<line_items> rate { get; set; }
        //public List<line_items> quantity { get; set; }
        //public List<line_items> unit { get; set; }
        //public List<line_items> Amount { get; set; }
        //public List<line_items> discount_amount { get; set; }
        //public List<line_items> discount { get; set; }
        //public List<line_items> tax_id { get; set; }
        //public List<line_items> tax_name { get; set; }
        //public List<line_items> tax_type { get; set; }
        //public List<line_items> tax_percentage { get; set; }
        //public List<line_items> tax_treatment_code { get; set; }
        //public List<line_items> header_name { get; set; }

        public string? ChargeId { get; set; }
        public string? Ex_Rate { get; set; }
        public double Tax_Amount { get; set; }
        public string? VendorInvoiceUpload { get; set; }

        // added on 17Nov2023
        public string GoodDescription { get; set; }
        public string Vol_Wei_Pack { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }

        public string region_currency_exchange_rate { get; set; }
        public string base_currency { get; set; }

        public bool ica_invoice { get; set; }

    }
    public class line_items1
    {
        public string item_id { get; set; }
        public string product_type { get; set; }
        public string hsn_or_sac { get; set; }
        public string chargeid { get; set; }
        [Required]
        public string name { get; set; }

        public string description { get; set; }
        public int item_order { get; set; }
        public double bcy_rate { get; set; }
        public string Currency { get; set; }
        public double rate { get; set; }
        public double Ex_Rate { get; set; }
        public double quantity { get; set; }
        public string unit { get; set; }
        public double Amount { get; set; }
        public double discount_amount { get; set; }
        public double discount { get; set; }
        public string tax_id { get; set; }
        public string tax_name { get; set; }
        public string tax_type { get; set; }
        public double tax_percentage { get; set; }
        public string tax_treatment_code { get; set; }
        public double Tax_Amount { get; set; }
        public string header_name { get; set; }
    }
}
