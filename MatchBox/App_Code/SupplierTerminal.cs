using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class SupplierTerminalModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgreementID { get; set; }
        public int AgreementPeriodID { get; set; }
        public int SupplierID { get; set; }
        public int TerminalID { get; set; }

        public string ErrorMessage { get; set; }

        public SupplierTerminalModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            AgreementID = 0;
            AgreementPeriodID = 0;
            SupplierID = 0;
            TerminalID = 0;

            ErrorMessage = "";
        }
    }

    public class SupplierTerminalAction
    {
        public static void Select(ref DataTable o_data_table, ref string s_error, int n_user_id, int n_agreement_id)
        {
            SqlCommand o_command = new SqlCommand("sprSupplierTerminal", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = n_agreement_id;

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);
        }

        public static void Select_Period(ref DataTable o_data_table, ref string s_error, int n_user_id, int n_agreement_id, DateTime d_start, DateTime d_end)
        {
            SqlCommand o_command = new SqlCommand("sprSupplierTerminalPeriod", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = n_agreement_id;

            o_command.Parameters.Add("@DateStart", SqlDbType.Date);
            o_command.Parameters["@DateStart"].Value = d_start;

            o_command.Parameters.Add("@DateEnd", SqlDbType.Date);
            o_command.Parameters["@DateEnd"].Value = d_end;

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);
        }

        public static void Update(ref SupplierTerminalModel o_supplier_terminal)
        {
            bool b_exists = SupplierTerminal_Exists(ref o_supplier_terminal);

            if (o_supplier_terminal.ErrorMessage != "") { return; }

            if (b_exists)
            {
                o_supplier_terminal.ErrorMessage = "Terminal already in use.";
                return;
            }

            SqlCommand o_command = new SqlCommand("sprSupplierTerminalInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_supplier_terminal.UserID;

            o_command.Parameters.Add("@AgreementPeriodID", SqlDbType.Int);
            o_command.Parameters["@AgreementPeriodID"].Value = o_supplier_terminal.AgreementPeriodID;

            o_command.Parameters.Add("@SupplierID", SqlDbType.Int);
            o_command.Parameters["@SupplierID"].Value = o_supplier_terminal.SupplierID;

            o_command.Parameters.Add("@TerminalID", SqlDbType.Int);
            o_command.Parameters["@TerminalID"].Value = o_supplier_terminal.TerminalID;

            try
            {
                o_command.Connection.Open();

                o_supplier_terminal.ID = (int)o_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                o_supplier_terminal.ErrorMessage = ex.Message;
                o_supplier_terminal.ErrorMessage = "Error on update Supplier-Terminal.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        private static bool SupplierTerminal_Exists(ref SupplierTerminalModel o_supplier_terminal)
        {
            bool b_exists = false;

            SqlCommand o_command = new SqlCommand("sprSupplierTerminalCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_supplier_terminal.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_supplier_terminal.UserID;

            o_command.Parameters.Add("@AgreementPeriodID", SqlDbType.Int);
            o_command.Parameters["@AgreementPeriodID"].Value = o_supplier_terminal.AgreementPeriodID;

            o_command.Parameters.Add("@TerminalID", SqlDbType.Int);
            o_command.Parameters["@TerminalID"].Value = o_supplier_terminal.TerminalID;

            try
            {
                o_command.Connection.Open();
                b_exists = (int)o_command.ExecuteScalar() > 0;
            }
            catch (Exception ex)
            {
                o_supplier_terminal.ErrorMessage = ex.Message;
                o_supplier_terminal.ErrorMessage = "Error on validate supplier-terminal.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_exists;
        }

        public static bool Delete(int n_id, int n_user_id, ref string s_error)
        {
            bool b_deleted = false;

            SqlCommand o_command = new SqlCommand("sprSupplierTerminalDelete", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = n_id;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete Supplier-Terminal.";
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