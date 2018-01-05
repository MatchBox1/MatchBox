using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class TerminalModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public long TerminalNumber { get; set; }

        public string TerminalName { get; set; }
        public string ErrorMessage { get; set; }

        public TerminalModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            CompanyID = 0;
            TerminalNumber = 0;

            TerminalName = "";
            ErrorMessage = "";
        }
    }

    public class TerminalAction
    {
        public static void Select(ref DataTable o_data_table, ref string s_error, int n_user_id, int n_company_id)
        {
            SqlCommand o_command = new SqlCommand("sprTerminal", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = n_company_id;

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);
        }

        public static void Update(ref TerminalModel o_terminal)
        {
            bool b_exists = TerminalNumber_Exists(o_terminal);

            if (o_terminal.ErrorMessage != "") { return; }

            if (b_exists)
            {
                o_terminal.ErrorMessage = "Terminal number already exists.";
                return;
            }

            SqlCommand o_command = new SqlCommand("sprTerminalInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_terminal.UserID;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = o_terminal.CompanyID;

            o_command.Parameters.Add("@TerminalNumber", SqlDbType.BigInt);
            o_command.Parameters["@TerminalNumber"].Value = o_terminal.TerminalNumber;

            o_command.Parameters.Add("@TerminalName", SqlDbType.NVarChar, 20);
            o_command.Parameters["@TerminalName"].Value = o_terminal.TerminalName;

            try
            {
                o_command.Connection.Open();

                o_terminal.ID = (int)o_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                o_terminal.ErrorMessage = ex.Message;
                o_terminal.ErrorMessage = "Error on update terminal.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Table(int n_user_id, int n_company_id, DataTable dt_terminal, ref int n_rows_affected, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprTerminalInsertTable", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = n_company_id;

            o_command.Parameters.AddWithValue("@TableTerminal", dt_terminal);
            o_command.Parameters["@TableTerminal"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update terminal table.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        private static bool TerminalNumber_Exists(TerminalModel o_terminal)
        {
            bool b_exists = false;

            SqlCommand o_command = new SqlCommand("sprTerminalCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_terminal.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_terminal.UserID;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = o_terminal.CompanyID;

            o_command.Parameters.Add("@TerminalNumber", SqlDbType.BigInt);
            o_command.Parameters["@TerminalNumber"].Value = o_terminal.TerminalNumber;

            try
            {
                o_command.Connection.Open();
                b_exists = (int)o_command.ExecuteScalar() > 0;
            }
            catch (Exception ex)
            {
                o_terminal.ErrorMessage = ex.Message;
                o_terminal.ErrorMessage = "Error on validate terminal number.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_exists;
        }

        public static bool Delete(ref TerminalModel o_terminal)
        {
            bool b_deleted = false;

            SqlCommand o_command = new SqlCommand("sprTerminalDelete", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_terminal.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_terminal.UserID;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = o_terminal.CompanyID;

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                o_terminal.ErrorMessage = ex.Message;
                o_terminal.ErrorMessage = "Error on delete terminal.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_deleted;
        }
    }
}