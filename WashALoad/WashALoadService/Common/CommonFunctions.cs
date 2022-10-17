using GoogleAuthenticatorService.Core;
using MySqlConnector;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WashALoadService.Common
{
    public class CommonFunctions
    {
        public enum TransactionType : ushort
        {
            Billing = 0,
            Booking = 1,
            Invoice = 2,
            SO = 3
        }

        public void BindParameter(MySqlCommand oCommand, string parameterName, System.Data.DbType dbType, Object value)
        {
            oCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = parameterName,
                DbType = dbType,
                Value = value

            });
        }

        //public async Task<ServiceResponse> SendEmailAsync(string emailAddress, string message)
        //{
        //    ServiceResponse serviceResponse = new ServiceResponse();
        //    try
        //    {
        //        serviceResponse.SetValues(0, "Initialized", "");



        //        int i = 0; ///if sent email is success;

        //        if (i > 0)
        //        {
        //            serviceResponse.SetValues(200, "Success", "");
        //        }
        //        else
        //        {
        //            serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        CreateLog(ex.ToString());
        //        serviceResponse.SetValues(500, ex.Message, "");
        //    }

        //    return serviceResponse;
        //}
        public ServiceResponse GenerateSetupcode(string secretKey)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                TwoFactorAuthenticator Authenticator = new TwoFactorAuthenticator();

                var SetupResult = Authenticator.GenerateSetupCode("Wash A Load", secretKey, 250, 250);

                GoogleAuthenticatorDetail googleAuthenticatorDetail = new GoogleAuthenticatorDetail();
                googleAuthenticatorDetail.qrCodeUrl = SetupResult.QrCodeSetupImageUrl;
                googleAuthenticatorDetail.manualCode = SetupResult.ManualEntryKey;

                string jsonString = JsonSerializer.Serialize(googleAuthenticatorDetail);
                serviceResponse.SetValues(200, "Success", jsonString);

            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                serviceResponse.SetValues(500, ex.Message, "");
            }

            return serviceResponse;
        }

        public ServiceResponse ValidateCode(string secretKey, string code)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                TwoFactorAuthenticator Authenticator = new TwoFactorAuthenticator();
                bool ValidateResult = Authenticator.ValidateTwoFactorPIN(secretKey, code);

                string jsonString = JsonSerializer.Serialize(ValidateResult);
                serviceResponse.SetValues(200, "Success", jsonString);

            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                serviceResponse.SetValues(500, ex.Message, "");
            }

            return serviceResponse;
        }

        public async Task<int> GenerateTransactionNumber(MySqlCommand oCommand, TransactionType type)
        {
            int entryNumber = 0;

            oCommand.CommandText = "";

            try
            {
               if(type == TransactionType.Booking)
                {
                    oCommand.CommandText = @"CALL `laundry`.`SP_GenerateBookingSeries`();";

                }else if(type == TransactionType.Billing)
                {
                    oCommand.CommandText = @"CALL `laundry`.`SP_GenerateBookingSeries`();";

                }
                else if (type == TransactionType.Invoice)
                {
                    oCommand.CommandText = @"CALL `laundry`.`SP_GenerateInvoiceSeries`();";
                }
                else if (type == TransactionType.SO)
                {
                    oCommand.CommandText = @"CALL `laundry`.`SP_GenerateSOSeries`();";
                }

                var oresult = await oCommand.ExecuteReaderAsync();

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {

                        entryNumber = oresult.GetInt32("account_series");

                    }
                }

                return entryNumber;
            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                return entryNumber;
            }
        }

        public async Task<double> ComputeTotalUnBilledBilling(MySqlCommand oCommand, int custId)
        {
            double unbilledAmount = 0.0;

            try
            {
                oCommand.CommandText = @"SELECT 	 SUM(`previous_unpaid_bill_subtotal_amount` +`current_bill_subtotal_amount` + `paid_so_adl_adjustments_subtotal_amount`) AS total 
                                        FROM 
	                                        `laundry`.`billing_references` 
                                        WHERE 
                                            `paid` = 0 AND  DATE(`billing_date`) < DATE(NOW()) AND `customer_id` = @customer_id;";

                oCommand.Parameters.Clear();

                BindParameter(oCommand, "@customer_id", System.Data.DbType.Int32, custId);

                var oresult = await oCommand.ExecuteReaderAsync();

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {

                        unbilledAmount = oresult.GetDouble("total");

                    }
                }

                return unbilledAmount;
            }
            catch (Exception ex)
            {
                return unbilledAmount;
            }
        }

        public String GenerateQR(string qrText)
        {
            //try
            //{
            //    QRCodeGenerator QrGenerator = new QRCodeGenerator();
            //    QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            //    QRCode QrCode = new QRCode(QrCodeInfo);
            //    Bitmap QrBitmap = QrCode.GetGraphic(60);
            //    byte[] BitmapArray = BitmapToByteArray(QrBitmap);
            //    string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));

            //    return QrUri;
            //}
            //catch(Exception ex)
            //{
            //    CreateLog(ex.ToString());
            //    return "";
            //}

            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            byte[] BitmapArray = BitmapToByteArray(QrBitmap);
            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));

            return QrUri;

        }

        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public void CreateLog(string txtLog)
        {
            try
            {
                // Set a variable to the Documents path.
                string docPath = Environment.CurrentDirectory + "/Logs/";

                if (!Directory.Exists(docPath))
                {
                    Directory.CreateDirectory(docPath);
                }

                // Append text to an existing file named "WriteLines.txt".
                string filename = DateTime.Today.ToString("yyyyMMdd") + ".log";

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, filename), true))
                {
                    txtLog = "Date Time:" + DateTime.Today.ToString() + "\nLog:" + txtLog + "\n";
                    outputFile.WriteLine(txtLog);
                }
            }
            catch(Exception ex)
            {

            }
            
        }
    }

    

    
}
