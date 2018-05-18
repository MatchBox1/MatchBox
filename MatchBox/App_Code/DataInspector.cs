using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public class DataSearchTables
    {
        public int UserID { get; set; }

        public DataTable TableCompany { get; set; }
        public DataTable TableNetwork { get; set; }
        public DataTable TableBranch { get; set; }
        public DataTable TableCashbox { get; set; }

        public DataTable TableSupplierGroup { get; set; }
        public DataTable TableSupplier { get; set; }
        public DataTable TableTerminal { get; set; }

        public DataTable TableCredit { get; set; }
        public DataTable TableCard { get; set; }

        public DataTable TableOperationType { get; set; }
        public DataTable TableDataStatus { get; set; }
        public DataTable TableMatchingAction { get; set; }
        public DataTable TableMatchingType { get; set; }

        public DataTable TableTemplate { get; set; }
        public DataTable TableDataFile { get; set; }
        public DataTable TableStrategy { get; set; }
        public DataTable TableMatching { get; set; }
        public DataTable TableCurrency { get; set; }

        public DataTable TableDataPriority { get; set; }
        public DataTable TableDataSort { get; set; }
        public DataTable TableDataField { get; set; }
        public DataTable TableDiscountName { get; set; }

        public string ErrorMessage { get; set; }

        public DataSearchTables()
        {
            Reset();
        }

        public void Reset()
        {
            UserID = 0;

            TableCompany = new DataTable();
            TableNetwork = new DataTable();
            TableBranch = new DataTable();
            TableCashbox = new DataTable();

            TableSupplierGroup = new DataTable();
            TableSupplier = new DataTable();
            TableTerminal = new DataTable();

            TableCredit = new DataTable();
            TableCard = new DataTable();

            TableOperationType = new DataTable();
            TableDataStatus = new DataTable();
            TableMatchingAction = new DataTable();
            TableMatchingType = new DataTable();

            TableTemplate = new DataTable();
            TableDataFile = new DataTable();
            TableStrategy = new DataTable();
            TableMatching = new DataTable();
            TableCurrency = new DataTable();

            TableDataPriority = new DataTable();
            TableDataSort = new DataTable();
            TableDataField = new DataTable();

            ErrorMessage = "";
        }
    }


    public class DataAction
    {

        public static void SelectLockedRecords(int n_user_id, ref DataTable dtDataFields, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprLockNonMatchedRecords", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);
                dtDataFields = o_data_set.Tables[0];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select data (paging).";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select(int n_user_id, string s_where_inside, string s_where_outside, string s_order_inside, string s_order_outside, int n_page_inside, int n_page_outside, int n_page_size, ref DataTable dt_inside, ref DataTable dt_inside_sum, ref DataTable dt_outside, ref DataTable dt_outside_sum, ref string s_error, string sortColumnName, string sortType, string sortColumnName_Out, string sortType_Out, string s_group_by, string strChkFilters, string s_selectColumns_by, string s_group_byout, string s_selectColumns_byout)
        {
            string strDBColumnName = string.Empty;
            if (sortColumnName != null)
            {
                if (!string.IsNullOrEmpty(sortColumnName.Trim()))
                {
                    strDBColumnName = AnchorFieldsMappings.AnchorFieldsMapping(sortColumnName.Trim());
                    if (strDBColumnName == "")
                        strDBColumnName = sortColumnName.Trim();
                }
            }

            string strDBColumnName_Out = string.Empty;
            if (sortColumnName_Out != null)
            {
                if (!string.IsNullOrEmpty(sortColumnName_Out.Trim()))
                {
                    strDBColumnName_Out = AnchorFieldsMappings.AnchorFieldsMapping(sortColumnName_Out.Trim());
                    if (strDBColumnName_Out == "")
                        strDBColumnName_Out = sortColumnName_Out.Trim();
                }
            }

            SqlCommand o_command = new SqlCommand("sprDataSelectPagingLazy3", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@PageNumberInside", SqlDbType.Int) { Value = n_page_inside });
            o_command.Parameters.Add(new SqlParameter("@PageNumberOutside", SqlDbType.Int) { Value = n_page_outside });
            o_command.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = n_page_size });
            //o_command.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = 200 });
            o_command.Parameters.Add(new SqlParameter("@WhereInside", SqlDbType.NVarChar, -1) { Value = s_where_inside });
            o_command.Parameters.Add(new SqlParameter("@WhereOutside", SqlDbType.NVarChar, -1) { Value = s_where_outside });
            o_command.Parameters.Add(new SqlParameter("@OrderInside", SqlDbType.NVarChar, -1) { Value = s_order_inside });
            o_command.Parameters.Add(new SqlParameter("@OrderOutside", SqlDbType.NVarChar, -1) { Value = s_order_outside });

            o_command.Parameters.Add(new SqlParameter("@sortColumnName", SqlDbType.NVarChar, -1) { Value = strDBColumnName });
            o_command.Parameters.Add(new SqlParameter("@sortType", SqlDbType.NVarChar, -1) { Value = sortType });
            o_command.Parameters.Add(new SqlParameter("@sortColumnName_Out", SqlDbType.NVarChar, -1) { Value = strDBColumnName_Out });
            o_command.Parameters.Add(new SqlParameter("@sortType_Out", SqlDbType.NVarChar, -1) { Value = sortType_Out });
            o_command.Parameters.Add(new SqlParameter("@Group_By", SqlDbType.NVarChar, -1) { Value = s_group_by });
            o_command.Parameters.Add(new SqlParameter("@ChkFilters", SqlDbType.NVarChar, -1) { Value = strChkFilters });
            o_command.Parameters.Add(new SqlParameter("@SelectColumns", SqlDbType.NVarChar, -1) { Value = s_selectColumns_by });
            o_command.Parameters.Add(new SqlParameter("@Group_ByOut", SqlDbType.NVarChar, -1) { Value = s_group_byout });
            o_command.Parameters.Add(new SqlParameter("@SelectColumnsOut", SqlDbType.NVarChar, -1) { Value = s_selectColumns_byout });
            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();
            try
            {
                o_command.CommandTimeout = 3600;
                o_data_adapter.Fill(o_data_set);

                dt_inside = o_data_set.Tables[0];
                dt_inside_sum = o_data_set.Tables[1];

                dt_outside = o_data_set.Tables[2];
                dt_outside_sum = o_data_set.Tables[3];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select data (paging).";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void SelectInside(int n_user_id, string s_where_inside, string s_order_inside, int n_page_inside, int n_page_size, ref DataTable dt_inside, ref DataTable dt_inside_sum, ref string s_error, string sortColumnName, string sortType, string s_group_by, string strChkFilters, string s_selectColumns_by)
        {
            string strDBColumnName = string.Empty;
            if (!string.IsNullOrEmpty(sortColumnName.Trim()))
            {
                strDBColumnName = AnchorFieldsMappings.AnchorFieldsMapping(sortColumnName.Trim());
                if (strDBColumnName == "")
                    strDBColumnName = sortColumnName.Trim();
            }

            SqlCommand o_command = new SqlCommand("sprDataSelectInsidePagingLazy", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@PageNumberInside", SqlDbType.Int) { Value = n_page_inside });
            o_command.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = n_page_size });
            o_command.Parameters.Add(new SqlParameter("@WhereInside", SqlDbType.NVarChar, -1) { Value = s_where_inside });
            o_command.Parameters.Add(new SqlParameter("@OrderInside", SqlDbType.NVarChar, -1) { Value = s_order_inside });

            o_command.Parameters.Add(new SqlParameter("@sortColumnName", SqlDbType.NVarChar, -1) { Value = strDBColumnName });
            o_command.Parameters.Add(new SqlParameter("@sortType", SqlDbType.NVarChar, -1) { Value = sortType });
            o_command.Parameters.Add(new SqlParameter("@Group_By", SqlDbType.NVarChar, -1) { Value = s_group_by });
            o_command.Parameters.Add(new SqlParameter("@ChkFilters", SqlDbType.NVarChar, -1) { Value = strChkFilters });
            o_command.Parameters.Add(new SqlParameter("@SelectColumns", SqlDbType.NVarChar, -1) { Value = s_selectColumns_by });
            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();
            try
            {
                o_data_adapter.Fill(o_data_set);
                dt_inside = o_data_set.Tables[0];
                dt_inside_sum = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select data (paging).";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void SelectOutside(int n_user_id, string s_where_outside, string s_order_outside, int n_page_outside, int n_page_size, ref DataTable dt_outside, ref DataTable dt_outside_sum, ref string s_error, string sortColumnName, string sortType, string s_group_by, string strChkFilters, string s_selectColumns_by)
        {
            string strDBColumnName = string.Empty;
            if (!string.IsNullOrEmpty(sortColumnName.Trim()))
                strDBColumnName = AnchorFieldsMappings.AnchorFieldsMapping(sortColumnName.Trim());
            if (strDBColumnName == "")
                strDBColumnName = sortColumnName.Trim();

            SqlCommand o_command = new SqlCommand("sprDataSelectOutsidePagingLazy", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@PageNumberOutside", SqlDbType.Int) { Value = n_page_outside });
            o_command.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = n_page_size });
            o_command.Parameters.Add(new SqlParameter("@WhereOutside", SqlDbType.NVarChar, -1) { Value = s_where_outside });
            o_command.Parameters.Add(new SqlParameter("@OrderOutside", SqlDbType.NVarChar, -1) { Value = s_order_outside });

            o_command.Parameters.Add(new SqlParameter("@sortColumnName", SqlDbType.NVarChar, -1) { Value = strDBColumnName });
            o_command.Parameters.Add(new SqlParameter("@sortType", SqlDbType.NVarChar, -1) { Value = sortType });
            o_command.Parameters.Add(new SqlParameter("@Group_By", SqlDbType.NVarChar, -1) { Value = s_group_by });
            o_command.Parameters.Add(new SqlParameter("@ChkFilters", SqlDbType.NVarChar, -1) { Value = strChkFilters });
            o_command.Parameters.Add(new SqlParameter("@SelectColumns", SqlDbType.NVarChar, -1) { Value = s_selectColumns_by });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();
            try
            {
                o_data_adapter.Fill(o_data_set);
                dt_outside = o_data_set.Tables[0];
                dt_outside_sum = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select data (paging).";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static DataTable Select_Side(int n_user_id, string s_where_inside, string s_where_outside, ref string s_error)
        {
            DataTable dt_data = new DataTable();

            SqlCommand o_command = new SqlCommand("sprDataSelect", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@WhereInside", SqlDbType.NVarChar, -1) { Value = s_where_inside });
            o_command.Parameters.Add(new SqlParameter("@WhereOutside", SqlDbType.NVarChar, -1) { Value = s_where_outside });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(dt_data);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select data (side).";
            }
            finally
            {
                o_data_adapter.Dispose();
                o_command.Dispose();
            }

            return dt_data;
        }

        public static DataTable Select_SideGroupBy(int n_user_id, string s_where_inside, string s_where_outside,string strGroupBy, string strChkFilters, ref string s_error)
        {
            DataTable dt_data = new DataTable();

            SqlCommand o_command = new SqlCommand("sprDataSelectGroupBy", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@WhereInside", SqlDbType.NVarChar, -1) { Value = s_where_inside });
            o_command.Parameters.Add(new SqlParameter("@WhereOutside", SqlDbType.NVarChar, -1) { Value = s_where_outside });
            o_command.Parameters.Add(new SqlParameter("@strGroupBy", SqlDbType.NVarChar, -1) { Value = strGroupBy });
            o_command.Parameters.Add(new SqlParameter("@strChkFilters", SqlDbType.NVarChar, -1) { Value = strChkFilters });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(dt_data);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select data (side).";
            }
            finally
            {
                o_data_adapter.Dispose();
                o_command.Dispose();
            }

            return dt_data;
        }

        public static void Select_Query(int n_user_id, int n_matching_id, string s_query_number_array, ref DataTable dt_inside, ref DataTable dt_outside, ref string s_error)
        {
            if (n_user_id <= 0 || n_matching_id <= 0 || s_query_number_array == "") { return; }

            SqlCommand o_command = new SqlCommand("sprDataSelectQuery", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@MatchingID", SqlDbType.Int);
            o_command.Parameters["@MatchingID"].Value = n_matching_id;

            o_command.Parameters.Add("@QueryNumberArray", SqlDbType.NVarChar, -1);
            o_command.Parameters["@QueryNumberArray"].Value = s_query_number_array;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_inside = o_data_set.Tables[0];
                dt_outside = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select query.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Match(int n_user_id, string s_query_id, ref DataTable dt_inside, ref DataTable dt_outside, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprDataSelectMatch", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@QueryID", SqlDbType.NVarChar, 36);
            o_command.Parameters["@QueryID"].Value = s_query_id;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_inside = o_data_set.Tables[0];
                dt_outside = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select match.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Payment(int n_user_id, string s_unique_id, string s_table, ref DataTable dt_data, ref DataTable dt_source, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprDataSelectPayment", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            //o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Decimal) { Precision = 36, Scale = 0, Value = n_id });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.NVarChar, 36) { Value = s_unique_id });
            o_command.Parameters.Add(new SqlParameter("@Table", SqlDbType.VarChar, 15) { Value = s_table });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_data = o_data_set.Tables[0];
                dt_source = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select source.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Search_Tables(ref DataSearchTables o_data_search_tables)
        {
            SqlCommand o_command = new SqlCommand("sprDataSearchTables", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_data_search_tables.UserID;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                o_data_search_tables.TableCompany = o_data_set.Tables[0];
                o_data_search_tables.TableNetwork = o_data_set.Tables[1];
                o_data_search_tables.TableBranch = o_data_set.Tables[2];
                o_data_search_tables.TableCashbox = o_data_set.Tables[3];

                o_data_search_tables.TableSupplierGroup = o_data_set.Tables[4];
                o_data_search_tables.TableSupplier = o_data_set.Tables[5];
                o_data_search_tables.TableTerminal = o_data_set.Tables[6];

                o_data_search_tables.TableCredit = o_data_set.Tables[7];
                o_data_search_tables.TableCard = o_data_set.Tables[8];

                o_data_search_tables.TableOperationType = o_data_set.Tables[9];
                o_data_search_tables.TableDataStatus = o_data_set.Tables[10];
                o_data_search_tables.TableMatchingAction = o_data_set.Tables[11];
                o_data_search_tables.TableMatchingType = o_data_set.Tables[12];

                o_data_search_tables.TableTemplate = o_data_set.Tables[13];
                o_data_search_tables.TableDataFile = o_data_set.Tables[14];
                o_data_search_tables.TableStrategy = o_data_set.Tables[15];
                o_data_search_tables.TableMatching = o_data_set.Tables[16];
                o_data_search_tables.TableCurrency = o_data_set.Tables[17];

                o_data_search_tables.TableDataPriority = o_data_set.Tables[18];
                o_data_search_tables.TableDataSort = o_data_set.Tables[19];
                o_data_search_tables.TableDataField = o_data_set.Tables[20];
                o_data_search_tables.TableDiscountName = o_data_set.Tables[21];
            }
            catch (Exception ex)
            {
                o_data_search_tables.ErrorMessage = ex.Message;
                o_data_search_tables.ErrorMessage = "Error on select data search tables.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Settings(int n_user_id, ref DataTable dt_field_priority, ref DataTable dt_field_sort, ref DataTable dt_data_field, ref string s_error)
        {
            if (n_user_id <= 0) { return; }

            SqlCommand o_command = new SqlCommand("sprDataFieldPriority", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_field_priority = o_data_set.Tables[0];
                dt_field_sort = o_data_set.Tables[1];
                dt_data_field = o_data_set.Tables[2];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select columns settings.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Recalculate_Payment(DataTable dt_data, string s_payment_number_array, ref int n_rows, ref double n_amount, ref string s_error)
        {
            List<int> lst_payment_number = Common.Convert_To_List(s_payment_number_array);

            int n_first_payment_number = lst_payment_number[0];
            int n_last_payment_number = lst_payment_number[lst_payment_number.Count - 1];

            string s_sql = String.Format(" DutyPaymentNumber IN ( {0} ) ", s_payment_number_array);

            if (dt_data.Select(s_sql + " AND QueryID <> '' ").FirstOrDefault() != null)
            {
                s_error = "Can't include match transactions.";
            }

            if (dt_data.Select(s_sql + " AND IsBalance = True ").FirstOrDefault() != null)
            {
                if (s_error != "") { s_error = String.Format("{0} <br />", s_error); }

                s_error = String.Format("{0} {1}", s_error, "Can't include balance transactions.");
            }

            if ((n_last_payment_number - n_first_payment_number + 1) != lst_payment_number.Count)
            {
                if (s_error != "") { s_error = String.Format("{0} <br />", s_error); }

                s_error = String.Format("{0} {1}", s_error, "Transactions must be selected by sequence.");
            }

            if (dt_data.Select(" DutyPaymentNumber > " + n_last_payment_number + " AND QueryID <> '' ").FirstOrDefault() != null)
            {
                if (s_error != "") { s_error = String.Format("{0} <br />", s_error); }

                s_error = String.Format("{0} {1}", s_error, "Selected transactions must be after all match transactions.");
            }

            s_error = s_error.Trim();

            DataRow[] dr_data = dt_data.Select(s_sql);

            if (dr_data.Length > 0)
            {
                n_rows = dr_data.Length;
                n_amount = dr_data.CopyToDataTable().AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
            }
        }

        public static void Recalculate_Match(DataTable dt_inside, DataTable dt_outside, string s_inside_id_array, string s_outside_id_array, ref int n_inside_rows, ref int n_outside_rows, ref double n_inside_amount, ref double n_outside_amount, ref string s_error)
        {
            string s_sql = " NOT ID IN ( {0} ) ";

            if (s_inside_id_array != "")
            {
                string s_sql_resplit = String.Format(" ID IN ( {0} ) AND MatchingActionName IS NOT NULL AND MatchingActionName = 'Resplit' ", s_inside_id_array);

                if (dt_inside.Select(s_sql_resplit).FirstOrDefault() != null)
                {
                    s_error = "Can't remove items from 'Match Group' marked as 'Resplit'.";
                }

                string s_inside_sql = String.Format(s_sql, s_inside_id_array);

                DataRow[] dr_inside = dt_inside.Select(s_inside_sql);

                if (dr_inside.Length > 0)
                {
                    n_inside_rows = dr_inside.Length;
                    n_inside_amount = dr_inside.CopyToDataTable().AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
                }
            }
            else
            {
                n_inside_rows = dt_inside.Rows.Count;
                n_inside_amount = dt_inside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
            }

            if (s_outside_id_array != "")
            {
                string s_outside_sql = String.Format(s_sql, s_outside_id_array);

                DataRow[] dr_outside = dt_outside.Select(s_outside_sql);

                if (dr_outside.Length > 0)
                {
                    n_outside_rows = dr_outside.Length;
                    n_outside_amount = dr_outside.CopyToDataTable().AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
                }
            }
            else
            {
                n_outside_rows = dt_outside.Rows.Count;
                n_outside_amount = dt_outside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
            }

            if (s_error == "" && n_inside_amount != n_outside_amount)
            {
                s_error = "Inside & Outside Amounts must be equals.";
            }
        }

        public static void Recalculate_Matching(int n_user_id, string s_inside_query_id_array, string s_outside_query_id_array, ref int n_inside_rows, ref int n_outside_rows, ref double n_inside_amount, ref double n_outside_amount, ref string s_error)
        {
            if (s_inside_query_id_array == "" && s_outside_query_id_array == "")
            {
                s_error = "No item was selected.";
                return;
            }

            string s_query_id_array = Common.Get_Distinct_Values(s_inside_query_id_array, s_outside_query_id_array);

            DataTable dt_query_id = Common.Convert_To_Table(s_query_id_array);

            SqlCommand o_command = new SqlCommand("sprDataRecalculateMatching", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });

            o_command.Parameters.AddWithValue("@TableQueryID", dt_query_id);
            o_command.Parameters["@TableQueryID"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    n_inside_rows = (int)o_reader["RowsInside"];
                    n_outside_rows = (int)o_reader["RowsOutside"];

                    n_inside_amount = (double)o_reader["AmountInside"];
                    n_outside_amount = (double)o_reader["AmountOutside"];

                    s_error = o_reader["Error"].ToString();
                }
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on racalculate matching transactions.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Recalculate_Not_Matching(int n_user_id, string s_inside_id_array, string s_outside_id_array, ref int n_company_id, ref int n_inside_rows_selected, ref int n_outside_rows_selected, ref double n_inside_amount_selected, ref double n_outside_amount_selected, ref string s_error)
        {
            if (s_inside_id_array == "" && s_outside_id_array == "")
            {
                s_error = "No item was selected.";
                return;
            }

            DataTable dt_inside_id = Common.Convert_To_Table(s_inside_id_array, "decimal");
            DataTable dt_outside_id = Common.Convert_To_Table(s_outside_id_array, "decimal");

            SqlCommand o_command = new SqlCommand("sprDataRecalculateNotMatching", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });

            o_command.Parameters.AddWithValue("@TableInsideID", dt_inside_id);
            o_command.Parameters["@TableInsideID"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableOutsideID", dt_outside_id);
            o_command.Parameters["@TableOutsideID"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    n_company_id = (int)o_reader["CompanyID"];

                    n_inside_rows_selected = (int)o_reader["RowsInside"];
                    n_outside_rows_selected = (int)o_reader["RowsOutside"];

                    n_inside_amount_selected = (double)o_reader["AmountInside"];
                    n_outside_amount_selected = (double)o_reader["AmountOutside"];

                    s_error = o_reader["Error"].ToString();
                }
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on racalculate not matching transactions.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static int Update_Matching(int n_user_id, string s_comment, string s_inside_id_array, string s_outside_id_array, MatchingBalanceModel o_matching_balance, ref string s_error, string allCheckBoxChecked, string s_where_inside, string s_where_outside)
        {
            int n_rows_affected = 0;

            //if (n_user_id <= 0 || (s_inside_id_array == "" && s_outside_id_array == "")) { goto Finish; }
            if (n_user_id <= 0) { goto Finish; }

            // CREATE dt_inside_id & dt_outside_id - TYPE OF typSingleColumnInt

            DataTable dt_inside_id = Common.Convert_To_Table(s_inside_id_array, "decimal");
            DataTable dt_outside_id = Common.Convert_To_Table(s_outside_id_array, "decimal");

            // EXECUTE QUERY

            SqlCommand o_command = new SqlCommand("sprMatchingUpdateData", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            if (o_matching_balance != null)
            {
                o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
                o_command.Parameters["@CompanyID"].Value = o_matching_balance.CompanyID;

                o_command.Parameters.Add("@OperationTypeID", SqlDbType.Int);
                o_command.Parameters["@OperationTypeID"].Value = o_matching_balance.OperationTypeID;

                o_command.Parameters.Add("@DutyPaymentAmount", SqlDbType.Float);
                o_command.Parameters["@DutyPaymentAmount"].Value = o_matching_balance.DutyPaymentAmount;
            }

            o_command.Parameters.Add("@QueryID", SqlDbType.NVarChar, 36);
            o_command.Parameters["@QueryID"].Value = Guid.NewGuid().ToString();

            o_command.Parameters.Add("@Comment", SqlDbType.NVarChar, 50);
            o_command.Parameters["@Comment"].Value = s_comment;

            o_command.Parameters.AddWithValue("@TableInsideID", dt_inside_id);
            o_command.Parameters["@TableInsideID"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableOutsideID", dt_outside_id);
            o_command.Parameters["@TableOutsideID"].SqlDbType = SqlDbType.Structured;
            ////////
            o_command.Parameters.Add("@allCheckBoxChecked", SqlDbType.NVarChar, 10);
            o_command.Parameters["@allCheckBoxChecked"].Value = allCheckBoxChecked;

            o_command.Parameters.Add("@s_where_inside", SqlDbType.NVarChar, 500);
            o_command.Parameters["@s_where_inside"].Value = s_where_inside;

            o_command.Parameters.Add("@s_where_outside", SqlDbType.NVarChar, 500);
            o_command.Parameters["@s_where_outside"].Value = s_where_outside;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update matching by ID.";
                goto Finish;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Finish:

            return n_rows_affected;
        }

        public static void Update_Settings(int n_user_id, string s_inside_field_priority, string s_outside_field_priority, string s_inside_field_hidden, string s_outside_field_hidden, string s_inside_sort_order, string s_outside_sort_order, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprDataFieldPriorityUpdate", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@InsideFieldPriority", SqlDbType.NVarChar, -1);
            o_command.Parameters["@InsideFieldPriority"].Value = s_inside_field_priority;

            o_command.Parameters.Add("@OutsideFieldPriority", SqlDbType.NVarChar, -1);
            o_command.Parameters["@OutsideFieldPriority"].Value = s_outside_field_priority;

            o_command.Parameters.Add("@InsideFieldHidden", SqlDbType.NVarChar, -1);
            o_command.Parameters["@InsideFieldHidden"].Value = s_inside_field_hidden;

            o_command.Parameters.Add("@OutsideFieldHidden", SqlDbType.NVarChar, -1);
            o_command.Parameters["@OutsideFieldHidden"].Value = s_outside_field_hidden;

            o_command.Parameters.Add("@InsideSortOrder", SqlDbType.NVarChar, -1);
            o_command.Parameters["@InsideSortOrder"].Value = s_inside_sort_order;

            o_command.Parameters.Add("@OutsideSortOrder", SqlDbType.NVarChar, -1);
            o_command.Parameters["@OutsideSortOrder"].Value = s_outside_sort_order;

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update columns settings.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static int Restore_Payment(int n_user_id, string s_unique_id, ref string s_error)
        {
            int n_rows_affected = 0;

            SqlCommand o_command = new SqlCommand("sprData_Restore_Payment", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.NVarChar, 36) { Value = s_unique_id });

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on data payment restore.";
                goto Finish;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Finish:

            return n_rows_affected;
        }

        public static int Change_Payment(DataTable dt_canceled, int n_user_id, int n_split_payments_count, double n_split_first_payment_amount, double n_split_duty_payment_amount, double n_remaining_payments_amount, string s_unique_id, bool b_splitted, ref string s_error)
        {
            int n_rows_affected = 0;

            if (dt_canceled.Rows.Count == 0 || n_user_id <= 0 || n_split_payments_count <= 0 || n_split_first_payment_amount == 0 || n_split_duty_payment_amount == 0 || Is_Guid(s_unique_id) == false)
            {
                s_error = "Error on Change Payment: missing parameters.";
                goto Finish;
            }

            SqlCommand o_command = new SqlCommand("sprData_Change_Payment", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@PaymentsCount_Split", SqlDbType.Int) { Value = n_split_payments_count });
            o_command.Parameters.Add(new SqlParameter("@FirstPaymentAmount_Split", SqlDbType.Float) { Value = n_split_first_payment_amount });
            o_command.Parameters.Add(new SqlParameter("@DutyPaymentAmount_Split", SqlDbType.Float) { Value = n_split_duty_payment_amount });
            o_command.Parameters.Add(new SqlParameter("@RemainingPaymentsAmount", SqlDbType.Float) { Value = n_remaining_payments_amount });
            o_command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.NVarChar, 36) { Value = s_unique_id });
            o_command.Parameters.Add(new SqlParameter("@QueryID", SqlDbType.NVarChar, 36) { Value = Guid.NewGuid().ToString() });
            o_command.Parameters.Add(new SqlParameter("@IsSplitted", SqlDbType.Bit) { Value = b_splitted });

            o_command.Parameters.AddWithValue("@TableCanceledID", dt_canceled);
            o_command.Parameters["@TableCanceledID"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on data payment change.";
                goto Finish;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Finish:

            return n_rows_affected;
        }

        public static int Change_Status(int n_user_id, int n_status_id, string s_inside_id_array, string s_outside_id_array, ref string s_error)
        {
            int n_rows_affected = 0;

            if (n_user_id <= 0 || n_status_id < 0 || (s_inside_id_array == "" && s_outside_id_array == ""))
            {
                s_error = "Error on Change Status: missing parameters.";
                goto Finish;
            }

            // CREATE dt_inside_id & dt_outside_id - TYPE OF typSingleColumnInt

            DataTable dt_inside_id = Common.Convert_To_Table(s_inside_id_array, "decimal");
            DataTable dt_outside_id = Common.Convert_To_Table(s_outside_id_array, "decimal");

            // EXECUTE QUERY

            SqlCommand o_command = new SqlCommand("sprData_Change_Status", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@StatusID", SqlDbType.Int);
            o_command.Parameters["@StatusID"].Value = n_status_id;

            o_command.Parameters.AddWithValue("@TableInsideID", dt_inside_id);
            o_command.Parameters["@TableInsideID"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableOutsideID", dt_outside_id);
            o_command.Parameters["@TableOutsideID"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on data status change.";
                goto Finish;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Finish:

            return n_rows_affected;
        }

        public static int Change_Comment(int n_user_id, string s_comment, string s_inside_id_array, string s_outside_id_array, ref string s_error)
        {
            int n_rows_affected = 0;

            if (n_user_id <= 0 || (s_inside_id_array == "" && s_outside_id_array == ""))
            {
                s_error = "Error on Change Comments: missing parameters.";
                goto Finish;
            }

            // CREATE dt_inside_id & dt_outside_id - TYPE OF typSingleColumnInt

            DataTable dt_inside_id = Common.Convert_To_Table(s_inside_id_array, "decimal");
            DataTable dt_outside_id = Common.Convert_To_Table(s_outside_id_array, "decimal");

            // EXECUTE QUERY

            SqlCommand o_command = new SqlCommand("sprData_Change_Comment", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@Comment", SqlDbType.NVarChar, 50) { Value = s_comment });

            o_command.Parameters.AddWithValue("@TableInsideID", dt_inside_id);
            o_command.Parameters["@TableInsideID"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableOutsideID", dt_outside_id);
            o_command.Parameters["@TableOutsideID"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on data comments change.";
                goto Finish;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Finish:

            return n_rows_affected;
        }

        public static int Delete_Match(int n_user_id, int? n_matching_id, string s_query_id_array, ref string s_error, string comment)
        {
            int n_rows_affected = 0;

            if (n_user_id <= 0 || s_query_id_array == "") { goto Finish; }

            // CREATE dt_query_id - TYPE OF typSingleColumnString

            DataTable dt_query_id = Common.Convert_To_Table(s_query_id_array);

            // EXECUTE QUERY

            SqlCommand o_command = new SqlCommand("sprMatchingDeleteMatch", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            if (n_matching_id != null)
            {
                o_command.Parameters.Add("@MatchingID", SqlDbType.Int);
                o_command.Parameters["@MatchingID"].Value = n_matching_id;
            }

            o_command.Parameters.AddWithValue("@TableQueryID", dt_query_id);
            o_command.Parameters["@TableQueryID"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.Add(new SqlParameter("@Comment", SqlDbType.NVarChar, 50) { Value = comment });

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete match.";
                goto Finish;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Finish:

            return n_rows_affected;
        }

        public static int Delete_Match_Item(int n_user_id, string s_query_id, string s_inside_id_array, string s_outside_id_array, ref string s_error)
        {
            int n_rows_affected = 0;

            if (n_user_id <= 0 || (s_inside_id_array == "" && s_outside_id_array == "")) { goto Finish; }

            // CREATE dt_inside_id & dt_outside_id - TYPE OF typSingleColumnInt

            DataTable dt_inside_id = Common.Convert_To_Table(s_inside_id_array, "decimal");
            DataTable dt_outside_id = Common.Convert_To_Table(s_outside_id_array, "decimal");

            // EXECUTE QUERY

            SqlCommand o_command = new SqlCommand("sprMatchingDeleteItem", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@QueryID", SqlDbType.NVarChar, 36);
            o_command.Parameters["@QueryID"].Value = s_query_id;

            o_command.Parameters.AddWithValue("@TableInsideID", dt_inside_id);
            o_command.Parameters["@TableInsideID"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableOutsideID", dt_outside_id);
            o_command.Parameters["@TableOutsideID"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete match item.";
                goto Finish;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Finish:

            return n_rows_affected;
        }

        public static void Update_Row(ref DataRow dr_data, int n_payments_count, double n_transaction_gross_amount, double n_first_payment_amount, ref string s_error)
        {
            // == Update_Row == FUNCTION IS NOT UPDATE dr_data IF n_payments_count = 1 !!!

            if (n_payments_count == 1) { return; }

            double n_duty_payment_amount = 0, n_remaining_payments_amount = 0;

            long n_transaction_gross_amount_cent = (long)Math.Round((n_transaction_gross_amount * 100), 0);
            long n_first_payment_amount_cent = (long)Math.Round((n_first_payment_amount * 100), 0);

            if (n_first_payment_amount != 0)
            {
                n_duty_payment_amount = n_first_payment_amount;

                long n_remaining_payments_amount_cent = n_transaction_gross_amount_cent - n_first_payment_amount_cent;
                long n_mod = n_remaining_payments_amount_cent % (n_payments_count - 1);

                if (n_mod != 0)
                {
                    s_error = "'First Payment Amount' does not allow to evenly distribute the balance.";
                }

                n_remaining_payments_amount = (double)n_remaining_payments_amount_cent / (double)100;
            }
            else
            {
                long n_mod = n_transaction_gross_amount_cent % n_payments_count;

                n_transaction_gross_amount_cent = n_transaction_gross_amount_cent - n_mod;

                long n_duty_payment_amount_cent = n_transaction_gross_amount_cent / n_payments_count;
                long n_remaining_payments_amount_cent = n_transaction_gross_amount_cent - n_duty_payment_amount_cent;

                n_first_payment_amount_cent = n_duty_payment_amount_cent + n_mod;

                n_first_payment_amount = (double)n_first_payment_amount_cent / (double)100;
                n_duty_payment_amount = (double)n_duty_payment_amount_cent / (double)100;
                n_remaining_payments_amount = (double)n_remaining_payments_amount_cent / (double)100;
            }

            string s_first_payment_amount = n_first_payment_amount.ToString();
            string s_duty_payment_amount = n_duty_payment_amount.ToString();
            string s_remaining_payments_amount = n_remaining_payments_amount.ToString();

            if (s_first_payment_amount.Contains("."))
            {
                if (s_first_payment_amount.Split('.')[1].Length > 2)
                {
                    n_first_payment_amount = Math.Round(n_first_payment_amount, 2);
                }
            }

            if (s_duty_payment_amount.Contains("."))
            {
                if (s_duty_payment_amount.Split('.')[1].Length > 2)
                {
                    n_duty_payment_amount = Math.Round(n_duty_payment_amount, 2);
                }
            }

            if (s_remaining_payments_amount.Contains("."))
            {
                if (s_remaining_payments_amount.Split('.')[1].Length > 2)
                {
                    n_remaining_payments_amount = Math.Round(n_remaining_payments_amount, 2);
                }
            }

            dr_data["FirstPaymentAmount"] = n_first_payment_amount;
            dr_data["DutyPaymentAmount"] = n_duty_payment_amount;
            dr_data["RemainingPaymentsAmount"] = n_remaining_payments_amount;
        }

        public static void Split_Row(ref DataTable dt_split, DataRow dr_first_payment, int n_start_payment_number, int n_end_payment_number, double n_duty_payment_amount, double n_remaining_payments_amount, DateTime? d_payment_date)
        {
            // == Split_Row == FUNCTION IS NOT ADD dr_first_payment TO dt_split !!!

            int n_next_payment_month = 0;

            for (int i = n_start_payment_number; i <= n_end_payment_number; i++)
            {
                n_next_payment_month++;

                n_remaining_payments_amount -= n_duty_payment_amount;

                DataRow dr_split = dt_split.NewRow();

                dr_split.ItemArray = (object[])dr_first_payment.ItemArray.Clone();

                dr_split["DutyPaymentNumber"] = i;
                dr_split["DutyPaymentAmount"] = n_duty_payment_amount;
                dr_split["RemainingPaymentsAmount"] = n_remaining_payments_amount;
                dr_split["IsSplitted"] = true;

                if (d_payment_date != null)
                {
                    DateTime d_date = d_payment_date.Value.AddMonths(n_next_payment_month);

                    dr_split["PaymentDate"] = d_date.Year + "-" + d_date.Month + "-" + d_date.Day;
                }

                dt_split.Rows.Add(dr_split);
            }
        }

        // BIND HEADER - INSIDE & OUTSIDE TABLES

        public static void Bind_Grid_Data_Header(GridViewRow gv_row, DataTable dt_data_field, string s_table, bool b_checked, bool b_disabled)
        {
            for (int i = 0; i < gv_row.Cells.Count; i++)
            {
                TableCell o_cell = gv_row.Cells[i];

                string s_cell_text = Get_Cell_Text(ref o_cell);

                if (s_cell_text == "IsSelected")
                {
                    o_cell.Controls.Add(Get_CheckBox_Select("0", s_table, true, b_checked, b_disabled));
                }
                else
                {
                    int n_width = 150;

                    switch (s_cell_text)
                    {
                        case "QueryNumber":
                        case "QueryID":
                        case "NetworkNumber":
                        case "BranchNumber":
                        case "CashBoxNumber":
                        case "TransactionCurrency":
                        case "PaymentCurrency":
                        case "BankNumber":
                        case "BankBranchNumber":

                        case "ClearingCommissionID":
                        case "DiscountCommissionID":
                        case "ClubManagementFeeCommissionID":
                        case "ClubDiscountCommissionID":
                            n_width = 50;
                            break;
                        case "UniqueID":
                        case "DataFileID":
                        case "CardPrefix":
                        case "CardNumber":
                        case "PaymentsCount":
                        case "DutyPaymentNumber":
                        case "ClearingCommission":
                        case "NotElectronicCommission":
                        case "ManualCommission":
                        case "CancellationCommission":
                        case "TelephoneCommission":
                        case "DiscountCommission":
                        case "ClubManagementCommission":
                        case "ClubSaving":
                        case "VAT":
                        case "ExchangeRate":
                        case "IsSplitted":
                        case "IsBalance":
                        case "IsAbroad":

                        case "AgPerClearingCommission":
                        case "AgPerDiscountCommission":
                        case "AgPerClubManagementFeeCommission":
                        case "AgPerClubDiscountFeeCommission":
                        case "CalculatedIclearingCommission":
                        case "CalculatedIDiscountCommission":
                        case "CalculatedIclubManagementFeeCommission":
                        case "CalculatedIclubDiscountFeeCommission":
                        case "AcPerClearingCommission":
                        case "AcPerDiscountCommission":
                        case "AcPerClubManagementFeeCommission":
                        case "AcPerClubDiscountFeeCommission":
                        case "DiffClearingCommission":
                        case "DiffDiscountCommission":
                        case "DiffClubManagementFeeCommission":
                        case "DiffClubDiscountFeeCommission":
                        case "correctincorrectcommissions":
                        case "CorrectIncorrectCommissionsDiscount":
                        case "CorrectIncorrectCommissionsClubManagementFee":
                        case "CorrectIncorrectCommissionsClubDiscountFee":
                        case "IsClubCommissionvalid":

                        case "GrossAmountCount":
                            n_width = 60;
                            break;
                        case "MatchingID":
                        case "MatchingActionName":
                        case "TerminalNumber":
                        case "SupplierNumber":
                        case "SupplierGroupNumber":
                        case "ClubNumber":
                            n_width = 70;
                            break;
                        case "CreditBrand":
                        case "CardBrand":
                        case "TransactionDate":
                        case "TransmissionDate":
                        case "PaymentDate":
                        case "AbsorptionDate":
                        case "PaymentDateActual":
                        case "InvoiceDate":
                        case "AccountNumber":

                        case "ClearinfCalculationDate":
                        case "DiscountCalculationDate":
                        case "ClubManagementFeeCalculationDate":
                        case "ClubDiscountFeeCalculationDate":

                        case "clearingcommission":
                        case "notelectroniccommission":
                        case "manualcommission":
                        case "cancellationcommission":
                        case "telephonecommission":
                        case "discountcommission":
                        case "clubmanagementcommission":
                        case "clubsaving":
                        case "vat":
                            n_width = 80;
                            break;
                        case "CompanyNumber":
                        case "OperationType":
                        case "TransactionGrossAmount":
                        case "FirstPaymentAmount":
                        case "DutyPaymentAmount":
                        case "RemainingPaymentsAmount":
                        case "netPaymentAmount":

                        case "DiscountName":
                            n_width = 90;
                            break;
                        case "ID":
                        case "MatchingTypeName":
                        case "ReceiptNumber":
                        case "TransmissionNumber":
                        case "VoucherNumber":
                        case "ConfirmationNumber":
                        case "InvoiceNumber":
                            n_width = 100;
                            break;
                        case "StatusName":
                            n_width = 110;
                            break;
                        case "TemplateName":
                        case "StrategyName":
                            n_width = 120;
                            break;
                    }

                    DataRow dr_data_field = dt_data_field.Select(" FieldName = '" + s_cell_text + "' ").FirstOrDefault();

                    if (dr_data_field != null)
                    {
                        s_cell_text = dr_data_field["FieldDescription"].ToString();
                    }

                    o_cell.Text = String.Format("<div style='width: {0}px;'>{1}</div>", n_width, s_cell_text);
                }
            }
        }

        // BIND ROW - INSIDE & OUTSIDE TABLES

        public static void Bind_Grid_Data_Row(GridViewRow gv_row, List<string> lst_field_priority, string s_select_inside, string s_select_outside, string s_table, string s_mode, string S_Group_By, string s_query_id_href = "")
        {
            string s_group_byout = System.Web.HttpContext.Current.Session["GroupByOut"] != null ? System.Web.HttpContext.Current.Session["GroupByOut"].ToString() : "";
            //string strGroup = string.Empty;
            //if (s_table.ToLower().Trim().Equals("inside"))
            //    strGroup = S_Group_By;
            //else
            //    strGroup = s_group_byout;
            if ((string.IsNullOrEmpty(S_Group_By) && s_table.ToLower().Trim().Equals("inside")) || (string.IsNullOrEmpty(s_group_byout) && s_table.ToLower().Trim().Equals("outside")))  //// Without Group By ---
            {
                // CREATE LISTS FOR SELECTED ITEMS

                List<string> lst_select_inside = new List<string>();
                List<string> lst_select_outside = new List<string>();

                if (s_select_inside != "")
                {
                    lst_select_inside = s_select_inside.Split(',').ToList();
                }

                if (s_select_outside != "")
                {
                    lst_select_outside = s_select_outside.Split(',').ToList();
                }

                bool b_inside = (s_table == "Inside");

                // == COMMON TableCell VARIABLES ==

                int i_select = lst_field_priority.IndexOf("IsSelected");

                int i_id = lst_field_priority.IndexOf("ID");
                int i_unique_id = lst_field_priority.IndexOf("UniqueID");
                int i_query_id = lst_field_priority.IndexOf("QueryID");
                int i_matching_id = lst_field_priority.IndexOf("MatchingID");
                int i_matching_action = lst_field_priority.IndexOf("MatchingActionName");
                int i_data_file_id = lst_field_priority.IndexOf("DataFileID");
                int i_duty_payment_number = lst_field_priority.IndexOf("DutyPaymentNumber");
                int i_transaction_gross_amount = lst_field_priority.IndexOf("TransactionGrossAmount");
                int i_duty_payment_amount = lst_field_priority.IndexOf("DutyPaymentAmount");
                int i_remaining_payments_amount = lst_field_priority.IndexOf("RemainingPaymentsAmount");
                int i_transaction_date = lst_field_priority.IndexOf("TransactionDate");
                int i_transmission_date = lst_field_priority.IndexOf("TransmissionDate");
                int i_paymen_date = lst_field_priority.IndexOf("PaymentDate");

                TableCell tc_select = gv_row.Cells[i_select];

                tc_select.CssClass = "bg-gray";

                TableCell tc_id = gv_row.Cells[i_id];
                TableCell tc_unique_id = gv_row.Cells[i_unique_id];
                TableCell tc_query_id = gv_row.Cells[i_query_id];
                TableCell tc_matching_id = gv_row.Cells[i_matching_id];
                TableCell tc_matching_action = gv_row.Cells[i_matching_action];
                TableCell tc_data_file_id = gv_row.Cells[i_data_file_id];
                TableCell tr_duty_payment_number = gv_row.Cells[i_duty_payment_number];
                TableCell tc_transaction_gross_amount = gv_row.Cells[i_transaction_gross_amount];
                TableCell tc_duty_payment_amount = gv_row.Cells[i_duty_payment_amount];
                TableCell tc_remaining_payments_amount = gv_row.Cells[i_remaining_payments_amount];
                TableCell tc_transaction_date = gv_row.Cells[i_transaction_date];
                TableCell tc_transmission_date = gv_row.Cells[i_transmission_date];
                TableCell tc_paymen_date = gv_row.Cells[i_paymen_date];

                decimal n_id = 0;
                decimal.TryParse(tc_id.Text, out n_id);

                bool b_select = false;
                bool.TryParse(tc_select.Text, out b_select);

                string s_unique_id = Get_Cell_Text(ref tc_unique_id);
                string s_query_id = Get_Cell_Text(ref tc_query_id);

                int n_duty_payment_number = 0;
                int.TryParse(tr_duty_payment_number.Text, out n_duty_payment_number);

                // ID

                if (n_id <= 0)
                {
                    tc_id.Controls.Add(Get_Anchor_Error("ID not exists."));
                }

                // UniqueID

                if (Is_Guid(s_unique_id) == false) { s_unique_id = ""; }

                if (s_unique_id != "")
                {
                    HtmlAnchor lnk_unique_id = new HtmlAnchor();

                    lnk_unique_id.InnerHtml = "Source";
                    lnk_unique_id.HRef = "javascript: void(0);";
                    lnk_unique_id.Attributes.Add("onclick", "javascript: payment_click('" + s_unique_id + "', '" + s_table + "');");

                    tc_unique_id.Controls.Add(lnk_unique_id);
                }

                // QueryID

                if (Is_Guid(s_query_id) == false) { s_query_id = ""; }

                if (s_query_id != "")
                {
                    HtmlAnchor lnk_query_id = new HtmlAnchor();

                    lnk_query_id.InnerHtml = "Match";

                    if (s_query_id_href != "")
                    {
                        lnk_query_id.HRef = String.Format("{0}&qid={1}", s_query_id_href, s_query_id);
                    }
                    else
                    {
                        lnk_query_id.HRef = "javascript: void(0);";
                        lnk_query_id.Attributes.Add("onclick", "javascript: match_click('" + s_query_id + "');");
                    }

                    tc_query_id.Controls.Add(lnk_query_id);
                }
                else if (b_select == true)
                {
                    tc_query_id.Controls.Add(Get_Anchor_Error("QueryID not exists."));
                }

                // MatchingID

                int n_matching_id = 0;
                int.TryParse(tc_matching_id.Text, out n_matching_id);

                tc_matching_id.Text = (n_matching_id > 0) ? String.Format("#{0}", n_matching_id) : "";

                // MatchingAction

                if (tc_matching_action.Text.Trim() == "Resplit")
                {
                    gv_row.CssClass = "bg-gray";
                    gv_row.Attributes.Add("data-class", gv_row.CssClass);
                }

                // DutyPaymentNumber

                if (n_duty_payment_number <= 0)
                {
                    tr_duty_payment_number.Controls.Add(Get_Anchor_Error("DutyPaymentNumber not exists."));
                }

                // CHECK-BOX 

                string s_checkbox_value = "";

                switch (s_mode)
                {
                    case "payment":
                        s_checkbox_value = n_duty_payment_number.ToString();
                        break;
                    case "matching":
                        s_checkbox_value = s_query_id;
                        break;
                    default:
                        s_checkbox_value = n_id.ToString();
                        break;
                }

                bool b_checked = false, b_disabled = false;

                switch (s_mode)
                {
                    case "all":
                        b_checked = b_select;
                        b_disabled = true;
                        break;
                    case "payment":
                        b_checked = (b_inside == true && lst_select_inside.Contains(s_checkbox_value) == true);
                        b_disabled = (b_inside == false);
                        //b_disabled = (b_inside == false || lst_select_inside.Count > 0);
                        break;
                    case "match":
                        b_checked = (b_inside == true) ? (lst_select_inside.Contains(s_checkbox_value) == false) : (lst_select_outside.Contains(s_checkbox_value) == false);
                        //b_disabled = (lst_select_inside.Count > 0 || lst_select_outside.Count > 0);
                        break;
                    case "matching":
                        List<string> lst_query_id = lst_select_inside.Union(lst_select_outside).ToList();   // GET DISTINCT VALUES OF lst_inside_query_id & lst_outside_query_id
                        b_checked = (lst_query_id.Contains(s_checkbox_value) == false);
                        //b_disabled = (lst_query_id.Count > 0);
                        break;
                    case "not-matching":
                        b_checked = (b_inside == true) ? (lst_select_inside.Contains(s_checkbox_value) == true) : (lst_select_outside.Contains(s_checkbox_value) == true);
                        //b_disabled = (lst_select_inside.Count > 0 || lst_select_outside.Count > 0);
                        break;
                }

                HtmlInputCheckBox chk_select = Get_CheckBox_Select(s_checkbox_value, s_table, false, b_checked, b_disabled);

                tc_select.Controls.Add(chk_select);

                // DataFileID

                int n_data_file_id = 0;
                int.TryParse(tc_data_file_id.Text, out n_data_file_id);

                tc_data_file_id.Text = (n_data_file_id > 0) ? String.Format("#{0}", n_data_file_id) : "";

                // TransactionGrossAmount

                double n_transaction_gross_amount = 0;
                double.TryParse(tc_transaction_gross_amount.Text, out n_transaction_gross_amount);

                tc_transaction_gross_amount.Text = String.Format("{0:n2}", n_transaction_gross_amount);

                // DutyPaymentAmount

                double n_duty_payment_amount = 0;
                double.TryParse(tc_duty_payment_amount.Text, out n_duty_payment_amount);

                tc_duty_payment_amount.Text = String.Format("{0:n2}", n_duty_payment_amount);

                // RemainingPaymentsAmount

                double n_remaining_payments_amount = 0;
                double.TryParse(tc_remaining_payments_amount.Text, out n_remaining_payments_amount);

                tc_remaining_payments_amount.Text = String.Format("{0:n2}", n_remaining_payments_amount);

                // TransactionDate
                try
                {
                    DateTime d_transaction_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transaction_date));
                    tc_transaction_date.Text = String.Format("{0:dd/MM/yyyy}", d_transaction_date);
                }
                catch (Exception ex) { }

                // TransmissionDate
                try
                {
                    DateTime d_transmission_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transmission_date));
                    tc_transmission_date.Text = String.Format("{0:dd/MM/yyyy}", d_transmission_date);
                }
                catch (Exception ex) { }

                // PaymentDate

                try
                {
                    DateTime d_paymen_date = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date));
                    tc_paymen_date.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date);
                }
                catch (Exception ex) { }

                // == UNIQUE TableCell VARIABLES ==

                if (b_inside == true)
                {
                    int i_first_payment_amount = lst_field_priority.IndexOf("FirstPaymentAmount");
                    TableCell tc_first_payment_amount = gv_row.Cells[i_first_payment_amount];

                    // FirstPaymentAmount
                    double n_first_payment_amount = 0;
                    double.TryParse(tc_first_payment_amount.Text, out n_first_payment_amount);

                    tc_first_payment_amount.Text = String.Format("{0:n2}", n_first_payment_amount);
                }
                else
                {
                    int i_net_payment_amount = lst_field_priority.IndexOf("NetPaymentAmount");
                    int i_absorption_date = lst_field_priority.IndexOf("AbsorptionDate");
                    int i_paymen_date_actual = lst_field_priority.IndexOf("PaymentDateActual");
                    int i_invoice_date = lst_field_priority.IndexOf("InvoiceDate");
                    ////// date format for outside ////
                    int i_clearinfCalculation_date = lst_field_priority.IndexOf("ClearinfCalculationDate");
                    int i_discountCalculationDate_date = lst_field_priority.IndexOf("DiscountCalculationDate");
                    int i_clubManagementFeeCalculation_date = lst_field_priority.IndexOf("ClubManagementFeeCalculationDate");
                    int i_clubDiscountFeeCalculation_date = lst_field_priority.IndexOf("ClubDiscountFeeCalculationDate");

                    TableCell tc_net_payment_amount = gv_row.Cells[i_net_payment_amount];
                    TableCell tc_absorption_date = gv_row.Cells[i_absorption_date];
                    TableCell tc_paymen_date_actual = gv_row.Cells[i_paymen_date_actual];
                    TableCell tc_invoice_date = gv_row.Cells[i_invoice_date];
                    ////// date format for outside ////
                    TableCell tc_clearinfCalculation_date = gv_row.Cells[i_clearinfCalculation_date];
                    TableCell tc_discountCalculationDate_date = gv_row.Cells[i_discountCalculationDate_date];
                    TableCell tc_clubManagementFeeCalculation_date = gv_row.Cells[i_clubManagementFeeCalculation_date];
                    TableCell tc_clubDiscountFeeCalculation_date = gv_row.Cells[i_clubDiscountFeeCalculation_date];

                    // NetPaymentAmount
                    double n_net_payment_amount = 0;
                    double.TryParse(tc_net_payment_amount.Text, out n_net_payment_amount);

                    tc_net_payment_amount.Text = String.Format("{0:n2}", n_net_payment_amount);

                    // AbsorptionDate
                    try
                    {
                        DateTime d_absorption_date = Convert.ToDateTime(Get_Cell_Text(ref tc_absorption_date));
                        tc_absorption_date.Text = String.Format("{0:dd/MM/yyyy}", d_absorption_date);
                    }
                    catch (Exception ex) { }

                    // PaymentDateActual
                    try
                    {
                        DateTime d_paymen_date_actual = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date_actual));
                        tc_paymen_date_actual.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date_actual);
                    }
                    catch (Exception ex) { }

                    // InvoiceDate
                    try
                    {
                        DateTime d_invoice_date = Convert.ToDateTime(Get_Cell_Text(ref tc_invoice_date));
                        tc_invoice_date.Text = String.Format("{0:dd/MM/yyyy}", d_invoice_date);
                    }
                    catch (Exception ex) { }

                    /// outside date format new columns ///////////////////////////
                    // ClearinfCalculation_date
                    try
                    {
                        DateTime d_clearinfCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clearinfCalculation_date));
                        tc_clearinfCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clearinfCalculation_date);
                    }
                    catch (Exception ex) { }

                    // DiscountCalculationDate
                    try
                    {
                        DateTime d_discountCalculationDate_date = Convert.ToDateTime(Get_Cell_Text(ref tc_discountCalculationDate_date));
                        tc_discountCalculationDate_date.Text = String.Format("{0:dd/MM/yyyy}", d_discountCalculationDate_date);
                    }
                    catch (Exception ex) { }

                    // ClubManagementFeeCalculation
                    try
                    {
                        DateTime d_clubManagementFeeCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clubManagementFeeCalculation_date));
                        tc_clubManagementFeeCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clubManagementFeeCalculation_date);
                    }
                    catch (Exception ex) { }

                    // ClubDiscountFeeCalculation
                    try
                    {
                        DateTime d_clubDiscountFeeCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clubDiscountFeeCalculation_date));
                        tc_clubDiscountFeeCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clubDiscountFeeCalculation_date);
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {  /// Group BY ///////////

                // CREATE LISTS FOR SELECTED ITEMS

                List<string> lst_select_inside = new List<string>();
                List<string> lst_select_outside = new List<string>();

                if (s_select_inside != "")
                {
                    lst_select_inside = s_select_inside.Split(',').ToList();
                }

                if (s_select_outside != "")
                {
                    lst_select_outside = s_select_outside.Split(',').ToList();
                }

                bool b_inside = (s_table == "Inside");

                // == COMMON TableCell VARIABLES ==

                int i_select = lst_field_priority.IndexOf("IsSelected");

                int i_id = lst_field_priority.IndexOf("ID");
                int i_unique_id = lst_field_priority.IndexOf("UniqueID");
                int i_query_id = lst_field_priority.IndexOf("QueryID");
                int i_matching_id = lst_field_priority.IndexOf("MatchingID");
                int i_matching_action = lst_field_priority.IndexOf("MatchingActionName");
                int i_data_file_id = lst_field_priority.IndexOf("DataFileID");
                int i_duty_payment_number = lst_field_priority.IndexOf("DutyPaymentNumber");
                int i_transaction_gross_amount = lst_field_priority.IndexOf("TransactionGrossAmount");
                int i_duty_payment_amount = lst_field_priority.IndexOf("DutyPaymentAmount");
                int i_remaining_payments_amount = lst_field_priority.IndexOf("RemainingPaymentsAmount");
                int i_transaction_date = lst_field_priority.IndexOf("TransactionDate");
                int i_transmission_date = lst_field_priority.IndexOf("TransmissionDate");
                int i_paymen_date = lst_field_priority.IndexOf("PaymentDate");

                bool b_select = false;
                TableCell tc_select = null;
                if (i_select != -1 && i_select < gv_row.Cells.Count)
                {
                    tc_select = gv_row.Cells[i_select];
                    tc_select.CssClass = "bg-gray";
                    bool.TryParse(tc_select.Text, out b_select);
                }

                decimal n_id = 0;
                if (i_id != -1 && i_id < gv_row.Cells.Count)
                {
                    TableCell tc_id = gv_row.Cells[i_id];
                    decimal.TryParse(tc_id.Text, out n_id);
                    if (n_id <= 0)
                    {
                        tc_id.Controls.Add(Get_Anchor_Error("ID not exists."));
                    }
                }
                string s_unique_id = string.Empty;
                TableCell tc_unique_id = null;
                if (i_unique_id != -1 && i_unique_id < gv_row.Cells.Count)
                {
                    tc_unique_id = gv_row.Cells[i_unique_id];
                    s_unique_id = Get_Cell_Text(ref tc_unique_id);
                }

                string s_query_id = string.Empty;
                TableCell tc_query_id = null;
                if (i_query_id != -1 && i_query_id < gv_row.Cells.Count)
                {
                    tc_query_id = gv_row.Cells[i_query_id];
                    s_query_id = Get_Cell_Text(ref tc_query_id);
                }
                TableCell tc_matching_id = null;
                if (i_matching_id != -1)
                {
                    tc_matching_id = gv_row.Cells[i_matching_id];
                }
                TableCell tc_matching_action = null;
                if (i_matching_action != -1 && i_matching_action < gv_row.Cells.Count)
                {
                    tc_matching_action = gv_row.Cells[i_matching_action];
                }
                TableCell tc_data_file_id = null;
                if (i_data_file_id != -1 && i_data_file_id < gv_row.Cells.Count)
                {
                    tc_data_file_id = gv_row.Cells[i_data_file_id];
                }
                int n_duty_payment_number = 0;
                TableCell tr_duty_payment_number = null;
                if (i_duty_payment_number != -1 && i_duty_payment_number < gv_row.Cells.Count)
                {
                    tr_duty_payment_number = gv_row.Cells[i_duty_payment_number];
                    int.TryParse(tr_duty_payment_number.Text, out n_duty_payment_number);
                }
                TableCell tc_transaction_gross_amount = null;
                if (i_transaction_gross_amount != -1 && i_transaction_gross_amount < gv_row.Cells.Count)
                {
                    tc_transaction_gross_amount = gv_row.Cells[i_transaction_gross_amount];
                }
                TableCell tc_duty_payment_amount = null;
                if (i_duty_payment_amount != -1 && i_duty_payment_amount < gv_row.Cells.Count)
                {
                    tc_duty_payment_amount = gv_row.Cells[i_duty_payment_amount];
                }
                TableCell tc_remaining_payments_amount = null;
                if (i_remaining_payments_amount != -1 && i_remaining_payments_amount < gv_row.Cells.Count)
                {
                    tc_remaining_payments_amount = gv_row.Cells[i_remaining_payments_amount];
                }
                TableCell tc_transaction_date = null;
                if (i_transaction_date != -1 && i_transaction_date < gv_row.Cells.Count)
                {
                    tc_transaction_date = gv_row.Cells[i_transaction_date];
                }
                TableCell tc_transmission_date = null;
                if (i_transmission_date != -1 && i_transmission_date < gv_row.Cells.Count)
                {
                    tc_transmission_date = gv_row.Cells[i_transmission_date];
                }
                TableCell tc_paymen_date = null;
                if (i_paymen_date != -1 && i_paymen_date < gv_row.Cells.Count)
                {
                    tc_paymen_date = gv_row.Cells[i_paymen_date];
                }

                //decimal n_id = 0;
                //decimal.TryParse(tc_id.Text, out n_id);

                //bool b_select = false;
                //bool.TryParse(tc_select.Text, out b_select);

                //string s_unique_id = Get_Cell_Text(ref tc_unique_id);
                //string s_query_id = Get_Cell_Text(ref tc_query_id);

                //int n_duty_payment_number = 0;
                //int.TryParse(tr_duty_payment_number.Text, out n_duty_payment_number);

                // ID

                //if (n_id <= 0)
                //{
                //    tc_id.Controls.Add(Get_Anchor_Error("ID not exists."));
                //}

                // UniqueID

                if (Is_Guid(s_unique_id) == false) { s_unique_id = ""; }

                if (s_unique_id != "")
                {
                    HtmlAnchor lnk_unique_id = new HtmlAnchor();

                    lnk_unique_id.InnerHtml = "Source";
                    lnk_unique_id.HRef = "javascript: void(0);";
                    lnk_unique_id.Attributes.Add("onclick", "javascript: payment_click('" + s_unique_id + "', '" + s_table + "');");

                    tc_unique_id.Controls.Add(lnk_unique_id);
                }

                // QueryID

                if (Is_Guid(s_query_id) == false) { s_query_id = ""; }

                if (s_query_id != "")
                {
                    HtmlAnchor lnk_query_id = new HtmlAnchor();

                    lnk_query_id.InnerHtml = "Match";

                    if (s_query_id_href != "")
                    {
                        lnk_query_id.HRef = String.Format("{0}&qid={1}", s_query_id_href, s_query_id);
                    }
                    else
                    {
                        lnk_query_id.HRef = "javascript: void(0);";
                        lnk_query_id.Attributes.Add("onclick", "javascript: match_click('" + s_query_id + "');");
                    }

                    tc_query_id.Controls.Add(lnk_query_id);
                }
                else if (b_select == true)
                {
                    tc_query_id.Controls.Add(Get_Anchor_Error("QueryID not exists."));
                }

                // MatchingID
                if (tc_matching_id != null)
                {
                    int n_matching_id = 0;
                    int.TryParse(tc_matching_id.Text, out n_matching_id);

                    tc_matching_id.Text = (n_matching_id > 0) ? String.Format("#{0}", n_matching_id) : "";
                }

                // MatchingAction
                if (tc_matching_action != null)
                {
                    if (tc_matching_action.Text.Trim() == "Resplit")
                    {
                        gv_row.CssClass = "bg-gray";
                        gv_row.Attributes.Add("data-class", gv_row.CssClass);
                    }
                }

                // DutyPaymentNumber

                if (n_duty_payment_number <= 0)
                {
                    if (tr_duty_payment_number != null)
                    {
                        tr_duty_payment_number.Controls.Add(Get_Anchor_Error("DutyPaymentNumber not exists."));
                    }
                }

                // CHECK-BOX 

                string s_checkbox_value = "";

                switch (s_mode)
                {
                    case "payment":
                        s_checkbox_value = n_duty_payment_number.ToString();
                        break;
                    case "matching":
                        s_checkbox_value = s_query_id;
                        break;
                    default:
                        s_checkbox_value = n_id.ToString();
                        break;
                }

                bool b_checked = false, b_disabled = false;

                switch (s_mode)
                {
                    case "all":
                        b_checked = b_select;
                        b_disabled = true;
                        break;
                    case "payment":
                        b_checked = (b_inside == true && lst_select_inside.Contains(s_checkbox_value) == true);
                        b_disabled = (b_inside == false);
                        //b_disabled = (b_inside == false || lst_select_inside.Count > 0);
                        break;
                    case "match":
                        b_checked = (b_inside == true) ? (lst_select_inside.Contains(s_checkbox_value) == false) : (lst_select_outside.Contains(s_checkbox_value) == false);
                        //b_disabled = (lst_select_inside.Count > 0 || lst_select_outside.Count > 0);
                        break;
                    case "matching":
                        List<string> lst_query_id = lst_select_inside.Union(lst_select_outside).ToList();   // GET DISTINCT VALUES OF lst_inside_query_id & lst_outside_query_id
                        b_checked = (lst_query_id.Contains(s_checkbox_value) == false);
                        //b_disabled = (lst_query_id.Count > 0);
                        break;
                    case "not-matching":
                        b_checked = (b_inside == true) ? (lst_select_inside.Contains(s_checkbox_value) == true) : (lst_select_outside.Contains(s_checkbox_value) == true);
                        //b_disabled = (lst_select_inside.Count > 0 || lst_select_outside.Count > 0);
                        break;
                }

                HtmlInputCheckBox chk_select = Get_CheckBox_Select(s_checkbox_value, s_table, false, b_checked, b_disabled);

                if (tc_select != null)
                {
                    tc_select.Controls.Add(chk_select);
                }

                // DataFileID

                if (tc_data_file_id != null)
                {
                    int n_data_file_id = 0;
                    int.TryParse(tc_data_file_id.Text, out n_data_file_id);
                    tc_data_file_id.Text = (n_data_file_id > 0) ? String.Format("#{0}", n_data_file_id) : "";
                }

                // TransactionGrossAmount

                if (tc_transaction_gross_amount != null)
                {
                    double n_transaction_gross_amount = 0;
                    double.TryParse(tc_transaction_gross_amount.Text, out n_transaction_gross_amount);
                    tc_transaction_gross_amount.Text = String.Format("{0:n2}", n_transaction_gross_amount);
                }

                

                // DutyPaymentAmount

                if (tc_duty_payment_amount != null)
                {
                    double n_duty_payment_amount = 0;
                    double.TryParse(tc_duty_payment_amount.Text, out n_duty_payment_amount);
                    tc_duty_payment_amount.Text = String.Format("{0:n2}", n_duty_payment_amount);
                }

                // RemainingPaymentsAmount

                if (tc_remaining_payments_amount != null)
                {
                    double n_remaining_payments_amount = 0;
                    double.TryParse(tc_remaining_payments_amount.Text, out n_remaining_payments_amount);
                    tc_remaining_payments_amount.Text = String.Format("{0:n2}", n_remaining_payments_amount);
                }

                // TransactionDate

                try
                {
                    if (tc_transaction_date != null)
                    {
                        if (tc_transaction_date.Text.Length > 7)
                        {
                            DateTime d_transaction_date = Convert.ToDateTime(Convert.ToDateTime(Get_Cell_Text(ref tc_transaction_date)).ToShortDateString());
                            tc_transaction_date.Text = String.Format("{0:dd/MM/yyyy}", d_transaction_date);
                        }
                    }
                }
                catch (Exception ex) { }

                // TransmissionDate

                try
                {
                    if (tc_transmission_date != null)
                    {
                        if (tc_transmission_date.Text.Length > 7)
                        {
                            DateTime d_transmission_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transmission_date));
                            tc_transmission_date.Text = String.Format("{0:dd/MM/yyyy}", d_transmission_date);
                        }
                    }
                }
                catch (Exception ex) { if (tc_transmission_date != null) tc_transmission_date.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"; }

                // PaymentDate

                try
                {
                    if (tc_paymen_date != null)
                    {
                        if (tc_paymen_date.Text.Length > 7)
                        {
                            DateTime d_paymen_date = Convert.ToDateTime(Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date)).ToShortDateString());
                            tc_paymen_date.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date);
                        }
                    }
                }
                catch (Exception ex) { }

                // == UNIQUE TableCell VARIABLES ==

                if (b_inside == true)
                {
                    int i_first_payment_amount = lst_field_priority.IndexOf("FirstPaymentAmount");
                    //////// date format for outside ////
                    //int i_clearinfCalculation_date = lst_field_priority.IndexOf("ClearinfCalculationDate");
                    //int i_discountCalculationDate_date = lst_field_priority.IndexOf("DiscountCalculationDate");
                    //int i_clubManagementFeeCalculation_date = lst_field_priority.IndexOf("ClubManagementFeeCalculationDate");
                    //int i_clubDiscountFeeCalculation_date = lst_field_priority.IndexOf("ClubDiscountFeeCalculationDate");

                    TableCell tc_first_payment_amount = gv_row.Cells[i_first_payment_amount];
                    //////// date format for outside ////
                    //TableCell tc_clearinfCalculation_date = gv_row.Cells[i_clearinfCalculation_date];
                    //TableCell tc_discountCalculationDate_date = gv_row.Cells[i_discountCalculationDate_date];
                    //TableCell tc_clubManagementFeeCalculation_date = gv_row.Cells[i_clubManagementFeeCalculation_date];
                    //TableCell tc_clubDiscountFeeCalculation_date = gv_row.Cells[i_clubDiscountFeeCalculation_date];

                    // FirstPaymentAmount
                    double n_first_payment_amount = 0;
                    double.TryParse(tc_first_payment_amount.Text, out n_first_payment_amount);

                    tc_first_payment_amount.Text = String.Format("{0:n2}", n_first_payment_amount);
                }
                else
                {
                    int i_net_payment_amount = lst_field_priority.IndexOf("netPaymentAmount");
                    int i_absorption_date = lst_field_priority.IndexOf("AbsorptionDate");
                    int i_paymen_date_actual = lst_field_priority.IndexOf("PaymentDateActual");
                    int i_invoice_date = lst_field_priority.IndexOf("InvoiceDate");

                    if (i_net_payment_amount != -1)
                    {
                        TableCell tc_net_payment_amount = gv_row.Cells[i_net_payment_amount];
                        double n_net_payment_amount = 0;
                        double.TryParse(tc_net_payment_amount.Text, out n_net_payment_amount);
                        tc_net_payment_amount.Text = String.Format("{0:n2}", n_net_payment_amount);
                    }
                    if (i_absorption_date != -1)
                    {
                        TableCell tc_absorption_date = gv_row.Cells[i_absorption_date];
                        try
                        {
                            DateTime d_absorption_date = Convert.ToDateTime(Get_Cell_Text(ref tc_absorption_date));
                            tc_absorption_date.Text = String.Format("{0:dd/MM/yyyy}", d_absorption_date);
                        }
                        catch (Exception ex) { }
                    }
                    if (i_paymen_date_actual != -1)
                    {
                        TableCell tc_paymen_date_actual = gv_row.Cells[i_paymen_date_actual];
                        try
                        {
                            DateTime d_paymen_date_actual = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date_actual));
                            tc_paymen_date_actual.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date_actual);
                        }
                        catch (Exception ex) { }
                    }
                    if (i_invoice_date != -1)
                    {
                        TableCell tc_invoice_date = gv_row.Cells[i_invoice_date];
                        try
                        {
                            DateTime d_invoice_date = Convert.ToDateTime(Get_Cell_Text(ref tc_invoice_date));
                            tc_invoice_date.Text = String.Format("{0:dd/MM/yyyy}", d_invoice_date);
                        }
                        catch (Exception ex) { }
                    }

                    // NetPaymentAmount
                    // AbsorptionDate
                    // PaymentDateActua
                    // InvoiceDate

                    ///// outside date format new columns ///////////////////////////
                    //// ClearinfCalculation_date
                    //try
                    //{
                    //    DateTime d_clearinfCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clearinfCalculation_date));
                    //    tc_clearinfCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clearinfCalculation_date);
                    //}
                    //catch (Exception ex) { }

                    //// DiscountCalculationDate
                    //try
                    //{
                    //    DateTime d_discountCalculationDate_date = Convert.ToDateTime(Get_Cell_Text(ref tc_discountCalculationDate_date));
                    //    tc_discountCalculationDate_date.Text = String.Format("{0:dd/MM/yyyy}", d_discountCalculationDate_date);
                    //}
                    //catch (Exception ex) { }

                    //// ClubManagementFeeCalculation
                    //try
                    //{
                    //    DateTime d_clubManagementFeeCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clubManagementFeeCalculation_date));
                    //    tc_clubManagementFeeCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clubManagementFeeCalculation_date);
                    //}
                    //catch (Exception ex) { }

                    //// ClubDiscountFeeCalculation
                    //try
                    //{
                    //    DateTime d_clubDiscountFeeCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clubDiscountFeeCalculation_date));
                    //    tc_clubDiscountFeeCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clubDiscountFeeCalculation_date);
                    //}
                    //catch (Exception ex) { }

                }

            }
        }

        // BIND ROW - INSIDE TABLE

        public static void Bind_Grid_Data_Row_Inside(TableRow gv_row, List<string> lst_field_priority, string s_select_inside, string s_table, string s_mode, bool IsChkAllCheckBox, string s_query_id_href = "")
        {
            string s_group_by = System.Web.HttpContext.Current.Session["GroupBy"] != null ? System.Web.HttpContext.Current.Session["GroupBy"].ToString() : "";
            if (string.IsNullOrEmpty(s_group_by))  //// Without Group By ---
            {
                // CREATE LISTS FOR SELECTED ITEMS

                List<string> lst_select_inside = new List<string>();
                List<string> lst_select_outside = new List<string>();

                if (s_select_inside != "")
                {
                    lst_select_inside = s_select_inside.Split(',').ToList();
                }
                bool b_inside = (s_table == "Inside");

                // == COMMON TableCell VARIABLES ==

                int i_select = lst_field_priority.IndexOf("IsSelected");

                int i_id = lst_field_priority.IndexOf("ID");
                int i_unique_id = lst_field_priority.IndexOf("UniqueID");
                int i_query_id = lst_field_priority.IndexOf("QueryID");
                int i_matching_id = lst_field_priority.IndexOf("MatchingID");
                int i_matching_action = lst_field_priority.IndexOf("MatchingActionName");
                int i_data_file_id = lst_field_priority.IndexOf("DataFileID");
                int i_duty_payment_number = lst_field_priority.IndexOf("DutyPaymentNumber");
                int i_transaction_gross_amount = lst_field_priority.IndexOf("TransactionGrossAmount");
                int i_duty_payment_amount = lst_field_priority.IndexOf("DutyPaymentAmount");
                int i_remaining_payments_amount = lst_field_priority.IndexOf("RemainingPaymentsAmount");
                int i_transaction_date = lst_field_priority.IndexOf("TransactionDate");
                int i_transmission_date = lst_field_priority.IndexOf("TransmissionDate");
                int i_paymen_date = lst_field_priority.IndexOf("PaymentDate");
                ///
                int i_IsSplitted = lst_field_priority.IndexOf("IsSplitted");
                int i_IsBalance = lst_field_priority.IndexOf("IsBalance");
                ///
                TableCell tc_select = gv_row.Cells[i_select];

                tc_select.CssClass = "bg-gray";

                TableCell tc_id = gv_row.Cells[i_id];
                TableCell tc_unique_id = gv_row.Cells[i_unique_id];
                TableCell tc_query_id = gv_row.Cells[i_query_id];
                TableCell tc_matching_id = gv_row.Cells[i_matching_id];
                TableCell tc_matching_action = gv_row.Cells[i_matching_action];
                TableCell tc_data_file_id = gv_row.Cells[i_data_file_id];
                TableCell tr_duty_payment_number = gv_row.Cells[i_duty_payment_number];
                TableCell tc_transaction_gross_amount = gv_row.Cells[i_transaction_gross_amount];
                TableCell tc_duty_payment_amount = gv_row.Cells[i_duty_payment_amount];
                TableCell tc_remaining_payments_amount = gv_row.Cells[i_remaining_payments_amount];
                TableCell tc_transaction_date = gv_row.Cells[i_transaction_date];
                TableCell tc_transmission_date = gv_row.Cells[i_transmission_date];
                TableCell tc_paymen_date = gv_row.Cells[i_paymen_date];
                ///
                TableCell tc_IsSplitted = gv_row.Cells[i_IsSplitted];
                TableCell tc_IsBalance = gv_row.Cells[i_IsBalance];
                ///

                decimal n_id = 0;
                decimal.TryParse(tc_id.Text, out n_id);

                bool b_select = false;
                bool.TryParse(tc_select.Text, out b_select);
                //////
                bool b_IsSplitted = false;
                bool.TryParse(tc_IsSplitted.Text, out b_IsSplitted);
                bool b_IsBalance = false;
                bool.TryParse(tc_IsBalance.Text, out b_IsBalance);
                //////
                string s_unique_id = Get_Cell_Text(ref tc_unique_id);
                string s_query_id = Get_Cell_Text(ref tc_query_id);

                int n_duty_payment_number = 0;
                int.TryParse(tr_duty_payment_number.Text, out n_duty_payment_number);

                // ID

                if (n_id <= 0)
                {
                    tc_id.Controls.Add(Get_Anchor_Error("ID not exists."));
                }

                // UniqueID

                if (Is_Guid(s_unique_id) == false) { s_unique_id = ""; }

                if (s_unique_id != "")
                {
                    HtmlAnchor lnk_unique_id = new HtmlAnchor();

                    lnk_unique_id.InnerHtml = "Source";
                    lnk_unique_id.HRef = "javascript: void(0);";
                    lnk_unique_id.Attributes.Add("onclick", "javascript: payment_click('" + s_unique_id + "', '" + s_table + "');");

                    tc_unique_id.Controls.Add(lnk_unique_id);
                }

                // QueryID

                if (Is_Guid(s_query_id) == false) { s_query_id = ""; }

                if (s_query_id != "")
                {
                    HtmlAnchor lnk_query_id = new HtmlAnchor();

                    lnk_query_id.InnerHtml = "Match";

                    if (s_query_id_href != "")
                    {
                        lnk_query_id.HRef = String.Format("{0}&qid={1}", s_query_id_href, s_query_id);
                    }
                    else
                    {
                        lnk_query_id.HRef = "javascript: void(0);";
                        lnk_query_id.Attributes.Add("onclick", "javascript: match_click('" + s_query_id + "');");
                    }

                    tc_query_id.Controls.Add(lnk_query_id);
                }
                else if (b_select == true)
                {
                    tc_query_id.Controls.Add(Get_Anchor_Error("QueryID not exists."));
                }

                // MatchingID

                int n_matching_id = 0;
                int.TryParse(tc_matching_id.Text, out n_matching_id);

                tc_matching_id.Text = (n_matching_id > 0) ? String.Format("#{0}", n_matching_id) : "";

                // MatchingAction

                if (tc_matching_action.Text.Trim() == "Resplit")
                {
                    gv_row.CssClass = "bg-gray";
                    gv_row.Attributes.Add("data-class", gv_row.CssClass);
                }

                // DutyPaymentNumber

                if (n_duty_payment_number <= 0)
                {
                    tr_duty_payment_number.Controls.Add(Get_Anchor_Error("DutyPaymentNumber not exists."));
                }

                // CHECK-BOX 

                string s_checkbox_value = "";

                switch (s_mode)
                {
                    case "payment":
                        s_checkbox_value = n_duty_payment_number.ToString();
                        break;
                    case "matching":
                        s_checkbox_value = s_query_id;
                        break;
                    default:
                        s_checkbox_value = n_id.ToString();
                        break;
                }

                bool b_checked = false, b_disabled = false;

                switch (s_mode)
                {
                    case "all":
                        b_checked = b_select;
                        b_disabled = true;
                        break;
                    case "payment":
                        b_checked = (b_inside == true && lst_select_inside.Contains(s_checkbox_value) == true);
                        b_disabled = (b_inside == false);
                        //b_disabled = (b_inside == false || lst_select_inside.Count > 0);
                        break;
                    case "match":
                        b_checked = (lst_select_inside.Contains(s_checkbox_value) == false);
                        //b_disabled = (lst_select_inside.Count > 0 || lst_select_outside.Count > 0);
                        break;
                    case "matching":
                        List<string> lst_query_id = lst_select_inside.Union(lst_select_outside).ToList();   // GET DISTINCT VALUES OF lst_inside_query_id & lst_outside_query_id
                        b_checked = (lst_query_id.Contains(s_checkbox_value) == false);
                        //b_disabled = (lst_query_id.Count > 0);
                        if (IsChkAllCheckBox == true)
                        {
                            b_disabled = true;
                            b_checked = false;
                        }
                        break;
                    case "not-matching":
                        b_checked = (lst_select_inside.Contains(s_checkbox_value) == true);
                        //b_disabled = (lst_select_inside.Count > 0 || lst_select_outside.Count > 0);
                        if (IsChkAllCheckBox == true)
                            b_disabled = true;
                        break;
                }

                HtmlInputCheckBox chk_select = Get_CheckBox_Select(s_checkbox_value, s_table, false, b_checked, b_disabled);
                tc_select.Controls.Add(chk_select);

                /////
                HtmlInputCheckBox chk_IsSplitted = Get_CheckBox_Select("", s_table, false, b_IsSplitted, true);
                tc_IsSplitted.Controls.Add(chk_IsSplitted);
                HtmlInputCheckBox chk_IsBalance = Get_CheckBox_Select("", s_table, false, b_IsBalance, true);
                tc_IsBalance.Controls.Add(chk_IsBalance);
                /////
                // DataFileID

                int n_data_file_id = 0;
                int.TryParse(tc_data_file_id.Text, out n_data_file_id);

                tc_data_file_id.Text = (n_data_file_id > 0) ? String.Format("#{0}", n_data_file_id) : "";

                // TransactionGrossAmount

                double n_transaction_gross_amount = 0;
                double.TryParse(tc_transaction_gross_amount.Text, out n_transaction_gross_amount);

                tc_transaction_gross_amount.Text = String.Format("{0:n2}", n_transaction_gross_amount);

                // DutyPaymentAmount

                double n_duty_payment_amount = 0;
                double.TryParse(tc_duty_payment_amount.Text, out n_duty_payment_amount);

                tc_duty_payment_amount.Text = String.Format("{0:n2}", n_duty_payment_amount);

                // RemainingPaymentsAmount

                double n_remaining_payments_amount = 0;
                double.TryParse(tc_remaining_payments_amount.Text, out n_remaining_payments_amount);

                tc_remaining_payments_amount.Text = String.Format("{0:n2}", n_remaining_payments_amount);

                // TransactionDate

                try
                {
                    DateTime d_transaction_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transaction_date));

                    tc_transaction_date.Text = String.Format("{0:dd/MM/yyyy}", d_transaction_date);
                }
                catch (Exception ex) { }

                // TransmissionDate

                try
                {
                    DateTime d_transmission_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transmission_date));

                    tc_transmission_date.Text = String.Format("{0:dd/MM/yyyy}", d_transmission_date);
                }
                catch (Exception ex) { }

                // PaymentDate

                try
                {
                    DateTime d_paymen_date = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date));

                    tc_paymen_date.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date);
                }
                catch (Exception ex) { }

                // == UNIQUE TableCell VARIABLES ==

                int i_first_payment_amount = lst_field_priority.IndexOf("FirstPaymentAmount");

                TableCell tc_first_payment_amount = gv_row.Cells[i_first_payment_amount];

                // FirstPaymentAmount

                double n_first_payment_amount = 0;
                double.TryParse(tc_first_payment_amount.Text, out n_first_payment_amount);

                tc_first_payment_amount.Text = String.Format("{0:n2}", n_first_payment_amount);
            }
            else
            {  /// Group BY ///////////

                // CREATE LISTS FOR SELECTED ITEMS

                List<string> lst_select_inside = new List<string>();
                List<string> lst_select_outside = new List<string>();

                if (s_select_inside != "")
                {
                    lst_select_inside = s_select_inside.Split(',').ToList();
                }

                //if (s_select_outside != "")
                //{
                //    lst_select_outside = s_select_outside.Split(',').ToList();
                //}

                bool b_inside = (s_table == "Inside");

                // == COMMON TableCell VARIABLES ==

                int i_select = lst_field_priority.IndexOf("IsSelected");

                int i_id = lst_field_priority.IndexOf("ID");
                int i_unique_id = lst_field_priority.IndexOf("UniqueID");
                int i_query_id = lst_field_priority.IndexOf("QueryID");
                int i_matching_id = lst_field_priority.IndexOf("MatchingID");
                int i_matching_action = lst_field_priority.IndexOf("MatchingActionName");
                int i_data_file_id = lst_field_priority.IndexOf("DataFileID");
                int i_duty_payment_number = lst_field_priority.IndexOf("DutyPaymentNumber");
                int i_transaction_gross_amount = lst_field_priority.IndexOf("TransactionGrossAmount");
                int i_duty_payment_amount = lst_field_priority.IndexOf("DutyPaymentAmount");
                int i_remaining_payments_amount = lst_field_priority.IndexOf("RemainingPaymentsAmount");
                int i_transaction_date = lst_field_priority.IndexOf("TransactionDate");
                int i_transmission_date = lst_field_priority.IndexOf("TransmissionDate");
                int i_paymen_date = lst_field_priority.IndexOf("PaymentDate");

                bool b_select = false;
                TableCell tc_select = null;
                if (i_select != -1)
                {
                    tc_select = gv_row.Cells[i_select];
                    tc_select.CssClass = "bg-gray";
                    bool.TryParse(tc_select.Text, out b_select);
                }

                decimal n_id = 0;
                if (i_id != -1)
                {
                    TableCell tc_id = gv_row.Cells[i_id];
                    decimal.TryParse(tc_id.Text, out n_id);
                    if (n_id <= 0)
                    {
                        tc_id.Controls.Add(Get_Anchor_Error("ID not exists."));
                    }
                }
                string s_unique_id = string.Empty;
                TableCell tc_unique_id = null;
                if (i_unique_id != -1)
                {
                    tc_unique_id = gv_row.Cells[i_unique_id];
                    s_unique_id = Get_Cell_Text(ref tc_unique_id);
                }

                string s_query_id = string.Empty;
                TableCell tc_query_id = null;
                if (i_query_id != -1)
                {
                    tc_query_id = gv_row.Cells[i_query_id];
                    s_query_id = Get_Cell_Text(ref tc_query_id);
                }
                TableCell tc_matching_id = null;
                if (i_matching_id != -1)
                {
                    tc_matching_id = gv_row.Cells[i_matching_id];
                }
                TableCell tc_matching_action = null;
                if (i_matching_action != -1)
                {
                    tc_matching_action = gv_row.Cells[i_matching_action];
                }
                TableCell tc_data_file_id = null;
                if (i_data_file_id != -1)
                {
                    tc_data_file_id = gv_row.Cells[i_data_file_id];
                }
                int n_duty_payment_number = 0;
                TableCell tr_duty_payment_number = null;
                if (i_duty_payment_number != -1)
                {
                    tr_duty_payment_number = gv_row.Cells[i_duty_payment_number];
                    int.TryParse(tr_duty_payment_number.Text, out n_duty_payment_number);
                }
                TableCell tc_transaction_gross_amount = null;
                if (i_transaction_gross_amount != -1)
                {
                    tc_transaction_gross_amount = gv_row.Cells[i_transaction_gross_amount];
                }
                TableCell tc_duty_payment_amount = null;
                if (i_duty_payment_amount != -1)
                {
                    tc_duty_payment_amount = gv_row.Cells[i_duty_payment_amount];
                }
                TableCell tc_remaining_payments_amount = null;
                if (i_remaining_payments_amount != -1)
                {
                    tc_remaining_payments_amount = gv_row.Cells[i_remaining_payments_amount];
                }
                TableCell tc_transaction_date = null;
                if (i_transaction_date != -1)
                {
                    tc_transaction_date = gv_row.Cells[i_transaction_date];
                }
                TableCell tc_transmission_date = null;
                if (i_transmission_date != -1)
                {
                    tc_transmission_date = gv_row.Cells[i_transmission_date];
                }
                TableCell tc_paymen_date = null;
                if (i_paymen_date != -1)
                {
                    tc_paymen_date = gv_row.Cells[i_paymen_date];
                }

                //decimal n_id = 0;
                //decimal.TryParse(tc_id.Text, out n_id);

                //bool b_select = false;
                //bool.TryParse(tc_select.Text, out b_select);

                //string s_unique_id = Get_Cell_Text(ref tc_unique_id);
                //string s_query_id = Get_Cell_Text(ref tc_query_id);

                //int n_duty_payment_number = 0;
                //int.TryParse(tr_duty_payment_number.Text, out n_duty_payment_number);

                // ID

                //if (n_id <= 0)
                //{
                //    tc_id.Controls.Add(Get_Anchor_Error("ID not exists."));
                //}

                // UniqueID

                if (Is_Guid(s_unique_id) == false) { s_unique_id = ""; }

                if (s_unique_id != "")
                {
                    HtmlAnchor lnk_unique_id = new HtmlAnchor();

                    lnk_unique_id.InnerHtml = "Source";
                    lnk_unique_id.HRef = "javascript: void(0);";
                    lnk_unique_id.Attributes.Add("onclick", "javascript: payment_click('" + s_unique_id + "', '" + s_table + "');");

                    tc_unique_id.Controls.Add(lnk_unique_id);
                }

                // QueryID

                if (Is_Guid(s_query_id) == false) { s_query_id = ""; }

                if (s_query_id != "")
                {
                    HtmlAnchor lnk_query_id = new HtmlAnchor();

                    lnk_query_id.InnerHtml = "Match";

                    if (s_query_id_href != "")
                    {
                        lnk_query_id.HRef = String.Format("{0}&qid={1}", s_query_id_href, s_query_id);
                    }
                    else
                    {
                        lnk_query_id.HRef = "javascript: void(0);";
                        lnk_query_id.Attributes.Add("onclick", "javascript: match_click('" + s_query_id + "');");
                    }

                    tc_query_id.Controls.Add(lnk_query_id);
                }
                else if (b_select == true)
                {
                    tc_query_id.Controls.Add(Get_Anchor_Error("QueryID not exists."));
                }

                // MatchingID
                if (tc_matching_id != null)
                {
                    int n_matching_id = 0;
                    int.TryParse(tc_matching_id.Text, out n_matching_id);

                    tc_matching_id.Text = (n_matching_id > 0) ? String.Format("#{0}", n_matching_id) : "";
                }

                // MatchingAction
                if (tc_matching_action != null)
                {
                    if (tc_matching_action.Text.Trim() == "Resplit")
                    {
                        gv_row.CssClass = "bg-gray";
                        gv_row.Attributes.Add("data-class", gv_row.CssClass);
                    }
                }

                // DutyPaymentNumber

                if (n_duty_payment_number <= 0)
                {
                    if (tr_duty_payment_number != null)
                    {
                        tr_duty_payment_number.Controls.Add(Get_Anchor_Error("DutyPaymentNumber not exists."));
                    }
                }

                // CHECK-BOX 

                string s_checkbox_value = "";

                switch (s_mode)
                {
                    case "payment":
                        s_checkbox_value = n_duty_payment_number.ToString();
                        break;
                    case "matching":
                        s_checkbox_value = s_query_id;
                        break;
                    default:
                        s_checkbox_value = n_id.ToString();
                        break;
                }

                bool b_checked = false, b_disabled = false;

                switch (s_mode)
                {
                    case "all":
                        b_checked = b_select;
                        b_disabled = true;
                        break;
                    case "payment":
                        b_checked = (b_inside == true && lst_select_inside.Contains(s_checkbox_value) == true);
                        b_disabled = (b_inside == false);
                        //b_disabled = (b_inside == false || lst_select_inside.Count > 0);
                        break;
                    case "match":
                        b_checked = (b_inside == true) ? (lst_select_inside.Contains(s_checkbox_value) == false) : (lst_select_outside.Contains(s_checkbox_value) == false);
                        //b_disabled = (lst_select_inside.Count > 0 || lst_select_outside.Count > 0);
                        break;
                    case "matching":
                        List<string> lst_query_id = lst_select_inside.Union(lst_select_outside).ToList();   // GET DISTINCT VALUES OF lst_inside_query_id & lst_outside_query_id
                        b_checked = (lst_query_id.Contains(s_checkbox_value) == false);
                        //b_disabled = (lst_query_id.Count > 0);
                        break;
                    case "not-matching":
                        b_checked = (b_inside == true) ? (lst_select_inside.Contains(s_checkbox_value) == true) : (lst_select_outside.Contains(s_checkbox_value) == true);
                        //b_disabled = (lst_select_inside.Count > 0 || lst_select_outside.Count > 0);
                        break;
                }

                HtmlInputCheckBox chk_select = Get_CheckBox_Select(s_checkbox_value, s_table, false, b_checked, b_disabled);

                if (tc_select != null)
                {
                    tc_select.Controls.Add(chk_select);
                }

                // DataFileID

                if (tc_data_file_id != null)
                {
                    int n_data_file_id = 0;
                    int.TryParse(tc_data_file_id.Text, out n_data_file_id);
                    tc_data_file_id.Text = (n_data_file_id > 0) ? String.Format("#{0}", n_data_file_id) : "";
                }

                // TransactionGrossAmount

                if (tc_transaction_gross_amount != null)
                {
                    double n_transaction_gross_amount = 0;
                    double.TryParse(tc_transaction_gross_amount.Text, out n_transaction_gross_amount);
                    tc_transaction_gross_amount.Text = String.Format("{0:n2}", n_transaction_gross_amount);
                }

                // DutyPaymentAmount

                if (tc_duty_payment_amount != null)
                {
                    double n_duty_payment_amount = 0;
                    double.TryParse(tc_duty_payment_amount.Text, out n_duty_payment_amount);
                    tc_duty_payment_amount.Text = String.Format("{0:n2}", n_duty_payment_amount);
                }

                // RemainingPaymentsAmount

                if (tc_remaining_payments_amount != null)
                {
                    double n_remaining_payments_amount = 0;
                    double.TryParse(tc_remaining_payments_amount.Text, out n_remaining_payments_amount);
                    tc_remaining_payments_amount.Text = String.Format("{0:n2}", n_remaining_payments_amount);
                }

                // TransactionDate

                try
                {
                    if (tc_transaction_date.Text.Length > 7)
                    {
                        DateTime d_transaction_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transaction_date));
                        tc_transaction_date.Text = String.Format("{0:dd/MM/yyyy}", d_transaction_date);
                    }
                }
                catch (Exception ex) { }

                // TransmissionDate

                try
                {
                    if (tc_transmission_date.Text.Length > 7)
                    {
                        DateTime d_transmission_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transmission_date));
                        tc_transmission_date.Text = String.Format("{0:dd/MM/yyyy}", d_transmission_date);
                    }
                }
                catch (Exception ex) { if (tc_transmission_date != null) tc_transmission_date.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"; }

                // PaymentDate

                try
                {
                    if (tc_paymen_date.Text.Length > 7)
                    {
                        DateTime d_paymen_date = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date));
                        tc_paymen_date.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date);
                    }
                }
                catch (Exception ex) { }

                // == UNIQUE TableCell VARIABLES ==

                if (b_inside == true)
                {
                    int i_first_payment_amount = lst_field_priority.IndexOf("FirstPaymentAmount");

                    TableCell tc_first_payment_amount = gv_row.Cells[i_first_payment_amount];

                    // FirstPaymentAmount

                    double n_first_payment_amount = 0;
                    double.TryParse(tc_first_payment_amount.Text, out n_first_payment_amount);

                    tc_first_payment_amount.Text = String.Format("{0:n2}", n_first_payment_amount);
                }
                else
                {
                    int i_net_payment_amount = lst_field_priority.IndexOf("netPaymentAmount");
                    int i_absorption_date = lst_field_priority.IndexOf("AbsorptionDate");
                    int i_paymen_date_actual = lst_field_priority.IndexOf("PaymentDateActual");
                    int i_invoice_date = lst_field_priority.IndexOf("InvoiceDate");

                    TableCell tc_net_payment_amount = gv_row.Cells[i_net_payment_amount];
                    TableCell tc_absorption_date = gv_row.Cells[i_absorption_date];
                    TableCell tc_paymen_date_actual = gv_row.Cells[i_paymen_date_actual];
                    TableCell tc_invoice_date = gv_row.Cells[i_invoice_date];

                    // NetPaymentAmount

                    double n_net_payment_amount = 0;
                    double.TryParse(tc_net_payment_amount.Text, out n_net_payment_amount);

                    tc_net_payment_amount.Text = String.Format("{0:n2}", n_net_payment_amount);

                    // AbsorptionDate

                    try
                    {
                        DateTime d_absorption_date = Convert.ToDateTime(Get_Cell_Text(ref tc_absorption_date));

                        tc_absorption_date.Text = String.Format("{0:dd/MM/yyyy}", d_absorption_date);
                    }
                    catch (Exception ex) { }

                    // PaymentDateActual

                    try
                    {
                        DateTime d_paymen_date_actual = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date_actual));

                        tc_paymen_date_actual.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date_actual);
                    }
                    catch (Exception ex) { }

                    // InvoiceDate

                    try
                    {
                        DateTime d_invoice_date = Convert.ToDateTime(Get_Cell_Text(ref tc_invoice_date));

                        tc_invoice_date.Text = String.Format("{0:dd/MM/yyyy}", d_invoice_date);
                    }
                    catch (Exception ex) { }
                }

            }
        }

        // BIND ROW - OUTSIDE TABLE

        public static void Bind_Grid_Data_Row_Outside(TableRow gv_row, List<string> lst_field_priority, string s_select_outside, string s_table, string s_mode, bool IsChkAllCheckBox, string s_query_id_href = "")
        {
            // CREATE LISTS FOR SELECTED ITEMS
            string s_group_by = System.Web.HttpContext.Current.Session["GroupBy"] != null ? System.Web.HttpContext.Current.Session["GroupBy"].ToString() : "";
            if (string.IsNullOrEmpty(s_group_by))  //// Without Group By ---
            {
                List<string> lst_select_outside = new List<string>();

                if (s_select_outside != "")
                {
                    lst_select_outside = s_select_outside.Split(',').ToList();
                }

                // bool b_inside = (s_table == "Inside");

                // == COMMON TableCell VARIABLES ==

                int i_select = lst_field_priority.IndexOf("IsSelected");

                int i_id = lst_field_priority.IndexOf("ID");
                int i_unique_id = lst_field_priority.IndexOf("UniqueID");
                int i_query_id = lst_field_priority.IndexOf("QueryID");
                int i_matching_id = lst_field_priority.IndexOf("MatchingID");
                int i_matching_action = lst_field_priority.IndexOf("MatchingActionName");
                int i_data_file_id = lst_field_priority.IndexOf("DataFileID");
                int i_duty_payment_number = lst_field_priority.IndexOf("DutyPaymentNumber");
                int i_transaction_gross_amount = lst_field_priority.IndexOf("TransactionGrossAmount");
                int i_duty_payment_amount = lst_field_priority.IndexOf("DutyPaymentAmount");
                int i_remaining_payments_amount = lst_field_priority.IndexOf("RemainingPaymentsAmount");
                int i_transaction_date = lst_field_priority.IndexOf("TransactionDate");
                int i_transmission_date = lst_field_priority.IndexOf("TransmissionDate");
                int i_paymen_date = lst_field_priority.IndexOf("PaymentDate");
                ///
                int i_IsAbroad = lst_field_priority.IndexOf("IsAbroad");
                ///
                TableCell tc_select = gv_row.Cells[i_select];
                tc_select.CssClass = "bg-gray";

                TableCell tc_id = gv_row.Cells[i_id];
                TableCell tc_unique_id = gv_row.Cells[i_unique_id];
                TableCell tc_query_id = gv_row.Cells[i_query_id];
                TableCell tc_matching_id = gv_row.Cells[i_matching_id];
                TableCell tc_matching_action = gv_row.Cells[i_matching_action];
                TableCell tc_data_file_id = gv_row.Cells[i_data_file_id];
                TableCell tr_duty_payment_number = gv_row.Cells[i_duty_payment_number];
                TableCell tc_transaction_gross_amount = gv_row.Cells[i_transaction_gross_amount];
                TableCell tc_duty_payment_amount = gv_row.Cells[i_duty_payment_amount];
                TableCell tc_remaining_payments_amount = gv_row.Cells[i_remaining_payments_amount];
                TableCell tc_transaction_date = gv_row.Cells[i_transaction_date];
                TableCell tc_transmission_date = gv_row.Cells[i_transmission_date];
                TableCell tc_paymen_date = gv_row.Cells[i_paymen_date];
                ///
                TableCell tc_IsAbroad = gv_row.Cells[i_IsAbroad];
                ///

                decimal n_id = 0;
                decimal.TryParse(tc_id.Text, out n_id);

                bool b_select = false;
                bool.TryParse(tc_select.Text, out b_select);

                string s_unique_id = Get_Cell_Text(ref tc_unique_id);
                string s_query_id = Get_Cell_Text(ref tc_query_id);

                int n_duty_payment_number = 0;
                int.TryParse(tr_duty_payment_number.Text, out n_duty_payment_number);

                ////
                bool b_IsAbroad = false;
                bool.TryParse(tc_IsAbroad.Text, out b_IsAbroad);
                ////

                // ID

                if (n_id <= 0)
                {
                    tc_id.Controls.Add(Get_Anchor_Error("ID not exists."));
                }

                // UniqueID

                if (Is_Guid(s_unique_id) == false) { s_unique_id = ""; }

                if (s_unique_id != "")
                {
                    HtmlAnchor lnk_unique_id = new HtmlAnchor();

                    lnk_unique_id.InnerHtml = "Source";
                    lnk_unique_id.HRef = "javascript: void(0);";
                    lnk_unique_id.Attributes.Add("onclick", "javascript: payment_click('" + s_unique_id + "', '" + s_table + "');");

                    tc_unique_id.Controls.Add(lnk_unique_id);
                }

                // QueryID

                if (Is_Guid(s_query_id) == false) { s_query_id = ""; }

                if (s_query_id != "")
                {
                    HtmlAnchor lnk_query_id = new HtmlAnchor();

                    lnk_query_id.InnerHtml = "Match";

                    if (s_query_id_href != "")
                    {
                        lnk_query_id.HRef = String.Format("{0}&qid={1}", s_query_id_href, s_query_id);
                    }
                    else
                    {
                        lnk_query_id.HRef = "javascript: void(0);";
                        lnk_query_id.Attributes.Add("onclick", "javascript: match_click('" + s_query_id + "');");
                    }

                    tc_query_id.Controls.Add(lnk_query_id);
                }
                else if (b_select == true)
                {
                    tc_query_id.Controls.Add(Get_Anchor_Error("QueryID not exists."));
                }

                // MatchingID

                int n_matching_id = 0;
                int.TryParse(tc_matching_id.Text, out n_matching_id);

                tc_matching_id.Text = (n_matching_id > 0) ? String.Format("#{0}", n_matching_id) : "";

                // MatchingAction

                if (tc_matching_action.Text.Trim() == "Resplit")
                {
                    gv_row.CssClass = "bg-gray";
                    gv_row.Attributes.Add("data-class", gv_row.CssClass);
                }

                // DutyPaymentNumber

                if (n_duty_payment_number <= 0)
                {
                    tr_duty_payment_number.Controls.Add(Get_Anchor_Error("DutyPaymentNumber not exists."));
                }

                // CHECK-BOX 

                string s_checkbox_value = "";

                switch (s_mode)
                {
                    case "payment":
                        s_checkbox_value = n_duty_payment_number.ToString();
                        break;
                    case "matching":
                        s_checkbox_value = s_query_id;
                        break;
                    default:
                        s_checkbox_value = n_id.ToString();
                        break;
                }

                bool b_checked = false, b_disabled = false;

                switch (s_mode)
                {
                    case "all":
                        b_checked = b_select;
                        b_disabled = true;
                        break;
                    case "payment":
                        b_checked = false;
                        b_disabled = false;
                        //b_disabled = (b_inside == false || lst_select_inside.Count > 0);
                        break;
                    case "match":
                        b_checked = (lst_select_outside.Contains(s_checkbox_value) == false);
                        b_disabled = lst_select_outside.Count > 0;
                        break;
                    case "matching":
                        List<string> lst_query_id = lst_select_outside.ToList();   // GET DISTINCT VALUES OF lst_inside_query_id & lst_outside_query_id
                        b_checked = (lst_query_id.Contains(s_checkbox_value) == false);
                        b_disabled = (lst_query_id.Count > 0);
                        if (IsChkAllCheckBox == true)
                        {
                            b_disabled = true;
                            b_checked = false;
                        }
                        break;
                    case "not-matching":
                        b_checked = (lst_select_outside.Contains(s_checkbox_value) == true);
                        b_disabled = (lst_select_outside.Count > 0);
                        if (IsChkAllCheckBox == true)
                            b_disabled = true;
                        break;
                }

                HtmlInputCheckBox chk_select = Get_CheckBox_Select(s_checkbox_value, s_table, false, b_checked, b_disabled);
                tc_select.Controls.Add(chk_select);
                ///// IsAbroad
                HtmlInputCheckBox chk_IsAbroad = Get_CheckBox_Select("", s_table, false, b_IsAbroad, true);
                tc_IsAbroad.Controls.Add(chk_IsAbroad);
                ////
                // DataFileID

                int n_data_file_id = 0;
                int.TryParse(tc_data_file_id.Text, out n_data_file_id);

                tc_data_file_id.Text = (n_data_file_id > 0) ? String.Format("#{0}", n_data_file_id) : "";

                // TransactionGrossAmount

                double n_transaction_gross_amount = 0;
                double.TryParse(tc_transaction_gross_amount.Text, out n_transaction_gross_amount);

                tc_transaction_gross_amount.Text = String.Format("{0:n2}", n_transaction_gross_amount);

                // DutyPaymentAmount

                double n_duty_payment_amount = 0;
                double.TryParse(tc_duty_payment_amount.Text, out n_duty_payment_amount);

                tc_duty_payment_amount.Text = String.Format("{0:n2}", n_duty_payment_amount);

                // RemainingPaymentsAmount

                double n_remaining_payments_amount = 0;
                double.TryParse(tc_remaining_payments_amount.Text, out n_remaining_payments_amount);

                tc_remaining_payments_amount.Text = String.Format("{0:n2}", n_remaining_payments_amount);

                // TransactionDate

                try
                {
                    DateTime d_transaction_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transaction_date));

                    tc_transaction_date.Text = String.Format("{0:dd/MM/yyyy}", d_transaction_date);
                }
                catch (Exception ex) { }

                // TransmissionDate

                try
                {
                    DateTime d_transmission_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transmission_date));

                    tc_transmission_date.Text = String.Format("{0:dd/MM/yyyy}", d_transmission_date);
                }
                catch (Exception ex) { }

                // PaymentDate

                try
                {
                    DateTime d_paymen_date = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date));

                    tc_paymen_date.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date);
                }
                catch (Exception ex) { }

                int i_net_payment_amount = lst_field_priority.IndexOf("NetPaymentAmount");
                int i_absorption_date = lst_field_priority.IndexOf("AbsorptionDate");
                int i_paymen_date_actual = lst_field_priority.IndexOf("PaymentDateActual");
                int i_invoice_date = lst_field_priority.IndexOf("InvoiceDate");
                ////// date format for outside ////
                int i_clearinfCalculation_date = lst_field_priority.IndexOf("ClearinfCalculationDate");
                int i_discountCalculationDate_date = lst_field_priority.IndexOf("DiscountCalculationDate");
                int i_clubManagementFeeCalculation_date = lst_field_priority.IndexOf("ClubManagementFeeCalculationDate");
                int i_clubDiscountFeeCalculation_date = lst_field_priority.IndexOf("ClubDiscountFeeCalculationDate");

                TableCell tc_net_payment_amount = gv_row.Cells[i_net_payment_amount];
                TableCell tc_absorption_date = gv_row.Cells[i_absorption_date];
                TableCell tc_paymen_date_actual = gv_row.Cells[i_paymen_date_actual];
                TableCell tc_invoice_date = gv_row.Cells[i_invoice_date];
                ////// date format for outside ////
                TableCell tc_clearinfCalculation_date = gv_row.Cells[i_clearinfCalculation_date];
                TableCell tc_discountCalculationDate_date = gv_row.Cells[i_discountCalculationDate_date];
                TableCell tc_clubManagementFeeCalculation_date = gv_row.Cells[i_clubManagementFeeCalculation_date];
                TableCell tc_clubDiscountFeeCalculation_date = gv_row.Cells[i_clubDiscountFeeCalculation_date];

                // NetPaymentAmount
                double n_net_payment_amount = 0;
                double.TryParse(tc_net_payment_amount.Text, out n_net_payment_amount);

                tc_net_payment_amount.Text = String.Format("{0:n2}", n_net_payment_amount);

                // AbsorptionDate
                try
                {
                    DateTime d_absorption_date = Convert.ToDateTime(Get_Cell_Text(ref tc_absorption_date));
                    tc_absorption_date.Text = String.Format("{0:dd/MM/yyyy}", d_absorption_date);
                }
                catch (Exception ex) { }

                // PaymentDateActual
                try
                {
                    DateTime d_paymen_date_actual = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date_actual));
                    tc_paymen_date_actual.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date_actual);
                }
                catch (Exception ex) { }

                // InvoiceDate
                try
                {
                    DateTime d_invoice_date = Convert.ToDateTime(Get_Cell_Text(ref tc_invoice_date));
                    tc_invoice_date.Text = String.Format("{0:dd/MM/yyyy}", d_invoice_date);
                }
                catch (Exception ex) { }

                /// outside date format new columns ///////////////////////////
                // ClearinfCalculation_date
                try
                {
                    DateTime d_clearinfCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clearinfCalculation_date));
                    tc_clearinfCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clearinfCalculation_date);
                }
                catch (Exception ex) { }

                // DiscountCalculationDate
                try
                {
                    DateTime d_discountCalculationDate_date = Convert.ToDateTime(Get_Cell_Text(ref tc_discountCalculationDate_date));
                    tc_discountCalculationDate_date.Text = String.Format("{0:dd/MM/yyyy}", d_discountCalculationDate_date);
                }
                catch (Exception ex) { }

                // ClubManagementFeeCalculation
                try
                {
                    DateTime d_clubManagementFeeCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clubManagementFeeCalculation_date));
                    tc_clubManagementFeeCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clubManagementFeeCalculation_date);
                }
                catch (Exception ex) { }

                // ClubDiscountFeeCalculation
                try
                {
                    DateTime d_clubDiscountFeeCalculation_date = Convert.ToDateTime(Get_Cell_Text(ref tc_clubDiscountFeeCalculation_date));
                    tc_clubDiscountFeeCalculation_date.Text = String.Format("{0:dd/MM/yyyy}", d_clubDiscountFeeCalculation_date);
                }
                catch (Exception ex) { }

            }
            else
            {
                List<string> lst_select_outside = new List<string>();

                if (s_select_outside != "")
                {
                    lst_select_outside = s_select_outside.Split(',').ToList();
                }

                // bool b_inside = (s_table == "Inside");

                // == COMMON TableCell VARIABLES ==

                int i_select = lst_field_priority.IndexOf("IsSelected");

                int i_id = lst_field_priority.IndexOf("ID");
                int i_unique_id = lst_field_priority.IndexOf("UniqueID");
                int i_query_id = lst_field_priority.IndexOf("QueryID");
                int i_matching_id = lst_field_priority.IndexOf("MatchingID");
                int i_matching_action = lst_field_priority.IndexOf("MatchingActionName");
                int i_data_file_id = lst_field_priority.IndexOf("DataFileID");
                int i_duty_payment_number = lst_field_priority.IndexOf("DutyPaymentNumber");
                int i_transaction_gross_amount = lst_field_priority.IndexOf("TransactionGrossAmount");
                int i_duty_payment_amount = lst_field_priority.IndexOf("DutyPaymentAmount");
                int i_remaining_payments_amount = lst_field_priority.IndexOf("RemainingPaymentsAmount");
                int i_transaction_date = lst_field_priority.IndexOf("TransactionDate");
                int i_transmission_date = lst_field_priority.IndexOf("TransmissionDate");
                int i_paymen_date = lst_field_priority.IndexOf("PaymentDate");
                ///
                int i_IsAbroad = lst_field_priority.IndexOf("IsAbroad");
                ///
                bool b_select = false;
                TableCell tc_select = null;
                if (i_select != -1)
                {
                    tc_select = gv_row.Cells[i_select];
                    tc_select.CssClass = "bg-gray";

                    bool.TryParse(tc_select.Text, out b_select);
                }

                decimal n_id = 0;
                if (i_id != -1)
                {
                    TableCell tc_id = gv_row.Cells[i_id];
                    decimal.TryParse(tc_id.Text, out n_id);
                    if (n_id <= 0)
                    {
                        tc_id.Controls.Add(Get_Anchor_Error("ID not exists."));
                    }
                }

                if (i_unique_id != -1)
                {
                    TableCell tc_unique_id = gv_row.Cells[i_unique_id];
                    string s_unique_id = Get_Cell_Text(ref tc_unique_id);

                    if (Is_Guid(s_unique_id) == false) { s_unique_id = ""; }

                    if (s_unique_id != "")
                    {
                        HtmlAnchor lnk_unique_id = new HtmlAnchor();

                        lnk_unique_id.InnerHtml = "Source";
                        lnk_unique_id.HRef = "javascript: void(0);";
                        lnk_unique_id.Attributes.Add("onclick", "javascript: payment_click('" + s_unique_id + "', '" + s_table + "');");

                        tc_unique_id.Controls.Add(lnk_unique_id);
                    }
                }

                string s_query_id = string.Empty;
                if (i_query_id != -1)
                {
                    TableCell tc_query_id = gv_row.Cells[i_query_id];
                    s_query_id = Get_Cell_Text(ref tc_query_id);

                    if (Is_Guid(s_query_id) == false) { s_query_id = ""; }

                    if (s_query_id != "")
                    {
                        HtmlAnchor lnk_query_id = new HtmlAnchor();

                        lnk_query_id.InnerHtml = "Match";

                        if (s_query_id_href != "")
                        {
                            lnk_query_id.HRef = String.Format("{0}&qid={1}", s_query_id_href, s_query_id);
                        }
                        else
                        {
                            lnk_query_id.HRef = "javascript: void(0);";
                            lnk_query_id.Attributes.Add("onclick", "javascript: match_click('" + s_query_id + "');");
                        }

                        tc_query_id.Controls.Add(lnk_query_id);
                    }
                    else if (b_select == true)
                    {
                        tc_query_id.Controls.Add(Get_Anchor_Error("QueryID not exists."));
                    }
                }

                if (i_matching_id != -1)
                {
                    TableCell tc_matching_id = gv_row.Cells[i_matching_id];
                    int n_matching_id = 0;
                    int.TryParse(tc_matching_id.Text, out n_matching_id);

                    tc_matching_id.Text = (n_matching_id > 0) ? String.Format("#{0}", n_matching_id) : "";
                }

                if (i_matching_action != -1)
                {
                    TableCell tc_matching_action = gv_row.Cells[i_matching_action];
                    if (tc_matching_action.Text.Trim() == "Resplit")
                    {
                        gv_row.CssClass = "bg-gray";
                        gv_row.Attributes.Add("data-class", gv_row.CssClass);
                    }
                }

                if (i_data_file_id != -1)
                {
                    TableCell tc_data_file_id = gv_row.Cells[i_data_file_id];
                    if (tc_data_file_id != null)
                    {
                        int n_data_file_id = 0;
                        int.TryParse(tc_data_file_id.Text, out n_data_file_id);

                        tc_data_file_id.Text = (n_data_file_id > 0) ? String.Format("#{0}", n_data_file_id) : "";
                    }
                }

                int n_duty_payment_number = 0;
                if (i_duty_payment_number != -1)
                {
                    TableCell tr_duty_payment_number = gv_row.Cells[i_duty_payment_number];
                    int.TryParse(tr_duty_payment_number.Text, out n_duty_payment_number);

                    if (n_duty_payment_number <= 0)
                    {
                        tr_duty_payment_number.Controls.Add(Get_Anchor_Error("DutyPaymentNumber not exists."));
                    }
                }
                
                ///

                TableCell tc_IsAbroad = null;
                bool b_IsAbroad = false;
                if (i_IsAbroad != -1)
                {
                    tc_IsAbroad = gv_row.Cells[i_IsAbroad];
                    bool.TryParse(tc_IsAbroad.Text, out b_IsAbroad);
                }
            
                // DutyPaymentNumber

                // CHECK-BOX 

                string s_checkbox_value = "";

                switch (s_mode)
                {
                    case "payment":
                        s_checkbox_value = n_duty_payment_number.ToString();
                        break;
                    case "matching":
                        s_checkbox_value = s_query_id;
                        break;
                    default:
                        s_checkbox_value = n_id.ToString();
                        break;
                }

                bool b_checked = false, b_disabled = false;

                switch (s_mode)
                {
                    case "all":
                        b_checked = b_select;
                        b_disabled = true;
                        break;
                    case "payment":
                        b_checked = false;
                        b_disabled = false;
                        //b_disabled = (b_inside == false || lst_select_inside.Count > 0);
                        break;
                    case "match":
                        b_checked = (lst_select_outside.Contains(s_checkbox_value) == false);
                        b_disabled = lst_select_outside.Count > 0;
                        break;
                    case "matching":
                        List<string> lst_query_id = lst_select_outside.ToList();   // GET DISTINCT VALUES OF lst_inside_query_id & lst_outside_query_id
                        b_checked = (lst_query_id.Contains(s_checkbox_value) == false);
                        b_disabled = (lst_query_id.Count > 0);
                        if (IsChkAllCheckBox == true)
                        {
                            b_disabled = true;
                            b_checked = false;
                        }
                        break;
                    case "not-matching":
                        b_checked = (lst_select_outside.Contains(s_checkbox_value) == true);
                        b_disabled = (lst_select_outside.Count > 0);
                        if (IsChkAllCheckBox == true)
                            b_disabled = true;
                        break;
                }

                if (tc_select != null)
                {
                    HtmlInputCheckBox chk_select = Get_CheckBox_Select(s_checkbox_value, s_table, false, b_checked, b_disabled);
                    tc_select.Controls.Add(chk_select);
                }
                ///// IsAbroad
                if (tc_IsAbroad != null)
                {
                    HtmlInputCheckBox chk_IsAbroad = Get_CheckBox_Select("", s_table, false, b_IsAbroad, true);
                    tc_IsAbroad.Controls.Add(chk_IsAbroad);
                }
                ////
                // DataFileID
              
                // TransactionGrossAmount

                if (i_transaction_gross_amount != -1)
                {
                    TableCell tc_transaction_gross_amount = gv_row.Cells[i_transaction_gross_amount];
                    double n_transaction_gross_amount = 0;
                    double.TryParse(tc_transaction_gross_amount.Text, out n_transaction_gross_amount);

                    tc_transaction_gross_amount.Text = String.Format("{0:n2}", n_transaction_gross_amount);
                }
                // DutyPaymentAmount

                if (i_duty_payment_amount != -1)
                {
                    TableCell tc_duty_payment_amount = gv_row.Cells[i_duty_payment_amount];
                    double n_duty_payment_amount = 0;
                    double.TryParse(tc_duty_payment_amount.Text, out n_duty_payment_amount);

                    tc_duty_payment_amount.Text = String.Format("{0:n2}", n_duty_payment_amount);
                }
                // RemainingPaymentsAmount

                if (i_remaining_payments_amount != -1)
                {
                    TableCell tc_remaining_payments_amount = gv_row.Cells[i_remaining_payments_amount];
                    double n_remaining_payments_amount = 0;
                    double.TryParse(tc_remaining_payments_amount.Text, out n_remaining_payments_amount);

                    tc_remaining_payments_amount.Text = String.Format("{0:n2}", n_remaining_payments_amount);
                }
                // TransactionDate

                if (i_transaction_date != -1)
                {
                    try
                    {
                        TableCell tc_transaction_date = gv_row.Cells[i_transaction_date];
                        if (tc_transaction_date.Text.Length > 7)
                        {
                            DateTime d_transaction_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transaction_date));
                            tc_transaction_date.Text = String.Format("{0:dd/MM/yyyy}", d_transaction_date);
                        }
                    }
                    catch (Exception ex) { }
                }

                // TransmissionDate

                if (i_transmission_date != -1)
                {
                    try
                    {
                        TableCell tc_transmission_date = gv_row.Cells[i_transmission_date];
                        if (tc_transmission_date.Text.Length > 7)
                        {
                            DateTime d_transmission_date = Convert.ToDateTime(Get_Cell_Text(ref tc_transmission_date));
                            tc_transmission_date.Text = String.Format("{0:dd/MM/yyyy}", d_transmission_date);
                        }
                    }
                    catch (Exception ex) { }
                }
                // PaymentDate

                if (i_paymen_date != -1)
                {
                    try
                    {
                        TableCell tc_paymen_date = gv_row.Cells[i_paymen_date];
                        if (tc_paymen_date.Text.Length > 7)
                        {
                            DateTime d_paymen_date = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date));
                            tc_paymen_date.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date);
                        }
                    }
                    catch (Exception ex) { }
                }

                int i_net_payment_amount = lst_field_priority.IndexOf("netPaymentAmount");
                int i_absorption_date = lst_field_priority.IndexOf("AbsorptionDate");
                int i_paymen_date_actual = lst_field_priority.IndexOf("PaymentDateActual");
                int i_invoice_date = lst_field_priority.IndexOf("InvoiceDate");

                if (i_net_payment_amount != -1)
                {
                    TableCell tc_net_payment_amount = gv_row.Cells[i_net_payment_amount];
                    double n_net_payment_amount = 0;
                    double.TryParse(tc_net_payment_amount.Text, out n_net_payment_amount);
                    tc_net_payment_amount.Text = String.Format("{0:n2}", n_net_payment_amount);
                }

                // NetPaymentAmount

                // AbsorptionDate
                if (i_absorption_date != -1)
                {
                    try
                    {
                        TableCell tc_absorption_date = gv_row.Cells[i_absorption_date];
                        DateTime d_absorption_date = Convert.ToDateTime(Get_Cell_Text(ref tc_absorption_date));
                        tc_absorption_date.Text = String.Format("{0:dd/MM/yyyy}", d_absorption_date);
                    }
                    catch (Exception ex) { }
                }

                // PaymentDateActual
                if (i_paymen_date_actual != -1)
                {
                    try
                    {
                        TableCell tc_paymen_date_actual = gv_row.Cells[i_paymen_date_actual];
                        DateTime d_paymen_date_actual = Convert.ToDateTime(Get_Cell_Text(ref tc_paymen_date_actual));
                        tc_paymen_date_actual.Text = String.Format("{0:dd/MM/yyyy}", d_paymen_date_actual);
                    }
                    catch (Exception ex) { }
                }

                // InvoiceDate

                if (i_invoice_date != -1)
                {
                    try
                    {
                        TableCell tc_invoice_date = gv_row.Cells[i_invoice_date];
                        DateTime d_invoice_date = Convert.ToDateTime(Get_Cell_Text(ref tc_invoice_date));
                        tc_invoice_date.Text = String.Format("{0:dd/MM/yyyy}", d_invoice_date);
                    }
                    catch (Exception ex) { }
                }
            }
        }

        // BIND ROW - SOURCE ( ARCHIVE ) TABLE

        public static void Bind_Grid_Source(GridViewRow gv_row, string s_table)
        {
            // IsSelected :     index = 0
            // UniqueID :       index = 1
            // ErrorMessage :   index = LAST

            TableCell tc_select = gv_row.Cells[0];
            TableCell tc_unique_id = gv_row.Cells[1];
            TableCell tc_error = gv_row.Cells[gv_row.Cells.Count - 1];

            if (gv_row.RowType == DataControlRowType.Header)
            {
                gv_row.CssClass = "bg-gray";

                tc_select.Controls.Add(Get_CheckBox_Select("0", "Source", true, false, true));

                tc_unique_id.Text = "Payment";

                tc_error.Text = "Error";
            }

            if (gv_row.RowType == DataControlRowType.DataRow)
            {
                // UniqueID

                string s_unique_id = Get_Cell_Text(ref tc_unique_id);

                if (Is_Guid(s_unique_id) == true)
                {
                    HtmlAnchor lnk_unique_id = new HtmlAnchor();

                    lnk_unique_id.InnerHtml = "Payment";
                    lnk_unique_id.HRef = "javascript: void(0);";
                    lnk_unique_id.Attributes.Add("onclick", "javascript: payment_click('" + s_unique_id + "', '" + s_table + "');");

                    tc_unique_id.Controls.Add(lnk_unique_id);
                }
                else
                {
                    tc_unique_id.Controls.Add(Get_Anchor_Error("UniqueID not exists."));
                }

                // IsSelected

                tc_select.Controls.Add(Get_CheckBox_Select(s_unique_id, "Source", false, false, true));

                // ErrorMessage

                string s_error = Get_Cell_Text(ref tc_error);

                if (s_error != "")
                {
                    tc_error.Controls.Add(Get_Anchor_Error(s_error));
                }
            }
        }

        public static string Get_Cell_Text(ref TableCell o_cell)
        {
            string s_text = o_cell.Text.Trim().Replace("&nbsp;", "").Replace("&#160;", "");

            o_cell.Text = s_text;

            return s_text;
        }

        private static HtmlInputCheckBox Get_CheckBox_Select(string s_value, string s_table, bool b_select_all, bool b_checked, bool b_disabled)
        {
            HtmlInputCheckBox o_checkbox = new HtmlInputCheckBox();

            string s_class = String.Format("checkbox-{0}", s_table.ToLower());

            o_checkbox.Attributes.Add("value", s_value);
            o_checkbox.Attributes.Add("class", s_class);

            if (b_select_all == true)
            {
                o_checkbox.Attributes.Add("onclick", "javascript: select_data_item_all('" + s_class + "', this);");
                //o_checkbox.Checked = true;
            }
            else
            {
                o_checkbox.Attributes.Add("onclick", "javascript: select_data_item('" + s_class + "', this);");
                //o_checkbox.Checked = b_checked;
            }

            o_checkbox.Checked = b_checked;
            o_checkbox.Disabled = b_disabled;

            return o_checkbox;
        }

        private static HtmlAnchor Get_Anchor_Error(string s_error)
        {
            HtmlAnchor o_anchor = new HtmlAnchor();

            o_anchor.InnerHtml = "???";
            o_anchor.Style.Add("color", "red");
            o_anchor.HRef = "javascript: void(0);";
            o_anchor.Attributes.Add("onclick", "javascript: alert('" + s_error + "');");

            return o_anchor;
        }

        private static bool Is_Guid(string s_guid)
        {
            Guid o_guid;
            return Guid.TryParse(s_guid, out o_guid);
        }
    }
}
