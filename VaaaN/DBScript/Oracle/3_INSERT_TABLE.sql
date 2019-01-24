/* Formatted on 05/09/2018 12:26:09 PM (QP5 v5.215.12089.38647) */
/*User*/

SET DEFINE OFF;
Insert into TBL_USER
   (USER_ID, LOGIN_NAME, FIRST_NAME, LAST_NAME, PASSWORD, 
    ROLE_ID, ACC_EXPIRY_DATE, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE, 
    USER_STATUS)
 Values
   (1, 'admin', 'System ', 'Administrator', '19a2854144b63a8f7617a6f225019b12', 
    1, TO_DATE('02/01/2050 12:55:46', 'MM/DD/YYYY HH24:MI:SS'), 1, TO_DATE('08/09/2018 15:50:53', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('08/09/2018 15:50:53', 'MM/DD/YYYY HH24:MI:SS'), 
    1);

/* MODULE */

/* Formatted on 18/09/2018 10:19:10 AM (QP5 v5.215.12089.38647) */
INSERT INTO TBL_MODULE (MODULE_ID,
                        MODULE_NAME,
                        IS_GUI_VISIBLE,
                        MODULE_URL)
     VALUES (1,
             'Dasboard',
             1,
             '/home/UserDashboard');

INSERT INTO TBL_MODULE (MODULE_ID,
                        MODULE_NAME,
                        IS_GUI_VISIBLE,
                        MODULE_URL)
     VALUES (2,
             'Admin',
             1,
             '');

INSERT INTO TBL_MODULE (MODULE_ID,
                        MODULE_NAME,
                        IS_GUI_VISIBLE,
                        MODULE_URL)
     VALUES (3,
             'Auditing',
             1,
             '/home/Report');

INSERT INTO TBL_MODULE (MODULE_ID,
                        MODULE_NAME,
                        IS_GUI_VISIBLE,
                        MODULE_URL)
     VALUES (4,
             'Live Monitoring',
             1,
             '/home/LiveMonitoringList');

INSERT INTO TBL_MODULE (MODULE_ID,
                        MODULE_NAME,
                        IS_GUI_VISIBLE,
                        MODULE_URL)
     VALUES (5,
             'VMS Controller',
             1,
             '');

INSERT INTO TBL_MODULE (MODULE_ID,
                        MODULE_NAME,
                        IS_GUI_VISIBLE,
                        MODULE_URL)
     VALUES (6,
             'POS',
             1,
             '');

/*---------------------------------Submodules----------------*/
/* Formatted on 18/09/2018 10:24:29 AM (QP5 v5.215.12089.38647) */
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (1,
             2,
             'Users',
             1,
             '/home/UsersList');
             
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (2,
             2,
             'Roles',
             1,
             '/home/RoleList');
             
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (3,
             2,
             'Plaza',
             1,
             '/home/PlazaList');
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (4,
             2,
             'Lane',
             1,
             '/home/LaneList');  
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (5,
             2,
             'Vehicle Class',
             1,
             '/home/VehicleClassList');      
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (6,
             2,
             'Fare',
             1,
             '/home/TollRateList');
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (7,
             2,
             'Hardware',
             1,
             '/home/HardwareList');
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (8,
             2,
             'Customer',
             1,
             '/home/CustomerList');
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (9,
             2,
             'Customer Vehicle',
             1,
             '/home/CustomerVehicleList');
INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                       MODULE_ID,
                       SUBMODULE_NAME,
                       IS_GUI_VISIBLE,
                       SUBMODULE_URL)
     VALUES (10,
             2,
             'Account History',
             1,
             '/home/AccountHistoryList');                                             

/*------------------------------------------Roles-------------------------*/
Insert into TBL_ROLE
   (ROLE_ID, ROLE_NAME, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE, 
    IS_ACTIVE)
 Values
   (2, 'Admin', 1, TO_DATE('08/31/2018 11:44:11', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('08/31/2018 11:44:11', 'MM/DD/YYYY HH24:MI:SS'), 
    1);
Insert into  TBL_ROLE
   (ROLE_ID, ROLE_NAME, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE, 
    IS_ACTIVE)
 Values
   (4, 'Manager', 1, TO_DATE('09/10/2018 13:13:24', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/10/2018 13:13:24', 'MM/DD/YYYY HH24:MI:SS'), 
    1);
Insert into  TBL_ROLE
   (ROLE_ID, ROLE_NAME, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE, 
    IS_ACTIVE)
 Values
   (1, 'Super Admin', 1, TO_DATE('08/31/2018 11:44:11', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('08/31/2018 11:44:11', 'MM/DD/YYYY HH24:MI:SS'), 
    1);
Insert into TBL_ROLE
   (ROLE_ID, ROLE_NAME, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE, 
    IS_ACTIVE)
 Values
   (3, 'Auditor', 1, TO_DATE('09/04/2018 11:18:42', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/04/2018 11:18:42', 'MM/DD/YYYY HH24:MI:SS'), 
    1);
/*-----------------------module_activity--------------------------*/
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (1, 1, 1);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (2, 2, 1);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (3, 2, 5);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (4, 3, 1);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (5, 3, 5);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (6, 4, 1);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (7, 4, 5);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (8, 5, 1);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (9, 5, 5);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (10, 6, 1);
Insert into TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (11, 7, 1);
Insert into  TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (12, 8, 1);
Insert into  TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (13, 9, 1);
Insert into  TBL_MODULE_ACTIVITY
   (ENTRY_ID, MODULE_ID, ACTIVITY_ID)
 Values
   (14, 9, 1);

/*-----------------------submodule_activity--------------------------*/
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (1, 1, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (2, 2, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (3, 3, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (4, 4, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (5, 4, 2);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (6, 4, 3);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (7, 4, 4);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (8, 5, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (9, 5, 2);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (10, 5, 3);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (11, 5, 4);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (12, 6, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (13, 6, 2);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (14, 6, 3);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (15, 6, 4);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (16, 7, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (17, 7, 2);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (18, 7, 3);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (19, 7, 4);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (20, 8, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (21, 8, 2);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (22, 8, 3);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (23, 8, 4);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (24, 9, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (25, 9, 2);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (26, 9, 3);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (27, 9, 4);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (28, 10, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (29, 10, 2);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (30, 10, 3);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (31, 10, 4);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (32, 11, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (33, 12, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (34, 12, 5);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (35, 13, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (36, 13, 5);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (37, 14, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (38, 14, 5);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (39, 15, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (40, 15, 5);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (41, 16, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (42, 17, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (43, 17, 5);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (44, 18, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (45, 18, 5);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (46, 19, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (47, 19, 5);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (48, 22, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (49, 23, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (50, 24, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (51, 25, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (52, 26, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (53, 27, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (54, 28, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (55, 29, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (56, 30, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (57, 31, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (58, 32, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (59, 33, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (60, 34, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (61, 35, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (62, 36, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (63, 37, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (64, 38, 1);
Insert into  TBL_SUBMODULE_ACTIVITY
   (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
 Values
   (65, 39, 1);

/*-----------------------user_module_right--------------------------*/

Insert into TBL_USER_MODULE_RIGHT
   (USER_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 4, 4, 1, 1, 
    1, 1);
Insert into TBL_USER_MODULE_RIGHT
   (USER_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 5, 5, 1, 1, 
    1, 1);
Insert into TBL_USER_MODULE_RIGHT
   (USER_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 6, 6, 1, 1, 
    1, 1);
Insert into TBL_USER_MODULE_RIGHT
   (USER_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 1, 1, 1, 1, 
    1, 1);
Insert into TBL_USER_MODULE_RIGHT
   (USER_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 2, 2, 1, 1, 
    1, 1);
Insert into TBL_USER_MODULE_RIGHT
   (USER_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 3, 3, 1, 1, 
    1, 1);


/*-----------------------user_submodule_right--------------------------*/
Insert into TBL_USER_SUBMODULE_RIGHT
   (USER_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 5, 5, 1, 1, 
    1, 1);
Insert into TBL_USER_SUBMODULE_RIGHT
   (USER_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 6, 6, 1, 1, 
    1, 1);
Insert into TBL_USER_SUBMODULE_RIGHT
   (USER_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 7, 7, 1, 1, 
    1, 1);
Insert into TBL_USER_SUBMODULE_RIGHT
   (USER_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 1, 1, 1, 1, 
    1, 1);
Insert into TBL_USER_SUBMODULE_RIGHT
   (USER_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 2, 2, 1, 1, 
    1, 1);
Insert into TBL_USER_SUBMODULE_RIGHT
   (USER_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 3, 3, 1, 1, 
    1, 1);
Insert into TBL_USER_SUBMODULE_RIGHT
   (USER_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 4, 4, 1, 1, 
    1, 1);
/*-----------------------role_module_right--------------------------*/
Insert into TBL_ROLE_MODULE_RIGHT
   (ROLE_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 1, 1, 1, 1, 
    1, 1);
Insert into TBL_ROLE_MODULE_RIGHT
   (ROLE_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 2, 2, 1, 1, 
    1, 1);
Insert into TBL_ROLE_MODULE_RIGHT
   (ROLE_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 3, 3, 1, 1, 
    1, 1);
Insert into TBL_ROLE_MODULE_RIGHT
   (ROLE_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 4, 4, 1, 1, 
    1, 1);
Insert into TBL_ROLE_MODULE_RIGHT
   (ROLE_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 5, 5, 1, 1, 
    1, 1);
Insert into TBL_ROLE_MODULE_RIGHT
   (ROLE_ID, ENTRY_ID, MODULE_ID, MODULE_VIEW, MODULE_ADD, 
    MODULE_EDIT, MODULE_DELETE)
 Values
   (1, 6, 6, 1, 1, 
    1, 1);

/*-----------------------role_module_right--------------------------*/

/*-----------------------role_submodule_right--------------------------*/

Insert into TBL_ROLE_SUBMODULE_RIGHT
   (ROLE_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 3, 3, 1, 1, 
    1, 1);
Insert into TBL_ROLE_SUBMODULE_RIGHT
   (ROLE_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 4, 4, 1, 1, 
    1, 1);
Insert into TBL_ROLE_SUBMODULE_RIGHT
   (ROLE_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 5, 5, 1, 1, 
    1, 1);
Insert into TBL_ROLE_SUBMODULE_RIGHT
   (ROLE_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 6, 6, 1, 1, 
    1, 1);
Insert into TBL_ROLE_SUBMODULE_RIGHT
   (ROLE_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 7, 7, 1, 1, 
    1, 1);
Insert into TBL_ROLE_SUBMODULE_RIGHT
   (ROLE_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 1, 1, 1, 1, 
    1, 1);
Insert into TBL_ROLE_SUBMODULE_RIGHT
   (ROLE_ID, ENTRY_ID, SUBMODULE_ID, SUB_MODULE_VIEW, SUB_MODULE_ADD, 
    SUB_MODULE_EDIT, SUB_MODULE_DELETE)
 Values
   (1, 2, 2, 1, 1, 
    1, 1);

/*-----------------------tbl_vehicle-class--------------------------*/
Insert into TBL_VEHICLE_CLASS
   (TMS_ID, VEHICLE_CLASS_ID, VEHICLE_CLASS_NAME, CREATION_DATE, 
    MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1,  1, 'MotorCycle', SYSDATE, 
    SYSDATE, 1, 1);
Insert into TBL_VEHICLE_CLASS
   (TMS_ID, VEHICLE_CLASS_ID, VEHICLE_CLASS_NAME, CREATION_DATE, 
    MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1,  2, 'Small Car', SYSDATE, 
    SYSDATE, 1, 1);
Insert into TBL_VEHICLE_CLASS
   (TMS_ID, VEHICLE_CLASS_ID, VEHICLE_CLASS_NAME, CREATION_DATE, 
    MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1,  3, 'Medium Car', SYSDATE, 
    SYSDATE, 1, 1);
Insert into TBL_VEHICLE_CLASS
   (TMS_ID, VEHICLE_CLASS_ID, VEHICLE_CLASS_NAME, CREATION_DATE, 
    MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1,  4, 'Big Car', SYSDATE, 
    SYSDATE, 1, 1);

/*-----------------------tbl_CUSTOMER_VEHICLE--------------------------*/
Insert into TBL_CUSTOMER_VEHICLE
   (TMS_ID, ENTRY_ID, ACCOUNT_ID, VEH_REG_NO, TAG_ID, 
    VEHICLE_CLASS_ID, CREATION_DATE, MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1, 5, 5, 'CH 06 Z 3450', '125456789105', 
    2, TO_DATE('09/18/2018 12:25:47', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:25:47', 'MM/DD/YYYY HH24:MI:SS'), 1, 1);
Insert into TBL_CUSTOMER_VEHICLE
   (TMS_ID, ENTRY_ID, ACCOUNT_ID, VEH_REG_NO, TAG_ID, 
    VEHICLE_CLASS_ID, CREATION_DATE, MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1, 1, 1, 'HR 51 Z 5034', '125456789101', 
    1, TO_DATE('09/18/2018 12:23:13', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:23:13', 'MM/DD/YYYY HH24:MI:SS'), 1, 1);
Insert into TBL_CUSTOMER_VEHICLE
   (TMS_ID, ENTRY_ID, ACCOUNT_ID, VEH_REG_NO, TAG_ID, 
    VEHICLE_CLASS_ID, CREATION_DATE, MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1, 2, 2, 'DL 06 Z 3450', '125456789102', 
    2, TO_DATE('09/18/2018 12:23:13', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:23:13', 'MM/DD/YYYY HH24:MI:SS'), 1, 1);
Insert into TBL_CUSTOMER_VEHICLE
   (TMS_ID, ENTRY_ID, ACCOUNT_ID, VEH_REG_NO, TAG_ID, 
    VEHICLE_CLASS_ID, CREATION_DATE, MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1, 3, 3, 'DL8CAG2893', '1234567890', 
    3, TO_DATE('09/18/2018 12:23:49', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:23:49', 'MM/DD/YYYY HH24:MI:SS'), 1, 1);
Insert into TBL_CUSTOMER_VEHICLE
   (TMS_ID, ENTRY_ID, ACCOUNT_ID, VEH_REG_NO, TAG_ID, 
    VEHICLE_CLASS_ID, CREATION_DATE, MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS)
 Values
   (1, 4, 4, 'X 06 Z 3450', '125456789104', 
    1, TO_DATE('09/18/2018 12:24:38', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:24:38', 'MM/DD/YYYY HH24:MI:SS'), 1, 1);

/*-----------------------tbl_CUSTOMER_ACCOUNT--------------------------*/
Insert into TBL_CUSTOMER_ACCOUNT
   (TMS_ID, ACCOUNT_ID, FIRST_NAME, LAST_NAME, MOB_NUMBER, 
    EMAIL_ID, DESCRIPTION, ADDRESS, ACCOUNT_BALANCE, MODIFIER_ID, 
    CREATION_DATE, MODIFICATION_DATE, CUSTOMER_IMAGE_PATH,  IS_DOC_VERIFIED, 
    ACCOUNT_STATUS, TRANSFER_STATUS)
 Values
   (1, 1, 'Manoj', 'Verma', '9711387656', 
    'manoj.verma@vaaaninfra.com', 'Manoj verma From Faridabad', 'Faridabad', 500, 1, 
    TO_DATE('09/18/2018 12:04:16', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:04:16', 'MM/DD/YYYY HH24:MI:SS'), 'Manoj.jpg', 1, 
    1, 1);
Insert into TBL_CUSTOMER_ACCOUNT
   (TMS_ID, ACCOUNT_ID, FIRST_NAME, LAST_NAME, MOB_NUMBER, 
    EMAIL_ID, DESCRIPTION, ADDRESS, ACCOUNT_BALANCE, MODIFIER_ID, 
    CREATION_DATE, MODIFICATION_DATE, CUSTOMER_IMAGE_PATH,  IS_DOC_VERIFIED, 
    ACCOUNT_STATUS, TRANSFER_STATUS)
 Values
   (1, 2, 'Mridul', 'Buragohain', '9871277497', 
    'mridul.buragohain@vaaaninfra.com', 'Manoj verma From Ghaziabad', 'Ghaziabad', 500, 1, 
    TO_DATE('09/18/2018 12:12:59', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:12:59', 'MM/DD/YYYY HH24:MI:SS'), 'Mridul.jpg', 1, 
    1, 1);
Insert into TBL_CUSTOMER_ACCOUNT
   (TMS_ID, ACCOUNT_ID, FIRST_NAME, LAST_NAME, MOB_NUMBER, 
    EMAIL_ID, DESCRIPTION, ADDRESS, ACCOUNT_BALANCE, MODIFIER_ID, 
    CREATION_DATE, MODIFICATION_DATE, CUSTOMER_IMAGE_PATH,  IS_DOC_VERIFIED, 
    ACCOUNT_STATUS, TRANSFER_STATUS)
 Values
   (1, 3, 'Hemant', 'Kumar', '7840028802', 
    'hemant.kumar@vaaaninfra.com', 'Hemant Kumar From Rohini', 'Rohini', 500, 1, 
    TO_DATE('09/18/2018 12:12:59', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:12:59', 'MM/DD/YYYY HH24:MI:SS'), 'Hemant.jpg', 1, 
    1, 1);
Insert into TBL_CUSTOMER_ACCOUNT
   (TMS_ID, ACCOUNT_ID, FIRST_NAME, LAST_NAME, MOB_NUMBER, 
    EMAIL_ID, DESCRIPTION, ADDRESS, ACCOUNT_BALANCE, MODIFIER_ID, 
    CREATION_DATE, MODIFICATION_DATE, CUSTOMER_IMAGE_PATH,  IS_DOC_VERIFIED, 
    ACCOUNT_STATUS, TRANSFER_STATUS)
 Values
   (1, 4, 'Yogesh', 'kumar', '9811559129', 
    'yogesh.kumar@vaaaninfra.com', 'Yogesh kumar From Delhi', 'Delhi', 500, 1, 
    TO_DATE('09/18/2018 12:12:59', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:12:59', 'MM/DD/YYYY HH24:MI:SS'), 'Yogesh.jpg', 1, 
    1, 1);
Insert into TBL_CUSTOMER_ACCOUNT
   (TMS_ID, ACCOUNT_ID, FIRST_NAME, LAST_NAME, MOB_NUMBER, 
    EMAIL_ID, DESCRIPTION, ADDRESS, ACCOUNT_BALANCE, MODIFIER_ID, 
    CREATION_DATE, MODIFICATION_DATE, CUSTOMER_IMAGE_PATH,  IS_DOC_VERIFIED, 
    ACCOUNT_STATUS, TRANSFER_STATUS)
 Values
   (1, 5, 'Arun', 'kumar', '9650233369', 
    'arun.kumar@vaaaninfra.com', 'Arun kumar From Gurgaon', 'Gurgaon', 500, 1, 
    TO_DATE('09/18/2018 12:12:59', 'MM/DD/YYYY HH24:MI:SS'), TO_DATE('09/18/2018 12:12:59', 'MM/DD/YYYY HH24:MI:SS'), 'Arun.jpg', 1, 
    1, 1);

/* -------------tbl_plaza----------------------- */
INSERT INTO TBL_PLAZA (TMS_ID,
                       PLAZA_ID,
                       PLAZA_NAME,
                       LOCATION,
                       IPADDRESS,
                       MODIFIER_ID,
                       CREATION_DATE,
                       MODIFICATION_DATE)
     VALUES (1,
             1,
             'Gantry 1',
             'Jakarta',
             '192.168.1.10',
             1,
             SYSDATE,
             SYSDATE);
INSERT INTO TBL_PLAZA (TMS_ID,
                       PLAZA_ID,
                       PLAZA_NAME,
                       LOCATION,
                       IPADDRESS,
                       MODIFIER_ID,
                       CREATION_DATE,
                       MODIFICATION_DATE)
     VALUES (1,
             2,
             'Gantry 2',
             'Jakarta',
             '192.168.1.207',
             1,
             SYSDATE,
             SYSDATE);             
INSERT INTO TBL_PLAZA (TMS_ID,
                       PLAZA_ID,
                       PLAZA_NAME,
                       LOCATION,
                       IPADDRESS,
                       MODIFIER_ID,
                       CREATION_DATE,
                       MODIFICATION_DATE)
     VALUES (1,
             3,
             'Gantry 3',
             'Jakarta',
             '192.168.1.207',
             1,
             SYSDATE,
             SYSDATE);           
/* ---TBL_HARDWARE----- */
INSERT INTO TBL_HARDWARE (TMS_ID,
                          PLAZA_ID,
                          HARDWARE_ID,
                          HARDWARE_NAME,
                          HARDWARE_TYPE,
                          MANUFACTURER_NAME,
                          MODEL_NAME,
                          IP_ADDRESS,
                          CREATION_DATE,
                          MODIFICATION_DATE,
                          MODIFIED_BY,
                          TRANSFER_STATUS)
     VALUES (1,
             1,
             1,
             'Anpr Cam 1',
             1,
             'Samsung',
             '2018',
             '192.168.1.10',
             SYSDATE,
             SYSDATE,
             1,
             1);

INSERT INTO TBL_HARDWARE (TMS_ID,
                          PLAZA_ID,
                          HARDWARE_ID,
                          HARDWARE_NAME,
                          HARDWARE_TYPE,
                          MANUFACTURER_NAME,
                          MODEL_NAME,
                          IP_ADDRESS,
                          CREATION_DATE,
                          MODIFICATION_DATE,
                          MODIFIED_BY,
                          TRANSFER_STATUS)
     VALUES (1,
             1,
             2,
             'Anpr Cam 2',
             1,
             'SONY',
             '2018',
             '192.168.1.207',
             SYSDATE,
             SYSDATE,
             1,
             1);

INSERT INTO TBL_HARDWARE (TMS_ID,
                          PLAZA_ID,
                          HARDWARE_ID,
                          HARDWARE_NAME,
                          HARDWARE_TYPE,
                          MANUFACTURER_NAME,
                          MODEL_NAME,
                          IP_ADDRESS,
                          CREATION_DATE,
                          MODIFICATION_DATE,
                          MODIFIED_BY,
                          TRANSFER_STATUS)
     VALUES (1,
             1,
             3,
             'Anpr Cam 3',
             1,
             'SANSUI',
             '2018',
             '192.168.1.193',
             SYSDATE,
             SYSDATE,
             1,
             1);

INSERT INTO TBL_HARDWARE (TMS_ID,
                          PLAZA_ID,
                          HARDWARE_ID,
                          HARDWARE_NAME,
                          HARDWARE_TYPE,
                          MANUFACTURER_NAME,
                          MODEL_NAME,
                          IP_ADDRESS,
                          CREATION_DATE,
                          MODIFICATION_DATE,
                          MODIFIED_BY,
                          TRANSFER_STATUS)
     VALUES (1,
             1,
             4,
             'RFID READER 1',
             2,
             'Samsung',
             '2018',
             '192.168.1.103',
             SYSDATE,
             SYSDATE,
             1,
             1);

INSERT INTO TBL_HARDWARE (TMS_ID,
                          PLAZA_ID,
                          HARDWARE_ID,
                          HARDWARE_NAME,
                          HARDWARE_TYPE,
                          MANUFACTURER_NAME,
                          MODEL_NAME,
                          IP_ADDRESS,
                          CREATION_DATE,
                          MODIFICATION_DATE,
                          MODIFIED_BY,
                          TRANSFER_STATUS)
     VALUES (1,
             1,
             5,
             'RFID READER 2',
             2,
             'SONY',
             '2018',
             '192.168.1.63',
             SYSDATE,
             SYSDATE,
             1,
             1);

INSERT INTO TBL_HARDWARE (TMS_ID,
                          PLAZA_ID,
                          HARDWARE_ID,
                          HARDWARE_NAME,
                          HARDWARE_TYPE,
                          MANUFACTURER_NAME,
                          MODEL_NAME,
                          IP_ADDRESS,
                          CREATION_DATE,
                          MODIFICATION_DATE,
                          MODIFIED_BY,
                          TRANSFER_STATUS)
     VALUES (1,
             1,
             6,
             'RFID READER 3',
             2,
             'SANSUI',
             '2018',
             '192.168.1.15',
             SYSDATE,
             SYSDATE,
             1,
             1);  
/*----------------TBL_LANE---------------*/
Insert into TBL_LANE
   (TMS_ID, PLAZA_ID, LANE_ID, LANE_NAME, CAMERA_ID_FRONT, CAMERA_ID_REAR,
    ETC_READER_ID, CREATION_DATE, MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS, 
    LANE_TYPE_ID)
 Values
   (1, 1, 1, 'LANE 1', 1, 1,
    4, TO_DATE('24/09/2018 3.44.46 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('24/09/2018 3.44.46 PM', 'DD/MM/YYYY HH:MI:SS AM'), 1, 1, 
    1);
Insert into TBL_LANE
   (TMS_ID, PLAZA_ID, LANE_ID, LANE_NAME, CAMERA_ID_FRONT, CAMERA_ID_REAR,
    ETC_READER_ID, CREATION_DATE, MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS, 
    LANE_TYPE_ID)
 Values
   (1, 1, 2, 'LANE 2', 2, 2,
    5, TO_DATE('24/09/2018 3.44.46 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('24/09/2018 3.44.46 PM', 'DD/MM/YYYY HH:MI:SS AM'), 1, 1, 
    1);
Insert into TBL_LANE
   (TMS_ID, PLAZA_ID, LANE_ID, LANE_NAME, CAMERA_ID_FRONT, CAMERA_ID_REAR,
    ETC_READER_ID, CREATION_DATE, MODIFICATION_DATE, MODIFIED_BY, TRANSFER_STATUS, 
    LANE_TYPE_ID)
 Values
   (1, 1, 3, 'LANE 3', 3, 3,
    6, TO_DATE('24/09/2018 3.44.46 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('24/09/2018 3.44.46 PM', 'DD/MM/YYYY HH:MI:SS AM'), 1, 1, 
    1);

/*--------------tbl_toll_rate-----*/
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 25, 1, 
    4, 1, '06.00', '10.00', 10, 
    'big_car-normal', 1, TO_DATE('21/09/2018 6.15.24 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('21/09/2018 6.15.28 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 26, 1, 
    4, 2, '06.00', '10.00', 200, 
    'big_car-trj', 1, TO_DATE('21/09/2018 6.16.37 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('21/09/2018 6.16.43 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 27, 1, 
    4, 1, '10:00', '16:00', 5, 
    'big_car-normal', 1, TO_DATE('22/09/2018 12.43.57 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.44.02 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 28, 1, 
    4, 2, '10.00', '16.00', 100, 
    'big_car-trj', 1, TO_DATE('22/09/2018 12.46.03 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.46.07 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 29, 1, 
    4, 1, '16.00', '20.00', 10, 
    'big_car-normal', 1, TO_DATE('22/09/2018 12.48.11 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.48.15 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 30, 1, 
    4, 2, '16.00', '20.00', 200, 
    'big_car-trj', 1, TO_DATE('22/09/2018 12.49.08 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.49.11 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 31, 1, 
    4, 1, '20.00', '06.00', 0, 
    'big_car-normal', 1, TO_DATE('22/09/2018 12.50.42 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.50.45 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 32, 1, 
    4, 2, '20.00', '06.00', 100, 
    'big_car-trj', 1, TO_DATE('22/09/2018 12.51.38 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.51.41 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 1, 1, 
    1, 1, '06.00', '10.00', 10, 
    'Motorcycle-normal', 1, TO_DATE('21/09/2018 6.15.24 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('21/09/2018 6.15.28 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 2, 1, 
    1, 2, '06.00', '10.00', 100, 
    'Motorcycle-trj', 1, TO_DATE('21/09/2018 6.16.37 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('21/09/2018 6.16.43 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 3, 1, 
    1, 1, '10:00', '16:00', 5, 
    'Motorcycle-normal', 1, TO_DATE('22/09/2018 12.43.57 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.44.02 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 4, 1, 
    1, 2, '10.00', '16.00', 50, 
    'Motorcycle-trj', 1, TO_DATE('22/09/2018 12.46.03 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.46.07 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 5, 1, 
    1, 1, '16.00', '20.00', 10, 
    'Motorcycle-normal', 1, TO_DATE('22/09/2018 12.48.11 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.48.15 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 6, 1, 
    1, 2, '16.00', '20.00', 100, 
    'Motorcycle-trj', 1, TO_DATE('22/09/2018 12.49.08 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.49.11 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 7, 1, 
    1, 1, '20.00', '06.00', 0, 
    'Motorcycle-normal', 1, TO_DATE('22/09/2018 12.50.42 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.50.45 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 8, 1, 
    1, 2, '20.00', '06.00', 50, 
    'Motorcycle-trj', 1, TO_DATE('22/09/2018 12.51.38 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.51.41 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 9, 1, 
    2, 1, '06.00', '10.00', 30, 
    'Small_car-normal', 1, TO_DATE('21/09/2018 6.15.24 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('21/09/2018 6.15.28 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 10, 1, 
    2, 2, '06.00', '10.00', 200, 
    'Small_car-trj', 1, TO_DATE('21/09/2018 6.16.37 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('21/09/2018 6.16.43 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 11, 1, 
    2, 1, '10:00', '16:00', 15, 
    'Small_car-normal', 1, TO_DATE('22/09/2018 12.43.57 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.44.02 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 12, 1, 
    2, 2, '10.00', '16.00', 100, 
    'Small_car-trj', 1, TO_DATE('22/09/2018 12.46.03 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.46.07 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 13, 1, 
    2, 1, '16.00', '20.00', 30, 
    'Small_car-normal', 1, TO_DATE('22/09/2018 12.48.11 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.48.15 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 14, 1, 
    2, 2, '16.00', '20.00', 200, 
    'Small_car-trj', 1, TO_DATE('22/09/2018 12.49.08 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.49.11 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 15, 1, 
    2, 1, '20.00', '06.00', 0, 
    'Small_car-normal', 1, TO_DATE('22/09/2018 12.50.42 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.50.45 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 16, 1, 
    2, 2, '20.00', '06.00', 100, 
    'Small_car-trj', 1, TO_DATE('22/09/2018 12.51.38 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.51.41 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 17, 1, 
    3, 1, '06.00', '10.00', 10, 
    'medium_car-normal', 1, TO_DATE('21/09/2018 6.15.24 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('21/09/2018 6.15.28 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 18, 1, 
    3, 2, '06.00', '10.00', 200, 
    'medium_car-trj', 1, TO_DATE('21/09/2018 6.16.37 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('21/09/2018 6.16.43 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 19, 1, 
    3, 1, '10:00', '16:00', 5, 
    'medium_car-normal', 1, TO_DATE('22/09/2018 12.43.57 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.44.02 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 20, 1, 
    3, 2, '10.00', '16.00', 100, 
    'medium_car-trj', 1, TO_DATE('22/09/2018 12.46.03 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.46.07 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 21, 1, 
    3, 1, '16.00', '20.00', 10, 
    'medium_car-normal', 1, TO_DATE('22/09/2018 12.48.11 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.48.15 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 22, 1, 
    3, 2, '16.00', '20.00', 200, 
    'medium_car-trj', 1, TO_DATE('22/09/2018 12.49.08 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.49.11 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 23, 1, 
    3, 1, '20.00', '06.00', 0, 
    'medium_car-normal', 1, TO_DATE('22/09/2018 12.50.42 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.50.45 PM', 'DD/MM/YYYY HH:MI:SS AM'));
Insert into TBL_TOLL_RATE
   (TMS_ID, PLAZA_ID, TRANSFER_STATUS, RATE_ID, PROFILE_ID, 
    VEHICLE_CLASS_ID, LANE_TYPE_ID, START_TIME, END_TIME, AMOUNT, 
    DESCRIPTION, MODIFIER_ID, CREATION_DATE, MODIFICATION_DATE)
 Values
   (1, 1, 1, 24, 1, 
    3, 2, '20.00', '06.00', 100, 
    'medium_car-trj', 1, TO_DATE('22/09/2018 12.51.38 PM', 'DD/MM/YYYY HH:MI:SS AM'), TO_DATE('22/09/2018 12.51.41 PM', 'DD/MM/YYYY HH:MI:SS AM'));
COMMIT;



COMMIT;