using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace MatchBox
{
    public class Common
    {
        public static DataTable Convert_To_Table(string s_comma_separated_string, string s_data_type = "", bool b_include_empty = false)
        {
            List<string> o_list = s_comma_separated_string.Split(',').ToList();

            if (b_include_empty == false)
            {
                o_list.Remove("");
            }

            return Convert_To_Table(o_list, s_data_type);
        }

        public static DataTable Convert_To_Table(List<string> o_list, string s_data_type = "")
        {
            DataTable o_data_table = new DataTable();

            switch (s_data_type)
            {
                case "int":
                    o_data_table.Columns.Add("ID", typeof(int));        // typSingleColumnInt
                    break;
                case "decimal":
                    o_data_table.Columns.Add("ID", typeof(decimal));    // typSingleColumnNumeric
                    break;
                default:
                    o_data_table.Columns.Add("RowData");                // typSingleColumnString
                    break;
            }

            foreach (string s_item in o_list)
            {
                DataRow o_data_row = o_data_table.NewRow();

                switch (s_data_type)
                {
                    case "int":
                        int n_int = 0;
                        int.TryParse(s_item, out n_int);

                        o_data_row["ID"] = n_int;

                        break;
                    case "decimal":
                        decimal n_decimal = 0;
                        decimal.TryParse(s_item, out n_decimal);

                        o_data_row["ID"] = n_decimal;

                        break;
                    default:
                        o_data_row["RowData"] = s_item;
                        break;
                }

                o_data_table.Rows.Add(o_data_row);
            }

            return o_data_table;
        }

        public static List<int> Convert_To_List(string s_comma_separated_string)
        {
            List<int> o_list = s_comma_separated_string.Split(',').Select(int.Parse).ToList();

            o_list.Sort();

            return o_list;
        }

        public static string Convert_To_SQL_Array(params string[] array_of_comma_separated_strings)
        {
            string s_sql_array = Get_Distinct_Values(array_of_comma_separated_strings);

            if (s_sql_array != "")
            {
                s_sql_array = String.Format("'{0}'", s_sql_array.Replace(",", "','"));  // ADD SINGLE QUOTES TO EACH ITEM
            }

            return s_sql_array;
        }

        public static DateTime? Convert_To_Date(string s_date)
        {
            DateTime d_date = new DateTime();

            string[] arr_date = s_date.Replace(" ", "").Split('/');

            bool b_valid = (arr_date.Length == 3);

            if (b_valid == false) { goto Finish; }

            int n_day = 0, n_month = 0, n_year = 0;

            int.TryParse(arr_date[0], out n_day);
            int.TryParse(arr_date[1], out n_month);
            int.TryParse(arr_date[2], out n_year);

            b_valid = (n_day > 0 && n_month > 0 && n_year > 0);

            if (b_valid == false) { goto Finish; }

            s_date = String.Format("{0}-{1}-{2}", n_year, n_month, n_day);

            b_valid = DateTime.TryParse(s_date, out d_date);

        Finish:

            if (b_valid == true)
            {
                return d_date;
            }
            else
            {
                return null;
            }
        }

        public static bool Convert_To_Date(string s_date, string s_date_format, ref DateTime d_date)
        {
            bool b_date = false;

            string[] arr_format = s_date_format.Split(';');

            if (arr_format[0].Trim() == "date")
            {
                // date ; dd MM yy ; /

                string s_format = arr_format[1].Trim();
                string s_delimiter = arr_format[2].Trim();

                if (s_delimiter == "none") { s_delimiter = ""; }
                if (s_delimiter == "space") { s_delimiter = " "; }

                s_format = s_format.Replace(" ", s_delimiter);

                b_date = DateTime.TryParseExact(s_date, s_format, CultureInfo.InvariantCulture, DateTimeStyles.None, out d_date);
            }
            else if (arr_format[0].Trim() == "number")
            {
                // number ; day ; 1899-12-30

                int n_date = 0;
                int.TryParse(s_date, out n_date);

                if (s_date == n_date.ToString())
                {
                    string[] s_base_date = arr_format[2].Trim().Split('-');

                    DateTime d_base_date = new DateTime(Convert.ToInt32(s_base_date[0]), Convert.ToInt32(s_base_date[1]), Convert.ToInt32(s_base_date[2]));

                    try
                    {
                        switch (arr_format[1].Trim())
                        {
                            case "year":
                                d_date = d_base_date.AddYears(n_date);
                                break;
                            case "month":
                                d_date = d_base_date.AddMonths(n_date);
                                break;
                            case "day":
                                d_date = d_base_date.AddDays(n_date);
                                break;
                            case "hour":
                                d_date = d_base_date.AddHours(n_date);
                                break;
                            case "minute":
                                d_date = d_base_date.AddMinutes(n_date);
                                break;
                            case "second":
                                d_date = d_base_date.AddSeconds(n_date);
                                break;
                        }

                        b_date = true;
                    }
                    catch (Exception ex)
                    {
                        string s_error = ex.Message;
                    }
                }
            }

            return b_date;
        }

        public static string Get_Distinct_Values(params string[] array_of_comma_separated_strings)
        {
            string s_distinct_array = "";

            foreach (string s_duplicate_array in array_of_comma_separated_strings)
            {
                if (s_duplicate_array != "")
                {
                    if (s_distinct_array != "") { s_distinct_array += ","; }

                    s_distinct_array += s_duplicate_array;
                }
            }

            if (s_distinct_array != "")
            {
                string[] arr_distinct = s_distinct_array.Split(',').Distinct().ToArray();   // GET DISTINCT VALUES

                s_distinct_array = String.Join(",", arr_distinct);
            }

            return s_distinct_array;
        }

        public static int Get_Pages_Count(int n_rows_count, int n_page_size)
        {
            int n_pages_count = 1;

            if (n_rows_count > n_page_size)
            {
                n_pages_count = (int)(n_rows_count / n_page_size);

                if (n_rows_count % n_page_size > 0) { n_pages_count += 1; }
            }

            return n_pages_count;
        }
    }
}
