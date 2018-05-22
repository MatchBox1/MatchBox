using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

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
        public string CreditType { get; set; }

        public int UserId { get; set; }

        public int AllOutsideCount { get; set; }
        public double AllOutsideAmount { get; set; }

        public DataTable TableOutsideSum { get; set; }
        public DataTable TableOutsideSumTotal { get; set; }
        public string Reprocess { get; set; }
    }

    public class DataActionCommission
    {

        public static void Bind_List_Control_Commission(string s_procedure, ref string s_error, ref DataTable dtOverlappingPeriod, List<SqlParameter> o_additional_parameters = null)
        {
            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (o_additional_parameters != null)
            {
                foreach (SqlParameter o_parameter in o_additional_parameters)
                {
                    o_command.Parameters.Add(o_parameter);
                }
            }

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.Fill(o_data_set);
                //if (o_data_set.Tables[0].Rows.Count > 0)
                //{
                //    if (o_data_set.Tables[0].Rows[0][0].ToString() != "period")
                //    {
                //        o_list.DataSource = o_data_set.Tables[0];
                //        o_list.DataBind();
                //        o_list.DataTextField = "CommissionName";
                //        o_list.DataValueField = "ID";
                //        o_list.DataBind();
                //    }
                //    else
                //    {

                //    }
                //}

                dtOverlappingPeriod = o_data_set.Tables[0];
            }
            catch (Exception ex)
            {
                //s_error = ex.Message;
                s_error = "Error on bind list control.";
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void FetchCommissionData(ref CommissionFilter o_matching_search, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprFetchCommissionData", DB.Get_Connection());
            //SqlCommand o_command = new SqlCommand("sprCommissionSearchOut_Result", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_matching_search.UserId;

            // outside
            if (o_matching_search.PaymentDateFrom != null)
            {
                o_command.Parameters.Add("@TransactionDateFrom", SqlDbType.Date);
                o_command.Parameters["@TransactionDateFrom"].Value = o_matching_search.PaymentDateFrom;
            }
            if (o_matching_search.PaymentDateTo != null)
            {
                o_command.Parameters.Add("@TransactionDateTo", SqlDbType.Date);
                o_command.Parameters["@TransactionDateTo"].Value = o_matching_search.PaymentDateTo;
            }

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = o_matching_search.CompanyId;

            o_command.Parameters.Add("@CreditID", SqlDbType.Int);
            o_command.Parameters["@CreditID"].Value = o_matching_search.CreditId;

            o_command.Parameters.Add("@CreditType", SqlDbType.VarChar);
            o_command.Parameters["@CreditType"].Value = o_matching_search.CreditType;

            o_command.Parameters.Add("@CommissionTypeID", SqlDbType.Int);
            o_command.Parameters["@CommissionTypeID"].Value = o_matching_search.CommissionTypeId;

            o_command.Parameters.Add("@ReprocessValue", SqlDbType.VarChar);
            o_command.Parameters["@ReprocessValue"].Value = o_matching_search.Reprocess;

            ////
            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.SelectCommand.CommandTimeout = 200000;
                o_data_adapter.Fill(o_data_set);

                o_matching_search.TableOutsideSum = o_data_set.Tables[0];
                //o_matching_search.TableOutsideSumTotal = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                s_error = "Error on select matching search.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static void SearchCommissionData(ref CommissionFilter o_matching_search, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprCommissionSearchOut", DB.Get_Connection());
            //SqlCommand o_command = new SqlCommand("sprCommissionSearchOut_Result", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_matching_search.UserId;

            // outside
            if (o_matching_search.PaymentDateFrom != null)
            {
                o_command.Parameters.Add("@TransactionDateFrom", SqlDbType.Date);
                o_command.Parameters["@TransactionDateFrom"].Value = o_matching_search.PaymentDateFrom;
            }
            if (o_matching_search.PaymentDateTo != null)
            {
                o_command.Parameters.Add("@TransactionDateTo", SqlDbType.Date);
                o_command.Parameters["@TransactionDateTo"].Value = o_matching_search.PaymentDateTo;
            }

            o_command.Parameters.Add("@CompanyID", SqlDbType.Int);
            o_command.Parameters["@CompanyID"].Value = o_matching_search.CompanyId;

            o_command.Parameters.Add("@CreditID", SqlDbType.Int);
            o_command.Parameters["@CreditID"].Value = o_matching_search.CreditId;

            o_command.Parameters.Add("@CreditType", SqlDbType.VarChar);
            o_command.Parameters["@CreditType"].Value = o_matching_search.CreditType;

            o_command.Parameters.Add("@CommissionID", SqlDbType.Int);
            o_command.Parameters["@CommissionID"].Value = o_matching_search.CommissionTypeId;

            ////
            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);
            DataSet o_data_set = new DataSet();

            try
            {
                o_data_adapter.SelectCommand.CommandTimeout = 200000;
                o_data_adapter.Fill(o_data_set);

                //o_matching_search.TableInsideSum = o_data_set.Tables[0];
                o_matching_search.TableOutsideSum = o_data_set.Tables[0];
                //o_matching_search.TableInsideSumTotal = o_data_set.Tables[2];
                o_matching_search.TableOutsideSumTotal = o_data_set.Tables[1];
            }
            catch (Exception ex)
            {
                //o_matching_search.ErrorMessage = ex.Message;
                s_error = "Error on select matching search.";
                return;
            }
            finally
            {
                o_data_set.Dispose();
                o_data_adapter.Dispose();
                o_command.Dispose();
            }
        }

        public static int SaveCommissionData(CommissionFilter filter, ref string s_error)
        {
            int n_rows_affected = 0;

            SqlCommand o_command = new SqlCommand("sprSaveCommission", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = filter.Id });
            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = filter.UserId });
            o_command.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.Int) { Value = filter.CompanyId });
            o_command.Parameters.Add(new SqlParameter("@CreditId", SqlDbType.Int) { Value = filter.CreditId });
            o_command.Parameters.Add(new SqlParameter("@CommissionTypeId", SqlDbType.Int) { Value = filter.CommissionTypeId });
            o_command.Parameters.Add(new SqlParameter("@TransactionDateFrom", SqlDbType.DateTime) { Value = filter.PaymentDateFrom });
            o_command.Parameters.Add(new SqlParameter("@TransactionDateTo", SqlDbType.DateTime) { Value = filter.PaymentDateTo });

            o_command.Parameters.Add(new SqlParameter("@AllOutsideCount", SqlDbType.Int) { Value = filter.AllOutsideCount });
            o_command.Parameters.Add(new SqlParameter("@AllOutsideAmount", SqlDbType.Decimal) { Value = filter.AllOutsideAmount });

            o_command.Parameters.Add(new SqlParameter("@Valid", SqlDbType.Bit) { Value = filter.Valid });
            o_command.Parameters.Add(new SqlParameter("@CreditType", SqlDbType.NVarChar) { Value = filter.CreditType });

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