using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class NodeFluxDAL
    {
        static string tableName = "TBL_NODEFLUX_PACKET";
        public NodeFluxDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nodeFluxPacket)
        {
            int entryId = nodeFluxPacket.EntryId;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "NF_PACKET_INSERT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, entryId, ParameterDirection.Output));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, nodeFluxPacket.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EVENT_TYPE", DbType.String, nodeFluxPacket.EventType, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TIMESTAMP", DbType.String, nodeFluxPacket.TimeStamp, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_GANTRY_ID", DbType.Int32, nodeFluxPacket.GantryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, nodeFluxPacket.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_POSITION", DbType.String, nodeFluxPacket.CameraPosition, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_ID", DbType.Int32, nodeFluxPacket.CameraId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_NAME", DbType.String, nodeFluxPacket.CameraName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_ADDRESS", DbType.String, nodeFluxPacket.CameraAddress, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_COORDINATE", DbType.String, nodeFluxPacket.CamaraCoordinate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLATE_NUMBER", DbType.String, nodeFluxPacket.PlateNumber, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CLASS_ID", DbType.Int32, nodeFluxPacket.VehicleClassId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_SPEED", DbType.Int32, nodeFluxPacket.VehicleSpeed, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLATE_THUMBNAIL", DbType.String, nodeFluxPacket.PlateThumbnail, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_THUMBNAIL", DbType.String, nodeFluxPacket.VehicleThumbnail, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VIDEO_URL", DbType.String, nodeFluxPacket.VideoURL, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVIDER", DbType.Int32, nodeFluxPacket.Provider, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, nodeFluxPacket.CreationDate, ParameterDirection.Input));


                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                entryId = Convert.ToInt32(command.Parameters["P_ENTRY_ID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entryId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nodeFluxPacket)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "NF_PACKET_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, nodeFluxPacket.EntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, nodeFluxPacket.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EVENT_TYPE", DbType.String, nodeFluxPacket.EventType, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TIMESTAMP", DbType.String, nodeFluxPacket.TimeStamp, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_GANTRY_ID", DbType.Int32, nodeFluxPacket.GantryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, nodeFluxPacket.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_POSITION", DbType.String, nodeFluxPacket.CameraPosition, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_ID", DbType.Int32, nodeFluxPacket.CameraId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_NAME", DbType.String, nodeFluxPacket.CameraName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_ADDRESS", DbType.String, nodeFluxPacket.CameraAddress, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_COORDINATE", DbType.String, nodeFluxPacket.CamaraCoordinate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLATE_NUMBER", DbType.String, nodeFluxPacket.PlateNumber, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CLASS_ID", DbType.Int32, nodeFluxPacket.VehicleClassId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_SPEED", DbType.Int32, nodeFluxPacket.VehicleSpeed, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLATE_THUMBNAIL", DbType.String, nodeFluxPacket.PlateThumbnail, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_THUMBNAIL", DbType.String, nodeFluxPacket.VehicleThumbnail, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VIDEO_URL", DbType.String, nodeFluxPacket.VideoURL, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVIDER", DbType.Int32, nodeFluxPacket.VideoURL, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, nodeFluxPacket.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, nodeFluxPacket.ModificationDate, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nodeFluxPacket)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "NF_PACKET_DELETE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, nodeFluxPacket.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_GANTRY_ID", DbType.Int32, nodeFluxPacket.GantryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, nodeFluxPacket.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, nodeFluxPacket.EntryId, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection nodeFluxPackets = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "NF_PACKET_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                nodeFluxPackets = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return nodeFluxPackets;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection GetRecent(Int32 plazaId, String vrn, DateTime nodeFluxReportingTime, int cameraPosition)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection nodeFluxPackets = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "NF_PACKET_GETRECENT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, plazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VRN", DbType.String, vrn, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_TIME", DbType.DateTime, nodeFluxReportingTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CAMERA_POSITION", DbType.Int32, cameraPosition, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                nodeFluxPackets = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return nodeFluxPackets;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE GetByEntryId(Int32 entryId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection nodeFluxPackets = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfPacket = null;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "NF_PACKET_GETBYENTRYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, entryId, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                nodeFluxPackets = ConvertDataTableToCollection(dt);

                if (nodeFluxPackets.Count > 0)
                    nfPacket = nodeFluxPackets[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return nfPacket;
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
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection objs = GetAll();

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
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection nodeFluxPackets = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nodeFluxPacket = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE();

                    if (dt.Rows[i]["ENTRY_ID"] != DBNull.Value)
                        nodeFluxPacket.EntryId = Convert.ToInt32(dt.Rows[i]["ENTRY_ID"]);

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        nodeFluxPacket.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["EVENT_TYPE"] != DBNull.Value)
                        nodeFluxPacket.EventType = Convert.ToString(dt.Rows[i]["EVENT_TYPE"]);

                    if (dt.Rows[i]["TIMESTAMP"] != DBNull.Value)
                        nodeFluxPacket.TimeStamp = Convert.ToString(dt.Rows[i]["TIMESTAMP"]);

                    if (dt.Rows[i]["GANTRY_ID"] != DBNull.Value)
                        nodeFluxPacket.GantryId = Convert.ToInt32(dt.Rows[i]["GANTRY_ID"]);

                    if (dt.Rows[i]["LANE_ID"] != DBNull.Value)
                        nodeFluxPacket.LaneId = Convert.ToInt32(dt.Rows[i]["LANE_ID"]);

                    //camera fields
                    if (dt.Rows[i]["CAMERA_POSITION"] != DBNull.Value)
                        nodeFluxPacket.CameraPosition = Convert.ToString(dt.Rows[i]["CAMERA_POSITION"]);

                    if (dt.Rows[i]["CAMERA_ID"] != DBNull.Value)
                        nodeFluxPacket.CameraId = Convert.ToInt32(dt.Rows[i]["CAMERA_ID"]);

                    if (dt.Rows[i]["CAMERA_NAME"] != DBNull.Value)
                        nodeFluxPacket.CameraName = Convert.ToString(dt.Rows[i]["CAMERA_NAME"]);

                    if (dt.Rows[i]["CAMERA_ADDRESS"] != DBNull.Value)
                        nodeFluxPacket.CameraAddress = Convert.ToString(dt.Rows[i]["CAMERA_ADDRESS"]);

                    if (dt.Rows[i]["CAMERA_COORDINATE"] != DBNull.Value)
                        nodeFluxPacket.CamaraCoordinate = Convert.ToString(dt.Rows[i]["CAMERA_COORDINATE"]);

                    if (dt.Rows[i]["CAMERA_ADDRESS"] != DBNull.Value)
                        nodeFluxPacket.CameraAddress = Convert.ToString(dt.Rows[i]["CAMERA_ADDRESS"]);

                    if (dt.Rows[i]["PLATE_NUMBER"] != DBNull.Value)
                        nodeFluxPacket.PlateNumber = Convert.ToString(dt.Rows[i]["PLATE_NUMBER"]);

                    if (dt.Rows[i]["VEHICLE_CLASS_ID"] != DBNull.Value)
                        nodeFluxPacket.VehicleClassId = Convert.ToInt32(dt.Rows[i]["VEHICLE_CLASS_ID"]);

                    if (dt.Rows[i]["VEHICLE_SPEED"] != DBNull.Value)
                        nodeFluxPacket.VehicleSpeed = Convert.ToInt32(dt.Rows[i]["VEHICLE_SPEED"]);

                    if (dt.Rows[i]["PLATE_THUMBNAIL"] != DBNull.Value)
                        nodeFluxPacket.PlateThumbnail = Convert.ToString(dt.Rows[i]["PLATE_THUMBNAIL"]);

                    if (dt.Rows[i]["VEHICLE_THUMBNAIL"] != DBNull.Value)
                        nodeFluxPacket.VehicleThumbnail = Convert.ToString(dt.Rows[i]["VEHICLE_THUMBNAIL"]);

                    if (dt.Rows[i]["VIDEO_URL"] != DBNull.Value)
                        nodeFluxPacket.VideoURL = Convert.ToString(dt.Rows[i]["VIDEO_URL"]);

                    if (dt.Rows[i]["PROVIDER"] != DBNull.Value)
                        nodeFluxPacket.Provider = Convert.ToInt32(dt.Rows[i]["PROVIDER"]);

                    //other fields
                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        nodeFluxPacket.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        nodeFluxPacket.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        nodeFluxPacket.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);


                    nodeFluxPackets.Add(nodeFluxPacket);
                }
                return nodeFluxPackets;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
