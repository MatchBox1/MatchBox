using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class DataFileModel
    {
        public static string InterfaceName
        {
            get { return "DATAFILE"; }
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public int TemplateID { get; set; }
        public int CompanyID { get; set; }
        public int NetworkID { get; set; }
        public int BranchID { get; set; }
        public int CashBoxID { get; set; }
        public int CardID { get; set; }

        public int HeaderRowsCount { get; set; }
        public int DataRowsCount { get; set; }
        public int SplittedRowsCount { get; set; }
        public int ExcelRowsCount { get; set; }
        public int ExcludedRowsCount { get; set; }

        public double DataMoneyAmount { get; set; }
        public double ExcelMoneyAmount { get; set; }
        public double ExcludedMoneyAmount { get; set; }

        public string NetworkNumber { get; set; }
        public string BranchNumber { get; set; }
        public string CashBoxNumber { get; set; }
        public string FileName { get; set; }
        public string ListColumns { get; set; }
        public string ErrorMessage { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public DateTime DateUpload { get; set; }
        public TimeSpan TimeUpload { get; set; }
        public TimeSpan ExecutionDuration { get; set; }

        public bool StopIfCardIsEmpty { get; set; }
        public bool StopIfTerminalIsZero { get; set; }
        public bool StopIfSupplierIsZero { get; set; }
        public bool StopIfTerminalNotExists { get; set; }
        public bool StopIfSupplierNotExists { get; set; }
        public bool StopIfAgreementNotExists { get; set; }
        public bool IsValid { get; set; }

        public DataTable TableExcel { get; set; }
        public DataTable TableExcelShort { get; set; }
        public DataTable TableDataInside { get; set; }
        public DataTable TableDataOutside { get; set; }

        public DataFileModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            TemplateID = 0;
            CompanyID = 0;
            NetworkID = 0;
            BranchID = 0;
            CashBoxID = 0;
            CardID = 0;

            HeaderRowsCount = 0;
            DataRowsCount = 0;
            SplittedRowsCount = 0;
            ExcelRowsCount = 0;
            ExcludedRowsCount = 0;

            DataMoneyAmount = 0;
            ExcelMoneyAmount = 0;
            ExcludedMoneyAmount = 0;

            NetworkNumber = "";
            BranchNumber = "";
            CashBoxNumber = "";
            FileName = "";
            ListColumns = "";
            ErrorMessage = "";

            DateFrom = null;
            DateTo = null;
            ExecutionDuration = new TimeSpan(0, 0, 0);

            StopIfCardIsEmpty = false;
            StopIfTerminalIsZero = false;
            StopIfSupplierIsZero = false;
            StopIfTerminalNotExists = false;
            StopIfSupplierNotExists = false;
            StopIfAgreementNotExists = false;
            IsValid = false;

            // TableDataInside

            TableDataInside = new DataTable();
            // int
            TableDataInside.Columns.Add("CompanyID", typeof(int));
            TableDataInside.Columns.Add("NetworkID", typeof(int));
            TableDataInside.Columns.Add("BranchID", typeof(int));
            TableDataInside.Columns.Add("CashBoxID", typeof(int));
            TableDataInside.Columns.Add("CashBoxPeriodTerminalID", typeof(int));
            TableDataInside.Columns.Add("AgreementDiscountTerminalID", typeof(int));
            TableDataInside.Columns.Add("CardID", typeof(int));
            TableDataInside.Columns.Add("OperationTypeID", typeof(int));
            TableDataInside.Columns.Add("TransactionCurrencyID", typeof(int));
            TableDataInside.Columns.Add("CardPrefix", typeof(int));
            TableDataInside.Columns.Add("CardNumber", typeof(int));
            TableDataInside.Columns.Add("PaymentsCount", typeof(int));
            TableDataInside.Columns.Add("DutyPaymentNumber", typeof(int));
            // bigint
            TableDataInside.Columns.Add("TransmissionNumber", typeof(long));
            // float
            TableDataInside.Columns.Add("TransactionGrossAmount", typeof(double));
            TableDataInside.Columns.Add("FirstPaymentAmount", typeof(double));          // ---
            TableDataInside.Columns.Add("DutyPaymentAmount", typeof(double));
            TableDataInside.Columns.Add("RemainingPaymentsAmount", typeof(double));
            // date
            TableDataInside.Columns.Add("TransactionDate", typeof(string));
            TableDataInside.Columns.Add("TransmissionDate", typeof(string));
            TableDataInside.Columns.Add("PaymentDate", typeof(string));
            // varchar(15)
            TableDataInside.Columns.Add("ReceiptNumber", typeof(string));               // ---
            TableDataInside.Columns.Add("VoucherNumber", typeof(string));
            TableDataInside.Columns.Add("ConfirmationNumber", typeof(string));
            // bit
            TableDataInside.Columns.Add("IsSplitted", typeof(bool));                     // ---
            // uniqueidentifier
            TableDataInside.Columns.Add("UniqueID", typeof(Guid));

            // TableDataOutside

            TableDataOutside = new DataTable();
            // int
            TableDataOutside.Columns.Add("CompanyID", typeof(int));
            TableDataOutside.Columns.Add("CashBoxPeriodTerminalID", typeof(int));
            TableDataOutside.Columns.Add("AgreementDiscountTerminalID", typeof(int));
            TableDataOutside.Columns.Add("TerminalID", typeof(int));                    // ---
            TableDataOutside.Columns.Add("SupplierID", typeof(int));                    // ---
            TableDataOutside.Columns.Add("SupplierGroupID", typeof(int));               // ---
            TableDataOutside.Columns.Add("CreditID", typeof(int));                      // ---
            TableDataOutside.Columns.Add("CardID", typeof(int));
            TableDataOutside.Columns.Add("OperationTypeID", typeof(int));
            TableDataOutside.Columns.Add("TransactionCurrencyID", typeof(int));
            TableDataOutside.Columns.Add("PaymentCurrencyID", typeof(int));             // ---
            TableDataOutside.Columns.Add("BankID", typeof(int));                        // ---
            TableDataOutside.Columns.Add("BankBranchID", typeof(int));                  // ---
            TableDataOutside.Columns.Add("AccountNumber", typeof(int));                 // ---
            TableDataOutside.Columns.Add("CardPrefix", typeof(int));
            TableDataOutside.Columns.Add("CardNumber", typeof(int));
            TableDataOutside.Columns.Add("PaymentsCount", typeof(int));
            TableDataOutside.Columns.Add("DutyPaymentNumber", typeof(int));
            // bigint
            TableDataOutside.Columns.Add("TransmissionNumber", typeof(long));
            TableDataOutside.Columns.Add("InvoiceNumber", typeof(long));                // ---
            TableDataOutside.Columns.Add("ClubNumber", typeof(long));                   // ---
            // float
            TableDataOutside.Columns.Add("TransactionGrossAmount", typeof(double));
            TableDataOutside.Columns.Add("DutyPaymentAmount", typeof(double));
            TableDataOutside.Columns.Add("RemainingPaymentsAmount", typeof(double));
            TableDataOutside.Columns.Add("NetPaymentAmount", typeof(double));           // ---
            TableDataOutside.Columns.Add("ClearingCommission", typeof(double));         // ---
            TableDataOutside.Columns.Add("NotElectronicCommission", typeof(double));    // ---
            TableDataOutside.Columns.Add("ManualCommission", typeof(double));           // ---
            TableDataOutside.Columns.Add("CancellationCommission", typeof(double));     // ---
            TableDataOutside.Columns.Add("TelephoneCommission", typeof(double));        // ---
            TableDataOutside.Columns.Add("DiscountCommission", typeof(double));         // ---
            TableDataOutside.Columns.Add("ClubManagementCommission", typeof(double));   // ---
            TableDataOutside.Columns.Add("ClubSaving", typeof(double));                 // ---
            TableDataOutside.Columns.Add("VAT", typeof(double));                        // ---
            TableDataOutside.Columns.Add("ExchangeRate", typeof(double));               // ---
            // date
            TableDataOutside.Columns.Add("TransactionDate", typeof(string));
            TableDataOutside.Columns.Add("TransmissionDate", typeof(string));
            TableDataOutside.Columns.Add("AbsorptionDate", typeof(string));             // ---
            TableDataOutside.Columns.Add("PaymentDate", typeof(string));                // ---
            TableDataOutside.Columns.Add("PaymentDateActual", typeof(string));          // ---
            TableDataOutside.Columns.Add("InvoiceDate", typeof(string));                // ---
            // varchar(15)
            TableDataOutside.Columns.Add("VoucherNumber", typeof(string));
            TableDataOutside.Columns.Add("ConfirmationNumber", typeof(string));
            // bit
            TableDataOutside.Columns.Add("IsAbroad", typeof(bool));                     // ---
            // uniqueidentifier
            TableDataOutside.Columns.Add("UniqueID", typeof(Guid));
        }
    }

    public class DataFileAction
    {
        public static void Select(ref DataTable o_data_table, ref int n_pages_count, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprDataFileSelectList", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

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
                        case "TemplateID":
                        case "CompanyID":
                        case "NetworkID":
                        case "BranchID":
                        case "CashBoxID":
                        case "CardID":
                        case "PageNumber":
                        case "PageSize":
                            o_db_type = SqlDbType.Int;
                            break;
                        case "FileName":
                            o_db_type = SqlDbType.NVarChar;
                            n_size = 100;
                            break;
                        case "OrderBy":
                            o_db_type = SqlDbType.VarChar;
                            n_size = 50;
                            break;
                        case "NetworkNumber":
                        case "BranchNumber":
                        case "CashBoxNumber":
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

        public static void Select(ref DataFileModel o_data_file)
        {
            if (o_data_file.ID == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprDataFileSelect", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_data_file.ID });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_data_file.UserID });

            try
            {
                o_command.Connection.Open();

                SqlDataReader o_reader = o_command.ExecuteReader();

                if (o_reader.Read())
                {
                    o_data_file.TemplateID = (int)o_reader["TemplateID"];
                    o_data_file.CompanyID = (int)o_reader["CompanyID"];
                    o_data_file.NetworkID = (int)o_reader["NetworkID"];
                    o_data_file.BranchID = (int)o_reader["BranchID"];
                    o_data_file.CashBoxID = (int)o_reader["CashBoxID"];
                    o_data_file.CardID = (int)o_reader["CardID"];

                    o_data_file.HeaderRowsCount = (int)o_reader["HeaderRowsCount"];
                    o_data_file.DataRowsCount = (int)o_reader["DataRowsCount"];
                    o_data_file.SplittedRowsCount = (int)o_reader["SplittedRowsCount"];
                    o_data_file.ExcelRowsCount = (int)o_reader["ExcelRowsCount"];
                    o_data_file.ExcludedRowsCount = (int)o_reader["ExcludedRowsCount"];

                    o_data_file.DataMoneyAmount = (double)o_reader["DataMoneyAmount"];
                    o_data_file.ExcelMoneyAmount = (double)o_reader["ExcelMoneyAmount"];
                    o_data_file.ExcludedMoneyAmount = (double)o_reader["ExcludedMoneyAmount"];

                    o_data_file.NetworkNumber = o_reader["NetworkNumber"].ToString();
                    o_data_file.BranchNumber = o_reader["BranchNumber"].ToString();
                    o_data_file.CashBoxNumber = o_reader["CashBoxNumber"].ToString();
                    o_data_file.FileName = o_reader["FileName"].ToString();

                    o_data_file.DateFrom = (DateTime)o_reader["DateFrom"];
                    o_data_file.DateTo = (DateTime)o_reader["DateTo"];
                    o_data_file.DateUpload = (DateTime)o_reader["DateUpload"];
                    o_data_file.TimeUpload = (TimeSpan)o_reader["TimeUpload"];
                    o_data_file.ExecutionDuration = (TimeSpan)o_reader["ExecutionDuration"];

                    o_data_file.StopIfCardIsEmpty = (bool)o_reader["StopIfCardIsEmpty"];
                    o_data_file.StopIfTerminalIsZero = (bool)o_reader["StopIfTerminalIsZero"];
                    o_data_file.StopIfSupplierIsZero = (bool)o_reader["StopIfSupplierIsZero"];
                    o_data_file.StopIfTerminalNotExists = (bool)o_reader["StopIfTerminalNotExists"];
                    o_data_file.StopIfSupplierNotExists = (bool)o_reader["StopIfSupplierNotExists"];
                    o_data_file.StopIfAgreementNotExists = (bool)o_reader["StopIfAgreementNotExists"];
                    o_data_file.IsValid = (bool)o_reader["IsValid"];
                }

                o_reader.Close();
                o_reader.Dispose();
            }
            catch (Exception ex)
            {
                o_data_file.ErrorMessage = ex.Message;
                o_data_file.ErrorMessage = "Error on select data file.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Select_Summary(ref DataTable dt_inside_summary, ref DataTable dt_inside_summary_splitted, ref DataTable dt_outside_summary, ref DataTable dt_total_summary, ref string s_error, Dictionary<string, string> o_query_dictionary = null)
        {
            SqlCommand o_command = new SqlCommand("sprData_Summary", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            if (o_query_dictionary != null)
            {
                foreach (string s_key in o_query_dictionary.Keys)
                {
                    SqlDbType o_db_type = new SqlDbType();

                    string s_value = o_query_dictionary[s_key];

                    switch (s_key)
                    {
                        case "UserID":
                        case "CompanyID":
                        case "YearFrom":
                        case "MonthFrom":
                        case "YearTo":
                        case "MonthTo":
                            o_db_type = SqlDbType.Int;
                            break;
                    }

                    switch (o_db_type)
                    {
                        case SqlDbType.Int:
                            o_command.Parameters.Add("@" + s_key, o_db_type);
                            o_command.Parameters["@" + s_key].Value = Convert.ToInt32(s_value);
                            break;
                    }
                }
            }

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);

                dt_inside_summary = o_data_set.Tables[0];
                dt_inside_summary_splitted = o_data_set.Tables[1];
                dt_outside_summary = o_data_set.Tables[2];
                dt_total_summary = o_data_set.Tables[3];
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on select Data Summary.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static string Check(ref DataFileModel o_data_file)
        {
            string s_result = "";

            SqlCommand o_command = new SqlCommand("sprDataFileCheck", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_data_file.ID });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_data_file.UserID });
            o_command.Parameters.Add(new SqlParameter("@TemplateID", SqlDbType.Int) { Value = o_data_file.TemplateID });
            o_command.Parameters.Add(new SqlParameter("@CardID", SqlDbType.Int) { Value = o_data_file.CardID });
            o_command.Parameters.Add(new SqlParameter("@DateFrom", SqlDbType.Date) { Value = o_data_file.DateFrom });
            o_command.Parameters.Add(new SqlParameter("@DateTo", SqlDbType.Date) { Value = o_data_file.DateTo });

            try
            {
                o_command.Connection.Open();
                s_result = o_command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                o_data_file.ErrorMessage = ex.Message;
                o_data_file.ErrorMessage = "Error on check data file.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return s_result;
        }

        public static void Update(ref DataFileModel o_data_file)
        {
            string s_procedure = "";

            if (o_data_file.ID == 0) { s_procedure = "sprDataFileInsert"; } else { s_procedure = "sprDataFileUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            if (o_data_file.ID > 0)
            {
                o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = o_data_file.ID });
            }

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = o_data_file.UserID });
            o_command.Parameters.Add(new SqlParameter("@TemplateID", SqlDbType.Int) { Value = o_data_file.TemplateID });
            o_command.Parameters.Add(new SqlParameter("@CompanyID", SqlDbType.Int) { Value = o_data_file.CompanyID });
            o_command.Parameters.Add(new SqlParameter("@NetworkID", SqlDbType.Int) { Value = o_data_file.NetworkID });
            o_command.Parameters.Add(new SqlParameter("@BranchID", SqlDbType.Int) { Value = o_data_file.BranchID });
            o_command.Parameters.Add(new SqlParameter("@CashBoxID", SqlDbType.Int) { Value = o_data_file.CashBoxID });
            o_command.Parameters.Add(new SqlParameter("@CardID", SqlDbType.Int) { Value = o_data_file.CardID });

            o_command.Parameters.Add(new SqlParameter("@NetworkNumber", SqlDbType.NVarChar, 100) { Value = o_data_file.NetworkNumber });
            o_command.Parameters.Add(new SqlParameter("@BranchNumber", SqlDbType.NVarChar, 100) { Value = o_data_file.BranchNumber });
            o_command.Parameters.Add(new SqlParameter("@CashBoxNumber", SqlDbType.NVarChar, 100) { Value = o_data_file.CashBoxNumber });
            o_command.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 100) { Value = o_data_file.FileName });

            o_command.Parameters.Add(new SqlParameter("@DateFrom", SqlDbType.Date) { Value = o_data_file.DateFrom });
            o_command.Parameters.Add(new SqlParameter("@DateTo", SqlDbType.Date) { Value = o_data_file.DateTo });

            o_command.Parameters.Add(new SqlParameter("@ExecutionDuration", SqlDbType.Time) { Value = o_data_file.ExecutionDuration });

            o_command.Parameters.Add(new SqlParameter("@StopIfCardIsEmpty", SqlDbType.Bit) { Value = o_data_file.StopIfCardIsEmpty });
            o_command.Parameters.Add(new SqlParameter("@StopIfTerminalIsZero", SqlDbType.Bit) { Value = o_data_file.StopIfTerminalIsZero });
            o_command.Parameters.Add(new SqlParameter("@StopIfSupplierIsZero", SqlDbType.Bit) { Value = o_data_file.StopIfSupplierIsZero });
            o_command.Parameters.Add(new SqlParameter("@StopIfTerminalNotExists", SqlDbType.Bit) { Value = o_data_file.StopIfTerminalNotExists });
            o_command.Parameters.Add(new SqlParameter("@StopIfSupplierNotExists", SqlDbType.Bit) { Value = o_data_file.StopIfSupplierNotExists });
            o_command.Parameters.Add(new SqlParameter("@StopIfAgreementNotExists", SqlDbType.Bit) { Value = o_data_file.StopIfAgreementNotExists });
            o_command.Parameters.Add(new SqlParameter("@IsValid", SqlDbType.Bit) { Value = o_data_file.IsValid });

            o_command.Parameters.Add(new SqlParameter("@ListColumns", SqlDbType.NVarChar, -1) { Value = o_data_file.ListColumns });

            o_command.Parameters.AddWithValue("@TableExcel", o_data_file.TableExcel);
            o_command.Parameters["@TableExcel"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableExcelShort", o_data_file.TableExcelShort);
            o_command.Parameters["@TableExcelShort"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableDataInside", o_data_file.TableDataInside);
            o_command.Parameters["@TableDataInside"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableDataOutside", o_data_file.TableDataOutside);
            o_command.Parameters["@TableDataOutside"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                if (o_data_file.ID == 0)
                {
                    o_data_file.ID = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                o_data_file.ErrorMessage = ex.Message;
                o_data_file.ErrorMessage = "Error on update data file.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }
    }

    public class DataField
    {
        // int
        public int CompanyID { get; set; }
        public int NetworkID { get; set; }                      // Inside
        public int BranchID { get; set; }                       // Inside
        public int CashBoxID { get; set; }                      // Inside
        public int CashBoxPeriodTerminalID { get; set; }
        public int AgreementItemTerminalID { get; set; }
        public int AgreementDiscountTerminalID { get; set; }
        public int TerminalID { get; set; }                     // Outside
        public int SupplierID { get; set; }                     // Outside
        public int SupplierGroupID { get; set; }                // Outside
        public int CreditID { get; set; }                       // Outside
        public int CardID { get; set; }
        public int OperationTypeID { get; set; }
        public int TransactionCurrencyID { get; set; }
        public int PaymentCurrencyID { get; set; }              // Outside
        public int BankID { get; set; }                         // Outside
        public int BankBranchID { get; set; }                   // Outside
        public int AccountNumber { get; set; }                  // Outside
        public int CardPrefix { get; set; }
        public int CardNumber { get; set; }
        public int PaymentsCount { get; set; }
        public int DutyPaymentNumber { get; set; }

        // bigint
        public long TransmissionNumber { get; set; }
        public long InvoiceNumber { get; set; }                 // Outside
        public long ClubNumber { get; set; }                    // Outside

        // float
        public double TransactionGrossAmount { get; set; }
        public double FirstPaymentAmount { get; set; }          // Inside
        public double DutyPaymentAmount { get; set; }
        public double RemainingPaymentsAmount { get; set; }
        public double NetPaymentAmount { get; set; }            // Outside
        public double ClearingCommission { get; set; }          // Outside
        public double NotElectronicCommission { get; set; }     // Outside
        public double ManualCommission { get; set; }            // Outside
        public double CancellationCommission { get; set; }      // Outside
        public double TelephoneCommission { get; set; }         // Outside
        public double DiscountCommission { get; set; }          // Outside
        public double ClubManagementCommission { get; set; }    // Outside
        public double ClubSaving { get; set; }                  // Outside
        public double VAT { get; set; }                         // Outside
        public double ExchangeRate { get; set; }                // Outside

        // date
        public DateTime? TransactionDate { get; set; }
        public DateTime? TransmissionDate { get; set; }
        public DateTime? AbsorptionDate { get; set; }           // Outside
        public DateTime? PaymentDate { get; set; }              // Outside
        public DateTime? PaymentDateActual { get; set; }        // Outside
        public DateTime? InvoiceDate { get; set; }              // Outside

        // varchar(15)
        public string ReceiptNumber { get; set; }               // Inside
        public string VoucherNumber { get; set; }
        public string ConfirmationNumber { get; set; }

        // bit
        public bool IsAbroad { get; set; }                      // Outside

        public DataField()
        {
            // int
            CompanyID = 0;
            NetworkID = 0;
            BranchID = 0;
            CashBoxID = 0;
            CashBoxPeriodTerminalID = 0;
            AgreementItemTerminalID = 0;
            AgreementDiscountTerminalID = 0;
            TerminalID = 0;
            SupplierID = 0;
            SupplierGroupID = 0;
            CreditID = 0;
            CardID = 0;
            OperationTypeID = 0;
            TransactionCurrencyID = 0;
            PaymentCurrencyID = 0;
            BankID = 0;
            BankBranchID = 0;
            AccountNumber = 0;
            CardPrefix = 0;
            CardNumber = 0;
            PaymentsCount = 0;
            DutyPaymentNumber = 0;

            // long
            TransmissionNumber = 0;
            InvoiceNumber = 0;
            ClubNumber = 0;

            // double
            TransactionGrossAmount = 0;
            FirstPaymentAmount = 0;
            DutyPaymentAmount = 0;
            RemainingPaymentsAmount = 0;
            NetPaymentAmount = 0;
            ClearingCommission = 0;
            NotElectronicCommission = 0;
            ManualCommission = 0;
            CancellationCommission = 0;
            TelephoneCommission = 0;
            DiscountCommission = 0;
            ClubManagementCommission = 0;
            ClubSaving = 0;
            VAT = 0;
            ExchangeRate = 0;

            // DateTime?
            TransactionDate = null;
            TransmissionDate = null;
            AbsorptionDate = null;
            PaymentDate = null;
            PaymentDateActual = null;
            InvoiceDate = null;

            // string
            ReceiptNumber = "";
            VoucherNumber = "";
            ConfirmationNumber = "";

            // bool
            IsAbroad = false;
        }
    }
}
