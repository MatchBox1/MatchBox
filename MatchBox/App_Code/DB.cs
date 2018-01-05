using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public class DB
    {
        public static SqlConnection Get_Connection(string s_db = "MatchBox")
        {
            string s_host = HttpContext.Current.Request.Url.Host.ToLower();
            string s_location = (s_host == "localhost") ? "Local" : "Server";
            string s_connection = String.Format("{0}_{1}", s_location, s_db);

            return new SqlConnection(ConfigurationManager.ConnectionStrings[s_connection].ConnectionString);
        }

        public static void Bind_List_Control(DropDownList o_list, string s_procedure, ref string s_error, List<string> o_additional_fields = null, List<SqlParameter> o_additional_parameters = null)
        {
            SqlCommand o_command = new SqlCommand(s_procedure, Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_additional_parameters != null)
            {
                foreach (SqlParameter o_parameter in o_additional_parameters)
                {
                    o_command.Parameters.Add(o_parameter);
                }
            }

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                while (o_reader.Read())
                {
                    ListItem o_list_ltem = new ListItem(o_reader[1].ToString(), o_reader[0].ToString());

                    if (o_additional_fields != null)
                    {
                        foreach (string s_field in o_additional_fields)
                        {
                            o_list_ltem.Attributes.Add("data-" + s_field, o_reader[s_field].ToString());
                        }
                    }

                    o_list.Items.Add(o_list_ltem);
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on bind list control.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Bind_Check_list(string s_procedure, ref CheckBoxList o_check_list, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            Bind_Check_list(o_command, ref o_check_list, ref s_error);
        }

        public static void Bind_Check_list(SqlCommand o_command, ref CheckBoxList o_check_list, ref string s_error)
        {
            DataTable o_data_table = new DataTable();

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);

            if (s_error != "") { return; }

            if (o_data_table.Rows.Count == 0) { return; }

            foreach (DataRow o_data_row in o_data_table.Rows)
            {
                ListItem o_list_item = new ListItem();

                o_list_item.Value = o_data_row[0].ToString();
                o_list_item.Text = o_data_row[1].ToString();

                try
                {
                    o_list_item.Selected = (o_data_row[2].ToString() == "1");
                }
                catch (Exception ex) { }

                o_check_list.Items.Add(o_list_item);
            }
        }

        public static void Bind_Data_Table(string s_procedure, ref DataTable o_data_table, ref string s_error, string s_parameter_name = "", string s_parameter_value = "")
        {
            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (s_parameter_name != "") { o_command.Parameters.Add(new SqlParameter(s_parameter_name, s_parameter_value)); }

            Bind_Data_Table(o_command, ref o_data_table, ref s_error);
        }

        public static void Bind_Data_Table(SqlCommand o_command, ref DataTable o_data_table, ref string s_error)
        {
            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(o_data_table);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on bind data table.";
            }
            finally
            {
                o_data_adapter.Dispose();
            }
        }

        public static void Bind_Data_Table(SqlCommand o_command, ref DataTable o_data_table, ref int n_pages_count, ref string s_error)
        {
            DataSet o_data_set = new DataSet();
            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(o_data_set);

                if (o_data_set.Tables.Count > 0)
                {
                    o_data_table = o_data_set.Tables[0];

                    if (o_data_set.Tables.Count > 1) { int.TryParse(o_data_set.Tables[1].Rows[0]["PagesCount"].ToString(), out n_pages_count); }
                }
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on bind data table.";
            }
            finally
            {
                o_data_adapter.Dispose();
            }
        }

        public static void Delete(int n_id, string s_procedure, ref string s_error, int? n_user_id = null)
        {
            if (n_id <= 0) { return; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection()) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = n_id });

            if (n_user_id != null)
            {
                o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            }

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete item.";
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
