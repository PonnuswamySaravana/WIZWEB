using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using WIZWEB.DAL;
using WIZWEB.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using Azure;
using WIZWEB.Methods;
using WIZWEB.Get_Functions;
using System.Diagnostics.Eventing.Reader;
using WIZWEB.Model;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection.Metadata;
using System.Net;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using System.Reflection;
using FluentFTP.Exceptions;
using FluentFTP;



namespace WIZWEB.Controllers
{
    [ApiController]
    [Route("api/Customer")]
    public class Tempcontroller : ControllerBase
    {
        string output1;
        static string Subbase64String;
        string CustomerType, eivtype;
        string resp;
        string VouType, Containerid, Product, str_FADbname, str_FAYear;
        DateTime voudate1;
        string org, prifx, sufix;
        int con1;
        string vt;
        int invoicenoapp, vouyear1;
        string vouty, voustr, ctype;
        string st;
        string div_id;
        string json1;
        string custid1ung, CHKCUS;
        string VoucherNo = "";
        int cntrycod, divcnt;
        string branchname1 = "";
        string CustomerDetails;
        string htmlUrl;
        string FADbname123 = "iFACTFA2223";
        string branchname = "CHENNAI";
        string divisionnamereport = "WIZ LOGTEC INDIA PRIVATE LIMITED ";
        string divisionname = "WIZ";
        string trantype = "OT";
        int bid1 = 1;
        int cid1 = 1;
        string username = "RAJAKUMARAN. P";
        int empid = 0357;
        string Filename = "";
        string responseMessage;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt_noof1 = new DataTable();



        [HttpPost("InsertCustDetails")]
        public ActionResult<string> Insertcust([FromBody] Customer data)
        {
            using SqlConnection con = BAL.GetSqlconnection();

            try
            {
                //LoadData loaddata = new LoadData();
                BAL Sqlhelper = new BAL();
                object CustData = null;
               
                if (data!=null)
                {
                    string details = JsonSerializer.Serialize(data);

                    
                    string quary1 = "Sp_MagikwebAPIRequest";
                    using(SqlCommand cmd= new SqlCommand(quary1,con))
                    {
                         CustomerDetails = "CU";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@APIRequest", details);
                        cmd.Parameters.AddWithValue("@APIRequesttype", CustomerDetails);
                        con.Open();
                        int n = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                       
                }
                if (ModelState.IsValid)
                {
                    string datadetails = "[" + data + "]";
                    datadetails.Replace("null", "");

                    CustomerType = data.contact_type switch
                    {
                        "CU" or "VD" => "C",
                        "AG" or "AC" => "P"
                    };

                    string quary2 = "Sp_WIZMASTERCUSTOMERNew";
                    using (SqlCommand cmd=new SqlCommand(quary2,con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", data.Customer_ID);
                        cmd.Parameters.AddWithValue("@ContactName", data.contact_name);
                        cmd.Parameters.AddWithValue("@CompanyName", data.company_name);
                        cmd.Parameters.AddWithValue("@Contacttype", data.contact_type);
                        cmd.Parameters.AddWithValue("@Phone", data.phone);
                        cmd.Parameters.AddWithValue("@Email", data.Email);
                        cmd.Parameters.AddWithValue("@Street1", data.Registered_address[0].street1);
                        cmd.Parameters.AddWithValue("@Street2", data.Registered_address[0].street2);
                        cmd.Parameters.AddWithValue("@Statecode", data.Registered_address[0].state_code);
                        cmd.Parameters.AddWithValue("@City", data.Registered_address[0].city);
                        cmd.Parameters.AddWithValue("@State", data.Registered_address[0].state);
                        cmd.Parameters.AddWithValue("@Zip", data.Registered_address[0].zip);
                        cmd.Parameters.AddWithValue("@Country", data.Registered_address[0].country);
                        cmd.Parameters.AddWithValue("@Creditlimit", data.credit_limit);
                        cmd.Parameters.AddWithValue("@Paymentterms", data.payment_terms);
                        cmd.Parameters.AddWithValue("@Currency", data.Currrency);
                        cmd.Parameters.AddWithValue("@VATaxregistrationNumber", data.tax_reg_no);
                        cmd.Parameters.AddWithValue("@CountryCode", data.country_code);
                        cmd.Parameters.AddWithValue("@VATTreatment", data.vat_treatment);
                        cmd.Parameters.AddWithValue("@TAXTreatment", data.tax_treatment);
                        cmd.Parameters.AddWithValue("@StatecodeGST", data.place_of_contact);
                        cmd.Parameters.AddWithValue("@GSTNumber", data.gst_no);
                        cmd.Parameters.AddWithValue("@GSTTreatment", data.gst_treatment);
                        cmd.Parameters.AddWithValue("@IsTaxable", data.is_taxable);
                        cmd.Parameters.AddWithValue("@customertype", CustomerType);
                        cmd.Parameters.AddWithValue("@Status", data.Status);
                        cmd.Parameters.AddWithValue("@IFACTCustomerid", data.IFACTCustomerid);
                        cmd.Parameters.AddWithValue("@Mobile", data.Mobile);
                        cmd.Parameters.AddWithValue("@customer_type", data.customer_type);
                        cmd.Parameters.AddWithValue("@onboarding_country", data.onboarding_country);
                        SqlParameter output1 = new SqlParameter("@eid", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(output1);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        string output = output1.Value?.ToString();
                        if (output != null)
                        {
                            string MailSubject = string.Empty;
                            string MailBody = "We have received the JSON file for the subject data.";
                            //loaddata.Mail("info@ifact.co.in", "subin.balakrishnan@ltsolutions.co.in", MailSubject, MailBody, Attachments, "inFACT87654@", "", "nambi.chandran@ltsolutions.co.in;haribalaji.ramesh@ltsolutions.co.in");
                            switch (data.contact_type)
                            {
                                case "CU": MailSubject = "Customer - " + data.company_name + " created on " + DateTime.Now; break;
                                case "VD": MailSubject = "Customer - " + data.company_name + " created on " + DateTime.Now; break;
                                case "AG": MailSubject = "Customer - " + data.company_name + " created on " + DateTime.Now; break;
                                case "AC": MailSubject = "Customer - " + data.company_name + " created on " + DateTime.Now; break;
                            }
                            string CustJson = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

                            if (data.Status== "CREATE")
                            {
                                st = "Customer Details Added Successfully";
                            }
                            else
                            {
                                st = "Customer Details Update Successfully";
                            }
                             CustData = new
                            {
                                Status = st,
                                Customer_ID = data.Customer_ID,
                                company_name = data.company_name,
                                contact_type= data.contact_type,
                                IFACTCustomerid=output
                            };
                        }
                        else
                        {
                            string  Responce = "Insert Failed";
                            Method.Insertresponse(Responce, CustomerDetails,Convert.ToInt32(output), 0, "Insert Failed");
                            return StatusCode(StatusCodes.Status500InternalServerError, Responce);

                        }

                    }

                }
                else
                {
                    string Responce = ModelState.ToString();
                    Method.Insertresponse(Responce, CustomerDetails, 0, 0, "Invaild Json");
                    return BadRequest(ModelState);

                }
                return Ok(CustData);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                string responce = msg;
                Method.Insertresponse(responce, "", 0, 0, msg);
                return StatusCode(StatusCodes.Status417ExpectationFailed, msg);

            }
            
        }

        [HttpPost("InsEstimatesDetails")]
        public ActionResult<string> estimatedata([FromBody] Estimates Getdata)
        {
            SqlConnection con = BAL.GetSqlconnection();
            BAL SqlHelper = new BAL();
            try
            {

                
                if (Getdata != null)
                {
                    BAL Sqlhelper = new BAL();
                    
                    string CustomerDetails = "ES";
                    string details = JsonSerializer.Serialize(Getdata);

                    
                    string quary = "Sp_MagikwebAPIRequest";
                    var parameters = new Dictionary<string, object>
                    {
                        {"@APIRequest", details},
                        {"@APIRequesttype", CustomerDetails }
                    };
                     bool success = SqlHelper.ExecuteNonQuery(quary, parameters);

                    //using (SqlCommand cmd= new SqlCommand(quary,con))
                    //{
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@APIRequest", details);
                    //    cmd.Parameters.AddWithValue("@APIRequesttype", CustomerDetails);
                    //    con.Open();
                    //    cmd.ExecuteNonQuery();
                    //    con.Close();
                    //}
                }
                if (ModelState.IsValid)
                {
                    voudate1 = Convert.ToDateTime(Getdata.date);

                    if (voudate1.Month < 4)
                    {
                        vouyear1 = Convert.ToInt32((voudate1.Year - 1).ToString());
                    }
                    else
                    {
                        vouyear1 = Convert.ToInt32((voudate1.Year).ToString());
                    }
                    org = vouyear1.ToString();
                    prifx = org.Substring(2);
                    con1 = Convert.ToInt32(prifx) + 1;
                    str_FAYear = prifx.ToString() + con1.ToString();
                    str_FADbname = "iFACTFA" + str_FAYear;

                    ctype = "C";
                    VouType = Getdata.voutype;
                    Containerid = Getdata.Containerid;
                    Product = Getdata.Product;

                    string quary2 = "sp_bid_chk";
                    var parameter1 = new Dictionary<string, object>
                    {
                        {"@gst",Getdata.Wiz_gst_no}
                    };
                    DataTable dat1 = SqlHelper.ExecuteTable1(quary2, parameter1);


                    if (dat1.Rows.Count == 0)
                    {
                         responseMessage = "Supply to customer GST No Is Incorrect. Please provide a correct GST NO";
                        Method.Insertresponse(responseMessage, CustomerDetails, Getdata.vou_number, Convert.ToInt32(Getdata.officllocation), 0, 0, responseMessage);
                        return BadRequest(new
                        {
                            Status = "Error",
                            Message = responseMessage
                        });
                    }

                    VouType = VouType switch
                    {
                        null or "" => "",
                        "Invoice" => "IN",
                        "Bill of Supply" => "BS",
                        "Reimbursement Note" => "RN",
                        "Vendor Invoice" => "VI",
                        "OSDN" => "OSDN",
                        "OSCN" => "OSCN",
                        "OTDN" => "OTDN",
                        "OTCN" => "OTCN",
                        _ => VouType
                    };

                    string quary3 = "cntrychka_sp";
                    DataTable cntry = new DataTable();
                    var parameter3 = new Dictionary<string, object>
                          {
                              {"@bid", Getdata.officllocation }
                          };
                    cntry = SqlHelper.ExecuteTable1(quary3, parameter3);
                    if (Getdata.region_currency_exchange_rate == "23" && string.IsNullOrWhiteSpace(Getdata.region_currency_exchange_rate))
                    {
                        string errorMessage = "Region_Currency_Exchange_Rate_is_Empty";
                        Method.Insertresponse(resp, CustomerDetails, Getdata.vou_number, Convert.ToInt32(Getdata.officllocation), 0, 0, errorMessage);
                        BadRequest(errorMessage);
                    }
                    if (cntrycod == 102)
                    {
                        string cust = "sp_customerCHK";
                        DataTable dqbidcust1 = new DataTable();
                        var paremeter4 = new Dictionary<string, object>
                         {
                             {"@cid",Getdata.billtocustomerid }
                         };
                        dqbidcust1 = SqlHelper.ExecuteTable1(cust, paremeter4);
                        if (dqbidcust1.Rows.Count > 0)
                        {
                            CHKCUS = dqbidcust1.Rows[0][0].ToString();
                            if (CHKCUS == "FALSE" && VouType == "RN")
                            {
                                string errorMessage = "Invalid Customer Type";
                                Method.Insertresponse(resp, CustomerDetails, Getdata.vou_number, Convert.ToInt32(Getdata.officllocation), 0, 0, errorMessage);
                                return BadRequest(errorMessage);
                            }
                        }
                        string chkgst = "sp_chkgstno";
                        DataTable dqchkgst = new DataTable();
                        var parameter5 = new Dictionary<string, object>
                            {
                                {"@cid",Getdata.customer_id},
                                {"@gst",Getdata.BilltoGST }
                            };
                        dqchkgst = SqlHelper.ExecuteTable1(chkgst, parameter5);
                        if (dqchkgst.Rows.Count == 0)
                        {
                            string errorMessage = "Bill to customer GST No Is Incorrect Pls Give Correct GST NO";
                            Method.Insertresponse(resp, CustomerDetails, Getdata.vou_number, Convert.ToInt32(Getdata.officllocation), 0, 0, errorMessage);
                            return BadRequest(errorMessage);
                        }


                        string chkgst1 = "sp_chkgstno";
                        DataTable dqchkgst1 = new DataTable();
                        var parameter6 = new Dictionary<string, object>
                            {
                                {"@cid",Getdata.supplytocustomerid},
                                {"@gst",Getdata.SupplytoGST }
                            };
                        dqchkgst = SqlHelper.ExecuteTable1(chkgst1, parameter6);
                        if (dqchkgst.Rows.Count == 0)
                        {
                            string errorMessage = "Supply to customer GST No Is Incorrect Pls Give Correct GST NO";
                            Method.Insertresponse(resp, CustomerDetails, Getdata.vou_number, Convert.ToInt32(Getdata.officllocation), 0, 0, errorMessage);
                            return BadRequest(errorMessage);
                        }

                    }
                    int branchid = Convert.ToInt32(Getdata.officllocation);
                    string Query = "sp_json_insert_Estimateshead_new_upd_1_02_12_2022";
                    var parameter7 = new Dictionary<string, object>
                    {
                       {"@Customer_ID", Getdata.customer_id},
                       {"@Customer_Type", Getdata.customer_type},
                       {"@OfficeLocation", Getdata.officllocation},
                       {"@Vou", Getdata.vou_number},
                       {"@VouType", VouType},
                       {"@reign_id", Getdata.reign_id},
                       {"@Place_of_supply", Getdata.place_of_supply},
                       {"@VAT_Treatment", Getdata.vat_treatment},
                       {"@Tax_Treatment", Getdata.tax_treatment},
                       {"@GST_treatment", Getdata.gst_treatment},
                       {"@GST_Number", Getdata.Wiz_gst_no},
                       {"@Blno", Getdata.Blno},
                       {"@Reference_Number_WIZ_ID", Getdata.reference_number},
                       {"@WizBookingdate", Getdata.WizBookingdate},
                       {"@Template_ID", Getdata.template_id},
                       {"@BillToCustomerID", Getdata.billtocustomerid},
                       {"@SupplyToCustomerID", Getdata.supplytocustomerid},
                       {"@POL", Getdata.pol},
                       {"@POD", Getdata.pod},
                       {"@VouDate", Getdata.date},
                       {"@Duedate", Getdata.DueDate},
                       {"@Payment_Terms", Getdata.payment_terms},
                       {"@Discount", Getdata.line_items[0].discount},
                       {"@Discount_before_tax", Getdata.is_discount_before_tax},
                       {"@Is_inclusive_of_tax", Getdata.is_inclusive_tax},
                       {"@Exchange_Rate", Getdata.exchange_rate},
                       {"@Notes", Getdata.notes},
                       {"@Terms", Getdata.terms},
                       {"@BillToName", Getdata.BillToName},
                       {"@BillToAddress", Getdata.BillToAddress},
                       {"@BilltoGST", Getdata.BilltoGST},
                       {"@SupplyToName", Getdata.SupplyToName},
                       {"@SupplyToAddress", Getdata.SupplyToAddress},
                       {"@MBL", Getdata.MBL},
                       {"@FreightTerms", Getdata.FreightTerms},
                       {"@VesselFlight", Getdata.VesselFlight},
                       {"@Shipper", Getdata.Shipper},
                       {"@Consignee", Getdata.Consignee},
                       {"@Line", Getdata.Line},
                       {"@IGM", Getdata.IGM},
                       {"@Billtolocation", Getdata.Billtolocation},
                       {"@SupplyTolocation", Getdata.SupplyTolocation},
                       {"@SupplyToGST", Getdata.SupplytoGST},
                       {"@Product", Getdata.Product},
                       {"@Containerid", Getdata.Containerid},
                       {"@GoodDescription", Getdata.GoodDescription},
                       {"@Vol_Wei_Pack", Getdata.Vol_Wei_Pack},
                       {"@ETD", Getdata.ETD},
                       {"@ETA", Getdata.ETA},
                       {"@region_currency_exchange_rate", Getdata.region_currency_exchange_rate},
                       {"@base_currency", Getdata.base_currency},
                       {"@icainvoice",Getdata.ica_invoice}
                    };
                    string output = SqlHelper.ExecuteNonQueryPara(Query, parameter7);

                    string QueryDet = "sp_json_insert_Estimatesdetails_new_upd";
                    for (int i = 0; i < Getdata.line_items.Count; i++)
                    {
                        var parameter8 = new Dictionary<string, object>
                        {
                          { "@Product_type", Getdata.line_items[i].product_type },
                          { "@HSN_or_SAC", Getdata.line_items[i].hsn_or_sac },
                          { "@ChargeId", Getdata.line_items[i].chargeid },
                          { "@Name", Getdata.line_items[i].name },
                          { "@Description", Getdata.line_items[i].description },
                          { "@Item_Order", Getdata.line_items[i].item_order },
                          { "@BCY_rate", Getdata.line_items[i].bcy_rate },
                          { "@Currency", Getdata.line_items[i].Currency },
                          { "@Rate", Getdata.line_items[i].rate },
                          { "@Ex_Rate", Getdata.line_items[i].Ex_Rate },
                          { "@Quantity", Getdata.line_items[i].quantity },
                          { "@Unit", Getdata.line_items[i].unit },
                          { "@Amount", Getdata.line_items[i].Amount },
                          { "@Discount_Amount", Getdata.line_items[i].discount_amount },
                          { "@Discount", Getdata.line_items[i].discount },
                          { "@Tax_ID", Getdata.line_items[i].tax_id },
                          { "@Tax_Name", Getdata.line_items[i].tax_name },
                          { "@Tax_Type", Getdata.line_items[i].tax_type },
                          { "@Tax_Percentage", Getdata.line_items[i].tax_percentage },
                          { "@Tax_Treatment_Code", Getdata.line_items[i].tax_treatment_code },
                          { "@Tax_Amount", Getdata.line_items[i].Tax_Amount },
                          { "@Header_Name", Getdata.line_items[i].header_name },
                          { "@Item_ID", Getdata.line_items[i].item_id },
                          { "@eid", output }
                        };
                        bool Resp = SqlHelper.ExecuteNonQuery(QueryDet, parameter8);
                    }

                    if (Getdata.VendorInvoiceUpload != null)
                    {
                        byte[] fileBytes = Convert.FromBase64String(Getdata.VendorInvoiceUpload);
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string fileName = $"{Getdata.reference_number}{timestamp}.pdf";
                        string ftpUrl = $"ftp://52.172.13.58/Wiz/WIZAPI/VendorReference/{fileName}";
                        var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        request.Credentials = new NetworkCredential("yourFtpUsername", "yourFtpPassword");
                        request.UseBinary = true;
                        request.UsePassive = true;
                        request.KeepAlive = false;
                        request.ContentLength = fileBytes.Length;
                        using (Stream requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(fileBytes, 0, fileBytes.Length);
                        }
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            // Optional: log or handle response.StatusDescription
                        }
                        string query = "Sp_mastervoucherapiEstimatesheadFileUpload";
                        var parameters = new Dictionary<string, object>
                        {
                            { "@Vou#", Getdata.vou_number },
                            { "@VouType", Getdata.voutype },
                            { "@VendorInvoiceFileName", fileName }
                        };

                        SqlHelper.ExecuteNonQuery(query, parameters);
                    }
                    string query4 = "apitoinvoice_chk";
                    var parameter9 = new Dictionary<string, object>
                    {
                        {"@eid",output }
                    };
                    output1 = SqlHelper.ExecuteNonQueryPara_sara(query4, parameter9);

                    Filename = Filename + "Profoma" + "_" + output1;
                    string queryinapp = "sp_proiv_app_json";
                    var parameters10 = new Dictionary<string, object>
                    {
                        {"@refno",output1},
                        {"@vt", VouType }
                    };
                    dt = SqlHelper.ExecuteTable1(queryinapp, parameters10);

                    string queryinapp1 = "sp_wiz_json_amt";
                    var paramerets11 = new Dictionary<string, object>
                    {
                        {"@refno", output1 },
                        {"@vt", VouType }
                    };
                    dt2 = SqlHelper.ExecuteTable1(queryinapp1, paramerets11);

                    if (cntry.Rows.Count > 0)
                    {
                        cntrycod = Convert.ToInt32(cntry.Rows[0][0].ToString());
                        divcnt = Convert.ToInt32(cntry.Rows[0][1].ToString());
                        branchname1 = cntry.Rows[0][2].ToString();
                    }
                    else
                    {
                        cntrycod = 102;
                        branchname1 = "RADAR VENTURES PRIVATE LIMITED";
                    }
                    branchname1 = branchname1.Replace(",", "");

                    switch (VouType)
                    {
                        case null:
                        case "":
                            vt = "";
                            break;
                        case "IN":
                            vt = "Invoice";
                            break;
                        case "BS":
                            vt = "BOS";
                            break;
                        case "VI":
                            vt = "PA";
                            break;
                        case "OSDN":
                            vt = "ProOSDN";
                            ctype = "P";
                            break;
                        case "OSCN":
                            vt = "ProOSCN";
                            ctype = "P";
                            break;

                        case "OTDN":
                            vt = "DN";
                            break;

                        case "OTCN":
                            vt = "CN";
                            break;

                        case "RN":
                            vt = "RN";
                            break;

                        case "INFC":
                            vt = "Invoice FC";
                            break;

                        case "VIFC":
                            vt = "PA FC";
                            break;

                        default:
                            vt = "";
                            break;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            string link = BAL.link;
                            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                            {
                                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                                if (vt == "ProOSDN" || vt == "ProOSCN")
                                {
                                    if (cntrycod == 102)
                                    {
                                        htmlUrl = $"{link}/Reportasp/InvoicerptFAOTDNCN.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dt.Rows[i][0]}" + $"&total={dt2.Rows[0][0]}" + $"&blno={dt.Rows[i][1]}" + $"&bltype=H" + $"&header={vt}" + $"&trantype={Product}" + $"&Profoma=Profoma" + $"&customertype={ctype}" + $"&&cid1=1" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1} " + $"&Containerid={Containerid}" + $"&CURR={dt2.Rows[0][1]}";
                                    }
                                    else
                                    {
                                        htmlUrl = $"{link}/Reportasp/VoucherRptNonIndia.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dt.Rows[i][0]}" + $"&total={dt2.Rows[0][0]}" + $"&blno={dt.Rows[i][1]}" + $"&bltype=H" + $"&header={vt}" + $"&trantype={Product}" + $"&Profoma=Profoma" + $"&customertype={ctype}" + $"&&cid1={divcnt}" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dt2.Rows[0][1]}";
                                    }
                                }

                                else if (vt == "Invoice FC" || vt == "PA FC")
                                {
                                    if (cntrycod == 102)
                                    {
                                        htmlUrl = $"{link}/Reportasp/InvoicerptFAOTFC.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dt.Rows[i][0]}" + $"&total={dt2.Rows[0][0]}" + $"&blno={dt.Rows[i][1]}" + $"&bltype=H" + $"&trantype={Product}" + $"&header={vt}" + $"&Profoma=Profoma" + $"&cid1=1" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dt2.Rows[0][1]}";
                                    }
                                    else
                                    {
                                        htmlUrl = $"{link}/Reportasp/VoucherRptNonIndia.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dt.Rows[i][0]}" + $"&total={dt2.Rows[0][0]}" + $"&blno={dt.Rows[i][1]}" + $"&bltype=H" + $"&trantype={Product}" + $"&header={vt}" + $"&Profoma=Profoma" + $"&cid1={divcnt}" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dt2.Rows[0][1]}";
                                    }
                                }
                                else
                                {
                                    if (cntrycod == 102)
                                    {
                                        htmlUrl = $"{link}/Reportasp/InvoicerptFAOT.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dt.Rows[i][0]}" + $"&total={dt2.Rows[0][0]}" + $"&blno={dt.Rows[i][1]}" + $"&bltype=H" + $"&trantype={Product}" + $"&header={vt}" + $"&Profoma=Profoma" + $"&cid1=1" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dt2.Rows[0][1]}";
                                    }
                                    else
                                    {
                                        htmlUrl = $"{link}/Reportasp/VoucherRptNonIndia.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dt.Rows[i][0]}" + $"&total={dt2.Rows[0][0]}" + $"&blno={dt.Rows[i][1]}" + $"&bltype=H" + $"&trantype={Product}" + $"&header={vt}" + $"&Profoma=Profoma" + $"&cid1={divcnt}" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dt2.Rows[0][1]}";
                                    }
                                }

                                Filename = "report" + output1 + ".pdf";
                                var pdfBytes = htmlToPdf.GeneratePdfFromFile(htmlUrl, null);

                                string noofcounts = "noofcountpdf";
                                var paramerets12 = new Dictionary<string, object>();
                                dt_noof1 = SqlHelper.ExecuteTable1(noofcounts, paramerets12);
                                int dt_noof = Convert.ToInt32(dt_noof1.Rows[0]["counts"].ToString());

                                string filename1 = $"Proforma_{dt_noof}.pdf";
                                byte[] Base64String = pdfBytes;
                                string path = $"ftp://52.172.13.58/Wiz_Radar_Live_pdf/Profoma/{filename1}";
                                LoadData loadData = new LoadData();
                                loadData.PDFUploadAsync(path, pdfBytes);

                                var dataBytes = pdfBytes;
                                var dataStream = new MemoryStream(dataBytes);
                                //HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                                //httpResponseMessage.Content = new StreamContent(dataStream);
                                //httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                                //httpResponseMessage.Content.Headers.ContentDisposition.FileName = bookName;
                                //httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                                string responseDetails = $"Attachment; filename={filename1}; Content-Type=application/octet-stream";
                                Method.InsertResponce(responseDetails, "ES", Getdata.vou_number,Convert.ToInt32(Getdata.officllocation),Convert.ToInt32(output),1,"Success",filename1);
                                return File(pdfBytes, "application/octet-stream", filename1);

                            }

                        }
                    }
                     responseMessage = "Insert Failed";
                    string responseStatus = $"StatusCode: 500, Message: {responseMessage}";
                    Method.Insertresponse(responseStatus, "ES", Convert.ToInt32(output), 0, responseMessage);
                    return StatusCode(StatusCodes.Status500InternalServerError, responseMessage);

                }
                else
                {
                    string responseContent = $"StatusCode: 400, Errors: {string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}";
                    Method.Insertresponse(responseContent,"ES",0, 0, "Invalid JSON");
                   return BadRequest(ModelState);

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string responseContent = $"StatusCode: 417, Message: {msg}";
                Method.Insertresponse(responseContent, "", 0, 0, msg);
                return StatusCode(StatusCodes.Status417ExpectationFailed, msg);

            }
            return Ok();
        }



        [HttpPost("ApproveEstimatesDetails")]
        public ActionResult<string> ApproveEstimates([FromBody] Estimatesapp Getdetails)
        {
            VoucherNo = Getdetails.vou_number;
            try
            {
                MagikwebAPIRequest Webmodel = new MagikwebAPIRequest();
                if (Getdetails != null)
                {
                    DataTable dt = new DataTable();
                    BAL SqlHelper = new BAL();
                    string details = JsonSerializer.Serialize(Getdetails);
                    int branchid = Convert.ToInt32(Getdetails.officllocation);
                    VouType = Convert.ToString(Getdetails.voutype);
                    vouty = Getdetails.voutype;
                    Containerid = Convert.ToString(Getdetails.Containerid);
                    Product = Convert.ToString(Getdetails.Product);

                    Webmodel.CustomerDetails = "AE";
                    int approvedby = 246;


                    string quary = "Sp_MagikwebAPIRequest";
                    var parameters13 = new Dictionary<string, object>
                    {
                        {"@APIRequest", details },
                        {"@APIRequesttype",Webmodel.CustomerDetails }
                    };
                    bool n = SqlHelper.ExecuteNonQuery(quary, parameters13);

                    if (ModelState.IsValid)
                    {
                        voudate1 = Convert.ToDateTime(Getdetails.date);

                        if (voudate1.Month < 4)
                        {
                            vouyear1 = Convert.ToInt32((voudate1.Year - 1).ToString());
                        }
                        else
                        {
                            vouyear1 = Convert.ToInt32((voudate1.Year).ToString());
                        }
                    }


                    org = vouyear1.ToString();
                    prifx = org.Substring(2);
                    con1 = Convert.ToInt32(prifx) + 1;
                    str_FAYear = prifx.ToString() + con1.ToString();
                    str_FADbname = "iFACTFA" + str_FAYear;

                    switch (VouType)
                    {
                        case "IN":
                            vouty = "Invoice";
                            voustr = "Transfer To Commercial Invoice";
                            break;
                        case "VI":
                            vouty = "PA";
                            voustr = "Transfer To Commercial PA";
                            break;
                        case "OTCN":
                            vouty = "CN";
                            voustr = "Credit Note";
                            break;
                        case "OTDN":
                            vouty = "DN";
                            voustr = "Debit Note";
                            break;
                        case "RN":
                            vouty = "Reimbursement";
                            voustr = "Transfer To Commercial REIMBURSEMENT";
                            break;
                        case "BS":
                            vouty = "Bill of Supply";
                            voustr = "Transfer To  BOS";
                            break;
                        case "OSDN":
                            vouty = "OSDN";
                            ctype = "P";
                            voustr = "Transfer To OSDN";
                            break;
                        case "OSCN":
                            vouty = "OSCN";
                            ctype = "P";
                            voustr = "Transfer To OSCN";
                            break;
                        case "INFC":
                            vouty = "Invoice FC";
                            ctype = "P";
                            voustr = "Transfer To Commercial Invoice FC";
                            break;
                        case "VIFC":
                            vouty = "PA FC";
                            ctype = "P";
                            voustr = "Transfer To Commercial PA FC";
                            break;
                    }

                    DataTable cntry = new DataTable();
                    string spcntrychk = "cntrychka_sp";
                    var parameters14 = new Dictionary<string, object>
                     {
                         {"@bid", Getdetails.officllocation },

                     };
                    cntry = SqlHelper.ExecuteTable1(spcntrychk, parameters14);

                    if (cntry.Rows.Count > 0)
                    {
                        cntrycod = Convert.ToInt32(cntry.Rows[0][0].ToString());
                        divcnt = Convert.ToInt32(cntry.Rows[0][1].ToString());
                        branchname1 = cntry.Rows[0][2].ToString();
                    }
                    else
                    {
                        cntrycod = 102;
                    }
                    foreach (var item in Getdetails.line_items)
                    {
                        if (item.Ex_Rate == 0)
                        {
                            string response = "Exchange_Rate_is_Zero";
                            Method.Insertresponse(response, Webmodel.CustomerDetails, 0, 0, response);
                            return BadRequest(response);
                        }
                    }



                    if (cntrycod == 102)
                    {
                        string q2 = "sp_bid_chk";
                        DataTable dqbid1 = new DataTable();
                        var obqbid = new Dictionary<string, object>
                        {
                            { "@gst",Getdetails.Wiz_gst_no }
                        };
                        dqbid1 = SqlHelper.ExecuteTable1(q2, obqbid);

                        if (dqbid1.Rows.Count == 0)
                        {
                            string response = "Given Company GST No Is Incorrect Pls Give Correct GST NO";
                            Method.Insertresponse(response, Webmodel.CustomerDetails, 0, 0, response);
                            return BadRequest(response);
                        }
                        string cust = "sp_customerCHK";
                        DataTable dqbidcust1 = new DataTable();
                        var obqbidcust = new Dictionary<string, object>
                        {
                            {"@cid",Getdetails.billtocustomerid}
                        };
                        dqbidcust1 = SqlHelper.ExecuteTable1(cust, obqbidcust);

                        if (dqbidcust1.Rows.Count == 0)
                        {
                            string CHKCUS = dqbidcust1.Rows[0][0].ToString();
                            if (CHKCUS == "FALSE" && VouType == "RN")
                            {
                                string response = "Invalid Customer Type";
                                Method.Insertresponse(response, Webmodel.CustomerDetails, 0, 0, response);
                                return BadRequest(response);
                            }
                        }

                        string chkgst = "sp_chkgstno";
                        DataTable dqchkgst = new DataTable();
                        var obqchkgst = new Dictionary<string, object>
                        {
                            {"@cid", Getdetails.billtocustomerid },
                            {"@gst" , Getdetails.BilltoGST }
                        };
                        dqchkgst = SqlHelper.ExecuteTable1(chkgst, obqchkgst);

                        if (dqchkgst.Rows.Count == 0)
                        {
                            string Responce = "Bill to customer GST No Is Incorrect Pls Give Correct GST NO";
                            Method.Insertresponse(Responce, Webmodel.CustomerDetails, 0, 0, Responce);
                            return BadRequest(Responce);
                        }

                        string chkgst1 = "sp_chkgstno";
                        DataTable dqchkgst1 = new DataTable();
                        var obqchkgst1 = new Dictionary<string, object>
                        {
                            {"@cid", Getdetails.supplytocustomerid },
                            {"@gst", Getdetails.SupplytoGST }
                        };
                        dqchkgst1 = SqlHelper.ExecuteTable1(chkgst1, obqchkgst1);
                        if (dqbidcust1.Rows.Count == 0)
                        {
                            string Responce = "Supply to customer GST No Is Incorrect Pls Give Correct GST NO";
                            Method.Insertresponse(Responce, Webmodel.CustomerDetails, 0, 0, Responce);
                            return BadRequest(Responce);
                        }
                    }
                    if (Getdetails.officllocation == "23" && Getdetails.region_currency_exchange_rate == "" && (VouType != "VI" && VouType != "OTDN" && VouType != "OSCN" && VouType != "OSDN"))
                    {
                        string Responce = "Region_Currency_Exchange_Rate_is_Empty";
                        Method.Insertresponse(Responce, Webmodel.CustomerDetails, 0, 0, Responce);
                        return BadRequest(Responce);
                    }

                    if (Getdetails.billtocustomerid == "" || Getdetails.supplytocustomerid == "")
                    {
                        string Responce = "Ifact_I'D_sent_as_Empty";
                        Method.Insertresponse(Responce, Webmodel.CustomerDetails, 0, 0, Responce);
                        return BadRequest(Responce);
                    }

                    string q1 = "sp_chk_appdata";
                    DataTable dq1 = new DataTable();
                    var obq = new Dictionary<string, object>
                        {
                            {"@vou", Getdetails.vou_number },
                            {"@vtyp", Getdetails.voutype },
                            {"blno", Getdetails.Blno },
                            {"@bid", Getdetails.officllocation },
                            {"@vouyear",vouyear1 }
                        };
                    dq1 = SqlHelper.ExecuteTable1(q1, obq);

                    if (dq1.Rows.Count > 0)
                    {
                        string Responce = "Already Approved the " + (vouty).ToString();
                        Method.Insertresponse(Responce, Webmodel.CustomerDetails, 0, 0, Responce);
                        return BadRequest(Responce);
                    }

                    string Query5 = "sp_approveEstimateshead_02_12_2022";
                    var objEst3 = new Dictionary<string, object>
                        {
                            { "@Customer_ID", Getdetails.customer_id },
                            { "@Customer_Type", Getdetails.customer_type },
                            { "@OfficeLocation", Getdetails.officllocation },
                            { "@Vou", Getdetails.vou_number },
                            { "@VouType", VouType },
                            { "@reign_id", Getdetails.reign_id },
                            { "@refVou", Getdetails.Ref_invoice_number },
                            { "@refVouType", Getdetails.Ref_voutype },
                            { "@Place_of_supply", Getdetails.place_of_supply },
                            { "@VAT_Treatment", Getdetails.vat_treatment },
                            { "@Tax_Treatment", Getdetails.tax_treatment },
                            { "@GST_treatment", Getdetails.gst_treatment },
                            { "@GST_Number", Getdetails.Wiz_gst_no },
                            { "@Blno", Getdetails.Blno },
                            { "@Reference_Number_WIZ_ID", Getdetails.reference_number },
                            { "@WizBookingdate", Getdetails.WizBookingdate},
                            { "@Template_ID", Getdetails.template_id},
                            { "@BillToCustomerID", Getdetails.billtocustomerid },
                            { "@SupplyToCustomerID", Getdetails.supplytocustomerid },
                            { "@POL", Getdetails.pol },
                            { "@POD", Getdetails.pod },
                            { "@VouDate", Getdetails.date },
                            { "@Duedate", Getdetails.DueDate },
                            { "@Payment_Terms", Getdetails.payment_terms },
                            { "@Discount", Getdetails.line_items[0].discount },
                            { "@Discount_before_tax", Getdetails.is_discount_before_tax },
                            { "@Is_inclusive_of_tax", Getdetails.is_inclusive_tax },
                            { "@Exchange_Rate", Getdetails.exchange_rate },
                            { "@Notes", Getdetails.notes },
                            { "@Terms", Getdetails.terms },
                            { "@BillToName", Getdetails.BillToName },
                            { "@BillToAddress", Getdetails.BillToAddress },
                            { "@BilltoGST", Getdetails.BilltoGST },
                            { "@SupplyToName", Getdetails.SupplyToName },
                            { "@SupplyToAddress", Getdetails.SupplyToAddress },
                            { "@MBL", Getdetails.MBL },
                            { "@FreightTerms", Getdetails.FreightTerms },
                            { "@VesselFlight", Getdetails.VesselFlight },
                            { "@Shipper", Getdetails.Shipper },
                            { "@Consignee", Getdetails.Consignee },
                            { "@Line", Getdetails.Line },
                            { "@IGM", Getdetails.IGM },
                            { "@Billtolocation", Getdetails.Billtolocation },
                            { "@SupplyTolocation", Getdetails.SupplyTolocation },
                            { "@SupplyToGST", Getdetails.SupplytoGST },
                            { "@Product", Getdetails.Product },
                            { "@Againt_Vou_No", Getdetails.Againt_Vou_No },
                            { "@Againt_Voudate", Getdetails.Againt_Voudate },
                            { "@Againt_voutype", Getdetails.Againt_voutype },
                            { "@Againt_officllocation", Getdetails.Againt_officllocation },
                            { "@Containerid", Getdetails.Containerid },
                            { "@GoodDescription", Getdetails.GoodDescription },
                            { "@Vol_Wei_Pack", Getdetails.Vol_Wei_Pack },
                            { "@ETD", Getdetails.ETD },
                            { "@ETA", Getdetails.ETA },
                            { "@region_currency_exchange_rate", Getdetails.region_currency_exchange_rate },
                            { "@base_currency", Getdetails.base_currency },
                            {"@icainvoice",Getdetails.ica_invoice }
                        };
                    string output = SqlHelper.ExecuteNonQueryPara(Query5, objEst3);

                    string QueryDet = "sp_approveEstimatesdetails";

                    for (int i = 0; i < Getdetails.line_items.Count; i++)
                    {
                        var item = Getdetails.line_items[i];

                        var objEstdet = new Dictionary<string, object>
                            {
                                { "@Product_type", item.product_type },
                                { "@HSN_or_SAC", item.hsn_or_sac },
                                { "@ChargeId", item.chargeid },
                                { "@Name", item.name },
                                { "@Description", item.description },
                                { "@Item_Order", item.item_order },
                                { "@BCY_rate", item.bcy_rate },
                                { "@Currency", item.Currency },
                                { "@Rate", item.rate },
                                { "@Ex_Rate", item.Ex_Rate },
                                { "@Quantity", item.quantity },
                                { "@Unit", item.unit },
                                { "@Amount", item.Amount },
                                { "@Discount_Amount", item.discount_amount },
                                { "@Discount", item.discount },
                                { "@Tax_ID", item.tax_id },
                                { "@Tax_Name", item.tax_name },
                                { "@Tax_Type", item.tax_type },
                                { "@Tax_Percentage", item.tax_percentage },
                                { "@Tax_Treatment_Code", item.tax_treatment_code },
                                { "@Tax_Amount", item.Tax_Amount },
                                { "@Header_Name", item.header_name },
                                { "@Item_ID", item.item_id },
                                { "@eid", output },
                                { "@Vou", Getdetails.vou_number },
                                { "@VouType", VouType }
                            };

                        bool Res = SqlHelper.ExecuteNonQuery(QueryDet, objEstdet);
                    }

                    DataTable dt2 = new DataTable();
                    //BAL sqlHelper = new BAL();
                    string Query1 = "SPUpdMCForProApproval_json";
                    var objEst2 = new Dictionary<string, object>
                        {
                            {"@branchid",branchid },
                            {"@type", voustr }
                        };
                    dt2 = SqlHelper.ExecuteTable1(Query1, objEst2);

                    if (dt2.Rows.Count > 0)
                    {
                        invoicenoapp = Convert.ToInt32(dt2.Rows[0][0]);
                    }

                    string query4 = "api_Approve_new_15_03_2023";
                    var objEst1 = new Dictionary<string, object>
                        {
                            {"@eid",output },
                            {"@invno",invoicenoapp }
                        };
                    output1 = SqlHelper.ExecuteNonQueryPara_sara(query4, objEst1);

                    if ((vouty == "Invoice" || vouty == "CN" || vouty == "Invoice FC" || vouty == "OSDN") && invoicenoapp > 0)
                    {
                        DataTable dtq5 = new DataTable();

                        if (vouty == "Invoice")
                        {
                            eivtype = "I";
                        }
                        else if (vouty == "CN")
                        {
                            eivtype = "E";
                        }
                        else if (vouty == "Invoice FC")
                        {
                            eivtype = "F";
                        }
                        else if (vouty == "OSDN")
                        {
                            eivtype = "D";
                        }

                        string q5 = "spinsmastergstdetailsMR";
                        var objq5 = new Dictionary<string, object>
                            {
                                {"@cid",1 }
                            };
                        dtq5 = SqlHelper.ExecuteTable1(q5, objq5);

                        if (dtq5.Rows.Count > 0)
                        {
                            div_id = dtq5.Rows[0][0].ToString();
                        }

                        DataTable dtq6 = new DataTable();
                        string q6 = "spgetunregcustvouchers";
                        var objq6 = new Dictionary<string, object>
                            {
                                {"@vouno",invoicenoapp},
                                {"@vouyear", vouyear1},
                                {"@bid",branchid },
                                {"@voutype",eivtype}
                            };
                        dtq6 = SqlHelper.ExecuteTable1(q6, objq6);

                        if (dtq6.Rows.Count > 0)
                        {
                            custid1ung = dtq6.Rows[0][0].ToString();
                        }

                        if (div_id == "1" && custid1ung == "0" && cntrycod == 102)
                        {

                            try
                            {
                                DataTable dtq7 = new DataTable();
                                string q7 = "sp_getgstindetjsonliveInv4API";
                                var objq7 = new Dictionary<string, object>
                                    {
                                        { "@vouno", invoicenoapp },
                                        { "@bid", branchid },
                                        { "@vouyear", vouyear1 },
                                        { "@voutype", eivtype }
                                    };
                                dtq7 = SqlHelper.ExecuteTable1(q7, objq7);

                                if (dtq7.Rows.Count > 0)
                                {
                                    json1 = dtq7.Rows[0][0].ToString();
                                }

                                string datajson = "";
                                //string datajson =   DineshhttpPostWebRequets("http://my.gstzen.in/~gstzen/a/post-einvoice-data/einvoice-json/", json1);

                                DataTable dtjson = new DataTable();
                                string status = "";
                                if (datajson != null)
                                {
                                    dtjson = ConvertJsonToDatatable(datajson);
                                    status = dtjson.Rows[0][0].ToString().Trim();
                                }
                                else
                                {
                                    status = "0";
                                }
                                string message1 = "";
                                string IRN1 = "";
                                string Ackdt = "";
                                string Ackno = "";
                                string status1 = "";
                                string SignedQRCode = "";
                                string SignedInvoice = "";

                                string uuid = "";
                                string SignedQrCodeImgUrl = "";
                                string IrnStatus = "";
                                string EwbStatus = "";
                                string Irp = "";
                                string EwbDt = "";
                                string EwbNo = "";
                                string EwbValidTill = "";
                                string Remarks = "";

                                if (status == "1")
                                {
                                    message1 = dtjson.Rows[0][1].ToString().Replace('"', ' ').Trim();           //	1                       
                                    IRN1 = dtjson.Rows[0][2].ToString().Replace('"', ' ').Trim();       // 2
                                    Ackdt = dtjson.Rows[0][3].ToString().Replace('"', ' ').Trim();     //3
                                    Ackno = dtjson.Rows[0][4].ToString().Replace('"', ' ').Trim();    // 4 
                                    status1 = dtjson.Rows[0][7].ToString().Replace('"', ' ').Trim();  // 7
                                    SignedQRCode = dtjson.Rows[0][10].ToString().Replace('"', ' ').Trim(); //10

                                    SignedInvoice = dtjson.Rows[0][11].ToString().Replace('"', ' ').Trim(); //11



                                    uuid = dtjson.Rows[0][12].ToString().Replace('"', ' ').Trim();  //12
                                    SignedQrCodeImgUrl = dtjson.Rows[0][13].ToString().Replace('"', ' ').Trim();// 13 
                                    IrnStatus = dtjson.Rows[0][14].ToString().Replace('"', ' ').Trim();  //14 
                                    EwbStatus = dtjson.Rows[0][15].ToString().Replace('"', ' ').Trim(); //15
                                    Irp = dtjson.Rows[0][16].ToString().Replace('"', ' ').Trim(); //16

                                    EwbDt = dtjson.Rows[0][5].ToString().Replace('"', ' ').Trim(); //5
                                    EwbNo = dtjson.Rows[0][6].ToString().Replace('"', ' ').Trim(); //6
                                    EwbValidTill = dtjson.Rows[0][9].ToString().Replace('"', ' ').Trim(); // 9
                                    Remarks = dtjson.Rows[0][8].ToString().Replace('"', ' ').Trim();//  8


                                    DataTable dtq9 = new DataTable();
                                    //string custid1ung;
                                    string q9 = "spinsmastergstdetails";
                                    var objq9 = new Dictionary<string, object>
                                        {
                                            { "@vouno", invoicenoapp },
                                            { "@vouyear", vouyear1 },
                                            { "@bid", branchid },
                                            { "@cid", 1 },
                                            { "@status", status },
                                            { "@message", message1 },
                                            { "@Irn", IRN1 },
                                            { "@AckDt", Ackdt },
                                            { "@AckNo", Ackno },
                                            { "@Status1", status1 },
                                            { "@SignedQRCode", SignedQRCode },
                                            { "@SignedInvoice", SignedInvoice },
                                            { "@uuid", uuid },
                                            { "@SignedQrCodeImgUrl", SignedQrCodeImgUrl },
                                            { "@IrnStatus", IrnStatus },
                                            { "@EwbStatus", EwbStatus },
                                            { "@Irp", Irp },
                                            { "@voutype", eivtype },
                                            { "@getdate", VouType },
                                            { "@EwbDt", EwbDt },
                                            { "@EwbNo", EwbNo },
                                            { "@EwbValidTill", EwbValidTill },
                                            { "@Remarks", Remarks }
                                        };
                                    bool Res = SqlHelper.ExecuteNonQuery(q9, objq9);

                                }
                                else
                                {
                                    if (datajson != null)
                                    {
                                        message1 = dtjson.Rows[0][1].ToString().Replace('"', ' ').Trim();
                                    }
                                    else
                                    {
                                        message1 = "The GSTZen user credentials provided in the request are invalid-.";
                                    }

                                    DataTable dtq10 = new DataTable();
                                    //string custid1ung;
                                    string q10 = "spinsmastergstdetails";
                                    var objq10 = new Dictionary<string, object>
                                        {
                                             {"@vouno", invoicenoapp },
                                             {"@vouyear", vouyear1 },
                                             {"@bid", branchid },
                                             {"@cid", 1 },
                                             {"@status", status },
                                             {"@message", message1 },
                                             {"@Irn", "" },
                                             {"@AckDt", "" },
                                             {"@AckNo", "" },
                                             {"@Status1", "" },
                                             {"@SignedQRCode", "" },
                                             {"@SignedInvoice", SignedInvoice },
                                             {"@uuid", "" },
                                             {"@SignedQrCodeImgUrl", "" },
                                             {"@IrnStatus", "" },
                                             {"@EwbStatus", "" },
                                             {"@Irp", "" },
                                             {"@voutype", eivtype },
                                            // {"@getdate", VouType},
                                             {"@EwbDt", "" },
                                             {"@EwbNo", "" },
                                             {"@EwbValidTill", "" },
                                             {"@Remarks", "" }
                                        };
                                    bool Res = SqlHelper.ExecuteNonQuery(q10, objq10);



                                }



                            }
                            catch (Exception ex)
                            {
                                string message1 = ex.Message;
                                string alertMessage = $"alert('{message1}');";
                                string Responce = $"iFact Touch  {alertMessage}";
                                Method.Insertresponse(Responce, Webmodel.CustomerDetails, Getdetails.vou_number, Convert.ToInt32(Getdetails.officllocation), Convert.ToInt32(output), 0, "alert('" + message1 + "');");


                            }

                        }

                    }


                    if (Getdetails.VendorInvoiceUpload != null)
                    {
                        LoadData loadData = new LoadData();
                        byte[] Base64String = Convert.FromBase64String(Getdetails.VendorInvoiceUpload);
                        string dateTimeReplace = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string path = $"ftp://52.172.13.58/Wiz/WIZAPI/VendorReference/{Getdetails.reference_number}{dateTimeReplace}.pdf";
                        loadData.PDFUpload(path, Base64String);

                        string Qey = "Sp_approveEstimatesheadFileUpload";
                        var objdic = new Dictionary<string, object>
                            {
                                {"@Vou#",Getdetails.vou_number },
                                {"@VouType",Getdetails.voutype },
                                {"@VendorInvoiceFileName", $"{Getdetails.reference_number}{dateTimeReplace}.pdf"}
                            };
                        SqlHelper.ExecuteNonQuery(Qey, objdic);

                    }

                    string queryinapp = "sp_iv_app_json_new";
                    DataTable dts1 = new DataTable();
                    var objEstapp2 = new Dictionary<string, object>
                        {
                                { "@refno", invoicenoapp },
                                { "@vt", Getdetails.voutype },
                                { "@bid", Getdetails.officllocation },
                                { "@vouyear", vouyear1 }
                        };
                    dts1 = SqlHelper.ExecuteTable1(queryinapp, objEstapp2);

                    string queryinapp1 = "sp_invwiz_json_amt_new";
                    DataTable dts2 = new DataTable();
                    var objEstapp3 = new Dictionary<string, object>
                        {
                                { "@refno", invoicenoapp },
                                { "@vt", Getdetails.voutype },
                                { "@bid", Getdetails.officllocation },
                                 { "@vouyear", vouyear1 }
                        };
                    dts2 = SqlHelper.ExecuteTable1(queryinapp1, objEstapp3);

                    if (dts1.Rows.Count > 0)
                    {

                        if (dt2.Rows.Count > 0)
                        {

                            for (int i = 0; i <= dts1.Rows.Count - 1; i++)
                            {
                                string htmlUrl;
                                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                                string link = BAL.link;
                                if (vouty == "OSDN" || vouty == "OSCN") //|| vouty == "CN" || vouty == "DN"
                                {
                                    if (cntrycod == 102)
                                    {
                                        htmlUrl = $"{link}/Reportasp/InvoicerptFAOTDNCN.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dts1.Rows[i][0]}" + $"&total={dts2.Rows[0][0]}" + $"&blno={dts1.Rows[i][1]}" + $"&bltype=H" + $"&header={vouty}" + $"&trantype={dts1.Rows[i][2]}" + $"&Profoma=" + $"&customertype={ctype}" + $"&FormName={vt}" + $"&uiid=2028" + $"&cid1=1" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dts2.Rows[0][1]}";
                                    }
                                    else
                                    {
                                        htmlUrl = $"{link}/Reportasp/VoucherRptNonIndia.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dts1.Rows[i][0]}" + $"&total={dts2.Rows[0][0]}" + $"&blno={dts1.Rows[i][1]}" + $"&bltype=H" + $"&header={vouty}" + $"&trantype={dts1.Rows[i][2]}" + $"&Profoma=" + $"&customertype={ctype}" + $"&FormName={vt}" + $"&uiid=2028" + $"&cid1={divcnt}" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dts2.Rows[0][1]}";
                                    }
                                }
                                else if (vouty == "Invoice FC" || vouty == "PA FC")
                                {
                                    if (cntrycod == 102)
                                    {
                                        htmlUrl = $"{link}/Reportasp/InvoicerptFAOTFC.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dts1.Rows[i][0]}" + $"&total={dts2.Rows[0][0]}" + $"&blno={dts1.Rows[i][1]}" + $"&bltype=H" + $"&header={vouty}" + $"&trantype={dts1.Rows[i][2]}" + $"&Profoma=" + $"&customertype={ctype}" + $"&FormName={vt}" + $"&uiid=2028" + $"&cid1=1" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dts2.Rows[0][1]}";
                                    }
                                    else
                                    {
                                        htmlUrl = $"{link}/Reportasp/VoucherRptNonIndia.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dts1.Rows[i][0]}" + $"&total={dts2.Rows[0][0]}" + $"&blno={dts1.Rows[i][1]}" + $"&bltype=H" + $"&header={vouty}" + $"&trantype={dts1.Rows[i][2]}" + $"&Profoma=" + $"&customertype={ctype}" + $"&FormName={vt}" + $"&uiid=2028" + $"&cid1={divcnt}" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&divisionnamereport={branchname1}" + $"&Containerid={Containerid}" + $"&CURR={dts2.Rows[0][1]}";
                                    }

                                }
                                else
                                {
                                    if (cntrycod == 102)
                                    {
                                        htmlUrl = $"{link}/Reportasp/InvoicerptFAOT.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dts1.Rows[i][0]}" + $"&total={dts2.Rows[0][0]}" + $"&blno={dts1.Rows[i][1]}" + $"&bltype=H" + $"&trantype={dts1.Rows[i][2]}" + $"&header={vouty}" + $"&cid1=1" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&FormName=Invoice" + $"&uiid=1983" + $"&Containerid={Containerid}" + $"&CURR={dts2.Rows[0][1]}" + $"&divisionnamereport={branchname1}";
                                    }
                                    else
                                    {
                                        htmlUrl = $"{link}/Reportasp/VoucherRptNonIndia.aspx" + $"?Invoiceno={output1}" + $"&vouyear={dts1.Rows[i][0]}" + $"&total={dts2.Rows[0][0]}" + $"&blno={dts1.Rows[i][1]}" + $"&bltype=H" + $"&trantype={dts1.Rows[i][2]}" + $"&header={vouty}" + $"&cid1={divcnt}" + $"&empid=246" + $"&bid1={branchid}" + $"&username=RAJAKUMARAN.P" + $"&FADbname123={str_FADbname}" + $"&FADbname={str_FADbname}" + $"&FormName=Invoice" + $"&uiid=1983" + $"&Containerid={Containerid}" + $"&CURR={dts2.Rows[0][1]}" + $"&divisionnamereport={branchname1}";
                                    }
                                }

                                LoadData loaddata = new LoadData();
                                string CustJson = JsonSerializer.Serialize(Getdetails, new JsonSerializerOptions { WriteIndented = true });
                                string Attachments = loaddata.Download(CustJson, Getdetails.voutype);


                                string Branchname = string.Empty;
                                DataTable dtbranch = new DataTable();
                                var objdic = new Dictionary<string, object>
                                    {
                                        {"@branchid",branchid }
                                    };
                                dtbranch = SqlHelper.ExecuteTable1("Sp_WiZApigetbranchName", objdic);
                                if (dtbranch.Rows.Count > 0)
                                {
                                    Branchname = dtbranch.Rows[0]["portname"].ToString();
                                }

                                string filename = $"report{output1}.pdf";
                                byte[] pdfBytes = htmlToPdf.GeneratePdfFromFile(htmlUrl, null);
                                string bookName = $"{vouty}_{output1}.pdf";
                                string Base64string = Convert.ToBase64String(pdfBytes);


                                string noofCountsProcName = "noofcountpdf";
                                var objEst36 = new Dictionary<string, object>();
                                DataTable dtNoof1 = SqlHelper.ExecuteTable1(noofCountsProcName, objEst36);
                                int dtNoof = Convert.ToInt32(dtNoof1.Rows[0]["counts"]);
                                string filename1 = $"Approve_{dtNoof}.pdf";
                                byte[] pdfBytesToUpload = pdfBytes;
                                string ftpPath = $"ftp://52.172.13.58/elyxr-retrieve-nonindia/Approve/{filename1}";
                                LoadData loadData = new LoadData();
                                loadData.PDFUpload(ftpPath, pdfBytesToUpload);



                                if ((Getdetails.voutype == "IN" || Getdetails.voutype == "BS" || Getdetails.voutype == "INFC") && cntrycod == 102)
                                {
                                    Random rnd = new Random();
                                    int randomNumber = rnd.Next(); // You can use rnd.Next(10, 20) if you want it between 10 and 20
                                    output1 += randomNumber.ToString();

                                    // DSC Signing endpoint
                                    string signUrl = "https://radardsc.ifact.co.in:444/op/api/v1.0/postjson";

                                    // Call API to sign the PDF
                                    string signedPdfBase64 = loaddata.Postdata(Base64string, signUrl, Branchname, output1, vouty, VoucherNo);

                                    if (signedPdfBase64 == "status")
                                    {
                                        // Signature failed — fallback to original PDF
                                        pdfBytes = Convert.FromBase64String(Base64string);
                                    }
                                    else
                                    {
                                        // Signature successful — decode signed PDF
                                        pdfBytes = Convert.FromBase64String(signedPdfBase64);
                                    }
                                }

                                var dataBytes = pdfBytes;
                                var dataStream = new MemoryStream(dataBytes);
                                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                                httpResponseMessage.Content = new StreamContent(dataStream);
                                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                                httpResponseMessage.Content.Headers.ContentDisposition.FileName = bookName;
                                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                                string Responce = httpResponseMessage.ToString();
                                Method.InsertResponce(Responce, Webmodel.CustomerDetails, Getdetails.vou_number, Convert.ToInt32(Getdetails.officllocation), Convert.ToInt32(output), 1, "Sucess", filename1);
                                return File(dataStream, "application/octet-stream", bookName);

                            }

                        }

                    }
                    string message = "Insert Failed";
                    string responceLog = $"StatusCode: 500 InternalServerError, Message: {message}";

                    Method.Insertresponse(responceLog, Webmodel.CustomerDetails, Getdetails.vou_number, Convert.ToInt32(Getdetails.officllocation), Convert.ToInt32(output), 0, message);
                    return StatusCode(StatusCodes.Status500InternalServerError, message);

                }
                else
                {
                    string errorDetails = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
                    string responseMessage = $"StatusCode: 400 BadRequest, Errors: {errorDetails}";
                    // Log the response
                    Method.Insertresponse(responseMessage, Webmodel.CustomerDetails, Getdetails.vou_number, Convert.ToInt32(Getdetails.officllocation), 0, 0, "Invalid Json");
                    return BadRequest(ModelState);

                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string responceLog = $"StatusCode: 417 ExpectationFailed, Message: {msg}";

                Method.Insertresponse(responceLog, "", 0, 0, msg);

                return StatusCode(StatusCodes.Status417ExpectationFailed, msg);
            }
            return Ok();
        }

        public static string DineshhttpPostWebRequets(string url, string postData)
        {
            string strResponse = null;
            string tokenValue = null;
            // (Optional) Set security protocols — usually not needed in .NET Core 3.1+
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                // Get token from DB
                string procedure = "sp_geteinvoicetoken";
                Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@divid", 1 }
            };
                BAL sqlHelper = new BAL();
                DataTable dt = sqlHelper.ExecuteTable1(procedure, parameters);

                if (dt.Rows.Count > 0)
                {
                    tokenValue = dt.Rows[0][0].ToString();
                }

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(120);

                    // Set token header
                    if (!string.IsNullOrEmpty(tokenValue))
                        client.DefaultRequestHeaders.Add("Token", tokenValue);

                    var content = new StringContent(postData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                    response.EnsureSuccessStatusCode(); // throws if not 2xx

                    strResponse = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                // Log ex.Message if needed
            }

            return strResponse;
        }

        protected DataTable ConvertJsonToDatatable(string jsonString)
        {
            var dt = new DataTable();

            try
            {
                // Parse the JSON string
                var jsonArray = JsonNode.Parse(jsonString)?.AsArray();
                if (jsonArray == null || jsonArray.Count == 0)
                    return dt;

                // First pass: Get all column names
                var firstItem = jsonArray[0]?.AsObject();
                if (firstItem == null)
                    return dt;

                foreach (var property in firstItem)
                {
                    dt.Columns.Add(property.Key);
                }

                // Second pass: Add rows
                foreach (var item in jsonArray)
                {
                    var row = dt.NewRow();
                    var itemObject = item?.AsObject();

                    if (itemObject == null)
                        continue;

                    foreach (var property in itemObject)
                    {
                        try
                        {
                            row[property.Key] = property.Value?.ToString() ?? string.Empty;
                        }
                        catch
                        {
                            // If the column doesn't exist, add it (handles case where some objects have extra properties)
                            if (!dt.Columns.Contains(property.Key))
                            {
                                dt.Columns.Add(property.Key);
                            }
                            row[property.Key] = property.Value?.ToString() ?? string.Empty;
                        }
                    }
                    dt.Rows.Add(row);
                }
            }
            catch (JsonException ex)
            {
                throw new Exception("Error parsing JSON: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error converting JSON to DataTable: " + ex.Message);
            }

            return dt;
        }


    }


  
}
