using System;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class AgreementClubModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgreementID { get; set; }
        public int AgreementPeriodID { get; set; }
        public int CompanyID { get; set; }
        public int CreditID { get; set; }

        public long ClubNumber { get; set; }

        public double Commission { get; set; }
        public double Saving { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public string ManagementCompany { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public string ClubName { get; set; }
        public string ErrorMessage { get; set; }

        public AgreementClubModel()
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

            ClubNumber = 0;

            Commission = 0;
            Saving = 0;

            ManagementCompany = "";
            ContactName = "";
            Phone = "";
            Mail = "";
            Address = "";
            ClubName = "";
            ErrorMessage = "";
        }
    }

    public class AgreementClubAction
    {
        //public static void Select(ref AgreementClubModel o_agreement_club)
        //{
        //    if (o_agreement_club.ID == 0) { return; }

        //    SqlCommand o_command = new SqlCommand("sprAgreementClubSelect", DB.Get_Connection());
        //    o_command.CommandType = CommandType.StoredProcedure;

        //    o_command.Parameters.Add("@ID", SqlDbType.Int);
        //    o_command.Parameters["@ID"].Value = o_agreement_club.ID;

        //    o_command.Parameters.Add("@UserID", SqlDbType.Int);
        //    o_command.Parameters["@UserID"].Value = o_agreement_club.UserID;

        //    o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
        //    o_command.Parameters["@AgreementID"].Value = o_agreement_club.AgreementID;

        //    SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
        //    DataSet o_data_set = new DataSet();

        //    try
        //    {
        //        o_data_adapter.Fill(o_data_set);

        //        DataRow dr_agreement_club = o_data_set.Tables[0].Rows[0];

        //        o_agreement_club.CompanyID = (int)dr_agreement_club["CompanyID"];
        //        o_agreement_club.CreditID = (int)dr_agreement_club["CreditID"];
        //        o_agreement_club.CardID = (int)dr_agreement_club["CardID"];
        //        o_agreement_club.OperationTypeID = (int)dr_agreement_club["OperationTypeID"];

        //        o_agreement_club.PaymentsFrom = (int)dr_agreement_club["PaymentsFrom"];
        //        o_agreement_club.PaymentsTo = (int)dr_agreement_club["PaymentsTo"];

        //        o_agreement_club.CommissionLocal = (double)dr_agreement_club["CommissionLocal"];
        //        o_agreement_club.CommissionAbroad = (double)dr_agreement_club["CommissionAbroad"];

        //        o_agreement_club.DateStart = (DateTime)dr_agreement_club["DateStart"];
        //        o_agreement_club.DateEnd = (DateTime)dr_agreement_club["DateEnd"];

        //        o_agreement_club.CardName = dr_agreement_club["CardName"].ToString();
        //        o_agreement_club.OperationTypeName = dr_agreement_club["OperationTypeName"].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        o_agreement_club.ErrorMessage = ex.Message;
        //        o_agreement_club.ErrorMessage = "Error on select agreement club.";
        //        return;
        //    }
        //    finally
        //    {
        //        o_data_set.Dispose();
        //        o_data_adapter.Dispose();
        //        o_command.Dispose();
        //    }
        //}

        public static void Select(ref DataTable dt_agreement_period, ref DataTable dt_agreement_club,  ref string s_error, int n_user_id, int? n_company_id = null, int? n_agreement_id = null)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementClub", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@CompanyID", SqlDbType.Int) { Value = n_company_id });
            o_command.Parameters.Add(new SqlParameter("@AgreementID", SqlDbType.Int) { Value = n_agreement_id });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_agreement_period = o_data_set.Tables[0];
                dt_agreement_club = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select agreement club.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Check(ref AgreementClubModel o_agreement_club)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementClubCheck", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement_club.UserID });
            o_command.Parameters.Add(new SqlParameter("@AgreementID", SqlDbType.Int) { Value = o_agreement_club.AgreementID });
            o_command.Parameters.Add(new SqlParameter("@CompanyID", SqlDbType.Int) { Value = o_agreement_club.CompanyID });
            o_command.Parameters.Add(new SqlParameter("@CreditID", SqlDbType.Int) { Value = o_agreement_club.CreditID });

            o_command.Parameters.Add(new SqlParameter("@ClubNumber", SqlDbType.BigInt) { Value = o_agreement_club.ClubNumber });

            o_command.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date) { Value = o_agreement_club.DateStart });
            o_command.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date) { Value = o_agreement_club.DateEnd });

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_agreement_club.ErrorMessage = o_reader["Error"].ToString();
                    o_agreement_club.AgreementPeriodID = (int)o_reader["AgreementPeriodID"];
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_agreement_club.ErrorMessage = ex.Message;
                o_agreement_club.ErrorMessage = "Error on check Agreement Club.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref AgreementClubModel o_agreement_club)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementClubInsert", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement_club.UserID });
            o_command.Parameters.Add(new SqlParameter("@AgreementPeriodID", SqlDbType.Int) { Value = o_agreement_club.AgreementPeriodID });

            o_command.Parameters.Add(new SqlParameter("@ManagementCompany", SqlDbType.NVarChar, 50) { Value = o_agreement_club.ManagementCompany });
            o_command.Parameters.Add(new SqlParameter("@ContactName", SqlDbType.NVarChar, 50) { Value = o_agreement_club.ContactName });

            o_command.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 15) { Value = o_agreement_club.Phone });
            o_command.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar, 50) { Value = o_agreement_club.Mail });
            o_command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 50) { Value = o_agreement_club.Address });

            o_command.Parameters.Add(new SqlParameter("@ClubNumber", SqlDbType.BigInt) { Value = o_agreement_club.ClubNumber });
            o_command.Parameters.Add(new SqlParameter("@ClubName", SqlDbType.NVarChar, 20) { Value = o_agreement_club.ClubName });

            o_command.Parameters.Add(new SqlParameter("@Commission", SqlDbType.Float) { Value = o_agreement_club.Commission });
            o_command.Parameters.Add(new SqlParameter("@Saving", SqlDbType.Float) { Value = o_agreement_club.Saving });

            o_command.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date) { Value = o_agreement_club.DateStart });
            o_command.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date) { Value = o_agreement_club.DateEnd });

            try
            {
                o_command.Connection.Open();

                o_agreement_club.ID = (int)o_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                o_agreement_club.ErrorMessage = ex.Message;
                o_agreement_club.ErrorMessage = "Error on update Agreement Club.";
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

            SqlCommand o_command = new SqlCommand("sprAgreementClubDelete", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

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
                s_error = "Error on delete Agreement Club.";
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
