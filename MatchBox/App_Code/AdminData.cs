using System;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class AdminDataModel
    {
        public static string InterfaceName
        {
            get { return "ADMINDATA"; }
        }

        public int CityID { get; set; }

        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ErrorMessage { get; set; }

        public AdminDataModel()
        {
            Reset();
        }

        public void Reset()
        {
            CityID = 0;

            CompanyName = "";
            Phone = "";
            Fax = "";
            Mail = "";
            City = "";
            Address = "";
            ErrorMessage = "";
        }
    }

    public class AdminDataAction
    {
        public static void Select(ref AdminDataModel o_admin_data)
        {
            SqlCommand o_command = new SqlCommand("sprMatchBoxSelect", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_admin_data.CityID = (int)o_reader["CityID"];

                    o_admin_data.CompanyName = o_reader["CompanyName"].ToString();
                    o_admin_data.Phone = o_reader["Phone"].ToString();
                    o_admin_data.Fax = o_reader["Fax"].ToString();
                    o_admin_data.Mail = o_reader["Mail"].ToString();
                    o_admin_data.City = o_reader["City"].ToString();
                    o_admin_data.Address = o_reader["Address"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_admin_data.ErrorMessage = ex.Message;
                o_admin_data.ErrorMessage = "Error on select company.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref AdminDataModel o_admin_data)
        {
            SqlCommand o_command = new SqlCommand("sprMatchBoxUpdate", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@CityID", SqlDbType.Int) { Value = o_admin_data.CityID });
            o_command.Parameters.Add(new SqlParameter("@CompanyName", SqlDbType.NVarChar, 50) { Value = o_admin_data.CompanyName });
            o_command.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 10) { Value = o_admin_data.Phone });
            o_command.Parameters.Add(new SqlParameter("@Fax", SqlDbType.VarChar, 10) { Value = o_admin_data.Fax });
            o_command.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar, 50) { Value = o_admin_data.Mail });
            o_command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 50) { Value = o_admin_data.Address });

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                o_admin_data.ErrorMessage = ex.Message;
                o_admin_data.ErrorMessage = "Error on update company.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }
    }
}
