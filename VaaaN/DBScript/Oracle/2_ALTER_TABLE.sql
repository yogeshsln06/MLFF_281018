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

ALTER TABLE TBL_CUSTOMER_ACCOUNT ADD( SCANNED_DOCS_PATH2 NVARCHAR2(200) ,
 SCANNED_DOCS_PATH3 NVARCHAR2(200), 
 SCANNED_DOCS_PATH4 NVARCHAR2(200), 
 RESIDENT_ID NUMBER(20), 
 BIRTH_PLACE NVARCHAR2(100), 
 BIRTH_DATE DATE, 
 GENDER NUMBER(1), 
 ADDRESS_LINE_2 NVARCHAR2(200), 
 ADDRESS_LINE_3 NVARCHAR2(200), 
 DISTRICT_ID NUMBER(5), 
 SUB_DISTRICT_ID NUMBER(5), 
 CITY_ID NUMBER(5), 
 PROVINCE_ID NUMBER(5), 
 POSTAL_CODE NUMBER(6), 
 MARITAL_STATUS NUMBER(2), 
 OCCUPATION NVARCHAR2(20), 
 NATIONALITY NUMBER(4), 
 VALID_UNTIL DATE, 
 REGISTRATION_THROUGH NUMBER(2),
 QUEUE_STATUS NUMBER(1)
);

ALTER TABLE TBL_CUSTOMER_ACCOUNT
RENAME COLUMN ADDRESS TO ADDRESS_LINE_1;

ALTER TABLE TBL_CUSTOMER_ACCOUNT
RENAME COLUMN SCANNED_DOCS_PATH TO SCANNED_DOCS_PATH1;

ALTER TABLE TBL_CUSTOMER_VEHICLE ADD(
VEHICLE_REG_CERT_ID     NUMBER(20), 
ADDRESS                 NVARCHAR2(100), 
BRAND                   NUMBER(3), 
VEHICLE_TYPE            NUMBER(2), 
VEHICLE_CATEGORY        NUMBER(2), 
MODEL_NO                NVARCHAR2(50), 
MANUFACTURING_YEAR      NUMBER(5), 
CYCLINDER_CAPACITY      NVARCHAR2(20), 
FRAME_NUMBER            NVARCHAR2(20), 
ENGINE_NUMBER           NVARCHAR2(20), 
VEHICLE_COLOR           NUMBER(3), 
FUEL_TYPE               NUMBER(2), 
LICENCE_PLATE_COLOR     NUMBER(2),
REGISTRATION_YEAR       NUMBER(5), 
LOCATION_CODE           NVARCHAR2(20), 
VEHICLEIMAGE_FRONT      NVARCHAR2(200), 
VEHICLEIMAGE_REAR       NVARCHAR2(200), 
VEHICLEIMAGE_RIGHTSIDE  NVARCHAR2(200), 
VEHICLEIMAGE_LEFTSIDE   NVARCHAR2(200), 
VALID_UNTIL             NUMBER(5)

 );

