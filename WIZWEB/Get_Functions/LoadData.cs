using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Runtime.Serialization.Formatters.Binary;
using FluentFTP;
using FluentFTP.Exceptions;



namespace WIZWEB.Get_Functions
{
    public class LoadData
    {
        private readonly string FTP_USERNAME = "ltadmin";
        private readonly string FTP_PASSWORD = "@)!#eroC@)@!";

        public string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z\._]", string.Empty);
        }

        public async Task FTPUploadAsync(string fileContent, string ftpFileName)
        {
            string url = $"ftp://52.172.13.58/Wiz/WIZAPI/JSONS/{ftpFileName}";

            byte[] fileBytes = Encoding.UTF8.GetBytes(fileContent);
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.ContentLength = fileBytes.Length;

            using var reqStream = await request.GetRequestStreamAsync();
            await reqStream.WriteAsync(fileBytes, 0, fileBytes.Length);
        }

        public async Task PDFUploadAsync(string path, byte[] fileBytes)
        {
            var request = (FtpWebRequest)WebRequest.Create(path);
            request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.ContentLength = fileBytes.Length;

            try
            {
                // Try to delete existing file if it exists
                var checkRequest = (FtpWebRequest)WebRequest.Create(path);
                checkRequest.Credentials = request.Credentials;
                checkRequest.Method = WebRequestMethods.Ftp.GetFileSize;

                using (var response = (FtpWebResponse)await checkRequest.GetResponseAsync()) { }

                var deleteRequest = (FtpWebRequest)WebRequest.Create(path);
                deleteRequest.Credentials = request.Credentials;
                deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                using (var response = (FtpWebResponse)await deleteRequest.GetResponseAsync()) { }
            }
            catch (WebException ex) when (((FtpWebResponse)ex.Response).StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
            {
                // File doesn't exist — that's fine
            }

            using var reqStream = await request.GetRequestStreamAsync();
            await reqStream.WriteAsync(fileBytes, 0, fileBytes.Length);
        }

        public async Task<string> PostdataAsync(string base64Pdf, string path, string branch, string txtin, string vocType, string voucherNo)
        {
            var reqData = new
            {
                request = new[]
                {
                new {
                    ts = "",
                    txn = $"TXN{txtin}",
                    command = "signpdf",
                    certificate = new[] {
                        new {
                            serial = "278190BF22",
                            dscuid = "",
                            variable = ""
                        }
                    },
                    options = new[] {
                        new {
                            page = "1",
                            cood = vocType switch
                            {
                                "Bill of Supply" => "410,230,560,190",
                                "Invoice FC" => "410,185,530,150",
                                _ => "410,205,550,165"
                            },
                            locked = "yes",
                            reason = vocType,
                            location = branch,
                            customtext = "Signed by VELKUMAR SUBRAMANIAN",
                            greenticked = "Yes",
                            enabletimestamp = "no",
                            dateformat = DateTime.Now.ToString(),
                            enableltv = "no"
                        }
                    },
                    responseurl = "",
                    pdfurl = "",
                    pdf64 = base64Pdf
                }
            }
            };

            var json = JsonConvert.SerializeObject(reqData);
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.Add("Token", "B892831AC10E28D3ED6649B6319F389F7AF9A3B7");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            // TODO: Handle `result` and parse as needed. For now just return it.
            return result;
        }

        public void PDFUpload(string path, byte[] Fileupload)
        {
            FtpWebRequest response = (FtpWebRequest)WebRequest.Create(path);
            FtpWebRequest req = (FtpWebRequest)(WebRequest.Create(path));
            req.Credentials = new NetworkCredential("ltadmin", "@)!#eroC@)@!");
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Proxy = null;
            response.Credentials = new NetworkCredential("ltadmin", "@)!#eroC@)@!");
            response.Method = WebRequestMethods.Ftp.GetFileSize;
            response.Proxy = null;
            try
            {
                FtpWebResponse responsee = (FtpWebResponse)response.GetResponse();
                FtpWebRequest reqdel = (FtpWebRequest)(WebRequest.Create(path));
                reqdel.Credentials = new NetworkCredential("ltadmin", "@)!#eroC@)@!");
                reqdel.Method = WebRequestMethods.Ftp.DeleteFile;
                reqdel.Proxy = null;
                FtpWebResponse responsedel = (FtpWebResponse)reqdel.GetResponse();
            }
            catch (WebException ex)
            {
                FtpWebResponse responsee = (FtpWebResponse)ex.Response;
                if (responsee.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                }
            }
            // byte[] files = Encoding.UTF8.GetBytes(Fileupload);
            req.ContentLength = Fileupload.Length;

            using (Stream requestStream = req.GetRequestStream())
            {
                requestStream.Write(Fileupload, 0, Fileupload.Length);
            }
            return;
        }

        //public void PDFUpload(string path, byte[] fileUpload)
        //{
        //    string username = "ltadmin";
        //    string password = "@)!#eroC@)@!";

        //    try
        //    {
        //        // Check if file exists
        //        bool fileExists = false;
        //        try
        //        {
        //            FtpWebRequest checkRequest = (FtpWebRequest)WebRequest.Create(path);
        //            checkRequest.Credentials = new NetworkCredential(username, password);
        //            checkRequest.Method = WebRequestMethods.Ftp.GetFileSize;
        //            checkRequest.Proxy = null;

        //            using var checkResponse = (FtpWebResponse)checkRequest.GetResponse();
        //            fileExists = true;
        //        }
        //        catch (WebException ex)
        //        {
        //            if (ex.Response is FtpWebResponse ftpResponse &&
        //                ftpResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
        //            {
        //                fileExists = false; // File doesn't exist, this is expected.
        //            }
        //            else
        //            {
        //                throw; // Other FTP error
        //            }
        //        }

        //        // Delete if file already exists
        //        if (fileExists)
        //        {
        //            FtpWebRequest deleteRequest = (FtpWebRequest)WebRequest.Create(path);
        //            deleteRequest.Credentials = new NetworkCredential(username, password);
        //            deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
        //            deleteRequest.Proxy = null;

        //            using var deleteResponse = (FtpWebResponse)deleteRequest.GetResponse();
        //        }

        //        // Upload the file
        //        FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(path);
        //        uploadRequest.Credentials = new NetworkCredential(username, password);
        //        uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
        //        uploadRequest.Proxy = null;
        //        uploadRequest.UseBinary = true;
        //        uploadRequest.UsePassive = true; // Passive mode - recommended unless server has issues
        //        uploadRequest.KeepAlive = false;
        //        uploadRequest.ContentLength = fileUpload.Length;
        //        using Stream requestStream = uploadRequest.GetRequestStream();
        //        requestStream.Write(fileUpload, 0, fileUpload.Length);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle/log exception as needed
        //        throw new ApplicationException("FTP upload failed", ex);
        //    }
        //}

        //public void PDFUpload(string path, byte[] fileUpload)
        //{
        //    string username = "ltadmin";
        //    string password = "@)!#eroC@)@!";

        //    string host = new Uri(path).Host;
        //    string remotePath = new Uri(path).AbsolutePath;

        //    try
        //    {
        //        using var client = new FtpClient(host, new NetworkCredential(username, password));
        //        client.Config.EncryptionMode = FtpEncryptionMode.None;
        //        client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;
        //        client.Connect();

        //        // Delete if file exists
        //        if (client.FileExists(remotePath))
        //        {
        //            client.DeleteFile(remotePath);
        //        }

        //        using var ms = new MemoryStream(fileUpload);
        //        var result = client.UploadStream(ms, remotePath, FtpRemoteExists.Overwrite);

        //        Console.WriteLine($"Upload status: {result}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException("FTP upload failed", ex);
        //    }
        //}



        public string Download(string custData, string fileName)
        {
            // Use a clean timestamp format
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Construct the final file name with timestamp
            string ftpFile = $"{fileName}{timestamp}.json";

            // Remove any special characters if necessary
            string output = RemoveSpecialChars(ftpFile);

            // Upload the file using your existing method
            FTPUpload(custData, output);

            // Build full FTP file path (still assuming local path; adjust for actual FTP server if needed)
            string ftpDocPath = Path.Combine(@"E:\FTP\Wiz\WIZAPI\JSONS", ftpFile);

            return ftpDocPath;
        }
        public void FTPUpload(string json, string ftpFileName)
        {
            string ftpUrl = $"ftp://52.172.13.58/Wiz/WIZAPI/JSONS/{ftpFileName}";
            NetworkCredential credentials = new NetworkCredential("ltadmin", "@)!#eroC@)@!");

            // Step 1: Check if the file already exists, and delete it if so
            try
            {
                FtpWebRequest checkRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                checkRequest.Credentials = credentials;
                checkRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                checkRequest.Proxy = null;

                using (FtpWebResponse checkResponse = (FtpWebResponse)checkRequest.GetResponse())
                {
                    // If exists, delete it
                    FtpWebRequest deleteRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                    deleteRequest.Credentials = credentials;
                    deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                    deleteRequest.Proxy = null;

                    using FtpWebResponse deleteResponse = (FtpWebResponse)deleteRequest.GetResponse();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is FtpWebResponse ftpResponse &&
                    ftpResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    // File does not exist — that's OK, continue to upload
                }
                else
                {
                    throw; // Rethrow any other exceptions
                }
            }

            // Step 2: Upload the file
            FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
            uploadRequest.Credentials = credentials;
            uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
            uploadRequest.Proxy = null;

            byte[] fileBytes = Encoding.UTF8.GetBytes(json);
            uploadRequest.ContentLength = fileBytes.Length;

            using (Stream requestStream = uploadRequest.GetRequestStream())
            {
                requestStream.Write(fileBytes, 0, fileBytes.Length);
            }

            using (FtpWebResponse uploadResponse = (FtpWebResponse)uploadRequest.GetResponse())
            {
                // You can log uploadResponse.StatusDescription if needed
            }
        }


        
        public class certificate
        {
            public string serial { get; set; }

            public string dscuid { get; set; }

            public string variable { get; set; }
        }

        public class requestdata
        {
            public string ts { get; set; }

            public string txn { get; set; }

            public string command { get; set; }

            public string responseurl { get; set; }

            public string pdfurl { get; set; }

            public string pdf64 { get; set; }
        }

        public class options
        {
            public string page { get; set; }
            public string cood { get; set; }
            public string locked { get; set; }
            public string reason { get; set; }
            public string location { get; set; }
            public string customtext { get; set; } //custometext
            public string greenticked { get; set; }
            public string enabletimestamp { get; set; }
            public string dateformat { get; set; }
            public string enableltv { get; set; }
        }
        public string Postdata(string BASE64STRING, string path, string branch, string TXTIN, string voctype, string VoucherNo)
        {
            string jsonfile = "\"" + BASE64STRING + "\"";
            string datetime = Convert.ToString(DateTime.Now);
            requestdata rd = new requestdata();
            certificate cert = new certificate();
            options op = new options();

            rd.ts = "";
            rd.txn = "TXN" + TXTIN;
            rd.command = "signpdf";
            rd.responseurl = "";
            rd.pdfurl = "";
            rd.pdf64 = BASE64STRING;
            cert.serial = "278190BF22"; //WizlogTec
            cert.dscuid = "";
            cert.variable = "";
            op.page = "1";

            if (voctype == "Bill of Supply")
            {
                op.cood = "410,230,560,190";
            }
            else if (voctype == "Invoice FC")
            {
                op.cood = "410,185,530,150";
            }
            else
            {
                op.cood = "410,205,550,165";
            }

            op.locked = "yes";
            op.reason = voctype;
            op.location = branch;
            op.customtext = "Signed by VELKUMAR SUBRAMANIAN";
            op.greenticked = "Yes";
            op.enabletimestamp = "no";
            op.dateformat = datetime;
            op.enableltv = "no";

            var loadjson = new
            {
                request = new[]{
            new {
                ts = rd.ts,
                txn = rd.txn,
                command = rd.command,
                certificate = new[] {
                    new {serial = cert.serial, dscuid = cert.dscuid, variable = cert.variable}
                },
                options = new[] {
                    new {
                        page = op.page,
                        cood = op.cood,
                        @lock = op.locked,
                        reason = op.reason,
                        location = op.location,
                        customtext = op.customtext,
                        greenticked = op.greenticked,
                        enabletimestamp = op.enabletimestamp,
                        dateformat = op.dateformat,
                        enableltv = op.enableltv
                    }
                },
                responseurl = rd.responseurl,
                pdfurl = rd.responseurl,
                pdf64 = rd.pdf64
            }
        }
            };

            string json = JsonConvert.SerializeObject(loadjson);

            HttpWebRequest requestObject = (HttpWebRequest)WebRequest.Create(path);
            requestObject.Method = "POST";
            requestObject.Headers["Token"] = "B892831AC10E28D3ED6649B6319F389F7AF9A3B7";
            requestObject.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(requestObject.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            string results = "";
            using (var httpResponse = (HttpWebResponse)requestObject.GetResponse())
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                results = result;
                string bas = result.ToString();
                string[] data = bas.Split(':');
                string pdfGenerate = data[4].ToString();
                string error = data[2].ToString();
                string[] errorMessage = error.Split('"');
                string ErrorCode = errorMessage[1].ToString();
                string responceDetail = data[3].ToString();
                string[] ResponceDetailspdf = responceDetail.Split('"');
                string ResponceMessage = ResponceDetailspdf[1].ToString();
                string[] pdf = pdfGenerate.Split('"');
                string pdfFile = pdf[1].ToString();
                string base64strings = pdfFile.Replace('\\', ' ');

                WIZWEB.DAL.BAL sqlHelper = new WIZWEB.DAL.BAL();
                Dictionary<string, object> objEstapp3 = new Dictionary<string, object>();
                objEstapp3.Add("@Result", results);
                objEstapp3.Add("@VocherType", voctype);
                objEstapp3.Add("@JsonResponce", json);
                objEstapp3.Add("@VoucherNo", VoucherNo);
                objEstapp3.Add("@ErrorMessage", ResponceMessage);
                objEstapp3.Add("@ErrorCode", ErrorCode);
                sqlHelper.ExecuteNonQuery("Sp_CapriconApiRequest", objEstapp3);
                return base64strings;
            }
        }



    }
}
