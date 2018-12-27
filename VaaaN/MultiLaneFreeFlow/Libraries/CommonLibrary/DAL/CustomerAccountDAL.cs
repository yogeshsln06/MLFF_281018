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
                //if (entryId <= 0)
                //{
                //    entryId = GetNextValue();
                //}
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, account.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_id", DbType.Int32, entryId, ParameterDirection.Output));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_name", DbType.String, account.FirstName.ToUpper(), ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_name", DbType.String, account.LastName, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_mob_number", DbType.String, account.MobileNo, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_email_id", DbType.String, account.EmailId, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, account.Description, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_address", DbType.String, account.Address, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_balance", DbType.Decimal, account.AccountBalance, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, account.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_image_path", DbType.String, account.CustomerImagePath, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_doc_verified", DbType.Int32, account.IsDocVerified, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_status", DbType.Int32, account.AccountStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, account.TransferStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_resident_id", DbType.String, account.ResidentId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_birth_place", DbType.String, account.BirthPlace, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_birth_date", DbType.DateTime, account.BirthDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_rt_rw", DbType.String, account.RT_RW, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_province_id", DbType.Int32, account.ProvinceId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_city_id", DbType.Int32, account.CityId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_district_id", DbType.Int32, account.DistrictId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_sub_district_id", DbType.Int32, account.SubDistrictId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_postal_code", DbType.Int32, account.PostalCode, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_nationality", DbType.Int32, account.Nationality, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_gender", DbType.Int32, account.Gender, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_marital_status", DbType.Int32, account.MaritalStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_occupation", DbType.String, account.Occupation, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_residentidcardimagepath", DbType.String, account.ResidentidcardImagePath, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_valid_until", DbType.DateTime, account.ValidUntil, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_registration_through", DbType.Int32, account.RegistartionThrough, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_userpassword", DbType.String, account.UserPassword, ParameterDirection.Input, 255));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                entryId = Convert.ToInt32(command.Parameters["p_account_id"].Value);
            }
            catch (Exception ex)
            {
                entryId = 0;
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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_id", DbType.Int32, account.AccountId, ParameterDirection.InputOutput));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_name", DbType.String, account.FirstName.ToUpper(), ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_name", DbType.String, account.LastName, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_mob_number", DbType.String, account.MobileNo, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_email_id", DbType.String, account.EmailId, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, account.Description, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_address", DbType.String, account.Address, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_balance", DbType.Decimal, account.AccountBalance, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, account.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, account.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_image_path", DbType.String, account.CustomerImagePath, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_doc_verified", DbType.Int32, account.IsDocVerified, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_status", DbType.Int32, account.AccountStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, account.TransferStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_resident_id", DbType.String, account.ResidentId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_birth_place", DbType.String, account.BirthPlace, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_birth_date", DbType.DateTime, account.BirthDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_rt_rw", DbType.String, account.RT_RW, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_province_id", DbType.Int32, account.ProvinceId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_city_id", DbType.Int32, account.CityId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_district_id", DbType.Int32, account.DistrictId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_sub_district_id", DbType.Int32, account.SubDistrictId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_postal_code", DbType.Int32, account.PostalCode, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_nationality", DbType.Int32, account.Nationality, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_gender", DbType.Int32, account.Gender, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_marital_status", DbType.Int32, account.MaritalStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_occupation", DbType.String, account.Occupation, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_residentidcardimagepath", DbType.String, account.ResidentidcardImagePath, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_valid_until", DbType.DateTime, account.ValidUntil, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, customer.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, customer.AccountId, ParameterDirection.Input));
                DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
                customers = ConvertDataTableToCollection(dt);
                return customers[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.CustomerAccountCBE GetCustomerByResidentId(CBE.CustomerAccountCBE customer)
        {
            try
            {

                CBE.CustomerAccountCollection customers = new CBE.CustomerAccountCollection();
                string spName = Constants.oraclePackagePrefix + "ACCOUNT_GETBY_RESIDENTID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, customer.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RESIDENT_ID", DbType.String, customer.ResidentId, ParameterDirection.Input));
                DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
                customers = ConvertDataTableToCollection(dt);
                return customers[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CBE.CustomerAccountCBE> ValidateCustomerAccount(CBE.CustomerAccountCBE customer)
        {
            try
            {

                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> accounts = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
                string spName = Constants.oraclePackagePrefix + "CUSTOMER_ACCOUNT_VALIDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MOB_NUMBER", DbType.String, customer.MobileNo, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RESIDENT_ID", DbType.String, customer.ResidentId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EMAIL_ID", DbType.String, customer.EmailId, ParameterDirection.Input));
                DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
                accounts = ConvertDataTableToList(dt);
                return accounts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CBE.CustomerAccountCBE> CustomerAccountLazyLoad(int PageIndex, int PageSize)
        {
            try
            {
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> accounts = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
                string spName = Constants.oraclePackagePrefix + "ACCOUNT_GETALL_LAZYLOAD";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.String, PageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.String, PageSize, ParameterDirection.Input));
                DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
                accounts = ConvertDataTableToListDirect(dt);
                return accounts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetById(Int32 CustomerId, Int32 tmsId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection customerAccounts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_GETBYID";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, CustomerId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, tmsId, ParameterDirection.Input));
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

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetLatestCustomerAccounts(DateTime lastCollectionUpdateTime)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection accounts = new CBE.CustomerAccountCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "ACCOUNT_LATEST_GETALL";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LAST_UPDATE_TIME", DbType.DateTime, lastCollectionUpdateTime, ParameterDirection.Input));

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
        #endregion

        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection accounts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection();

                foreach (DataRow dr in dt.Rows)
                {
                    accounts.Add(ConvertDataTableToCBE(dr));
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

            List<CBE.CustomerAccountCBE> customerAccountList = new List<CBE.CustomerAccountCBE>();

            foreach (DataRow dr in dt.Rows)
            {
                customerAccountList.Add(ConvertDataTableToCBE(dr));
            }

            return customerAccountList;
        }

        private static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> ConvertDataTableToListDirect(DataTable dt)
        {
            try
            {

                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerAccountList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();

                    if (dt.Rows[i]["ACCOUNT_ID"] != DBNull.Value)
                        account.AccountId = Convert.ToInt32(dt.Rows[i]["ACCOUNT_ID"]);

                    if (dt.Rows[i]["FIRST_NAME"] != DBNull.Value)
                        account.FirstName = Convert.ToString(dt.Rows[i]["FIRST_NAME"]);

                    if (dt.Rows[i]["MOB_NUMBER"] != DBNull.Value)
                        account.MobileNo = Convert.ToString(dt.Rows[i]["MOB_NUMBER"]);

                    if (dt.Rows[i]["EMAIL_ID"] != DBNull.Value)
                        account.EmailId = Convert.ToString(dt.Rows[i]["EMAIL_ID"]);

                    if (dt.Rows[i]["ADDRESS"] != DBNull.Value)
                        account.Address = Convert.ToString(dt.Rows[i]["ADDRESS"]);

                    if (dt.Rows[i]["CUSTOMER_IMAGE_PATH"] != DBNull.Value)
                        account.CustomerImagePath = Convert.ToString(dt.Rows[i]["CUSTOMER_IMAGE_PATH"]);


                    if (dt.Rows[i]["RESIDENT_ID"] != DBNull.Value)
                        account.ResidentId = Convert.ToString(dt.Rows[i]["RESIDENT_ID"]);

                    if (dt.Rows[i]["RESIDENTIDCARDIMAGE"] != DBNull.Value)
                        account.ResidentidcardImagePath = Convert.ToString(dt.Rows[i]["RESIDENTIDCARDIMAGE"]);
                    customerAccountList.Add(account);
                }
                return customerAccountList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static CBE.CustomerAccountCBE ConvertDataTableToCBE(DataRow row)
        {

            CBE.CustomerAccountCBE account = new CBE.CustomerAccountCBE();

            if (row["TMS_ID"] != DBNull.Value)
                account.TmsId = Convert.ToInt32(row["TMS_ID"]);

            if (row["ACCOUNT_ID"] != DBNull.Value)
                account.AccountId = Convert.ToInt32(row["ACCOUNT_ID"]);

            if (row["FIRST_NAME"] != DBNull.Value)
                account.FirstName = Convert.ToString(row["FIRST_NAME"]);

            if (row["LAST_NAME"] != DBNull.Value)
                account.LastName = Convert.ToString(row["LAST_NAME"]);

            if (row["MOB_NUMBER"] != DBNull.Value)
                account.MobileNo = Convert.ToString(row["MOB_NUMBER"]);

            if (row["EMAIL_ID"] != DBNull.Value)
                account.EmailId = Convert.ToString(row["EMAIL_ID"]);

            if (row["DESCRIPTION"] != DBNull.Value)
                account.Description = Convert.ToString(row["DESCRIPTION"]);

            if (row["ADDRESS"] != DBNull.Value)
            {
                account.Address = Convert.ToString(row["ADDRESS"]);
                account.FullAddress = Convert.ToString(row["ADDRESS"]);
            }

            if (row["ACCOUNT_BALANCE"] != DBNull.Value)
                account.AccountBalance = Convert.ToDecimal(row["ACCOUNT_BALANCE"]);

            if (row["MODIFIER_ID"] != DBNull.Value)
                account.ModifierId = Convert.ToInt32(row["MODIFIER_ID"]);

            if (row["CREATION_DATE"] != DBNull.Value)
                account.CreationDate = Convert.ToDateTime(row["CREATION_DATE"]);

            if (row["MODIFICATION_DATE"] != DBNull.Value)
                account.ModificationDate = Convert.ToDateTime(row["MODIFICATION_DATE"]);

            if (row["CUSTOMER_IMAGE_PATH"] != DBNull.Value)
                account.CustomerImagePath = Convert.ToString(row["CUSTOMER_IMAGE_PATH"]);

            if (row["IS_DOC_VERIFIED"] != DBNull.Value)
                account.IsDocVerified = Convert.ToInt32(row["IS_DOC_VERIFIED"]);

            if (row["ACCOUNT_STATUS"] != DBNull.Value)
                account.AccountStatus = Convert.ToInt32(row["ACCOUNT_STATUS"]);

            if (row["TRANSFER_STATUS"] != DBNull.Value)
                account.TransferStatus = Convert.ToInt32(row["TRANSFER_STATUS"]);

            if (row["RESIDENT_ID"] != DBNull.Value)
                account.ResidentId = Convert.ToString(row["RESIDENT_ID"]);

            if (row["BIRTH_PLACE"] != DBNull.Value)
                account.BirthPlace = Convert.ToString(row["BIRTH_PLACE"]);

            if (row["BIRTH_DATE"] != DBNull.Value)
                account.BirthDate = Convert.ToDateTime(row["BIRTH_DATE"]);

            if (row["RT_RW"] != DBNull.Value)
                account.RT_RW = Convert.ToString(row["RT_RW"]);

            if (row["RT_RW"] != DBNull.Value)
            {
                string RT_RW = Convert.ToString(row["RT_RW"]);
                if (RT_RW.Contains("/"))
                {
                    string[] RTRW = RT_RW.Split('/');
                    account.RT = RTRW[0].ToString();
                    if (RTRW.Length > 1)
                    {
                        account.RW = RTRW[1].ToString();
                    }
                }
            }

            if (row["PROVINCE_ID"] != DBNull.Value)
                account.ProvinceId = Convert.ToInt32(row["PROVINCE_ID"]);

            if (row["PROVINCE_NAME"] != DBNull.Value)
            {
                account.ProvinceName = Convert.ToString(row["PROVINCE_NAME"]);
                account.FullAddress = account.FullAddress + " " + Convert.ToString(row["PROVINCE_NAME"]);
            }

            if (row["CITY_ID"] != DBNull.Value)
                account.CityId = Convert.ToInt32(row["CITY_ID"]);

            if (row["CITY_NAME"] != DBNull.Value)
            {
                account.CityName = Convert.ToString(row["CITY_NAME"]);
                account.FullAddress = account.FullAddress + " " + Convert.ToString(row["CITY_NAME"]);
            }

            if (row["DISTRICT_ID"] != DBNull.Value)
                account.DistrictId = Convert.ToInt32(row["DISTRICT_ID"]);

            if (row["DISTRICT_NAME"] != DBNull.Value)
            {
                account.DistrictName = Convert.ToString(row["DISTRICT_NAME"]);
                account.FullAddress = account.FullAddress + " " + Convert.ToString(row["DISTRICT_NAME"]);
            }

            if (row["SUB_DISTRICT_ID"] != DBNull.Value)
                account.SubDistrictId = Convert.ToInt32(row["SUB_DISTRICT_ID"]);

            if (row["SUB_DISTRICT_NAME"] != DBNull.Value)
            {
                account.SubDistrictName = Convert.ToString(row["SUB_DISTRICT_NAME"]);
                account.FullAddress = account.FullAddress + " " + Convert.ToString(row["SUB_DISTRICT_NAME"]);
            }

            if (row["POSTAL_CODE"] != DBNull.Value)
            {
                account.PostalCode = Convert.ToInt32(row["POSTAL_CODE"]);
                account.FullAddress = account.FullAddress + " " + Convert.ToString(row["POSTAL_CODE"]);
            }

            if (row["NATIONALITY"] != DBNull.Value)
            {
                account.Nationality = Convert.ToInt32(row["NATIONALITY"]);
                if (Convert.ToInt32(row["NATIONALITY"]) > 0)
                    account.NationalityName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.NationalityName[Convert.ToInt32(row["NATIONALITY"]) - 1];
            }


            if (row["GENDER"] != DBNull.Value)
            {
                account.Gender = Convert.ToInt32(row["GENDER"]);
                if (Convert.ToInt32(row["GENDER"]) > 0)
                    account.GenderName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GenderName[Convert.ToInt32(row["GENDER"]) - 1];
            }


            if (row["MARITAL_STATUS"] != DBNull.Value)
            {
                account.MaritalStatus = Convert.ToInt32(row["MARITAL_STATUS"]);
                if (Convert.ToInt32(row["MARITAL_STATUS"]) > 0)
                    account.MaritalStatusName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MaritalStatusName[Convert.ToInt32(row["MARITAL_STATUS"]) - 1];
            }

            if (row["OCCUPATION"] != DBNull.Value)
                account.Occupation = Convert.ToString(row["OCCUPATION"]);

            if (row["RESIDENTIDCARDIMAGE"] != DBNull.Value)
                account.ResidentidcardImagePath = Convert.ToString(row["RESIDENTIDCARDIMAGE"]);

            if (row["VALID_UNTIL"] != DBNull.Value)
                account.ValidUntil = Convert.ToDateTime(row["VALID_UNTIL"]);

            if (row["REGISTRATION_THROUGH"] != DBNull.Value)
                account.RegistartionThrough = Convert.ToInt32(row["REGISTRATION_THROUGH"]);

            return account;
        }

        #endregion
    }
}
