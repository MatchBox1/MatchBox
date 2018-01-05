using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class AgreementModel
    {
        public static string InterfaceName
        {
            get { return "AGREEMENT"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int CreditID { get; set; }

        public string CompanyName { get; set; }
        public string CreditName { get; set; }
        public string ErrorMessage { get; set; }

        public AgreementModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            CompanyID = 0;
            CreditID = 0;

            CompanyName = "";
            CreditName = "";
            ErrorMessage = "";
        }
    }

    public class AgreementAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprAgreementSelectList", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

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
                        case "CreditID":
                        case "PageNumber":
                        case "PageSize":
                            o_db_type = SqlDbType.Int;
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
                            o_command.Parameters.Add("@" + s_key, o_db_type, n_size);
                            o_command.Parameters["@" + s_key].Value = s_value;
                            break;
                    }
                }
            }

            DB.Bind_Data_Table(o_command, ref o_data_table, ref n_pages_count, ref s_error);
        }

        public static void Select(ref AgreementModel o_agreement)
        {
            if (o_agreement.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprAgreementSelect", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_agreement.ID });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement.UserID });

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_agreement.CompanyID = (int)o_reader["CompanyID"];
                    o_agreement.CreditID = (int)o_reader["CreditID"];

                    o_agreement.CompanyName = o_reader["CompanyName"].ToString();
                    o_agreement.CreditName = o_reader["CreditName"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_agreement.ErrorMessage = ex.Message;
                o_agreement.ErrorMessage = "Error on select agreement.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref AgreementModel o_agreement)
        {
            bool b_exists = Agreement_Exists(o_agreement);

            if (o_agreement.ErrorMessage != "") { return; }

            if (b_exists)
            {
                o_agreement.ErrorMessage = "Agreement already exists.";
                return;
            }

            string s_procedure = "";

            if (o_agreement.ID == 0) { s_procedure = "sprAgreementInsert"; } else { s_procedure = "sprAgreementUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            if (o_agreement.ID > 0)
            {
                o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_agreement.ID });
            }

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement.UserID });
            o_command.Parameters.Add(new SqlParameter("@CompanyID", SqlDbType.Int) { Value = o_agreement.CompanyID });
            o_command.Parameters.Add(new SqlParameter("@CreditID", SqlDbType.Int) { Value = o_agreement.CreditID });

            try
            {
                o_command.Connection.Open();

                if (o_agreement.ID == 0)
                {
                    o_agreement.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_agreement.ErrorMessage = ex.Message;
                o_agreement.ErrorMessage = "Error on update agreement.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        private static bool Agreement_Exists(AgreementModel o_agreement)
        {
            bool b_exists = false;

            SqlCommand o_command = new SqlCommand("sprAgreementCheck", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_agreement.ID });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_agreement.UserID });
            o_command.Parameters.Add(new SqlParameter("@CompanyID", SqlDbType.Int) { Value = o_agreement.CompanyID });
            o_command.Parameters.Add(new SqlParameter("@CreditID", SqlDbType.Int) { Value = o_agreement.CreditID });

            try
            {
                o_command.Connection.Open();
                b_exists = (int)o_command.ExecuteScalar() > 0;
            }
            catch (Exception ex)
            {
                o_agreement.ErrorMessage = ex.Message;
                o_agreement.ErrorMessage = "Error on validate agreement.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_exists;
        }
    }
}
