using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class CashBoxModel
    {
        public static string InterfaceName
        {
            get { return "CASHBOX"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int NetworkID { get; set; }
        public int BranchID { get; set; }

        public string CashBoxNumber { get; set; }
        public string CashBoxName { get; set; }
        public string ErrorMessage { get; set; }

        public CashBoxModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            CompanyID = 0;
            NetworkID = 0;
            BranchID = 0;

            CashBoxNumber = "";
            CashBoxName = "";
            ErrorMessage = "";
        }
    }

    public class CashBoxAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxSelectList", DB.Get_Connection());
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
                        case "BranchID":
                        case "PageNumber":
                        case "PageSize":
                            o_db_type = SqlDbType.Int;
                            break;
                        case "CashBoxName":
                            o_db_type = SqlDbType.NVarChar;
                            n_size = 50;
                            break;
                        case "OrderBy":
                            o_db_type = SqlDbType.VarChar;
                            n_size = 50;
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

        public static void Select(ref CashBoxModel o_cash_box)
        {
            if (o_cash_box.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprCashBoxSelect", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_cash_box.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_cash_box.UserID;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_cash_box.CompanyID = (int)o_reader["CompanyID"];
                    o_cash_box.NetworkID = (int)o_reader["NetworkID"];
                    o_cash_box.BranchID = (int)o_reader["BranchID"];

                    o_cash_box.CashBoxNumber = o_reader["CashBoxNumber"].ToString();
                    o_cash_box.CashBoxName = o_reader["CashBoxName"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_cash_box.ErrorMessage = ex.Message;
                o_cash_box.ErrorMessage = "Error on select Cashbox.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Check(ref CashBoxModel o_cash_box)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_cash_box.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_cash_box.UserID;

            o_command.Parameters.Add("@BranchID", SqlDbType.Int);
            o_command.Parameters["@BranchID"].Value = o_cash_box.BranchID;

            o_command.Parameters.Add("@CashBoxNumber", SqlDbType.VarChar, 15);
            o_command.Parameters["@CashBoxNumber"].Value = o_cash_box.CashBoxNumber;

            try
            {
                o_command.Connection.Open();

                o_cash_box.ErrorMessage = o_command.ExecuteScalar().ToString().Trim();
            }
            catch (Exception ex)
            {
                o_cash_box.ErrorMessage = ex.Message;
                o_cash_box.ErrorMessage = "Error on check cashbox.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref CashBoxModel o_cash_box)
        {
            Check(ref o_cash_box);

            if (o_cash_box.ErrorMessage != "") { return; }

            string s_procedure = "";

            if (o_cash_box.ID == 0) { s_procedure = "sprCashBoxInsert"; } else { s_procedure = "sprCashBoxUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_cash_box.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_cash_box.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_cash_box.UserID;

            o_command.Parameters.Add("@BranchID", SqlDbType.Int);
            o_command.Parameters["@BranchID"].Value = o_cash_box.BranchID;

            o_command.Parameters.Add("@CashBoxNumber", SqlDbType.VarChar, 15);
            o_command.Parameters["@CashBoxNumber"].Value = o_cash_box.CashBoxNumber;

            o_command.Parameters.Add("@CashBoxName", SqlDbType.NVarChar, 50);
            o_command.Parameters["@CashBoxName"].Value = o_cash_box.CashBoxName;

            try
            {
                o_command.Connection.Open();

                if (o_cash_box.ID == 0)
                {
                    o_cash_box.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_cash_box.ErrorMessage = ex.Message;
                o_cash_box.ErrorMessage = "Error on update Cashbox.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Table(int n_user_id, DataTable dt_cashbox, ref int n_rows_affected, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxInsertTable", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.AddWithValue("@TableCashBox", dt_cashbox);
            o_command.Parameters["@TableCashBox"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update cashbox table.";
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
