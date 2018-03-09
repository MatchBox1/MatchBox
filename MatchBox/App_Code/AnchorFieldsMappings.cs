using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatchBox
{
    public static class AnchorFieldsMappings
    {
        //Dictionary<string,string> objDic = new Dictionary<string,string>();
        public static string AnchorFieldsMapping(string strColumnName)
        {
            IDictionary<string, string> ObjDic = new Dictionary<string, string>();
            ObjDic.Add("ID", "ID");
            ObjDic.Add("Status", "StatusName");
            ObjDic.Add("Source", "UniqueID");
            ObjDic.Add("Template", "TemplateName");
            ObjDic.Add("File", "DataFileID");
            ObjDic.Add("Strategy", "StrategyName");
            ObjDic.Add("Matching", "MatchingID");
            ObjDic.Add("Action", "MatchingActionName");
            ObjDic.Add("Query", "QueryNumber");
            ObjDic.Add("Type", "MatchingTypeName");
            ObjDic.Add("Match", "QueryID");
            ObjDic.Add("HP of the company", "CompanyNumber");
            ObjDic.Add("Network number", "NetworkNumber");
            ObjDic.Add("branch number", "BranchNumber");
            ObjDic.Add("POSITION NUMBER", "CashBoxNumbe");
            ObjDic.Add("terminal number", "TerminalNumber");
            ObjDic.Add("provider number", "SupplierNumber");
            ObjDic.Add("Network master number", "SupplierGroupNumber");
            ObjDic.Add("credit Company", "CreditBrand");
            ObjDic.Add("Brand", "CardBrand");
            ObjDic.Add("The type of action", "OperationType");
            ObjDic.Add("Currency Transaction", "TransactionCurrency");
            ObjDic.Add("Card prefix", "CardPrefix");
            ObjDic.Add("Ticket Number", "CardNumber");
            ObjDic.Add("Amount of payments", "PaymentsCount");
            ObjDic.Add("Fixed payment number", "DutyPaymentNumber");
            ObjDic.Add("Gross amount", "TransactionGrossAmount");
            ObjDic.Add("First payment amount", "FirstPaymentAmount");
            ObjDic.Add("The payment amount is outstanding", "DutyPaymentAmount");
            ObjDic.Add("Amount of other payments", "RemainingPaymentsAmount");
            ObjDic.Add("Date of transaction execution", "TransactionDate");
            ObjDic.Add("Transaction Transaction Date", "TransmissionDate");
            ObjDic.Add("Credit Date -Date", "PaymentDate");
            ObjDic.Add("Reception number", "ReceiptNumber");
            ObjDic.Add("Broadcast number", "TransmissionNumber");
            ObjDic.Add("voucher number", "VoucherNumber");
            ObjDic.Add("confirmation number", "ConfirmationNumber");
            ObjDic.Add("Splitted", "IsSplitted");
            ObjDic.Add("Balance", "IsBalance");
            ObjDic.Add("Comment", "Comment");
            ObjDic.Add("DataFileStrategyID", "DataFileStrategyID");

            string statusValue = "";
            try
            {
                statusValue = ObjDic[strColumnName];
            }
            catch (Exception ex)
            {
                return "";
            }
            return statusValue;
        }
    }
}