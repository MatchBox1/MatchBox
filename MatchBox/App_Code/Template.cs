using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MatchBox
{
    public class TemplateModel
    {
        public static string InterfaceName
        {
            get { return "TEMPLATE"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int DataSourceID { get; set; }
        public int CreditID { get; set; }
        public int DiscountID { get; set; }
        public int HeaderRowsCount { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsImportable { get; set; }

        public string TerminalField { get; set; }
        public string ErrorMessage { get; set; }

        public DataTable TableTemplateField { get; set; }
        public DataTable TableTemplateCurrency { get; set; }
        public DataTable TableTemplateCredit { get; set; }
        public DataTable TableTemplateCard { get; set; }
        public DataTable TableTemplateOperationType { get; set; }
        public DataTable TableTemplateService { get; set; }

        public TemplateModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            DataSourceID = 0;
            CreditID = 0;
            DiscountID = 0;
            HeaderRowsCount = 0;

            IsCompleted = false;
            IsImportable = false;

            TerminalField = "";
            ErrorMessage = "";

            TableTemplateField = new DataTable();
            TableTemplateField.Columns.Add("FieldFromExcel");
            TableTemplateField.Columns.Add("FieldFromDB");
            TableTemplateField.Columns.Add("FieldExclude");
            TableTemplateField.Columns.Add("FieldFormat");
            TableTemplateField.Columns.Add("FieldType");
            TableTemplateField.Columns.Add("FieldDescription");
            TableTemplateField.Columns.Add("IsBaseDate");

            TableTemplateCurrency = new DataTable();
            TableTemplateCurrency.Columns.Add("CurrencyID");
            TableTemplateCurrency.Columns.Add("CurrencyFromUser");
            TableTemplateCurrency.Columns.Add("CurrencyName");

            TableTemplateCredit = new DataTable();
            TableTemplateCredit.Columns.Add("CreditID");
            TableTemplateCredit.Columns.Add("CreditFromUser");
            TableTemplateCredit.Columns.Add("CreditName");

            TableTemplateCard = new DataTable();
            TableTemplateCard.Columns.Add("CardID");
            TableTemplateCard.Columns.Add("CardFromUser");
            TableTemplateCard.Columns.Add("CardName");

            TableTemplateOperationType = new DataTable();
            TableTemplateOperationType.Columns.Add("OperationTypeID");
            TableTemplateOperationType.Columns.Add("OperationTypeFromUser");
            TableTemplateOperationType.Columns.Add("OperationTypeName");
            TableTemplateOperationType.Columns.Add("IsAgreementPart");

            TableTemplateService = new DataTable();
            TableTemplateService.Columns.Add("ServiceID");
            TableTemplateService.Columns.Add("IsCompleted");
            TableTemplateService.Columns.Add("CompletedMessage");
        }
    }

    public class TemplateAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprTemplateSelectList", DB.Get_Connection());
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
                        case "IsImportable":
                        case "NoPaging":
                            o_db_type = SqlDbType.Bit;
                            break;
                        case "UserID":
                        case "DataSourceID":
                        case "CreditID":
                        case "DiscountID":
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
                        case SqlDbType.NVarChar:
                            o_command.Parameters.Add("@" + s_key, o_db_type, n_size);
                            o_command.Parameters["@" + s_key].Value = s_value;
                            break;
                    }
                }
            }

            DB.Bind_Data_Table(o_command, ref o_data_table, ref n_pages_count, ref s_error);
        }

        public static void Select(ref TemplateModel o_template)
        {
            if (o_template.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprTemplateSelect", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_template.ID });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_template.UserID });
            o_command.Parameters.Add(new SqlParameter("@IsImportable", SqlDbType.Bit) { Value = o_template.IsImportable });

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                DataRow dr_template = o_data_set.Tables[0].Rows[0];

                o_template.DataSourceID = (int)dr_template["DataSourceID"];
                o_template.CreditID = (int)dr_template["CreditID"];
                o_template.DiscountID = (int)dr_template["DiscountID"];
                o_template.HeaderRowsCount = (int)dr_template["HeaderRowsCount"];

                o_template.IsCompleted = (bool)dr_template["IsCompleted"];

                o_template.TerminalField = dr_template["TerminalField"].ToString();

                o_template.TableTemplateField = o_data_set.Tables[1];
                o_template.TableTemplateCurrency = o_data_set.Tables[2];
                o_template.TableTemplateCredit = o_data_set.Tables[3];
                o_template.TableTemplateCard = o_data_set.Tables[4];
                o_template.TableTemplateOperationType = o_data_set.Tables[5];
                o_template.TableTemplateService = o_data_set.Tables[6];
            }
            catch (Exception ex)
            {
                o_template.ErrorMessage = ex.Message;
                o_template.ErrorMessage = "Error on select template.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void Validate(ref TemplateModel o_template)
        {
            string[] arr_template_field = new string[] { "FieldFromExcel", "FieldFromDB", "FieldExclude", "FieldFormat" };

            DataTable dt_template_field = new DataView(o_template.TableTemplateField).ToTable(true, arr_template_field);

            SqlCommand o_command = new SqlCommand("sprTemplateValidate", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@TemplateID", SqlDbType.Int);
            o_command.Parameters["@TemplateID"].Value = o_template.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_template.UserID;

            o_command.Parameters.Add("@DataSourceID", SqlDbType.Int);
            o_command.Parameters["@DataSourceID"].Value = o_template.DataSourceID;

            o_command.Parameters.Add("@CreditID", SqlDbType.Int);
            o_command.Parameters["@CreditID"].Value = o_template.CreditID;

            o_command.Parameters.Add("@DiscountID", SqlDbType.Int);
            o_command.Parameters["@DiscountID"].Value = o_template.DiscountID;

            o_command.Parameters.AddWithValue("@TableTemplateField", dt_template_field);
            o_command.Parameters["@TableTemplateField"].SqlDbType = SqlDbType.Structured;

            int n_error_id = 0;
            string s_error_description = "";

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                o_reader.Read();

                n_error_id = Convert.ToInt32(o_reader["ErrorID"]);
                s_error_description = o_reader["ErrorDescription"].ToString();

                o_reader.Close();
            }
            catch (Exception ex)
            {
                o_template.ErrorMessage = ex.Message;
                o_template.ErrorMessage = "Error on validate template.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            if (n_error_id > 0)
            {
                o_template.ErrorMessage = s_error_description;
                return;
            }
        }

        public static void Validate_Completed(ref TemplateModel o_template, DataTable dt_data_field, int[] arr_user_service_id)
        {
            string s_error = "";

            DataTable dt_service = new DataTable();

            DB.Bind_Data_Table("sprService", ref dt_service, ref s_error);

            if (s_error != "")
            {
                o_template.ErrorMessage = s_error;
                return;
            }

            string s_table_name = "";

            if (o_template.DataSourceID > 0)
            {
                s_table_name = "tblData_Inside";
            }
            else if (o_template.CreditID > 0 || o_template.DiscountID > 0)
            {
                s_table_name = "tblData_Outside";
            }

            DataTable dt_service_field = new DataTable();

            DB.Bind_Data_Table("sprServiceField", ref dt_service_field, ref s_error, "@TableName", s_table_name);

            if (s_error != "")
            {
                o_template.ErrorMessage = s_error;
                return;
            }

            // === CHECK FIELDS === Sort s_terminal_field by arr_terminal_field_all order ===

            List<string> lst_template_field = (
                from r in o_template.TableTemplateField.AsEnumerable()
                where r.Field<string>("FieldFromDB") != ""
                select r.Field<string>("FieldFromDB")
                ).ToList();

            string[] arr_terminal_field_all = { "TerminalNumber", "SupplierNumber", "CompanyNumber", "NetworkNumber", "BranchNumber", "CashBoxNumber" };

            string[] arr_terminal_field = (
                from f in lst_template_field
                where arr_terminal_field_all.Contains(f)
                select f
                ).ToArray();

            string s_terminal_field = "";

            for (int i = 0; i < arr_terminal_field_all.Length; i++)
            {
                string s_field = arr_terminal_field_all[i];

                if (arr_terminal_field.Contains(s_field))
                {
                    if (s_terminal_field != "") { s_terminal_field += ","; }

                    s_terminal_field += s_field;
                }
            }

            if (s_terminal_field != "")
            {
                bool b_cashbox = (s_terminal_field.Contains("CompanyNumber") || s_terminal_field.Contains("NetworkNumber") || s_terminal_field.Contains("BranchNumber") || s_terminal_field.Contains("CashBoxNumber"));

                // REMOVE TerminalNumber & SupplierNumber IF CASHBOX EXISTS

                if (b_cashbox == true)
                {
                    s_terminal_field = s_terminal_field.Replace("TerminalNumber,", "").Replace("SupplierNumber,", "");
                }
            }

            o_template.TerminalField = s_terminal_field;

            // CHECK Service

            foreach (DataRow dr_service in dt_service.Rows)
            {
                int n_service_id = Convert.ToInt32(dr_service["ID"]);

                if (dt_service_field.Select(" ServiceID = " + n_service_id).Length == 0) { continue; }

                string s_service_message = "";
                string s_service_name = dr_service["ServiceName"].ToString();

                // AlternativeFunction = False

                foreach (DataRow dr_service_field in dt_service_field.Select(" ServiceID = " + n_service_id + " AND AlternativeFunction = False "))
                {
                    string s_exists_field = "";
                    string s_required_field = dr_service_field["RequiredField"].ToString();

                    // CreditBrand is actual only for Discount
                    if (s_required_field == "CreditBrand" && o_template.DiscountID == 0) { continue; }

                    // DutyPaymentAmount & DutyPaymentNumber & PaymentDateActual is not required for Discount
                    if ((s_required_field == "DutyPaymentAmount" || s_required_field == "DutyPaymentNumber" || s_required_field == "PaymentDateActual") && o_template.DiscountID > 0) { continue; }

                    string[] arr_alternative_field = dr_service_field["AlternativeField"].ToString().Split(',');

                    bool b_exists = lst_template_field.Contains(s_required_field);

                    if (b_exists == false)
                    {
                        foreach (string s_alternative_field in arr_alternative_field)
                        {
                            b_exists = lst_template_field.Contains(s_alternative_field);

                            if (b_exists)
                            {
                                s_exists_field = s_alternative_field;
                                break;
                            }
                        }
                    }
                    else
                    {
                        s_exists_field = s_required_field;
                    }

                    if (b_exists == false)
                    {
                        string s_missing_fields = " <b>" + dt_data_field.Select(" FieldName = '" + s_required_field + "' ").First()["FieldDescription"] + "</b> ";

                        foreach (string s_alternative_field in arr_alternative_field)
                        {
                            if (s_alternative_field != "") { s_missing_fields += " OR <b>" + dt_data_field.Select(" FieldName = '" + s_alternative_field + "' ").First()["FieldDescription"] + "</b> "; }
                        }

                        s_service_message += "<li>Required to select ( " + s_missing_fields + " )</li>";
                    }
                    else
                    {
                        string s_format = (
                            from r in o_template.TableTemplateField.AsEnumerable()
                            where r.Field<string>("FieldFromDB") == s_exists_field
                            select r.Field<string>("FieldFormat")
                            ).First();

                        if (s_format == "")
                        {
                            string s_data_type = dt_data_field.Select(" FieldName = '" + s_exists_field + "' ").First()["FieldType"].ToString();

                            switch (s_data_type)
                            {
                                case "date":
                                    s_service_message += "<li>Required to select date format for ( <b>" + dt_data_field.Select(" FieldName = '" + s_exists_field + "' ").First()["FieldDescription"] + "</b> )</li>";
                                    break;
                                case "bit":
                                    s_service_message += "<li>Required to enter value/s separated by commas for ( <b>" + dt_data_field.Select(" FieldName = '" + s_exists_field + "' ").First()["FieldDescription"] + "</b> )</li>";
                                    break;
                            }
                        }
                    }
                }

                // AlternativeFunction = True

                foreach (DataRow dr_service_field in dt_service_field.Select(" ServiceID = " + n_service_id + " AND AlternativeFunction = True "))
                {
                    string s_required_field = dr_service_field["RequiredField"].ToString();

                    bool b_exists = lst_template_field.Contains(s_required_field);

                    if (b_exists == true)
                    {
                        string s_format = (
                            from r in o_template.TableTemplateField.AsEnumerable()
                            where r.Field<string>("FieldFromDB") == s_required_field
                            select r.Field<string>("FieldFormat")
                            ).First();

                        if (s_format == "")
                        {
                            string s_data_type = dt_data_field.Select(" FieldName = '" + s_required_field + "' ").First()["FieldType"].ToString();

                            switch (s_data_type)
                            {
                                case "date":
                                    s_service_message += "<li>Required to select date format for ( <b>" + dt_data_field.Select(" FieldName = '" + s_required_field + "' ").First()["FieldDescription"] + "</b> )</li>";
                                    break;
                                case "bit":
                                    s_service_message += "<li>Required to enter value/s separated by commas for ( <b>" + dt_data_field.Select(" FieldName = '" + s_required_field + "' ").First()["FieldDescription"] + "</b> )</li>";
                                    break;
                            }
                        }
                    }
                }

                bool b_service_completed = (s_service_message == "");

                if (b_service_completed)
                {
                    s_service_message = "<h4>Template is completed for '" + s_service_name + "' service.</h4>";
                }
                else
                {
                    s_service_message = "<h4>Template is not completed for '" + s_service_name + "' service. Required fields:</h4>" + "<ul>" + s_service_message + "</ul>";
                }

                DataRow dr_template_service = o_template.TableTemplateService.NewRow();

                dr_template_service["ServiceID"] = n_service_id;
                dr_template_service["IsCompleted"] = b_service_completed;
                dr_template_service["CompletedMessage"] = s_service_message;

                o_template.TableTemplateService.Rows.Add(dr_template_service);
            }

            // === END FIELDS ===

            o_template.IsCompleted = (arr_user_service_id.Length > 0) && (o_template.TableTemplateService.Select(" ServiceID IN ( " + String.Join(",", arr_user_service_id) + " ) AND IsCompleted = True ").Length > 0);
        }

        public static void Update(ref TemplateModel o_template)
        {
            string[] arr_template_field = new string[] { "FieldFromExcel", "FieldFromDB", "FieldExclude", "FieldFormat" };

            DataTable dt_template_field = new DataView(o_template.TableTemplateField).ToTable(true, arr_template_field);

            string s_procedure = "";

            if (o_template.ID == 0) { s_procedure = "sprTemplateInsert"; } else { s_procedure = "sprTemplateUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_template.ID > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = o_template.ID;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_template.UserID;

            o_command.Parameters.Add("@DataSourceID", SqlDbType.Int);
            o_command.Parameters["@DataSourceID"].Value = o_template.DataSourceID;

            o_command.Parameters.Add("@CreditID", SqlDbType.Int);
            o_command.Parameters["@CreditID"].Value = o_template.CreditID;

            o_command.Parameters.Add("@DiscountID", SqlDbType.Int);
            o_command.Parameters["@DiscountID"].Value = o_template.DiscountID;

            o_command.Parameters.Add("@HeaderRowsCount", SqlDbType.Int);
            o_command.Parameters["@HeaderRowsCount"].Value = o_template.HeaderRowsCount;

            o_command.Parameters.Add("@IsCompleted", SqlDbType.Bit);
            o_command.Parameters["@IsCompleted"].Value = o_template.IsCompleted;

            o_command.Parameters.Add("@TerminalField", SqlDbType.NVarChar, 150);
            o_command.Parameters["@TerminalField"].Value = o_template.TerminalField;

            o_command.Parameters.AddWithValue("@TableTemplateField", dt_template_field);
            o_command.Parameters["@TableTemplateField"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableTemplateService", o_template.TableTemplateService);
            o_command.Parameters["@TableTemplateService"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                if (o_template.ID == 0)
                {
                    o_template.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_template.ErrorMessage = ex.Message;
                o_template.ErrorMessage = "Error on update template.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Currency(int n_user_id, int n_template_id, DataTable dt_template_currency, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprTemplateCurrencyInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@TemplateID", SqlDbType.Int);
            o_command.Parameters["@TemplateID"].Value = n_template_id;

            o_command.Parameters.AddWithValue("@TableTemplateCurrency", dt_template_currency);
            o_command.Parameters["@TableTemplateCurrency"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update data file template currency.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Credit(int n_user_id, int n_template_id, DataTable dt_template_credit, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprTemplateCreditInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@TemplateID", SqlDbType.Int);
            o_command.Parameters["@TemplateID"].Value = n_template_id;

            o_command.Parameters.AddWithValue("@TableTemplateCredit", dt_template_credit);
            o_command.Parameters["@TableTemplateCredit"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update data file template credit.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Card(int n_user_id, int n_template_id, DataTable dt_template_card, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprTemplateCardInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@TemplateID", SqlDbType.Int);
            o_command.Parameters["@TemplateID"].Value = n_template_id;

            o_command.Parameters.AddWithValue("@TableTemplateCard", dt_template_card);
            o_command.Parameters["@TableTemplateCard"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update data file template card.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Operation_Type(int n_user_id, int n_template_id, DataTable dt_template_operation_type, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprTemplateOperationTypeInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@TemplateID", SqlDbType.Int);
            o_command.Parameters["@TemplateID"].Value = n_template_id;

            o_command.Parameters.AddWithValue("@TableTemplateOperationType", dt_template_operation_type);
            o_command.Parameters["@TableTemplateOperationType"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update data file template operation type.";
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
