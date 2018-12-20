using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class CrossTalkDAL
    {
        static string tableName = "TBL_CROSSTALK_PACKET";
        public CrossTalkDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE crossTalkPacket)
        {
            int entryId = crossTalkPacket.EntryId;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CT_PACKET_INSERT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, crossTalkPacket.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_id", DbType.Int32, crossTalkPacket.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_lane_id", DbType.Int32, crossTalkPacket.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, entryId, ParameterDirection.Output));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_event_type", DbType.String, crossTalkPacket.EventType, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_time_stamp", DbType.String, crossTalkPacket.TimeStamp, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_uuid", DbType.String, crossTalkPacket.UUID, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_location_id", DbType.String, crossTalkPacket.LocationId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_parent_uuid", DbType.String, crossTalkPacket.ParentUUID, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_object_id", DbType.String, crossTalkPacket.ObjectId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_read", DbType.String, crossTalkPacket.FirstRead, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_read", DbType.String, crossTalkPacket.LastRead, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_observation_uuid", DbType.String, crossTalkPacket.ObservationUUID, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_reads", DbType.String, crossTalkPacket.Reads, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, crossTalkPacket.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_id", DbType.Int32, crossTalkPacket.VehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plate_number", DbType.String, crossTalkPacket.PlateNumber, ParameterDirection.Input));


                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                entryId = Convert.ToInt32(command.Parameters["p_entry_id"].Value);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entryId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE crossTalkPacket)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CT_PACKET_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, crossTalkPacket.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_id", DbType.Int32, crossTalkPacket.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_lane_id", DbType.Int32, crossTalkPacket.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, crossTalkPacket.EntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_event_type", DbType.String, crossTalkPacket.EventType, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_time_stamp", DbType.String, crossTalkPacket.TimeStamp, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_uuid", DbType.String, crossTalkPacket.UUID, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_location_id", DbType.String, crossTalkPacket.LocationId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_parent_uuid", DbType.String, crossTalkPacket.ParentUUID, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_object_id", DbType.String, crossTalkPacket.ObjectId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_read", DbType.String, crossTalkPacket.FirstRead, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_read", DbType.String, crossTalkPacket.LastRead, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_observation_uuid", DbType.String, crossTalkPacket.ObservationUUID, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_reads", DbType.String, crossTalkPacket.Reads, ParameterDirection.Input, 100));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, crossTalkPacket.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, crossTalkPacket.ModificationDate, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE crossTalkPacket)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CT_PACKET_DELETE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, crossTalkPacket.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_id", DbType.Int32, crossTalkPacket.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_lane_id", DbType.Int32, crossTalkPacket.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, crossTalkPacket.EntryId, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection crossTalkPackets = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CT_PACKET_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                crossTalkPackets = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return crossTalkPackets;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection GetRecent(Int32 plazaId, String tagId, DateTime tagReportingTime)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection crossTalkPackets = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CT_PACKET_GETRECENT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, plazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TAG_ID", DbType.String, tagId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TAG_TIME", DbType.DateTime, tagReportingTime, ParameterDirection.Input));


                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                crossTalkPackets = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return crossTalkPackets;
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
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection objs = GetAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].EntryId;
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
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection crossTalkPackets = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE crossTalkPacket = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        crossTalkPacket.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["PLAZA_ID"] != DBNull.Value)
                        crossTalkPacket.PlazaId = Convert.ToInt32(dt.Rows[i]["PLAZA_ID"]);

                    if (dt.Rows[i]["LANE_ID"] != DBNull.Value)
                        crossTalkPacket.LaneId = Convert.ToInt32(dt.Rows[i]["LANE_ID"]);

                    if (dt.Rows[i]["ENTRY_ID"] != DBNull.Value)
                        crossTalkPacket.EntryId = Convert.ToInt32(dt.Rows[i]["ENTRY_ID"]);

                    if (dt.Rows[i]["EVENT_TYPE"] != DBNull.Value)
                        crossTalkPacket.EventType = Convert.ToString(dt.Rows[i]["EVENT_TYPE"]);

                    if (dt.Rows[i]["TIME_STAMP"] != DBNull.Value)
                        crossTalkPacket.TimeStamp = Convert.ToString(dt.Rows[i]["TIME_STAMP"]);

                    if (dt.Rows[i]["UUID"] != DBNull.Value)
                        crossTalkPacket.UUID = Convert.ToString(dt.Rows[i]["UUID"]);

                    if (dt.Rows[i]["LOCATION_ID"] != DBNull.Value)
                        crossTalkPacket.LocationId = Convert.ToString(dt.Rows[i]["LOCATION_ID"]);

                    if (dt.Rows[i]["PARENT_UUID"] != DBNull.Value)
                        crossTalkPacket.ParentUUID = Convert.ToString(dt.Rows[i]["PARENT_UUID"]);

                    if (dt.Rows[i]["OBJECT_ID"] != DBNull.Value)
                        crossTalkPacket.ObjectId = Convert.ToString(dt.Rows[i]["OBJECT_ID"]);

                    if (dt.Rows[i]["FIRST_READ"] != DBNull.Value)
                        crossTalkPacket.FirstRead = Convert.ToString(dt.Rows[i]["FIRST_READ"]);

                    if (dt.Rows[i]["LAST_READ"] != DBNull.Value)
                        crossTalkPacket.LastRead = Convert.ToString(dt.Rows[i]["LAST_READ"]);

                    if (dt.Rows[i]["OBSERVATION_UUID"] != DBNull.Value)
                        crossTalkPacket.ObservationUUID = Convert.ToString(dt.Rows[i]["OBSERVATION_UUID"]);

                    if (dt.Rows[i]["READS"] != DBNull.Value)
                        crossTalkPacket.Reads = Convert.ToString(dt.Rows[i]["READS"]);

                    //if (dt.Rows[i]["REPORT_TIME_STAMP"] != DBNull.Value)
                    //    crossTalkPacket.ReportTimeStamp = Convert.ToString(dt.Rows[i]["REPORT_TIME_STAMP"]);

                    //if (dt.Rows[i]["REPORT_LOCATION_ID"] != DBNull.Value)
                    //    crossTalkPacket.ReportLocationId = Convert.ToString(dt.Rows[i]["REPORT_LOCATION_ID"]);

                    //if (dt.Rows[i]["REPORT_UUID"] != DBNull.Value)
                    //    crossTalkPacket.ReportUUID = Convert.ToString(dt.Rows[i]["REPORT_UUID"]);

                    //if (dt.Rows[i]["REPORT_OBJECT_ID"] != DBNull.Value)
                    //    crossTalkPacket.ReportObjectId = Convert.ToString(dt.Rows[i]["REPORT_OBJECT_ID"]);

                    //if (dt.Rows[i]["REPORT_START_TIME"] != DBNull.Value)
                    //    crossTalkPacket.ReportStartTime = Convert.ToString(dt.Rows[i]["REPORT_START_TIME"]);

                    //if (dt.Rows[i]["REPORT_STOP_TIME"] != DBNull.Value)
                    //    crossTalkPacket.ReportStopTime = Convert.ToString(dt.Rows[i]["REPORT_STOP_TIME"]);

                    //if (dt.Rows[i]["REPORT_START_EVENT"] != DBNull.Value)
                    //    crossTalkPacket.ReportStartEvent = Convert.ToString(dt.Rows[i]["REPORT_START_EVENT"]);

                    //if (dt.Rows[i]["REPORT_STOP_EVENT"] != DBNull.Value)
                    //    crossTalkPacket.ReportStopEvent = Convert.ToString(dt.Rows[i]["REPORT_STOP_EVENT"]);

                    //if (dt.Rows[i]["REPORT_CONDITION"] != DBNull.Value)
                    //    crossTalkPacket.ReportCondition = Convert.ToString(dt.Rows[i]["REPORT_CONDITION"]);

                    //if (dt.Rows[i]["REPORT_TAG_COUNT"] != DBNull.Value)
                    //    crossTalkPacket.ReportTagCount = Convert.ToString(dt.Rows[i]["REPORT_TAG_COUNT"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        crossTalkPacket.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        crossTalkPacket.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        crossTalkPacket.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);


                    crossTalkPackets.Add(crossTalkPacket);
                }
                return crossTalkPackets;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
