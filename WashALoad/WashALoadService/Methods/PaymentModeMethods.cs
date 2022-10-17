using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WashALoadService.Common;
using WashALoadService.Models;

namespace WashALoadService.Methods
{
    public class PaymentModeMethods
    {
        internal AppDb_WashALoad gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal PaymentModeMethods(AppDb_WashALoad db)
        {
            gDb = db;
        }
        public PaymentModeMethods() { }

        public async Task<ServiceResponse> FindAllAsync()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `payment_code`, 
	                                            `description`, 
	                                            `non_cash`, 
	                                            `accounting_only`, 
	                                            `float`, 
	                                            `require_proof`
	 
	                                    FROM 
	                                            `laundry`.`payment_modes` ;";

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<PaymentMode>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new PaymentMode();
                        hasValue = true;

                        oEntity.payment_code = oresult.GetString("payment_code");
                        oEntity.description = oresult.GetString("description");
                        oEntity.non_cash = oresult.GetInt32("non_cash");
                        oEntity.non_cash = oresult.GetInt32("non_cash");
                        oEntity.accounting_only = oresult.GetInt32("accounting_only");
                        oEntity.Float = oresult.GetInt32("float");
                        oEntity.require_proof = oresult.GetInt32("require_proof");
                        oEntities.Add(oEntity);
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(oEntities);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> FindOneAsync(string payment_code)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `payment_code`, 
	                                            `description`, 
	                                            `non_cash`, 
	                                            `accounting_only`, 
	                                            `float`, 
	                                            `require_proof`
	 
	                                    FROM 
	                                            `laundry`.`payment_modes`
                                        WHERE payment_code = @payment_code;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@payment_code", System.Data.DbType.String, payment_code);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<PaymentMode>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new PaymentMode();
                        hasValue = true;

                        oEntity.payment_code = oresult.GetString("payment_code");
                        oEntity.description = oresult.GetString("description");
                        oEntity.non_cash = oresult.GetInt32("non_cash");
                        oEntity.non_cash = oresult.GetInt32("non_cash");
                        oEntity.accounting_only = oresult.GetInt32("accounting_only");
                        oEntity.Float = oresult.GetInt32("float");
                        oEntity.require_proof = oresult.GetInt32("require_proof");
                        oEntities.Add(oEntity);
                    }
                }

                if (hasValue == false)
                {
                    serviceResponse.SetValues(404, "No data found", "");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(oEntities);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse> SavePaymentModeAsync(PaymentMode paymentMode)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO  `laundry`.`payment_modes`
                                            (
                                                `payment_code`, 
	                                            `description`, 
	                                            `non_cash`, 
	                                            `accounting_only`, 
	                                            `float`, 
	                                            `require_proof`
                                            )
	 
	                                    VALUES 
	                                         (
                                                @payment_code,
                                                @description,
                                                @non_cash,
                                                @accounting_only,
                                                @float,
                                                @require_proof
                                             );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@payment_code", System.Data.DbType.String, paymentMode.payment_code);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, paymentMode.description);
                commonFunctions.BindParameter(oCommand, "@non_cash", System.Data.DbType.Int32, paymentMode.non_cash);
                commonFunctions.BindParameter(oCommand, "@accounting_only", System.Data.DbType.Int32, paymentMode.accounting_only);
                commonFunctions.BindParameter(oCommand, "@float", System.Data.DbType.Int32, paymentMode.Float);
                commonFunctions.BindParameter(oCommand, "@require_proof", System.Data.DbType.Int32, paymentMode.require_proof);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(paymentMode);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdatePaymentModeAsync(PaymentMode paymentMode)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE  `laundry`.`payment_modes`
                                            SET
                                                `description` = @description, 
	                                            `non_cash` =  @non_cash, 
	                                            `accounting_only` = @accounting_only, 
	                                            `float` =  @float, 
	                                            `require_proof` = @require_proof
                                            WHERE
	                                            `payment_code` = @payment_code;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@payment_code", System.Data.DbType.String, paymentMode.payment_code);
                commonFunctions.BindParameter(oCommand, "@description", System.Data.DbType.String, paymentMode.description);
                commonFunctions.BindParameter(oCommand, "@non_cash", System.Data.DbType.Int32, paymentMode.non_cash);
                commonFunctions.BindParameter(oCommand, "@accounting_only", System.Data.DbType.Int32, paymentMode.accounting_only);
                commonFunctions.BindParameter(oCommand, "@float", System.Data.DbType.Int32, paymentMode.Float);
                commonFunctions.BindParameter(oCommand, "@require_proof", System.Data.DbType.Int32, paymentMode.require_proof);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(paymentMode);

                    serviceResponse.SetValues(200, "Success", jsonString);
                }
                else
                {
                    serviceResponse.SetValues(500, "Could not process request. Please try again later.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeletePaymentModeAsync(string paymentCode)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `laundry`.`payment_modes`
                                            WHERE
	                                            `payment_code` = @payment_code;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@payment_code", System.Data.DbType.String, paymentCode);

                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    serviceResponse.SetValues(200, "Success", "");
                }
                else
                {
                    serviceResponse.SetValues(400, "Could not delete Payment Mode because it does not existed or it is being used.", "");
                }

            }
            catch (Exception ex)
            {
                commonFunctions.CreateLog(ex.ToString());
                serviceResponse.SetValues(500, "Internal Server Error", "");
            }

            return serviceResponse;
        }


    }
}
