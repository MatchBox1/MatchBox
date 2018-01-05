using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MatchBox
{
    public class SupplierGroupModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgreementID { get; set; }
        public long SupplierGroupNumber { get; set; }

        public string SupplierGroupName { get; set; }
        public string ErrorMessage { get; set; }

        public SupplierGroupModel()
        {
            Reset();
        }

        public void Reset()
        {
            ID = 0;
            UserID = 0;
            AgreementID = 0;
            SupplierGroupNumber = 0;

            SupplierGroupName = "";
            ErrorMessage = "";
        }
    }

    public class SupplierGroupAction
    {
        public static void Select(ref DataTable o_data_table, ref string s_error, int n_user_id, int n_agreement_id)
        {
            SqlCommand o_command = new SqlCommand("sprSupplierGroup", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = n_agreement_id;

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);
        }

        public static void Update(ref SupplierGroupModel o_supplier_group)
        {
            bool b_exists = SupplierGroupNumber_Exists(ref o_supplier_group);

            if (o_supplier_group.ErrorMessage != "") { return; }

            if (b_exists)
            {
                o_supplier_group.ErrorMessage = "Supplier group number already exists.";
                return;
            }

            SqlCommand o_command = new SqlCommand("sprSupplierGroupInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_supplier_group.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_supplier_group.AgreementID;

            o_command.Parameters.Add("@SupplierGroupNumber", SqlDbType.BigInt);
            o_command.Parameters["@SupplierGroupNumber"].Value = o_supplier_group.SupplierGroupNumber;

            o_command.Parameters.Add("@SupplierGroupName", SqlDbType.NVarChar, 20);
            o_command.Parameters["@SupplierGroupName"].Value = o_supplier_group.SupplierGroupName;

            try
            {
                o_command.Connection.Open();

                o_supplier_group.ID = (int)o_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                o_supplier_group.ErrorMessage = ex.Message;
                o_supplier_group.ErrorMessage = "Error on update Supplier Group.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        public static void Update_Table(int n_user_id, int n_agreement_id, DataTable dt_supplier_group, ref int n_rows_affected, ref string s_error)
        {
            SqlCommand o_command = new SqlCommand("sprSupplierGroupInsertTable", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = n_user_id;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = n_agreement_id;

            o_command.Parameters.AddWithValue("@TableSupplierGroup", dt_supplier_group);
            o_command.Parameters["@TableSupplierGroup"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                n_rows_affected = o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on update Supplier Group Table.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }
        }

        private static bool SupplierGroupNumber_Exists(ref SupplierGroupModel o_supplier_group)
        {
            bool b_exists = false;

            SqlCommand o_command = new SqlCommand("sprSupplierGroupCheck", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_supplier_group.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_supplier_group.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_supplier_group.AgreementID;

            o_command.Parameters.Add("@SupplierGroupNumber", SqlDbType.BigInt);
            o_command.Parameters["@SupplierGroupNumber"].Value = o_supplier_group.SupplierGroupNumber;

            try
            {
                o_command.Connection.Open();
                b_exists = (int)o_command.ExecuteScalar() > 0;
            }
            catch (Exception ex)
            {
                o_supplier_group.ErrorMessage = ex.Message;
                o_supplier_group.ErrorMessage = "Error on validate supplier group number.";
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            return b_exists;
        }

        public static bool Delete(SupplierGroupModel o_supplier_group, ref string s_error)
        {
            bool b_deleted = false;

            SqlCommand o_command = new SqlCommand("sprSupplierGroupDelete", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Value = o_supplier_group.ID;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_supplier_group.UserID;

            o_command.Parameters.Add("@AgreementID", SqlDbType.Int);
            o_command.Parameters["@AgreementID"].Value = o_supplier_group.AgreementID;

            try
            {
                o_command.Connection.Open();
                b_deleted = o_command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on delete supplier group.";
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
