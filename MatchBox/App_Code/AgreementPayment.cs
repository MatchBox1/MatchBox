using System;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class AgreementPaymentModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int AgreementID { get; set; }
        public int AgreementPeriodID { get; set; }
        public int CardID { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public string ErrorMessage { get; set; }

        public DataTable TableAgreementPaymentSettings { get; set; }

        public AgreementPaymentModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            CompanyID = 0;
            AgreementID = 0;
            AgreementPeriodID = 0;
            CardID = 0;

            ErrorMessage = "";

            TableAgreementPaymentSettings = new DataTable();
        }
    }

    public class AgreementPaymentAction
    {
        public static void Select(ref DataTable dt_agreement_period, ref DataTable dt_agreement_payment, ref DataTable dt_agreement_payment_settings, ref string s_error, int n_user_id, int? n_company_id = null, int? n_agreement_id = null)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementPayment", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = n_company_id;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = n_agreement_id;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                if (o_data_set.Tables.Count > 0)
                {
                    dt_agreement_period = o_data_set.Tables[0];

                    if (o_data_set.Tables.Count > 1)
                    {
                        dt_agreement_payment = o_data_set.Tables[1];

                        if (o_data_set.Tables.Count > 2)
                        {
                            dt_agreement_payment_settings = o_data_set.Tables[2];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select agreement payment.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select(ref AgreementPaymentModel o_agreement_payment)
        {
            if (o_agreement_payment.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprAgreementPaymentSelect", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_agreement_payment.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_agreement_payment.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_agreement_payment.AgreementID;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                DataRow dr_agreement_payment = o_data_set.Tables[0].Rows[0];

                o_agreement_payment.CompanyID = (int)dr_agreement_payment["CompanyID"];
                o_agreement_payment.AgreementPeriodID = (int)dr_agreement_payment["AgreementPeriodID"];
                o_agreement_payment.CardID = (int)dr_agreement_payment["CardID"];

                o_agreement_payment.DateStart = (DateTime)dr_agreement_payment["DateStart"];
                o_agreement_payment.DateEnd = (DateTime)dr_agreement_payment["DateEnd"];

                if (o_data_set.Tables.Count > 1)
                {
                    o_agreement_payment.TableAgreementPaymentSettings = o_data_set.Tables[1];
                }
            }
            catch (Exception ex)
            {
                o_agreement_payment.ErrorMessage = ex.Message;
                o_agreement_payment.ErrorMessage = "Error on select agreement payment.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Check(ref AgreementPaymentModel o_agreement_payment)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementPaymentCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_agreement_payment.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_agreement_payment.UserID;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = o_agreement_payment.CompanyID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_agreement_payment.AgreementID;

            o_command.Parameters.Add("@CardID", SqlDbType.Int);
            o_command.Parameters["@CardID"].Value = o_agreement_payment.CardID;

            o_command.Parameters.Add("@DateStart", SqlDbType.Date);
            o_command.Parameters["@DateStart"].Value = o_agreement_payment.DateStart;

            o_command.Parameters.Add("@DateEnd", SqlDbType.Date);
            o_command.Parameters["@DateEnd"].Value = o_agreement_payment.DateEnd;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_agreement_payment.ErrorMessage = o_reader["Error"].ToString();
                    o_agreement_payment.AgreementPeriodID = (int)o_reader["AgreementPeriodID"];
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_agreement_payment.ErrorMessage = ex.Message;
                o_agreement_payment.ErrorMessage = "Error on check Agreement Payment.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref AgreementPaymentModel o_agreement_payment)
        {
            string s_procedure = "";

            if (o_agreement_payment.ID == 0) { s_procedure = "sprAgreementPaymentInsert"; } else { s_procedure = "sprAgreementPaymentUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_agreement_payment.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_agreement_payment.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_agreement_payment.UserID;

            o_command.Parameters.Add("@AgreementPeriodID", SqlDbType.Int);
            o_command.Parameters["@AgreementPeriodID"].Value = o_agreement_payment.AgreementPeriodID;

            o_command.Parameters.Add("@CardID", SqlDbType.Int);
            o_command.Parameters["@CardID"].Value = o_agreement_payment.CardID;

            o_command.Parameters.Add("@DateStart", SqlDbType.Date);
            o_command.Parameters["@DateStart"].Value = o_agreement_payment.DateStart;

            o_command.Parameters.Add("@DateEnd", SqlDbType.Date);
            o_command.Parameters["@DateEnd"].Value = o_agreement_payment.DateEnd;

            o_command.Parameters.AddWithValue("@TableAgreementPaymentSettings", o_agreement_payment.TableAgreementPaymentSettings);
            o_command.Parameters["@TableAgreementPaymentSettings"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                if (o_agreement_payment.ID == 0)
                {
                    o_agreement_payment.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_agreement_payment.ErrorMessage = ex.Message;
                o_agreement_payment.ErrorMessage = "Error on update Agreement Payment.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static bool Delete(ref AgreementPaymentModel o_agreement_payment)
        {
            bool b_deleted = false;
          
            SqlCommand o_command = new SqlCommand("sprAgreementPaymentDelete", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_agreement_payment.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_agreement_payment.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_agreement_payment.AgreementID;

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                o_agreement_payment.ErrorMessage = ex.Message;
                o_agreement_payment.ErrorMessage = "Error on delete agreement payment.";
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
