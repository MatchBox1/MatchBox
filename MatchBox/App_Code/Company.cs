using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class CompanyModel
    {
        public static string InterfaceName
        {
            get { return "COMPANY"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int CityID { get; set; }
        public long CompanyNumber { get; set; }

        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ErrorMessage { get; set; }

        public CompanyModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            CityID = 0;
            CompanyNumber = 0;

            CompanyName = "";
            Phone = "";
            Fax = "";
            Mail = "";
            City = "";
            Address = "";
            ErrorMessage = "";
        }
    }

    public class CompanyAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprCompanySelectList", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_query_dictionary != null)
            {
                foreach (string s_key in o_query_dictionary.Keys)
                {
                    SqlDbType o_db_type = new SqlDbType();

                    int n_size = -1;
                    string s_value = o_query_dictionary[s_key];

                    switch (s_key)
                    {
                        case "NoPaging":
                            o_db_type = SqlDbType.Bit;
                            break;
                        case "UserID":
                        case "CityID":
                        case "PageNumber":
                        case "PageSize":
                            o_db_type = SqlDbType.Int;
                            break;
                        case "CompanyNumber":
                            o_db_type = SqlDbType.BigInt;
                            break;
                        case "CompanyName":
                        case "Address":
                            o_db_type = SqlDbType.NVarChar;
                            n_size = 50;
                            break;
                        case "Mail":
                        case "OrderBy":
                            o_db_type = SqlDbType.VarChar;
                            n_size = 50;
                            break;
                        case "Phone":
                        case "Fax":
                            o_db_type = SqlDbType.VarChar;
                            n_size = 15;
                            break;
                    }

                    switch (o_db_type)
                    {
                        case SqlDbType.Bit:
                            o_command.Parameters.Add("@" + s_key, o_db_type);
                            o_command.Parameters["@" + s_key].Value = Convert.ToBoolean(s_value);
                            break;
                        case SqlDbType.Int:
                            o_command.Parameters.Add("@" + s_key, o_db_type);
                            o_command.Parameters["@" + s_key].Value = Convert.ToInt32(s_value);
                            break;
                        case SqlDbType.VarChar:
                        case SqlDbType.NVarChar:
                            o_command.Parameters.Add("@" + s_key, o_db_type, n_size);
                            o_command.Parameters["@" + s_key].Value = s_value;
                            break;
                    }
                }
            }

            DB.Bind_Data_Table(o_command, ref o_data_table, ref n_pages_count, ref s_error);
        }

        public static void Select(ref CompanyModel o_company)
        {
            if (o_company.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprCompanySelect", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_company.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_company.UserID;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_company.CityID = (int)o_reader["CityID"];
                    o_company.CompanyNumber = (long)o_reader["CompanyNumber"];

                    o_company.CompanyName = o_reader["CompanyName"].ToString();
                    o_company.Phone = o_reader["Phone"].ToString();
                    o_company.Fax = o_reader["Fax"].ToString();
                    o_company.Mail = o_reader["Mail"].ToString();
                    o_company.City = o_reader["City"].ToString();
                    o_company.Address = o_reader["Address"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_company.ErrorMessage = ex.Message;
                o_company.ErrorMessage = "Error on select Company.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref CompanyModel o_company)
        {
            bool b_exists = CompanyNumber_Exists(o_company);

            if (o_company.ErrorMessage != "") { return; }

            if (b_exists)
            {
                o_company.ErrorMessage = "Company number already exists.";
                return;
            }

            string s_procedure = "";

            if (o_company.ID == 0) { s_procedure = "sprCompanyInsert"; } else { s_procedure = "sprCompanyUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_company.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_company.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_company.UserID;

            o_command.Parameters.Add("@CityID", SqlDbType.Int);
            o_command.Parameters["@CityID"].Value = o_company.CityID;

            o_command.Parameters.Add("@CompanyNumber", SqlDbType.BigInt);
            o_command.Parameters["@CompanyNumber"].Value = o_company.CompanyNumber;

            o_command.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 50);
            o_command.Parameters["@CompanyName"].Value = o_company.CompanyName;

            o_command.Parameters.Add("@Phone", SqlDbType.VarChar, 15);
            o_command.Parameters["@Phone"].Value = o_company.Phone;

            o_command.Parameters.Add("@Fax", SqlDbType.VarChar, 15);
            o_command.Parameters["@Fax"].Value = o_company.Fax;

            o_command.Parameters.Add("@Mail", SqlDbType.VarChar, 50);
            o_command.Parameters["@Mail"].Value = o_company.Mail;

            o_command.Parameters.Add("@Address", SqlDbType.NVarChar, 50);
            o_command.Parameters["@Address"].Value = o_company.Address;

            try
            {
                o_command.Connection.Open();

                if (o_company.ID == 0)
                {
                    o_company.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_company.ErrorMessage = ex.Message;
                o_company.ErrorMessage = "Error on update company.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        private static bool CompanyNumber_Exists(CompanyModel o_company)
        {
            bool b_exists = false;

            SqlCommand o_command = new SqlCommand("sprCompanyCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_company.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_company.UserID;

            o_command.Parameters.Add("@CompanyNumber", SqlDbType.BigInt);
            o_command.Parameters["@CompanyNumber"].Value = o_company.CompanyNumber;

            try
            {
                o_command.Connection.Open();
                b_exists = (int)o_command.ExecuteScalar() > 0;
            }
            catch (Exception ex)
            {
                o_company.ErrorMessage = ex.Message;
                o_company.ErrorMessage = "Error on validate company number.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_exists;
        }
    }
}
