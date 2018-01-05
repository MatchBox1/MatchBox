using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class NetworkModel
    {
        public static string InterfaceName
        {
            get { return "NETWORK"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }

        public string NetworkNumber { get; set; }
        public string NetworkName { get; set; }
        public string ErrorMessage { get; set; }

        public NetworkModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            CompanyID = 0;

            NetworkNumber = "";
            NetworkName = "";
            ErrorMessage = "";
        }
    }

    public class NetworkAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprNetworkSelectList", DB.Get_Connection());
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
                        case "PageNumber":
                        case "PageSize":
                            o_db_type = SqlDbType.Int;
                            break;
                        case "NetworkName":
                            o_db_type = SqlDbType.NVarChar;
                            n_size = 50;
                            break;
                        case "OrderBy":
                            o_db_type = SqlDbType.VarChar;
                            n_size = 50;
                            break;
                        case "NetworkNumber":
                            o_db_type = SqlDbType.VarChar;
                            n_size = 15;
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

        public static void Select(ref NetworkModel o_network)
        {
            if (o_network.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprNetworkSelect", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_network.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_network.UserID;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_network.CompanyID = (int)o_reader["CompanyID"];
                    o_network.NetworkNumber = o_reader["NetworkNumber"].ToString();
                    o_network.NetworkName = o_reader["NetworkName"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_network.ErrorMessage = ex.Message;
                o_network.ErrorMessage = "Error on select network.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Check(ref NetworkModel o_network)
        {
            SqlCommand o_command = new SqlCommand("sprNetworkCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_network.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_network.UserID;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = o_network.CompanyID;

            o_command.Parameters.Add("@NetworkNumber", SqlDbType.VarChar, 15);
            o_command.Parameters["@NetworkNumber"].Value = o_network.NetworkNumber;

            try
            {
                o_command.Connection.Open();

                o_network.ErrorMessage = o_command.ExecuteScalar().ToString().Trim();
            }
            catch (Exception ex)
            {
                o_network.ErrorMessage = ex.Message;
                o_network.ErrorMessage = "Error on check network.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update(ref NetworkModel o_network)
        {
            Check(ref o_network);

            if (o_network.ErrorMessage != "") { return; }

            string s_procedure = "";

            if (o_network.ID == 0) { s_procedure = "sprNetworkInsert"; } else { s_procedure = "sprNetworkUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_network.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_network.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_network.UserID;

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = o_network.CompanyID;

            o_command.Parameters.Add("@NetworkNumber", SqlDbType.VarChar, 15);
            o_command.Parameters["@NetworkNumber"].Value = o_network.NetworkNumber;

            o_command.Parameters.Add("@NetworkName", SqlDbType.NVarChar, 50);
            o_command.Parameters["@NetworkName"].Value = o_network.NetworkName;

            try
            {
                o_command.Connection.Open();

                if (o_network.ID == 0)
                {
                    o_network.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_network.ErrorMessage = ex.Message;
                o_network.ErrorMessage = "Error on update network.";
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
