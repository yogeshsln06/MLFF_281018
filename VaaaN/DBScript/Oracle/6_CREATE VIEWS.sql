/* Formatted on 26/01/2019 11:04:24 (QP5 v5.215.12089.38647) */
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
            T.MEARGED_TRAN_ID,
            (CASE NVL (T.AUDIT_STATUS, 0) WHEN 0 THEN 'Auto' ELSE 'Manual' END)
               TSOURCE,
            NVL (SH.GATEWAY_RESPONSE_CODE, 0) GATEWAY_RESPONSE_CODE,
            NVL (SH.OPERATOR_RESPONSE_CODE, 0) OPERATOR_RESPONSE_CODE,
            (CASE NVL (SH.OPERATOR_RESPONSE_CODE, 0)
                WHEN 0 THEN 'blank'
                WHEN 1 THEN 'Pending'
                WHEN 2 THEN 'Delivered'
                WHEN 3 THEN 'Read'
                WHEN 4 THEN 'Rejected'
                WHEN 5 THEN 'Failed'
             END)
               OPERATOR_RESPONSE_TEXT
       FROM TBL_TRANSACTION T
            LEFT OUTER JOIN TBL_PLAZA P
               ON T.PLAZA_ID = P.PLAZA_ID
            LEFT OUTER JOIN TBL_LANE L
               ON T.LANE_ID = L.LANE_ID
            LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
               ON T.TRANSACTION_ID = AH.TRANSACTION_ID
            LEFT OUTER JOIN TBL_SMS_COMM_HISTORY SH
               ON AH.ENTRY_ID = SH.ACCOUNT_HISTORY_ID
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
    WHERE TRANSACTION_TYPE = 2 AND NVL (CUSTOMER_VEHICLE_ENTRY_ID, 0) <> 0;

DROP VIEW SMS_SEND_GATWAY;

CREATE VIEW SMS_SEND_GATWAY
AS
   SELECT SC.ENTRY_ID,
          SC.TMS_ID,
          SC.CUSTOMER_ACCOUNT_ID,
          SC.CUSTOMER_NAME,
          SC.MOBILE_NUMBER,
          SC.MESSAGE_DIRECTION,
          SC.MESSAGE_BODY,
          SC.SENT_STATUS,
          SC.RECEIVED_PROCESS_STATUS,
          SC.MESSAGE_SEND_TIME,
          SC.MESSAGE_RECEIVE_TIME,
          SC.MESSAGE_DELIVERY_STATUS,
          SC.ATTEMPT_COUNT,
          SC.CREATION_DATE,
          SC.MODIFICATION_DATE,
          SC.MODIFIED_BY,
          SC.GATEWAY_RESPONSE_CODE,
          SC.OPERATOR_RESPONSE_CODE,
          AH.TRANSACTION_TYPE,
          (CASE AH.TRANSACTION_TYPE
              WHEN 1 THEN 'Sale'
              WHEN 2 THEN 'Top-Up'
              WHEN 3 THEN 'Refund'
              WHEN 4 THEN 'Charge'
           END)
             TRANSACTION_SUBJECT,
          EMAIL_ID,
          SC.TRANSACTION_ID,
          '' VEHICLE_RC_NO
     FROM TBL_SMS_COMM_HISTORY SC
          LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
             ON SC.ACCOUNT_HISTORY_ID = AH.ENTRY_ID
          LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
             ON SC.CUSTOMER_ACCOUNT_ID = CA.ACCOUNT_ID
    WHERE SC.SENT_STATUS = 1 AND SC.MESSAGE_DIRECTION = 2;


DROP VIEW SMS_PENDING_GATWAY_STATUS;

CREATE VIEW SMS_PENDING_GATWAY_STATUS
AS
   SELECT SC.ENTRY_ID,
          SC.TMS_ID,
          SC.CUSTOMER_ACCOUNT_ID,
          SC.CUSTOMER_NAME,
          SC.MOBILE_NUMBER,
          SC.MESSAGE_DIRECTION,
          SC.MESSAGE_BODY,
          SC.SENT_STATUS,
          SC.RECEIVED_PROCESS_STATUS,
          SC.MESSAGE_SEND_TIME,
          SC.MESSAGE_RECEIVE_TIME,
          SC.MESSAGE_DELIVERY_STATUS,
          SC.ATTEMPT_COUNT,
          SC.CREATION_DATE,
          SC.MODIFICATION_DATE,
          SC.MODIFIED_BY,
          SC.GATEWAY_RESPONSE_CODE,
          SC.OPERATOR_RESPONSE_CODE,
          AH.TRANSACTION_TYPE,
          (CASE AH.TRANSACTION_TYPE
              WHEN 1 THEN 'Sale'
              WHEN 2 THEN 'Top-Up'
              WHEN 3 THEN 'Refund'
              WHEN 4 THEN 'Charge'
           END)
             TRANSACTION_SUBJECT,
          EMAIL_ID,
          SC.TRANSACTION_ID,
          '' VEHICLE_RC_NO
     FROM TBL_SMS_COMM_HISTORY SC
          LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
             ON SC.ACCOUNT_HISTORY_ID = AH.ENTRY_ID
          LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
             ON SC.CUSTOMER_ACCOUNT_ID = CA.ACCOUNT_ID
    WHERE     SC.SENT_STATUS = 2
          AND SC.MESSAGE_DIRECTION = 2
          AND NVL (OPERATOR_RESPONSE_CODE, 0) IN (0, 1, 5);

DROP VIEW MOBILE_PENDING_NOTIFICATION;

CREATE VIEW MOBILE_PENDING_NOTIFICATION
AS
   SELECT SC.ENTRY_ID,
          SC.TMS_ID,
          SC.CUSTOMER_ACCOUNT_ID,
          SC.CUSTOMER_NAME,
          SC.MOBILE_NUMBER,
          SC.MESSAGE_DIRECTION,
          SC.MESSAGE_BODY,
          SC.SENT_STATUS,
          SC.RECEIVED_PROCESS_STATUS,
          SC.MESSAGE_SEND_TIME,
          SC.MESSAGE_RECEIVE_TIME,
          SC.MESSAGE_DELIVERY_STATUS,
          SC.ATTEMPT_COUNT,
          SC.CREATION_DATE,
          SC.MODIFICATION_DATE,
          SC.MODIFIED_BY,
          SC.GATEWAY_RESPONSE_CODE,
          SC.OPERATOR_RESPONSE_CODE,
          AH.TRANSACTION_TYPE,
          (CASE AH.TRANSACTION_TYPE
              WHEN 1 THEN 'Sale'
              WHEN 2 THEN 'Top-Up'
              WHEN 3 THEN 'Refund'
              WHEN 4 THEN 'Charge'
           END)
             TRANSACTION_SUBJECT,
          EMAIL_ID,
          SC.TRANSACTION_ID,
          CV.VEHICLE_RC_NO
     FROM TBL_SMS_COMM_HISTORY SC
          LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
             ON SC.ACCOUNT_HISTORY_ID = AH.ENTRY_ID
          LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
             ON SC.CUSTOMER_ACCOUNT_ID = CA.ACCOUNT_ID
          LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
             ON AH.CUSTOMER_VEHICLE_ENTRY_ID = CV.ENTRY_ID
    WHERE     SC.MESSAGE_DIRECTION = 2
          AND NVL (MOBILE_SEND_STATUS, 1) = 1
          AND NVL (CV.VEHICLE_RC_NO, '') <> '';