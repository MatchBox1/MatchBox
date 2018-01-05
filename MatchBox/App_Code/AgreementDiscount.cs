using System;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class AgreementDiscountModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgreementItemID { get; set; }
        public int DiscountID { get; set; }
        public int DiscountPeriodTypeID { get; set; }
        public int PaymentsFrom { get; set; }
        public int PaymentsTo { get; set; }
        public int WeekStart { get; set; }
        public int PaymentAfterDays { get; set; }

        public double Commission { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public string BaseDate { get; set; }
        public string ErrorMessage { get; set; }

        public DataTable TableAgreementDiscountSettings { get; set; }
        public DataTable TableAgreementDiscountTerminal { get; set; }

        public AgreementDiscountModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            AgreementItemID = 0;
            DiscountID = 0;
            DiscountPeriodTypeID = 0;

            PaymentsFrom = 0;
            PaymentsTo = 0;

            WeekStart = 0;
            PaymentAfterDays = 0;

            Commission = 0;

            BaseDate = "";
            ErrorMessage = "";

            TableAgreementDiscountSettings = new DataTable();
            TableAgreementDiscountSettings.Columns.Add("DayFrom");
            TableAgreementDiscountSettings.Columns.Add("DayTo");
            TableAgreementDiscountSettings.Columns.Add("PaymentAfterDays");

            TableAgreementDiscountTerminal = new DataTable();
            TableAgreementDiscountTerminal.Columns.Add("ID");
        }
    }

    public class AgreementDiscountAction
    {
        public static void Select(ref DataTable dt_agreement_discount, ref DataTable dt_agreement_discount_terminal, ref DataTable dt_agreement_discount_settings, ref string s_error, int n_user_id, int? n_agreement_id = null)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementDiscount", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@AgreementID", SqlDbType.Int) { Value = n_agreement_id });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_agreement_discount = o_data_set.Tables[0];
                dt_agreement_discount_terminal = o_data_set.Tables[1];
                dt_agreement_discount_settings = o_data_set.Tables[2];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select agreement discount.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Check(ref AgreementDiscountModel o_agreement_discount)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementDiscountCheck", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement_discount.UserID });
            o_command.Parameters.Add(new SqlParameter("@AgreementItemID", SqlDbType.Int) { Value = o_agreement_discount.AgreementItemID });
            o_command.Parameters.Add(new SqlParameter("@PaymentsFrom", SqlDbType.Int) { Value = o_agreement_discount.PaymentsFrom });
            o_command.Parameters.Add(new SqlParameter("@PaymentsTo", SqlDbType.Int) { Value = o_agreement_discount.PaymentsTo });

            o_command.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date) { Value = o_agreement_discount.DateStart });
            o_command.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date) { Value = o_agreement_discount.DateEnd });

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_agreement_discount.ErrorMessage = o_reader["Error"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_agreement_discount.ErrorMessage = ex.Message;
                o_agreement_discount.ErrorMessage = "Error on check Agreement Discount.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref AgreementDiscountModel o_agreement_discount)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementDiscountInsert", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement_discount.UserID });
            o_command.Parameters.Add(new SqlParameter("@AgreementItemID", SqlDbType.Int) { Value = o_agreement_discount.AgreementItemID });
            o_command.Parameters.Add(new SqlParameter("@DiscountID", SqlDbType.Int) { Value = o_agreement_discount.DiscountID });
            o_command.Parameters.Add(new SqlParameter("@DiscountPeriodTypeID", SqlDbType.Int) { Value = o_agreement_discount.DiscountPeriodTypeID });

            o_command.Parameters.Add(new SqlParameter("@PaymentsFrom", SqlDbType.Int) { Value = o_agreement_discount.PaymentsFrom });
            o_command.Parameters.Add(new SqlParameter("@PaymentsTo", SqlDbType.Int) { Value = o_agreement_discount.PaymentsTo });

            o_command.Parameters.Add(new SqlParameter("@WeekStart", SqlDbType.Int) { Value = o_agreement_discount.WeekStart });
            o_command.Parameters.Add(new SqlParameter("@PaymentAfterDays", SqlDbType.Int) { Value = o_agreement_discount.PaymentAfterDays });

            o_command.Parameters.Add(new SqlParameter("@Commission", SqlDbType.Float) { Value = o_agreement_discount.Commission });

            o_command.Parameters.Add(new SqlParameter("@BaseDate", SqlDbType.NVarChar, 50) { Value = o_agreement_discount.BaseDate });

            o_command.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date) { Value = o_agreement_discount.DateStart });
            o_command.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date) { Value = o_agreement_discount.DateEnd });

            o_command.Parameters.AddWithValue("@TableAgreementDiscountSettings", o_agreement_discount.TableAgreementDiscountSettings);
            o_command.Parameters["@TableAgreementDiscountSettings"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableAgreementDiscountTerminal", o_agreement_discount.TableAgreementDiscountTerminal);
            o_command.Parameters["@TableAgreementDiscountTerminal"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                o_agreement_discount.ID = (int)o_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                o_agreement_discount.ErrorMessage = ex.Message;
                o_agreement_discount.ErrorMessage = "Error on update Agreement Discount.";
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

            SqlCommand o_command = new SqlCommand("sprAgreementDiscountDelete", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

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
                s_error = "Error on delete Agreement Discount.";
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
