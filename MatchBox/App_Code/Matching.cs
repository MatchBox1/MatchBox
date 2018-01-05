using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MatchBox
{
    public class MatchingModel
    {
        public static string InterfaceName
        {
            get { return "MATCHING"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int StrategyID { get; set; }
        public int MatchingActionID { get; set; }

        public int AllInsideCount { get; set; }
        public int AllOutsideCount { get; set; }
        public double AllInsideAmount { get; set; }
        public double AllOutsideAmount { get; set; }

        public int MatchingInsideCount { get; set; }
        public int MatchingOutsideCount { get; set; }
        public double MatchingInsideAmount { get; set; }
        public double MatchingOutsideAmount { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public DataTable TableMatchingSummary { get; set; }
        public DataTable TableDataField { get; set; }

        public string ErrorMessage { get; set; }

        public MatchingModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            StrategyID = 0;
            MatchingActionID = 0;

            AllInsideCount = 0;
            AllOutsideCount = 0;
            AllInsideAmount = 0;
            AllOutsideAmount = 0;

            MatchingInsideCount = 0;
            MatchingOutsideCount = 0;
            MatchingInsideAmount = 0;
            MatchingOutsideAmount = 0;

            TableMatchingSummary = new DataTable();
            TableDataField = new DataTable();

            ErrorMessage = "";
        }
    }

    public class MatchingSearchModel
    {
        public int UserID { get; set; }

        // INPUT TABLES

        public DateTime? TransactionDateFrom { get; set; }
        public DateTime? TransactionDateTo { get; set; }
        public DateTime? PaymentDateFrom { get; set; }
        public DateTime? PaymentDateTo { get; set; }

        public bool IsEmptyPaymentDate { get; set; }

        public DataTable TableCompany { get; set; }
        public DataTable TableNetwork { get; set; }
        public DataTable TableBranch { get; set; }
        public DataTable TableCashbox { get; set; }

        public DataTable TableSupplierGroup { get; set; }
        public DataTable TableSupplier { get; set; }
        public DataTable TableTerminal { get; set; }

        public DataTable TableCredit { get; set; }
        public DataTable TableCard { get; set; }

        // OUTPUT TABLES

        public DataTable TableInsideSum { get; set; }
        public DataTable TableOutsideSum { get; set; }
        public DataTable TableInsideSumTotal { get; set; }
        public DataTable TableOutsideSumTotal { get; set; }

        public string ErrorMessage { get; set; }

        public MatchingSearchModel()
        {
            Reset();
        }

        public void Reset()
        {
            UserID = 0;

            TransactionDateFrom = null;
            TransactionDateTo = null;
            PaymentDateFrom = null;
            PaymentDateTo = null;

            IsEmptyPaymentDate = false;

            TableCompany = new DataTable();
            TableNetwork = new DataTable();
            TableBranch = new DataTable();
            TableCashbox = new DataTable();

            TableSupplierGroup = new DataTable();
            TableSupplier = new DataTable();
            TableTerminal = new DataTable();

            TableCredit = new DataTable();
            TableCard = new DataTable();

            TableCompany.Columns.Add("ID");
            TableNetwork.Columns.Add("ID");
            TableBranch.Columns.Add("ID");
            TableCashbox.Columns.Add("ID");

            TableSupplierGroup.Columns.Add("ID");
            TableSupplier.Columns.Add("ID");
            TableTerminal.Columns.Add("ID");

            TableCredit.Columns.Add("ID");
            TableCard.Columns.Add("ID");

            ErrorMessage = "";
        }
    }

    public class MatchingBalanceModel
    {
        public int CompanyID { get; set; }
        public int OperationTypeID { get; set; }
        public double DutyPaymentAmount { get; set; }
        public string QueryID { get; set; }

        public MatchingBalanceModel()
        {
            Reset();
        }

        public void Reset()
        {
            CompanyID = 0;
            OperationTypeID = 0;
            DutyPaymentAmount = 0;
            QueryID = "";
        }
    }

    public class MatchingAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprMatchingSelectList", DB.Get_Connection());
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
                        case "StrategyID":
                        case "MatchingActionID":
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

        public static void Select(ref MatchingModel o_matching)
        {
            if (o_matching.ID == 0) { return; }

            DataTable dt_srategy_field = Common.Convert_To_Table(StrategyModel.List_Strategy_Field());

            SqlCommand o_command = new SqlCommand("sprMatchingSelect", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_matching.ID });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_matching.UserID });

            o_command.Parameters.AddWithValue("@TableListStrategyField", dt_srategy_field);
            o_command.Parameters["@TableListStrategyField"].SqlDbType = SqlDbType.Structured;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                DataRow dr_matching = o_data_set.Tables[0].Rows[0];

                o_matching.StrategyID = (int)dr_matching["StrategyID"];
                o_matching.MatchingActionID = (int)dr_matching["MatchingActionID"];

                o_matching.AllInsideCount = (int)dr_matching["AllInsideCount"];
                o_matching.AllOutsideCount = (int)dr_matching["AllOutsideCount"];
                o_matching.AllInsideAmount = (double)dr_matching["AllInsideAmount"];
                o_matching.AllOutsideAmount = (double)dr_matching["AllOutsideAmount"];

                o_matching.MatchingInsideCount = (int)dr_matching["MatchingInsideCount"];
                o_matching.MatchingOutsideCount = (int)dr_matching["MatchingOutsideCount"];
                o_matching.MatchingInsideAmount = (double)dr_matching["MatchingInsideAmount"];
                o_matching.MatchingOutsideAmount = (double)dr_matching["MatchingOutsideAmount"];

                o_matching.DateStart = (DateTime)dr_matching["DateStart"];
                o_matching.DateEnd = (DateTime)dr_matching["DateEnd"];

                if (o_data_set.Tables.Count > 1)
                {
                    o_matching.TableMatchingSummary = o_data_set.Tables[1];
                    o_matching.TableDataField = o_data_set.Tables[2];
                }
            }
            catch (Exception ex)
            {
                o_matching.ErrorMessage = ex.Message;
                o_matching.ErrorMessage = "Error on select matching item.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Search(ref MatchingSearchModel o_matching_search)
        {
            SqlCommand o_command = new SqlCommand("sprMatchingSearch", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_matching_search.UserID;

            if (o_matching_search.TransactionDateFrom != null)
            {
                o_command.Parameters.Add("@TransactionDateFrom", SqlDbType.Date);
                o_command.Parameters["@TransactionDateFrom"].Value = o_matching_search.TransactionDateFrom;
            }

            if (o_matching_search.TransactionDateTo != null)
            {
                o_command.Parameters.Add("@TransactionDateTo", SqlDbType.Date);
                o_command.Parameters["@TransactionDateTo"].Value = o_matching_search.TransactionDateTo;
            }

            if (o_matching_search.PaymentDateFrom != null)
            {
                o_command.Parameters.Add("@PaymentDateFrom", SqlDbType.Date);
                o_command.Parameters["@PaymentDateFrom"].Value = o_matching_search.PaymentDateFrom;
            }

            if (o_matching_search.PaymentDateTo != null)
            {
                o_command.Parameters.Add("@PaymentDateTo", SqlDbType.Date);
                o_command.Parameters["@PaymentDateTo"].Value = o_matching_search.PaymentDateTo;
            }

            o_command.Parameters.Add("@IsEmptyPaymentDate", SqlDbType.Bit);
            o_command.Parameters["@IsEmptyPaymentDate"].Value = o_matching_search.IsEmptyPaymentDate;

            o_command.Parameters.AddWithValue("@TableCompany", o_matching_search.TableCompany);
            o_command.Parameters["@TableCompany"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableNetwork", o_matching_search.TableNetwork);
            o_command.Parameters["@TableNetwork"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableBranch", o_matching_search.TableBranch);
            o_command.Parameters["@TableBranch"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableCashbox", o_matching_search.TableCashbox);
            o_command.Parameters["@TableCashbox"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableSupplierGroup", o_matching_search.TableSupplierGroup);
            o_command.Parameters["@TableSupplierGroup"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableSupplier", o_matching_search.TableSupplier);
            o_command.Parameters["@TableSupplier"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableTerminal", o_matching_search.TableTerminal);
            o_command.Parameters["@TableTerminal"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableCredit", o_matching_search.TableCredit);
            o_command.Parameters["@TableCredit"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableCard", o_matching_search.TableCard);
            o_command.Parameters["@TableCard"].SqlDbType = SqlDbType.Structured;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                o_matching_search.TableInsideSum = o_data_set.Tables[0];
                o_matching_search.TableOutsideSum = o_data_set.Tables[1];
                o_matching_search.TableInsideSumTotal = o_data_set.Tables[2];
                o_matching_search.TableOutsideSumTotal = o_data_set.Tables[3];
            }
            catch (Exception ex)
            {
                o_matching_search.ErrorMessage = ex.Message;
                o_matching_search.ErrorMessage = "Error on select matching search.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Data(int n_user_id, string s_where_inside, string s_where_outside, bool b_many_to_many, ref DataTable dt_inside, ref DataTable dt_outside, ref DataTable dt_inside_sum, ref DataTable dt_outside_sum, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprMatchingData", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });

            o_command.Parameters.Add(new SqlParameter("@WhereInside", SqlDbType.NVarChar, -1) { Value = s_where_inside });
            o_command.Parameters.Add(new SqlParameter("@WhereOutside", SqlDbType.NVarChar, -1) { Value = s_where_outside });

            if (b_many_to_many == true)
            {
                o_command.Parameters.Add(new SqlParameter("@IsManyToMany", SqlDbType.Bit) { Value = true });
            }

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_inside = o_data_set.Tables[0];
                dt_outside = o_data_set.Tables[1];

                if (b_many_to_many == false)
                {
                    dt_inside_sum = o_data_set.Tables[2];
                    dt_outside_sum = o_data_set.Tables[3];
                }
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select matching data.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }

            if (dt_inside.Rows.Count == 0 || dt_outside.Rows.Count == 0)
            {
                s_error = "Can't create matching without data in both sides (inside & outside).";
            }
        }

        public static void Select_Data(MatchingSearchModel o_matching_search, bool b_many_to_many, ref DataTable dt_inside, ref DataTable dt_outside, ref DataTable dt_inside_sum, ref DataTable dt_outside_sum, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprMatchingData_1", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_matching_search.UserID;

            if (o_matching_search.TransactionDateFrom != null)
            {
                o_command.Parameters.Add("@TransactionDateFrom", SqlDbType.Date);
                o_command.Parameters["@TransactionDateFrom"].Value = o_matching_search.TransactionDateFrom;
            }

            if (o_matching_search.TransactionDateTo != null)
            {
                o_command.Parameters.Add("@TransactionDateTo", SqlDbType.Date);
                o_command.Parameters["@TransactionDateTo"].Value = o_matching_search.TransactionDateTo;
            }

            if (o_matching_search.PaymentDateFrom != null)
            {
                o_command.Parameters.Add("@PaymentDateFrom", SqlDbType.Date);
                o_command.Parameters["@PaymentDateFrom"].Value = o_matching_search.PaymentDateFrom;
            }

            if (o_matching_search.PaymentDateTo != null)
            {
                o_command.Parameters.Add("@PaymentDateTo", SqlDbType.Date);
                o_command.Parameters["@PaymentDateTo"].Value = o_matching_search.PaymentDateTo;
            }

            o_command.Parameters.Add("@IsEmptyPaymentDate", SqlDbType.Bit);
            o_command.Parameters["@IsEmptyPaymentDate"].Value = o_matching_search.IsEmptyPaymentDate;

            if (b_many_to_many == true)
            {
                o_command.Parameters.Add("@IsManyToMany", SqlDbType.Bit);
                o_command.Parameters["@IsManyToMany"].Value = true;
            }

            o_command.Parameters.AddWithValue("@TableCompany", o_matching_search.TableCompany);
            o_command.Parameters["@TableCompany"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableNetwork", o_matching_search.TableNetwork);
            o_command.Parameters["@TableNetwork"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableBranch", o_matching_search.TableBranch);
            o_command.Parameters["@TableBranch"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableCashbox", o_matching_search.TableCashbox);
            o_command.Parameters["@TableCashbox"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableSupplierGroup", o_matching_search.TableSupplierGroup);
            o_command.Parameters["@TableSupplierGroup"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableSupplier", o_matching_search.TableSupplier);
            o_command.Parameters["@TableSupplier"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableTerminal", o_matching_search.TableTerminal);
            o_command.Parameters["@TableTerminal"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableCredit", o_matching_search.TableCredit);
            o_command.Parameters["@TableCredit"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableCard", o_matching_search.TableCard);
            o_command.Parameters["@TableCard"].SqlDbType = SqlDbType.Structured;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_inside = o_data_set.Tables[0];
                dt_outside = o_data_set.Tables[1];

                if (b_many_to_many == false)
                {
                    dt_inside_sum = o_data_set.Tables[2];
                    dt_outside_sum = o_data_set.Tables[3];
                }
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select matching data.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }

            if (dt_inside.Rows.Count == 0 || dt_outside.Rows.Count == 0)
            {
                s_error = "Can't create matching without data in both sides (inside & outside).";
            }
        }

        public static DataTable Table_Matching_Field(DataTable dt_inside, DataTable dt_outside, DataTable dt_strategy_field, ref List<string> lst_matching_field, ref int n_company_id, ref string s_error)
        {
            // WHEN AUTO MATCHING USE STRATEGY, PASSED dt_strategy_field AND RETURNED dt_matching_field CONTAINS 5 FIELDS : Priority / Tolerance_1 / Tolerance_2 / Tolerance_3 / StrategyField
            // WHEN AUTO MATCHING NOT USE STRATEGY, PASSED dt_strategy_field AND RETURNED dt_matching_field CONTAINS 2 FIELDS : StrategyField / Tolerance

            DataTable dt_matching_field = dt_strategy_field.Copy();

            lst_matching_field.Add("ID");   // ADD TO lst_matching_field ID

            // SEPARATELY CHECK IF CompanyID INCLUDED IN lst_matching_field, BECAUSE TableStrategyField NOT CONTAINS ROW WITH StrategyField = CompanyID
            // IF NO ERROR AND CompanyID NOT INCLUDED IN lst_matching_field, Table_Matching_Field FUNCTION RETURNS COMMON n_company_id FOR BOTH TABLES ( n_company_id USED WHEN BALANCE TRANSACTION WAS CREATED AND b_include_company_id IS FALSE )

            bool b_include_company_id = true;

            DataTable dt_inside_check = new DataView(dt_inside).ToTable(true, "CompanyID");
            DataTable dt_outside_check = new DataView(dt_outside).ToTable(true, "CompanyID");

            if (dt_inside_check.Rows.Count <= 1 || dt_outside_check.Rows.Count <= 1)
            {
                string s_inside_company_id = (dt_inside_check.Rows.Count == 1) ? dt_inside_check.Rows[0][0].ToString().Trim() : "";
                string s_outside_company_id = (dt_outside_check.Rows.Count == 1) ? dt_outside_check.Rows[0][0].ToString().Trim() : "";

                int n_inside_company_id = 0, n_outside_company_id = 0;

                int.TryParse(s_inside_company_id, out n_inside_company_id);
                int.TryParse(s_outside_company_id, out n_outside_company_id);

                string s_error_in_table = "";

                if (n_inside_company_id == 0 && n_outside_company_id == 0)
                {
                    s_error_in_table = "'Inside' and 'Otside' tables";
                }
                else if (n_inside_company_id == 0)
                {
                    s_error_in_table = "'Inside' table";
                }
                else if (n_outside_company_id == 0)
                {
                    s_error_in_table = "'Otside' table";
                }

                if (s_error_in_table != "")
                {
                    s_error = String.Format("Error: 'Company' not exists in {0}. ", s_error_in_table);
                    goto Finish;
                }

                if (n_inside_company_id == n_outside_company_id)
                {
                    n_company_id = n_inside_company_id;
                    b_include_company_id = false;
                }
                else
                {
                    s_error = "Error: Transactions in 'Inside' and 'Outside' tables are associated with two different Companies.";
                    goto Finish;
                }
            }

            if (b_include_company_id == true)
            {
                lst_matching_field.Add("CompanyID");
            }

            // CHECK IF INCLUDE IN lst_matching_field & dt_matching_field, STRATEGY FIELDS FROM TableStrategyField.

            foreach (DataRow dr_strategy_field in dt_strategy_field.Rows)
            {
                string s_strategy_field = dr_strategy_field["StrategyField"].ToString();

                List<string> lst_field = new List<string>();

                switch (s_strategy_field)
                {
                    case "TerminalNumber":
                        lst_field.Add("TerminalID");
                        break;
                    case "SupplierNumber":
                        lst_field.Add("SupplierID");
                        break;
                    case "CreditBrand":
                        lst_field.Add("CreditID");
                        break;
                    case "CardBrand":
                        lst_field.Add("CardID");
                        break;
                    case "TransactionCurrency":
                        lst_field.Add("TransactionCurrencyID");
                        break;
                    case "CardPrefix":
                    case "CardNumber":
                        // CardPrefix EXISTS ONLY WHEN AUTO MATCHING NOT USE STRATEGY
                        if (lst_field.Contains("CardPrefix") == false) { lst_field.Add("CardPrefix"); }
                        if (lst_field.Contains("CardNumber") == false) { lst_field.Add("CardNumber"); }
                        break;
                    default:
                        lst_field.Add(s_strategy_field);
                        break;
                }

                foreach (string s_field in lst_field)
                {
                    bool b_exclude = false;

                    dt_inside_check = new DataView(dt_inside).ToTable(true, s_field);
                    dt_outside_check = new DataView(dt_outside).ToTable(true, s_field);

                    if (dt_inside_check.Rows.Count == dt_outside_check.Rows.Count && dt_inside_check.Rows.Count <= 1 && dt_outside_check.Rows.Count <= 1)
                    {
                        switch (dt_inside_check.Rows.Count)
                        {
                            case 0:
                                b_exclude = true;
                                break;
                            case 1:
                                string s_inside_value = dt_inside_check.Rows[0][0].ToString().Trim();
                                string s_outside_value = dt_outside_check.Rows[0][0].ToString().Trim();

                                b_exclude = (s_inside_value == s_outside_value);

                                break;
                        }
                    }

                    if (b_exclude == false)
                    {
                        if (lst_matching_field.Contains(s_field) == false) { lst_matching_field.Add(s_field); }
                    }
                    else
                    {
                        DataRow dr_matching_field = dt_matching_field.Select(" StrategyField = '" + s_strategy_field + "' ").FirstOrDefault();

                        if (dr_matching_field != null)
                        {
                            dt_matching_field.Rows.Remove(dr_matching_field);
                        }
                    }
                }
            }

            lst_matching_field.Add("QueryID");  // ADD TO lst_matching_field QueryID

        Finish:

            return dt_matching_field;
        }

        public static void Update(ref MatchingModel o_matching, DataTable dt_matching_query, DataTable dt_matching_data, DataTable dt_matching_balance)
        {
            string s_procedure = "";

            if (o_matching.ID == 0) { s_procedure = "sprMatchingInsert"; } else { s_procedure = "sprMatchingUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            if (o_matching.ID > 0)
            {
                o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_matching.ID });
            }

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_matching.UserID });

            if (o_matching.ID == 0)
            {
                o_command.Parameters.Add(new SqlParameter("@StrategyID", SqlDbType.Int) { Value = o_matching.StrategyID });

                o_command.Parameters.Add(new SqlParameter("@AllInsideCount", SqlDbType.Int) { Value = o_matching.AllInsideCount });
                o_command.Parameters.Add(new SqlParameter("@AllOutsideCount", SqlDbType.Int) { Value = o_matching.AllOutsideCount });

                o_command.Parameters.Add(new SqlParameter("@AllInsideAmount", SqlDbType.Float) { Value = o_matching.AllInsideAmount });
                o_command.Parameters.Add(new SqlParameter("@AllOutsideAmount", SqlDbType.Float) { Value = o_matching.AllOutsideAmount });
            }

            o_command.Parameters.AddWithValue("@TableMatchingQuery", dt_matching_query);
            o_command.Parameters["@TableMatchingQuery"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableMatchingData", dt_matching_data);
            o_command.Parameters["@TableMatchingData"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableMatchingBalance", dt_matching_balance);
            o_command.Parameters["@TableMatchingBalance"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                if (o_matching.ID == 0)
                {
                    o_matching.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_matching.ErrorMessage = ex.Message;
                o_matching.ErrorMessage = "Error on update matching.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static int Update_Quick(int n_user_id, string s_where_inside, string s_where_outside, DataTable dt_query, ref string s_error)
        {
            int n_matching_id = 0;

            // "AND QueryID IS NULL" IS ICLUDED IN sprMatchingInsertQuick !!!

            //s_where_inside = s_where_inside.Replace("AND QueryID IS NULL", "");
            //s_where_outside = s_where_inside.Replace("AND QueryID IS NULL", "");

            SqlCommand o_command = new SqlCommand("sprMatchingInsertQuick", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@WhereInside", SqlDbType.NVarChar, -1) { Value = s_where_inside });
            o_command.Parameters.Add(new SqlParameter("@WhereOutside", SqlDbType.NVarChar, -1) { Value = s_where_outside });

            o_command.Parameters.AddWithValue("@TableQuery", dt_query);
            o_command.Parameters["@TableQuery"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    n_matching_id = (int)o_reader["MatchingID"];
                    s_error = o_reader["Error"].ToString();
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update auto matching.";
                goto Finish;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

        Finish:

            return n_matching_id;
        }

        public static int Delete_Query(int n_user_id, int n_matching_id, string s_query_number_array, ref string s_error)
        {
            int n_rows_affected = 0;

            if (n_user_id <= 0 || n_matching_id <= 0 || s_query_number_array == "") { goto Finish; }

            // CREATE dt_query_number - TYPE OF typSingleColumnInt

            DataTable dt_query_number = Common.Convert_To_Table(s_query_number_array, "int");

            // EXECUTE QUERY

            SqlCommand o_command = new SqlCommand("sprMatchingDeleteQuery", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@MatchingID", SqlDbType.Int) { Value = n_matching_id });

            o_command.Parameters.AddWithValue("@TableQueryNumber", dt_query_number);
            o_command.Parameters["@TableQueryNumber"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete matchings query by QueryNumber.";
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
    }
}
