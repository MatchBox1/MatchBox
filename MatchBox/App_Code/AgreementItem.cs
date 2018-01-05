using System;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class AgreementItemModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgreementID { get; set; }
        public int AgreementPeriodID { get; set; }
        public int CompanyID { get; set; }
        public int CreditID { get; set; }
        public int CardID { get; set; }
        public int OperationTypeID { get; set; }
        public int PaymentsFrom { get; set; }
        public int PaymentsTo { get; set; }

        public double CommissionLocal { get; set; }
        public double CommissionAbroad { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public string CardName { get; set; }
        public string OperationTypeName { get; set; }
        public string ErrorMessage { get; set; }

        public DataTable TableAgreementItemTerminal { get; set; }

        public AgreementItemModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            AgreementID = 0;
            AgreementPeriodID = 0;
            CompanyID = 0;
            CreditID = 0;
            CardID = 0;
            OperationTypeID = 0;

            PaymentsFrom = 0;
            PaymentsTo = 0;

            CommissionLocal = 0;
            CommissionAbroad = 0;

            CardName = "";
            OperationTypeName = "";
            ErrorMessage = "";

            TableAgreementItemTerminal = new DataTable();
        }
    }

    public class AgreementItemAction
    {
        public static void Select(ref AgreementItemModel o_agreement_item)
        {
            if (o_agreement_item.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprAgreementItemSelect", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_agreement_item.ID });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement_item.UserID });
            o_command.Parameters.Add(new SqlParameter("@AgreementID", SqlDbType.Int) { Value = o_agreement_item.AgreementID });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                DataRow dr_agreement_item = o_data_set.Tables[0].Rows[0];

                o_agreement_item.CompanyID = (int)dr_agreement_item["CompanyID"];
                o_agreement_item.CreditID = (int)dr_agreement_item["CreditID"];
                o_agreement_item.CardID = (int)dr_agreement_item["CardID"];
                o_agreement_item.OperationTypeID = (int)dr_agreement_item["OperationTypeID"];

                o_agreement_item.PaymentsFrom = (int)dr_agreement_item["PaymentsFrom"];
                o_agreement_item.PaymentsTo = (int)dr_agreement_item["PaymentsTo"];

                o_agreement_item.CommissionLocal = (double)dr_agreement_item["CommissionLocal"];
                o_agreement_item.CommissionAbroad = (double)dr_agreement_item["CommissionAbroad"];

                o_agreement_item.DateStart = (DateTime)dr_agreement_item["DateStart"];
                o_agreement_item.DateEnd = (DateTime)dr_agreement_item["DateEnd"];

                o_agreement_item.CardName = dr_agreement_item["CardName"].ToString();
                o_agreement_item.OperationTypeName = dr_agreement_item["OperationTypeName"].ToString();

                o_agreement_item.TableAgreementItemTerminal = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                o_agreement_item.ErrorMessage = ex.Message;
                o_agreement_item.ErrorMessage = "Error on select agreement item.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select(ref DataTable dt_cashbox_period, ref DataTable dt_agreement_period, ref DataTable dt_agreement_payment_card, ref DataTable dt_agreement_item, ref DataTable dt_agreement_item_terminal, ref string s_error, int n_user_id, int? n_company_id = null, int? n_agreement_id = null)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementItem", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@CompanyID", SqlDbType.Int) { Value = n_company_id });
            o_command.Parameters.Add(new SqlParameter("@AgreementID", SqlDbType.Int) { Value = n_agreement_id });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                if (o_data_set.Tables.Count > 0)
                {
                    dt_cashbox_period = o_data_set.Tables[0];

                    if (o_data_set.Tables.Count > 1)
                    {
                        dt_agreement_period = o_data_set.Tables[1];

                        if (o_data_set.Tables.Count > 2)
                        {
                            dt_agreement_payment_card = o_data_set.Tables[2];

                            if (o_data_set.Tables.Count > 3)
                            {
                                dt_agreement_item = o_data_set.Tables[3];

                                if (o_data_set.Tables.Count > 4)
                                {
                                    dt_agreement_item_terminal = o_data_set.Tables[4];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select agreement item.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static DataTable Select_Terminal(int n_user_id, int n_terminal_id, ref string s_error)
        {
            DataTable o_data_table = new DataTable();

            SqlCommand o_command = new SqlCommand("sprAgreementItemTerminal", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@TerminalID", SqlDbType.Int) { Value = n_terminal_id });

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);

            return o_data_table;
        }

        public static void Check(ref AgreementItemModel o_agreement_item, DataTable dt_supplier, DataTable dt_terminal, DataTable dt_supplier_terminal)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementItemCheck", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement_item.UserID });
            o_command.Parameters.Add(new SqlParameter("@AgreementID", SqlDbType.Int) { Value = o_agreement_item.AgreementID });
            o_command.Parameters.Add(new SqlParameter("@CompanyID", SqlDbType.Int) { Value = o_agreement_item.CompanyID });
            o_command.Parameters.Add(new SqlParameter("@CardID", SqlDbType.Int) { Value = o_agreement_item.CardID });
            o_command.Parameters.Add(new SqlParameter("@OperationTypeID", SqlDbType.Int) { Value = o_agreement_item.OperationTypeID });

            o_command.Parameters.Add(new SqlParameter("@PaymentsFrom", SqlDbType.Int) { Value = o_agreement_item.PaymentsFrom });
            o_command.Parameters.Add(new SqlParameter("@PaymentsTo", SqlDbType.Int) { Value = o_agreement_item.PaymentsTo });

            o_command.Parameters.Add(new SqlParameter("@CommissionLocal", SqlDbType.Float) { Value = o_agreement_item.CommissionLocal });
            o_command.Parameters.Add(new SqlParameter("@CommissionAbroad", SqlDbType.Float) { Value = o_agreement_item.CommissionAbroad });

            o_command.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date) { Value = o_agreement_item.DateStart });
            o_command.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date) { Value = o_agreement_item.DateEnd });

            o_command.Parameters.AddWithValue("@TableSupplier", dt_supplier);
            o_command.Parameters["@TableSupplier"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableTerminal", dt_terminal);
            o_command.Parameters["@TableTerminal"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableSupplierTerminal", dt_supplier_terminal);
            o_command.Parameters["@TableSupplierTerminal"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_agreement_item.ErrorMessage = o_reader["Error"].ToString();
                    o_agreement_item.AgreementPeriodID = (int)o_reader["AgreementPeriodID"];
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_agreement_item.ErrorMessage = ex.Message;
                o_agreement_item.ErrorMessage = "Error on check Agreement Item.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref AgreementItemModel o_agreement_item, DataTable dt_supplier_terminal, DataTable dt_cashbox_period_supplier_terminal)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementItemInsert", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement_item.UserID });
            o_command.Parameters.Add(new SqlParameter("@AgreementPeriodID", SqlDbType.Int) { Value = o_agreement_item.AgreementPeriodID });
            o_command.Parameters.Add(new SqlParameter("@CardID", SqlDbType.Int) { Value = o_agreement_item.CardID });
            o_command.Parameters.Add(new SqlParameter("@OperationTypeID", SqlDbType.Int) { Value = o_agreement_item.OperationTypeID });

            o_command.Parameters.Add(new SqlParameter("@PaymentsFrom", SqlDbType.Int) { Value = o_agreement_item.PaymentsFrom });
            o_command.Parameters.Add(new SqlParameter("@PaymentsTo", SqlDbType.Int) { Value = o_agreement_item.PaymentsTo });

            o_command.Parameters.Add(new SqlParameter("@CommissionLocal", SqlDbType.Float) { Value = o_agreement_item.CommissionLocal });
            o_command.Parameters.Add(new SqlParameter("@CommissionAbroad", SqlDbType.Float) { Value = o_agreement_item.CommissionAbroad });

            o_command.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date) { Value = o_agreement_item.DateStart });
            o_command.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date) { Value = o_agreement_item.DateEnd });

            o_command.Parameters.AddWithValue("@TableSupplierTerminal", dt_supplier_terminal);
            o_command.Parameters["@TableSupplierTerminal"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableCashBoxPeriodSupplierTerminal", dt_cashbox_period_supplier_terminal);
            o_command.Parameters["@TableCashBoxPeriodSupplierTerminal"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                o_agreement_item.ID = (int)o_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                o_agreement_item.ErrorMessage = ex.Message;
                o_agreement_item.ErrorMessage = "Error on update Agreement Item.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static bool Delete(int n_id, int n_user_id, int n_agreement_id, ref string s_error)
        {
            bool b_deleted = false;

            SqlCommand o_command = new SqlCommand("sprAgreementItemDelete", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = n_id });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@AgreementID", SqlDbType.Int) { Value = n_agreement_id });

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete Agreement Item.";
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
