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
            //ObjDic.Add("Status", "StatusName");
            ObjDic.Add("סטטוס", "StatusName");
            //ObjDic.Add("Source", "UniqueID");
            ObjDic.Add("מקור", "UniqueID");
            //ObjDic.Add("Template", "TemplateName");
            ObjDic.Add("תבנית", "TemplateName");
            //ObjDic.Add("File", "DataFileID");
            ObjDic.Add("קובץ", "DataFileID");
            //ObjDic.Add("Strategy", "StrategyName");
            ObjDic.Add("אסטרטגיה", "StrategyName");
            //ObjDic.Add("Matching", "MatchingID");
            ObjDic.Add("מס התאמה", "MatchingID");
            //ObjDic.Add("Action", "MatchingActionName");
            ObjDic.Add("פעולה", "MatchingActionName");
            //ObjDic.Add("Query", "QueryNumber");
            ObjDic.Add("שאילתא", "QueryNumber");
            //ObjDic.Add("Type", "MatchingTypeName");
            ObjDic.Add("סוג", "MatchingTypeName");
            //ObjDic.Add("Match", "QueryID");
            ObjDic.Add("התאמה", "QueryID");

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


            ObjDic.Add("סהכ כמות", "GrossAmountCount");

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
            //ObjDic.Add("Comment", "Comment");
            ObjDic.Add("הערות", "Comment");
            ObjDic.Add("DataFileStrategyID", "DataFileStrategyID");


            ObjDic.Add("עמלת ביטול", "Cancellationcommission");
            ObjDic.Add("הנחת מועדון", "clubsaving");
            ObjDic.Add("עמלת ניהול מועדון", "clubmanagementcommission");
            ObjDic.Add("עמלת נכיון", "discountcommission");
            ObjDic.Add("עמלה טלפונית", "telephonecommission");
            ObjDic.Add("עמלה ידנית", "manualcommission");
            ObjDic.Add("עמלה שאינה אלקטרונית", "notelectroniccommission");
            ObjDic.Add("עמלת סליקה", "clearingcommission");
            ObjDic.Add("נטו לתשלום", "netpaymentamount");
            ObjDic.Add("מע\"מ", "vat");
            ObjDic.Add("עסקת חו\"ל","IsAbroad");
            // New columns ///
            ObjDic.Add("אחוז עמלת סליקה הסכם", "AgPerClearingCommission");
             ObjDic.Add("עמלת סליקה הסכם", "CalculatedIclearingCommission");
             ObjDic.Add("תאריך עיבוד עמלת סליקה", "ClearinfCalculationDate");
            ObjDic.Add("אחוז עמלת סליקה בפועל", "AcPerClearingCommission");
            ObjDic.Add("הפרש עמלת סליקה", "DiffClearingCommission");
            ObjDic.Add("עמלת סליקה תקין?", "correctincorrectcommissions");
            ObjDic.Add("אחוז עמלת נכיון הסכם", "AgPerDiscountCommission");
            ObjDic.Add("עמלת נכיון הסכם", "CalculatedIDiscountCommission");
            ObjDic.Add("תאריך עיבוד עמלת נכיון", "DiscountCalculationDate");
            ObjDic.Add("אחוז עמלת נכיון בפועל", "AcPerDiscountCommission");
            ObjDic.Add("הפרש עמלת נכיון", "DiffDiscountCommission");
            ObjDic.Add("עמלת נכיון תקין?", "CorrectIncorrectCommissionsDiscount");
            ObjDic.Add("חברת נכיון", "DiscountName");
            ObjDic.Add("אחוז עמלת ד.נ מועדון הסכם", "AgPerClubManagementFeeCommission");
            ObjDic.Add("עמלת ד.נ מועדון הסכם", "CalculatedIclubManagementFeeCommission");
            ObjDic.Add("תאריך עיבוד עמלת ד.נ מועדון", "ClubManagementFeeCalculationDate");
            ObjDic.Add("אחוז עמלת ד.נ מועדון בפועל", "AcPerClubManagementFeeCommission");
            ObjDic.Add("הפרש עמלת ד.נ מועדון", "DiffClubManagementFeeCommission");
            ObjDic.Add("עמלת ד.נ מועדון תקין?", "CorrectIncorrectCommissionsClubManagementFee");
            ObjDic.Add("אחוז עמלת הנחת מועדון הסכם", "AgPerClubDiscountFeeCommission");
            ObjDic.Add("עמלת הנחת מועדון הסכם", "CalculatedIclubDiscountFeeCommission");
            ObjDic.Add("תאריך עיבוד עמלת הנחת מועדון", "ClubDiscountFeeCalculationDate");
            ObjDic.Add("אחוז עמלת הנחת מועדון בפועל", "AcPerClubDiscountFeeCommission");
            ObjDic.Add("הפרש עמלת הנחת מועדון", "DiffClubDiscountFeeCommission");
            ObjDic.Add("עמלת הנחת מועדון תקין?", "CorrectIncorrectCommissionsClubDiscountFee");
            ObjDic.Add("עסקת מועדון?", "IsClubCommissionvalid");

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