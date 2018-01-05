using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class BranchModel
    {
        public static string InterfaceName
        {
            get { return "BRANCH"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int NetworkID { get; set; }
        public int CityID { get; set; }

        public string BranchNumber { get; set; }
        public string BranchName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ErrorMessage { get; set; }

        public BranchModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            CompanyID = 0;
            NetworkID = 0;
            CityID = 0;

            BranchNumber = "";
            BranchName = "";
            Phone = "";
            Fax = "";
            Mail = "";
            City = "";
            Address = "";
            ErrorMessage = "";
        }
    }

    public class BranchAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprBranchSelectList", DB.Get_Connection());
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
                        case "CompanyID":
                        case "NetworkID":
                        case "CityID":
                        case "PageNumber":
                        case "PageSize":
                            o_db_type = SqlDbType.Int;
                            break;
                        case "BranchName":
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

        public static void Select(ref BranchModel o_branch)
        {
            if (o_branch.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprBranchSelect", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_branch.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_branch.UserID;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_branch.CompanyID = (int)o_reader["CompanyID"];
                    o_branch.NetworkID = (int)o_reader["NetworkID"];
                    o_branch.CityID = (int)o_reader["CityID"];

                    o_branch.BranchNumber = o_reader["BranchNumber"].ToString();
                    o_branch.BranchName = o_reader["BranchName"].ToString();
                    o_branch.Phone = o_reader["Phone"].ToString();
                    o_branch.Fax = o_reader["Fax"].ToString();
                    o_branch.Mail = o_reader["Mail"].ToString();
                    o_branch.City = o_reader["City"].ToString();
                    o_branch.Address = o_reader["Address"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_branch.ErrorMessage = ex.Message;
                o_branch.ErrorMessage = "Error on select branch.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Check(ref BranchModel o_branch)
        {
            SqlCommand o_command = new SqlCommand("sprBranchCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_branch.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_branch.UserID;

            o_command.Parameters.Add("@NetworkID", SqlDbType.Int);
            o_command.Parameters["@NetworkID"].Value = o_branch.NetworkID;

            o_command.Parameters.Add("@BranchNumber", SqlDbType.VarChar, 15);
            o_command.Parameters["@BranchNumber"].Value = o_branch.BranchNumber;

            try
            {
                o_command.Connection.Open();

                o_branch.ErrorMessage = o_command.ExecuteScalar().ToString().Trim();
            }
            catch (Exception ex)
            {
                o_branch.ErrorMessage = ex.Message;
                o_branch.ErrorMessage = "Error on check branch.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref BranchModel o_branch)
        {
            Check(ref o_branch);

            if (o_branch.ErrorMessage != "") { return; }

            string s_procedure = "";

            if (o_branch.ID == 0) { s_procedure = "sprBranchInsert"; } else { s_procedure = "sprBranchUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_branch.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_branch.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_branch.UserID;

            o_command.Parameters.Add("@NetworkID", SqlDbType.Int);
            o_command.Parameters["@NetworkID"].Value = o_branch.NetworkID;

            o_command.Parameters.Add("@CityID", SqlDbType.Int);
            o_command.Parameters["@CityID"].Value = o_branch.CityID;

            o_command.Parameters.Add("@BranchNumber", SqlDbType.VarChar, 15);
            o_command.Parameters["@BranchNumber"].Value = o_branch.BranchNumber;

            o_command.Parameters.Add("@BranchName", SqlDbType.NVarChar, 50);
            o_command.Parameters["@BranchName"].Value = o_branch.BranchName;

            o_command.Parameters.Add("@Phone", SqlDbType.VarChar, 15);
            o_command.Parameters["@Phone"].Value = o_branch.Phone;

            o_command.Parameters.Add("@Fax", SqlDbType.VarChar, 15);
            o_command.Parameters["@Fax"].Value = o_branch.Fax;

            o_command.Parameters.Add("@Mail", SqlDbType.VarChar, 50);
            o_command.Parameters["@Mail"].Value = o_branch.Mail;

            o_command.Parameters.Add("@Address", SqlDbType.NVarChar, 50);
            o_command.Parameters["@Address"].Value = o_branch.Address;

            try
            {
                o_command.Connection.Open();

                if (o_branch.ID == 0)
                {
                    o_branch.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_branch.ErrorMessage = ex.Message;
                o_branch.ErrorMessage = "Error on update branch.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Table(int n_user_id, DataTable dt_branch, ref int n_rows_affected, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprBranchInsertTable", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.AddWithValue("@TableBranch", dt_branch);
            o_command.Parameters["@TableBranch"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update branch table.";
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
