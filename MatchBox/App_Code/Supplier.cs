using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class SupplierModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgreementID { get; set; }
        public int SupplierGroupID { get; set; }
        public long SupplierNumber { get; set; }

        public string SupplierName { get; set; }
        public string ErrorMessage { get; set; }

        public SupplierModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            AgreementID = 0;
            SupplierGroupID = 0;
            SupplierNumber = 0;

            SupplierName = "";
            ErrorMessage = "";
        }
    }

    public class SupplierAction
    {
        public static void Select(ref DataTable o_data_table, ref string s_error, int n_user_id, int n_agreement_id, string s_order_by = "")
        {
            SqlCommand o_command = new SqlCommand("sprSupplier", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = n_agreement_id;

            if (s_order_by != "")
            {
                o_command.Parameters.Add("@OrderBy", SqlDbType.VarChar, 50);
                o_command.Parameters["@OrderBy"].Value = s_order_by;
            }

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);
        }

        public static void Update(ref SupplierModel o_supplier)
        {
            bool b_exists = SupplierNumber_Exists(ref o_supplier);

            if (o_supplier.ErrorMessage != "") { return; }

            if (b_exists)
            {
                o_supplier.ErrorMessage = "Supplier number already exists.";
                return;
            }

            SqlCommand o_command = new SqlCommand("sprSupplierInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_supplier.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_supplier.AgreementID;

            o_command.Parameters.Add("@SupplierGroupID", SqlDbType.Int);
            o_command.Parameters["@SupplierGroupID"].Value = o_supplier.SupplierGroupID;

            o_command.Parameters.Add("@SupplierNumber", SqlDbType.BigInt);
            o_command.Parameters["@SupplierNumber"].Value = o_supplier.SupplierNumber;

            o_command.Parameters.Add("@SupplierName", SqlDbType.NVarChar, 20);
            o_command.Parameters["@SupplierName"].Value = o_supplier.SupplierName;

            try
            {
                o_command.Connection.Open();

                o_supplier.ID = (int)o_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                o_supplier.ErrorMessage = ex.Message;
                o_supplier.ErrorMessage = "Error on update Supplier.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Table(int n_user_id, int n_agreement_id, DataTable dt_supplier, ref int n_rows_affected, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprSupplierInsertTable", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = n_agreement_id;

            o_command.Parameters.AddWithValue("@TableSupplier", dt_supplier);
            o_command.Parameters["@TableSupplier"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update Supplier Table.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        private static bool SupplierNumber_Exists(ref SupplierModel o_supplier)
        {
            bool b_exists = false;

            SqlCommand o_command = new SqlCommand("sprSupplierCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_supplier.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_supplier.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_supplier.AgreementID;

            o_command.Parameters.Add("@SupplierNumber", SqlDbType.BigInt);
            o_command.Parameters["@SupplierNumber"].Value = o_supplier.SupplierNumber;

            try
            {
                o_command.Connection.Open();
                b_exists = (int)o_command.ExecuteScalar() > 0;
            }
            catch (Exception ex)
            {
                o_supplier.ErrorMessage = ex.Message;
                o_supplier.ErrorMessage = "Error on validate supplier number.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_exists;
        }

        public static bool Delete(SupplierModel o_supplier, ref string s_error)
        {
            bool b_deleted = false;

            SqlCommand o_command = new SqlCommand("sprSupplierDelete", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_supplier.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_supplier.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_supplier.AgreementID;

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete Supplier.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_deleted;
        }
    }
}
