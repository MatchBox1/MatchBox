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
            ObjDic.Add("ח.פ. של החברה", "CompanyNumber");

            ObjDic.Add("Network number", "NetworkNumber");
            ObjDic.Add("מספר רשת", "NetworkNumber");

            ObjDic.Add("branch number", "BranchNumber");
            ObjDic.Add("מספר סניף", "BranchNumber");

            ObjDic.Add("POSITION NUMBER", "CashBoxNumber");
            ObjDic.Add("מספר קופה", "CashBoxNumber");

            ObjDic.Add("terminal number", "TerminalNumber");
            ObjDic.Add("מספר מסוף", "TerminalNumber");

            ObjDic.Add("provider number", "SupplierNumber");
            ObjDic.Add("מספר ספק", "SupplierNumber");

            ObjDic.Add("Network master number", "SupplierGroupNumber");
            ObjDic.Add("מספר אב רשת", "SupplierGroupNumber");

            ObjDic.Add("credit Company", "CreditBrand");
            ObjDic.Add("חברת האשראי", "CreditBrand");

            ObjDic.Add("Brand", "CardBrand");
            ObjDic.Add("מותג", "CardBrand");

            ObjDic.Add("The type of action", "OperationType");
            ObjDic.Add("סוג הפעולה", "OperationType");

            ObjDic.Add("Currency Transaction", "TransactionCurrency");
            ObjDic.Add("מטבע עסקה", "TransactionCurrency");

            ObjDic.Add("Card prefix", "CardPrefix");
            ObjDic.Add("קידומת כרטיס", "CardPrefix");

            ObjDic.Add("Ticket Number", "CardNumber");
            ObjDic.Add("מספר כרטיס", "CardNumber");

            ObjDic.Add("Amount of payments", "PaymentsCount");
            ObjDic.Add("כמות תשלומים", "PaymentsCount");

            ObjDic.Add("Fixed payment number", "DutyPaymentNumber");
            ObjDic.Add("מספר תשלום תורן", "DutyPaymentNumber");

            ObjDic.Add("Gross amount", "TransactionGrossAmount");
            ObjDic.Add("סכום ברוטו", "TransactionGrossAmount");

            ObjDic.Add("First payment amount", "FirstPaymentAmount");
            ObjDic.Add("סכום תשלום ראשון", "FirstPaymentAmount");

            ObjDic.Add("The payment amount is outstanding", "DutyPaymentAmount");
            ObjDic.Add("סכום תשלום תורן", "DutyPaymentAmount");

            ObjDic.Add("Amount of other payments", "RemainingPaymentsAmount");
            ObjDic.Add("סכום יתר התשלומים", "RemainingPaymentsAmount");

            ObjDic.Add("Date of transaction execution", "TransactionDate");
            ObjDic.Add("תאריך ביצוע עסקה", "TransactionDate");

            ObjDic.Add("Transaction Transaction Date", "TransmissionDate");
            ObjDic.Add("תאריך שידור עסקה", "TransmissionDate");

            ObjDic.Add("Credit Date -Date", "PaymentDate");
            ObjDic.Add("תאריך זיכוי - מועד", "PaymentDate");

            ObjDic.Add("Reception number", "ReceiptNumber");
            ObjDic.Add("מספר קבלה", "ReceiptNumber");

            ObjDic.Add("Broadcast number", "TransmissionNumber");
            ObjDic.Add("מספר שידור", "TransmissionNumber");

            ObjDic.Add("voucher number", "VoucherNumber");
            ObjDic.Add("מספר שובר", "VoucherNumber");

            ObjDic.Add("confirmation number", "ConfirmationNumber");
            ObjDic.Add("מספר אישור", "ConfirmationNumber");

            ObjDic.Add("Splitted", "IsSplitted");
            ObjDic.Add("Balance", "IsBalance");
            ObjDic.Add("Comment", "Comment");
            ObjDic.Add("DataFileStrategyID", "DataFileStrategyID");


            //ObjDic.Add("עמלת ביטול", "Cancellationcommission");
            //ObjDic.Add("הנחת מועדון", "clubsaving");
            //ObjDic.Add("עמלת ניהול מועדון","clubmanagementcommission");
            //ObjDic.Add("עמלת נכיון", "discountcommission");
            //ObjDic.Add("עמלה טלפונית", "telephonecommission");
            //ObjDic.Add("עמלה ידנית", "manualcommission");
            //ObjDic.Add("עמלה ידנית", "manualcommission");
            //ObjDic.Add("עמלה שאינה אלקטרונית", "notelectroniccommission");
            //ObjDic.Add("עמלת סליקה", "clearingcommission");
            //ObjDic.Add("נטו לתשלום", "netpaymentamount");
            //ObjDic.Add("מע\"מ","vat");

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