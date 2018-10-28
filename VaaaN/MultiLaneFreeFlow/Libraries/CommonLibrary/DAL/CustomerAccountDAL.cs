using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class CustomerAccountDAL
    {
        static string tableName = "TBL_CUSTOMER_ACCOUNT";
        public CustomerAccountDAL()
        {
        }

        #region Insert/Update
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account)
        {
            int entryId = 0;

            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                if (entryId <= 0)
                {
                    entryId = GetNextValue();
                }
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, account.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_id", DbType.Int32, entryId, ParameterDirection.InputOutput));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_name", DbType.String, account.FirstName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_name", DbType.String, account.LastName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_mob_number", DbType.String, account.MobileNo, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_email_id", DbType.String, account.EmailId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, account.Description, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_address", DbType.String, account.Address, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_balance", DbType.Decimal, account.AccountBalance, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, account.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_image_path", DbType.String, account.CustomerImagePath, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_scanned_docs_path", DbType.String, account.ScannedDocsPath, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_doc_verified", DbType.Int32, account.IsDocVerified, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_status", DbType.Int32, account.AccountStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, account.TransferStatus, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entryId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, account.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_id", DbType.Int32, account.AccountId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_name", DbType.String, account.FirstName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_name", DbType.String, account.LastName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_mob_number", DbType.String, account.MobileNo, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_email_id", DbType.String, account.EmailId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, account.Description, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_address", DbType.String, account.Address, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_balance", DbType.Decimal, account.AccountBalance, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, account.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, account.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, account.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_image_path", DbType.String, account.CustomerImagePath, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_scanned_docs_path", DbType.String, account.ScannedDocsPath, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_doc_verified", DbType.Int32, account.IsDocVerified, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_status", DbType.Int32, account.AccountStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, account.TransferStatus, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static Int32 UpdateBalance(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account, Decimal DeductionAmount)
        //{
        //    int successCode = 0;
        //    try
        //    {
        //        string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_UPDATE_BALANCE";
        //        DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, account.TmsId, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_id", DbType.Int32, account.AccountId, ParameterDirection.Input));

        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_deduction_amount", DbType.Decimal, DeductionAmount, ParameterDirection.Input));

        //        VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

        //        successCode = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        successCode = 0;
        //        throw ex;

        //    }

        //    return successCode;
        //}

        #endregion

        #region Update Customer Account
        public static void UpdateBalance(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account, Decimal amount)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_BALANCEUPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, account.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_id", DbType.Int32, account.AccountId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_amount", DbType.Decimal, amount, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Methods
        private static int GetNextValue()
        {
            //next value will be 1 if there is no row in the datatable.
            int nextValue = 1;

            try
            {
                //Get object collection
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection objs = GetAllAsCollection();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].AccountId;
                }

                //Sort the object id
                Array.Sort(sortedObjsId);

                for (int j = 0; j < sortedObjsId.Length; j++)
                {
                    if (j + 1 < sortedObjsId.Length)
                    {
                        if (sortedObjsId[j] + 1 < sortedObjsId[j + 1])
                        {
                            nextValue = sortedObjsId[j] + 1;
                            break;
                        }
                    }
                    else
                    {
                        nextValue = sortedObjsId[sortedObjsId.Length - 1] + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return nextValue;
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetByMobileNumber(string mobileNumber)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection accounts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_GETBYMOBILENO";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_mob_number", DbType.String, mobileNumber, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                accounts = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return accounts;
        }

        public static CBE.CustomerAccountCBE GetCustomerById(CBE.CustomerAccountCBE customer)
        {
            try
            {

                CBE.CustomerAccountCollection customers = new CBE.CustomerAccountCollection();

                string spName = Constants.oraclePackagePrefix + "ACCOUNT_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, customer.AccountId, ParameterDirection.Input));
                customers = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return customers[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static CBE.UserCBE GetUserById(CBE.UserCBE user)
        //{
        //    try
        //    {

        //        CBE.UserCollection users = new CBE.UserCollection();

        //        string spName = Constants.oraclePackagePrefix + "User_GetById";
        //        DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, user.UserId, ParameterDirection.Input));

        //        users = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

        //        return users[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetById(Int32 CustomerId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection customerAccounts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_GETBYID";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.String, CustomerId, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                customerAccounts = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customerAccounts;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetAllAsCollection()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection accounts = new CBE.CustomerAccountCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_GETALL";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                accounts = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return accounts;
        }

        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> GetAllAsList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> accounts = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_GETALL";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                accounts = ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return accounts;
        }

      
        #endregion

        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection accounts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        account.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["ACCOUNT_ID"] != DBNull.Value)
                        account.AccountId = Convert.ToInt32(dt.Rows[i]["ACCOUNT_ID"]);

                    if (dt.Rows[i]["FIRST_NAME"] != DBNull.Value)
                        account.FirstName = Convert.ToString(dt.Rows[i]["FIRST_NAME"]);

                    if (dt.Rows[i]["LAST_NAME"] != DBNull.Value)
                        account.LastName = Convert.ToString(dt.Rows[i]["LAST_NAME"]);

                    if (dt.Rows[i]["MOB_NUMBER"] != DBNull.Value)
                        account.MobileNo = Convert.ToString(dt.Rows[i]["MOB_NUMBER"]);

                    if (dt.Rows[i]["EMAIL_ID"] != DBNull.Value)
                        account.EmailId = Convert.ToString(dt.Rows[i]["EMAIL_ID"]);

                    if (dt.Rows[i]["DESCRIPTION"] != DBNull.Value)
                        account.Description = Convert.ToString(dt.Rows[i]["DESCRIPTION"]);

                    if (dt.Rows[i]["ADDRESS"] != DBNull.Value)
                        account.Address = Convert.ToString(dt.Rows[i]["ADDRESS"]);

                    if (dt.Rows[i]["ACCOUNT_BALANCE"] != DBNull.Value)
                        account.AccountBalance = Convert.ToDecimal(dt.Rows[i]["ACCOUNT_BALANCE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        account.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        account.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        account.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    if (dt.Rows[i]["CUSTOMER_IMAGE_PATH"] != DBNull.Value)
                        account.CustomerImagePath = Convert.ToString(dt.Rows[i]["CUSTOMER_IMAGE_PATH"]);

                    if (dt.Rows[i]["SCANNED_DOCS_PATH"] != DBNull.Value)
                        account.ScannedDocsPath = Convert.ToString(dt.Rows[i]["SCANNED_DOCS_PATH"]);

                    if (dt.Rows[i]["IS_DOC_VERIFIED"] != DBNull.Value)
                        account.IsDocVerified = Convert.ToInt32(dt.Rows[i]["IS_DOC_VERIFIED"]);

                    if (dt.Rows[i]["ACCOUNT_STATUS"] != DBNull.Value)
                        account.AccountStatus = Convert.ToInt32(dt.Rows[i]["ACCOUNT_STATUS"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        account.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    accounts.Add(account);
                }
                return accounts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> ConvertDataTableToList(DataTable dt)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection customerAccounts = ConvertDataTableToCollection(dt);

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerAccountList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE ca in customerAccounts)
            {
                customerAccountList.Add(ca);
            }

            return customerAccountList;
        }
        #endregion
    }
}
