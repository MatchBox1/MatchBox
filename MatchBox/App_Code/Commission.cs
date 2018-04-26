using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MatchBox.App_Code
{
    public class Commission
    {
        public static void PaymentDate(string txtDate, ref DateTime? strDateFrom, ref DateTime? strDateTo, ref string s_error)
        {
            s_error = string.Empty;
            try
            {
                string transactionDate = txtDate.Trim();

                if (transactionDate != "")
                {
                    bool isPlus = transactionDate.Contains("+");
                    if (isPlus)
                    {
                        string[] transactionDateSplit = transactionDate.Split('+');
                        if (transactionDateSplit[0] != "" && transactionDateSplit[1] != "")
                        {
                            strDateFrom = DateTime.ParseExact(transactionDateSplit[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            strDateTo = DateTime.ParseExact(transactionDateSplit[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else if (transactionDateSplit[0] == "")
                        {
                            strDateFrom = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            strDateTo = DateTime.ParseExact(transactionDateSplit[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            strDateFrom = DateTime.ParseExact(transactionDateSplit[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            strDateTo = DateTime.Now;
                        }
                    }
                    else
                    {
                        strDateFrom = DateTime.ParseExact(transactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        strDateTo = DateTime.ParseExact(transactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    strDateFrom = null; //Convert.ToDateTime("01/01/1900");
                    strDateTo = null; // Convert.ToDateTime(DateTime.Now.ToShortDateString());
                }
            }
            catch (Exception ex)
            {
                //s_error = ex.Message;
                s_error = "Error on validating date.";
            }
        }
    }

    public class CommissionFilter
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CreditId { get; set; }
        public int CommissionTypeId { get; set; }
        public DateTime? PaymentDateFrom { get; set; }
        public DateTime? PaymentDateTo { get; set; }
        public bool Valid { get; set; }

        public int UserId { get; set; }
    }

    public class DataActionCommission
    {

        public static int SaveCommissionData(CommissionFilter filter, ref string s_error)
        {
            int n_rows_affected = 0;

            SqlCommand o_command = new SqlCommand("sprSaveCommission", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = filter.Id });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = filter.UserId });
            o_command.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.Int) { Value = filter.CompanyId });
            o_command.Parameters.Add(new SqlParameter("@CreditId", SqlDbType.Int) { Value = filter.CreditId });
            o_command.Parameters.Add(new SqlParameter("@CommissionTypeId", SqlDbType.Int) { Value = filter.CommissionTypeId });
            o_command.Parameters.Add(new SqlParameter("@PaymentDateFrom", SqlDbType.DateTime) { Value = filter.PaymentDateFrom });
            o_command.Parameters.Add(new SqlParameter("@PaymentDateTo", SqlDbType.DateTime) { Value = filter.PaymentDateTo });
            o_command.Parameters.Add(new SqlParameter("@Valid", SqlDbType.Bit) { Value = filter.Valid });

            //SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            //DataSet o_data_set = new DataSet();

            try
            {
                o_command.Connection.Open();
                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //s_error = ex.Message;
                s_error = "Error on saving commission data.";
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