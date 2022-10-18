
using System;
using inventory.Models;
using System.Collections.Generic;
using inventory.Common;
using System.Threading.Tasks;
using System.Text.Json;

namespace inventory.Methods
{
   public class Entities_BarangayMethods
   {

        internal AppDb gDb { get; set; }

        internal CommonFunctions commonFunctions = new CommonFunctions();
        internal Entities_BarangayMethods(AppDb db)
        {
            gDb = db;
        }

        public async Task<ServiceResponse> SaveBarangay(Entities_BarangayDistrictsModel barangay)
         {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                 serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"INSERT INTO `entities`.`barangays_districts` 
	                                        (
                                                `barangay_district_name`
	                                        )
	                                     VALUES
                                            (
                                                @barangay_district_name
                                            );";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@barangay_district_name", System.Data.DbType.String, barangay.barangay_district_name);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    barangay.barangay_district_id = oCommand.LastInsertedId;
                    string jsonString = JsonSerializer.Serialize(barangay);

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

        public async Task<ServiceResponse> GetAllBarangay(int townId)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `barangay_district_id`, 
	                                            `barangay_district_name`
	 
	                                    FROM 
	                                        `entities`.`barangays_districts`
                                        WHERE town_id = @town_id";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@town_id", System.Data.DbType.Int32, townId);



                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_BarangayDistrictsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_BarangayDistrictsModel();
                        hasValue = true;

                        oEntity.barangay_district_id = oresult.GetInt32("barangay_district_id");
                        oEntity.barangay_district_name = oresult.GetString("barangay_district_name");

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

        public async Task<ServiceResponse> GetOneBarangay(long barangay_district_id)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");

                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"SELECT `barangay_district_id`, 
	                                            `barangay_district_name`
	 
	                                    FROM 
	                                        `entities`.`barangays_districts`
                                        WHERE
                                            `barangay_district_id` = @barangay_district_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@barangay_district_id", System.Data.DbType.Int32, barangay_district_id);

                var oresult = await oCommand.ExecuteReaderAsync();

                var oEntities = new List<Entities_BarangayDistrictsModel>();

                bool hasValue = false;

                using (oresult)
                {
                    while (await oresult.ReadAsync())
                    {
                        var oEntity = new Entities_BarangayDistrictsModel();
                        hasValue = true;

                        oEntity.barangay_district_id = oresult.GetInt32("barangay_district_id");
                        oEntity.barangay_district_name = oresult.GetString("barangay_district_name");

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

        public async Task<ServiceResponse> UpdateBarangay(Entities_BarangayDistrictsModel  barangay)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"UPDATE `entities`.`barangays_districts` 
	                                        SET
                                                `barangay_district_name` =  @barangay_district_name
	                                        WHERE
                                                `barangay_district_id` = @barangay_district_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@barangay_district_name", System.Data.DbType.String, barangay.barangay_district_name);
                commonFunctions.BindParameter(oCommand, "@barangay_district_id", System.Data.DbType.Int32, barangay.barangay_district_id);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    string jsonString = JsonSerializer.Serialize(barangay);

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

        public async Task<ServiceResponse> DeleteBarangay(Int32 barangay_district_name)
        {

            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                serviceResponse.SetValues(0, "Initialized", "");
                using var oCommand = gDb.Connection.CreateCommand();
                oCommand.CommandText = @"DELETE FROM  `entities`.`barangays_districts` 
	                                        WHERE
                                                `barangay_district_id` = @barangay_district_id;";

                oCommand.Parameters.Clear();

                commonFunctions.BindParameter(oCommand, "@barangay_district_id", System.Data.DbType.Int32, barangay_district_name);


                int i = await oCommand.ExecuteNonQueryAsync();

                if (i > 0)
                {
                    serviceResponse.SetValues(200, "Success", "");
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
    }
}
 

