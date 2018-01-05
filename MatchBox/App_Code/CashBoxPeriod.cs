using System;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class CashBoxPeriodModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CashBoxID { get; set; }
        public int TerminalID { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public string Terminal { get; set; }
        public string ErrorMessage { get; set; }

        public CashBoxPeriodModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            CashBoxID = 0;
            TerminalID = 0;

            DateStart = null;
            DateEnd = null;

            Terminal = "N / A";
            ErrorMessage = "";
        }
    }

    public class CashBoxPeriodAction
    {
        public static void Select(ref DataTable dt_cashbox_period, ref DataTable dt_cashbox_period_terminal, ref string s_error, int n_user_id, int n_cashbox_id)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxPeriod", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@CashBoxID", SqlDbType.Int);
            o_command.Parameters["@CashBoxID"].Value = n_cashbox_id;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_cashbox_period = o_data_set.Tables[0];
                dt_cashbox_period_terminal = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select cashbox period table.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select(ref CashBoxPeriodModel o_cashbox_period)
        {
            if (o_cashbox_period.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodSelect", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_cashbox_period.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_cashbox_period.UserID;

            o_command.Parameters.Add("@CashBoxID", SqlDbType.Int);
            o_command.Parameters["@CashBoxID"].Value = o_cashbox_period.CashBoxID;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_cashbox_period.TerminalID = (int)o_reader["TerminalID"];

                    if (o_cashbox_period.TerminalID > 0) { o_cashbox_period.Terminal = o_reader["Terminal"].ToString(); }

                    o_cashbox_period.DateStart = (DateTime)o_reader["DateStart"];

                    if (o_reader["DateEnd"] != DBNull.Value) { o_cashbox_period.DateEnd = (DateTime)o_reader["DateEnd"]; }
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_cashbox_period.ErrorMessage = ex.Message;
                o_cashbox_period.ErrorMessage = "Error on select cashbox period.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Select_All(ref DataTable dt_cashbox_period, ref string s_error, int n_user_id)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodAll", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(dt_cashbox_period);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select cashbox period table.";
                return;
            }
            finally
            {
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Terminal(ref DataTable o_data_table, ref string s_error, int n_user_id, int n_company_id, DateTime d_start, DateTime? d_end)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodTerminal", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = n_company_id;

            o_command.Parameters.Add("@DateStart", SqlDbType.Date);
            o_command.Parameters["@DateStart"].Value = d_start;

            if (d_end != null)
            {
                o_command.Parameters.Add("@DateEnd", SqlDbType.Date);
                o_command.Parameters["@DateEnd"].Value = d_end;
            }

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);
        }

        public static void Check(ref CashBoxPeriodModel o_cashbox_period)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_cashbox_period.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_cashbox_period.UserID;

            o_command.Parameters.Add("@CashBoxID", SqlDbType.Int);
            o_command.Parameters["@CashBoxID"].Value = o_cashbox_period.CashBoxID;

            o_command.Parameters.Add("@TerminalID", SqlDbType.Int);
            o_command.Parameters["@TerminalID"].Value = o_cashbox_period.TerminalID;

            o_command.Parameters.Add("@DateStart", SqlDbType.Date);
            o_command.Parameters["@DateStart"].Value = o_cashbox_period.DateStart;

            if (o_cashbox_period.DateEnd != null)
            {
                o_command.Parameters.Add("@DateEnd", SqlDbType.Date);
                o_command.Parameters["@DateEnd"].Value = o_cashbox_period.DateEnd;
            }

            try
            {
                o_command.Connection.Open();
                o_cashbox_period.ErrorMessage = o_command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                o_cashbox_period.ErrorMessage = ex.Message;
                o_cashbox_period.ErrorMessage = "Error on check cashbox period.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref CashBoxPeriodModel o_cashbox_period)
        {
            string s_procedure = "";

            if (o_cashbox_period.ID == 0) { s_procedure = "sprCashBoxPeriodInsert"; } else { s_procedure = "sprCashBoxPeriodUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_cashbox_period.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_cashbox_period.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_cashbox_period.UserID;

            o_command.Parameters.Add("@CashBoxID", SqlDbType.Int);
            o_command.Parameters["@CashBoxID"].Value = o_cashbox_period.CashBoxID;

            o_command.Parameters.Add("@DateStart", SqlDbType.Date);
            o_command.Parameters["@DateStart"].Value = o_cashbox_period.DateStart;

            if (o_cashbox_period.DateEnd != null)
            {
                o_command.Parameters.Add("@DateEnd", SqlDbType.Date);
                o_command.Parameters["@DateEnd"].Value = o_cashbox_period.DateEnd;
            }

            try
            {
                o_command.Connection.Open();

                if (o_cashbox_period.ID == 0)
                {
                    o_cashbox_period.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_cashbox_period.ErrorMessage = ex.Message;
                o_cashbox_period.ErrorMessage = "Error on update cashbox period.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Table(int n_user_id, DataTable dt_cashbox_period_terminal, ref int n_rows_affected, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodInsertTable", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });

            o_command.Parameters.AddWithValue("@TableCashBoxPeriodTerminal", dt_cashbox_period_terminal);
            o_command.Parameters["@TableCashBoxPeriodTerminal"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = (int)o_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update cashbox period table.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static string Check_Terminal(int n_user_id, int n_cashbox_period_id, int n_terminal_id, DateTime d_start, DateTime? d_end, ref string s_error)
        {
            string s_result = "";

            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodTerminalCheck", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@CashBoxPeriodID", SqlDbType.Int) { Value = n_cashbox_period_id });
            o_command.Parameters.Add(new SqlParameter("@TerminalID", SqlDbType.Int) { Value = n_terminal_id });
            o_command.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date) { Value = d_start });

            if (d_end != null)
            {
                o_command.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date) { Value = d_end });
            }

            try
            {
                o_command.Connection.Open();

                s_error = o_command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on check cashbox period terminal.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return s_result;
        }

        public static void Update_Terminal(int n_user_id, int n_cashbox_period_id, int n_terminal_id, DateTime d_start, DateTime? d_end, DataTable dt_agreement_item_terminal, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodTerminalInsert", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@CashBoxPeriodID", SqlDbType.Int) { Value = n_cashbox_period_id });
            o_command.Parameters.Add(new SqlParameter("@TerminalID", SqlDbType.Int) { Value = n_terminal_id });
            o_command.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date) { Value = d_start });

            if (d_end != null)
            {
                o_command.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date) { Value = d_end });
            }

            o_command.Parameters.AddWithValue("@TableAgreementItemTerminal", dt_agreement_item_terminal);
            o_command.Parameters["@TableAgreementItemTerminal"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update cashbox period terminal.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static bool Delete(ref CashBoxPeriodModel o_cashbox_period)
        {
            bool b_deleted = false;

            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodDelete", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_cashbox_period.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_cashbox_period.UserID;

            o_command.Parameters.Add("@CashBoxID", SqlDbType.Int);
            o_command.Parameters["@CashBoxID"].Value = o_cashbox_period.CashBoxID;

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                o_cashbox_period.ErrorMessage = ex.Message;
                o_cashbox_period.ErrorMessage = "Error on delete cashbox period.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_deleted;
        }

        // CashBoxPeriodTerminal

        public static bool Delete_Terminal(int n_id, int n_user_id, int n_cashbox_period_id, ref string s_error)
        {
            bool b_deleted = false;

            SqlCommand o_command = new SqlCommand("sprCashBoxPeriodTerminalDelete", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = n_id;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@CashBoxPeriodID", SqlDbType.Int);
            o_command.Parameters["@CashBoxPeriodID"].Value = n_cashbox_period_id;

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete cashbox period terminal.";
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
