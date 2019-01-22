/* Formatted on 21/01/2019 17:50:52 (QP5 v5.215.12089.38647) */
DROP VIEW TRANS_UNREVIEWED;

CREATE VIEW TRANS_UNREVIEWED
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            TO_CHAR (T.TRANSACTION_DATETIME, 'DD-Mon-YYYY HH:MI:SS AM')
               F_TRANSACTION_DATETIME,
            (CASE NVL (T.CT_ENTRY_ID, 0)
                WHEN 0 THEN T.CT_ENTRY_ID_REAR
                ELSE T.CT_ENTRY_ID
             END)
               CT_ENTRY_ID,
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



DROP VIEW TRANS_REVIEWED;

CREATE VIEW TRANS_REVIEWED
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            TO_CHAR (T.TRANSACTION_DATETIME, 'DD-Mon-YYYY HH:MI:SS AM')
               F_TRANSACTION_DATETIME,
            (CASE NVL (T.CT_ENTRY_ID, 0)
                WHEN 0 THEN T.CT_ENTRY_ID_REAR
                ELSE T.CT_ENTRY_ID
             END)
               CT_ENTRY_ID,
            T.NF_ENTRY_ID_FRONT,
            T.NF_ENTRY_ID_REAR,
            T.IS_REGISTERED,
            T.VEHICLESPEED,
            T.AUDITOR_ID,
            T.AUDIT_DATE,
            T.AUDITED_VEHICLE_CLASS_ID,
            T.AUDITED_VRN,
            T.MEARGED_TRAN_ID,
            T.TRANS_STATUS,
            (CASE TRANS_STATUS
                WHEN 1 THEN 'Charged'
                WHEN 2 THEN 'Merged'
                WHEN 3 THEN 'Violation'
                WHEN 4 THEN 'Unidentified'
                ELSE 'Unknown'
             END)
               TRANS_STATUS_NAME
       FROM TBL_TRANSACTION T
            LEFT OUTER JOIN TBL_PLAZA P
               ON T.PLAZA_ID = P.PLAZA_ID
            LEFT OUTER JOIN TBL_LANE L
               ON T.LANE_ID = L.LANE_ID
      WHERE NVL (T.AUDIT_STATUS, 2) = 1
   ORDER BY T.AUDIT_DATE DESC;

  DROP VIEW TRANS_CHARGED;

CREATE VIEW TRANS_CHARGED
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            TO_CHAR (T.TRANSACTION_DATETIME, 'DD-Mon-YYYY HH:MI:SS AM')
               F_TRANSACTION_DATETIME,
            (CASE NVL (T.CT_ENTRY_ID, 0)
                WHEN 0 THEN T.CT_ENTRY_ID_REAR
                ELSE T.CT_ENTRY_ID
             END)
               CT_ENTRY_ID,
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
   ORDER BY T.TRANSACTION_DATETIME DESC;

  DROP VIEW TRANS_VIOLATION;

CREATE VIEW TRANS_VIOLATION
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            TO_CHAR (T.TRANSACTION_DATETIME, 'DD-Mon-YYYY HH:MI:SS AM')
               F_TRANSACTION_DATETIME,
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
   ORDER BY T.AUDIT_DATE DESC;

DROP VIEW TRANS_UNIDENTIFIED;

CREATE VIEW TRANS_UNIDENTIFIED
AS
     SELECT T.TMS_ID,
            T.PLAZA_ID,
            P.PLAZA_NAME,
            T.LANE_ID,
            L.LANE_NAME,
            T.TRANSACTION_ID,
            T.TRANSACTION_DATETIME,
            TO_CHAR (T.TRANSACTION_DATETIME, 'DD-Mon-YYYY HH:MI:SS AM')
               F_TRANSACTION_DATETIME,
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
   ORDER BY T.AUDIT_DATE DESC;


DROP VIEW TRANS_TOPUP;

CREATE VIEW TRANS_TOPUP
AS
   SELECT ENTRY_ID,
          ACCOUNT_ID,
          CUSTOMER_VEHICLE_ENTRY_ID,
          AMOUNT,
          CREATION_DATE,
          TO_CHAR (CREATION_DATE, 'DD-Mon-YYYY HH:MI:SS AM') F_CREATION_DATE
     FROM TBL_ACCOUNT_HISTORY
    WHERE TRANSACTION_TYPE = 2 AND NVL (CUSTOMER_VEHICLE_ENTRY_ID, 0) <> 0