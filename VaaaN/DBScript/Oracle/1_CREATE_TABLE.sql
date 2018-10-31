
/* Formatted on 18/09/2018 10.50.07 AM (QP5 v5.326) */
/*
© Copyright 2018 VaaaN Infra (P) Ltd. All rights reserved.

This file contains Proprietary information of VaaaN Infra (P) Ltd.
and should be treated as proprietary and confidential.

The use, copying, disclosure or modification of the programs and/or 
source code is prohibited unless otherwise provided for in the license 
or confidential agreements.

========================================================
Author           :  VaaaN Infra (P) Ltd.                  
Company          :  VaaaN Infra (P) Ltd.     
Date of Creation :  14-Sep-2018                            
========================================================
*/

CREATE TABLE TBL_USER
(
    USER_ID              NUMBER (3) NOT NULL,
    LOGIN_NAME           NVARCHAR2 (100),
    FIRST_NAME           NVARCHAR2 (100),
    LAST_NAME            NVARCHAR2 (100),
    PASSWORD             NVARCHAR2 (100),
    DESCRIPTION          NVARCHAR2 (100),
    ADDRESS              NVARCHAR2 (200),
    ROLE_ID              NUMBER (3),
    ACC_EXPIRY_DATE      DATE,
    MODIFIER_ID          NUMBER (10),
    FINGER_PRINT_1       LONG,
    CREATION_DATE        DATE,
    MODIFICATION_DATE    DATE,
    MOBILE_NO            VARCHAR2 (10 BYTE),
    EMAIL_ID             NVARCHAR2 (50),
    USER_STATUS          NUMBER,
    DOB                  DATE,
    PRIMARY KEY (USER_ID)
);

CREATE TABLE TBL_ROLE
(
    ROLE_ID              NUMBER (3) NOT NULL,
    ROLE_NAME            NVARCHAR2 (100),
    DESCRIPTION          NVARCHAR2 (100),
    MODIFIER_ID          NUMBER (10),
    CREATION_DATE        DATE,
    MODIFICATION_DATE    DATE,
    IS_ACTIVE            NUMBER,
    PRIMARY KEY (ROLE_ID)
);

CREATE TABLE TBL_MODULE
(
    MODULE_ID         NUMBER (3),
    MODULE_NAME       NVARCHAR2 (100),
    IS_GUI_VISIBLE    NUMBER (1),
    MODULE_URL        NVARCHAR2 (100),
    PRIMARY KEY (MODULE_ID)
);


CREATE TABLE TBL_SUBMODULE
(
    SUBMODULE_ID      NUMBER (3) NOT NULL,
    MODULE_ID         NUMBER (3) NOT NULL,
    SUBMODULE_NAME    NVARCHAR2 (100),
    IS_GUI_VISIBLE    NUMBER (1),
    SUBMODULE_URL     NVARCHAR2 (100),
    PRIMARY KEY (SUBMODULE_ID)
);


CREATE TABLE TBL_MODULE_ACTIVITY
(
    ENTRY_ID       NUMBER (3) NOT NULL,
    MODULE_ID      NUMBER (3),
    ACTIVITY_ID    NUMBER (3),
    PRIMARY KEY (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
);


CREATE TABLE TBL_ROLE_MODULE_RIGHT
(
    ROLE_ID          NUMBER (3) NOT NULL,
    ENTRY_ID         NUMBER,
    MODULE_ID        NUMBER,
    MODULE_VIEW      NUMBER,
    MODULE_ADD       NUMBER,
    MODULE_EDIT      NUMBER,
    MODULE_DELETE    NUMBER,
    PRIMARY KEY (ROLE_ID, MODULE_ID)
);


CREATE TABLE TBL_ROLE_SUBMODULE_RIGHT
(
    ROLE_ID              NUMBER (3) NOT NULL,
    ENTRY_ID             NUMBER,
    SUBMODULE_ID         NUMBER,
    SUB_MODULE_VIEW      NUMBER,
    SUB_MODULE_ADD       NUMBER,
    SUB_MODULE_EDIT      NUMBER,
    SUB_MODULE_DELETE    NUMBER,
    PRIMARY KEY (ENTRY_ID, ROLE_ID, SUBMODULE_ID)
);



CREATE TABLE TBL_SUBMODULE_ACTIVITY
(
    ENTRY_ID        NUMBER (3) NOT NULL,
    SUBMODULE_ID    NUMBER (3),
    ACTIVITY_ID     NUMBER (3),
    PRIMARY KEY (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
);



CREATE TABLE TBL_USER_LOGIN_DETAIL
(
    ENTRY_ID       NUMBER (10) NOT NULL,
    USER_ID        NUMBER (10) NOT NULL,
    LOGIN_DATE     DATE,
    LOGOUT_DATE    DATE,
    PRIMARY KEY (ENTRY_ID)
);


CREATE TABLE TBL_USER_MODULE_RIGHT
(
    USER_ID          NUMBER (3) NOT NULL,
    ENTRY_ID         NUMBER,
    MODULE_ID        NUMBER,
    MODULE_VIEW      NUMBER,
    MODULE_ADD       NUMBER,
    MODULE_EDIT      NUMBER,
    MODULE_DELETE    NUMBER,
PRIMARY KEY (ENTRY_ID, USER_ID,MODULE_ID)
);

CREATE TABLE TBL_USER_SUBMODULE_RIGHT
(
    USER_ID              NUMBER (3) NOT NULL,
    ENTRY_ID             NUMBER,
    SUBMODULE_ID         NUMBER,
    SUB_MODULE_VIEW      NUMBER,
    SUB_MODULE_ADD       NUMBER,
    SUB_MODULE_EDIT      NUMBER,
    SUB_MODULE_DELETE    NUMBER,
PRIMARY KEY (ENTRY_ID, USER_ID,SUBMODULE_ID)
);


CREATE TABLE TBL_TOLL_RATE
(
    TMS_ID               NUMBER (2),
    PLAZA_ID             NUMBER (2),
    TRANSFER_STATUS      NUMBER (2),
    RATE_ID              NUMBER (5) NOT NULL,
    PROFILE_ID           NUMBER (2),
    VEHICLE_CLASS_ID     NUMBER (2),
    LANE_TYPE_ID         NUMBER (1),        --Normal lane=1,TransJakartaLane=2
    START_TIME           NVARCHAR2 (10),
    END_TIME             NVARCHAR2 (10),
    AMOUNT               NUMBER (10, 2),
    DESCRIPTION          NVARCHAR2 (100),
    MODIFIER_ID          NUMBER (6),
    CREATION_DATE        DATE,
    MODIFICATION_DATE    DATE,
    PRIMARY KEY
        (TMS_ID,
         PLAZA_ID,
         RATE_ID,
         PROFILE_ID,
         VEHICLE_CLASS_ID,
         LANE_TYPE_ID)
);



CREATE TABLE TBL_CROSSTALK_PACKET
(
    TMS_ID               NUMBER (2) NOT NULL,
    PLAZA_ID             NUMBER (2) NOT NULL,
    ENTRY_ID             NUMBER (10) NOT NULL,
    LANE_ID              NUMBER (10) NOT NULL,
    EVENT_TYPE           NVARCHAR2 (100),
    TIME_STAMP           NVARCHAR2 (100),
    UUID                 NVARCHAR2 (100),
    LOCATION_ID          NVARCHAR2 (100),
    PARENT_UUID          NVARCHAR2 (100),
    OBJECT_ID            NVARCHAR2 (100),
    FIRST_READ           NVARCHAR2 (100),
    LAST_READ            NVARCHAR2 (100),
    OBSERVATION_UUID     NVARCHAR2 (100),
    READS                NVARCHAR2 (100),
    MODIFIER_ID          NUMBER (3),
    CREATION_DATE        DATE,
    MODIFICATION_DATE    DATE,
    PRIMARY KEY (TMS_ID, PLAZA_ID, ENTRY_ID)
);



CREATE TABLE TBL_NODEFLUX_PACKET
(
	ENTRY_ID            	NUMBER (10) NOT NULL,
    	TMS_ID                  NUMBER (2) NOT NULL,
	EVENT_TYPE              NVARCHAR2 (100),
	TIMESTAMP               DATE,
    	GANTRY_ID               NUMBER (5) NOT NULL,
    	LANE_ID                 NUMBER (5) NOT NULL,
    	CAMERA_POSITION		NVARCHAR2 (10),   
    	CAMERA_ID               NUMBER (2),
    	CAMERA_NAME             NVARCHAR2 (100),
    	CAMERA_ADDRESS          NVARCHAR2 (100),
    	CAMERA_COORDINATE	NVARCHAR2 (100),
    	PLATE_NUMBER            NVARCHAR2 (100),
    	VEHICLE_CLASS_ID        NUMBER (5),
	VEHICLE_SPEED		NUMBER (5),
    	PLATE_THUMBNAIL         NVARCHAR2 (100),
    	VEHICLE_THUMBNAIL    	NVARCHAR2 (100),
    	VIDEO_URL	        NVARCHAR2 (100),
	CREATION_DATE           DATE,
    	MODIFIER_ID             NUMBER (3),   
    	MODIFICATION_DATE       DATE,
    	PRIMARY KEY (TMS_ID, GANTRY_ID, ENTRY_ID)
);


CREATE TABLE TBL_CUSTOMER_ACCOUNT
(
    TMS_ID                 NUMBER (2),
    ACCOUNT_ID             NUMBER (10) NOT NULL,
    FIRST_NAME             NVARCHAR2 (50),
    LAST_NAME              NVARCHAR2 (50),
    MOB_NUMBER             NVARCHAR2 (20),
    EMAIL_ID               NVARCHAR2 (100),
    DESCRIPTION            NVARCHAR2 (100),
    ADDRESS                NVARCHAR2 (200),
    ACCOUNT_BALANCE        NUMBER (10, 2),
    MODIFIER_ID            NUMBER (6),
    CREATION_DATE          DATE,
    MODIFICATION_DATE      DATE,
    CUSTOMER_IMAGE_PATH    NVARCHAR2 (100),
    SCANNED_DOCS_PATH      NVARCHAR2 (100),
    IS_DOC_VERIFIED        NUMBER (1),
    ACCOUNT_STATUS         NUMBER (1),
    TRANSFER_STATUS        NUMBER (1),
    PRIMARY KEY (TMS_ID, ACCOUNT_ID)
);

CREATE TABLE TBL_CUSTOMER_VEHICLE
(
    TMS_ID               NUMBER (2),
    ENTRY_ID             NUMBER (10) NOT NULL,
    ACCOUNT_ID           NUMBER (10),
    VEH_REG_NO           NVARCHAR2 (20),
    TAG_ID               NVARCHAR2 (50),
    VEHICLE_CLASS_ID     NUMBER (2),
    CREATION_DATE        DATE,
    MODIFICATION_DATE    DATE,
    MODIFIED_BY          NUMBER (10),
    TRANSFER_STATUS      NUMBER (1),
    PRIMARY KEY (TMS_ID, ENTRY_ID)
);


CREATE TABLE TBL_HARDWARE
(
    TMS_ID               NUMBER (2),
    PLAZA_ID             NUMBER (2),
    HARDWARE_ID          NUMBER (10) NOT NULL,
    HARDWARE_NAME        NVARCHAR2 (50),
    HARDWARE_TYPE        NUMBER (1),                   --CAMERA=1,RFID=2,VMS=3
    HARDWARE_POSITION        NUMBER (1),                   --FRONT=1, REAR=2 only for camera
    MANUFACTURER_NAME    NVARCHAR2 (100),
    MODEL_NAME           NVARCHAR2 (100),
    IP_ADDRESS           NVARCHAR2 (20),
    CREATION_DATE        DATE,
    MODIFICATION_DATE    DATE,
    MODIFIED_BY          NUMBER (10),
    TRANSFER_STATUS      NUMBER (1),
    PRIMARY KEY (TMS_ID, HARDWARE_ID)
);


CREATE TABLE TBL_LANE
(
    TMS_ID               NUMBER (2),
    PLAZA_ID             NUMBER (2),
    LANE_ID              NUMBER (3) NOT NULL,
    LANE_TYPE_ID         NUMBER(1) DEFAULT 1,
    LANE_NAME            NVARCHAR2 (50),
    CAMERA_ID_FRONT            NUMBER (4),
CAMERA_ID_REAR            NUMBER (4),
    ETC_ANTENNA_ID_FRONT        NUMBER (4),
    ETC_ANTENNA_ID_REAR        NUMBER (4),
    CREATION_DATE        DATE,
    MODIFICATION_DATE    DATE,
    MODIFIED_BY          NUMBER (10),
    TRANSFER_STATUS      NUMBER (1),
    PRIMARY KEY (TMS_ID, PLAZA_ID, LANE_ID)
);


CREATE TABLE TBL_ACCOUNT_HISTORY
(
    TMS_ID                       NUMBER (2),
    ACCOUNT_ID                   NUMBER (10),
    ENTRY_ID                     NUMBER (3) NOT NULL,
    CUSTOMER_VEHICLE_ENTRY_ID    NUMBER (10),
    TRANSACTION_TYPE             NUMBER (1), --SALE=1, RECHARGE=2, FINE, PENALTY, REFUND
    AMOUNT                       NUMBER (10, 2),
    TRANSACTION_ID               NUMBER (10),
    SMS_SENT                     NUMBER (1),
    EMAIL_SENT                   NUMBER (1),
    CREATION_DATE                DATE,
    MODIFIED_BY                  NUMBER (10),
    TRANSFER_STATUS              NUMBER (1),
    PRIMARY KEY (TMS_ID, ENTRY_ID)
);

 

CREATE TABLE TBL_VEHICLE_CLASS
(
    TMS_ID               NUMBER (2),
    VEHICLE_CLASS_ID     NUMBER (1) NOT NULL,
    VEHICLE_CLASS_NAME   NVARCHAR2 (20),
    CREATION_DATE        DATE,
    MODIFICATION_DATE    DATE,
    MODIFIED_BY          NUMBER (10),
    TRANSFER_STATUS      NUMBER (1),
    PRIMARY KEY (TMS_ID,  VEHICLE_CLASS_ID)
);

CREATE TABLE TBL_SMS_COMM_HISTORY
(
    ENTRY_ID                   NUMBER (10),
    TMS_ID                     NUMBER (2),
    CUSTOMER_ACCOUNT_ID        NUMBER (10),
    CUSTOMER_NAME              NVARCHAR2 (50),
    MOBILE_NUMBER              NVARCHAR2 (20),
    MESSAGE_DIRECTION          NUMBER (1),             --INCOMING=1,OUTGOING=2
    MESSAGE_BODY               NVARCHAR2 (200),
    SENT_STATUS                NUMBER (1),         --UNSENT=1,SENT=2,IGNORED=3
    RECEIVED_PROCESS_STATUS    NUMBER (1),         --UNPROCESSED=1,PROCESSED=2
    MESSAGE_SEND_TIME          DATE,
    MESSAGE_RECEIVE_TIME       DATE,
    MESSAGE_DELIVERY_STATUS    NUMBER (1), --UNKNOWN=0,DELIVERED=1,UNDELIVERED=2
    ATTEMPT_COUNT              NUMBER(1),
    CREATION_DATE              DATE,
    MODIFICATION_DATE          DATE,
    MODIFIED_BY                NUMBER (10),
    PRIMARY KEY (TMS_ID, ENTRY_ID, CUSTOMER_ACCOUNT_ID)
);

CREATE TABLE TBL_TRANSACTION
(
	TMS_ID                      NUMBER(2),
	PLAZA_ID                    NUMBER(2),
	LANE_ID                     NUMBER(2), --To be determined by which RFID reporting it
	TRANSACTION_ID              NUMBER(20)NOT NULL,--Primary Key of Table
	TRANSACTION_DATETIME         DATE,
	CT_ENTRY_ID           NUMBER(10), -- Link from nodeflux packet table, this field may be remain empty
	NF_ENTRY_ID_FRONT           NUMBER(10), -- Link from nodeflux packet table, this field may be remain empty
	NF_ENTRY_ID_REAR           NUMBER(10), -- Link from nodeflux packet table, this field may be remain empty
	IS_BALANCE_UPDATED          NUMBER(1),
	IS_TRANSFERED               NUMBER(1),
	IS_VIOLATION                NUMBER(1), --When the tag vehicle class mismatched with nodeflux class ==> required human audit.undefined if nodeflux vrn is not available ==> required audit
	MODIFIER_ID                 NUMBER(1),
	CREATION_DATE               DATE,
	MODIFICATION_DATE           DATE,
	AUDIT_STATUS                NUMBER(1) DEFAULT 0,
	AUDITOR_ID		    NUMBER(2),
	AUDIT_DATE              DATE,
	AUDITED_VEHICLE_CLASS_ID    NUMBER(2),
	AUDITED_VRN                  NVARCHAR2(20),
	PRIMARY KEY (TMS_ID, PLAZA_ID,LANE_ID, TRANSACTION_ID)
);

CREATE TABLE TBL_PLAZA
(
    TMS_ID                NUMBER(2),
    PLAZA_ID              NUMBER(2) NOT NULL,
    PLAZA_NAME            NVARCHAR2(50),
    LOCATION              NVARCHAR2(100),
    IPADDRESS             NVARCHAR2(20),
    MODIFIER_ID           NUMBER(2),
    CREATION_DATE         DATE,
    MODIFICATION_DATE     DATE,
    PRIMARY KEY (TMS_ID, PLAZA_ID)
);

CREATE SEQUENCE USER_SEQ START WITH 1
                         MAXVALUE 9999999999
                         MINVALUE 1
                         NOCYCLE
                         NOCACHE
                         NOORDER;


CREATE SEQUENCE USER_LOGIN_HISTORY_SEQ START WITH 1
                                       MAXVALUE 9999999999
                                       MINVALUE 1
                                       NOCYCLE
                                       NOCACHE
                                       NOORDER;

CREATE SEQUENCE CROSSTALK_PACKET_SEQ START WITH 1
                                     MAXVALUE 9999999999
                                     MINVALUE 1
                                     NOCYCLE
                                     NOCACHE
                                     NOORDER;

CREATE SEQUENCE NODEFLUX_PACKET_SEQ START WITH 1
                                    MAXVALUE 9999999999
                                    MINVALUE 1
                                    NOCYCLE
                                    NOCACHE
                                    NOORDER;

CREATE SEQUENCE SMS_COMM_HISTORY_SEQ START WITH 1
                                    MAXVALUE 9999999999
                                    MINVALUE 1
                                    NOCYCLE
                                    NOCACHE
                                    NOORDER;
CREATE SEQUENCE TOLL_RATE_SEQ START WITH 1
                         MAXVALUE 9999999999
                         MINVALUE 1
                         NOCYCLE
                         NOCACHE
                         NOORDER;
CREATE SEQUENCE CUSTOMER_ACCOUNT_SEQ START WITH 1
                         MAXVALUE 9999999999
                         MINVALUE 1
                         NOCYCLE
                         NOCACHE
                         NOORDER;
						 

 CREATE SEQUENCE TRANSACTION_SEQ START WITH 1
				 MAXVALUE 9999999999
				 MINVALUE 1
				 NOCYCLE
				 NOCACHE
				 NOORDER;
				 
	
	CREATE SEQUENCE ACCOUNT_HISTORY_SEQ START WITH 1
				 MAXVALUE 9999999999
				 MINVALUE 1
				 NOCYCLE
				 NOCACHE
				 NOORDER;



COMMIT;