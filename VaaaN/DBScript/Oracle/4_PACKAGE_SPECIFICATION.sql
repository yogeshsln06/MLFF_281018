/* Formatted on 20/12/2018 18:52:05 (QP5 v5.215.12089.38647) */
CREATE OR REPLACE PACKAGE MLFF.MLFF_PACKAGE
AS
   TYPE T_CURSOR IS REF CURSOR;


   /*USER*/
   PROCEDURE USER_INSERT (P_USER_ID             IN NUMBER,
                          P_LOGIN_NAME          IN NVARCHAR2,
                          P_FIRST_NAME          IN NVARCHAR2,
                          P_LAST_NAME           IN NVARCHAR2 := NULL,
                          P_DESCRIPTION         IN NVARCHAR2,
                          P_PASSWORD            IN NVARCHAR2,
                          P_ADDRESS             IN NVARCHAR2 := NULL,
                          P_ROLE_ID             IN NUMBER,
                          P_ACC_EXPIRY_DATE     IN DATE,
                          P_MODIFIER_ID         IN NUMBER,
                          P_CREATION_DATE       IN DATE,
                          P_MODIFICATION_DATE   IN DATE,
                          P_FINGER_PRINT_1      IN NVARCHAR2,
                          P_MOBILE_NO           IN NVARCHAR2,
                          P_EMAIL_ID            IN NVARCHAR2,
                          P_USER_STATUS         IN NUMBER,
                          P_DOB                 IN DATE);

   PROCEDURE USER_UPDATE (P_USER_ID             IN NUMBER,
                          P_LOGIN_NAME          IN NVARCHAR2,
                          P_FIRST_NAME          IN NVARCHAR2,
                          P_LAST_NAME           IN NVARCHAR2 := NULL,
                          P_DESCRIPTION         IN NVARCHAR2,
                          P_ADDRESS             IN NVARCHAR2 := NULL,
                          P_ROLE_ID             IN NUMBER,
                          P_ACC_EXPIRY_DATE     IN DATE,
                          P_MODIFIER_ID         IN NUMBER,
                          P_CREATION_DATE       IN DATE,
                          P_MODIFICATION_DATE   IN DATE,
                          P_FINGER_PRINT_1      IN NVARCHAR2,
                          P_USER_STATUS         IN NUMBER,
                          P_MOBILE_NO           IN NVARCHAR2,
                          P_EMAIL_ID            IN NVARCHAR2,
                          P_DOB                 IN DATE);

   PROCEDURE USER_UPDATEWITHPASSWORD (P_USER_ID             IN NUMBER,
                                      P_LOGIN_NAME          IN NVARCHAR2,
                                      P_FIRST_NAME          IN NVARCHAR2,
                                      P_LAST_NAME           IN NVARCHAR2,
                                      P_DESCRIPTION         IN NVARCHAR2,
                                      P_PASSWORD            IN NVARCHAR2,
                                      P_ADDRESS             IN NVARCHAR2,
                                      P_ROLE_ID             IN NUMBER,
                                      P_ACC_EXPIRY_DATE     IN DATE,
                                      P_MODIFIER_ID         IN NUMBER,
                                      P_CREATION_DATE       IN DATE,
                                      P_MODIFICATION_DATE   IN DATE,
                                      P_FINGER_PRINT_1      IN NVARCHAR2);

   PROCEDURE UPDATEPASSWORD (P_OLD_PASSWORD       IN     NVARCHAR2,
                             P_CHANGED_PASSWORD   IN     NVARCHAR2,
                             P_USER_ID            IN     NUMBER,
                             P_EMAIL_ID           IN     NVARCHAR2,
                             P_RETURNMSG             OUT NVARCHAR2);


   PROCEDURE USER_DELETE (P_USER_ID IN NUMBER);


   PROCEDURE USER_INSERT_LOGININFO (P_ENTRY_ID        OUT NUMBER,
                                    P_USER_ID      IN     NUMBER,
                                    P_LOGIN_DATE   IN     DATE);

   PROCEDURE USER_UPDATE_LOGININFO (P_ENTRY_ID      IN NUMBER,
                                    P_USER_ID       IN NUMBER,
                                    P_LOGOUT_DATE   IN DATE);

   PROCEDURE USER_GETBYID (P_USER_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   PROCEDURE USER_GETBYNAME (P_LOGIN_NAME IN NVARCHAR2, CUR_OUT OUT T_CURSOR);

   PROCEDURE GETUSERBYEMAILID (P_EMAIL_ID IN NVARCHAR2, CUR_OUT OUT T_CURSOR);


   PROCEDURE USER_VALIDATEUSER (P_LOGIN_NAME   IN     NVARCHAR2,
                                P_PASSWORD     IN     NVARCHAR2,
                                CUR_OUT           OUT T_CURSOR);

   PROCEDURE VALIDATELOGINNAME (P_LOGIN_NAME   IN     NVARCHAR2,
                                P_EMAIL_ID     IN     NVARCHAR2,
                                P_RETURNMSG       OUT NVARCHAR2);

   PROCEDURE USER_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE USER_PAGEDGETALL (CUR_OUT                OUT T_CURSOR,
                               P_START_ROW_INDEX   IN     NUMBER,
                               P_END_ROW_INDEX     IN     NUMBER,
                               P_TOTAL_ROWS           OUT NUMBER);

   PROCEDURE USER_GETALL_PAGING (CUR_OUT             OUT T_CURSOR,
                                 P_PAGE_INDEX     IN     NUMBER,
                                 P_PAGE_SIZE      IN     NUMBER,
                                 P_SEARCH_TEXT    IN     NVARCHAR2,
                                 P_RECORD_COUNT      OUT NUMBER);



   /*ROLE*/
   PROCEDURE ROLE_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE ROLE_INSERT (P_ROLE_NAME           IN     NVARCHAR2,
                          P_DESCRIPTION         IN     NVARCHAR2,
                          P_MODIFIER_ID         IN     NUMBER,
                          P_CREATION_DATE       IN     DATE,
                          P_MODIFICATION_DATE   IN     DATE,
                          P_IS_ACTIVE           IN     NUMBER,
                          P_RETURNMSG              OUT NVARCHAR2);

   PROCEDURE ROLE_UPDATE (P_ROLE_ID             IN     NUMBER,
                          P_ROLE_NAME           IN     NVARCHAR2,
                          P_DESCRIPTION         IN     NVARCHAR2,
                          P_MODIFIER_ID         IN     NUMBER,
                          P_CREATION_DATE       IN     DATE,
                          P_MODIFICATION_DATE   IN     DATE,
                          P_IS_ACTIVE           IN     NUMBER,
                          P_RETURNMSG              OUT NVARCHAR2,
                          P_OLD_ROLE_NAME       IN     NVARCHAR2);



   PROCEDURE ROLE_DELETE (P_ROLE_ID IN NUMBER);


   PROCEDURE ROLE_GETBYID (P_ROLE_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   PROCEDURE ROLE_GETBYNAME (P_ROLE_NAME IN NVARCHAR2, CUR_OUT OUT T_CURSOR);


   PROCEDURE ROLE_PAGEDGETALL (CUR_OUT                OUT T_CURSOR,
                               P_START_ROW_INDEX   IN     NUMBER,
                               P_END_ROW_INDEX     IN     NUMBER,
                               P_TOTAL_ROWS           OUT NUMBER);


   /*ROLE MODULE RIGHT*/
   PROCEDURE ROLEMODRIGHT_INSERT (P_ROLE_ID         IN NUMBER,
                                  P_ENTRY_ID        IN NUMBER,
                                  P_MODULE_ID       IN NUMBER,
                                  P_MODULE_ADD      IN NUMBER,
                                  P_MODULE_VIEW     IN NUMBER,
                                  P_MODULE_EDIT     IN NUMBER,
                                  P_MODULE_DELETE   IN NUMBER);

   PROCEDURE ROLEMODRIGHT_UPDATE (P_ROLE_ID         IN NUMBER,
                                  P_ENTRY_ID        IN NUMBER,
                                  P_MODULE_ID       IN NUMBER,
                                  P_MODULE_ADD      IN NUMBER,
                                  P_MODULE_VIEW     IN NUMBER,
                                  P_MODULE_EDIT     IN NUMBER,
                                  P_MODULE_DELETE   IN NUMBER);



   PROCEDURE ROLEMODRIGHT_DELETE (P_ROLE_ID NUMBER, ENTRY_ID IN NUMBER);



   PROCEDURE ROLEMODRIGHT_DELETEBYROLEID (P_ROLE_ID NUMBER);



   PROCEDURE ROLEMODRIGHT_GETALL (CUR_OUT OUT T_CURSOR);



   /*USER MODULE RIGHT*/
   PROCEDURE USERMODRIGHT_INSERT (P_USER_ID IN NUMBER, P_ENTRY_ID IN NUMBER);



   PROCEDURE USERMODRIGHT_DELETE (P_USER_ID IN NUMBER, P_ENTRY_ID IN NUMBER);



   PROCEDURE USERMODRIGHT_DELETEBYUSERID (P_USER_ID IN NUMBER);



   PROCEDURE USER_MODULE_RIGHT_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE USERMODULE_ACT_UPDATE (P_ENTRY_ID        IN NUMBER,
                                    P_USER_ID         IN NUMBER,
                                    P_MODULE_ID       IN NUMBER,
                                    P_MODULE_VIEW     IN NUMBER,
                                    P_MODULE_ADD      IN NUMBER,
                                    P_MODULE_EDIT     IN NUMBER,
                                    P_MODULE_DELETE   IN NUMBER);

   PROCEDURE USERSUBMOD_ACT_UPDATE (P_ENTRY_ID            IN NUMBER,
                                    P_USER_ID             IN NUMBER,
                                    P_SUBMODULE_ID        IN NUMBER,
                                    P_SUB_MODULE_VIEW     IN NUMBER,
                                    P_SUB_MODULE_ADD      IN NUMBER,
                                    P_SUB_MODULE_EDIT     IN NUMBER,
                                    P_SUB_MODULE_DELETE   IN NUMBER);

   /*MODULE ACTIVITY*/
   PROCEDURE MODULE_ACTIVITY_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE MODULE_ACTIVITY_INSERT (P_MODULE_ID     IN NUMBER,
                                     P_ACTIVITY_ID   IN NUMBER);


   PROCEDURE MODULE_ACTIVITY_DELETE (P_ENTRY_ID    IN NUMBER,
                                     P_MODULE_ID   IN NUMBER);

   PROCEDURE USERMOD_ACT_GETBYUSERID (P_USER_ID   IN     NUMBER,
                                      CUR_OUT        OUT T_CURSOR);

   PROCEDURE USERMOD_ACT_GETBYENTRYID (P_ENTRY_ID   IN     NUMBER,
                                       CUR_OUT         OUT T_CURSOR);

   /*USER SUBMODULE RIGHT*/
   PROCEDURE USERSUBMODRIGHT_INSERT (P_USER_ID    IN NUMBER,
                                     P_ENTRY_ID   IN NUMBER);



   PROCEDURE USERSUBMODRIGHT_DELETE (P_USER_ID    IN NUMBER,
                                     P_ENTRY_ID   IN NUMBER);



   PROCEDURE USERSUBMODRIGHT_DELETEBYUSERID (P_USER_ID NUMBER);



   PROCEDURE USER_SUBMODULE_RIGHT_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE USERSUBRIGHT_BYUSERIDSUBMODID (P_USER_ID        IN     NUMBER,
                                            P_SUBMODULE_ID   IN     NUMBER,
                                            CUR_OUT             OUT T_CURSOR);

   PROCEDURE USERSUBMOD_ACT_BYUSERID (P_USER_ID     IN     NUMBER,
                                      P_MODULE_ID   IN     NUMBER,
                                      CUR_OUT          OUT T_CURSOR);

   PROCEDURE USERSUBMOD_ACT_GETBYENTRYID (P_ENTRY_ID   IN     NUMBER,
                                          CUR_OUT         OUT T_CURSOR);

   /*ROLE SUBMODULE RIGHT*/
   PROCEDURE ROLESUBMODRIGHT_INSERT (P_ROLE_ID             IN NUMBER,
                                     P_ENTRY_ID            IN NUMBER,
                                     P_SUBMODULE_ID        IN NUMBER,
                                     P_SUB_MODULE_VIEW     IN NUMBER,
                                     P_SUB_MODULE_ADD      IN NUMBER,
                                     P_SUB_MODULE_EDIT     IN NUMBER,
                                     P_SUB_MODULE_DELETE   IN NUMBER);

   PROCEDURE ROLESUBMODRIGHT_UPDATE (P_ROLE_ID             IN NUMBER,
                                     P_ENTRY_ID            IN NUMBER,
                                     P_SUBMODULE_ID        IN NUMBER,
                                     P_SUB_MODULE_VIEW     IN NUMBER,
                                     P_SUB_MODULE_ADD      IN NUMBER,
                                     P_SUB_MODULE_EDIT     IN NUMBER,
                                     P_SUB_MODULE_DELETE   IN NUMBER);



   PROCEDURE ROLESUBMODRIGHT_DELETE (P_ROLE_ID IN NUMBER, P_ENTRY_ID NUMBER);



   PROCEDURE ROLESUBMODRIGHT_DELETEBYROLEID (P_ROLE_ID NUMBER);



   PROCEDURE ROLESUBMODRIGHT_GETALL (CUR_OUT OUT T_CURSOR);


   PROCEDURE ROLESUBMODACT_GETBYROLEID (P_ROLE_ID     IN     NUMBER,
                                        P_MODULE_ID   IN     NUMBER,
                                        CUR_OUT          OUT T_CURSOR);


   /*SUBMODULE ACTIVITY*/
   PROCEDURE SUBMODULEACTIVITY_INSERT (P_ENTRY_ID       IN NUMBER,
                                       P_SUBMODULE_ID   IN NUMBER,
                                       P_ACTIVITY_ID    IN NUMBER);



   PROCEDURE SUBMODULEACTIVITY_DELETE (P_ENTRY_ID IN NUMBER);



   PROCEDURE SUBMODULE_ACTIVITY_GETALL (CUR_OUT OUT T_CURSOR);



   /*SUBMODULE*/
   PROCEDURE SUBMODULE_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE SUBMODULE_GETBYUSER (P_USER_ID     IN     NUMBER,
                                  P_MODULE_ID   IN     NUMBER,
                                  CUR_OUT          OUT T_CURSOR);

   PROCEDURE SUBMODULE_GETBYMODULEID (P_MODULE_ID   IN     NUMBER,
                                      CUR_OUT          OUT T_CURSOR);


   PROCEDURE SUBMODULE_INSERT (P_SUBMODULE_ID     IN NUMBER,
                               P_MODULE_ID        IN NUMBER,
                               P_SUBMODULE_NAME   IN NVARCHAR2,
                               P_IS_GUI_VISIBLE   IN NUMBER);



   PROCEDURE SUBMODULE_UPDATE (P_SUBMODULE_ID     IN NUMBER,
                               P_MODULE_ID        IN NUMBER,
                               P_SUBMODULE_NAME   IN NVARCHAR2,
                               P_IS_GUI_VISIBLE   IN NUMBER);



   PROCEDURE SUBMODULE_DELETE (P_SUBMODULE_ID NUMBER);



   PROCEDURE SUBMODULE_GETBYID (P_SUBMODULE_ID   IN     NUMBER,
                                CUR_OUT             OUT T_CURSOR);

   /*MODULE*/
   PROCEDURE MODULE_GETALL (CUR_OUT OUT T_CURSOR);


   PROCEDURE MODULE_INSERT (P_MODULE_ID        IN NUMBER,
                            P_MODULE_NAME      IN NVARCHAR2,
                            P_IS_GUI_VISIBLE   IN NUMBER);

   PROCEDURE MODULE_UPDATE (P_MODULE_ID        IN NUMBER,
                            P_MODULE_NAME      IN NVARCHAR2,
                            P_IS_GUI_VISIBLE   IN NUMBER);

   PROCEDURE MODULE_DELETE (P_MODULE_ID NUMBER);


   PROCEDURE MODULE_GETBYID (P_MODULE_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   PROCEDURE MODULE_GETBYUSER (P_USER_ID IN NUMBER, CUR_OUT OUT T_CURSOR);



   PROCEDURE ROLE_GETALL_PAGING (CUR_OUT             OUT T_CURSOR,
                                 P_PAGE_INDEX     IN     NUMBER,
                                 P_PAGE_SIZE      IN     NUMBER,
                                 P_SEARCH_TEXT    IN     NVARCHAR2,
                                 P_RECORD_COUNT      OUT NUMBER);

   PROCEDURE MODULEACTIVITY_BYROLEID (CUR_OUT        OUT T_CURSOR,
                                      P_ROLE_ID   IN     NUMBER);

   /*CROSSTALK EVENT*/
   PROCEDURE CT_PACKET_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE CT_PACKET_GETRECENT (P_PLAZA_ID   IN     NUMBER,
                                  P_TAG_ID     IN     NVARCHAR2,
                                  P_TAG_TIME   IN     DATE,
                                  CUR_OUT         OUT T_CURSOR);

   PROCEDURE CT_PACKET_INSERT (P_TMS_ID             IN     NUMBER,
                               P_PLAZA_ID           IN     NUMBER,
                               P_ENTRY_ID              OUT NUMBER,
                               P_LANE_ID            IN     NUMBER,
                               P_EVENT_TYPE         IN     NVARCHAR2,
                               P_TIME_STAMP         IN     NVARCHAR2,
                               P_UUID               IN     NVARCHAR2,
                               P_LOCATION_ID        IN     NVARCHAR2,
                               P_PARENT_UUID        IN     NVARCHAR2,
                               P_OBJECT_ID          IN     NVARCHAR2,
                               P_FIRST_READ         IN     NVARCHAR2,
                               P_LAST_READ          IN     NVARCHAR2,
                               P_OBSERVATION_UUID   IN     NVARCHAR2,
                               P_READS              IN     NVARCHAR2,
                               P_CREATION_DATE      IN     DATE,
                               P_VEHICLE_CLASS_ID   IN     NUMBER,
                               P_PLATE_NUMBER       IN     NVARCHAR2);

   PROCEDURE CT_PACKET_UPDATE (P_TMS_ID              IN NUMBER,
                               P_PLAZA_ID            IN NUMBER,
                               P_ENTRY_ID            IN NUMBER,
                               P_LANE_ID             IN NUMBER,
                               P_EVENT_TYPE          IN NVARCHAR2,
                               P_TIME_STAMP          IN NVARCHAR2,
                               P_UUID                IN NVARCHAR2,
                               P_LOCATION_ID         IN NVARCHAR2,
                               P_PARENT_UUID         IN NVARCHAR2,
                               P_OBJECT_ID           IN NVARCHAR2,
                               P_FIRST_READ          IN NVARCHAR2,
                               P_LAST_READ           IN NVARCHAR2,
                               P_OBSERVATION_UUID    IN NVARCHAR2,
                               P_READS               IN NVARCHAR2,
                               P_MODIFIER_ID         IN NUMBER,
                               P_MODIFICATION_DATE   IN DATE);

   PROCEDURE CT_PACKET_DELETE (P_TMS_ID     IN NUMBER,
                               P_PLAZA_ID   IN NUMBER,
                               P_ENTRY_ID   IN NUMBER);

   /*NODEFLUX EVENT*/
   PROCEDURE NF_PACKET_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE NF_PACKET_GETBYENTRYID (P_ENTRY_ID   IN     NUMBER,
                                     CUR_OUT         OUT T_CURSOR);

   PROCEDURE NF_PACKET_GETRECENT (P_PLAZA_ID                 NUMBER,
                                  P_VRN               IN     NVARCHAR2,
                                  P_NF_TIME           IN     DATE,
                                  P_CAMERA_POSITION   IN     NUMBER,
                                  CUR_OUT                OUT T_CURSOR);

   PROCEDURE NF_PACKET_INSERT (P_ENTRY_ID               OUT NUMBER,
                               P_TMS_ID              IN     NUMBER,
                               P_EVENT_TYPE          IN     NVARCHAR2,
                               P_TIMESTAMP           IN     NVARCHAR2,
                               P_GANTRY_ID           IN     NUMBER,
                               P_LANE_ID             IN     NUMBER,
                               P_CAMERA_POSITION     IN     NVARCHAR2,
                               P_CAMERA_ID           IN     NUMBER,
                               P_CAMERA_NAME         IN     NVARCHAR2,
                               P_CAMERA_ADDRESS      IN     NVARCHAR2,
                               P_CAMERA_COORDINATE   IN     NVARCHAR2,
                               P_PLATE_NUMBER        IN     NVARCHAR2,
                               P_VEHICLE_CLASS_ID    IN     NUMBER,
                               P_VEHICLE_SPEED       IN     NUMBER,
                               P_PLATE_THUMBNAIL     IN     NVARCHAR2,
                               P_VEHICLE_THUMBNAIL   IN     NVARCHAR2,
                               P_VIDEO_URL           IN     NVARCHAR2,
                               P_PROVIDER            IN     NUMBER,
                               P_CREATION_DATE       IN     DATE);

   PROCEDURE NF_PACKET_UPDATE (P_ENTRY_ID            IN NUMBER,
                               P_TMS_ID              IN NUMBER,
                               P_EVENT_TYPE          IN NVARCHAR2,
                               P_TIMESTAMP           IN NVARCHAR2,
                               P_GANTRY_ID           IN NUMBER,
                               P_LANE_ID             IN NUMBER,
                               P_CAMERA_POSITION     IN NVARCHAR2,
                               P_CAMERA_ID           IN NUMBER,
                               P_CAMERA_NAME         IN NVARCHAR2,
                               P_CAMERA_ADDRESS      IN NVARCHAR2,
                               P_CAMERA_COORDINATE   IN NVARCHAR2,
                               P_PLATE_NUMBER        IN NVARCHAR2,
                               P_VEHICLE_CLASS_ID    IN NUMBER,
                               P_VEHICLE_SPEED       IN NUMBER,
                               P_PLATE_THUMBNAIL     IN NVARCHAR2,
                               P_VEHICLE_THUMBNAIL   IN NVARCHAR2,
                               P_VIDEO_URL           IN NVARCHAR2,
                               P_PROVIDER            IN NUMBER,
                               P_MODIFIER_ID         IN NUMBER,
                               P_MODIFICATION_DATE   IN DATE);

   PROCEDURE NF_PACKET_DELETE (P_TMS_ID      IN NUMBER,
                               P_GANTRY_ID   IN NUMBER,
                               P_ENTRY_ID    IN NUMBER);

   /*TRANSACTION*/
   PROCEDURE TRAN_INSERT_BY_CTP (P_TMS_ID                 IN     NUMBER,
                                 P_PLAZA_ID               IN     NUMBER,
                                 P_LANE_ID                IN     NUMBER,
                                 P_TRANSACTION_ID            OUT NUMBER,
                                 P_TRANSACTION_DATETIME   IN     DATE,
                                 P_CT_ENTRY_ID            IN     NUMBER,
                                 P_IS_REGISTERED          IN     NUMBER,
                                 P_CREATION_DATE          IN     DATE);

   PROCEDURE TRAN_INSERT_BY_NFP_FRONT (
      P_TMS_ID                 IN     NUMBER,
      P_PLAZA_ID               IN     NUMBER,
      P_LANE_ID                IN     NUMBER,
      P_TRANSACTION_ID            OUT NUMBER,
      P_TRANSACTION_DATETIME   IN     DATE,
      P_NF_ENTRY_ID_FRONT      IN     NUMBER,
      P_IS_REGISTERED          IN     NUMBER,
      P_CREATION_DATE          IN     DATE);

   PROCEDURE TRAN_INSERT_BY_NFP_REAR (P_TMS_ID                 IN     NUMBER,
                                      P_PLAZA_ID               IN     NUMBER,
                                      P_LANE_ID                IN     NUMBER,
                                      P_TRANSACTION_ID            OUT NUMBER,
                                      P_TRANSACTION_DATETIME   IN     DATE,
                                      P_NF_ENTRY_ID_REAR       IN     NUMBER,
                                      P_IS_REGISTERED          IN     NUMBER,
                                      P_CREATION_DATE          IN     DATE);

   PROCEDURE TRAN_UPDATE_BY_NFP_FRONT (P_TMS_ID              IN NUMBER,
                                       P_PLAZA_ID            IN NUMBER,
                                       P_LANE_ID             IN NUMBER,
                                       P_TRANSACTION_ID      IN NUMBER,
                                       P_VEHICLESPEED        IN DECIMAL,
                                       P_NF_ENTRY_ID_FRONT   IN NUMBER);

   PROCEDURE TRAN_UPDATE_BY_NFP_REAR (P_TMS_ID             IN NUMBER,
                                      P_PLAZA_ID           IN NUMBER,
                                      P_LANE_ID            IN NUMBER,
                                      P_TRANSACTION_ID     IN NUMBER,
                                      P_VEHICLESPEED       IN DECIMAL,
                                      P_NF_ENTRY_ID_REAR   IN NUMBER);

   PROCEDURE TRAN_UPDATE (P_TMS_ID                 IN NUMBER,
                          P_PLAZA_ID               IN NUMBER,
                          P_LANE_ID                IN NUMBER,
                          P_TRANSACTION_ID         IN NUMBER,
                          P_TRANSACTION_DATETIME   IN DATE,
                          P_CT_ENTRY_ID            IN NUMBER,
                          P_NF_ENTRY_ID_FRONT      IN NUMBER,
                          P_NF_ENTRY_ID_REAR       IN NUMBER,
                          P_IS_BALANCE_UPDATED     IN NUMBER,
                          P_IS_TRANSFERED          IN NUMBER,
                          P_IS_VIOLATION           IN NUMBER,
                          P_IS_REGISTERED          IN NUMBER,
                          P_MODIFIER_ID            IN NUMBER,
                          P_MODIFICATION_DATE      IN DATE);

   PROCEDURE TRAN_UPDATE_AUDIT_SECTION (
      P_TMS_ID                     IN NUMBER,
      P_PLAZA_ID                   IN NUMBER,
      P_LANE_ID                    IN NUMBER,
      P_TRANSACTION_ID             IN NUMBER,
      P_AUDIT_STATUS               IN NUMBER,
      P_AUDITOR_ID                 IN NUMBER,
      P_AUDIT_DATE                 IN DATE,
      P_AUDITED_VEHICLE_CLASS_ID   IN NUMBER,
      P_AUDITED_VRN                IN NVARCHAR2);

   PROCEDURE TRAN_UPDATE_CTP (P_TMS_ID           IN NUMBER,
                              P_PLAZA_ID         IN NUMBER,
                              P_LANE_ID          IN NUMBER,
                              P_TRANSACTION_ID   IN NUMBER,
                              P_CT_ENTRY_ID      IN NUMBER);


   PROCEDURE TRAN_UPDATE_NF_FRONT (P_TMS_ID              IN NUMBER,
                                   P_PLAZA_ID            IN NUMBER,
                                   P_LANE_ID             IN NUMBER,
                                   P_TRANSACTION_ID      IN NUMBER,
                                   P_NF_ENTRY_ID_FRONT   IN NUMBER);

   PROCEDURE TRAN_UPDATE_NF_REAR (P_TMS_ID             IN NUMBER,
                                  P_PLAZA_ID           IN NUMBER,
                                  P_LANE_ID            IN NUMBER,
                                  P_TRANSACTION_ID     IN NUMBER,
                                  P_NF_ENTRY_ID_REAR   IN NUMBER);

   PROCEDURE TRAN_MARK_AS_VIOLATION (P_TMS_ID           IN NUMBER,
                                     P_PLAZA_ID         IN NUMBER,
                                     P_LANE_ID          IN NUMBER,
                                     P_TRANSACTION_ID   IN NUMBER);

   PROCEDURE TRAN_MARK_AS_BAL_UPD (P_TMS_ID           IN NUMBER,
                                   P_PLAZA_ID         IN NUMBER,
                                   P_LANE_ID          IN NUMBER,
                                   P_TRANSACTION_ID   IN NUMBER);

   PROCEDURE TRAN_DELETE (P_TRANSACTION_ID IN NUMBER);

   PROCEDURE TRAN_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE TRAN_GETBYID (P_TRANSACTION_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   PROCEDURE TRAN_GETBYPLAZAID (P_PLAZA_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   PROCEDURE TRAN_GET_CORR_IN_ALL (P_TMS_ID      IN     NUMBER,
                                   P_PLAZA_ID    IN     NUMBER,
                                   P_TIMESTAMP   IN     DATE,
                                   P_VRN         IN     NVARCHAR2,
                                   CUR_OUT          OUT T_CURSOR);

   PROCEDURE TRAN_GET_CORR_IN_NF (P_TMS_ID      IN     NUMBER,
                                  P_PLAZA_ID    IN     NUMBER,
                                  P_TIMESTAMP   IN     DATE,
                                  P_VRN         IN     NVARCHAR2,
                                  CUR_OUT          OUT T_CURSOR);

   PROCEDURE TRAN_GET_CORR_IN_CT (P_TMS_ID      IN     NUMBER,
                                  P_PLAZA_ID    IN     NUMBER,
                                  P_TIMESTAMP   IN     DATE,
                                  P_VRN         IN     NVARCHAR2,
                                  CUR_OUT          OUT T_CURSOR);

   PROCEDURE TRAN_LIVEDATA (CUR_OUT OUT T_CURSOR);

   PROCEDURE TRAN_LIVEDATABYID (P_TRANSACTION_ID   IN     NUMBER,
                                CUR_OUT               OUT T_CURSOR);

   PROCEDURE TRAN_GETFILTERED (P_FILTER IN NVARCHAR2, CUR_OUT OUT T_CURSOR);

   PROCEDURE TRAN_GETFILTERED_BYID (P_TRANSACTION_ID   IN     NUMBER,
                                    CUR_OUT               OUT T_CURSOR);

   PROCEDURE TRAN_UNREVIEWED_FILTERED (P_FILTER   IN     NVARCHAR2,
                                       CUR_OUT       OUT T_CURSOR);

   PROCEDURE TRAN_UNREVIEWED_BY_ID (P_TRANSACTION_ID   IN     NUMBER,
                                    CUR_OUT               OUT T_CURSOR);

   PROCEDURE TRAN_REVIEWED_FILTERED (P_FILTER   IN     NVARCHAR2,
                                     CUR_OUT       OUT T_CURSOR);

   PROCEDURE TRAN_CHARGED_FILTERED (P_FILTER   IN     NVARCHAR2,
                                    CUR_OUT       OUT T_CURSOR);

   PROCEDURE TRAN_VIOLATION_FILTERED (P_FILTER   IN     NVARCHAR2,
                                      CUR_OUT       OUT T_CURSOR);



   PROCEDURE TRAN_UNIDENTIFIED_FILTERED (P_FILTER   IN     NVARCHAR2,
                                         CUR_OUT       OUT T_CURSOR);

   /*PLAZA*/
   PROCEDURE PLAZA_INSERT (P_TMS_ID          IN NUMBER,
                           P_PLAZA_ID        IN NUMBER,
                           P_PLAZA_NAME      IN NVARCHAR2,
                           P_LOCATION        IN NVARCHAR2,
                           P_IPADDRESS       IN NVARCHAR2,
                           P_CREATION_DATE   IN DATE);

   PROCEDURE PLAZA_UPDATE (P_TMS_ID              IN NUMBER,
                           P_PLAZA_ID            IN NUMBER,
                           P_PLAZA_NAME          IN NVARCHAR2,
                           P_LOCATION            IN NVARCHAR2,
                           P_IPADDRESS           IN NVARCHAR2,
                           P_MODIFIER_ID         IN NUMBER,
                           P_MODIFICATION_DATE   IN DATE);

   PROCEDURE PLAZA_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE PLAZA_GETBYID (P_PLAZA_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   /*LANE*/
   PROCEDURE LANE_INSERT (P_TMS_ID                 IN NUMBER,
                          P_PLAZA_ID               IN NUMBER,
                          P_LANE_ID                IN NUMBER,
                          P_LANE_TYPE_ID           IN NUMBER,
                          P_LANE_NAME              IN NVARCHAR2,
                          P_CAMERA_ID_FRONT        IN NUMBER,
                          P_CAMERA_ID_REAR         IN NUMBER,
                          P_ETC_ANTENNA_ID_FRONT   IN NUMBER,
                          P_ETC_ANTENNA_ID_REAR    IN NUMBER,
                          P_CREATION_DATE          IN DATE,
                          P_TRANSFER_STATUS        IN NUMBER);

   PROCEDURE LANE_UPDATE (P_TMS_ID                 IN NUMBER,
                          P_PLAZA_ID               IN NUMBER,
                          P_LANE_ID                IN NUMBER,
                          P_LANE_TYPE_ID           IN NUMBER,
                          P_LANE_NAME              IN NVARCHAR2,
                          P_CAMERA_ID_FRONT        IN NUMBER,
                          P_CAMERA_ID_REAR         IN NUMBER,
                          P_ETC_ANTENNA_ID_FRONT   IN NUMBER,
                          P_ETC_ANTENNA_ID_REAR    IN NUMBER,
                          P_MODIFIED_BY            IN NUMBER,
                          P_MODIFICATION_DATE      IN DATE,
                          P_TRANSFER_STATUS        IN NUMBER);

   PROCEDURE LANE_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE LANE_GETBYID (P_LANE_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   PROCEDURE LANE_GETBYPLAZAID (P_PLAZA_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   /*HARDWARE*/
   PROCEDURE HARDWARE_INSERT (P_TMS_ID              IN NUMBER,
                              P_PLAZA_ID            IN NUMBER,
                              P_HARDWARE_ID         IN NUMBER,
                              P_HARDWARE_NAME       IN NVARCHAR2,
                              P_HARDWARE_TYPE       IN NUMBER,
                              P_HARDWARE_POSITION   IN NUMBER,
                              P_MANUFACTURER_NAME   IN NVARCHAR2,
                              P_MODEL_NAME          IN NVARCHAR2,
                              P_IP_ADDRESS          IN NVARCHAR2,
                              P_CREATION_DATE       IN DATE,
                              P_TRANSFER_STATUS     IN NUMBER);

   PROCEDURE HARDWARE_UPDATE (P_TMS_ID              IN NUMBER,
                              P_PLAZA_ID            IN NUMBER,
                              P_HARDWARE_ID         IN NUMBER,
                              P_HARDWARE_NAME       IN NVARCHAR2,
                              P_HARDWARE_TYPE       IN NUMBER,
                              P_HARDWARE_POSITION   IN NUMBER,
                              P_MANUFACTURER_NAME   IN NVARCHAR2,
                              P_MODEL_NAME          IN NVARCHAR2,
                              P_IP_ADDRESS          IN NVARCHAR2,
                              P_MODIFIED_BY         IN NUMBER,
                              P_MODIFICATION_DATE   IN DATE,
                              P_TRANSFER_STATUS     IN NUMBER);

   PROCEDURE HARDWARE_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE HARDWARE_GETBYID (P_HARDWARE_ID IN NUMBER, CUR_OUT OUT T_CURSOR);

   PROCEDURE HARDWARE_GETBYTYPE (P_HARDWARE_TYPE   IN     NUMBER,
                                 CUR_OUT              OUT T_CURSOR);

   PROCEDURE HARDWARE_LATEST_GETALL (P_LAST_UPDATE_TIME   IN     DATE,
                                     CUR_OUT                 OUT T_CURSOR);

   /*VEHICLE CLASS*/
   PROCEDURE VEHICLE_CLASS_INSERT (P_TMS_ID               IN NUMBER,
                                   P_VEHICLE_CLASS_ID     IN NUMBER,
                                   P_VEHICLE_CLASS_NAME   IN NVARCHAR2,
                                   P_CREATION_DATE        IN DATE,
                                   P_TRANSFER_STATUS      IN NUMBER);

   PROCEDURE VEHICLE_CLASS_UPDATE (P_TMS_ID               IN NUMBER,
                                   P_VEHICLE_CLASS_ID     IN NUMBER,
                                   P_VEHICLE_CLASS_NAME   IN NVARCHAR2,
                                   P_MODIFIED_BY          IN NUMBER,
                                   P_MODIFICATION_DATE    IN DATE,
                                   P_TRANSFER_STATUS      IN NUMBER);

   PROCEDURE VEHICLE_CLASS_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE VEHICLE_CLASS_GETBYID (P_VEHICLE_CLASS_ID   IN     NUMBER,
                                    CUR_OUT                 OUT T_CURSOR);

   PROCEDURE VEHICLE_CLASS_GETBYNAME (
      P_VEHICLE_CLASS_NAME   IN     NVARCHAR2,
      CUR_OUT                   OUT T_CURSOR);

   /*TOLL RATE*/
   PROCEDURE TOLL_RATE_INSERT (P_TMS_ID             IN NUMBER,
                               P_PLAZA_ID           IN NUMBER,
                               P_TRANSFER_STATUS    IN NUMBER,
                               P_RATE_ID            IN NUMBER,
                               P_PROFILE_ID         IN NUMBER,
                               P_VEHICLE_CLASS_ID   IN NUMBER,
                               P_LANE_TYPE_ID       IN NUMBER,
                               P_START_TIME         IN NVARCHAR2,
                               P_END_TIME           IN NVARCHAR2,
                               P_AMOUNT             IN NUMBER,
                               P_DESCRIPTION        IN NVARCHAR2,
                               P_CREATION_DATE      IN DATE);


   PROCEDURE TOLL_RATE_UPDATE (P_TMS_ID              IN NUMBER,
                               P_PLAZA_ID            IN NUMBER,
                               P_TRANSFER_STATUS     IN NUMBER,
                               P_RATE_ID             IN NUMBER,
                               P_PROFILE_ID          IN NUMBER,
                               P_VEHICLE_CLASS_ID    IN NUMBER,
                               P_LANE_TYPE_ID        IN NUMBER,
                               P_START_TIME          IN NVARCHAR2,
                               P_END_TIME            IN NVARCHAR2,
                               P_AMOUNT              IN NUMBER,
                               P_DESCRIPTION         IN NVARCHAR2,
                               P_MODIFIER_ID         IN NUMBER,
                               P_MODIFICATION_DATE   IN DATE);

   PROCEDURE TOLL_RATE_GETBYID (P_RATE_ID    IN     NUMBER,
                                P_TMS_ID     IN     NUMBER,
                                P_PLAZA_ID   IN     NUMBER,
                                CUR_OUT         OUT T_CURSOR);

   PROCEDURE TOLL_RATE_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE TOLLRATE_LATEST_GETALL (P_LAST_UPDATE_TIME   IN     DATE,
                                     CUR_OUT                 OUT T_CURSOR);

   /*SMS HISTORY*/
   PROCEDURE SMS_HISTORY_INSERT (P_ENTRY_ID                     OUT NUMBER,
                                 P_TMS_ID                    IN     NUMBER,
                                 P_CUSTOMER_ACCOUNT_ID       IN     NUMBER,
                                 P_CUSTOMER_VECHILE_ID       IN     NUMBER,
                                 P_CUSTOMER_NAME             IN     NVARCHAR2,
                                 P_MOBILE_NUMBER             IN     NVARCHAR2,
                                 P_MESSAGE_DIRECTION         IN     NUMBER,
                                 P_MESSAGE_BODY              IN     NVARCHAR2,
                                 P_SENT_STATUS               IN     NUMBER,
                                 P_RECEIVED_PROCESS_STATUS   IN     NUMBER,
                                 P_MESSAGE_SEND_TIME         IN     DATE,
                                 P_MESSAGE_RECEIVE_TIME      IN     DATE,
                                 P_MESSAGE_DELIVERY_STATUS   IN     NUMBER,
                                 P_ATTEMPT_COUNT             IN     NUMBER,
                                 P_CREATION_DATE             IN     DATE,
                                 P_MODIFICATION_DATE         IN     DATE,
                                 P_MODIFIED_BY               IN     NUMBER);

   PROCEDURE SMS_HISTORY_UPDATE (P_ENTRY_ID                  IN NUMBER,
                                 P_TMS_ID                    IN NUMBER,
                                 P_CUSTOMER_ACCOUNT_ID       IN NUMBER,
                                 P_CUSTOMER_NAME             IN NVARCHAR2,
                                 P_MOBILE_NUMBER             IN NVARCHAR2,
                                 P_MESSAGE_DIRECTION         IN NUMBER,
                                 P_MESSAGE_BODY              IN NVARCHAR2,
                                 P_SENT_STATUS               IN NUMBER,
                                 P_RECEIVED_PROCESS_STATUS   IN NUMBER,
                                 P_MESSAGE_SEND_TIME         IN DATE,
                                 P_MESSAGE_RECEIVE_TIME      IN DATE,
                                 P_MESSAGE_DELIVERY_STATUS   IN NUMBER,
                                 P_ATTEMPT_COUNT             IN NUMBER,
                                 P_CREATION_DATE             IN DATE,
                                 P_MODIFICATION_DATE         IN DATE,
                                 P_MODIFIED_BY               IN NUMBER);

   PROCEDURE SMS_HISTORY_GETFILTERED (P_FILTER   IN     NVARCHAR2,
                                      CUR_OUT       OUT T_CURSOR);

   /*CUSTOMER ACCOUNT*/
   PROCEDURE ACCOUNT_INSERT (P_TMS_ID                    IN     NUMBER,
                             P_ACCOUNT_ID                   OUT NUMBER,
                             P_FIRST_NAME                IN     NVARCHAR2,
                             P_LAST_NAME                 IN     NVARCHAR2,
                             P_MOB_NUMBER                IN     NVARCHAR2,
                             P_EMAIL_ID                  IN     NVARCHAR2,
                             P_DESCRIPTION               IN     NVARCHAR2,
                             P_ADDRESS                   IN     NVARCHAR2,
                             P_ACCOUNT_BALANCE           IN     NUMBER,
                             P_CREATION_DATE             IN     DATE,
                             P_CUSTOMER_IMAGE_PATH       IN     NVARCHAR2,
                             P_IS_DOC_VERIFIED           IN     NUMBER,
                             P_ACCOUNT_STATUS            IN     NUMBER,
                             P_TRANSFER_STATUS           IN     NUMBER,
                             P_RESIDENT_ID               IN     NVARCHAR2,
                             P_BIRTH_PLACE               IN     NVARCHAR2,
                             P_BIRTH_DATE                IN     DATE := NULL,
                             P_RT_RW                     IN     NVARCHAR2,
                             P_PROVINCE_ID               IN     NUMBER,
                             P_CITY_ID                   IN     NUMBER,
                             P_DISTRICT_ID               IN     NUMBER,
                             P_SUB_DISTRICT_ID           IN     NUMBER,
                             P_POSTAL_CODE               IN     NUMBER,
                             P_NATIONALITY               IN     NUMBER,
                             P_GENDER                    IN     NUMBER,
                             P_MARITAL_STATUS            IN     NUMBER,
                             P_OCCUPATION                IN     NVARCHAR2,
                             P_RESIDENTIDCARDIMAGEPATH   IN     NVARCHAR2,
                             P_VALID_UNTIL               IN     DATE := NULL,
                             P_REGISTRATION_THROUGH      IN     NUMBER,
                             P_USERPASSWORD              IN     NVARCHAR2);

   PROCEDURE ACCOUNT_UPDATE (P_TMS_ID                    IN NUMBER,
                             P_ACCOUNT_ID                IN NUMBER,
                             P_FIRST_NAME                IN NVARCHAR2,
                             P_LAST_NAME                 IN NVARCHAR2,
                             P_MOB_NUMBER                IN NVARCHAR2,
                             P_EMAIL_ID                  IN NVARCHAR2,
                             P_DESCRIPTION               IN NVARCHAR2,
                             P_ADDRESS                   IN NVARCHAR2,
                             P_ACCOUNT_BALANCE           IN NUMBER,
                             P_MODIFIER_ID               IN NUMBER,
                             P_MODIFICATION_DATE         IN DATE,
                             P_CUSTOMER_IMAGE_PATH       IN NVARCHAR2,
                             P_IS_DOC_VERIFIED           IN NUMBER,
                             P_ACCOUNT_STATUS            IN NUMBER,
                             P_TRANSFER_STATUS           IN NUMBER,
                             P_RESIDENT_ID               IN NVARCHAR2,
                             P_BIRTH_PLACE               IN NVARCHAR2,
                             P_BIRTH_DATE                IN DATE := NULL,
                             P_RT_RW                     IN NVARCHAR2,
                             P_PROVINCE_ID               IN NUMBER,
                             P_CITY_ID                   IN NUMBER,
                             P_DISTRICT_ID               IN NUMBER,
                             P_SUB_DISTRICT_ID           IN NUMBER,
                             P_POSTAL_CODE               IN NUMBER,
                             P_NATIONALITY               IN NUMBER,
                             P_GENDER                    IN NUMBER,
                             P_MARITAL_STATUS            IN NUMBER,
                             P_OCCUPATION                IN NVARCHAR2,
                             P_RESIDENTIDCARDIMAGEPATH   IN NVARCHAR2,
                             P_VALID_UNTIL               IN DATE := NULL);

   PROCEDURE ACCOUNT_GETBYMOBILENO (P_MOB_NUMBER   IN     NVARCHAR2,
                                    CUR_OUT           OUT T_CURSOR);

   PROCEDURE ACCOUNT_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE ACCOUNT_GETBYID (P_TMS_ID       IN     NUMBER,
                              P_ACCOUNT_ID   IN     NUMBER,
                              CUR_OUT           OUT T_CURSOR);

   PROCEDURE ACCOUNT_GETBY_RESIDENTID (P_TMS_ID        IN     NUMBER,
                                       P_RESIDENT_ID   IN     NVARCHAR2,
                                       CUR_OUT            OUT T_CURSOR);

   PROCEDURE CUSTOMER_ACCOUNT_VALIDATE (P_MOB_NUMBER    IN     NVARCHAR2,
                                        P_RESIDENT_ID   IN     NVARCHAR2,
                                        P_EMAIL_ID      IN     NVARCHAR2,
                                        CUR_OUT            OUT T_CURSOR);

   PROCEDURE ACCOUNT_BALANCEUPDATE (P_TMS_ID       IN NUMBER,
                                    P_ACCOUNT_ID   IN NUMBER,
                                    P_AMOUNT       IN NUMBER);

   PROCEDURE ACCOUNT_LATEST_GETALL (P_LAST_UPDATE_TIME   IN     DATE,
                                    CUR_OUT                 OUT T_CURSOR);

   PROCEDURE ACCOUNT_GETALL_LAZYLOAD (PAGEINDEX   IN     NUMBER,
                                      PAGESIZE    IN     NUMBER,
                                      CUR_OUT        OUT T_CURSOR);

   /*CUSTOMER VEHICLE*/
   PROCEDURE CUSTOMER_VEHICLE_INSERT (
      P_TMS_ID                 IN     NUMBER,
      P_ENTRY_ID                  OUT NUMBER,
      P_ACCOUNT_ID             IN     NUMBER,
      P_VEH_REG_NO             IN     NVARCHAR2,
      P_TAG_ID                 IN     NVARCHAR2,
      P_VEHICLE_CLASS_ID       IN     NUMBER,
      P_CREATION_DATE          IN     DATE,
      P_TRANSFER_STATUS        IN     NUMBER,
      P_VEHICLE_RC_NO          IN     NVARCHAR2,
      P_OWNER_NAME             IN     NVARCHAR2,
      P_OWNER_ADDRESS          IN     NVARCHAR2,
      P_BRAND                  IN     NVARCHAR2,
      P_VEHICLE_TYPE           IN     NVARCHAR2,
      P_VEHICLE_CATEGORY       IN     NVARCHAR2,
      P_MODEL_NO               IN     NVARCHAR2,
      P_MANUFACTURING_YEAR     IN     NUMBER,
      P_CYCLINDER_CAPACITY     IN     NVARCHAR2,
      P_FRAME_NUMBER           IN     NVARCHAR2,
      P_ENGINE_NUMBER          IN     NVARCHAR2,
      P_VEHICLE_COLOR          IN     NVARCHAR2,
      P_FUEL_TYPE              IN     NUMBER,
      P_LICENCE_PLATE_COLOR    IN     NUMBER,
      P_REGISTRATION_YEAR      IN     NUMBER,
      P_VEHICLE_OWNERSHIP_NO   IN     NVARCHAR2,
      P_LOCATION_CODE          IN     NVARCHAR2,
      P_REG_QUEUE_NO           IN     NVARCHAR2,
      P_VEHICLEIMAGE_FRONT     IN     NVARCHAR2,
      P_VEHICLEIMAGE_REAR      IN     NVARCHAR2,
      P_VEHICLEIMAGE_RIGHT     IN     NVARCHAR2,
      P_VEHICLEIMAGE_LEFT      IN     NVARCHAR2,
      P_VEHICLE_RC_NO_PATH     IN     NVARCHAR2,
      P_EXCEPTION_FLAG         IN     NUMBER,
      P_STATUS                 IN     NUMBER,
      P_VALID_UNTIL            IN     DATE := NULL,
      P_TID_FRONT              IN     NVARCHAR2,
      P_TID_REAR               IN     NVARCHAR2,
      P_ACCOUNT_BALANCE        IN     NUMBER,
      P_REGISTRATION_THROUGH   IN     NUMBER,
      P_IS_DOC_VERIFIED        IN     NUMBER,
      P_QUEUE_STATUS           IN     NUMBER);

   PROCEDURE CUSTOMER_VEHICLE_UPDATE (
      P_TMS_ID                 IN NUMBER,
      P_ENTRY_ID               IN NUMBER,
      P_ACCOUNT_ID             IN NUMBER,
      P_VEH_REG_NO             IN NVARCHAR2,
      P_TAG_ID                 IN NVARCHAR2,
      P_VEHICLE_CLASS_ID       IN NUMBER,
      P_MODIFICATION_DATE      IN DATE,
      P_MODIFIED_BY            IN NUMBER,
      P_TRANSFER_STATUS        IN NUMBER,
      P_VEHICLE_RC_NO          IN NVARCHAR2,
      P_OWNER_NAME             IN NVARCHAR2,
      P_OWNER_ADDRESS          IN NVARCHAR2,
      P_BRAND                  IN NVARCHAR2,
      P_VEHICLE_TYPE           IN NVARCHAR2,
      P_VEHICLE_CATEGORY       IN NVARCHAR2,
      P_MODEL_NO               IN NVARCHAR2,
      P_MANUFACTURING_YEAR     IN NUMBER,
      P_CYCLINDER_CAPACITY     IN NVARCHAR2,
      P_FRAME_NUMBER           IN NVARCHAR2,
      P_ENGINE_NUMBER          IN NVARCHAR2,
      P_VEHICLE_COLOR          IN NVARCHAR2,
      P_FUEL_TYPE              IN NUMBER,
      P_LICENCE_PLATE_COLOR    IN NUMBER,
      P_REGISTRATION_YEAR      IN NUMBER,
      P_VEHICLE_OWNERSHIP_NO   IN NVARCHAR2,
      P_LOCATION_CODE          IN NVARCHAR2,
      P_REG_QUEUE_NO           IN NVARCHAR2,
      P_VEHICLEIMAGE_FRONT     IN NVARCHAR2,
      P_VEHICLEIMAGE_REAR      IN NVARCHAR2,
      P_VEHICLEIMAGE_RIGHT     IN NVARCHAR2,
      P_VEHICLEIMAGE_LEFT      IN NVARCHAR2,
      P_VEHICLE_RC_NO_PATH     IN NVARCHAR2,
      P_EXCEPTION_FLAG         IN NUMBER,
      P_STATUS                 IN NUMBER,
      P_VALID_UNTIL            IN DATE := NULL,
      P_TID_FRONT              IN NVARCHAR2,
      P_TID_REAR               IN NVARCHAR2,
      P_ACCOUNT_BALANCE        IN NUMBER,
      P_IS_DOC_VERIFIED        IN NUMBER,
      P_QUEUE_STATUS           IN NUMBER);

   PROCEDURE VEHICLE_BALANCE_UPDATE (P_ENTRY_ID          IN     NUMBER,
                                     P_VEH_REG_NO        IN     NVARCHAR2,
                                     P_AMOUNT            IN     NUMBER,
                                     P_ACCOUNT_BALANCE      OUT NUMBER);

   PROCEDURE CUSTOMER_VEHICLE_GETBYID (P_VEHICLE_CLASS_ID   IN     NUMBER,
                                       P_TMS_ID             IN     NUMBER,
                                       P_ACCOUNT_ID         IN     NUMBER,
                                       CUR_OUT                 OUT T_CURSOR);

   PROCEDURE CUSTOMERVEHICLE_GETBYID (P_ENTRY_ID   IN     NUMBER,
                                      CUR_OUT         OUT T_CURSOR);

   PROCEDURE CV_GET_BY_TRANCTPENTRYID (P_TRAN_CT_EN_ID   IN     NUMBER,
                                       CUR_OUT              OUT T_CURSOR);

   PROCEDURE CUSTOMERVEHICLE_GETBYACCOUNTID (P_TMS_ID       IN     NUMBER,
                                             P_ACCOUNT_ID   IN     NUMBER,
                                             CUR_OUT           OUT T_CURSOR);

   PROCEDURE CUSTOMER_VEHICLE_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE CUSTOMER_VEHICLE_GETBYTAGID (P_TAG_ID   IN     NVARCHAR2,
                                          CUR_OUT       OUT T_CURSOR);

   PROCEDURE CUSTOMERVEHICLE_GETBYVEHREGNO (P_VEH_REG_NO   IN     NVARCHAR2,
                                            CUR_OUT           OUT T_CURSOR);

   PROCEDURE VALIDATE_VEHICLE_DETAILS (P_VEH_REG_NO      IN     NVARCHAR2,
                                       P_RESIDENT_ID     IN     NVARCHAR2,
                                       P_VEHICLE_RC_NO   IN     NVARCHAR2,
                                       CUR_OUT              OUT T_CURSOR);

   PROCEDURE VEHICLE_LATEST_GETALL (P_LAST_UPDATE_TIME   IN     DATE,
                                    CUR_OUT                 OUT T_CURSOR);


   /*ACCOUNT HISTORY*/
   PROCEDURE ACCOUNT_HISTORY_INSERT (
      P_TMS_ID                      IN     NUMBER,
      P_ACCOUNT_ID                  IN     NUMBER,
      P_ENTRY_ID                       OUT NUMBER,
      P_CUSTOMER_VEHICLE_ENTRY_ID   IN     NUMBER,
      P_TRANSACTION_TYPE            IN     NUMBER,
      P_TRANSACTION_ID              IN     NUMBER,
      P_AMOUNT                      IN     NUMBER,
      P_IS_SMS_SENT                 IN     NUMBER,
      P_IS_EMAIL_SENT               IN     NUMBER,
      P_CREATION_DATE               IN     DATE,
      P_TRANSFER_STATUS             IN     NUMBER);

   PROCEDURE ACCOUNT_HISTORY_UPDATE (P_TMS_ID                      IN NUMBER,
                                     P_ACCOUNT_ID                  IN NUMBER,
                                     P_ENTRY_ID                    IN NUMBER,
                                     P_CUSTOMER_VEHICLE_ENTRY_ID   IN NUMBER,
                                     P_TRANSACTION_TYPE            IN NUMBER,
                                     P_TRANSACTION_ID              IN NUMBER,
                                     P_AMOUNT                      IN NUMBER,
                                     P_IS_SMS_SENT                 IN NUMBER,
                                     P_IS_EMAIL_SENT               IN NUMBER,
                                     P_MODIFIED_BY                 IN NUMBER,
                                     P_TRANSFER_STATUS             IN NUMBER);

   PROCEDURE ACCOUNT_HISTORY_DELETE (P_TMS_ID IN NUMBER, P_ENTRY_ID NUMBER);

   PROCEDURE ACCOUNT_HISTORY_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE ACCOUNT_HISTORY_BYACCOUNTID (
      P_ACCOUNT_ID         IN     NUMBER,
      P_TRANSACTION_TYPE   IN     NUMBER,
      CUR_OUT                 OUT T_CURSOR);

   /*CSV*/
   PROCEDURE TRAN_CSV_GETNORMALTRAN (P_START_TIME   IN     DATE,
                                     P_END_TIME     IN     DATE,
                                     CUR_OUT           OUT T_CURSOR);

   PROCEDURE TRAN_CSV_GETVIOTRAN (P_START_TIME   IN     DATE,
                                  P_END_TIME     IN     DATE,
                                  CUR_OUT           OUT T_CURSOR);

   PROCEDURE TRAN_CSV_GETTRANWITHOUTTAG (P_START_TIME   IN     DATE,
                                         P_END_TIME     IN     DATE,
                                         CUR_OUT           OUT T_CURSOR);

   PROCEDURE JOIN_AUDIT_TRANSACTIONS (
      P_PARENT_TRANSACTION_ID      IN NUMBER := NULL,
      P_CHILD_1_TRANSACTION_ID     IN NUMBER := NULL,
      P_CHILD_2_TRANSACTION_ID     IN NUMBER := NULL,
      P_AUDITED_VRN                IN NVARCHAR2,
      P_AUDITED_VEHICLE_CLASS_ID   IN NUMBER,
      P_AUDITOR_ID                 IN NUMBER);

   /*VEHICLE CLASS MAPPING*/
   PROCEDURE VEHICLE_CLASS_MAPPING_INSERT (
      P_TMS_ID                     IN NUMBER,
      P_MAPPING_VEHICLE_CLASS_ID   IN NUMBER,
      P_MLFF_VEHICLE_CLASS_ID      IN NUMBER,
      P_ANPR_VEHICLE_CLASS_ID      IN NUMBER,
      P_ANPR_VEHICLE_CLASS_NAME    IN NVARCHAR2,
      P_CREATION_DATE              IN DATE,
      P_TRANSFER_STATUS            IN NUMBER);


   PROCEDURE VEHICLE_CLASS_MAPPING_UPDATE (
      P_TMS_ID                     IN NUMBER,
      P_MAPPING_VEHICLE_CLASS_ID   IN NUMBER,
      P_MLFF_VEHICLE_CLASS_ID      IN NUMBER,
      P_ANPR_VEHICLE_CLASS_ID      IN NUMBER,
      P_ANPR_VEHICLE_CLASS_NAME    IN NVARCHAR2,
      P_MODIFIED_BY                IN NUMBER,
      P_MODIFICATION_DATE          IN DATE,
      P_TRANSFER_STATUS            IN NUMBER);


   PROCEDURE VEHICLE_CLASS_MAPPING_GETALL (CUR_OUT OUT T_CURSOR);


   PROCEDURE VEHICLE_CLASS_MAPPING_GETBYID (
      P_MAPPING_VEHICLE_CLASS_ID   IN     NUMBER,
      CUR_OUT                         OUT T_CURSOR);

   PROCEDURE VEHICLECLASS_MAPPING_GETBYNAME (
      P_ANPR_VEHICLE_CLASS_NAME   IN     NVARCHAR2,
      CUR_OUT                        OUT T_CURSOR);

   /*PROVINCE*/
   PROCEDURE PROVINCE_GETALL (CUR_OUT OUT T_CURSOR);

   /*CITY*/
   PROCEDURE CITY_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE CITY_GETBYPROVINCEID (P_TMS_ID        IN     NUMBER,
                                   P_PROVINCE_ID   IN     NUMBER,
                                   CUR_OUT            OUT T_CURSOR);

   /*DISTRICT*/
   PROCEDURE DISTRICT_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE DISTRICT_GETBYCITYID (P_TMS_ID    IN     NUMBER,
                                   P_CITY_ID   IN     NUMBER,
                                   CUR_OUT        OUT T_CURSOR);

   /*SUBDISTRICT*/
   PROCEDURE SUBDISTRICT_GETALL (CUR_OUT OUT T_CURSOR);

   PROCEDURE SUBDISTRICT_GETBYDISTRICTID (P_TMS_ID        IN     NUMBER,
                                          P_DISTRICT_ID   IN     NUMBER,
                                          CUR_OUT            OUT T_CURSOR);

   /*CUSTOMER APPOINTMENT*/
   PROCEDURE CUSTOMER_APPOINTMENT_INSERT (
      P_TMS_ID                    IN     NUMBER,
      P_CUSTOMER_APPOINTMENT_ID      OUT NUMBER,
      P_ACCOUNT_ID                IN     NUMBER,
      P_APPOINTMENT_LOCATION      IN     NVARCHAR2,
      P_APPOINTMENT_DATE          IN     DATE,
      P_APPOINTED_BY              IN     NUMBER,
      P_ATTENDED_BY               IN     NUMBER,
      P_CREATION_DATE             IN     DATE,
      P_TRANSFER_STATUS           IN     NUMBER);

   PROCEDURE CUSTOMER_APPOINTMENT_UPDATE (
      P_TMS_ID                    IN NUMBER,
      P_CUSTOMER_APPOINTMENT_ID   IN NUMBER,
      P_ACCOUNT_ID                IN NUMBER,
      P_APPOINTMENT_LOCATION      IN NVARCHAR2,
      P_APPOINTMENT_DATE          IN DATE,
      P_APPOINTED_BY              IN NUMBER,
      P_ATTENDED_BY               IN NUMBER,
      P_MODIFIER_ID               IN NUMBER,
      P_MODIFICATION_DATE         IN DATE,
      P_TRANSFER_STATUS           IN NUMBER);



   PROCEDURE CUSTOMER_APPOINTMENT_GETALL (CUR_OUT OUT T_CURSOR);


   PROCEDURE CUSTOMER_APPOINTMENT_GETBYID (
      P_TMS_ID                    IN     NUMBER,
      P_CUSTOMER_APPOINTMENT_ID   IN     NUMBER,
      CUR_OUT                        OUT T_CURSOR);


   PROCEDURE CUSTAPPOINTMENT_GETACC_ID (P_TMS_ID       IN     NUMBER,
                                        P_ACCOUNT_ID   IN     NUMBER,
                                        CUR_OUT           OUT T_CURSOR);

   PROCEDURE TRANSCATION_HISTORY_DETAILS (P_VEH_REG_NO      IN     NVARCHAR2,
                                          P_RESIDENT_ID     IN     NVARCHAR2,
                                          P_VEHICLE_RC_NO   IN     NVARCHAR2,
                                          CUR_OUT              OUT T_CURSOR);

   PROCEDURE TRANS_HISTORY_DETAILS_PAGING (
      P_VEH_REG_NO      IN     NVARCHAR2,
      P_RESIDENT_ID     IN     NVARCHAR2,
      P_VEHICLE_RC_NO   IN     NVARCHAR2,
      P_PAGE_INDEX      IN     NUMBER,
      P_PAGE_SIZE       IN     NUMBER,
      CUR_OUT              OUT T_CURSOR);

   PROCEDURE MEARGED_AUDIT_TRANSACTIONS (
      P_PARENT_TRANSACTION_ID      IN NUMBER,
      P_IKEENTRYID                 IN NUMBER,
      P_ANPRFRONTENTRYID           IN NUMBER,
      P_ANPRREARENTRYID            IN NUMBER,
      P_AUDITED_VRN                IN NVARCHAR2,
      P_AUDITED_VEHICLE_CLASS_ID   IN NUMBER,
      P_AUDITOR_ID                 IN NUMBER,
      P_TRANS_STATUS               IN NUMBER);
END MLFF_PACKAGE;
/