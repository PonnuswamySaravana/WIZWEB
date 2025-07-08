using System;
using System.ComponentModel.DataAnnotations;


namespace WIZWEB.Models
{
    public class Customer
    {
        [Required]
        public string Customer_ID { get; set; }

        public string contact_name { get; set; }
        [Required]

        public string company_name { get; set; }
        [Required]
        public string contact_type { get; set; }

        public string phone { get; set; }

        public string Email { get; set; }

        public List<Registered_address> Registered_address { get; set; }

        public int  credit_limit { get; set; }

        public int payment_terms { get; set; }

        public string Currrency { get; set; }

        public string tax_reg_no { get; set; }


        public string country_code { get; set; }

        public string vat_treatment { get; set; }

        public string tax_treatment { get; set; }

        public string place_of_contact { get; set; }

        public string gst_no { get; set; }

        public string gst_treatment { get; set; }

        public string is_taxable { get; set; }

        public string Mobile { get; set; }

        public string Status { get; set; }

        public int IFACTCustomerid { get; set; }

        public string customer_type { get; set; }

        public string onboarding_country { get; set; }

    }
    public class Registered_address
    {
        [Required]
        public string street1 { get; set; }

        public string street2 { get; set; }

        public string state_code { get; set; }
        [Required]
        public string city { get; set; }

        public string state { get; set; }

        public string zip { get; set; }
        public string country { get; set; }

    }
}
