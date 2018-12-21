/* Formatted on 20/12/2018 09:45:42 (QP5 v5.215.12089.38647) */
CREATE VIEW TRANS_UNREVIEWED
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            T.CT_ENTRY_ID,
            T.NF_ENTRY_ID_FRONT,
            T.NF_ENTRY_ID_REAR,
            T.IS_BALANCE_UPDATED,
            T.IS_TRANSFERED,
            T.IS_VIOLATION,
            T.IS_REGISTERED,
            T.AUDIT_STATUS,
            T.AUDITOR_ID,
            T.AUDIT_DATE,
            T.AUDITED_VEHICLE_CLASS_ID,
            T.AUDITED_VRN,
            T.VEHICLESPEED,
            T.MEARGED_TRAN_ID
       FROM TBL_TRANSACTION T
            LEFT OUTER JOIN TBL_PLAZA P
               ON T.PLAZA_ID = P.PLAZA_ID
            LEFT OUTER JOIN TBL_LANE L
               ON T.LANE_ID = L.LANE_ID
      WHERE NVL (T.IS_BALANCE_UPDATED, 2) <> 1 AND NVL (T.AUDIT_STATUS, 2) <> 1
   ORDER BY T.TRANSACTION_ID DESC;



CREATE VIEW TRANS_REVIEWED
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            T.CT_ENTRY_ID,
            T.NF_ENTRY_ID_FRONT,
            T.NF_ENTRY_ID_REAR,
            T.IS_BALANCE_UPDATED,
            T.IS_TRANSFERED,
            T.IS_VIOLATION,
            T.IS_REGISTERED,
            T.AUDIT_STATUS,
            T.AUDITOR_ID,
            T.AUDIT_DATE,
            T.AUDITED_VEHICLE_CLASS_ID,
            T.AUDITED_VRN,
            T.VEHICLESPEED,
            T.MEARGED_TRAN_ID
       FROM TBL_TRANSACTION T
            LEFT OUTER JOIN TBL_PLAZA P
               ON T.PLAZA_ID = P.PLAZA_ID
            LEFT OUTER JOIN TBL_LANE L
               ON T.LANE_ID = L.LANE_ID
      WHERE NVL (T.AUDIT_STATUS, 2) = 1
   ORDER BY T.TRANSACTION_ID DESC;


CREATE VIEW TRANS_CHARGED
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            T.CT_ENTRY_ID,
            T.NF_ENTRY_ID_FRONT,
            T.NF_ENTRY_ID_REAR,
            T.IS_BALANCE_UPDATED,
            T.IS_TRANSFERED,
            T.IS_VIOLATION,
            T.IS_REGISTERED,
            T.AUDIT_STATUS,
            T.AUDITOR_ID,
            T.AUDIT_DATE,
            T.AUDITED_VEHICLE_CLASS_ID,
            T.AUDITED_VRN,
            T.VEHICLESPEED,
            T.MEARGED_TRAN_ID
       FROM TBL_TRANSACTION T
            LEFT OUTER JOIN TBL_PLAZA P
               ON T.PLAZA_ID = P.PLAZA_ID
            LEFT OUTER JOIN TBL_LANE L
               ON T.LANE_ID = L.LANE_ID
      WHERE NVL (T.IS_BALANCE_UPDATED, 0) = 1
   ORDER BY T.TRANSACTION_ID DESC;

CREATE VIEW TRANS_VIOLATION
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            T.CT_ENTRY_ID,
            T.NF_ENTRY_ID_FRONT,
            T.NF_ENTRY_ID_REAR,
            T.IS_BALANCE_UPDATED,
            T.IS_TRANSFERED,
            T.IS_VIOLATION,
            T.IS_REGISTERED,
            T.AUDIT_STATUS,
            T.AUDITOR_ID,
            T.AUDIT_DATE,
            T.AUDITED_VEHICLE_CLASS_ID,
            T.AUDITED_VRN,
            T.VEHICLESPEED,
            T.MEARGED_TRAN_ID
       FROM TBL_TRANSACTION T
            LEFT OUTER JOIN TBL_PLAZA P
               ON T.PLAZA_ID = P.PLAZA_ID
            LEFT OUTER JOIN TBL_LANE L
               ON T.LANE_ID = L.LANE_ID
      WHERE NVL (T.AUDIT_STATUS, 2) = 1 AND NVL (T.IS_VIOLATION, 0) = 1
   ORDER BY T.TRANSACTION_ID DESC;


CREATE VIEW TRANS_UNIDENTIFIED
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            T.CT_ENTRY_ID,
            T.NF_ENTRY_ID_FRONT,
            T.NF_ENTRY_ID_REAR,
            T.IS_BALANCE_UPDATED,
            T.IS_TRANSFERED,
            T.IS_VIOLATION,
            T.IS_REGISTERED,
            T.AUDIT_STATUS,
            T.AUDITOR_ID,
            T.AUDIT_DATE,
            T.AUDITED_VEHICLE_CLASS_ID,
            T.AUDITED_VRN,
            T.VEHICLESPEED,
            T.MEARGED_TRAN_ID
       FROM TBL_TRANSACTION T
            LEFT OUTER JOIN TBL_PLAZA P
               ON T.PLAZA_ID = P.PLAZA_ID
            LEFT OUTER JOIN TBL_LANE L
               ON T.LANE_ID = L.LANE_ID
      WHERE     NVL (T.AUDIT_STATUS, 2) = 1
            AND NVL (T.AUDITED_VEHICLE_CLASS_ID, 0) = 0
   ORDER BY T.TRANSACTION_ID DESC;