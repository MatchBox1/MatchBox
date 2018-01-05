using System;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class AgreementPeriodModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgreementID { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public string ErrorMessage { get; set; }

        public AgreementPeriodModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            AgreementID = 0;

            DateStart = null;
            DateEnd = null;

            ErrorMessage = "";
        }
    }

    public class AgreementPeriodAction
    {
        public static void Select(ref DataTable o_data_table, ref string s_error, int n_user_id, int n_agreement_id)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementPeriod", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = n_agreement_id;

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);
        }

        public static void Select(ref AgreementPeriodModel o_agreement_period)
        {
            if (o_agreement_period.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprAgreementPeriodSelect", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_agreement_period.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_agreement_period.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_agreement_period.AgreementID;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_agreement_period.DateStart = (DateTime)o_reader["DateStart"];

                    if (o_reader["DateEnd"] != DBNull.Value) { o_agreement_period.DateEnd = (DateTime)o_reader["DateEnd"]; }
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_agreement_period.ErrorMessage = ex.Message;
                o_agreement_period.ErrorMessage = "Error on select agreement period.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref AgreementPeriodModel o_agreement_period)
        {
            string s_procedure = "";

            if (o_agreement_period.ID == 0) { s_procedure = "sprAgreementPeriodInsert"; } else { s_procedure = "sprAgreementPeriodUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_agreement_period.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_agreement_period.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_agreement_period.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_agreement_period.AgreementID;

            o_command.Parameters.Add("@DateStart", SqlDbType.Date);
            o_command.Parameters["@DateStart"].Value = o_agreement_period.DateStart;

            if (o_agreement_period.DateEnd != null)
            {
                o_command.Parameters.Add("@DateEnd", SqlDbType.Date);
                o_command.Parameters["@DateEnd"].Value = o_agreement_period.DateEnd;
            }

            try
            {
                o_command.Connection.Open();

                if (o_agreement_period.ID == 0)
                {
                    o_agreement_period.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_agreement_period.ErrorMessage = ex.Message;
                o_agreement_period.ErrorMessage = "Error on update agreement period.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static string Check(ref AgreementPeriodModel o_agreement_period)
        {
            string s_result = "";

            SqlCommand o_command = new SqlCommand("sprAgreementPeriodCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_agreement_period.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_agreement_period.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_agreement_period.AgreementID;

            o_command.Parameters.Add("@DateStart", SqlDbType.Date);
            o_command.Parameters["@DateStart"].Value = o_agreement_period.DateStart;

            if (o_agreement_period.DateEnd != null)
            {
                o_command.Parameters.Add("@DateEnd", SqlDbType.Date);
                o_command.Parameters["@DateEnd"].Value = o_agreement_period.DateEnd;
            }

            try
            {
                o_command.Connection.Open();
                s_result = o_command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                o_agreement_period.ErrorMessage = ex.Message;
                o_agreement_period.ErrorMessage = "Error on check agreement period date.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return s_result;
        }

        public static bool Delete(ref AgreementPeriodModel o_agreement_period)
        {
            bool b_deleted = false;

            SqlCommand o_command = new SqlCommand("sprAgreementPeriodDelete", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_agreement_period.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_agreement_period.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_agreement_period.AgreementID;

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                o_agreement_period.ErrorMessage = ex.Message;
                o_agreement_period.ErrorMessage = "Error on delete agreement period.";
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
