using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace MatchBox
{
    public class ExcelAction
    {
        public static string ExcelFolder
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("Temp");
            }
        }

        public static int MaxColumnsCount
        {
            get { return 150; }
        }

        // Reserve 5 columns for different fields (Like UniqueID or ErrorMessage)

        public static int MaxFieldsCount
        {
            get { return 155; }
        }

        private static List<int> ValidChars
        {
            get
            {
                List<int> n_valid_chars_list = new List<int>();

                // NUMBERS
                for (int i = 48; i <= 57; i++)
                {
                    n_valid_chars_list.Add(i);
                }

                // ENGLISH
                for (int i = 65; i <= 90; i++)
                {
                    n_valid_chars_list.Add(i);
                }

                for (int i = 97; i <= 122; i++)
                {
                    n_valid_chars_list.Add(i);
                }

                // HEBREW
                for (int i = 1488; i <= 1514; i++)
                {
                    n_valid_chars_list.Add(i);
                }

                // SPECIAL
                char c_space = ' ';
                char c_minus = '-';
                char c_apostrophe = '\'';
                char c_apostrophes = '"';
                char c_slash = '/';
                char c_slash_back = '\\';
                char c_bracket_left = '(';
                char c_bracket_right = ')';
                char c_percent = '%';

                n_valid_chars_list.Add((int)c_space);
                n_valid_chars_list.Add((int)c_minus);
                n_valid_chars_list.Add((int)c_apostrophe);
                n_valid_chars_list.Add((int)c_apostrophes);
                n_valid_chars_list.Add((int)c_slash);
                n_valid_chars_list.Add((int)c_slash_back);
                n_valid_chars_list.Add((int)c_bracket_left);
                n_valid_chars_list.Add((int)c_bracket_right);
                n_valid_chars_list.Add((int)c_percent);

                return n_valid_chars_list;
            }
        }

        public static OleDbConnection Get_Connection(string s_data_source, string s_hdr = "YES")
        {
            OleDbConnectionStringBuilder o_connection_string = new OleDbConnectionStringBuilder();

            // XLS  Microsoft.Jet.OLEDB.4.0
            // XLSX Microsoft.ACE.OLEDB.12.0

            o_connection_string.Provider = "Microsoft.ACE.OLEDB.12.0";
            o_connection_string.DataSource = s_data_source;
            o_connection_string.Add("Extended Properties", "Excel 12.0 Xml;HDR=" + s_hdr);

            return new OleDbConnection(o_connection_string.ConnectionString);
        }

        public static void Bind_Data_Table(string s_excel_file_path, ref DataTable o_data_table, ref string s_error, string s_excel_sql = "", string s_hdr = "YES", int n_rows = 0)
        {
            if (s_excel_sql == "")
            {
                OleDbConnection o_connection = Get_Connection(s_excel_file_path);

                o_connection.Open();

                string s_table = ((DataTable)o_connection.GetSchema("Tables")).Rows[0]["TABLE_NAME"].ToString();

                if (n_rows > 0)
                {
                    s_excel_sql = String.Format(" SELECT TOP {0} * FROM [{1}] ", n_rows, s_table);
                }
                else
                {
                    s_excel_sql = String.Format(" SELECT * FROM [{0}] ", s_table);
                }

                o_connection.Close();
                o_connection.Dispose();
            }

            OleDbCommand o_command = new OleDbCommand(s_excel_sql, Get_Connection(s_excel_file_path, s_hdr));

            try
            {
                o_command.Connection.Open();

                OleDbDataAdapter o_adapter = new OleDbDataAdapter(o_command);

                o_adapter.Fill(o_data_table);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on bind table from excel file.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static bool IsValid(FileUpload o_file_upload)
        {
            bool b_valid = false;

            string[] EXL_EXT = { "xls", "xlsx" };
            //string[] EXL_EXT = { "xls", "xlsx", "csv" };

            string s_file_name = o_file_upload.PostedFile.FileName.ToLower();

            //if (s_file_name.Split('.').Length > 2) { goto Finish; }

            string s_file_ext = s_file_name.Substring(s_file_name.LastIndexOf(".") + 1);

            for (int i = 0; i < EXL_EXT.Length; i++)
            {
                if (s_file_ext == EXL_EXT[i])
                {
                    b_valid = true;
                    break;
                }
            }

        //Finish:

            return b_valid;
        }

        public static string Valid_Field_Name(string s_check_text)
        {
            string s_valid_text = "";

            char[] c_char_array = s_check_text.ToCharArray();

            for (int i = 0; i < c_char_array.Length; i++)
            {
                char c_char = c_char_array[i];

                if (ValidChars.Contains((int)c_char))
                {
                    if (c_char.ToString() == "'" || c_char == '"') { c_char = '`'; } // change apostrophe to similar char

                    s_valid_text += c_char.ToString();
                }
            }

            s_valid_text = Remove_Double_Space(s_valid_text);

            return s_valid_text;
        }

        public static string Valid_Field_Value(string s_check_text)
        {
            string s_valid_text = s_check_text.Trim().Replace("'", "`").Replace('"', '`');  // change apostrophe to similar char

            s_valid_text = Remove_Double_Space(s_valid_text);

            return s_valid_text;
        }

        public static double Valid_Amount(string s_amount)
        {
            double n_amount = 0;

            char[] c_char_array = s_amount.Trim().ToCharArray();

            s_amount = "";

            for (int i = 0; i < c_char_array.Length; i++)
            {
                char c_char = c_char_array[i];

                if ((c_char >= 48 && c_char <= 57) || c_char == 46 || c_char == 45)
                {
                    s_amount += c_char.ToString();
                }
            }

            double.TryParse(s_amount, out n_amount);

            if (s_amount.Contains("."))
            {
                if (s_amount.Split('.')[1].Length > 2)
                {
                    n_amount = Math.Round(n_amount, 2);
                }
            }

            return n_amount;
        }

        public static string Remove_Double_Space(string s_check_text)
        {
            string s_valid_text = s_check_text.Trim();
            string s_double_space = "  ";
            string s_single_space = " ";

            while (s_valid_text.Contains(s_double_space))
            {
                s_valid_text = s_valid_text.Replace(s_double_space, s_single_space);
            }

            return s_valid_text;
        }

        public static string Save(FileUpload o_file_upload, ref string s_error)
        {
            string s_file_name = "";

            if (!IsValid(o_file_upload))
            {
                s_error = "File not valid.";
                goto Finish;
            }

            string s_posted_file_name = o_file_upload.PostedFile.FileName.ToLower();
            string s_posted_file_ext = s_posted_file_name.Substring(s_posted_file_name.LastIndexOf(".") + 1);

            s_file_name = Guid.NewGuid() + "." + s_posted_file_ext;

            try
            {
                o_file_upload.SaveAs(ExcelFolder + "\\" + s_file_name);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on save file.";
                s_file_name = "";
            }

        Finish:

            return s_file_name;
        }

        public static void Remove(string s_path, ref string s_error)
        {
            try
            {
                File.Delete(s_path);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on remove file.";
            }
        }
    }
}
