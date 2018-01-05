using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MatchBox
{
    public class UserModel
    {
        public static string InterfaceName
        {
            get { return "USER"; }
        }

        public bool IsAdmin { get; set; }
        public bool IsUser { get; set; }

        public int ID { get; set; }
        public int StatusID { get; set; }
        public int CityID { get; set; }

        public string Status { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ErrorMessage { get; set; }

        public DataTable TableUserService { get; set; }

        public UserModel()
        {
            Reset();
        }

        public void Reset()
        {
            IsAdmin = false;
            IsUser = false;

            ID = 0;
            StatusID = 0;
            CityID = 0;

            Status = "";
            FullName = "";
            UserName = "";
            Password = "";
            Mail = "";
            Phone = "";
            Mobile = "";
            City = "";
            Address = "";
            ErrorMessage = "";

            TableUserService = new DataTable();
            TableUserService.Columns.Add("ServiceID");
            TableUserService.Columns.Add("DependencyID");
            TableUserService.Columns.Add("ServiceName");
            TableUserService.Columns.Add("IsSelected");
        }
    }

    public class UserAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprUserSelectList", DB.Get_Connection());
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
                        case "StatusID":
                        case "CityID":
                        case "PageNumber":
                        case "PageSize":
                            o_db_type = SqlDbType.Int;
                            break;
                        case "FullName":
                        case "UserName":
                        case "Password":
                            o_db_type = SqlDbType.NVarChar;
                            n_size = 25;
                            break;
                        case "Address":
                            o_db_type = SqlDbType.NVarChar;
                            n_size = 50;
                            break;
                        case "Mail":
                        case "OrderBy":
                            o_db_type = SqlDbType.VarChar;
                            n_size = 50;
                            break;
                        case "Phone":
                            o_db_type = SqlDbType.VarChar;
                            n_size = 10;
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

        public static void Select(ref UserModel o_user)
        {
            if (o_user.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprUserSelect", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_user.ID;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                DataRow dr_user = o_data_set.Tables[0].Rows[0];

                o_user.StatusID = (int)dr_user["StatusID"];
                o_user.CityID = (int)dr_user["CityID"];

                o_user.Status = dr_user["Status"].ToString();
                o_user.FullName = dr_user["FullName"].ToString();
                o_user.UserName = dr_user["UserName"].ToString();
                o_user.Password = dr_user["Password"].ToString();
                o_user.Mail = dr_user["Mail"].ToString();
                o_user.Phone = dr_user["Phone"].ToString();
                o_user.Mobile = dr_user["Mobile"].ToString();
                o_user.City = dr_user["City"].ToString();
                o_user.Address = dr_user["Address"].ToString();

                o_user.TableUserService = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                o_user.ErrorMessage = ex.Message;
                o_user.ErrorMessage = "Error on select user.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void SelectService(ref UserModel o_user)
        {
            SqlCommand o_command = new SqlCommand("sprService_Selected", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_user.ID;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(o_user.TableUserService);
            }
            catch (Exception ex)
            {
                o_user.ErrorMessage = ex.Message;
                o_user.ErrorMessage = "Error on select service.";
                return;
            }
            finally
            {
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Update(ref UserModel o_user, bool b_admin)
        {
            string s_error = "";

            bool b_exists = UserName_Exists(o_user.ID, o_user.UserName, ref s_error);

            if (s_error != "")
            {
                o_user.ErrorMessage = s_error;
                return;
            }

            if (b_exists)
            {
                o_user.ErrorMessage = "User name already exists.";
                return;
            }

            string s_procedure = "";

            if (o_user.ID == 0) { s_procedure = "sprUserInsert"; } else { s_procedure = "sprUserUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_user.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_user.ID;
            }

            o_command.Parameters.Add("@IsAdmin", SqlDbType.Bit);
            o_command.Parameters["@IsAdmin"].Value = b_admin;

            o_command.Parameters.Add("@StatusID", SqlDbType.Int);
            o_command.Parameters["@StatusID"].Value = o_user.StatusID;

            o_command.Parameters.Add("@CityID", SqlDbType.Int);
            o_command.Parameters["@CityID"].Value = o_user.CityID;

            o_command.Parameters.Add("@FullName", SqlDbType.NVarChar, 25);
            o_command.Parameters["@FullName"].Value = o_user.FullName;

            o_command.Parameters.Add("@UserName", SqlDbType.NVarChar, 25);
            o_command.Parameters["@UserName"].Value = o_user.UserName;

            o_command.Parameters.Add("@Password", SqlDbType.NVarChar, 25);
            o_command.Parameters["@Password"].Value = o_user.Password;

            o_command.Parameters.Add("@Mail", SqlDbType.VarChar, 50);
            o_command.Parameters["@Mail"].Value = o_user.Mail;

            o_command.Parameters.Add("@Phone", SqlDbType.VarChar, 10);
            o_command.Parameters["@Phone"].Value = o_user.Phone;

            o_command.Parameters.Add("@Mobile", SqlDbType.VarChar, 10);
            o_command.Parameters["@Mobile"].Value = o_user.Mobile;

            o_command.Parameters.Add("@Address", SqlDbType.NVarChar, 50);
            o_command.Parameters["@Address"].Value = o_user.Address;

            DataTable o_data_table_selected = new DataTable();

            o_data_table_selected.Columns.Add("ID", typeof(int));

            if (b_admin == true)
            {
                foreach (DataRow o_data_row_selected in o_user.TableUserService.Select("IsSelected = True"))
                {
                    DataRow o_data_row = o_data_table_selected.NewRow();

                    o_data_row["ID"] = o_data_row_selected["ServiceID"];

                    o_data_table_selected.Rows.Add(o_data_row);
                }
            }

            o_command.Parameters.AddWithValue("@TableUserServiceID", o_data_table_selected);
            o_command.Parameters["@TableUserServiceID"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                if (o_user.ID == 0)
                {
                    o_user.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_user.ErrorMessage = ex.Message;
                o_user.ErrorMessage = "Error on update user.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Login(ref UserModel o_user)
        {
            o_user.UserName = o_user.UserName.Trim();
            o_user.Password = o_user.Password.Trim();

            if (o_user.UserName == "" || o_user.Password == "") { return; }

            int n_id = 0;
            string s_type = "";

            SqlCommand o_command = new SqlCommand("sprUserLogin", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserName", SqlDbType.NVarChar, 25);
            o_command.Parameters["@UserName"].Value = o_user.UserName;

            o_command.Parameters.Add("@Password", SqlDbType.NVarChar, 25);
            o_command.Parameters["@Password"].Value = o_user.Password;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    n_id = (int)o_reader["ID"];
                    s_type = o_reader["Type"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_user.Reset();
                o_user.ErrorMessage = ex.Message;
                //o_user.ErrorMessage = "Error on login user.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            if (s_type == "ADMIN")
            {
                o_user.IsAdmin = true;
            }
            else if (s_type == "USER" && n_id > 0)
            {
                o_user.ID = n_id;

                Select(ref o_user);

                if (o_user.ErrorMessage != "") { return; }

                o_user.IsUser = true;
            }
            else
            {
                o_user.Reset();
                o_user.ErrorMessage = "Wrong username or password";
                return;
            }
        }

        public static void Register(ref UserModel o_user)
        {
            string s_error = "";

            bool b_exists = UserName_Exists(o_user.ID, o_user.UserName, ref s_error);

            if (s_error != "")
            {
                o_user.ErrorMessage = s_error;
                return;
            }

            if (b_exists)
            {
                o_user.ErrorMessage = "User name already exists.";
                return;
            }

            SqlCommand o_command = new SqlCommand("sprUserRegister", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@FullName", SqlDbType.NVarChar, 25);
            o_command.Parameters["@FullName"].Value = o_user.FullName;

            o_command.Parameters.Add("@UserName", SqlDbType.NVarChar, 25);
            o_command.Parameters["@UserName"].Value = o_user.UserName;

            o_command.Parameters.Add("@Password", SqlDbType.NVarChar, 25);
            o_command.Parameters["@Password"].Value = o_user.Password;

            o_command.Parameters.Add("@Mail", SqlDbType.NVarChar, 50);
            o_command.Parameters["@Mail"].Value = o_user.Mail;

            try
            {
                o_command.Connection.Open();

                o_user.ID = (int)o_command.ExecuteScalar();

                if (o_user.ID > 0) { o_user.IsUser = true; }
            }
            catch (Exception ex)
            {
                o_user.ErrorMessage = ex.Message;
                o_user.ErrorMessage = "Error on register user.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        private static bool UserName_Exists(int n_id, string s_user_name, ref string s_error)
        {
            bool b_exists = false;

            SqlCommand o_command = new SqlCommand("sprUserCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = n_id;

            o_command.Parameters.Add("@UserName", SqlDbType.NVarChar, 25);
            o_command.Parameters["@UserName"].Value = s_user_name.Trim();

            try
            {
                o_command.Connection.Open();
                b_exists = (int)o_command.ExecuteScalar() > 0;
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on validate user name.";
                return true;
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
