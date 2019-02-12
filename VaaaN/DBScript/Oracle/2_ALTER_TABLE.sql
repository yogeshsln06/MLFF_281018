/* Formatted on 09/12/2018 22:09:44 (QP5 v5.215.12089.38647) */
ALTER TABLE TBL_LANE
 ADD (LANE_TYPE_ID  NUMBER(1) DEFAULT 1);

 ALTER TABLE TBL_TRANSACTION
 ADD (IS_REGISTERED  NUMBER(1) DEFAULT 0);

  ALTER TABLE TBL_TRANSACTION
 ADD (VEHICLESPEED  DECIMAL(5,2) DEFAULT 0);
 
 

ALTER TABLE TBL_NODEFLUX_PACKET ADD  PROVIDER NUMBER(1);

ALTER TABLE TBL_LANE MODIFY CAMERA_ID_FRONT NUMBER(20);

ALTER TABLE TBL_LANE MODIFY CAMERA_ID_REAR NUMBER(20);

ALTER TABLE TBL_NODEFLUX_PACKET MODIFY CAMERA_ID NUMBER(20);

ALTER TABLE TBL_NODEFLUX_PACKET MODIFY TIMESTAMP NVARCHAR2 (100);

ALTER TABLE TBL_CUSTOMER_ACCOUNT
DROP COLUMN SCANNED_DOCS_PATH;

ALTER TABLE TBL_CUSTOMER_ACCOUNT ADD(
 RESIDENT_ID NVARCHAR2(255),
 BIRTH_PLACE NVARCHAR2(255),
 BIRTH_DATE DATE,
 RT_RW NVARCHAR2(255),
 PROVINCE_ID NUMBER(5),
 CITY_ID NUMBER(5),
 DISTRICT_ID NUMBER(5),
 SUB_DISTRICT_ID NUMBER(5),
 POSTAL_CODE NUMBER(6),
 NATIONALITY NUMBER(4),
 GENDER NUMBER(1),
 MARITAL_STATUS NUMBER(1),
 OCCUPATION NVARCHAR2(255),
 RESIDENTIDCARDIMAGE NVARCHAR2(255),
 VALID_UNTIL DATE,
 REGISTRATION_THROUGH NUMBER(2),
 USER_PASSWORD NVARCHAR2 (255)
);



ALTER TABLE TBL_CUSTOMER_VEHICLE ADD(
VEHICLE_RC_NO     NVARCHAR2(255),
OWNER_NAME              NVARCHAR2(255),
OWNER_ADDRESS           NVARCHAR2(255),
BRAND                   NVARCHAR2(255),
VEHICLE_TYPE            NVARCHAR2(255),
VEHICLE_CATEGORY        NVARCHAR2(255),
MODEL_NO                NVARCHAR2(255),
MANUFACTURING_YEAR      NUMBER(4),
CYCLINDER_CAPACITY      NVARCHAR2(255),
FRAME_NUMBER            NVARCHAR2(255),
ENGINE_NUMBER           NVARCHAR2(255),
VEHICLE_COLOR           NVARCHAR2(255),
FUEL_TYPE               NUMBER(2),
LICENCE_PLATE_COLOR     NUMBER(2),
REGISTRATION_YEAR       NUMBER(4),
VEHICLE_OWNERSHIP_NO    NVARCHAR2(255),
LOCATION_CODE           NVARCHAR2(255),
REG_QUEUE_NO            NVARCHAR2(255),
VEHICLEIMAGE_FRONT      NVARCHAR2(255),
VEHICLEIMAGE_REAR       NVARCHAR2(255),
VEHICLEIMAGE_RIGHT      NVARCHAR2(255),
VEHICLEIMAGE_LEFT       NVARCHAR2(255),
VEHICLE_RC_NO_PATH      NVARCHAR2(255),
EXCEPTION_FLAG          NUMBER (1),
STATUS                  NUMBER (1),
VALID_UNTIL             DATE,
TID_Front               NVARCHAR2(255),
TID_REAR                NVARCHAR2(255),
ACCOUNT_BALANCE         NUMBER (10, 2),
REGISTRATION_THROUGH   NUMBER(2),
IS_DOC_VERIFIED       NUMBER (1),
QUEUE_STATUS          NUMBER(1)
 );
 
   CREATE SEQUENCE CUSTOMER_ACCOUNT_SEQ
   START WITH 1
   MAXVALUE 9999999999
   MINVALUE 1
   NOCYCLE
   NOCACHE
   NOORDER;

   CREATE SEQUENCE CUSTOMER_VEHICLE_SEQ
   START WITH 1
   MAXVALUE 9999999999
   MINVALUE 1
   NOCYCLE
   NOCACHE
   NOORDER;
   
 ALTER TABLE TBL_SMS_COMM_HISTORY ADD CUSTOMER_VECHILE_ID NUMBER(10);
 
 ALTER TABLE TBL_MODULE ADD ICON NVARCHAR2 (100);

 
 ALTER TABLE TBL_SUBMODULE ADD ICON NVARCHAR2 (100);
 
 ALTER TABLE TBL_TRANSACTION
 ADD (MEARGED_TRAN_ID  NUMBER(20) DEFAULT 0);
 
 ALTER TABLE TBL_CROSSTALK_PACKET
 ADD (VEHICLE_CLASS_ID  NUMBER (5));
 
 ALTER TABLE TBL_CROSSTALK_PACKET
 ADD (PLATE_NUMBER   NVARCHAR2 (100));
 
  ALTER TABLE TBL_TRANSACTION
 ADD (TRANS_STATUS  NUMBER(1) DEFAULT 0);
 
ALTER TABLE TBL_ACCOUNT_HISTORY MODIFY ENTRY_ID NUMBER(20);
 
CREATE INDEX TRANSID ON TBL_TRANSACTION (TRANSACTION_ID);

CREATE INDEX AUDIRSTTAUS_IX ON TBL_TRANSACTION (AUDIT_STATUS);

CREATE INDEX IS_BALANCE_UPDATED_IX ON TBL_TRANSACTION (IS_BALANCE_UPDATED);

 
 CREATE INDEX ENTRY_ID_ix ON TBL_NODEFLUX_PACKET (ENTRY_ID);

CREATE INDEX VEHICLE_CLASS_ID_ix ON TBL_NODEFLUX_PACKET (VEHICLE_CLASS_ID);

CREATE INDEX PLATE_NUMBER_ix ON TBL_NODEFLUX_PACKET (PLATE_NUMBER);


ALTER TABLE  TBL_ACCOUNT_HISTORY ADD (OPENING_BALANCE NUMBER (10, 2));
ALTER TABLE  TBL_ACCOUNT_HISTORY ADD (CLOSING_BALANCE NUMBER (10, 2));

ALTER TABLE  TBL_SMS_COMM_HISTORY ADD (TRANSACTION_ID NVARCHAR2 (500));
ALTER TABLE  TBL_SMS_COMM_HISTORY ADD (GATEWAY_RESPONSE NVARCHAR2 (500));
ALTER TABLE  TBL_SMS_COMM_HISTORY ADD (GATEWAY_RESPONSE_CODE NUMBER(10));
ALTER TABLE  TBL_SMS_COMM_HISTORY ADD (OPERATOR_RESPONSE NVARCHAR2 (500));
ALTER TABLE  TBL_SMS_COMM_HISTORY ADD (OPERATOR_RESPONSE_CODE NUMBER(10));

ALTER TABLE  TBL_SMS_COMM_HISTORY ADD (OPERATOR_ATTEMPT_COUNT NUMBER (1));

ALTER TABLE  TBL_TRANSACTION ADD (CT_ENTRY_ID_REAR NUMBER (10));

ALTER TABLE TBL_CROSSTALK_PACKET
DROP COLUMN UUID;

ALTER TABLE TBL_CROSSTALK_PACKET
DROP COLUMN PARENT_UUID;

ALTER TABLE TBL_CROSSTALK_PACKET
DROP COLUMN FIRST_READ;

ALTER TABLE TBL_CROSSTALK_PACKET
DROP COLUMN LAST_READ;

ALTER TABLE TBL_CROSSTALK_PACKET
DROP COLUMN OBSERVATION_UUID;

CREATE TABLE TBL_ANPR_SYSTEM (ANPR_NAME NVARCHAR2 (100),STATUS NUMBER(1));

INSERT INTO TBL_ANPR_SYSTEM (ANPR_NAME, STATUS)
     VALUES ('hikvision', 0);

INSERT INTO TBL_ANPR_SYSTEM (ANPR_NAME, STATUS)
     VALUES ('nodeflux', 1);

INSERT INTO TBL_ANPR_SYSTEM (ANPR_NAME, STATUS)
     VALUES ('openalpr', 0);

COMMIT;

ALTER TABLE TBL_NODEFLUX_PACKET MODIFY PLATE_THUMBNAIL NVARCHAR2(500);

ALTER TABLE TBL_NODEFLUX_PACKET MODIFY VEHICLE_THUMBNAIL NVARCHAR2(500);

ALTER TABLE TBL_NODEFLUX_PACKET MODIFY VIDEO_URL NVARCHAR2(500);

ALTER TABLE TBL_ACCOUNT_HISTORY ADD SENT_STATUS  NUMBER (1);           --UNSENT=1,SENT=2,IGNORED=3

ALTER TABLE TBL_ACCOUNT_HISTORY ADD RESPONSE NVARCHAR2 (500);   

ALTER TABLE TBL_SMS_COMM_HISTORY ADD ACCOUNT_HISTORY_ID NUMBER(20);

ALTER TABLE TBL_SMS_COMM_HISTORY ADD REFERENCE_NO NVARCHAR2(100);

ALTER TABLE TBL_SMS_COMM_HISTORY ADD MOBILE_SEND_STATUS NUMBER(2);

ALTER TABLE TBL_CUSTOMER_ACCOUNT ADD(
 MOBILE_SEND_STATUS  NUMBER (1),           --UNSENT=1,SENT=2,IGNORED=3
 MOBILE_RESPONSE NVARCHAR2 (255)
);

ALTER TABLE TBL_PLAZA ADD(
 LONGITUDE  NUMBER(19,6),
 LATITUDE  NUMBER(19,6),
);
