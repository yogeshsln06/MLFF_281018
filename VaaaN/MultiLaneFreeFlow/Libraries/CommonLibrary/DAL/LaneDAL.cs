using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class LaneDAL
    {
        static string tableName = "TBL_LANE";
        public LaneDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane)
        {
            int laneId = lane.LaneId;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "LANE_INSERT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                if (laneId <= 0)
                {
                    laneId = GetNextValue();
                }
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, lane.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, lane.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, laneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_TYPE_ID", DbType.Int32, lane.LaneTypeId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_NAME", DbType.String, lane.LaneName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_ID_FRONT", DbType.Int32, lane.CameraIdFront, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_ID_REAR", DbType.Int32, lane.CameraIdRear, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ETC_ANTENNA_ID_FRONT", DbType.Int32, lane.AntennaIdFront, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ETC_ANTENNA_ID_REAR", DbType.Int32, lane.AntennaIdRear, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, lane.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, lane.TransferStatus, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return laneId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "LANE_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, lane.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, lane.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, lane.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_TYPE_ID", DbType.Int32, lane.LaneTypeId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_NAME", DbType.String, lane.LaneName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_ID_FRONT", DbType.Int32, lane.CameraIdFront, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_ID_REAR", DbType.Int32, lane.CameraIdRear, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ETC_ANTENNA_ID_FRONT", DbType.Int32, lane.AntennaIdFront, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ETC_ANTENNA_ID_REAR", DbType.Int32, lane.AntennaIdRear, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIED_BY", DbType.Int32, lane.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, lane.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, lane.TransferStatus, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection GetAllAsCollection()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection lanes = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "LANE_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                lanes = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lanes;
        }

        public static CBE.LaneCollection GetLaneByPlazaId(CBE.LaneCBE lane)
        {
            CBE.LaneCollection lanes = new CBE.LaneCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "LANE_GETBYPLAZAID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, lane.PlazaId, ParameterDirection.Input));
                lanes = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lanes;
        }

        public static CBE.LaneCBE GetLaneById(CBE.LaneCBE lane)
        {
            CBE.LaneCollection lanes = new CBE.LaneCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "LANE_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, lane.LaneId, ParameterDirection.Input));
                lanes = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return lanes[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }


        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> GetAllAsList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> lanes = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE>();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "LANE_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                lanes = ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lanes;
        }

        /// <summary>
        /// Get next value for the primary key
        /// </summary>
        /// <returns></returns>
        private static int GetNextValue()
        {
            //next value will be 1 if there is no row in the datatable.
            int nextValue = 1;

            try
            {
                //Get object collection
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection objs = GetAllAsCollection();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].LaneId;
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
        #endregion

        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection lanes = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        lane.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["PLAZA_ID"] != DBNull.Value)
                        lane.PlazaId = Convert.ToInt32(dt.Rows[i]["PLAZA_ID"]);

                    if (dt.Rows[i]["PLAZA_NAME"] != DBNull.Value)
                        lane.PlazaName = Convert.ToString(dt.Rows[i]["PLAZA_NAME"]);

                    if (dt.Rows[i]["LANE_ID"] != DBNull.Value)
                        lane.LaneId = Convert.ToInt32(dt.Rows[i]["LANE_ID"]);

                    if (dt.Rows[i]["LANE_TYPE_ID"] != DBNull.Value)
                        lane.LaneTypeId = Convert.ToInt32(dt.Rows[i]["LANE_TYPE_ID"]);

                    if (dt.Rows[i]["LANE_NAME"] != DBNull.Value)
                        lane.LaneName = Convert.ToString(dt.Rows[i]["LANE_NAME"]);

                    if (dt.Rows[i]["CAMERA_ID_FRONT"] != DBNull.Value)
                        lane.CameraIdFront = Convert.ToInt32(dt.Rows[i]["CAMERA_ID_FRONT"]);

                    if (dt.Rows[i]["CAMERA_NAME_FRONT"] != DBNull.Value)
                        lane.CameraNameFront = Convert.ToString(dt.Rows[i]["CAMERA_NAME_FRONT"]);

                    if (dt.Rows[i]["CAMERA_ID_REAR"] != DBNull.Value)
                        lane.CameraIdRear = Convert.ToInt32(dt.Rows[i]["CAMERA_ID_REAR"]);

                    if (dt.Rows[i]["CAMERA_NAME_REAR"] != DBNull.Value)
                        lane.CameraNameRear = Convert.ToString(dt.Rows[i]["CAMERA_NAME_REAR"]);

                    if (dt.Rows[i]["ETC_ANTENNA_ID_FRONT"] != DBNull.Value)
                        lane.AntennaIdFront = Convert.ToInt32(dt.Rows[i]["ETC_ANTENNA_ID_FRONT"]);

                    if (dt.Rows[i]["ETC_ANTENNA_ID_REAR"] != DBNull.Value)
                        lane.AntennaIdRear = Convert.ToInt32(dt.Rows[i]["ETC_ANTENNA_ID_REAR"]);

                    if (dt.Rows[i]["ETC_ANTENNA_NAME_FRONT"] != DBNull.Value)
                        lane.EtcAntennaNameFront = Convert.ToString(dt.Rows[i]["ETC_ANTENNA_NAME_FRONT"]);

                    if (dt.Rows[i]["ETC_ANTENNA_NAME_REAR"] != DBNull.Value)
                        lane.EtcAntennaNameRear = Convert.ToString(dt.Rows[i]["ETC_ANTENNA_NAME_REAR"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        lane.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        lane.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    if (dt.Rows[i]["MODIFIED_BY"] != DBNull.Value)
                        lane.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIED_BY"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        lane.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    lanes.Add(lane);
                }
                return lanes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> ConvertDataTableToList(DataTable dt)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection lanes = ConvertDataTableToCollection(dt);

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> laneList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE>();

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane in lanes)
            {
                laneList.Add(lane);
            }

            return laneList;
        }
        #endregion
    }
}
