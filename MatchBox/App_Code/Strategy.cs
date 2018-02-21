using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MatchBox
{
    public class StrategyModel
    {
        public static string InterfaceName
        {
            get { return "STRATEGY"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int StatusID { get; set; }

        public double QueryPercent { get; set; }

        public string StrategyName { get; set; }
        public string StatusName { get; set; }
        public string ErrorMessage { get; set; }

        public DataTable TableStrategyTemplate { get; set; }
        public DataTable TableStrategyField { get; set; }

        public DataTable TableTemplateField { get; set; }
        public DataTable TableDataField { get; set; }

        public const int MAX_PRIORITY = 5;
        public const int MAX_TOLERANCE = 6;

        public static List<string> List_Strategy_Field()
        {
            List<string> lst_strategy_field = new List<string>();

            // ALL AVAILABLE FIELDS FOR STRATEGY

            lst_strategy_field.Add("TerminalNumber");             // TerminalID               // Priority //
            lst_strategy_field.Add("SupplierNumber");             // SupplierID               // Priority //
            lst_strategy_field.Add("CreditBrand");                // CreditID                 // Priority //
            lst_strategy_field.Add("CardBrand");                  // CardID                   // Priority //
            lst_strategy_field.Add("TransactionCurrency");        // TransactionCurrencyID    // Const    //
            lst_strategy_field.Add("CardNumber");                 // CardNumber + CardPrefix  // Const + Priority //
            lst_strategy_field.Add("PaymentsCount");              // PaymentsCount            // Priority //
            lst_strategy_field.Add("DutyPaymentNumber");          // DutyPaymentNumber        // Priority //
            lst_strategy_field.Add("TransmissionNumber");         // TransmissionNumber       // Priority //
            lst_strategy_field.Add("TransactionGrossAmount");     // TransactionGrossAmount   // Priority // Tolerance (Decimal - Money +/-)
            lst_strategy_field.Add("DutyPaymentAmount");          // DutyPaymentAmount        // Const    // Tolerance (Decimal - Money +/-)
            lst_strategy_field.Add("TransactionDate");            // TransactionDate          // Const    // Tolerance (Int - Days +/-)
            lst_strategy_field.Add("PaymentDate");                // PaymentDate              // Const    // Tolerance (Int - Days +/-)
            lst_strategy_field.Add("VoucherNumber");              // VoucherNumber            // Priority // Tolerance (Int - Chars count left)
            lst_strategy_field.Add("ConfirmationNumber");         // ConfirmationNumber       // Priority // Tolerance (Int - Chars count left)

            return lst_strategy_field;
        }

        public StrategyModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            StatusID = 0;

            QueryPercent = 0;

            StrategyName = "";
            StatusName = "";
            ErrorMessage = "";

            TableStrategyTemplate = new DataTable();            // TEMPLATES IN STRATEGY - LIST OF TEMPLATES THAT ALSO CONTAINS INDICATION ABOUT SELECTED TEMPLATES ( ID / TemplateName / DataSource / DateFromMin / DateToMax / IsSelected )
            TableStrategyField = new DataTable();               // FIELDS IN STRATEGY - LIST OF TEMPLATES FIELDS THAT EXISTS IN BOTH SIDES (INSIDE & OUTSIDE) AND INCLUDED IN STRATEGY ( Priority / Tolerance_1 / Tolerance_2 / Tolerance_3 / StrategyField )

            TableTemplateField = new DataTable();               // FIELDS IN TEMPLATE - LIST OF FIELDS IN EACH TEMPLATE - CONTAIN FIELDS ONLY FROM List_Strategy_Field ( TemplateID / FieldFromDB )
            TableDataField = new DataTable();                   // FIELDS NAMES TRANSLATION - CONTAIN FIELDS ONLY FROM List_Strategy_Field ( FieldName / FieldDescription )

            TableStrategyField.Columns.Add("Priority");
            TableStrategyField.Columns.Add("Tolerance_1");
            TableStrategyField.Columns.Add("Tolerance_2");
            TableStrategyField.Columns.Add("Tolerance_3");
            TableStrategyField.Columns.Add("StrategyField");
        }
    }

    public class StrategyAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprStrategySelectList", DB.Get_Connection());
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
                        case "StatusID":
                        case "PageNumber":
                        case "PageSize":
                            o_db_type = SqlDbType.Int;
                            break;
                        case "QueryPercent":
                            o_db_type = SqlDbType.Float;
                            break;
                        case "StrategyName":
                            o_db_type = SqlDbType.NVarChar;
                            n_size = 50;
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
                        case SqlDbType.Float:
                            o_command.Parameters.Add("@" + s_key, o_db_type);
                            o_command.Parameters["@" + s_key].Value = Convert.ToDouble(s_value);
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

        public static void Select(ref StrategyModel o_strategy)
        {
            if (o_strategy.ID == 0) { return; }

            DataTable dt_srategy_field = Common.Convert_To_Table(StrategyModel.List_Strategy_Field());

            SqlCommand o_command = new SqlCommand("sprStrategySelect", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_strategy.ID });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_strategy.UserID });

            o_command.Parameters.AddWithValue("@TableListStrategyField", dt_srategy_field);
            o_command.Parameters["@TableListStrategyField"].SqlDbType = SqlDbType.Structured;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                DataRow dr_strategy = o_data_set.Tables[0].Rows[0];

                o_strategy.StatusID = Convert.ToInt32(dr_strategy["StatusID"]);

                o_strategy.StrategyName = dr_strategy["StrategyName"].ToString();
                o_strategy.StatusName = dr_strategy["StatusName"].ToString();

                o_strategy.QueryPercent = Convert.ToDouble(dr_strategy["QueryPercent"]);

                o_strategy.TableStrategyTemplate = o_data_set.Tables[1];
                o_strategy.TableStrategyField = o_data_set.Tables[2];

                o_strategy.TableTemplateField = o_data_set.Tables[3];
                o_strategy.TableDataField = o_data_set.Tables[4];
            }
            catch (Exception ex)
            {
                o_strategy.ErrorMessage = ex.Message;
                o_strategy.ErrorMessage = "Error on select strategy.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_StopExecution(int n_strategy_id, int n_user_id, DataTable dt_strategy, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprStrategyStopExecution", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = n_strategy_id });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(dt_strategy);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select single strategy.";
                return;
            }
            finally
            {
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Single(int n_strategy_id, int n_user_id, ref DataTable dt_strategy, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprStrategySingle", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = n_strategy_id });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(dt_strategy);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select single strategy.";
                return;
            }
            finally
            {
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Template(ref StrategyModel o_strategy)
        {
            DataTable dt_srategy_field = Common.Convert_To_Table(StrategyModel.List_Strategy_Field());

            SqlCommand o_command = new SqlCommand("sprStrategyTemplate", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_strategy.UserID;

            o_command.Parameters.Add("@StrategyID", SqlDbType.Int);
            o_command.Parameters["@StrategyID"].Value = o_strategy.ID;

            o_command.Parameters.AddWithValue("@TableListStrategyField", dt_srategy_field);
            o_command.Parameters["@TableListStrategyField"].SqlDbType = SqlDbType.Structured;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                o_strategy.TableStrategyTemplate = o_data_set.Tables[0];
                o_strategy.TableTemplateField = o_data_set.Tables[2];
                o_strategy.TableDataField = o_data_set.Tables[3];
            }
            catch (Exception ex)
            {
                o_strategy.ErrorMessage = ex.Message;
                o_strategy.ErrorMessage = "Error on select strategy fields.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Data(int n_user_id, int n_strategy_id, ref DataTable dt_strategy, ref DataTable dt_data_file, ref DataTable dt_inside, ref DataTable dt_outside, ref DataTable dt_inside_total, ref DataTable dt_outside_total, ref DataTable dt_inside_data_file, ref DataTable dt_outside_data_file, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprStrategySelectDataSum", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@StrategyID", SqlDbType.Int) { Value = n_strategy_id });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_strategy = o_data_set.Tables[0];
                dt_data_file = o_data_set.Tables[1];
                dt_inside = o_data_set.Tables[2];
                dt_outside = o_data_set.Tables[3];
                dt_inside_total = o_data_set.Tables[4];
                dt_outside_total = o_data_set.Tables[5];
                dt_inside_data_file = o_data_set.Tables[6];
                dt_outside_data_file = o_data_set.Tables[7];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select strategy data.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Select_Query(DataTable dt_matching_field, bool b_add_card_prefix, bool b_gui, ref DataTable dt_query)
        {
            // TABLE dt_query CONTAINS ONE QUERY IN EACH ROW.
            // EACH CELL IN dt_query ROWS CAN CONTAINS 3 TYPE OF VALUES
            // 1. VALUE = EMPTY - FIELD NOT EXISTS IN QUERY
            // 2. VALUE = * - FIELD EXISTS IN QUERY WITHOUT TOLERANCE
            // 3. VALUE > 0 - FIELD EXISTS IN QUERY WITH [VALUE] TOLERANCE

            // IF b_add_card_prefix IS TRUE THEN ADD CardPrefix TO dt_matching_field IN LOWER PRIORITY. IN THIS CASE Select_Query FUNCTION CREATE ADITIONAL QUERY FOR CardNumber WITHOUT CardPrefix.

            if (b_add_card_prefix == true)
            {
                int n_priority_last = 0;
                int.TryParse(dt_matching_field.Rows[dt_matching_field.Rows.Count - 1]["Priority"].ToString(), out n_priority_last);

                DataRow dr_strategy_field = dt_matching_field.NewRow();

                dr_strategy_field["Priority"] = n_priority_last + 1;
                dr_strategy_field["Tolerance_1"] = 0;
                dr_strategy_field["Tolerance_2"] = 0;
                dr_strategy_field["Tolerance_3"] = 0;
                dr_strategy_field["StrategyField"] = "CardPrefix";

                dt_matching_field.Rows.Add(dr_strategy_field);
            }

            // ADD ROW NUMBER COLUMN

            dt_query.Columns.Add("QueryNumber");

            List<string> lst_strategy_field_const = new List<string>();
            List<string> lst_strategy_field_priority = new List<string>();

            DataTable dt_strategy_field_tolerance = new DataTable();

            foreach (DataRow dr_strategy_field in dt_matching_field.Rows)
            {
                string s_strategy_field = dr_strategy_field["StrategyField"].ToString();

                dt_query.Columns.Add(s_strategy_field);

                int n_priority = 0;
                int.TryParse(dr_strategy_field["Priority"].ToString(), out n_priority);

                if (n_priority > 0)
                {
                    lst_strategy_field_priority.Add(s_strategy_field);
                }
                else
                {
                    lst_strategy_field_const.Add(s_strategy_field);
                }

                float n_tolerance_1 = 0;
                float.TryParse(dr_strategy_field["Tolerance_1"].ToString(), out n_tolerance_1);

                float n_tolerance_2 = 0;
                float.TryParse(dr_strategy_field["Tolerance_2"].ToString(), out n_tolerance_2);

                float n_tolerance_3 = 0;
                float.TryParse(dr_strategy_field["Tolerance_3"].ToString(), out n_tolerance_3);

                if (n_tolerance_1 > 0 || n_tolerance_2 > 0 || n_tolerance_3 > 0)
                {
                    if (dt_strategy_field_tolerance.Columns.Contains(s_strategy_field) == false)
                    {
                        dt_strategy_field_tolerance.Columns.Add(s_strategy_field);
                    }

                    if (n_tolerance_1 > 0)
                    {
                        if (dt_strategy_field_tolerance.Rows.Count < 1)
                        {
                            DataRow o_data_row = dt_strategy_field_tolerance.NewRow();

                            o_data_row[s_strategy_field] = n_tolerance_1;

                            dt_strategy_field_tolerance.Rows.Add(o_data_row);
                        }
                        else
                        {
                            dt_strategy_field_tolerance.Rows[0][s_strategy_field] = n_tolerance_1;
                        }
                    }

                    if (n_tolerance_2 > 0)
                    {
                        if (dt_strategy_field_tolerance.Rows.Count < 2)
                        {
                            DataRow o_data_row = dt_strategy_field_tolerance.NewRow();

                            o_data_row[s_strategy_field] = n_tolerance_2;

                            dt_strategy_field_tolerance.Rows.Add(o_data_row);
                        }
                        else
                        {
                            dt_strategy_field_tolerance.Rows[1][s_strategy_field] = n_tolerance_2;
                        }
                    }

                    if (n_tolerance_3 > 0)
                    {
                        if (dt_strategy_field_tolerance.Rows.Count < 3)
                        {
                            DataRow o_data_row = dt_strategy_field_tolerance.NewRow();

                            o_data_row[s_strategy_field] = n_tolerance_3;

                            dt_strategy_field_tolerance.Rows.Add(o_data_row);
                        }
                        else
                        {
                            dt_strategy_field_tolerance.Rows[2][s_strategy_field] = n_tolerance_3;
                        }
                    }
                }
            }

            // ADD COMBINATIONS

            int i_row = 0;
            int n_size_item = lst_strategy_field_priority.Count;

            for (int n_size_temp = n_size_item; n_size_temp >= 1; n_size_temp--)
            {
                // ADD GENERAL COMBINATIONS

                List<List<string>> lst_combination_table = new List<List<string>>();

                Combination<string>.Bind_Combination(lst_strategy_field_priority.ToArray(), n_size_item, n_size_temp, ref lst_combination_table);

                foreach (List<string> lst_combination in lst_combination_table)
                {
                    Select_Query_Row(lst_strategy_field_const, lst_strategy_field_priority, lst_combination, dt_strategy_field_tolerance, b_gui, ref dt_query, ref i_row);
                }

                // ADD SEPARATOR ROW

                if (b_gui == true)
                {
                    DataRow dr_query_separator = dt_query.NewRow();

                    dr_query_separator["QueryNumber"] = "-";

                    dt_query.Rows.Add(dr_query_separator);
                }
            }

            Select_Query_Row(lst_strategy_field_const, lst_strategy_field_priority, new List<string>(), dt_strategy_field_tolerance, b_gui, ref dt_query, ref i_row);
        }

        private static void Select_Query_Row(List<string> lst_strategy_field_const, List<string> lst_strategy_field_priority, List<string> lst_combination, DataTable dt_strategy_field_tolerance, bool b_gui, ref DataTable dt_query, ref int i_row)
        {
            i_row++;

            DataRow dr_query = dt_query.NewRow();

            dr_query["QueryNumber"] = i_row;

            List<string> lst_tolerance = new List<string>();

            foreach (string s_field in lst_strategy_field_const)
            {
                dr_query[s_field] = "*";

                if (dt_strategy_field_tolerance.Columns.Contains(s_field) == true) { lst_tolerance.Add(s_field); }
            }

            List<string> lst_priority_selected = new List<string>();

            foreach (string s_field in lst_strategy_field_priority)
            {
                if (lst_combination.Contains(s_field))
                {
                    dr_query[s_field] = "*";

                    lst_priority_selected.Add(s_field);

                    if (dt_strategy_field_tolerance.Columns.Contains(s_field) == true) { lst_tolerance.Add(s_field); }
                }
            }

            dt_query.Rows.Add(dr_query);

            // GET TOLERANCE COMBINATIONS

            if (lst_tolerance.Count > 0)
            {
                DataTable dt_tolerance = new DataView(dt_strategy_field_tolerance).ToTable(true, lst_tolerance.ToArray());

                Dictionary<string, string> dic_tolerance_1 = new Dictionary<string, string>();
                Dictionary<string, string> dic_tolerance_2 = new Dictionary<string, string>();
                Dictionary<string, string> dic_tolerance_3 = new Dictionary<string, string>();

                for (int i = 0; i < dt_tolerance.Rows.Count; i++)
                {
                    for (int j = dt_tolerance.Columns.Count - 1; j >= 0; j--)
                    {
                        string s_value = dt_tolerance.Rows[i][j].ToString();

                        if (s_value != "")
                        {
                            switch (i)
                            {
                                case 0:
                                    dic_tolerance_1.Add(dt_tolerance.Columns[j].ColumnName, s_value);
                                    break;
                                case 1:
                                    dic_tolerance_2.Add(dt_tolerance.Columns[j].ColumnName, s_value);
                                    break;
                                case 2:
                                    dic_tolerance_3.Add(dt_tolerance.Columns[j].ColumnName, s_value);
                                    break;
                            }
                        }
                    }
                }

                if (dic_tolerance_1.Count > 0)
                {
                    int n_size_item_tolerance = dic_tolerance_1.Count;

                    for (int n_size_temp_tolerance = 1; n_size_temp_tolerance <= n_size_item_tolerance; n_size_temp_tolerance++)
                    {
                        List<List<string>> lst_tolerance_combination_table = new List<List<string>>();

                        Combination<string>.Bind_Combination(dic_tolerance_1.Keys.ToArray(), n_size_item_tolerance, n_size_temp_tolerance, ref lst_tolerance_combination_table);

                        foreach (List<string> lst_tolerance_combination in lst_tolerance_combination_table)
                        {
                            Select_Query_Row_Tolerance(lst_strategy_field_const, lst_strategy_field_priority, lst_priority_selected, lst_tolerance_combination, dic_tolerance_1, b_gui, ref dt_query, ref i_row);

                            if (dic_tolerance_2.Count > 0)
                            {
                                bool b_repeate = false;

                                Dictionary<string, string> dic_tolerance_repeate = new Dictionary<string, string>(dic_tolerance_1);

                                foreach (string s_key in dic_tolerance_2.Keys)
                                {
                                    if (lst_tolerance_combination.Contains(s_key) && dic_tolerance_repeate.Keys.Contains(s_key))
                                    {
                                        b_repeate = true;

                                        dic_tolerance_repeate[s_key] = dic_tolerance_2[s_key];
                                    }
                                }

                                if (b_repeate == true)
                                {
                                    Select_Query_Row_Tolerance(lst_strategy_field_const, lst_strategy_field_priority, lst_priority_selected, lst_tolerance_combination, dic_tolerance_repeate, b_gui, ref dt_query, ref i_row);
                                }
                            }

                            if (dic_tolerance_3.Count > 0)
                            {
                                bool b_repeate = false;

                                Dictionary<string, string> dic_tolerance_repeate = new Dictionary<string, string>(dic_tolerance_1);

                                foreach (string s_key in dic_tolerance_3.Keys)
                                {
                                    if (lst_tolerance_combination.Contains(s_key) && dic_tolerance_repeate.Keys.Contains(s_key))
                                    {
                                        b_repeate = true;

                                        dic_tolerance_repeate[s_key] = dic_tolerance_3[s_key];
                                    }
                                }

                                if (b_repeate == true)
                                {
                                    Select_Query_Row_Tolerance(lst_strategy_field_const, lst_strategy_field_priority, lst_priority_selected, lst_tolerance_combination, dic_tolerance_repeate, b_gui, ref dt_query, ref i_row);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void Select_Query_Row_Tolerance(List<string> lst_strategy_field_const, List<string> lst_strategy_field_priority, List<string> lst_priority_selected, List<string> lst_tolerance_combination, Dictionary<string, string> dic_tolerance, bool b_gui, ref DataTable dt_query, ref int i_row)
        {
            i_row++;

            DataRow dr_query = dt_query.NewRow();

            dr_query["QueryNumber"] = i_row;

            foreach (string s_field in lst_strategy_field_const)
            {
                string s_value = "*";

                if (lst_tolerance_combination.Contains(s_field))
                {
                    s_value = dic_tolerance[s_field];
                }

                dr_query[s_field] = s_value;
            }

            foreach (string s_field in lst_strategy_field_priority)
            {
                string s_value = "";

                if (lst_priority_selected.Contains(s_field))
                {
                    if (lst_tolerance_combination.Contains(s_field))
                    {
                        s_value = dic_tolerance[s_field];
                    }
                    else
                    {
                        s_value = "*";
                    }
                }

                dr_query[s_field] = s_value;
            }

            dt_query.Rows.Add(dr_query);
        }

        public static void Update(ref StrategyModel o_strategy)
        {
            string s_procedure = "";

            if (o_strategy.ID == 0) { s_procedure = "sprStrategyInsert"; } else { s_procedure = "sprStrategyUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            if (o_strategy.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_strategy.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_strategy.UserID;

            o_command.Parameters.Add("@StrategyName", SqlDbType.NVarChar, 50);
            o_command.Parameters["@StrategyName"].Value = o_strategy.StrategyName;

            o_command.Parameters.Add("@QueryPercent", SqlDbType.Float);
            o_command.Parameters["@QueryPercent"].Value = o_strategy.QueryPercent;

            o_command.Parameters.AddWithValue("@TableStrategyTemplate", o_strategy.TableStrategyTemplate);
            o_command.Parameters["@TableStrategyTemplate"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableStrategyField", o_strategy.TableStrategyField);
            o_command.Parameters["@TableStrategyField"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                if (o_strategy.ID == 0)
                {
                    o_strategy.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_strategy.ErrorMessage = ex.Message;
                o_strategy.ErrorMessage = "Error on update strategy.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static int Update_Data(int n_user_id, int n_strategy_id, DataTable dt_data_file, ref string s_error)
        {
            int n_rows_affected = 0;

            SqlCommand o_command = new SqlCommand("sprStrategyUpdateData", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@StrategyID", SqlDbType.Int);
            o_command.Parameters["@StrategyID"].Value = n_strategy_id;

            o_command.Parameters.AddWithValue("@TableDataFile", dt_data_file);
            o_command.Parameters["@TableDataFile"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update strategy data files.";
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

        public static int Update_Status(int n_user_id, int n_strategy_id, int n_status_id, ref string s_error)
        {
            int n_rows_affected = 0;

            SqlCommand o_command = new SqlCommand("sprStrategyUpdateStatus", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@StrategyID", SqlDbType.Int) { Value = n_strategy_id });
            o_command.Parameters.Add(new SqlParameter("@StatusID", SqlDbType.Int) { Value = n_status_id });

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update strategy status.";
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

        public static int Delete_Data(int n_user_id, int n_strategy_id, int n_data_file_id, ref string s_error)
        {
            int n_rows_affected = 0;

            SqlCommand o_command = new SqlCommand("sprStrategyDeleteData", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@StrategyID", SqlDbType.Int);
            o_command.Parameters["@StrategyID"].Value = n_strategy_id;

            o_command.Parameters.Add("@DataFileID", SqlDbType.Int);
            o_command.Parameters["@DataFileID"].Value = n_data_file_id;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete strategy data files.";
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
