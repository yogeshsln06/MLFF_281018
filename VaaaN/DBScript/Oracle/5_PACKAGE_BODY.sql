/* Formatted on 05-03-2019 15:07:56 (QP5 v5.215.12089.38647) */
CREATE OR REPLACE PACKAGE BODY MLFF.MLFF_PACKAGE
AS
   /*USER*/



   PROCEDURE USER_INSERT (P_USER_ID                OUT NUMBER,
                          P_LOGIN_NAME          IN     NVARCHAR2,
                          P_FIRST_NAME          IN     NVARCHAR2,
                          P_LAST_NAME           IN     NVARCHAR2 := NULL,
                          P_DESCRIPTION         IN     NVARCHAR2,
                          P_PASSWORD            IN     NVARCHAR2,
                          P_ADDRESS             IN     NVARCHAR2 := NULL,
                          P_ROLE_ID             IN     NUMBER,
                          P_ACC_EXPIRY_DATE     IN     DATE,
                          P_MODIFIER_ID         IN     NUMBER,
                          P_CREATION_DATE       IN     DATE,
                          P_MODIFICATION_DATE   IN     DATE,
                          P_FINGER_PRINT_1      IN     NVARCHAR2,
                          P_MOBILE_NO           IN     NVARCHAR2,
                          P_EMAIL_ID            IN     NVARCHAR2,
                          P_USER_STATUS         IN     NUMBER,
                          P_DOB                 IN     DATE)
   AS
   BEGIN
      INSERT INTO TBL_USER (USER_ID,
                            LOGIN_NAME,
                            FIRST_NAME,
                            LAST_NAME,
                            DESCRIPTION,
                            "PASSWORD",
                            ADDRESS,
                            ROLE_ID,
                            ACC_EXPIRY_DATE,
                            MODIFIER_ID,
                            CREATION_DATE,
                            MODIFICATION_DATE,
                            FINGER_PRINT_1,
                            MOBILE_NO,
                            EMAIL_ID,
                            USER_STATUS,
                            DOB)
           VALUES (USER_SEQ.NEXTVAL,
                   P_LOGIN_NAME,
                   P_FIRST_NAME,
                   P_LAST_NAME,
                   P_DESCRIPTION,
                   P_PASSWORD,
                   P_ADDRESS,
                   P_ROLE_ID,
                   P_ACC_EXPIRY_DATE,
                   P_MODIFIER_ID,
                   P_CREATION_DATE,
                   P_MODIFICATION_DATE,
                   P_FINGER_PRINT_1,
                   P_MOBILE_NO,
                   P_EMAIL_ID,
                   P_USER_STATUS,
                   P_DOB);



      INSERT INTO TBL_USER_MODULE_RIGHT (USER_ID,
                                         ENTRY_ID,
                                         MODULE_ID,
                                         MODULE_VIEW,
                                         MODULE_ADD,
                                         MODULE_EDIT,
                                         MODULE_DELETE)
         SELECT USER_SEQ.CURRVAL,
                ENTRY_ID,
                MODULE_ID,
                MODULE_VIEW,
                MODULE_ADD,
                MODULE_EDIT,
                MODULE_DELETE
           FROM TBL_ROLE_MODULE_RIGHT
          WHERE ROLE_ID = P_ROLE_ID;



      INSERT INTO TBL_USER_SUBMODULE_RIGHT (USER_ID,
                                            ENTRY_ID,
                                            SUBMODULE_ID,
                                            SUB_MODULE_VIEW,
                                            SUB_MODULE_ADD,
                                            SUB_MODULE_EDIT,
                                            SUB_MODULE_DELETE)
         SELECT USER_SEQ.CURRVAL,
                ENTRY_ID,
                SUBMODULE_ID,
                SUB_MODULE_VIEW,
                SUB_MODULE_ADD,
                SUB_MODULE_EDIT,
                SUB_MODULE_DELETE
           FROM TBL_ROLE_SUBMODULE_RIGHT
          WHERE ROLE_ID = P_ROLE_ID;

      P_USER_ID := USER_SEQ.CURRVAL;
   END USER_INSERT;



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
                          P_DOB                 IN DATE)
   AS
   BEGIN
      UPDATE TBL_USER
         SET LOGIN_NAME = P_LOGIN_NAME,
             FIRST_NAME = P_FIRST_NAME,
             LAST_NAME = P_LAST_NAME,
             DESCRIPTION = P_DESCRIPTION,
             ADDRESS = P_ADDRESS,
             ROLE_ID = P_ROLE_ID,
             ACC_EXPIRY_DATE = P_ACC_EXPIRY_DATE,
             MODIFIER_ID = P_MODIFIER_ID,
             CREATION_DATE = P_CREATION_DATE,
             MODIFICATION_DATE = P_MODIFICATION_DATE,
             FINGER_PRINT_1 = P_FINGER_PRINT_1,
             USER_STATUS = P_USER_STATUS,
             MOBILE_NO = P_MOBILE_NO,
             EMAIL_ID = P_EMAIL_ID,
             DOB = P_DOB
       WHERE USER_ID = P_USER_ID;
   END USER_UPDATE;



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
                                      P_FINGER_PRINT_1      IN NVARCHAR2)
   AS
   BEGIN
      UPDATE TBL_USER
         SET LOGIN_NAME = P_LOGIN_NAME,
             FIRST_NAME = P_FIRST_NAME,
             LAST_NAME = P_LAST_NAME,
             DESCRIPTION = P_DESCRIPTION,
             "PASSWORD" = P_PASSWORD,
             ADDRESS = P_ADDRESS,
             ROLE_ID = P_ROLE_ID,
             ACC_EXPIRY_DATE = P_ACC_EXPIRY_DATE,
             MODIFIER_ID = P_MODIFIER_ID,
             CREATION_DATE = P_CREATION_DATE,
             FINGER_PRINT_1 = P_FINGER_PRINT_1,
             MODIFICATION_DATE = P_MODIFICATION_DATE
       WHERE USER_ID = P_USER_ID;
   END USER_UPDATEWITHPASSWORD;



   PROCEDURE UPDATEPASSWORD (P_OLD_PASSWORD       IN     NVARCHAR2,
                             P_CHANGED_PASSWORD   IN     NVARCHAR2,
                             P_USER_ID            IN     NUMBER,
                             P_EMAIL_ID           IN     NVARCHAR2,
                             P_RETURNMSG             OUT NVARCHAR2)
   IS
      C_COUNT   NUMBER;



      E_COUNT   NUMBER;
   BEGIN
      C_COUNT := 0;



      E_COUNT := 0;



      P_RETURNMSG := '';



      IF (P_USER_ID != 0)
      THEN
         SELECT COUNT (*)
           INTO C_COUNT
           FROM TBL_USER
          WHERE PASSWORD = P_OLD_PASSWORD AND USER_ID = P_USER_ID;



         IF (C_COUNT > 0)
         THEN
            UPDATE TBL_USER U
               SET U.PASSWORD = P_CHANGED_PASSWORD
             WHERE USER_ID = P_USER_ID;



            P_RETURNMSG := 'changed';
         ELSE
            P_RETURNMSG := 'not changed';
         END IF;
      ELSE
         SELECT COUNT (*)
           INTO E_COUNT
           FROM TBL_USER
          WHERE EMAIL_ID = P_EMAIL_ID;



         IF (E_COUNT > 0)
         THEN
            UPDATE TBL_USER
               SET PASSWORD = P_CHANGED_PASSWORD
             WHERE EMAIL_ID = P_EMAIL_ID;



            P_RETURNMSG := 'changed';
         END IF;
      END IF;
   END UPDATEPASSWORD;



   PROCEDURE USER_DELETE (P_USER_ID IN NUMBER)
   AS
   BEGIN
      DELETE TBL_USER
       WHERE USER_ID = P_USER_ID;
   END USER_DELETE;



   PROCEDURE USER_INSERT_LOGININFO (P_ENTRY_ID        OUT NUMBER,
                                    P_USER_ID      IN     NUMBER,
                                    P_LOGIN_DATE   IN     DATE)
   AS
   BEGIN
      INSERT INTO TBL_USER_LOGIN_DETAIL (ENTRY_ID, USER_ID, LOGIN_DATE)
           VALUES (USER_LOGIN_HISTORY_SEQ.NEXTVAL, P_USER_ID, P_LOGIN_DATE);



      P_ENTRY_ID := USER_LOGIN_HISTORY_SEQ.CURRVAL;
   END USER_INSERT_LOGININFO;



   PROCEDURE USER_UPDATE_LOGININFO (P_ENTRY_ID      IN NUMBER,
                                    P_USER_ID       IN NUMBER,
                                    P_LOGOUT_DATE   IN DATE)
   AS
   BEGIN
      UPDATE TBL_USER_LOGIN_DETAIL
         SET LOGOUT_DATE = P_LOGOUT_DATE
       WHERE USER_ID = P_USER_ID AND ENTRY_ID = P_ENTRY_ID;
   END USER_UPDATE_LOGININFO;



   PROCEDURE USER_GETBYID (P_USER_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT U.USER_ID,
                  U.LOGIN_NAME,
                  U.FIRST_NAME,
                  U.LAST_NAME,
                  U."DESCRIPTION",
                  U."PASSWORD",
                  U.ADDRESS,
                  U.ROLE_ID,
                  R.ROLE_NAME,
                  U.ACC_EXPIRY_DATE,
                  U.MODIFIER_ID,
                  U.CREATION_DATE,
                  U.FINGER_PRINT_1,
                  U.MODIFICATION_DATE,
                  U.USER_STATUS,
                  U.EMAIL_ID,
                  U.MOBILE_NO,
                  U.DOB
             FROM    TBL_USER U
                  LEFT OUTER JOIN
                     TBL_ROLE R
                  ON R.ROLE_ID = U.ROLE_ID
            WHERE USER_ID = P_USER_ID
         ORDER BY U.USER_ID;
   END USER_GETBYID;



   PROCEDURE USER_GETBYNAME (P_LOGIN_NAME IN NVARCHAR2, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT U.USER_ID,
                  U.LOGIN_NAME,
                  U.FIRST_NAME,
                  U.LAST_NAME,
                  U.DESCRIPTION,
                  U."PASSWORD",
                  U.ADDRESS,
                  U.ROLE_ID,
                  R.ROLE_NAME,
                  U.ACC_EXPIRY_DATE,
                  U.MODIFIER_ID,
                  U.CREATION_DATE,
                  U.FINGER_PRINT_1,
                  U.MODIFICATION_DATE,
                  U.DOB
             FROM    TBL_USER U
                  LEFT OUTER JOIN
                     TBL_ROLE R
                  ON R.ROLE_ID = U.ROLE_ID
            WHERE LOWER (LOGIN_NAME) = LOWER (P_LOGIN_NAME)
         ORDER BY U.USER_ID;
   END USER_GETBYNAME;



   PROCEDURE GETUSERBYEMAILID (P_EMAIL_ID IN NVARCHAR2, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT U.USER_ID,
                LOGIN_NAME,
                FIRST_NAME,
                LAST_NAME,
                PASSWORD,
                MOBILE_NO,
                EMAIL_ID,
                U.ROLE_ID,
                ROLE_NAME,
                USER_STATUS,
                U.CREATION_DATE,
                U.MODIFIER_ID,
                U.MODIFICATION_DATE,
                U.ACC_EXPIRY_DATE,
                U.DESCRIPTION,
                U.ADDRESS,
                U.DOB
           FROM TBL_USER U INNER JOIN TBL_ROLE R ON U.ROLE_ID = R.ROLE_ID
          WHERE EMAIL_ID = P_EMAIL_ID;
   END GETUSERBYEMAILID;



   PROCEDURE USER_VALIDATEUSER (P_LOGIN_NAME   IN     NVARCHAR2,
                                P_PASSWORD     IN     NVARCHAR2,
                                CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT U.USER_ID,
                  U.LOGIN_NAME,
                  U.FIRST_NAME,
                  U.LAST_NAME,
                  U.DESCRIPTION,
                  U."PASSWORD",
                  U.ADDRESS,
                  U.ROLE_ID,
                  R.ROLE_NAME,
                  U.ACC_EXPIRY_DATE,
                  U.MODIFIER_ID,
                  U.CREATION_DATE,
                  U.FINGER_PRINT_1,
                  U.MODIFICATION_DATE,
                  U.USER_STATUS,
                  U.MOBILE_NO,
                  U.EMAIL_ID,
                  U.DOB
             FROM    TBL_USER U
                  LEFT OUTER JOIN
                     TBL_ROLE R
                  ON R.ROLE_ID = U.ROLE_ID
            WHERE     LOWER (LOGIN_NAME) = LOWER (P_LOGIN_NAME)
                  AND U.PASSWORD = P_PASSWORD
         ORDER BY U.USER_ID;
   END USER_VALIDATEUSER;



   PROCEDURE VALIDATELOGINNAME (P_LOGIN_NAME   IN     NVARCHAR2,
                                P_EMAIL_ID     IN     NVARCHAR2,
                                P_RETURNMSG       OUT NVARCHAR2)
   IS
      C_COUNT   NUMBER;



      E_COUNT   NUMBER;
   BEGIN
      E_COUNT := 0;



      C_COUNT := 0;



      P_RETURNMSG := '';



      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_USER
       WHERE LOGIN_NAME = P_LOGIN_NAME;



      SELECT COUNT (*)
        INTO E_COUNT
        FROM TBL_USER
       WHERE EMAIL_ID = P_EMAIL_ID;



      IF C_COUNT > 0
      THEN
         P_RETURNMSG := 'Login name found';
      ELSIF E_COUNT > 0
      THEN
         P_RETURNMSG := 'Email Id found';
      ELSIF E_COUNT > 0 AND C_COUNT > 0
      THEN
         P_RETURNMSG := 'Both found';
      ELSE
         P_RETURNMSG := 'not found';
      END IF;
   END VALIDATELOGINNAME;



   PROCEDURE USER_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT U.USER_ID,
                  U.LOGIN_NAME,
                  U.DESCRIPTION,
                  U.FIRST_NAME,
                  U.LAST_NAME,
                  U."PASSWORD",
                  U.ADDRESS,
                  U.ROLE_ID,
                  R.ROLE_NAME,
                  U.ACC_EXPIRY_DATE,
                  U.MODIFIER_ID,
                  U.CREATION_DATE,
                  U.MODIFICATION_DATE,
                  U.FINGER_PRINT_1,
                  U.EMAIL_ID,
                  U.MOBILE_NO,
                  U.USER_STATUS,
                  U.DOB
             FROM    TBL_USER U
                  LEFT OUTER JOIN
                     TBL_ROLE R
                  ON R.ROLE_ID = U.ROLE_ID
         ORDER BY U.LOGIN_NAME;
   END USER_GETALL;



   PROCEDURE USER_PAGEDGETALL (CUR_OUT                OUT T_CURSOR,
                               P_START_ROW_INDEX   IN     NUMBER,
                               P_END_ROW_INDEX     IN     NUMBER,
                               P_TOTAL_ROWS           OUT NUMBER)
   IS
   BEGIN
      SELECT COUNT (*) INTO P_TOTAL_ROWS FROM TBL_USER;



      OPEN CUR_OUT FOR
           SELECT *
             FROM (  SELECT ROW_NUMBER () OVER (ORDER BY U.USER_ID) AS ROW_NUM,
                            U.USER_ID,
                            U.LOGIN_NAME,
                            U.DESCRIPTION,
                            U.FIRST_NAME,
                            U.LAST_NAME,
                            U."PASSWORD",
                            U.ADDRESS,
                            U.ROLE_ID,
                            R.ROLE_NAME,
                            U.ACC_EXPIRY_DATE,
                            U.MODIFIER_ID,
                            U.CREATION_DATE,
                            U.FINGER_PRINT_1,
                            U.MODIFICATION_DATE
                       FROM    TBL_USER U
                            LEFT OUTER JOIN
                               TBL_ROLE R
                            ON R.ROLE_ID = U.ROLE_ID
                   ORDER BY U.USER_ID)
            WHERE ROW_NUM BETWEEN P_START_ROW_INDEX AND P_END_ROW_INDEX
         ORDER BY USER_ID;
   END USER_PAGEDGETALL;



   PROCEDURE USER_GETALL_PAGING (CUR_OUT             OUT T_CURSOR,
                                 P_PAGE_INDEX     IN     NUMBER,
                                 P_PAGE_SIZE      IN     NUMBER,
                                 P_SEARCH_TEXT    IN     NVARCHAR2,
                                 P_RECORD_COUNT      OUT NUMBER)
   IS
   BEGIN
      WITH CTE_USER
           AS (  SELECT ROW_NUMBER () OVER (ORDER BY U.USER_ID) AS ROWNUMBER,
                        U.USER_ID,
                        U.LOGIN_NAME,
                        U.FIRST_NAME,
                        U.LAST_NAME,
                        R.ROLE_NAME,
                        R.ROLE_ID
                   FROM    TBL_USER U
                        INNER JOIN
                           TBL_ROLE R
                        ON U.ROLE_ID = R.ROLE_ID
                  WHERE     1 = 1
                        AND    NVL (LOWER (FIRST_NAME), '''')
                            || NVL (LOWER (LAST_NAME), '''')
                            || NVL (LOWER (ROLE_NAME), '''') LIKE
                               '%' || LOWER (P_SEARCH_TEXT) || '%'
               ORDER BY U.USER_ID)
      SELECT COUNT (*)
        INTO P_RECORD_COUNT
        FROM (  SELECT *
                  FROM CTE_USER
                 WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                     AND   (  (    (P_PAGE_INDEX - 1)
                                                 * P_PAGE_SIZE
                                               + 1)
                                            + P_PAGE_SIZE)
                                         - 1
              ORDER BY ROWNUMBER ASC);



      OPEN CUR_OUT FOR
           SELECT ROW_NUMBER () OVER (ORDER BY U.USER_ID) AS ROWNUMBER,
                  U.USER_ID,
                  U.LOGIN_NAME,
                  U.FIRST_NAME,
                  U.LAST_NAME,
                  R.ROLE_NAME,
                  R.ROLE_ID
             FROM TBL_USER U INNER JOIN TBL_ROLE R ON U.ROLE_ID = R.ROLE_ID
            WHERE     1 = 1
                  AND    NVL (LOWER (FIRST_NAME), '''')
                      || NVL (LOWER (LAST_NAME), '''')
                      || NVL (LOWER (ROLE_NAME), '''') LIKE
                         '%' || LOWER (P_SEARCH_TEXT) || '%'
         ORDER BY U.USER_ID;
   END USER_GETALL_PAGING;



   /*ROLE*/



   PROCEDURE ROLE_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_ROLE
         ORDER BY ROLE_ID;
   END ROLE_GETALL;



   PROCEDURE ROLE_INSERT (P_ROLE_NAME           IN     NVARCHAR2,
                          P_DESCRIPTION         IN     NVARCHAR2,
                          P_MODIFIER_ID         IN     NUMBER,
                          P_CREATION_DATE       IN     DATE,
                          P_MODIFICATION_DATE   IN     DATE,
                          P_IS_ACTIVE           IN     NUMBER,
                          P_RETURNMSG              OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_ROLE
       WHERE ROLE_NAME = P_ROLE_NAME;



      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'ROLE FOUND';
      ELSE
         INSERT INTO TBL_ROLE (ROLE_ID,
                               ROLE_NAME,
                               DESCRIPTION,
                               MODIFIER_ID,
                               CREATION_DATE,
                               MODIFICATION_DATE,
                               IS_ACTIVE)
              VALUES (NVL ( (SELECT MAX (ROLE_ID) FROM TBL_ROLE), 0) + 1,
                      P_ROLE_NAME,
                      P_DESCRIPTION,
                      P_MODIFIER_ID,
                      P_CREATION_DATE,
                      P_MODIFICATION_DATE,
                      P_IS_ACTIVE);



         P_RETURNMSG := 'ROLE CREATED';
      END IF;
   END ROLE_INSERT;



   PROCEDURE ROLE_UPDATE (P_ROLE_ID             IN     NUMBER,
                          P_ROLE_NAME           IN     NVARCHAR2,
                          P_DESCRIPTION         IN     NVARCHAR2,
                          P_MODIFIER_ID         IN     NUMBER,
                          P_CREATION_DATE       IN     DATE,
                          P_MODIFICATION_DATE   IN     DATE,
                          P_IS_ACTIVE           IN     NUMBER,
                          P_RETURNMSG              OUT NVARCHAR2,
                          P_OLD_ROLE_NAME       IN     NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      IF (P_ROLE_NAME = P_OLD_ROLE_NAME)
      THEN
         UPDATE TBL_ROLE
            SET ROLE_NAME = P_ROLE_NAME,
                MODIFIER_ID = P_MODIFIER_ID,
                MODIFICATION_DATE = P_MODIFICATION_DATE,
                IS_ACTIVE = P_IS_ACTIVE
          WHERE ROLE_ID = P_ROLE_ID;



         P_RETURNMSG := 'ROLE CREATED';
      ELSE
         SELECT COUNT (*)
           INTO C_COUNT
           FROM TBL_ROLE
          WHERE ROLE_NAME = P_ROLE_NAME;



         IF (C_COUNT > 0)
         THEN
            P_RETURNMSG := 'ROLE FOUND';
         ELSE
            UPDATE TBL_ROLE
               SET ROLE_NAME = P_ROLE_NAME,
                   DESCRIPTION = P_DESCRIPTION,
                   MODIFIER_ID = P_MODIFIER_ID,
                   CREATION_DATE = P_CREATION_DATE,
                   MODIFICATION_DATE = P_MODIFICATION_DATE,
                   IS_ACTIVE = P_IS_ACTIVE
             WHERE ROLE_ID = P_ROLE_ID;



            P_RETURNMSG := 'ROLE CREATED';
         END IF;
      END IF;
   END ROLE_UPDATE;



   PROCEDURE ROLE_DELETE (P_ROLE_ID IN NUMBER)
   AS
   BEGIN
      DELETE TBL_ROLE
       WHERE ROLE_ID = P_ROLE_ID;
   END ROLE_DELETE;



   PROCEDURE ROLE_GETBYNAME (P_ROLE_NAME IN NVARCHAR2, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT R.ROLE_ID,
                  R.ROLE_NAME,
                  R.DESCRIPTION,
                  R.MODIFIER_ID,
                  R.CREATION_DATE,
                  R.MODIFICATION_DATE
             FROM TBL_ROLE R
            WHERE LOWER (ROLE_NAME) = LOWER (P_ROLE_NAME)
         ORDER BY R.ROLE_ID;
   END ROLE_GETBYNAME;



   PROCEDURE ROLE_GETBYID (P_ROLE_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_ROLE
            WHERE ROLE_ID = P_ROLE_ID
         ORDER BY ROLE_ID;
   END ROLE_GETBYID;



   PROCEDURE ROLE_PAGEDGETALL (CUR_OUT                OUT T_CURSOR,
                               P_START_ROW_INDEX   IN     NUMBER,
                               P_END_ROW_INDEX     IN     NUMBER,
                               P_TOTAL_ROWS           OUT NUMBER)
   IS
   BEGIN
      SELECT COUNT (*) INTO P_TOTAL_ROWS FROM TBL_ROLE;



      OPEN CUR_OUT FOR
           SELECT *
             FROM (  SELECT ROW_NUMBER () OVER (ORDER BY R.ROLE_ID) AS ROW_NUM,
                            R.ROLE_ID,
                            R.ROLE_NAME,
                            R.DESCRIPTION,
                            R.MODIFIER_ID,
                            R.CREATION_DATE,
                            R.MODIFICATION_DATE
                       FROM TBL_ROLE R
                   ORDER BY R.ROLE_ID)
            WHERE ROW_NUM BETWEEN P_START_ROW_INDEX AND P_END_ROW_INDEX
         ORDER BY ROLE_ID;
   END ROLE_PAGEDGETALL;



   /*ROLE MODULE RIGHT*/



   PROCEDURE ROLEMODRIGHT_INSERT (P_ROLE_ID         IN NUMBER,
                                  P_ENTRY_ID        IN NUMBER,
                                  P_MODULE_ID       IN NUMBER,
                                  P_MODULE_ADD      IN NUMBER,
                                  P_MODULE_VIEW     IN NUMBER,
                                  P_MODULE_EDIT     IN NUMBER,
                                  P_MODULE_DELETE   IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_ROLE_MODULE_RIGHT (ROLE_ID,
                                         ENTRY_ID,
                                         MODULE_ID,
                                         MODULE_VIEW,
                                         MODULE_ADD,
                                         MODULE_EDIT,
                                         MODULE_DELETE)
           VALUES (
                     P_ROLE_ID,
                       NVL (
                          (SELECT MAX (ENTRY_ID) FROM TBL_ROLE_MODULE_RIGHT),
                          0)
                     + 1,
                     P_MODULE_ID,
                     P_MODULE_VIEW,
                     P_MODULE_ADD,
                     P_MODULE_EDIT,
                     P_MODULE_DELETE);
   END ROLEMODRIGHT_INSERT;



   PROCEDURE ROLEMODRIGHT_UPDATE (P_ROLE_ID         IN NUMBER,
                                  P_ENTRY_ID        IN NUMBER,
                                  P_MODULE_ID       IN NUMBER,
                                  P_MODULE_ADD      IN NUMBER,
                                  P_MODULE_VIEW     IN NUMBER,
                                  P_MODULE_EDIT     IN NUMBER,
                                  P_MODULE_DELETE   IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_ROLE_MODULE_RIGHT
         SET MODULE_ADD = P_MODULE_ADD,
             MODULE_VIEW = P_MODULE_VIEW,
             MODULE_DELETE = P_MODULE_DELETE,
             MODULE_EDIT = P_MODULE_EDIT
       WHERE ENTRY_ID = P_ENTRY_ID;
   END ROLEMODRIGHT_UPDATE;



   PROCEDURE ROLEMODRIGHT_DELETE (P_ROLE_ID NUMBER, ENTRY_ID IN NUMBER)
   AS
   BEGIN
      DELETE TBL_ROLE_MODULE_RIGHT
       WHERE ENTRY_ID = ENTRY_ID AND ROLE_ID = P_ROLE_ID;
   END ROLEMODRIGHT_DELETE;



   PROCEDURE ROLEMODRIGHT_DELETEBYROLEID (P_ROLE_ID NUMBER)
   AS
   BEGIN
      DELETE TBL_ROLE_MODULE_RIGHT
       WHERE ROLE_ID = P_ROLE_ID;
   END ROLEMODRIGHT_DELETEBYROLEID;



   PROCEDURE ROLEMODRIGHT_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR SELECT * FROM TBL_ROLE_MODULE_RIGHT;
   END ROLEMODRIGHT_GETALL;



   /*USER MODULE RIGHT*/



   PROCEDURE USERMODRIGHT_INSERT (P_USER_ID IN NUMBER, P_ENTRY_ID IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_USER_MODULE_RIGHT (USER_ID, ENTRY_ID)
           VALUES (P_USER_ID, P_ENTRY_ID);
   END USERMODRIGHT_INSERT;



   PROCEDURE USERMODRIGHT_DELETE (P_USER_ID IN NUMBER, P_ENTRY_ID IN NUMBER)
   AS
   BEGIN
      DELETE TBL_USER_MODULE_RIGHT
       WHERE ENTRY_ID = P_ENTRY_ID AND USER_ID = P_USER_ID;
   END USERMODRIGHT_DELETE;



   PROCEDURE USERMODRIGHT_DELETEBYUSERID (P_USER_ID IN NUMBER)
   AS
   BEGIN
      DELETE TBL_USER_MODULE_RIGHT
       WHERE USER_ID = P_USER_ID;
   END USERMODRIGHT_DELETEBYUSERID;



   PROCEDURE USER_MODULE_RIGHT_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_USER_MODULE_RIGHT
         ORDER BY USER_ID;
   END USER_MODULE_RIGHT_GETALL;



   PROCEDURE USERMODULE_ACT_UPDATE (P_ENTRY_ID        IN NUMBER,
                                    P_USER_ID         IN NUMBER,
                                    P_MODULE_ID       IN NUMBER,
                                    P_MODULE_VIEW     IN NUMBER,
                                    P_MODULE_ADD      IN NUMBER,
                                    P_MODULE_EDIT     IN NUMBER,
                                    P_MODULE_DELETE   IN NUMBER)
   AS
   BEGIN
      IF (P_ENTRY_ID = 0)
      THEN
         INSERT INTO TBL_USER_MODULE_RIGHT (USER_ID,
                                            ENTRY_ID,
                                            MODULE_ID,
                                            MODULE_VIEW,
                                            MODULE_ADD,
                                            MODULE_EDIT,
                                            MODULE_DELETE)
              VALUES (
                        P_USER_ID,
                          NVL (
                             (SELECT MAX (ENTRY_ID)
                                FROM TBL_USER_MODULE_RIGHT),
                             0)
                        + 1,
                        P_MODULE_ID,
                        P_MODULE_VIEW,
                        P_MODULE_ADD,
                        P_MODULE_EDIT,
                        P_MODULE_DELETE);
      ELSE
         UPDATE TBL_USER_MODULE_RIGHT
            SET MODULE_VIEW = P_MODULE_VIEW,
                MODULE_ADD = P_MODULE_ADD,
                MODULE_EDIT = P_MODULE_EDIT,
                MODULE_DELETE = P_MODULE_DELETE
          WHERE ENTRY_ID = P_ENTRY_ID;
      END IF;
   END USERMODULE_ACT_UPDATE;



   PROCEDURE USERSUBMOD_ACT_UPDATE (P_ENTRY_ID            IN NUMBER,
                                    P_USER_ID             IN NUMBER,
                                    P_SUBMODULE_ID        IN NUMBER,
                                    P_SUB_MODULE_VIEW     IN NUMBER,
                                    P_SUB_MODULE_ADD      IN NUMBER,
                                    P_SUB_MODULE_EDIT     IN NUMBER,
                                    P_SUB_MODULE_DELETE   IN NUMBER)
   AS
   BEGIN
      IF (P_ENTRY_ID = 0)
      THEN
         INSERT INTO TBL_USER_SUBMODULE_RIGHT (ENTRY_ID,
                                               USER_ID,
                                               SUBMODULE_ID,
                                               SUB_MODULE_VIEW,
                                               SUB_MODULE_ADD,
                                               SUB_MODULE_EDIT,
                                               SUB_MODULE_DELETE)
              VALUES (
                          NVL (
                             (SELECT MAX (ENTRY_ID)
                                FROM TBL_USER_SUBMODULE_RIGHT),
                             0)
                        + 1,
                        P_USER_ID,
                        P_SUBMODULE_ID,
                        P_SUB_MODULE_VIEW,
                        P_SUB_MODULE_ADD,
                        P_SUB_MODULE_EDIT,
                        P_SUB_MODULE_DELETE);
      ELSE
         UPDATE TBL_USER_SUBMODULE_RIGHT
            SET SUB_MODULE_VIEW = P_SUB_MODULE_VIEW,
                SUB_MODULE_ADD = P_SUB_MODULE_ADD,
                SUB_MODULE_EDIT = P_SUB_MODULE_EDIT,
                SUB_MODULE_DELETE = P_SUB_MODULE_DELETE
          WHERE ENTRY_ID = P_ENTRY_ID;
      END IF;
   END USERSUBMOD_ACT_UPDATE;



   /*MODULE ACTIVITY*/



   PROCEDURE MODULE_ACTIVITY_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_MODULE_ACTIVITY
         ORDER BY MODULE_ID;
   END MODULE_ACTIVITY_GETALL;



   PROCEDURE MODULE_ACTIVITY_INSERT (P_MODULE_ID     IN NUMBER,
                                     P_ACTIVITY_ID   IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_MODULE_ACTIVITY (MODULE_ID, ACTIVITY_ID)
           VALUES (P_MODULE_ID, P_ACTIVITY_ID);
   END MODULE_ACTIVITY_INSERT;



   PROCEDURE MODULE_ACTIVITY_DELETE (P_ENTRY_ID    IN NUMBER,
                                     P_MODULE_ID   IN NUMBER)
   AS
   BEGIN
      DELETE TBL_MODULE_ACTIVITY
       WHERE ENTRY_ID = P_ENTRY_ID AND MODULE_ID = P_MODULE_ID;
   END MODULE_ACTIVITY_DELETE;



   PROCEDURE USERMOD_ACT_GETBYUSERID (P_USER_ID   IN     NUMBER,
                                      CUR_OUT        OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT MODULE_NAME,
                NVL (U.USER_ID, P_USER_ID) USER_ID,
                NVL (U.ENTRY_ID, 0) ENTRY_ID,
                NVL (U.MODULE_ID, M.MODULE_ID) MODULE_ID,
                NVL (U.MODULE_VIEW, 0) MODULE_VIEW,
                NVL (U.MODULE_ADD, 0) MODULE_ADD,
                NVL (U.MODULE_EDIT, 0) MODULE_EDIT,
                NVL (U.MODULE_DELETE, 0) MODULE_DELETE
           FROM    TBL_MODULE M
                LEFT OUTER JOIN
                   TBL_USER_MODULE_RIGHT U
                ON M.MODULE_ID = U.MODULE_ID AND U.USER_ID = P_USER_ID;
   END USERMOD_ACT_GETBYUSERID;



   PROCEDURE USERMOD_ACT_GETBYENTRYID (P_ENTRY_ID   IN     NUMBER,
                                       CUR_OUT         OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT MODULE_NAME,
                USER_ID,
                NVL (U.ENTRY_ID, 0) ENTRY_ID,
                NVL (U.MODULE_ID, M.MODULE_ID) MODULE_ID,
                NVL (U.MODULE_VIEW, 0) MODULE_VIEW,
                NVL (U.MODULE_ADD, 0) MODULE_ADD,
                NVL (U.MODULE_EDIT, 0) MODULE_EDIT,
                NVL (U.MODULE_DELETE, 0) MODULE_DELETE
           FROM    TBL_USER_MODULE_RIGHT U
                INNER JOIN
                   TBL_MODULE M
                ON M.MODULE_ID = U.MODULE_ID
          WHERE U.ENTRY_ID = P_ENTRY_ID;
   END USERMOD_ACT_GETBYENTRYID;



   /*USER SUBMODULE RIGHT*/



   PROCEDURE USERSUBMODRIGHT_INSERT (P_USER_ID    IN NUMBER,
                                     P_ENTRY_ID   IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_USER_SUBMODULE_RIGHT (USER_ID, ENTRY_ID)
           VALUES (P_USER_ID, P_ENTRY_ID);
   END USERSUBMODRIGHT_INSERT;



   PROCEDURE USERSUBMODRIGHT_DELETE (P_USER_ID    IN NUMBER,
                                     P_ENTRY_ID   IN NUMBER)
   AS
   BEGIN
      DELETE TBL_USER_SUBMODULE_RIGHT
       WHERE ENTRY_ID = P_ENTRY_ID AND USER_ID = P_USER_ID;
   END USERSUBMODRIGHT_DELETE;



   PROCEDURE USERSUBMODRIGHT_DELETEBYUSERID (P_USER_ID NUMBER)
   AS
   BEGIN
      DELETE TBL_USER_SUBMODULE_RIGHT
       WHERE USER_ID = P_USER_ID;
   END USERSUBMODRIGHT_DELETEBYUSERID;



   PROCEDURE USER_SUBMODULE_RIGHT_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_USER_SUBMODULE_RIGHT
         ORDER BY USER_ID;
   END USER_SUBMODULE_RIGHT_GETALL;



   PROCEDURE USERSUBRIGHT_BYUSERIDSUBMODID (P_USER_ID        IN     NUMBER,
                                            P_SUBMODULE_ID   IN     NUMBER,
                                            CUR_OUT             OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT U.ENTRY_ID,
                U.USER_ID,
                M.MODULE_ID,
                U.SUBMODULE_ID,
                MODULE_NAME,
                SUBMODULE_NAME,
                SUB_MODULE_VIEW,
                SUB_MODULE_ADD,
                SUB_MODULE_EDIT,
                SUB_MODULE_DELETE
           FROM TBL_USER_SUBMODULE_RIGHT U
                INNER JOIN TBL_SUBMODULE SM
                   ON U.SUBMODULE_ID = SM.SUBMODULE_ID
                INNER JOIN TBL_MODULE M
                   ON SM.MODULE_ID = M.MODULE_ID
          WHERE U.USER_ID = P_USER_ID AND U.SUBMODULE_ID = P_SUBMODULE_ID;
   END USERSUBRIGHT_BYUSERIDSUBMODID;



   PROCEDURE USERSUBMOD_ACT_BYUSERID (P_USER_ID     IN     NUMBER,
                                      P_MODULE_ID   IN     NUMBER,
                                      CUR_OUT          OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT M.MODULE_NAME,
                SUBMODULE_NAME,
                NVL (USER_ID, P_USER_ID) USER_ID,
                NVL (U.ENTRY_ID, 0) ENTRY_ID,
                NVL (SM.MODULE_ID, P_MODULE_ID) MODULE_ID,
                NVL (SM.SUBMODULE_ID, M.MODULE_ID) SUBMODULE_ID,
                NVL (U.SUB_MODULE_VIEW, 0) SUB_MODULE_VIEW,
                NVL (U.SUB_MODULE_ADD, 0) SUB_MODULE_ADD,
                NVL (U.SUB_MODULE_EDIT, 0) SUB_MODULE_EDIT,
                NVL (U.SUB_MODULE_DELETE, 0) SUB_MODULE_DELETE
           FROM TBL_SUBMODULE SM
                LEFT OUTER JOIN TBL_MODULE M
                   ON SM.MODULE_ID = M.MODULE_ID
                LEFT OUTER JOIN TBL_USER_SUBMODULE_RIGHT U
                   ON     SM.SUBMODULE_ID = U.SUBMODULE_ID
                      AND U.USER_ID = P_USER_ID
          WHERE SM.MODULE_ID = P_MODULE_ID;
   END USERSUBMOD_ACT_BYUSERID;



   PROCEDURE USERSUBMOD_ACT_GETBYENTRYID (P_ENTRY_ID   IN     NUMBER,
                                          CUR_OUT         OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT ENTRY_ID,
                USER_ID,
                M.MODULE_ID,
                U.SUBMODULE_ID,
                MODULE_NAME,
                SUBMODULE_NAME,
                SUB_MODULE_VIEW,
                SUB_MODULE_ADD,
                SUB_MODULE_EDIT,
                SUB_MODULE_DELETE
           FROM TBL_USER_SUBMODULE_RIGHT U
                INNER JOIN TBL_SUBMODULE SM
                   ON U.SUBMODULE_ID = SM.SUBMODULE_ID
                INNER JOIN TBL_MODULE M
                   ON SM.MODULE_ID = M.MODULE_ID
          WHERE ENTRY_ID = P_ENTRY_ID;
   END USERSUBMOD_ACT_GETBYENTRYID;



   /*SUBMODULE ACTIVITY*/



   PROCEDURE SUBMODULE_ACTIVITY_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_SUBMODULE_ACTIVITY
         ORDER BY SUBMODULE_ID;
   END SUBMODULE_ACTIVITY_GETALL;



   /*SUBMODULE*/



   PROCEDURE SUBMODULE_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_SUBMODULE
         ORDER BY SUBMODULE_ID;
   END SUBMODULE_GETALL;



   PROCEDURE SUBMODULE_GETBYUSER (P_USER_ID     IN     NUMBER,
                                  P_MODULE_ID   IN     NUMBER,
                                  CUR_OUT          OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT SM.SUBMODULE_ID,
                  SM.MODULE_ID,
                  SM.SUBMODULE_NAME,
                  SM.SUBMODULE_URL,
                  SM.IS_GUI_VISIBLE,
                  SMA.SUB_MODULE_ADD,
                  SMA.SUB_MODULE_EDIT,
                  SMA.SUB_MODULE_VIEW
             FROM    TBL_SUBMODULE SM
                  LEFT OUTER JOIN
                     TBL_USER_SUBMODULE_RIGHT SMA
                  ON SMA.SUBMODULE_ID = SM.SUBMODULE_ID
            WHERE     SM.IS_GUI_VISIBLE = 1
                  AND SM.MODULE_ID = P_MODULE_ID
                  AND SMA.USER_ID = P_USER_ID
                  AND SMA.SUB_MODULE_VIEW = 1
         ORDER BY SUBMODULE_ID;
   END SUBMODULE_GETBYUSER;



   PROCEDURE SUBMODULE_GETBYMODULEID (P_MODULE_ID   IN     NUMBER,
                                      CUR_OUT          OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_SUBMODULE SM
            WHERE SM.IS_GUI_VISIBLE = 1 AND SM.MODULE_ID = P_MODULE_ID
         ORDER BY SUBMODULE_ID;
   END SUBMODULE_GETBYMODULEID;



   PROCEDURE SUBMODULE_GETBYID (P_SUBMODULE_ID   IN     NUMBER,
                                CUR_OUT             OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_SUBMODULE
            WHERE SUBMODULE_ID = P_SUBMODULE_ID
         ORDER BY SUBMODULE_ID;
   END SUBMODULE_GETBYID;



   PROCEDURE SUBMODULE_INSERT (P_SUBMODULE_ID     IN NUMBER,
                               P_MODULE_ID        IN NUMBER,
                               P_SUBMODULE_NAME   IN NVARCHAR2,
                               P_IS_GUI_VISIBLE   IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_SUBMODULE (SUBMODULE_ID,
                                 MODULE_ID,
                                 SUBMODULE_NAME,
                                 IS_GUI_VISIBLE)
           VALUES (P_SUBMODULE_ID,
                   P_MODULE_ID,
                   P_SUBMODULE_NAME,
                   P_IS_GUI_VISIBLE);
   END SUBMODULE_INSERT;



   PROCEDURE SUBMODULE_UPDATE (P_SUBMODULE_ID     IN NUMBER,
                               P_MODULE_ID        IN NUMBER,
                               P_SUBMODULE_NAME   IN NVARCHAR2,
                               P_IS_GUI_VISIBLE   IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_SUBMODULE
         SET MODULE_ID = P_MODULE_ID,
             SUBMODULE_NAME = P_SUBMODULE_NAME,
             IS_GUI_VISIBLE = P_IS_GUI_VISIBLE
       WHERE SUBMODULE_ID = P_SUBMODULE_ID;
   END SUBMODULE_UPDATE;



   PROCEDURE SUBMODULE_DELETE (P_SUBMODULE_ID NUMBER)
   AS
   BEGIN
      DELETE TBL_SUBMODULE
       WHERE SUBMODULE_ID = P_SUBMODULE_ID;
   END SUBMODULE_DELETE;



   /*ROLE SUBMODULE RIGHT*/



   PROCEDURE ROLESUBMODRIGHT_INSERT (P_ROLE_ID             IN NUMBER,
                                     P_ENTRY_ID            IN NUMBER,
                                     P_SUBMODULE_ID        IN NUMBER,
                                     P_SUB_MODULE_VIEW     IN NUMBER,
                                     P_SUB_MODULE_ADD      IN NUMBER,
                                     P_SUB_MODULE_EDIT     IN NUMBER,
                                     P_SUB_MODULE_DELETE   IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_ROLE_SUBMODULE_RIGHT (ROLE_ID,
                                            ENTRY_ID,
                                            SUBMODULE_ID,
                                            SUB_MODULE_VIEW,
                                            SUB_MODULE_ADD,
                                            SUB_MODULE_EDIT,
                                            SUB_MODULE_DELETE)
           VALUES (
                     P_ROLE_ID,
                       NVL (
                          (SELECT MAX (ENTRY_ID)
                             FROM TBL_ROLE_SUBMODULE_RIGHT),
                          0)
                     + 1,
                     P_SUBMODULE_ID,
                     P_SUB_MODULE_VIEW,
                     P_SUB_MODULE_ADD,
                     P_SUB_MODULE_EDIT,
                     P_SUB_MODULE_DELETE);
   END ROLESUBMODRIGHT_INSERT;



   PROCEDURE ROLESUBMODRIGHT_UPDATE (P_ROLE_ID             IN NUMBER,
                                     P_ENTRY_ID            IN NUMBER,
                                     P_SUBMODULE_ID        IN NUMBER,
                                     P_SUB_MODULE_VIEW     IN NUMBER,
                                     P_SUB_MODULE_ADD      IN NUMBER,
                                     P_SUB_MODULE_EDIT     IN NUMBER,
                                     P_SUB_MODULE_DELETE   IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_ROLE_SUBMODULE_RIGHT
         SET SUB_MODULE_ADD = P_SUB_MODULE_ADD,
             SUB_MODULE_VIEW = P_SUB_MODULE_VIEW,
             SUB_MODULE_DELETE = P_SUB_MODULE_DELETE,
             SUB_MODULE_EDIT = P_SUB_MODULE_EDIT
       WHERE ENTRY_ID = P_ENTRY_ID;
   END ROLESUBMODRIGHT_UPDATE;



   PROCEDURE ROLESUBMODRIGHT_DELETE (P_ROLE_ID IN NUMBER, P_ENTRY_ID NUMBER)
   AS
   BEGIN
      DELETE TBL_ROLE_SUBMODULE_RIGHT
       WHERE ENTRY_ID = P_ENTRY_ID AND ROLE_ID = P_ROLE_ID;
   END ROLESUBMODRIGHT_DELETE;



   PROCEDURE ROLESUBMODRIGHT_DELETEBYROLEID (P_ROLE_ID NUMBER)
   AS
   BEGIN
      DELETE TBL_ROLE_SUBMODULE_RIGHT
       WHERE ROLE_ID = P_ROLE_ID;
   END ROLESUBMODRIGHT_DELETEBYROLEID;



   PROCEDURE ROLESUBMODRIGHT_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR SELECT * FROM TBL_ROLE_SUBMODULE_RIGHT;
   END ROLESUBMODRIGHT_GETALL;



   PROCEDURE ROLESUBMODACT_GETBYROLEID (P_ROLE_ID     IN     NUMBER,
                                        P_MODULE_ID   IN     NUMBER,
                                        CUR_OUT          OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT RS.ENTRY_ID,
                NVL (RS.ROLE_ID, P_ROLE_ID) ROLE_ID,
                M.MODULE_ID,
                M.MODULE_NAME,
                S.SUBMODULE_ID,
                S.SUBMODULE_NAME,
                NVL (SUB_MODULE_VIEW, 0) SUB_MODULE_VIEW,
                NVL (SUB_MODULE_ADD, 0) SUB_MODULE_ADD,
                NVL (SUB_MODULE_EDIT, 0) SUB_MODULE_EDIT,
                NVL (SUB_MODULE_DELETE, 0) SUB_MODULE_DELETE
           FROM TBL_SUBMODULE S
                LEFT OUTER JOIN TBL_ROLE_SUBMODULE_RIGHT RS
                   ON     S.SUBMODULE_ID = RS.SUBMODULE_ID
                      AND RS.ROLE_ID = P_ROLE_ID
                LEFT OUTER JOIN TBL_MODULE M
                   ON S.MODULE_ID = M.MODULE_ID
          WHERE M.IS_GUI_VISIBLE = 1 AND M.MODULE_ID = P_MODULE_ID;
   END ROLESUBMODACT_GETBYROLEID;



   /*SUBMODULE ACTIVITY*/



   PROCEDURE SUBMODULEACTIVITY_INSERT (P_ENTRY_ID       IN NUMBER,
                                       P_SUBMODULE_ID   IN NUMBER,
                                       P_ACTIVITY_ID    IN NUMBER)
   AS
   BEGIN
      INSERT
        INTO TBL_SUBMODULE_ACTIVITY (ENTRY_ID, SUBMODULE_ID, ACTIVITY_ID)
      VALUES (P_ENTRY_ID, P_SUBMODULE_ID, P_ACTIVITY_ID);
   END SUBMODULEACTIVITY_INSERT;



   PROCEDURE SUBMODULEACTIVITY_DELETE (P_ENTRY_ID IN NUMBER)
   AS
   BEGIN
      DELETE TBL_SUBMODULE_ACTIVITY
       WHERE ENTRY_ID = P_ENTRY_ID;
   END SUBMODULEACTIVITY_DELETE;



   PROCEDURE SUBMODULEACTIVITY_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR SELECT * FROM TBL_SUBMODULE_ACTIVITY;
   END SUBMODULEACTIVITY_GETALL;



   /*MODULE*/



   PROCEDURE MODULE_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_MODULE
            WHERE IS_GUI_VISIBLE = 1
         ORDER BY MODULE_ID;
   END MODULE_GETALL;



   PROCEDURE MODULE_GETBYID (P_MODULE_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_MODULE
            WHERE MODULE_ID = P_MODULE_ID
         ORDER BY MODULE_ID;
   END MODULE_GETBYID;



   PROCEDURE MODULE_GETBYUSER (P_USER_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT M.MODULE_ID,
                  M.MODULE_NAME,
                  M.IS_GUI_VISIBLE,
                  M.MODULE_URL
             FROM    TBL_MODULE M
                  LEFT OUTER JOIN
                     TBL_USER_MODULE_RIGHT MA
                  ON MA.MODULE_ID = M.MODULE_ID
            WHERE     M.IS_GUI_VISIBLE = 1
                  AND MA.USER_ID = P_USER_ID
                  AND MA.MODULE_VIEW = 1
         ORDER BY MODULE_ID;
   END MODULE_GETBYUSER;



   PROCEDURE MODULE_INSERT (P_MODULE_ID        IN NUMBER,
                            P_MODULE_NAME      IN NVARCHAR2,
                            P_IS_GUI_VISIBLE   IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_MODULE (MODULE_ID, MODULE_NAME, IS_GUI_VISIBLE)
           VALUES (P_MODULE_ID, P_MODULE_NAME, P_IS_GUI_VISIBLE);
   END MODULE_INSERT;



   PROCEDURE MODULE_UPDATE (P_MODULE_ID        IN NUMBER,
                            P_MODULE_NAME      IN NVARCHAR2,
                            P_IS_GUI_VISIBLE   IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_MODULE
         SET MODULE_NAME = P_MODULE_NAME, IS_GUI_VISIBLE = P_IS_GUI_VISIBLE
       WHERE MODULE_ID = P_MODULE_ID;
   END MODULE_UPDATE;



   PROCEDURE MODULE_DELETE (P_MODULE_ID NUMBER)
   AS
   BEGIN
      DELETE TBL_MODULE
       WHERE MODULE_ID = P_MODULE_ID;
   END MODULE_DELETE;



   PROCEDURE ROLE_GETALL_PAGING (CUR_OUT             OUT T_CURSOR,
                                 P_PAGE_INDEX     IN     NUMBER,
                                 P_PAGE_SIZE      IN     NUMBER,
                                 P_SEARCH_TEXT    IN     NVARCHAR2,
                                 P_RECORD_COUNT      OUT NUMBER)
   IS
   BEGIN
      WITH CTE_ROLE
           AS (  SELECT ROW_NUMBER () OVER (ORDER BY R.ROLE_ID) AS ROWNUMBER,
                        R.ROLE_ID,
                        ROLE_NAME,
                        IS_ACTIVE,
                        U.FIRST_NAME || ' ' || U.LAST_NAME USER_NAME
                   FROM    TBL_ROLE R
                        INNER JOIN
                           TBL_USER U
                        ON R.MODIFIER_ID = U.USER_ID
                  WHERE     1 = 1
                        AND NVL (LOWER (ROLE_NAME), '''') LIKE
                               '%' || LOWER (P_SEARCH_TEXT) || '%'
               ORDER BY R.ROLE_ID)
      SELECT COUNT (*)
        INTO P_RECORD_COUNT
        FROM (  SELECT *
                  FROM CTE_ROLE
                 WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                     AND   (  (    (P_PAGE_INDEX - 1)
                                                 * P_PAGE_SIZE
                                               + 1)
                                            + P_PAGE_SIZE)
                                         - 1
              ORDER BY ROWNUMBER ASC);



      OPEN CUR_OUT FOR
           SELECT ROW_NUMBER () OVER (ORDER BY R.ROLE_ID) AS ROWNUMBER,
                  R.ROLE_ID,
                  ROLE_NAME,
                  IS_ACTIVE,
                  U.FIRST_NAME || ' ' || U.LAST_NAME USER_NAME
             FROM TBL_ROLE R INNER JOIN TBL_USER U ON R.MODIFIER_ID = U.USER_ID
            WHERE     1 = 1
                  AND NVL (LOWER (ROLE_NAME), '''') LIKE
                         '%' || LOWER (P_SEARCH_TEXT) || '%'
         ORDER BY R.ROLE_ID;
   END ROLE_GETALL_PAGING;



   PROCEDURE MODULEACTIVITY_BYROLEID (CUR_OUT        OUT T_CURSOR,
                                      P_ROLE_ID   IN     NUMBER)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT RM.ENTRY_ID,
                NVL (RM.ROLE_ID, P_ROLE_ID) ROLE_ID,
                M.MODULE_ID,
                M.MODULE_NAME,
                NVL (RM.MODULE_VIEW, 0) MODULE_VIEW,
                NVL (RM.MODULE_ADD, 0) MODULE_ADD,
                NVL (RM.MODULE_EDIT, 0) MODULE_EDIT,
                NVL (RM.MODULE_DELETE, 0) MODULE_DELETE
           FROM    TBL_MODULE M
                LEFT OUTER JOIN
                   TBL_ROLE_MODULE_RIGHT RM
                ON M.MODULE_ID = RM.MODULE_ID AND RM.ROLE_ID = P_ROLE_ID
          WHERE M.IS_GUI_VISIBLE = 1;
   END MODULEACTIVITY_BYROLEID;



   /*CROSSTALK EVENT*/



   PROCEDURE CT_PACKET_GETALL (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CTP.TMS_ID,
                  CTP.PLAZA_ID,
                  CTP.ENTRY_ID,
                  CTP.EVENT_TYPE,
                  CTP.TIME_STAMP,
                  CTP.LOCATION_ID,
                  CTP.OBJECT_ID,
                  CTP.READS,
                  CTP.MODIFIER_ID,
                  CTP.CREATION_DATE,
                  CTP.MODIFICATION_DATE
             FROM TBL_CROSSTALK_PACKET CTP
         ORDER BY CTP.ENTRY_ID;
   END CT_PACKET_GETALL;



   PROCEDURE CT_PACKET_GETRECENT (P_PLAZA_ID          NUMBER,
                                  P_TAG_ID     IN     NVARCHAR2,
                                  P_TAG_TIME   IN     DATE,
                                  CUR_OUT         OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CTP.TMS_ID,
                  CTP.PLAZA_ID,
                  CTP.ENTRY_ID,
                  CTP.LANE_ID,
                  CTP.EVENT_TYPE,
                  CTP.TIME_STAMP,
                  CTP.LOCATION_ID,
                  CTP.OBJECT_ID,
                  CTP.READS,
                  CTP.MODIFIER_ID,
                  CTP.CREATION_DATE,
                  CTP.MODIFICATION_DATE
             FROM TBL_CROSSTALK_PACKET CTP
            WHERE     TO_DATE (SUBSTR (CTP.TIME_STAMP, 0, 19),
                               'YYYY-MM-DD HH24:MI:SS') >
                         (P_TAG_TIME - INTERVAL '60' SECOND)
                  AND CTP.PLAZA_ID = P_PLAZA_ID
                  AND CTP.OBJECT_ID = P_TAG_ID
         ORDER BY CTP.ENTRY_ID;
   END CT_PACKET_GETRECENT;



   PROCEDURE CT_PACKET_INSERT (P_TMS_ID             IN     NUMBER,
                               P_PLAZA_ID           IN     NUMBER,
                               P_ENTRY_ID              OUT NUMBER,
                               P_LANE_ID            IN     NUMBER,
                               P_EVENT_TYPE         IN     NVARCHAR2,
                               P_TIME_STAMP         IN     NVARCHAR2,
                               P_LOCATION_ID        IN     NVARCHAR2,
                               P_OBJECT_ID          IN     NVARCHAR2,
                               P_READS              IN     NVARCHAR2,
                               P_CREATION_DATE      IN     DATE,
                               P_VEHICLE_CLASS_ID   IN     NUMBER,
                               P_PLATE_NUMBER       IN     NVARCHAR2)
   AS
   BEGIN
      INSERT INTO TBL_CROSSTALK_PACKET (TMS_ID,
                                        PLAZA_ID,
                                        ENTRY_ID,
                                        LANE_ID,
                                        EVENT_TYPE,
                                        TIME_STAMP,
                                        LOCATION_ID,
                                        OBJECT_ID,
                                        READS,
                                        CREATION_DATE,
                                        VEHICLE_CLASS_ID,
                                        PLATE_NUMBER)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   CROSSTALK_PACKET_SEQ.NEXTVAL,
                   P_LANE_ID,
                   P_EVENT_TYPE,
                   P_TIME_STAMP,
                   P_LOCATION_ID,
                   P_OBJECT_ID,
                   P_READS,
                   P_CREATION_DATE,
                   P_VEHICLE_CLASS_ID,
                   P_PLATE_NUMBER);



      P_ENTRY_ID := CROSSTALK_PACKET_SEQ.CURRVAL;
   END CT_PACKET_INSERT;



   PROCEDURE CT_PACKET_UPDATE (P_TMS_ID              IN NUMBER,
                               P_PLAZA_ID            IN NUMBER,
                               P_ENTRY_ID            IN NUMBER,
                               P_LANE_ID             IN NUMBER,
                               P_EVENT_TYPE          IN NVARCHAR2,
                               P_TIME_STAMP          IN NVARCHAR2,
                               P_LOCATION_ID         IN NVARCHAR2,
                               P_OBJECT_ID           IN NVARCHAR2,
                               P_READS               IN NVARCHAR2,
                               P_MODIFIER_ID         IN NUMBER,
                               P_MODIFICATION_DATE   IN DATE)
   AS
   BEGIN
      UPDATE TBL_CROSSTALK_PACKET
         SET TMS_ID = P_TMS_ID,
             PLAZA_ID = P_PLAZA_ID,
             ENTRY_ID = P_ENTRY_ID,
             LANE_ID = P_LANE_ID,
             EVENT_TYPE = P_EVENT_TYPE,
             TIME_STAMP = P_TIME_STAMP,
             LOCATION_ID = P_LOCATION_ID,
             OBJECT_ID = P_OBJECT_ID,
             READS = P_READS,
             MODIFIER_ID = P_MODIFIER_ID,
             MODIFICATION_DATE = P_MODIFICATION_DATE
       WHERE     ENTRY_ID = P_ENTRY_ID
             AND TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID;
   END CT_PACKET_UPDATE;



   PROCEDURE CT_PACKET_DELETE (P_TMS_ID     IN NUMBER,
                               P_PLAZA_ID   IN NUMBER,
                               P_ENTRY_ID   IN NUMBER)
   AS
   BEGIN
      DELETE TBL_CROSSTALK_PACKET
       WHERE     ENTRY_ID = P_ENTRY_ID
             AND TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID;
   END CT_PACKET_DELETE;



   /*NODEFLUX EVENT*/



   PROCEDURE NF_PACKET_GETALL (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT NFP.ENTRY_ID,
                  NFP.TMS_ID,
                  NFP.EVENT_TYPE,
                  NFP.TIMESTAMP,
                  NFP.GANTRY_ID,
                  NFP.LANE_ID,
                  NFP.CAMERA_POSITION,
                  NFP.CAMERA_ID,
                  NFP.CAMERA_NAME,
                  NFP.CAMERA_ADDRESS,
                  NFP.CAMERA_COORDINATE,
                  NFP.PLATE_NUMBER,
                  NFP.VEHICLE_CLASS_ID,
                  NFP.VEHICLE_SPEED,
                  NFP.PLATE_THUMBNAIL,
                  NFP.VEHICLE_THUMBNAIL,
                  NFP.VIDEO_URL,
                  NFP.PROVIDER,
                  NFP.CREATION_DATE,
                  NFP.MODIFIER_ID,
                  NFP.MODIFICATION_DATE
             FROM TBL_NODEFLUX_PACKET NFP
         ORDER BY NFP.ENTRY_ID;
   END NF_PACKET_GETALL;



   PROCEDURE NF_PACKET_GETBYENTRYID (P_ENTRY_ID   IN     NUMBER,
                                     CUR_OUT         OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT NFP.ENTRY_ID,
                  NFP.TMS_ID,
                  NFP.EVENT_TYPE,
                  NFP.TIMESTAMP,
                  NFP.GANTRY_ID,
                  NFP.LANE_ID,
                  NFP.CAMERA_POSITION,
                  NFP.CAMERA_ID,
                  NFP.CAMERA_NAME,
                  NFP.CAMERA_ADDRESS,
                  NFP.CAMERA_COORDINATE,
                  NFP.PLATE_NUMBER,
                  NFP.VEHICLE_CLASS_ID,
                  NFP.VEHICLE_SPEED,
                  NFP.PLATE_THUMBNAIL,
                  NFP.VEHICLE_THUMBNAIL,
                  NFP.VIDEO_URL,
                  NFP.PROVIDER,
                  NFP.CREATION_DATE,
                  NFP.MODIFIER_ID,
                  NFP.MODIFICATION_DATE
             FROM TBL_NODEFLUX_PACKET NFP
            WHERE NFP.ENTRY_ID = P_ENTRY_ID
         ORDER BY NFP.ENTRY_ID;
   END NF_PACKET_GETBYENTRYID;



   PROCEDURE NF_PACKET_GETRECENT (P_PLAZA_ID                 NUMBER,
                                  P_VRN               IN     NVARCHAR2,
                                  P_NF_TIME           IN     DATE,
                                  P_CAMERA_POSITION   IN     NUMBER,
                                  CUR_OUT                OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT NFP.ENTRY_ID,
                  NFP.TMS_ID,
                  NFP.EVENT_TYPE,
                  NFP.TIMESTAMP,
                  NFP.GANTRY_ID,
                  NFP.LANE_ID,
                  NFP.CAMERA_POSITION,
                  NFP.CAMERA_ID,
                  NFP.CAMERA_NAME,
                  NFP.CAMERA_ADDRESS,
                  NFP.CAMERA_COORDINATE,
                  NFP.PLATE_NUMBER,
                  NFP.VEHICLE_CLASS_ID,
                  NFP.VEHICLE_SPEED,
                  NFP.PLATE_THUMBNAIL,
                  NFP.VEHICLE_THUMBNAIL,
                  NFP.VIDEO_URL,
                  NFP.PROVIDER,
                  NFP.CREATION_DATE,
                  NFP.MODIFIER_ID,
                  NFP.MODIFICATION_DATE
             FROM TBL_NODEFLUX_PACKET NFP
            WHERE     TO_DATE (SUBSTR (NFP.TIMESTAMP, 0, 19),
                               'YYYY-MM-DD HH24:MI:SS') >
                         (P_NF_TIME - INTERVAL '60' SECOND)
                  AND NFP.GANTRY_ID = P_PLAZA_ID
                  AND NFP.PLATE_NUMBER = P_VRN
                  AND NFP.CAMERA_POSITION = P_CAMERA_POSITION
         ORDER BY NFP.ENTRY_ID;
   END NF_PACKET_GETRECENT;



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
                               P_CREATION_DATE       IN     DATE)
   AS
   BEGIN
      INSERT INTO TBL_NODEFLUX_PACKET (ENTRY_ID,
                                       TMS_ID,
                                       EVENT_TYPE,
                                       TIMESTAMP,
                                       GANTRY_ID,
                                       LANE_ID,
                                       CAMERA_POSITION,
                                       CAMERA_ID,
                                       CAMERA_NAME,
                                       CAMERA_ADDRESS,
                                       CAMERA_COORDINATE,
                                       PLATE_NUMBER,
                                       VEHICLE_CLASS_ID,
                                       VEHICLE_SPEED,
                                       PLATE_THUMBNAIL,
                                       VEHICLE_THUMBNAIL,
                                       VIDEO_URL,
                                       PROVIDER,
                                       CREATION_DATE)
           VALUES (NODEFLUX_PACKET_SEQ.NEXTVAL,
                   P_TMS_ID,
                   P_EVENT_TYPE,
                   P_TIMESTAMP,
                   P_GANTRY_ID,
                   P_LANE_ID,
                   P_CAMERA_POSITION,
                   P_CAMERA_ID,
                   P_CAMERA_NAME,
                   P_CAMERA_ADDRESS,
                   P_CAMERA_COORDINATE,
                   P_PLATE_NUMBER,
                   P_VEHICLE_CLASS_ID,
                   P_VEHICLE_SPEED,
                   P_PLATE_THUMBNAIL,
                   P_VEHICLE_THUMBNAIL,
                   P_VIDEO_URL,
                   P_PROVIDER,
                   P_CREATION_DATE);



      P_ENTRY_ID := NODEFLUX_PACKET_SEQ.CURRVAL;
   END NF_PACKET_INSERT;



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
                               P_MODIFICATION_DATE   IN DATE)
   AS
   BEGIN
      UPDATE TBL_NODEFLUX_PACKET
         SET ENTRY_ID = P_ENTRY_ID,
             TMS_ID = P_TMS_ID,
             EVENT_TYPE = P_EVENT_TYPE,
             TIMESTAMP = P_TIMESTAMP,
             GANTRY_ID = P_GANTRY_ID,
             LANE_ID = P_LANE_ID,
             CAMERA_POSITION = P_CAMERA_POSITION,
             CAMERA_ID = P_CAMERA_ID,
             CAMERA_NAME = P_CAMERA_NAME,
             CAMERA_ADDRESS = P_CAMERA_ADDRESS,
             CAMERA_COORDINATE = P_CAMERA_COORDINATE,
             PLATE_NUMBER = P_PLATE_NUMBER,
             VEHICLE_CLASS_ID = P_VEHICLE_CLASS_ID,
             VEHICLE_SPEED = P_VEHICLE_SPEED,
             PLATE_THUMBNAIL = P_PLATE_THUMBNAIL,
             VEHICLE_THUMBNAIL = P_VEHICLE_THUMBNAIL,
             VIDEO_URL = P_VIDEO_URL,
             PROVIDER = P_PROVIDER,
             MODIFIER_ID = P_MODIFIER_ID,
             MODIFICATION_DATE = P_MODIFICATION_DATE
       WHERE     ENTRY_ID = P_ENTRY_ID
             AND TMS_ID = P_TMS_ID
             AND GANTRY_ID = P_GANTRY_ID;
   END NF_PACKET_UPDATE;



   PROCEDURE NF_PACKET_DELETE (P_TMS_ID      IN NUMBER,
                               P_GANTRY_ID   IN NUMBER,
                               P_ENTRY_ID    IN NUMBER)
   AS
   BEGIN
      DELETE TBL_NODEFLUX_PACKET
       WHERE     ENTRY_ID = P_ENTRY_ID
             AND TMS_ID = P_TMS_ID
             AND GANTRY_ID = P_GANTRY_ID;
   END NF_PACKET_DELETE;



   /*TRANSACTIONS*/



   /*TRAN_INSERT_BY_CTP*/



   PROCEDURE TRAN_INSERT_BY_CTP (P_TMS_ID                 IN     NUMBER,
                                 P_PLAZA_ID               IN     NUMBER,
                                 P_LANE_ID                IN     NUMBER,
                                 P_TRANSACTION_ID            OUT NUMBER,
                                 P_TRANSACTION_DATETIME   IN     DATE,
                                 P_CT_ENTRY_ID            IN     NUMBER,
                                 P_IS_REGISTERED          IN     NUMBER,
                                 P_CREATION_DATE          IN     DATE)
   AS
   BEGIN
      INSERT INTO TBL_TRANSACTION (TMS_ID,
                                   PLAZA_ID,
                                   LANE_ID,
                                   TRANSACTION_ID,
                                   TRANSACTION_DATETIME,
                                   CT_ENTRY_ID,
                                   IS_REGISTERED,
                                   CREATION_DATE)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   P_LANE_ID,
                   TRANSACTION_SEQ.NEXTVAL,
                   P_TRANSACTION_DATETIME,
                   P_CT_ENTRY_ID,
                   P_IS_REGISTERED,
                   P_CREATION_DATE);



      P_TRANSACTION_ID := TRANSACTION_SEQ.CURRVAL;
   END TRAN_INSERT_BY_CTP;


   PROCEDURE TRAN_INSERT_BY_CTP_REAR (P_TMS_ID                 IN     NUMBER,
                                      P_PLAZA_ID               IN     NUMBER,
                                      P_LANE_ID                IN     NUMBER,
                                      P_TRANSACTION_ID            OUT NUMBER,
                                      P_TRANSACTION_DATETIME   IN     DATE,
                                      P_CT_ENTRY_ID            IN     NUMBER,
                                      P_IS_REGISTERED          IN     NUMBER,
                                      P_CREATION_DATE          IN     DATE)
   AS
   BEGIN
      INSERT INTO TBL_TRANSACTION (TMS_ID,
                                   PLAZA_ID,
                                   LANE_ID,
                                   TRANSACTION_ID,
                                   TRANSACTION_DATETIME,
                                   CT_ENTRY_ID_REAR,
                                   IS_REGISTERED,
                                   CREATION_DATE)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   P_LANE_ID,
                   TRANSACTION_SEQ.NEXTVAL,
                   P_TRANSACTION_DATETIME,
                   P_CT_ENTRY_ID,
                   P_IS_REGISTERED,
                   P_CREATION_DATE);



      P_TRANSACTION_ID := TRANSACTION_SEQ.CURRVAL;
   END TRAN_INSERT_BY_CTP_REAR;

   /*TRAN_INSERT_BY_NFP_FRONT*/



   PROCEDURE TRAN_INSERT_BY_NFP_FRONT (
      P_TMS_ID                 IN     NUMBER,
      P_PLAZA_ID               IN     NUMBER,
      P_LANE_ID                IN     NUMBER,
      P_TRANSACTION_ID            OUT NUMBER,
      P_TRANSACTION_DATETIME   IN     DATE,
      P_NF_ENTRY_ID_FRONT      IN     NUMBER,
      P_IS_REGISTERED          IN     NUMBER,
      P_CREATION_DATE          IN     DATE,
      P_VEHICLESPEED           IN     NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_TRANSACTION (TMS_ID,
                                   PLAZA_ID,
                                   LANE_ID,
                                   TRANSACTION_ID,
                                   TRANSACTION_DATETIME,
                                   NF_ENTRY_ID_FRONT,
                                   IS_REGISTERED,
                                   CREATION_DATE,
                                   VEHICLESPEED)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   P_LANE_ID,
                   TRANSACTION_SEQ.NEXTVAL,
                   P_TRANSACTION_DATETIME,
                   P_NF_ENTRY_ID_FRONT,
                   P_IS_REGISTERED,
                   P_CREATION_DATE,
                   P_VEHICLESPEED);



      P_TRANSACTION_ID := TRANSACTION_SEQ.CURRVAL;
   END TRAN_INSERT_BY_NFP_FRONT;



   /*TRAN_INSERT_BY_NFP_REAR*/



   PROCEDURE TRAN_INSERT_BY_NFP_REAR (P_TMS_ID                 IN     NUMBER,
                                      P_PLAZA_ID               IN     NUMBER,
                                      P_LANE_ID                IN     NUMBER,
                                      P_TRANSACTION_ID            OUT NUMBER,
                                      P_TRANSACTION_DATETIME   IN     DATE,
                                      P_NF_ENTRY_ID_REAR       IN     NUMBER,
                                      P_IS_REGISTERED          IN     NUMBER,
                                      P_CREATION_DATE          IN     DATE,
                                      P_VEHICLESPEED           IN     NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_TRANSACTION (TMS_ID,
                                   PLAZA_ID,
                                   LANE_ID,
                                   TRANSACTION_ID,
                                   TRANSACTION_DATETIME,
                                   NF_ENTRY_ID_REAR,
                                   IS_REGISTERED,
                                   CREATION_DATE,
                                   VEHICLESPEED)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   P_LANE_ID,
                   TRANSACTION_SEQ.NEXTVAL,
                   P_TRANSACTION_DATETIME,
                   P_NF_ENTRY_ID_REAR,
                   P_IS_REGISTERED,
                   P_CREATION_DATE,
                   P_VEHICLESPEED);



      P_TRANSACTION_ID := TRANSACTION_SEQ.CURRVAL;
   END TRAN_INSERT_BY_NFP_REAR;



   /*TRAN_UPDATE_BY_NFP_FRONT*/



   PROCEDURE TRAN_UPDATE_BY_NFP_FRONT (P_TMS_ID              IN NUMBER,
                                       P_PLAZA_ID            IN NUMBER,
                                       P_LANE_ID             IN NUMBER,
                                       P_TRANSACTION_ID      IN NUMBER,
                                       P_VEHICLESPEED        IN DECIMAL,
                                       P_NF_ENTRY_ID_FRONT   IN NUMBER)
   AS
      LASTSPEED   NUMBER;
   BEGIN
      SELECT NVL (VEHICLESPEED, 0)
        INTO LASTSPEED
        FROM TBL_TRANSACTION
       WHERE TRANSACTION_ID = P_TRANSACTION_ID;

      IF (P_VEHICLESPEED > LASTSPEED)
      THEN
         LASTSPEED := P_VEHICLESPEED;
      END IF;

      UPDATE TBL_TRANSACTION
         SET NF_ENTRY_ID_FRONT = P_NF_ENTRY_ID_FRONT,
             LANE_ID = P_LANE_ID,
             VEHICLESPEED = LASTSPEED
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_UPDATE_BY_NFP_FRONT;



   /*TRAN_UPDATE_BY_NFP_REAR*/



   PROCEDURE TRAN_UPDATE_BY_NFP_REAR (P_TMS_ID             IN NUMBER,
                                      P_PLAZA_ID           IN NUMBER,
                                      P_LANE_ID            IN NUMBER,
                                      P_TRANSACTION_ID     IN NUMBER,
                                      P_VEHICLESPEED       IN DECIMAL,
                                      P_NF_ENTRY_ID_REAR   IN NUMBER)
   AS
      LASTSPEED       NUMBER;
      NFFRONTID       NUMBER;
      NFFRONTLANEID   NUMBER;
   BEGIN
      NFFRONTLANEID := P_LANE_ID;

      SELECT NVL (VEHICLESPEED, 0)
        INTO LASTSPEED
        FROM TBL_TRANSACTION
       WHERE TRANSACTION_ID = P_TRANSACTION_ID;

      SELECT NVL (NF_ENTRY_ID_FRONT, 0)
        INTO NFFRONTID
        FROM TBL_TRANSACTION
       WHERE TRANSACTION_ID = P_TRANSACTION_ID;

      IF (NFFRONTID > 0)
      THEN
         SELECT NVL (LANE_ID, 0)
           INTO NFFRONTLANEID
           FROM TBL_NODEFLUX_PACKET
          WHERE ENTRY_ID = NFFRONTID;
      END IF;

      IF (P_VEHICLESPEED > LASTSPEED)
      THEN
         LASTSPEED := P_VEHICLESPEED;
      END IF;


      UPDATE TBL_TRANSACTION
         SET NF_ENTRY_ID_REAR = P_NF_ENTRY_ID_REAR,
             LANE_ID = NFFRONTLANEID,
             VEHICLESPEED = LASTSPEED
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_UPDATE_BY_NFP_REAR;



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
                          P_MODIFICATION_DATE      IN DATE)
   AS
   BEGIN
      UPDATE TBL_TRANSACTION
         SET TRANSACTION_DATETIME = P_TRANSACTION_DATETIME,
             CT_ENTRY_ID = P_CT_ENTRY_ID,
             NF_ENTRY_ID_FRONT = P_NF_ENTRY_ID_FRONT,
             NF_ENTRY_ID_REAR = P_NF_ENTRY_ID_REAR,
             IS_BALANCE_UPDATED = P_IS_BALANCE_UPDATED,
             IS_TRANSFERED = P_IS_TRANSFERED,
             IS_VIOLATION = P_IS_VIOLATION,
             IS_REGISTERED = P_IS_REGISTERED,
             MODIFIER_ID = P_MODIFIER_ID,
             MODIFICATION_DATE = P_MODIFICATION_DATE
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND LANE_ID = P_LANE_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_UPDATE;



   PROCEDURE TRAN_UPDATE_CTP (P_TMS_ID           IN NUMBER,
                              P_PLAZA_ID         IN NUMBER,
                              P_LANE_ID          IN NUMBER,
                              P_TRANSACTION_ID   IN NUMBER,
                              P_CT_ENTRY_ID      IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_TRANSACTION
         SET CT_ENTRY_ID = P_CT_ENTRY_ID
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_UPDATE_CTP;

   PROCEDURE TRAN_UPDATE_CTP_REAR (P_TMS_ID           IN NUMBER,
                                   P_PLAZA_ID         IN NUMBER,
                                   P_LANE_ID          IN NUMBER,
                                   P_TRANSACTION_ID   IN NUMBER,
                                   P_CT_ENTRY_ID      IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_TRANSACTION
         SET CT_ENTRY_ID_REAR = P_CT_ENTRY_ID
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_UPDATE_CTP_REAR;



   PROCEDURE TRAN_UPDATE_NF_FRONT (P_TMS_ID              IN NUMBER,
                                   P_PLAZA_ID            IN NUMBER,
                                   P_LANE_ID             IN NUMBER,
                                   P_TRANSACTION_ID      IN NUMBER,
                                   P_NF_ENTRY_ID_FRONT   IN NUMBER,
                                   P_VEHICLESPEED        IN NUMBER)
   AS
      LASTSPEED   NUMBER;
   BEGIN
      SELECT NVL (VEHICLESPEED, 0)
        INTO LASTSPEED
        FROM TBL_TRANSACTION
       WHERE TRANSACTION_ID = P_TRANSACTION_ID;

      IF (P_VEHICLESPEED > LASTSPEED)
      THEN
         LASTSPEED := P_VEHICLESPEED;
      END IF;

      UPDATE TBL_TRANSACTION
         SET NF_ENTRY_ID_FRONT = P_NF_ENTRY_ID_FRONT,
             LANE_ID = P_LANE_ID,
             VEHICLESPEED = LASTSPEED
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND LANE_ID = P_LANE_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_UPDATE_NF_FRONT;



   PROCEDURE TRAN_UPDATE_NF_REAR (P_TMS_ID             IN NUMBER,
                                  P_PLAZA_ID           IN NUMBER,
                                  P_LANE_ID            IN NUMBER,
                                  P_TRANSACTION_ID     IN NUMBER,
                                  P_NF_ENTRY_ID_REAR   IN NUMBER,
                                  P_VEHICLESPEED       IN NUMBER)
   AS
      LASTSPEED   NUMBER;
   BEGIN
      SELECT NVL (VEHICLESPEED, 0)
        INTO LASTSPEED
        FROM TBL_TRANSACTION
       WHERE TRANSACTION_ID = P_TRANSACTION_ID;

      IF (P_VEHICLESPEED > LASTSPEED)
      THEN
         LASTSPEED := P_VEHICLESPEED;
      END IF;

      UPDATE TBL_TRANSACTION
         SET NF_ENTRY_ID_REAR = P_NF_ENTRY_ID_REAR,
             LANE_ID = P_LANE_ID,
             VEHICLESPEED = LASTSPEED
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND LANE_ID = P_LANE_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_UPDATE_NF_REAR;



   PROCEDURE TRAN_MARK_AS_VIOLATION (P_TMS_ID           IN NUMBER,
                                     P_PLAZA_ID         IN NUMBER,
                                     P_LANE_ID          IN NUMBER,
                                     P_TRANSACTION_ID   IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_TRANSACTION
         SET IS_VIOLATION = 1
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND LANE_ID = P_LANE_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_MARK_AS_VIOLATION;



   PROCEDURE TRAN_MARK_AS_BAL_UPD (P_TMS_ID           IN NUMBER,
                                   P_PLAZA_ID         IN NUMBER,
                                   P_LANE_ID          IN NUMBER,
                                   P_TRANSACTION_ID   IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_TRANSACTION
         SET IS_BALANCE_UPDATED = 1, IS_VIOLATION = NULL
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND LANE_ID = P_LANE_ID
             AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_MARK_AS_BAL_UPD;



   PROCEDURE TRAN_MARK_AS_UNREGISTRED (P_TRANSACTION_ID IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_TRANSACTION
         SET IS_REGISTERED = 2
       WHERE TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_MARK_AS_UNREGISTRED;



   PROCEDURE TRAN_DELETE (P_TRANSACTION_ID IN NUMBER)
   AS
   BEGIN
      DELETE TBL_TRANSACTION
       WHERE TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_DELETE;



   PROCEDURE TRAN_GETALL (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_TRANSACTION
         ORDER BY TRANSACTION_ID;
   END TRAN_GETALL;



   PROCEDURE TRAN_GETBYID (P_TRANSACTION_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_TRANSACTION
            WHERE TRANSACTION_ID = P_TRANSACTION_ID
         ORDER BY TRANSACTION_ID;
   END TRAN_GETBYID;



   PROCEDURE TRAN_GETBYPLAZAID (P_PLAZA_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_TRANSACTION
            WHERE PLAZA_ID = P_PLAZA_ID
         ORDER BY TRANSACTION_ID;
   END TRAN_GETBYPLAZAID;



   PROCEDURE TRAN_GET_CORR_IN_ALL (P_TMS_ID      IN     NUMBER,
                                   P_PLAZA_ID    IN     NUMBER,
                                   P_TIMESTAMP   IN     DATE,
                                   P_VRN         IN     NVARCHAR2,
                                   CUR_OUT          OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         WITH TEMP_TRAN
              AS (SELECT *
                    FROM TBL_TRANSACTION
                   WHERE     TMS_ID = P_TMS_ID
                         AND PLAZA_ID = P_PLAZA_ID
                         AND TRANSACTION_DATETIME BETWEEN (  P_TIMESTAMP
                                                           -   30
                                                             / (24 * 60 * 60))
                                                      AND (  P_TIMESTAMP
                                                           +   30
                                                             / (24 * 60 * 60)))
         SELECT *
           FROM TEMP_TRAN T
                LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP1
                   ON NVL (T.NF_ENTRY_ID_FRONT, 0) = NFP1.ENTRY_ID
                LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP2
                   ON NVL (T.NF_ENTRY_ID_REAR, 0) = NFP2.ENTRY_ID
                LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                   ON NVL (T.CT_ENTRY_ID, 0) = CTP.ENTRY_ID
                LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                   ON CTP.OBJECT_ID = CV.TAG_ID
          WHERE    NFP1.PLATE_NUMBER = P_VRN
                OR NFP2.PLATE_NUMBER = P_VRN
                OR CV.VEH_REG_NO = P_VRN;
   END TRAN_GET_CORR_IN_ALL;



   PROCEDURE TRAN_GET_CORR_IN_NF (P_TMS_ID      IN     NUMBER,
                                  P_PLAZA_ID    IN     NUMBER,
                                  P_TIMESTAMP   IN     DATE,
                                  P_VRN         IN     NVARCHAR2,
                                  CUR_OUT          OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         WITH TEMP_TRAN
              AS (SELECT *
                    FROM TBL_TRANSACTION
                   WHERE     TMS_ID = P_TMS_ID
                         AND PLAZA_ID = P_PLAZA_ID
                         AND TRANSACTION_DATETIME BETWEEN (  P_TIMESTAMP
                                                           -   30
                                                             / (24 * 60 * 60))
                                                      AND (  P_TIMESTAMP
                                                           +   30
                                                             / (24 * 60 * 60)))
         SELECT *
           FROM TEMP_TRAN T
                LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP1
                   ON NVL (T.NF_ENTRY_ID_FRONT, 0) = NFP1.ENTRY_ID
                LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP2
                   ON NVL (T.NF_ENTRY_ID_REAR, 0) = NFP2.ENTRY_ID
          WHERE NFP1.PLATE_NUMBER = P_VRN OR NFP2.PLATE_NUMBER = P_VRN;
   END TRAN_GET_CORR_IN_NF;



   PROCEDURE TRAN_GET_CORR_IN_CT (P_TMS_ID      IN     NUMBER,
                                  P_PLAZA_ID    IN     NUMBER,
                                  P_TIMESTAMP   IN     DATE,
                                  P_VRN         IN     NVARCHAR2,
                                  CUR_OUT          OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         WITH TEMP_TRAN
              AS (SELECT *
                    FROM TBL_TRANSACTION
                   WHERE     TMS_ID = P_TMS_ID
                         AND PLAZA_ID = P_PLAZA_ID
                         AND TRANSACTION_DATETIME BETWEEN (  P_TIMESTAMP
                                                           -   30
                                                             / (24 * 60 * 60))
                                                      AND (  P_TIMESTAMP
                                                           +   30
                                                             / (24 * 60 * 60)))
         SELECT *
           FROM TEMP_TRAN T
                INNER JOIN TBL_CROSSTALK_PACKET CTP
                   ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                INNER JOIN TBL_CUSTOMER_VEHICLE CV
                   ON CTP.OBJECT_ID = CV.TAG_ID
          WHERE CV.VEH_REG_NO = P_VRN;
   END TRAN_GET_CORR_IN_CT;



   PROCEDURE TRAN_LIVEDATA (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT T.TMS_ID,
                  NVL (T.PLAZA_ID, 0) PLAZA_ID,
                  NVL (P.PLAZA_NAME, '') PLAZA_NAME,
                  NVL (T.LANE_ID, '') LANE_ID,
                  NVL (L.LANE_NAME, '') LANE_NAME,
                  T.TRANSACTION_ID,
                  T.TRANSACTION_DATETIME,
                  NVL (CTP.OBJECT_ID, '') CROSSTALK_TAG_ID,
                  NVL (CV.VEHICLE_CLASS_ID, '') CROSSTALK_VEHICLE_CLASS_ID,
                  NVL (CV.VEH_REG_NO, '') CROSSTALK_VRN,
                  NVL (CTP.TIME_STAMP, '') CROSSTALK_TIMESTAMP,
                  NVL (NFP1.PLATE_NUMBER, '') NODEFLUX_VRN,
                  NVL (NFP1.VEHICLE_CLASS_ID, '') NODEFLUX_VEHICLE_CLASS_ID,
                  NVL (NFP1.TIMESTAMP, '') NODEFLUX_TIMESTAMP,
                  NVL (CTP.ENTRY_ID, 0) CROSSTALK_ENTRY_ID,
                  NVL (T.NF_ENTRY_ID_FRONT, 0) NODEFLUX_ENTRY_ID,
                  NVL (T.IS_BALANCE_UPDATED, 0) IS_BALANCE_UPDATED,
                  NVL (T.IS_TRANSFERED, 0) IS_TRANSFERED,
                  NVL (T.IS_VIOLATION, 0) IS_VIOLATION,
                  NVL (T.MODIFIER_ID, 0) MODIFIER_ID,
                  T.CREATION_DATE,
                  T.MODIFICATION_DATE,
                  NVL (NFP1.PLATE_THUMBNAIL, '') PLATE_THUMBNAIL,
                  NVL (NFP1.VEHICLE_THUMBNAIL, '') VEHICLE_THUMBNAIL,
                  NVL (CV.VEH_REG_NO, ''),
                  VEH_REG_NO,
                  NVL (VC.VEHICLE_CLASS_NAME, '') VEHICLE_CLASS_NAME
             FROM TBL_TRANSACTION T
                  LEFT OUTER JOIN TBL_LANE L
                     ON T.LANE_ID = L.LANE_ID
                  LEFT OUTER JOIN TBL_PLAZA P
                     ON T.PLAZA_ID = P.PLAZA_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP1
                     ON T.NF_ENTRY_ID_FRONT = NFP1.ENTRY_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP2
                     ON T.NF_ENTRY_ID_REAR = NFP2.ENTRY_ID
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                     ON CTP.OBJECT_ID = CV.TAG_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
         ORDER BY T.TRANSACTION_DATETIME DESC;
   END TRAN_LIVEDATA;



   PROCEDURE TRAN_LIVEDATABYID (P_TRANSACTION_ID   IN     NUMBER,
                                CUR_OUT               OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT T.TMS_ID,
                  NVL (T.PLAZA_ID, 0) PLAZA_ID,
                  NVL (P.PLAZA_NAME, '') PLAZA_NAME,
                  NVL (T.LANE_ID, '') LANE_ID,
                  NVL (L.LANE_NAME, '') LANE_NAME,
                  T.TRANSACTION_ID,
                  T.TRANSACTION_DATETIME,
                  T.CREATION_DATE,
                  T.MODIFICATION_DATE,
                  NVL (NFP.PLATE_THUMBNAIL, '') PLATE_THUMBNAIL,
                  NVL (NFP.VEHICLE_THUMBNAIL, '') VEHICLE_THUMBNAIL,
                  NVL (CV.VEH_REG_NO, ''),
                  VEH_REG_NO,
                  NVL (VC.VEHICLE_CLASS_NAME, '') VEHICLE_CLASS_NAME
             FROM TBL_TRANSACTION T
                  LEFT OUTER JOIN TBL_LANE L
                     ON T.LANE_ID = L.LANE_ID
                  LEFT OUTER JOIN TBL_PLAZA P
                     ON T.PLAZA_ID = P.PLAZA_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP
                     ON T.NF_ENTRY_ID_FRONT = NFP.ENTRY_ID
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                     ON CTP.OBJECT_ID = CV.TAG_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
         ORDER BY T.TRANSACTION_DATETIME DESC;
   END TRAN_LIVEDATABYID;



   PROCEDURE TRAN_UPDATE_AUDIT_SECTION (
      P_TMS_ID                       IN NUMBER,
      P_PLAZA_ID                     IN NUMBER,
      P_LANE_ID                      IN NUMBER,
      P_TRANSACTION_ID               IN NUMBER,
      P_AUDIT_STATUS                 IN NUMBER,
      P_AUDITOR_ID                   IN NUMBER,
      P_AUDIT_DATE                   IN DATE,
      P_AUDITED_VEHICLE_CLASS_ID     IN NUMBER,
      P_AUDITED_VRN                  IN NVARCHAR2,
      P_AUDITED_TRANSCATION_STATUS   IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_TRANSACTION
         SET AUDIT_STATUS = P_AUDIT_STATUS,
             AUDITOR_ID = P_AUDITOR_ID,
             AUDIT_DATE = P_AUDIT_DATE,
             AUDITED_VEHICLE_CLASS_ID = P_AUDITED_VEHICLE_CLASS_ID,
             AUDITED_VRN = P_AUDITED_VRN,
             TRANS_STATUS = P_AUDITED_TRANSCATION_STATUS
       WHERE PLAZA_ID = P_PLAZA_ID AND TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_UPDATE_AUDIT_SECTION;



   PROCEDURE TRAN_GETFILTERED (P_FILTER IN NVARCHAR2, CUR_OUT OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT T.TMS_ID,
       T.PLAZA_ID,
       P.PLAZA_NAME,
       T.LANE_ID,
       L.LANE_NAME,
       T.TRANSACTION_ID,
       T.TRANSACTION_DATETIME,
       T.CT_ENTRY_ID,
       CTP.OBJECT_ID AS TAG_ID,
       CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
       VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
       CTP.PLATE_NUMBER AS CTP_VRN,
       T.CT_ENTRY_ID_REAR,
       CTPR.OBJECT_ID AS TAG_IDR,
       CTPR.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_IDR,
       VC_CTPR.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAMER,
       CTPR.PLATE_NUMBER AS CTP_VRNR,
       T.NF_ENTRY_ID_FRONT,
       NFPF.PLATE_NUMBER AS FRONT_VRN,
       NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
       VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
       NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
       NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
       T.NF_ENTRY_ID_REAR,
       NFPR.PLATE_NUMBER AS REAR_VRN,
       NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
       VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
       NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
       NFPR.VIDEO_URL AS REAR_VIDEO_URL,
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
       AH.AMOUNT,
       T.MEARGED_TRAN_ID,
       T.TRANS_STATUS
  FROM TBL_TRANSACTION T
       LEFT OUTER JOIN TBL_PLAZA P
          ON T.PLAZA_ID = P.PLAZA_ID
       LEFT OUTER JOIN TBL_LANE L
          ON T.LANE_ID = L.LANE_ID
       LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
          ON T.CT_ENTRY_ID = CTP.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
          ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTPR
          ON T.CT_ENTRY_ID_REAR = CTPR.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTPR
          ON VC_CTPR.VEHICLE_CLASS_ID = CTPR.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
          ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPF
          ON CV_NFPF.VEH_REG_NO = NFPF.PLATE_NUMBER
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
          ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
          ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPR
          ON CV_NFPR.VEH_REG_NO = NFPR.PLATE_NUMBER
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
          ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
          ON T.TRANSACTION_ID = AH.TRANSACTION_ID '
         || P_FILTER
         || '



ORDER BY TRANSACTION_DATETIME DESC';



      OPEN CUR_OUT FOR SQLQUERY;
   END TRAN_GETFILTERED;



   PROCEDURE TRAN_GETFILTERED_BYID (P_TRANSACTION_ID   IN     NUMBER,
                                    CUR_OUT               OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT T.TMS_ID,
                T.PLAZA_ID,
                P.PLAZA_NAME,
                T.LANE_ID,
                L.LANE_NAME,
                T.TRANSACTION_ID,
                T.TRANSACTION_DATETIME,
                T.CT_ENTRY_ID,
                CTP.OBJECT_ID AS TAG_ID,
                CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
                VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
                CTP.PLATE_NUMBER AS CTP_VRN,
                T.CT_ENTRY_ID_REAR,
                CTPR.OBJECT_ID AS TAG_IDR,
                CTPR.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_IDR,
                VC_CTPR.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAMER,
                CTPR.PLATE_NUMBER AS CTP_VRNR,
                T.NF_ENTRY_ID_FRONT,
                NFPF.PLATE_NUMBER AS FRONT_VRN,
                NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
                VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
                NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
                NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
                T.NF_ENTRY_ID_REAR,
                NFPR.PLATE_NUMBER AS REAR_VRN,
                NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
                VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
                NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
                NFPR.VIDEO_URL AS REAR_VIDEO_URL,
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
                AH.AMOUNT,
                T.MEARGED_TRAN_ID,
                T.TRANS_STATUS
           FROM TBL_TRANSACTION T
                LEFT OUTER JOIN TBL_PLAZA P
                   ON T.PLAZA_ID = P.PLAZA_ID
                LEFT OUTER JOIN TBL_LANE L
                   ON T.LANE_ID = L.LANE_ID
                LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                   ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
                   ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
                LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTPR
                   ON T.CT_ENTRY_ID_REAR = CTPR.ENTRY_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTPR
                   ON VC_CTPR.VEHICLE_CLASS_ID = CTPR.VEHICLE_CLASS_ID
                LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
                   ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
                LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPF
                   ON CV_NFPF.VEH_REG_NO = NFPF.PLATE_NUMBER
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
                   ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
                LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
                   ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
                LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPR
                   ON CV_NFPR.VEH_REG_NO = NFPR.PLATE_NUMBER
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
                   ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
                LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
                   ON T.TRANSACTION_ID = AH.TRANSACTION_ID
          WHERE T.TRANSACTION_ID = P_TRANSACTION_ID;
   END TRAN_GETFILTERED_BYID;



   PROCEDURE TRAN_UNREVIEWED_FILTERED (P_FILTER   IN     NVARCHAR2,
                                       CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT ROW_NUMBER () OVER (ORDER BY TRANSACTION_DATETIME DESC) AS ROWNUMBER,
       T.TMS_ID,
       T.PLAZA_ID,
       T.PLAZA_NAME,
       T.LANE_ID,
       T.LANE_NAME,
       T.TRANSACTION_ID,
       T.F_TRANSACTION_DATETIME,
       T.TRANSACTION_DATETIME,
       T.CT_ENTRY_ID,
       CTP.OBJECT_ID AS TAG_ID,
       CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
       VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
       CTP.PLATE_NUMBER AS CTP_VRN,
       T.NF_ENTRY_ID_FRONT,
       NFPF.PLATE_NUMBER AS FRONT_VRN,
       NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
       VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
       NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
       NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
       T.NF_ENTRY_ID_REAR,
       NFPR.PLATE_NUMBER AS REAR_VRN,
       NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
       VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
       NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
       NFPR.VIDEO_URL AS REAR_VIDEO_URL,
       T.IS_BALANCE_UPDATED,
       T.IS_TRANSFERED,
       T.IS_VIOLATION,
       T.IS_REGISTERED,
       T.AUDIT_STATUS,
       T.VEHICLESPEED,
        (CASE
           WHEN NVL(CA_CTP.FIRST_NAME,''-1'')=''-1''
           THEN
              (CASE
                  WHEN NVL(CA_NFPF.FIRST_NAME,''-1'')=''-1''
                  THEN
                     NVL(CA_NFPR.FIRST_NAME,''Not Registered'')
                  ELSE
                     NVL(CA_NFPF.FIRST_NAME,''Not Registered'')
               END)
           ELSE
              NVL(CA_CTP.FIRST_NAME,''Not Registered'')
        END)
         AS FNAME
       
  FROM TRANS_UNREVIEWED T
       LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
          ON T.CT_ENTRY_ID = CTP.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
          ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_CTP
          ON CV_CTP.VEH_REG_NO = CTP.PLATE_NUMBER
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_CTP
          ON CA_CTP.ACCOUNT_ID = CV_CTP.ACCOUNT_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
          ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
          ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPF
          ON CV_NFPF.VEH_REG_NO = NFPF.PLATE_NUMBER
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_NFPF
          ON CA_NFPF.ACCOUNT_ID = CV_NFPF.ACCOUNT_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
          ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
          ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPR
          ON CV_NFPR.VEH_REG_NO = NFPR.PLATE_NUMBER
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_NFPR
          ON CA_NFPR.ACCOUNT_ID = CV_NFPR.ACCOUNT_ID
       LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
          ON T.TRANSACTION_ID = AH.TRANSACTION_ID '
         || P_FILTER
         || '
         
         ORDER BY T.TRANSACTION_DATETIME DESC';

      OPEN CUR_OUT FOR SQLQUERY;
   END TRAN_UNREVIEWED_FILTERED;



   PROCEDURE UNREVIEWED_TRANS_LAZYLOAD (P_PAGE_INDEX   IN     NUMBER,
                                        P_PAGE_SIZE    IN     NUMBER,
                                        CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_TRANS_HISTORY
              AS (  SELECT ROW_NUMBER ()
                              OVER (ORDER BY TRANSACTION_DATETIME DESC)
                              AS ROWNUMBER,
                           T.TMS_ID,
                           T.PLAZA_ID,
                           P.PLAZA_NAME,
                           T.LANE_ID,
                           L.LANE_NAME,
                           T.TRANSACTION_ID,
                           T.F_TRANSACTION_DATETIME AS TRANSACTION_DATETIME,
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
                      FROM TRANS_UNREVIEWED T
                           LEFT OUTER JOIN TBL_PLAZA P
                              ON T.PLAZA_ID = P.PLAZA_ID
                           LEFT OUTER JOIN TBL_LANE L
                              ON T.LANE_ID = L.LANE_ID
                     WHERE     NVL (T.IS_BALANCE_UPDATED, 2) <> 1
                           AND NVL (T.AUDIT_STATUS, 2) <> 1
                           AND ROWNUM <= (P_PAGE_INDEX * P_PAGE_SIZE)
                  ORDER BY TRANSACTION_DATETIME DESC)
           SELECT ROWNUMBER,
                  T.TMS_ID,
                  T.PLAZA_ID,
                  T.PLAZA_NAME,
                  T.LANE_ID,
                  T.LANE_NAME,
                  T.TRANSACTION_ID,
                  T.TRANSACTION_DATETIME,
                  T.CT_ENTRY_ID,
                  CTP.OBJECT_ID AS TAG_ID,
                  CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
                  VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
                  CTP.PLATE_NUMBER AS CTP_VRN,
                  T.NF_ENTRY_ID_FRONT,
                  NFPF.PLATE_NUMBER AS FRONT_VRN,
                  NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
                  VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
                  NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
                  NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
                  T.NF_ENTRY_ID_REAR,
                  NFPR.PLATE_NUMBER AS REAR_VRN,
                  NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
                  VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
                  NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
                  NFPR.VIDEO_URL AS REAR_VIDEO_URL,
                  T.IS_BALANCE_UPDATED,
                  T.IS_TRANSFERED,
                  T.IS_VIOLATION,
                  T.IS_REGISTERED,
                  T.AUDIT_STATUS,
                  T.VEHICLESPEED
             FROM CTE_TRANS_HISTORY T
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
                     ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
                     ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
                     ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
                     ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
                     ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END UNREVIEWED_TRANS_LAZYLOAD;



   PROCEDURE TRAN_UNREVIEWED_BY_ID (P_TRANSACTION_ID   IN     NUMBER,
                                    CUR_OUT               OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT T.TMS_ID,
                  T.PLAZA_ID,
                  T.PLAZA_NAME,
                  T.LANE_ID,
                  T.LANE_NAME,
                  T.TRANSACTION_ID,
                  TO_CHAR (T.TRANSACTION_DATETIME, 'DD-Mon-YYYY HH:MI:SS AM')
                     TRANSACTION_DATETIME,
                  T.CT_ENTRY_ID,
                  CTP.OBJECT_ID AS TAG_ID,
                  CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
                  VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
                  CTP.PLATE_NUMBER AS CTP_VRN,
                  T.NF_ENTRY_ID_FRONT,
                  NFPF.PLATE_NUMBER AS FRONT_VRN,
                  NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
                  VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
                  NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
                  NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
                  T.NF_ENTRY_ID_REAR,
                  NFPR.PLATE_NUMBER AS REAR_VRN,
                  NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
                  VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
                  NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
                  NFPR.VIDEO_URL AS REAR_VIDEO_URL,
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
                  AH.AMOUNT,
                  (CASE
                      WHEN NVL (CA_CTP.FIRST_NAME, '-1') = '-1'
                      THEN
                         (CASE
                             WHEN NVL (CA_NFPF.FIRST_NAME, '-1') = '-1'
                             THEN
                                NVL (CA_NFPR.FIRST_NAME, 'Not Registered')
                             ELSE
                                NVL (CA_NFPF.FIRST_NAME, 'Not Registered')
                          END)
                      ELSE
                         NVL (CA_CTP.FIRST_NAME, 'Not Registered')
                   END)
                     AS FNAME
             FROM TRANS_UNREVIEWED T
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
                     ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_CTP
                     ON CV_CTP.VEH_REG_NO = CTP.PLATE_NUMBER
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_CTP
                     ON CA_CTP.ACCOUNT_ID = CV_CTP.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
                     ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
                     ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPF
                     ON CV_NFPF.VEH_REG_NO = NFPF.PLATE_NUMBER
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_NFPF
                     ON CA_NFPF.ACCOUNT_ID = CV_NFPF.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
                     ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
                     ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPR
                     ON CV_NFPR.VEH_REG_NO = NFPR.PLATE_NUMBER
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_NFPR
                     ON CA_NFPR.ACCOUNT_ID = CV_NFPR.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
                     ON T.TRANSACTION_ID = AH.TRANSACTION_ID
            WHERE T.TRANSACTION_ID = P_TRANSACTION_ID
         ORDER BY TRANSACTION_DATETIME DESC;
   END TRAN_UNREVIEWED_BY_ID;



   PROCEDURE TRAN_REVIEWED_FILTERED (P_FILTER   IN     NVARCHAR2,
                                     CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT ROW_NUMBER () OVER (ORDER BY TRANSACTION_DATETIME DESC) AS ROWNUMBER,
                  T.TMS_ID,
                  T.PLAZA_ID,
                  T.PLAZA_NAME,
                  T.LANE_ID,
                  T.LANE_NAME,
                  T.TRANSACTION_ID,
                  T.F_TRANSACTION_DATETIME, 
                  T.TRANSACTION_DATETIME,
                  T.CT_ENTRY_ID,
                  CTP.OBJECT_ID AS TAG_ID,
                  CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
                  VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
                  CTP.PLATE_NUMBER AS CTP_VRN,
                  T.NF_ENTRY_ID_FRONT,
                  NFPF.PLATE_NUMBER AS FRONT_VRN,
                  NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
                  VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
                  NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
                  NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
                  T.NF_ENTRY_ID_REAR,
                  NFPR.PLATE_NUMBER AS REAR_VRN,
                  NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
                  VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
                  NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
                  NFPR.VIDEO_URL AS REAR_VIDEO_URL,
                  T.IS_REGISTERED,
                  T.VEHICLESPEED,
                  T.AUDITOR_ID,
                  U.FIRST_NAME AS AUDITOR_NAME,
                  T.AUDIT_DATE,
                  T.AUDITED_VEHICLE_CLASS_ID,
                  VC_AUDIT.VEHICLE_CLASS_NAME AS AUDIT_VEHICLE_CLASS_NAME,
                  T.AUDITED_VRN,
                  T.MEARGED_TRAN_ID,
                  T.TRANS_STATUS,
                  T.TRANS_STATUS_NAME,
                  (CASE
           WHEN NVL(CA_CTP.FIRST_NAME,''-1'')=''-1''
           THEN
              (CASE
                  WHEN NVL(CA_NFPF.FIRST_NAME,''-1'')=''-1''
                  THEN
                     NVL(CA_NFPR.FIRST_NAME,''Not Registered'')
                  ELSE
                     NVL(CA_NFPF.FIRST_NAME,''Not Registered'')
               END)
           ELSE
              NVL(CA_CTP.FIRST_NAME,''Not Registered'')
        END)
         AS FNAME
         FROM TRANS_REVIEWED T
       LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
          ON T.CT_ENTRY_ID = CTP.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
          ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_CTP
          ON CV_CTP.VEH_REG_NO = CTP.PLATE_NUMBER
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_CTP
          ON CA_CTP.ACCOUNT_ID = CV_CTP.ACCOUNT_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
          ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
          ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPF
          ON CV_NFPF.VEH_REG_NO = NFPF.PLATE_NUMBER
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_NFPF
          ON CA_NFPF.ACCOUNT_ID = CV_NFPF.ACCOUNT_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
          ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
          ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPR
          ON CV_NFPR.VEH_REG_NO = NFPR.PLATE_NUMBER
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_NFPR
          ON CA_NFPR.ACCOUNT_ID = CV_NFPR.ACCOUNT_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_AUDIT
          ON T.AUDITED_VEHICLE_CLASS_ID = VC_AUDIT.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_USER U
          ON T.AUDITOR_ID = U.USER_ID '
         || P_FILTER
         || ' ORDER BY TRANSACTION_DATETIME DESC';

      OPEN CUR_OUT FOR SQLQUERY;
   END TRAN_REVIEWED_FILTERED;



   PROCEDURE TRAN_REVIEWED_LAZYLOAD (P_PAGE_INDEX   IN     NUMBER,
                                     P_PAGE_SIZE    IN     NUMBER,
                                     CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_TRANS_HISTORY
              AS (  SELECT ROW_NUMBER () OVER (ORDER BY AUDIT_DATE DESC)
                              AS ROWNUMBER,
                           TMS_ID,
                           PLAZA_ID,
                           PLAZA_NAME,
                           LANE_ID,
                           LANE_NAME,
                           TRANSACTION_ID,
                           TRANSACTION_DATETIME,
                           F_TRANSACTION_DATETIME,
                           CT_ENTRY_ID,
                           NF_ENTRY_ID_FRONT,
                           NF_ENTRY_ID_REAR,
                           IS_REGISTERED,
                           VEHICLESPEED,
                           AUDITOR_ID,
                           AUDIT_DATE,
                           AUDITED_VEHICLE_CLASS_ID,
                           AUDITED_VRN,
                           MEARGED_TRAN_ID,
                           TRANS_STATUS,
                           TRANS_STATUS_NAME
                      FROM TRANS_REVIEWED T
                     WHERE ROWNUM <= (P_PAGE_INDEX * P_PAGE_SIZE)
                  ORDER BY AUDIT_DATE DESC)
           SELECT ROWNUMBER,
                  T.TMS_ID,
                  T.PLAZA_ID,
                  T.PLAZA_NAME,
                  T.LANE_ID,
                  T.LANE_NAME,
                  T.TRANSACTION_ID,
                  F_TRANSACTION_DATETIME AS TRANSACTION_DATETIME,
                  T.CT_ENTRY_ID,
                  CTP.OBJECT_ID AS TAG_ID,
                  CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
                  VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
                  CTP.PLATE_NUMBER AS CTP_VRN,
                  T.NF_ENTRY_ID_FRONT,
                  NFPF.PLATE_NUMBER AS FRONT_VRN,
                  NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
                  VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
                  NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
                  NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
                  T.NF_ENTRY_ID_REAR,
                  NFPR.PLATE_NUMBER AS REAR_VRN,
                  NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
                  VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
                  NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
                  NFPR.VIDEO_URL AS REAR_VIDEO_URL,
                  T.IS_REGISTERED,
                  T.VEHICLESPEED,
                  T.AUDITOR_ID,
                  U.FIRST_NAME AS AUDITOR_NAME,
                  T.AUDIT_DATE,
                  T.AUDITED_VEHICLE_CLASS_ID,
                  VC_AUDIT.VEHICLE_CLASS_NAME AS AUDIT_VEHICLE_CLASS_NAME,
                  T.AUDITED_VRN,
                  T.MEARGED_TRAN_ID,
                  T.TRANS_STATUS,
                  T.TRANS_STATUS_NAME
             FROM CTE_TRANS_HISTORY T
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
                     ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
                     ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
                     ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
                     ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
                     ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_AUDIT
                     ON T.AUDITED_VEHICLE_CLASS_ID = VC_AUDIT.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_USER U
                     ON T.AUDITOR_ID = U.USER_ID
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END TRAN_REVIEWED_LAZYLOAD;



   PROCEDURE TRAN_CHARGED_FILTERED (P_FILTER   IN     NVARCHAR2,
                                    CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT ROWNUM AS ROWNUMBER,
       T.TMS_ID,
       T.PLAZA_ID,
       T.PLAZA_NAME,
       T.LANE_ID,
       T.LANE_NAME,
       T.TRANSACTION_ID,
       T.F_TRANSACTION_DATETIME, 
       T.TRANSACTION_DATETIME,
       T.CT_ENTRY_ID,
       CTP.OBJECT_ID AS TAG_ID,
       CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
       VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
       CTP.PLATE_NUMBER AS CTP_VRN,
       T.NF_ENTRY_ID_FRONT,
       NFPF.PLATE_NUMBER AS FRONT_VRN,
       NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
       VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
       NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
       NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
       T.NF_ENTRY_ID_REAR,
       NFPR.PLATE_NUMBER AS REAR_VRN,
       NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
       VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
       NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
       NFPR.VIDEO_URL AS REAR_VIDEO_URL,
       T.IS_BALANCE_UPDATED,
       T.IS_TRANSFERED,
       T.IS_VIOLATION,
       T.IS_REGISTERED,
       T.AUDIT_STATUS,
       T.VEHICLESPEED,
       T.AMOUNT,
       T.TSOURCE,
       T.FNAME,
       T.GATEWAY_RESPONSE_CODE,
       T.OPERATOR_RESPONSE_CODE,
       T.OPERATOR_RESPONSE_TEXT,
       T.GATEWAY_RESPONSE_CODE || ''
            - '' || T.OPERATOR_RESPONSE_TEXT AS SMSSTATUS
       
  FROM TRANS_CHARGED T
       LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
          ON T.CT_ENTRY_ID = CTP.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_CT
          ON CTP.PLATE_NUMBER = CV_CT.VEH_REG_NO
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_CT
          ON CV_CT.ACCOUNT_ID = CA_CT.ACCOUNT_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
          ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
          ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPF
          ON CV_NFPF.VEH_REG_NO = NFPF.PLATE_NUMBER
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_NFPF
          ON CV_NFPF.ACCOUNT_ID = CA_NFPF.ACCOUNT_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
          ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
          ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPR
          ON CV_NFPR.VEH_REG_NO = NFPR.PLATE_NUMBER
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA_NFPR
          ON CV_NFPR.ACCOUNT_ID = CA_NFPR.ACCOUNT_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
          ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID '
         || P_FILTER
         || ' ORDER BY T.TRANSACTION_DATETIME DESC';

      OPEN CUR_OUT FOR SQLQUERY;
   END TRAN_CHARGED_FILTERED;



   PROCEDURE CHARGED_TRANS_LAZYLOAD_TEST (P_PAGE_INDEX   IN     NUMBER,
                                          P_PAGE_SIZE    IN     NUMBER,
                                          CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_TRANS_HISTORY
              AS (  SELECT ROW_NUMBER ()
                              OVER (ORDER BY T.TRANSACTION_DATETIME DESC)
                              AS ROWNUMBER,
                           T.TMS_ID,
                           T.PLAZA_ID,
                           T.PLAZA_NAME,
                           T.LANE_ID,
                           T.LANE_NAME,
                           T.TRANSACTION_ID,
                           T.F_TRANSACTION_DATETIME AS TRANSACTION_DATETIME,
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
                      FROM TRANS_CHARGED T
                     WHERE ROWNUM <= (P_PAGE_INDEX * P_PAGE_SIZE)
                  ORDER BY T.TRANSACTION_DATETIME DESC)
           SELECT ROWNUMBER,
                  T.TMS_ID,
                  T.PLAZA_ID,
                  T.PLAZA_NAME,
                  T.LANE_ID,
                  T.LANE_NAME,
                  T.TRANSACTION_ID,
                  T.TRANSACTION_DATETIME,
                  T.CT_ENTRY_ID,
                  CTP.OBJECT_ID AS TAG_ID,
                  CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
                  VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
                  CTP.PLATE_NUMBER AS CTP_VRN,
                  T.NF_ENTRY_ID_FRONT,
                  NFPF.PLATE_NUMBER AS FRONT_VRN,
                  NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
                  VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
                  NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
                  NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
                  T.NF_ENTRY_ID_REAR,
                  NFPR.PLATE_NUMBER AS REAR_VRN,
                  NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
                  VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
                  NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
                  NFPR.VIDEO_URL AS REAR_VIDEO_URL,
                  T.IS_BALANCE_UPDATED,
                  T.IS_TRANSFERED,
                  T.IS_VIOLATION,
                  T.IS_REGISTERED,
                  T.AUDIT_STATUS,
                  T.VEHICLESPEED,
                  AH.AMOUNT
             FROM CTE_TRANS_HISTORY T
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
                     ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
                     ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
                     ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
                     ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
                     ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
                     ON T.TRANSACTION_ID = AH.TRANSACTION_ID
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END CHARGED_TRANS_LAZYLOAD_TEST;



   PROCEDURE TRAN_VIOLATION_FILTERED (P_FILTER   IN     NVARCHAR2,
                                      CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' 
SELECT T.TMS_ID,
       T.PLAZA_ID,
       T.PLAZA_NAME,
       T.LANE_ID,
       T.LANE_NAME,
       T.TRANSACTION_ID,
       T.TRANSACTION_DATETIME,
       T.CT_ENTRY_ID,
       CTP.OBJECT_ID AS TAG_ID,
       CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
       VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
       CTP.PLATE_NUMBER AS CTP_VRN,
       T.NF_ENTRY_ID_FRONT,
       NFPF.PLATE_NUMBER AS FRONT_VRN,
       NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
       VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
       NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
       NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
       T.NF_ENTRY_ID_REAR,
       NFPR.PLATE_NUMBER AS REAR_VRN,
       NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
       VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
       NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
       NFPR.VIDEO_URL AS REAR_VIDEO_URL,
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
       AH.AMOUNT
  FROM TRANS_UNREVIEWED T
       LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
          ON T.CT_ENTRY_ID = CTP.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
          ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
          ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPF
          ON CV_NFPF.VEH_REG_NO = NFPF.PLATE_NUMBER
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
          ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
          ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPR
          ON CV_NFPR.VEH_REG_NO = NFPR.PLATE_NUMBER
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
          ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
          ON T.TRANSACTION_ID = AH.TRANSACTION_ID '
         || P_FILTER
         || '



ORDER BY TRANSACTION_DATETIME DESC';



      OPEN CUR_OUT FOR SQLQUERY;
   END TRAN_VIOLATION_FILTERED;



   PROCEDURE VIOLATION_TRANS_LAZYLOAD (P_PAGE_INDEX   IN     NUMBER,
                                       P_PAGE_SIZE    IN     NUMBER,
                                       CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_TRANS_HISTORY
              AS (  SELECT ROW_NUMBER () OVER (ORDER BY AUDIT_DATE DESC)
                              AS ROWNUMBER,
                           T.TMS_ID,
                           T.PLAZA_ID,
                           T.PLAZA_NAME,
                           T.LANE_ID,
                           T.LANE_NAME,
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
                      FROM TRANS_VIOLATION T
                     WHERE ROWNUM <= (P_PAGE_INDEX * P_PAGE_SIZE)
                  ORDER BY AUDIT_DATE DESC)
           SELECT ROWNUMBER,
                  T.TMS_ID,
                  T.PLAZA_ID,
                  T.PLAZA_NAME,
                  T.LANE_ID,
                  T.LANE_NAME,
                  T.TRANSACTION_ID,
                  T.TRANSACTION_DATETIME,
                  T.CT_ENTRY_ID,
                  CTP.OBJECT_ID AS TAG_ID,
                  CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
                  VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
                  CTP.PLATE_NUMBER AS CTP_VRN,
                  T.NF_ENTRY_ID_FRONT,
                  NFPF.PLATE_NUMBER AS FRONT_VRN,
                  NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
                  VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
                  NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
                  NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
                  T.NF_ENTRY_ID_REAR,
                  NFPR.PLATE_NUMBER AS REAR_VRN,
                  NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
                  VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
                  NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
                  NFPR.VIDEO_URL AS REAR_VIDEO_URL,
                  T.IS_BALANCE_UPDATED,
                  T.IS_TRANSFERED,
                  T.IS_VIOLATION,
                  T.IS_REGISTERED,
                  T.AUDIT_STATUS,
                  T.VEHICLESPEED
             FROM CTE_TRANS_HISTORY T
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
                     ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
                     ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
                     ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
                     ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
                     ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END VIOLATION_TRANS_LAZYLOAD;



   PROCEDURE TRAN_UNIDENTIFIED_FILTERED (P_FILTER   IN     NVARCHAR2,
                                         CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' 
SELECT T.TMS_ID,
       T.PLAZA_ID,
       T.PLAZA_NAME,
       T.LANE_ID,
       T.LANE_NAME,
       T.TRANSACTION_ID,
       T.TRANSACTION_DATETIME,
       T.CT_ENTRY_ID,
       CTP.OBJECT_ID AS TAG_ID,
       CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
       VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
       CTP.PLATE_NUMBER AS CTP_VRN,
       T.NF_ENTRY_ID_FRONT,
       NFPF.PLATE_NUMBER AS FRONT_VRN,
       NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
       VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
       NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
       NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
       T.NF_ENTRY_ID_REAR,
       NFPR.PLATE_NUMBER AS REAR_VRN,
       NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
       VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
       NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
       NFPR.VIDEO_URL AS REAR_VIDEO_URL,
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
       AH.AMOUNT
  FROM TRANS_UNREVIEWED T
       LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
          ON T.CT_ENTRY_ID = CTP.ENTRY_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
          ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
          ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPF
          ON CV_NFPF.VEH_REG_NO = NFPF.PLATE_NUMBER
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
          ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
          ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
       LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV_NFPR
          ON CV_NFPR.VEH_REG_NO = NFPR.PLATE_NUMBER
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
          ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
       LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
          ON T.TRANSACTION_ID = AH.TRANSACTION_ID '
         || P_FILTER
         || '



ORDER BY TRANSACTION_DATETIME DESC';



      OPEN CUR_OUT FOR SQLQUERY;
   END TRAN_UNIDENTIFIED_FILTERED;



   PROCEDURE UNIDENTIFIED_TRANS_LAZYLOAD (P_PAGE_INDEX   IN     NUMBER,
                                          P_PAGE_SIZE    IN     NUMBER,
                                          CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_TRANS_HISTORY
              AS (  SELECT ROW_NUMBER () OVER (ORDER BY AUDIT_DATE DESC)
                              AS ROWNUMBER,
                           T.TMS_ID,
                           T.PLAZA_ID,
                           T.PLAZA_NAME,
                           T.LANE_ID,
                           T.LANE_NAME,
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
                      FROM TRANS_UNIDENTIFIED T
                     WHERE ROWNUM <= (P_PAGE_INDEX * P_PAGE_SIZE)
                  ORDER BY AUDIT_DATE DESC)
           SELECT ROWNUMBER,
                  T.TMS_ID,
                  T.PLAZA_ID,
                  T.PLAZA_NAME,
                  T.LANE_ID,
                  T.LANE_NAME,
                  T.TRANSACTION_ID,
                  T.TRANSACTION_DATETIME,
                  T.CT_ENTRY_ID,
                  CTP.OBJECT_ID AS TAG_ID,
                  CTP.VEHICLE_CLASS_ID AS CTP_VEHICLE_CLASS_ID,
                  VC_CTP.VEHICLE_CLASS_NAME AS CTP_VEHICLE_CLASS_NAME,
                  CTP.PLATE_NUMBER AS CTP_VRN,
                  T.NF_ENTRY_ID_FRONT,
                  NFPF.PLATE_NUMBER AS FRONT_VRN,
                  NFPF.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_FRONT,
                  VC_NFPF.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_FRONT,
                  NFPF.PLATE_THUMBNAIL AS FRONT_IMAGE,
                  NFPF.VIDEO_URL AS FRONT_VIDEO_URL,
                  T.NF_ENTRY_ID_REAR,
                  NFPR.PLATE_NUMBER AS REAR_VRN,
                  NFPR.VEHICLE_CLASS_ID AS NFP_VEHICLE_CLASS_ID_REAR,
                  VC_NFPR.VEHICLE_CLASS_NAME AS NFP_VEHICLE_CLASS_NAME_REAR,
                  NFPR.PLATE_THUMBNAIL AS REAR_IMAGE,
                  NFPR.VIDEO_URL AS REAR_VIDEO_URL,
                  T.IS_BALANCE_UPDATED,
                  T.IS_TRANSFERED,
                  T.IS_VIOLATION,
                  T.IS_REGISTERED,
                  T.AUDIT_STATUS,
                  T.VEHICLESPEED
             FROM CTE_TRANS_HISTORY T
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON T.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_CTP
                     ON VC_CTP.VEHICLE_CLASS_ID = CTP.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPF
                     ON T.NF_ENTRY_ID_FRONT = NFPF.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPF
                     ON NFPF.VEHICLE_CLASS_ID = VC_NFPF.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFPR
                     ON T.NF_ENTRY_ID_REAR = NFPR.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC_NFPR
                     ON NFPR.VEHICLE_CLASS_ID = VC_NFPR.VEHICLE_CLASS_ID
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END UNIDENTIFIED_TRANS_LAZYLOAD;



   /*PLAZA*/



   PROCEDURE PLAZA_INSERT (P_TMS_ID          IN NUMBER,
                           P_PLAZA_ID        IN NUMBER,
                           P_PLAZA_NAME      IN NVARCHAR2,
                           P_LOCATION        IN NVARCHAR2,
                           P_IPADDRESS       IN NVARCHAR2,
                           P_LONGITUDE       IN NUMBER,
                           P_LATITUDE        IN NUMBER,
                           P_CREATION_DATE   IN DATE)
   AS
   BEGIN
      INSERT INTO TBL_PLAZA (TMS_ID,
                             PLAZA_ID,
                             PLAZA_NAME,
                             LOCATION,
                             IPADDRESS,
                             LONGITUDE,
                             LATITUDE,
                             CREATION_DATE)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   P_PLAZA_NAME,
                   P_LOCATION,
                   P_IPADDRESS,
                   P_LONGITUDE,
                   P_LATITUDE,
                   P_CREATION_DATE);
   END PLAZA_INSERT;



   PROCEDURE PLAZA_UPDATE (P_TMS_ID              IN NUMBER,
                           P_PLAZA_ID            IN NUMBER,
                           P_PLAZA_NAME          IN NVARCHAR2,
                           P_LOCATION            IN NVARCHAR2,
                           P_IPADDRESS           IN NVARCHAR2,
                           P_LONGITUDE           IN NUMBER,
                           P_LATITUDE            IN NUMBER,
                           P_MODIFIER_ID         IN NUMBER,
                           P_MODIFICATION_DATE   IN DATE)
   AS
   BEGIN
      UPDATE TBL_PLAZA
         SET PLAZA_NAME = P_PLAZA_NAME,
             LOCATION = P_LOCATION,
             IPADDRESS = P_IPADDRESS,
             LONGITUDE = P_LONGITUDE,
             LATITUDE = P_LATITUDE,
             MODIFIER_ID = P_MODIFIER_ID,
             MODIFICATION_DATE = MODIFICATION_DATE
       WHERE TMS_ID = P_TMS_ID AND PLAZA_ID = P_PLAZA_ID;
   END PLAZA_UPDATE;



   PROCEDURE PLAZA_GETALL (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_PLAZA
         ORDER BY PLAZA_ID;
   END PLAZA_GETALL;



   PROCEDURE PLAZA_GETBYID (P_PLAZA_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT *
           FROM TBL_PLAZA
          WHERE PLAZA_ID = P_PLAZA_ID;
   END PLAZA_GETBYID;



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
                          P_TRANSFER_STATUS        IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_LANE (TMS_ID,
                            PLAZA_ID,
                            LANE_ID,
                            LANE_TYPE_ID,
                            LANE_NAME,
                            CAMERA_ID_FRONT,
                            CAMERA_ID_REAR,
                            ETC_ANTENNA_ID_FRONT,
                            ETC_ANTENNA_ID_REAR,
                            CREATION_DATE,
                            TRANSFER_STATUS)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   P_LANE_ID,
                   P_LANE_TYPE_ID,
                   P_LANE_NAME,
                   P_CAMERA_ID_FRONT,
                   P_CAMERA_ID_REAR,
                   P_ETC_ANTENNA_ID_FRONT,
                   P_ETC_ANTENNA_ID_REAR,
                   P_CREATION_DATE,
                   P_TRANSFER_STATUS);
   END LANE_INSERT;



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
                          P_TRANSFER_STATUS        IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_LANE
         SET LANE_NAME = P_LANE_NAME,
             LANE_TYPE_ID = P_LANE_TYPE_ID,
             CAMERA_ID_FRONT = P_CAMERA_ID_FRONT,
             CAMERA_ID_REAR = P_CAMERA_ID_REAR,
             ETC_ANTENNA_ID_FRONT = P_ETC_ANTENNA_ID_FRONT,
             ETC_ANTENNA_ID_REAR = P_ETC_ANTENNA_ID_REAR,
             MODIFIED_BY = P_MODIFIED_BY,
             MODIFICATION_DATE = MODIFICATION_DATE,
             TRANSFER_STATUS = P_TRANSFER_STATUS
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND LANE_ID = P_LANE_ID;
   END LANE_UPDATE;



   PROCEDURE LANE_GETALL (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT L.CAMERA_ID_FRONT,
                  L.CAMERA_ID_REAR,
                  L.CREATION_DATE,
                  L.ETC_ANTENNA_ID_FRONT,
                  L.ETC_ANTENNA_ID_REAR,
                  L.LANE_ID,
                  L.LANE_TYPE_ID,
                  L.LANE_NAME,
                  L.MODIFICATION_DATE,
                  L.MODIFIED_BY,
                  L.PLAZA_ID,
                  (SELECT P.PLAZA_NAME
                     FROM TBL_PLAZA P
                    WHERE P.PLAZA_ID = L.PLAZA_ID)
                     AS PLAZA_NAME,
                  L.TMS_ID,
                  L.TRANSFER_STATUS,
                  (SELECT H.HARDWARE_NAME
                     FROM TBL_HARDWARE H
                    WHERE H.HARDWARE_ID = L.CAMERA_ID_FRONT)
                     AS CAMERA_NAME_FRONT,
                  (SELECT H.HARDWARE_NAME
                     FROM TBL_HARDWARE H
                    WHERE H.HARDWARE_ID = L.CAMERA_ID_REAR)
                     AS CAMERA_NAME_REAR,
                  (SELECT H.HARDWARE_NAME
                     FROM TBL_HARDWARE H
                    WHERE H.HARDWARE_ID = L.ETC_ANTENNA_ID_FRONT)
                     AS ETC_ANTENNA_NAME_FRONT,
                  (SELECT H.HARDWARE_NAME
                     FROM TBL_HARDWARE H
                    WHERE H.HARDWARE_ID = L.ETC_ANTENNA_ID_REAR)
                     AS ETC_ANTENNA_NAME_REAR
             FROM TBL_LANE L
         ORDER BY LANE_ID;
   END LANE_GETALL;



   PROCEDURE LANE_GETBYID (P_LANE_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT L.CAMERA_ID_FRONT,
                L.CAMERA_ID_REAR,
                L.CREATION_DATE,
                L.ETC_ANTENNA_ID_FRONT,
                L.ETC_ANTENNA_ID_REAR,
                L.LANE_ID,
                L.LANE_TYPE_ID,
                L.LANE_NAME,
                L.MODIFICATION_DATE,
                L.MODIFIED_BY,
                L.PLAZA_ID,
                (SELECT P.PLAZA_NAME
                   FROM TBL_PLAZA P
                  WHERE P.PLAZA_ID = L.PLAZA_ID)
                   AS PLAZA_NAME,
                L.TMS_ID,
                L.TRANSFER_STATUS,
                (SELECT H.HARDWARE_NAME
                   FROM TBL_HARDWARE H
                  WHERE H.HARDWARE_ID = L.CAMERA_ID_FRONT)
                   AS CAMERA_NAME_FRONT,
                (SELECT H.HARDWARE_NAME
                   FROM TBL_HARDWARE H
                  WHERE H.HARDWARE_ID = L.CAMERA_ID_REAR)
                   AS CAMERA_NAME_REAR,
                (SELECT H.HARDWARE_NAME
                   FROM TBL_HARDWARE H
                  WHERE H.HARDWARE_ID = L.ETC_ANTENNA_ID_FRONT)
                   AS ETC_ANTENNA_NAME_FRONT,
                (SELECT H.HARDWARE_NAME
                   FROM TBL_HARDWARE H
                  WHERE H.HARDWARE_ID = L.ETC_ANTENNA_ID_REAR)
                   AS ETC_ANTENNA_NAME_REAR
           FROM TBL_LANE L
          WHERE L.LANE_ID = P_LANE_ID;
   END LANE_GETBYID;



   PROCEDURE LANE_GETBYPLAZAID (P_PLAZA_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT L.CAMERA_ID_FRONT,
                L.CAMERA_ID_REAR,
                L.CREATION_DATE,
                L.ETC_ANTENNA_ID_FRONT,
                L.ETC_ANTENNA_ID_REAR,
                L.LANE_ID,
                L.LANE_TYPE_ID,
                L.LANE_NAME,
                L.MODIFICATION_DATE,
                L.MODIFIED_BY,
                L.PLAZA_ID,
                (SELECT P.PLAZA_NAME
                   FROM TBL_PLAZA P
                  WHERE P.PLAZA_ID = L.PLAZA_ID)
                   AS PLAZA_NAME,
                L.TMS_ID,
                L.TRANSFER_STATUS,
                (SELECT H.HARDWARE_NAME
                   FROM TBL_HARDWARE H
                  WHERE H.HARDWARE_ID = L.CAMERA_ID_FRONT)
                   AS CAMERA_NAME_FRONT,
                (SELECT H.HARDWARE_NAME
                   FROM TBL_HARDWARE H
                  WHERE H.HARDWARE_ID = L.CAMERA_ID_REAR)
                   AS CAMERA_NAME_REAR,
                (SELECT H.HARDWARE_NAME
                   FROM TBL_HARDWARE H
                  WHERE H.HARDWARE_ID = L.ETC_ANTENNA_ID_FRONT)
                   AS ETC_ANTENNA_NAME_FRONT,
                (SELECT H.HARDWARE_NAME
                   FROM TBL_HARDWARE H
                  WHERE H.HARDWARE_ID = L.ETC_ANTENNA_ID_REAR)
                   AS ETC_ANTENNA_NAME_REAR
           FROM TBL_LANE L
          WHERE L.PLAZA_ID = P_PLAZA_ID;
   END LANE_GETBYPLAZAID;



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
                              P_TRANSFER_STATUS     IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_HARDWARE (TMS_ID,
                                PLAZA_ID,
                                HARDWARE_ID,
                                HARDWARE_NAME,
                                HARDWARE_TYPE,
                                HARDWARE_POSITION,
                                MANUFACTURER_NAME,
                                MODEL_NAME,
                                IP_ADDRESS,
                                CREATION_DATE,
                                TRANSFER_STATUS)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   P_HARDWARE_ID,
                   P_HARDWARE_NAME,
                   P_HARDWARE_TYPE,
                   P_HARDWARE_POSITION,
                   P_MANUFACTURER_NAME,
                   P_MODEL_NAME,
                   P_IP_ADDRESS,
                   P_CREATION_DATE,
                   P_TRANSFER_STATUS);
   END HARDWARE_INSERT;



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
                              P_TRANSFER_STATUS     IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_HARDWARE
         SET HARDWARE_NAME = P_HARDWARE_NAME,
             HARDWARE_TYPE = P_HARDWARE_TYPE,
             HARDWARE_POSITION = P_HARDWARE_POSITION,
             MANUFACTURER_NAME = P_MANUFACTURER_NAME,
             MODEL_NAME = P_MODEL_NAME,
             IP_ADDRESS = P_IP_ADDRESS,
             MODIFIED_BY = P_MODIFIED_BY,
             MODIFICATION_DATE = MODIFICATION_DATE,
             TRANSFER_STATUS = P_TRANSFER_STATUS
       WHERE     TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID
             AND HARDWARE_ID = P_HARDWARE_ID;
   END HARDWARE_UPDATE;



   PROCEDURE HARDWARE_GETALL (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_HARDWARE
         ORDER BY HARDWARE_ID;
   END HARDWARE_GETALL;



   PROCEDURE HARDWARE_GETBYID (P_HARDWARE_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT *
           FROM TBL_HARDWARE
          WHERE HARDWARE_ID = P_HARDWARE_ID;
   END HARDWARE_GETBYID;



   PROCEDURE HARDWARE_GETBYTYPE (P_HARDWARE_TYPE   IN     NUMBER,
                                 CUR_OUT              OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT *
           FROM TBL_HARDWARE
          WHERE HARDWARE_TYPE = P_HARDWARE_TYPE;
   END HARDWARE_GETBYTYPE;



   PROCEDURE HARDWARE_LATEST_GETALL (P_LAST_UPDATE_TIME   IN     DATE,
                                     CUR_OUT                 OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_HARDWARE
            WHERE CREATION_DATE > P_LAST_UPDATE_TIME
         ORDER BY HARDWARE_ID;
   END HARDWARE_LATEST_GETALL;



   /*VEHICLE CLASS*/



   PROCEDURE VEHICLE_CLASS_INSERT (P_TMS_ID               IN NUMBER,
                                   P_VEHICLE_CLASS_ID     IN NUMBER,
                                   P_VEHICLE_CLASS_NAME   IN NVARCHAR2,
                                   P_CREATION_DATE        IN DATE,
                                   P_TRANSFER_STATUS      IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_VEHICLE_CLASS (TMS_ID,
                                     VEHICLE_CLASS_ID,
                                     VEHICLE_CLASS_NAME,
                                     CREATION_DATE,
                                     TRANSFER_STATUS)
           VALUES (P_TMS_ID,
                   P_VEHICLE_CLASS_ID,
                   P_VEHICLE_CLASS_NAME,
                   P_CREATION_DATE,
                   P_TRANSFER_STATUS);
   END VEHICLE_CLASS_INSERT;



   PROCEDURE VEHICLE_CLASS_UPDATE (P_TMS_ID               IN NUMBER,
                                   P_VEHICLE_CLASS_ID     IN NUMBER,
                                   P_VEHICLE_CLASS_NAME   IN NVARCHAR2,
                                   P_MODIFIED_BY          IN NUMBER,
                                   P_MODIFICATION_DATE    IN DATE,
                                   P_TRANSFER_STATUS      IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_VEHICLE_CLASS
         SET VEHICLE_CLASS_NAME = P_VEHICLE_CLASS_NAME,
             MODIFIED_BY = P_MODIFIED_BY,
             MODIFICATION_DATE = MODIFICATION_DATE,
             TRANSFER_STATUS = P_TRANSFER_STATUS
       WHERE TMS_ID = P_TMS_ID AND VEHICLE_CLASS_ID = P_VEHICLE_CLASS_ID;
   END VEHICLE_CLASS_UPDATE;



   PROCEDURE VEHICLE_CLASS_GETALL (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_VEHICLE_CLASS
         ORDER BY VEHICLE_CLASS_ID;
   END VEHICLE_CLASS_GETALL;



   PROCEDURE VEHICLE_CLASS_GETBYID (P_VEHICLE_CLASS_ID   IN     NUMBER,
                                    CUR_OUT                 OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT *
           FROM TBL_VEHICLE_CLASS
          WHERE VEHICLE_CLASS_ID = P_VEHICLE_CLASS_ID;
   END VEHICLE_CLASS_GETBYID;



   PROCEDURE VEHICLE_CLASS_GETBYNAME (
      P_VEHICLE_CLASS_NAME   IN     NVARCHAR2,
      CUR_OUT                   OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT *
           FROM TBL_VEHICLE_CLASS
          WHERE LOWER (VEHICLE_CLASS_NAME) = LOWER (P_VEHICLE_CLASS_NAME);
   END VEHICLE_CLASS_GETBYNAME;



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
                               P_CREATION_DATE      IN DATE)
   AS
   BEGIN
      INSERT INTO TBL_TOLL_RATE (TMS_ID,
                                 PLAZA_ID,
                                 TRANSFER_STATUS,
                                 RATE_ID,
                                 PROFILE_ID,
                                 VEHICLE_CLASS_ID,
                                 LANE_TYPE_ID,
                                 START_TIME,
                                 END_TIME,
                                 AMOUNT,
                                 DESCRIPTION,
                                 CREATION_DATE)
           VALUES (P_TMS_ID,
                   P_PLAZA_ID,
                   P_TRANSFER_STATUS,
                   TOLL_RATE_SEQ.NEXTVAL,
                   P_PROFILE_ID,
                   P_VEHICLE_CLASS_ID,
                   P_LANE_TYPE_ID,
                   P_START_TIME,
                   P_END_TIME,
                   P_AMOUNT,
                   P_DESCRIPTION,
                   P_CREATION_DATE);
   END TOLL_RATE_INSERT;



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
                               P_MODIFICATION_DATE   IN DATE)
   AS
   BEGIN
      UPDATE TBL_TOLL_RATE
         SET TRANSFER_STATUS = P_TRANSFER_STATUS,
             PROFILE_ID = P_PROFILE_ID,
             VEHICLE_CLASS_ID = P_VEHICLE_CLASS_ID,
             LANE_TYPE_ID = P_LANE_TYPE_ID,
             START_TIME = P_START_TIME,
             END_TIME = P_END_TIME,
             AMOUNT = P_AMOUNT,
             DESCRIPTION = P_DESCRIPTION,
             MODIFIER_ID = P_MODIFIER_ID,
             MODIFICATION_DATE = P_MODIFICATION_DATE
       WHERE     RATE_ID = P_RATE_ID
             AND TMS_ID = P_TMS_ID
             AND PLAZA_ID = P_PLAZA_ID;
   END TOLL_RATE_UPDATE;



   PROCEDURE TOLL_RATE_GETBYID (P_RATE_ID    IN     NUMBER,
                                P_TMS_ID     IN     NUMBER,
                                P_PLAZA_ID   IN     NUMBER,
                                CUR_OUT         OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT *
           FROM TBL_TOLL_RATE
          WHERE     RATE_ID = P_RATE_ID
                AND TMS_ID = P_TMS_ID
                AND PLAZA_ID = P_PLAZA_ID;
   END TOLL_RATE_GETBYID;



   PROCEDURE TOLL_RATE_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT TR.AMOUNT,
                  TR.CREATION_DATE,
                  TR.DESCRIPTION,
                  TR.END_TIME,
                  TR.LANE_TYPE_ID,
                  TR.MODIFICATION_DATE,
                  TR.MODIFIER_ID,
                  TR.MODIFIER_ID,
                  TR.PLAZA_ID,
                  TR.PROFILE_ID,
                  TR.RATE_ID,
                  TR.START_TIME,
                  TR.TMS_ID,
                  TR.TRANSFER_STATUS,
                  TR.VEHICLE_CLASS_ID,
                  (SELECT VEHICLE_CLASS_NAME
                     FROM TBL_VEHICLE_CLASS L
                    WHERE L.VEHICLE_CLASS_ID = TR.VEHICLE_CLASS_ID)
                     AS VEHICLE_CLASS_NAME
             FROM TBL_TOLL_RATE TR
         ORDER BY RATE_ID;
   END TOLL_RATE_GETALL;



   PROCEDURE TOLLRATE_LATEST_GETALL (P_LAST_UPDATE_TIME   IN     DATE,
                                     CUR_OUT                 OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT TR.AMOUNT,
                  TR.CREATION_DATE,
                  TR.DESCRIPTION,
                  TR.END_TIME,
                  TR.LANE_TYPE_ID,
                  TR.MODIFICATION_DATE,
                  TR.MODIFIER_ID,
                  TR.MODIFIER_ID,
                  TR.PLAZA_ID,
                  TR.PROFILE_ID,
                  TR.RATE_ID,
                  TR.START_TIME,
                  TR.TMS_ID,
                  TR.TRANSFER_STATUS,
                  TR.VEHICLE_CLASS_ID,
                  (SELECT VEHICLE_CLASS_NAME
                     FROM TBL_VEHICLE_CLASS L
                    WHERE L.VEHICLE_CLASS_ID = TR.VEHICLE_CLASS_ID)
                     AS VEHICLE_CLASS_NAME
             FROM TBL_TOLL_RATE TR
            WHERE TR.CREATION_DATE > P_LAST_UPDATE_TIME
         ORDER BY RATE_ID;
   END TOLLRATE_LATEST_GETALL;



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
                                 P_MODIFIED_BY               IN     NUMBER,
                                 P_ACCOUNT_HISTORY_ID        IN     NUMBER)
   IS
      C_COUNT_HISTORY   NUMBER;
   BEGIN
      C_COUNT_HISTORY := 0;



      P_ENTRY_ID := 0;



      SELECT COUNT (*)
        INTO C_COUNT_HISTORY
        FROM TBL_SMS_COMM_HISTORY
       WHERE     CUSTOMER_ACCOUNT_ID = P_CUSTOMER_ACCOUNT_ID
             AND CUSTOMER_VECHILE_ID = P_CUSTOMER_VECHILE_ID
             AND CUSTOMER_NAME = P_CUSTOMER_NAME
             AND MESSAGE_BODY = P_MESSAGE_BODY;



      IF (C_COUNT_HISTORY = 0)
      THEN
         INSERT INTO TBL_SMS_COMM_HISTORY (ENTRY_ID,
                                           TMS_ID,
                                           CUSTOMER_ACCOUNT_ID,
                                           CUSTOMER_VECHILE_ID,
                                           CUSTOMER_NAME,
                                           MOBILE_NUMBER,
                                           MESSAGE_DIRECTION,
                                           MESSAGE_BODY,
                                           SENT_STATUS,
                                           RECEIVED_PROCESS_STATUS,
                                           ATTEMPT_COUNT,
                                           CREATION_DATE,
                                           MODIFICATION_DATE,
                                           MODIFIED_BY,
                                           ACCOUNT_HISTORY_ID)
              VALUES (SMS_COMM_HISTORY_SEQ.NEXTVAL,
                      P_TMS_ID,
                      P_CUSTOMER_ACCOUNT_ID,
                      P_CUSTOMER_VECHILE_ID,
                      P_CUSTOMER_NAME,
                      P_MOBILE_NUMBER,
                      P_MESSAGE_DIRECTION,
                      P_MESSAGE_BODY,
                      P_SENT_STATUS,
                      P_RECEIVED_PROCESS_STATUS,
                      0,
                      P_CREATION_DATE,
                      P_MODIFICATION_DATE,
                      P_MODIFIED_BY,
                      P_ACCOUNT_HISTORY_ID);



         SELECT SMS_COMM_HISTORY_SEQ.CURRVAL INTO P_ENTRY_ID FROM DUAL;
      END IF;
   END SMS_HISTORY_INSERT;



   PROCEDURE SMS_HISTORY_UPDATE_FIRST (
      P_ENTRY_ID                  IN NUMBER,
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
      P_MODIFIED_BY               IN NUMBER,
      P_TRANSACTION_ID            IN NVARCHAR2,
      P_GATEWAY_RESPONSE          IN NVARCHAR2,
      P_GATEWAY_RESPONSE_CODE     IN NVARCHAR2,
      P_REFERENCE_NO              IN NVARCHAR2)
   AS
   BEGIN
      UPDATE TBL_SMS_COMM_HISTORY
         SET ENTRY_ID = P_ENTRY_ID,
             TMS_ID = P_TMS_ID,
             CUSTOMER_ACCOUNT_ID = P_CUSTOMER_ACCOUNT_ID,
             CUSTOMER_NAME = P_CUSTOMER_NAME,
             MOBILE_NUMBER = P_MOBILE_NUMBER,
             MESSAGE_DIRECTION = P_MESSAGE_DIRECTION,
             MESSAGE_BODY = P_MESSAGE_BODY,
             SENT_STATUS = P_SENT_STATUS,
             RECEIVED_PROCESS_STATUS = P_RECEIVED_PROCESS_STATUS,
             MESSAGE_SEND_TIME = P_MESSAGE_SEND_TIME,
             MESSAGE_RECEIVE_TIME = P_MESSAGE_RECEIVE_TIME,
             MESSAGE_DELIVERY_STATUS = P_MESSAGE_DELIVERY_STATUS,
             ATTEMPT_COUNT = P_ATTEMPT_COUNT,
             CREATION_DATE = P_CREATION_DATE,
             MODIFICATION_DATE = P_MODIFICATION_DATE,
             MODIFIED_BY = P_MODIFIED_BY,
             TRANSACTION_ID = P_TRANSACTION_ID,
             GATEWAY_RESPONSE = P_GATEWAY_RESPONSE,
             GATEWAY_RESPONSE_CODE = P_GATEWAY_RESPONSE_CODE,
             REFERENCE_NO = P_REFERENCE_NO
       WHERE ENTRY_ID = P_ENTRY_ID AND TMS_ID = P_TMS_ID;
   END SMS_HISTORY_UPDATE_FIRST;

   PROCEDURE SMS_HISTORY_UPDATE_SECOND (
      P_SENT_STATUS               IN NUMBER,
      P_MESSAGE_RECEIVE_TIME      IN DATE,
      P_MESSAGE_DELIVERY_STATUS   IN NUMBER,
      P_OPERATOR_ATTEMPT_COUNT    IN NUMBER,
      P_TRANSACTION_ID            IN NVARCHAR2,
      P_OPERATOR_RESPONSE         IN NVARCHAR2,
      P_OPERATOR_RESPONSE_CODE    IN NVARCHAR2,
      P_MODIFICATION_DATE         IN DATE)
   AS
   BEGIN
      UPDATE TBL_SMS_COMM_HISTORY
         SET SENT_STATUS = P_SENT_STATUS,
             MESSAGE_RECEIVE_TIME = P_MESSAGE_RECEIVE_TIME,
             MESSAGE_DELIVERY_STATUS = P_MESSAGE_DELIVERY_STATUS,
             OPERATOR_ATTEMPT_COUNT =
                NVL (OPERATOR_ATTEMPT_COUNT, 0) + P_OPERATOR_ATTEMPT_COUNT,
             TRANSACTION_ID = P_TRANSACTION_ID,
             OPERATOR_RESPONSE = P_OPERATOR_RESPONSE,
             OPERATOR_RESPONSE_CODE = P_OPERATOR_RESPONSE_CODE,
             MODIFICATION_DATE = P_MODIFICATION_DATE
       WHERE TRANSACTION_ID = P_TRANSACTION_ID;
   END SMS_HISTORY_UPDATE_SECOND;


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
                                 P_MODIFIED_BY               IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_SMS_COMM_HISTORY
         SET ENTRY_ID = P_ENTRY_ID,
             TMS_ID = P_TMS_ID,
             CUSTOMER_ACCOUNT_ID = P_CUSTOMER_ACCOUNT_ID,
             CUSTOMER_NAME = P_CUSTOMER_NAME,
             MOBILE_NUMBER = P_MOBILE_NUMBER,
             MESSAGE_DIRECTION = P_MESSAGE_DIRECTION,
             MESSAGE_BODY = P_MESSAGE_BODY,
             SENT_STATUS = P_SENT_STATUS,
             RECEIVED_PROCESS_STATUS = P_RECEIVED_PROCESS_STATUS,
             MESSAGE_SEND_TIME = P_MESSAGE_SEND_TIME,
             MESSAGE_RECEIVE_TIME = P_MESSAGE_RECEIVE_TIME,
             MESSAGE_DELIVERY_STATUS = P_MESSAGE_DELIVERY_STATUS,
             ATTEMPT_COUNT = P_ATTEMPT_COUNT,
             CREATION_DATE = P_CREATION_DATE,
             MODIFICATION_DATE = P_MODIFICATION_DATE,
             MODIFIED_BY = P_MODIFIED_BY
       WHERE ENTRY_ID = P_ENTRY_ID AND TMS_ID = P_TMS_ID;
   END SMS_HISTORY_UPDATE;



   PROCEDURE SMS_HISTORY_GETFILTERED (P_FILTER   IN     NVARCHAR2,
                                      CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (2000);
   BEGIN
      SQLQUERY :=
            ' SELECT ENTRY_ID,
       TMS_ID,
       CUSTOMER_ACCOUNT_ID,
       CUSTOMER_NAME,
       MOBILE_NUMBER,
       MESSAGE_DIRECTION,
       MESSAGE_BODY,
       SENT_STATUS,
       RECEIVED_PROCESS_STATUS,
       MESSAGE_SEND_TIME,
       MESSAGE_RECEIVE_TIME,
       MESSAGE_DELIVERY_STATUS,
       ATTEMPT_COUNT,
       CREATION_DATE,
       MODIFICATION_DATE,
       MODIFIED_BY,
       GATEWAY_RESPONSE_CODE,
       OPERATOR_RESPONSE_CODE,
       TRANSACTION_ID,
       0 AS TRANSACTION_TYPE,
       ''blank'' AS TRANSACTION_SUBJECT,
       ''blank'' AS EMAIL_ID,
       ''blank''  VEHICLE_RC_NO
  FROM TBL_SMS_COMM_HISTORY '
         || P_FILTER
         || ' ORDER BY ENTRY_ID';



      OPEN CUR_OUT FOR SQLQUERY;
   END SMS_HISTORY_GETFILTERED;



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
                             P_USERPASSWORD              IN     NVARCHAR2)
   AS
   BEGIN
      INSERT INTO TBL_CUSTOMER_ACCOUNT (TMS_ID,
                                        ACCOUNT_ID,
                                        FIRST_NAME,
                                        LAST_NAME,
                                        MOB_NUMBER,
                                        EMAIL_ID,
                                        DESCRIPTION,
                                        ADDRESS,
                                        ACCOUNT_BALANCE,
                                        CREATION_DATE,
                                        CUSTOMER_IMAGE_PATH,
                                        IS_DOC_VERIFIED,
                                        ACCOUNT_STATUS,
                                        TRANSFER_STATUS,
                                        RESIDENT_ID,
                                        BIRTH_PLACE,
                                        BIRTH_DATE,
                                        RT_RW,
                                        PROVINCE_ID,
                                        CITY_ID,
                                        DISTRICT_ID,
                                        SUB_DISTRICT_ID,
                                        POSTAL_CODE,
                                        NATIONALITY,
                                        GENDER,
                                        MARITAL_STATUS,
                                        OCCUPATION,
                                        RESIDENTIDCARDIMAGE,
                                        VALID_UNTIL,
                                        REGISTRATION_THROUGH,
                                        USER_PASSWORD)
           VALUES (P_TMS_ID,
                   CUSTOMER_ACCOUNT_SEQ.NEXTVAL,
                   P_FIRST_NAME,
                   P_LAST_NAME,
                   P_MOB_NUMBER,
                   P_EMAIL_ID,
                   P_DESCRIPTION,
                   P_ADDRESS,
                   P_ACCOUNT_BALANCE,
                   P_CREATION_DATE,
                   P_CUSTOMER_IMAGE_PATH,
                   P_IS_DOC_VERIFIED,
                   P_ACCOUNT_STATUS,
                   P_TRANSFER_STATUS,
                   P_RESIDENT_ID,
                   P_BIRTH_PLACE,
                   P_BIRTH_DATE,
                   P_RT_RW,
                   P_PROVINCE_ID,
                   P_CITY_ID,
                   P_DISTRICT_ID,
                   P_SUB_DISTRICT_ID,
                   P_POSTAL_CODE,
                   P_NATIONALITY,
                   P_GENDER,
                   P_MARITAL_STATUS,
                   P_OCCUPATION,
                   P_RESIDENTIDCARDIMAGEPATH,
                   P_VALID_UNTIL,
                   P_REGISTRATION_THROUGH,
                   P_USERPASSWORD);



      P_ACCOUNT_ID := CUSTOMER_ACCOUNT_SEQ.CURRVAL;
   END ACCOUNT_INSERT;



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
                             P_VALID_UNTIL               IN DATE := NULL)
   AS
   BEGIN
      UPDATE TBL_CUSTOMER_ACCOUNT
         SET TMS_ID = P_TMS_ID,
             FIRST_NAME = P_FIRST_NAME,
             LAST_NAME = P_LAST_NAME,
             MOB_NUMBER = P_MOB_NUMBER,
             EMAIL_ID = P_EMAIL_ID,
             DESCRIPTION = P_DESCRIPTION,
             ADDRESS = P_ADDRESS,
             ACCOUNT_BALANCE = P_ACCOUNT_BALANCE,
             MODIFIER_ID = P_MODIFIER_ID,
             MODIFICATION_DATE = P_MODIFICATION_DATE,
             CUSTOMER_IMAGE_PATH = P_CUSTOMER_IMAGE_PATH,
             IS_DOC_VERIFIED = P_IS_DOC_VERIFIED,
             ACCOUNT_STATUS = P_ACCOUNT_STATUS,
             TRANSFER_STATUS = P_TRANSFER_STATUS,
             RESIDENT_ID = P_RESIDENT_ID,
             BIRTH_PLACE = P_BIRTH_PLACE,
             BIRTH_DATE = P_BIRTH_DATE,
             RT_RW = P_RT_RW,
             PROVINCE_ID = P_PROVINCE_ID,
             CITY_ID = P_CITY_ID,
             DISTRICT_ID = P_DISTRICT_ID,
             SUB_DISTRICT_ID = P_SUB_DISTRICT_ID,
             POSTAL_CODE = P_POSTAL_CODE,
             NATIONALITY = P_NATIONALITY,
             MARITAL_STATUS = P_MARITAL_STATUS,
             OCCUPATION = P_OCCUPATION,
             RESIDENTIDCARDIMAGE = P_RESIDENTIDCARDIMAGEPATH,
             VALID_UNTIL = P_VALID_UNTIL
       WHERE TMS_ID = P_TMS_ID AND ACCOUNT_ID = P_ACCOUNT_ID;
   END ACCOUNT_UPDATE;



   PROCEDURE ACCOUNT_GETBYMOBILENO (P_MOB_NUMBER   IN     NVARCHAR2,
                                    CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CA.TMS_ID,
                CA.ACCOUNT_ID,
                CA.FIRST_NAME,
                CA.LAST_NAME,
                CA.MOB_NUMBER,
                CA.EMAIL_ID,
                CA.DESCRIPTION,
                CA.ADDRESS,
                CA.ACCOUNT_BALANCE,
                CA.CREATION_DATE,
                CA.CUSTOMER_IMAGE_PATH,
                CA.IS_DOC_VERIFIED,
                CA.ACCOUNT_STATUS,
                CA.TRANSFER_STATUS,
                CA.RESIDENT_ID,
                CA.BIRTH_PLACE,
                CA.BIRTH_DATE,
                CA.RT_RW,
                CA.PROVINCE_ID,
                P.PROVINCE_NAME,
                CA.CITY_ID,
                C.CITY_NAME,
                CA.DISTRICT_ID,
                D.DISTRICT_NAME,
                SD.SUB_DISTRICT_NAME,
                CA.SUB_DISTRICT_ID,
                CA.POSTAL_CODE,
                CA.NATIONALITY,
                CA.GENDER,
                CA.MARITAL_STATUS,
                CA.OCCUPATION,
                CA.RESIDENTIDCARDIMAGE,
                CA.VALID_UNTIL,
                CA.REGISTRATION_THROUGH,
                CA.USER_PASSWORD,
                CA.MODIFIER_ID,
                CA.CREATION_DATE,
                CA.MODIFICATION_DATE
           FROM TBL_CUSTOMER_ACCOUNT CA
                LEFT OUTER JOIN TBL_PROVINCE P
                   ON CA.PROVINCE_ID = P.PROVINCE_ID
                LEFT OUTER JOIN TBL_CITY C
                   ON CA.CITY_ID = C.CITY_ID
                LEFT OUTER JOIN TBL_DISTRICT D
                   ON CA.DISTRICT_ID = D.DISTRICT_ID
                LEFT OUTER JOIN TBL_SUB_DISTRICT SD
                   ON CA.SUB_DISTRICT_ID = SD.SUB_DISTRICT_ID
          WHERE CA.MOB_NUMBER = P_MOB_NUMBER;
   END ACCOUNT_GETBYMOBILENO;



   PROCEDURE ACCOUNT_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CA.TMS_ID,
                  CA.ACCOUNT_ID,
                  CA.FIRST_NAME,
                  CA.LAST_NAME,
                  CA.MOB_NUMBER,
                  CA.EMAIL_ID,
                  CA.DESCRIPTION,
                  CA.ADDRESS,
                  CA.ACCOUNT_BALANCE,
                  CA.CREATION_DATE,
                  CA.CUSTOMER_IMAGE_PATH,
                  CA.IS_DOC_VERIFIED,
                  CA.ACCOUNT_STATUS,
                  CA.TRANSFER_STATUS,
                  CA.RESIDENT_ID,
                  CA.BIRTH_PLACE,
                  CA.BIRTH_DATE,
                  CA.RT_RW,
                  CA.PROVINCE_ID,
                  P.PROVINCE_NAME,
                  CA.CITY_ID,
                  C.CITY_NAME,
                  CA.DISTRICT_ID,
                  D.DISTRICT_NAME,
                  SD.SUB_DISTRICT_NAME,
                  CA.SUB_DISTRICT_ID,
                  CA.POSTAL_CODE,
                  CA.NATIONALITY,
                  CA.GENDER,
                  CA.MARITAL_STATUS,
                  CA.OCCUPATION,
                  CA.RESIDENTIDCARDIMAGE,
                  CA.VALID_UNTIL,
                  CA.REGISTRATION_THROUGH,
                  CA.USER_PASSWORD,
                  CA.MODIFIER_ID,
                  CA.CREATION_DATE,
                  CA.MODIFICATION_DATE
             FROM TBL_CUSTOMER_ACCOUNT CA
                  LEFT OUTER JOIN TBL_PROVINCE P
                     ON CA.PROVINCE_ID = P.PROVINCE_ID
                  LEFT OUTER JOIN TBL_CITY C
                     ON CA.CITY_ID = C.CITY_ID
                  LEFT OUTER JOIN TBL_DISTRICT D
                     ON CA.DISTRICT_ID = D.DISTRICT_ID
                  LEFT OUTER JOIN TBL_SUB_DISTRICT SD
                     ON CA.SUB_DISTRICT_ID = SD.SUB_DISTRICT_ID
         ORDER BY CA.CREATION_DATE DESC, CA.MODIFICATION_DATE DESC;
   END ACCOUNT_GETALL;



   PROCEDURE ACCOUNT_GETALLCSV (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CA.ACCOUNT_ID AS "Customer ID",
                  CA.RESIDENT_ID AS "Resident ID",
                  CA.FIRST_NAME AS "Name",
                  CA.BIRTH_PLACE AS "Birthplace",
                  CA.BIRTH_DATE AS "Birthdate",
                  (CASE CA.NATIONALITY WHEN 2 THEN 'WNA' ELSE 'WNI' END)
                     AS "Nationality",
                  (CASE CA.GENDER WHEN 2 THEN 'PEREMPUAN' ELSE 'LAKI-LAKI' END)
                     AS "Gender",
                  (CASE CA.MARITAL_STATUS
                      WHEN 2 THEN 'KAWIN'
                      ELSE 'BELUM KAWIN'
                   END)
                     AS "Marital Status",
                  CA.OCCUPATION AS "Occupation",
                  CA.VALID_UNTIL AS "Valid Until",
                  CA.MOB_NUMBER AS "Mobile Phone",
                  CA.EMAIL_ID AS "Email",
                  P.PROVINCE_NAME AS "Province",
                  C.CITY_NAME AS "Kabupaten/Kota",
                  D.DISTRICT_NAME AS "Kecamatan",
                  SD.SUB_DISTRICT_NAME AS "Kelurahan/Desa",
                  CA.RT_RW AS "RT/RW",
                  CA.ADDRESS AS "Address",
                  CA.POSTAL_CODE AS "Postal Code"
             FROM TBL_CUSTOMER_ACCOUNT CA
                  LEFT OUTER JOIN TBL_PROVINCE P
                     ON CA.PROVINCE_ID = P.PROVINCE_ID
                  LEFT OUTER JOIN TBL_CITY C
                     ON CA.CITY_ID = C.CITY_ID
                  LEFT OUTER JOIN TBL_DISTRICT D
                     ON CA.DISTRICT_ID = D.DISTRICT_ID
                  LEFT OUTER JOIN TBL_SUB_DISTRICT SD
                     ON CA.SUB_DISTRICT_ID = SD.SUB_DISTRICT_ID
         ORDER BY CA.CREATION_DATE DESC, CA.MODIFICATION_DATE DESC;
   END ACCOUNT_GETALLCSV;

   PROCEDURE ACCOUNT_CSVWITHFILTER (P_FILTER   IN     NVARCHAR2,
                                    CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT ACCOUNT_ID AS "Customer ID",
                  RESIDENT_ID AS "Resident ID",
                  FIRST_NAME AS "Name",
                  BIRTH_PLACE AS "Birthplace",
                  BIRTH_DATE AS "Birthdate",
                  (CASE NATIONALITY WHEN 2 THEN ''WNA'' ELSE ''WNI'' END)
                     AS "Nationality",
                  (CASE GENDER WHEN 2 THEN ''PEREMPUAN'' ELSE ''LAKI-LAKI'' END)
                     AS "Gender",
                  (CASE MARITAL_STATUS
                      WHEN 2 THEN ''KAWIN''
                      ELSE ''BELUM KAWIN''
                   END)
                     AS "Marital Status",
                  OCCUPATION AS "Occupation",
                  VALID_UNTIL AS "Valid Until",
                  MOB_NUMBER AS "Mobile Phone",
                  EMAIL_ID AS "Email",
                  PROVINCE_NAME AS "Province",
                  CITY_NAME AS "Kabupaten/Kota",
                  DISTRICT_NAME AS "Kecamatan",
                  SUB_DISTRICT_NAME AS "Kelurahan/Desa",
                  RT_RW AS "RT/RW",
                  ADDRESS AS "Address",
                  POSTAL_CODE AS "Postal Code"
            FROM (SELECT CA.ACCOUNT_ID,
         CA.RESIDENT_ID,
         CA.FIRST_NAME,
         CA.BIRTH_PLACE,
         CA.BIRTH_DATE,
         CA.NATIONALITY,
         CA.GENDER,
         CA.MARITAL_STATUS,
         CA.OCCUPATION,
         CA.VALID_UNTIL,
         CA.MOB_NUMBER,
         CA.EMAIL_ID,
         P.PROVINCE_NAME,
         C.CITY_NAME,
         D.DISTRICT_NAME,
         SD.SUB_DISTRICT_NAME,
         CA.RT_RW,
         CA.ADDRESS,
         CA.POSTAL_CODE
    FROM TBL_CUSTOMER_ACCOUNT CA
         LEFT OUTER JOIN TBL_PROVINCE P
            ON CA.PROVINCE_ID = P.PROVINCE_ID
         LEFT OUTER JOIN TBL_CITY C
            ON CA.CITY_ID = C.CITY_ID
         LEFT OUTER JOIN TBL_DISTRICT D
            ON CA.DISTRICT_ID = D.DISTRICT_ID
         LEFT OUTER JOIN TBL_SUB_DISTRICT SD
            ON CA.SUB_DISTRICT_ID = SD.SUB_DISTRICT_ID
         LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
            ON CA.ACCOUNT_ID = CV.ACCOUNT_ID '
         || P_FILTER
         || ' GROUP BY CA.ACCOUNT_ID,
         CA.RESIDENT_ID,
         CA.FIRST_NAME,
         CA.BIRTH_PLACE,
         CA.BIRTH_DATE,
         CA.NATIONALITY,
         CA.GENDER,
         CA.MARITAL_STATUS,
         CA.OCCUPATION,
         CA.VALID_UNTIL,
         CA.MOB_NUMBER,
         CA.EMAIL_ID,
         P.PROVINCE_NAME,
         C.CITY_NAME,
         D.DISTRICT_NAME,
         SD.SUB_DISTRICT_NAME,
         CA.RT_RW,
         CA.ADDRESS,
         CA.POSTAL_CODE,
         CA.CREATION_DATE,
         CA.MODIFICATION_DATE
         ORDER BY CA.CREATION_DATE DESC, CA.MODIFICATION_DATE DESC) ';

      OPEN CUR_OUT FOR SQLQUERY;
   END ACCOUNT_CSVWITHFILTER;



   PROCEDURE ACCOUNT_GETBYID (P_TMS_ID       IN     NUMBER,
                              P_ACCOUNT_ID   IN     NUMBER,
                              CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CA.TMS_ID,
                CA.ACCOUNT_ID,
                CA.FIRST_NAME,
                CA.LAST_NAME,
                CA.MOB_NUMBER,
                CA.EMAIL_ID,
                CA.DESCRIPTION,
                CA.ADDRESS,
                CA.ACCOUNT_BALANCE,
                CA.CREATION_DATE,
                CA.CUSTOMER_IMAGE_PATH,
                CA.IS_DOC_VERIFIED,
                CA.ACCOUNT_STATUS,
                CA.TRANSFER_STATUS,
                CA.RESIDENT_ID,
                CA.BIRTH_PLACE,
                CA.BIRTH_DATE,
                CA.RT_RW,
                CA.PROVINCE_ID,
                P.PROVINCE_NAME,
                CA.CITY_ID,
                C.CITY_NAME,
                CA.DISTRICT_ID,
                D.DISTRICT_NAME,
                SD.SUB_DISTRICT_NAME,
                CA.SUB_DISTRICT_ID,
                CA.POSTAL_CODE,
                CA.NATIONALITY,
                CA.GENDER,
                CA.MARITAL_STATUS,
                CA.OCCUPATION,
                CA.RESIDENTIDCARDIMAGE,
                CA.VALID_UNTIL,
                CA.REGISTRATION_THROUGH,
                CA.USER_PASSWORD,
                CA.MODIFIER_ID,
                CA.CREATION_DATE,
                CA.MODIFICATION_DATE
           FROM TBL_CUSTOMER_ACCOUNT CA
                LEFT OUTER JOIN TBL_PROVINCE P
                   ON CA.PROVINCE_ID = P.PROVINCE_ID
                LEFT OUTER JOIN TBL_CITY C
                   ON CA.CITY_ID = C.CITY_ID
                LEFT OUTER JOIN TBL_DISTRICT D
                   ON CA.DISTRICT_ID = D.DISTRICT_ID
                LEFT OUTER JOIN TBL_SUB_DISTRICT SD
                   ON CA.SUB_DISTRICT_ID = SD.SUB_DISTRICT_ID
          WHERE CA.TMS_ID = P_TMS_ID AND CA.ACCOUNT_ID = P_ACCOUNT_ID;
   END ACCOUNT_GETBYID;



   PROCEDURE ACCOUNT_GETBY_RESIDENTID (P_TMS_ID        IN     NUMBER,
                                       P_RESIDENT_ID   IN     NVARCHAR2,
                                       CUR_OUT            OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CA.TMS_ID,
                CA.ACCOUNT_ID,
                CA.FIRST_NAME,
                CA.LAST_NAME,
                CA.MOB_NUMBER,
                CA.EMAIL_ID,
                CA.DESCRIPTION,
                CA.ADDRESS,
                CA.ACCOUNT_BALANCE,
                CA.CREATION_DATE,
                CA.CUSTOMER_IMAGE_PATH,
                CA.IS_DOC_VERIFIED,
                CA.ACCOUNT_STATUS,
                CA.TRANSFER_STATUS,
                CA.RESIDENT_ID,
                CA.BIRTH_PLACE,
                CA.BIRTH_DATE,
                CA.RT_RW,
                CA.PROVINCE_ID,
                P.PROVINCE_NAME,
                CA.CITY_ID,
                C.CITY_NAME,
                CA.DISTRICT_ID,
                D.DISTRICT_NAME,
                SD.SUB_DISTRICT_NAME,
                CA.SUB_DISTRICT_ID,
                CA.POSTAL_CODE,
                CA.NATIONALITY,
                CA.GENDER,
                CA.MARITAL_STATUS,
                CA.OCCUPATION,
                CA.RESIDENTIDCARDIMAGE,
                CA.VALID_UNTIL,
                CA.REGISTRATION_THROUGH,
                CA.USER_PASSWORD,
                CA.MODIFIER_ID,
                CA.CREATION_DATE,
                CA.MODIFICATION_DATE
           FROM TBL_CUSTOMER_ACCOUNT CA
                LEFT OUTER JOIN TBL_PROVINCE P
                   ON CA.PROVINCE_ID = P.PROVINCE_ID
                LEFT OUTER JOIN TBL_CITY C
                   ON CA.CITY_ID = C.CITY_ID
                LEFT OUTER JOIN TBL_DISTRICT D
                   ON CA.DISTRICT_ID = D.DISTRICT_ID
                LEFT OUTER JOIN TBL_SUB_DISTRICT SD
                   ON CA.SUB_DISTRICT_ID = SD.SUB_DISTRICT_ID
          WHERE CA.TMS_ID = P_TMS_ID AND CA.RESIDENT_ID = P_RESIDENT_ID;
   END ACCOUNT_GETBY_RESIDENTID;



   PROCEDURE CUSTOMER_ACCOUNT_VALIDATE (P_MOB_NUMBER    IN     NVARCHAR2,
                                        P_RESIDENT_ID   IN     NVARCHAR2,
                                        P_EMAIL_ID      IN     NVARCHAR2,
                                        CUR_OUT            OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CA.TMS_ID,
                CA.ACCOUNT_ID,
                CA.FIRST_NAME,
                CA.LAST_NAME,
                CA.MOB_NUMBER,
                CA.EMAIL_ID,
                CA.DESCRIPTION,
                CA.ADDRESS,
                CA.ACCOUNT_BALANCE,
                CA.CREATION_DATE,
                CA.CUSTOMER_IMAGE_PATH,
                CA.IS_DOC_VERIFIED,
                CA.ACCOUNT_STATUS,
                CA.TRANSFER_STATUS,
                CA.RESIDENT_ID,
                CA.BIRTH_PLACE,
                CA.BIRTH_DATE,
                CA.RT_RW,
                CA.PROVINCE_ID,
                P.PROVINCE_NAME,
                CA.CITY_ID,
                C.CITY_NAME,
                CA.DISTRICT_ID,
                D.DISTRICT_NAME,
                SD.SUB_DISTRICT_NAME,
                CA.SUB_DISTRICT_ID,
                CA.POSTAL_CODE,
                CA.NATIONALITY,
                CA.GENDER,
                CA.MARITAL_STATUS,
                CA.OCCUPATION,
                CA.RESIDENTIDCARDIMAGE,
                CA.VALID_UNTIL,
                CA.REGISTRATION_THROUGH,
                CA.USER_PASSWORD,
                CA.MODIFIER_ID,
                CA.CREATION_DATE,
                CA.MODIFICATION_DATE
           FROM TBL_CUSTOMER_ACCOUNT CA
                LEFT OUTER JOIN TBL_PROVINCE P
                   ON CA.PROVINCE_ID = P.PROVINCE_ID
                LEFT OUTER JOIN TBL_CITY C
                   ON CA.CITY_ID = C.CITY_ID
                LEFT OUTER JOIN TBL_DISTRICT D
                   ON CA.DISTRICT_ID = D.DISTRICT_ID
                LEFT OUTER JOIN TBL_SUB_DISTRICT SD
                   ON CA.SUB_DISTRICT_ID = SD.SUB_DISTRICT_ID
          WHERE     MOB_NUMBER = P_MOB_NUMBER
                AND RESIDENT_ID = P_RESIDENT_ID
                AND EMAIL_ID = P_EMAIL_ID;
   END CUSTOMER_ACCOUNT_VALIDATE;



   PROCEDURE ACCOUNT_BALANCEUPDATE (P_TMS_ID       IN NUMBER,
                                    P_ACCOUNT_ID   IN NUMBER,
                                    P_AMOUNT       IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_CUSTOMER_ACCOUNT
         SET ACCOUNT_BALANCE = ACCOUNT_BALANCE + P_AMOUNT
       WHERE TMS_ID = P_TMS_ID AND ACCOUNT_ID = P_ACCOUNT_ID;
   END ACCOUNT_BALANCEUPDATE;



   PROCEDURE ACCOUNT_LATEST_GETALL (P_LAST_UPDATE_TIME   IN     DATE,
                                    CUR_OUT                 OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CA.TMS_ID,
                  CA.ACCOUNT_ID,
                  CA.FIRST_NAME,
                  CA.LAST_NAME,
                  CA.MOB_NUMBER,
                  CA.EMAIL_ID,
                  CA.DESCRIPTION,
                  CA.ADDRESS,
                  CA.ACCOUNT_BALANCE,
                  CA.CREATION_DATE,
                  CA.CUSTOMER_IMAGE_PATH,
                  CA.IS_DOC_VERIFIED,
                  CA.ACCOUNT_STATUS,
                  CA.TRANSFER_STATUS,
                  CA.RESIDENT_ID,
                  CA.BIRTH_PLACE,
                  CA.BIRTH_DATE,
                  CA.RT_RW,
                  CA.PROVINCE_ID,
                  P.PROVINCE_NAME,
                  CA.CITY_ID,
                  C.CITY_NAME,
                  CA.DISTRICT_ID,
                  D.DISTRICT_NAME,
                  SD.SUB_DISTRICT_NAME,
                  CA.SUB_DISTRICT_ID,
                  CA.POSTAL_CODE,
                  CA.NATIONALITY,
                  CA.GENDER,
                  CA.MARITAL_STATUS,
                  CA.OCCUPATION,
                  CA.RESIDENTIDCARDIMAGE,
                  CA.VALID_UNTIL,
                  CA.REGISTRATION_THROUGH,
                  CA.USER_PASSWORD,
                  CA.MODIFIER_ID,
                  CA.CREATION_DATE,
                  CA.MODIFICATION_DATE
             FROM TBL_CUSTOMER_ACCOUNT CA
                  LEFT OUTER JOIN TBL_PROVINCE P
                     ON CA.PROVINCE_ID = P.PROVINCE_ID
                  LEFT OUTER JOIN TBL_CITY C
                     ON CA.CITY_ID = C.CITY_ID
                  LEFT OUTER JOIN TBL_DISTRICT D
                     ON CA.DISTRICT_ID = D.DISTRICT_ID
                  LEFT OUTER JOIN TBL_SUB_DISTRICT SD
                     ON CA.SUB_DISTRICT_ID = SD.SUB_DISTRICT_ID
            WHERE CA.CREATION_DATE > P_LAST_UPDATE_TIME
         ORDER BY CA.ACCOUNT_ID;
   END ACCOUNT_LATEST_GETALL;



   PROCEDURE ACCOUNT_GETALL_LAZYLOAD (P_PAGE_INDEX   IN     NUMBER,
                                      P_PAGE_SIZE    IN     NUMBER,
                                      CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_CUSTOMER_HISTORY
              AS (  SELECT ROW_NUMBER () OVER (ORDER BY CREATION_DATE DESC)
                              AS ROWNUMBER,
                           CA.ACCOUNT_ID,
                           CA.FIRST_NAME,
                           CA.MOB_NUMBER,
                           CA.EMAIL_ID,
                           CA.ADDRESS,
                           CA.CUSTOMER_IMAGE_PATH,
                           CA.RESIDENT_ID,
                           CA.RESIDENTIDCARDIMAGE
                      FROM TBL_CUSTOMER_ACCOUNT CA
                  ORDER BY CREATION_DATE DESC)
           SELECT ROWNUMBER,
                  ACCOUNT_ID,
                  FIRST_NAME,
                  MOB_NUMBER,
                  EMAIL_ID,
                  ADDRESS,
                  CUSTOMER_IMAGE_PATH,
                  RESIDENT_ID,
                  RESIDENTIDCARDIMAGE,
                  (SELECT COUNT (1) FROM CTE_CUSTOMER_HISTORY) AS RECORDCOUNT
             FROM CTE_CUSTOMER_HISTORY
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END ACCOUNT_GETALL_LAZYLOAD;



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
      P_QUEUE_STATUS           IN     NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_CUSTOMER_VEHICLE (TMS_ID,
                                        ENTRY_ID,
                                        ACCOUNT_ID,
                                        VEH_REG_NO,
                                        TAG_ID,
                                        VEHICLE_CLASS_ID,
                                        CREATION_DATE,
                                        TRANSFER_STATUS,
                                        VEHICLE_RC_NO,
                                        OWNER_NAME,
                                        OWNER_ADDRESS,
                                        BRAND,
                                        VEHICLE_TYPE,
                                        VEHICLE_CATEGORY,
                                        MODEL_NO,
                                        MANUFACTURING_YEAR,
                                        CYCLINDER_CAPACITY,
                                        FRAME_NUMBER,
                                        ENGINE_NUMBER,
                                        VEHICLE_COLOR,
                                        FUEL_TYPE,
                                        LICENCE_PLATE_COLOR,
                                        REGISTRATION_YEAR,
                                        VEHICLE_OWNERSHIP_NO,
                                        LOCATION_CODE,
                                        REG_QUEUE_NO,
                                        VEHICLEIMAGE_FRONT,
                                        VEHICLEIMAGE_REAR,
                                        VEHICLEIMAGE_RIGHT,
                                        VEHICLEIMAGE_LEFT,
                                        VEHICLE_RC_NO_PATH,
                                        EXCEPTION_FLAG,
                                        STATUS,
                                        VALID_UNTIL,
                                        TID_FRONT,
                                        TID_REAR,
                                        ACCOUNT_BALANCE,
                                        REGISTRATION_THROUGH,
                                        IS_DOC_VERIFIED,
                                        QUEUE_STATUS)
           VALUES (P_TMS_ID,
                   CUSTOMER_VEHICLE_SEQ.NEXTVAL,
                   P_ACCOUNT_ID,
                   P_VEH_REG_NO,
                   P_TAG_ID,
                   P_VEHICLE_CLASS_ID,
                   P_CREATION_DATE,
                   P_TRANSFER_STATUS,
                   P_VEHICLE_RC_NO,
                   P_OWNER_NAME,
                   P_OWNER_ADDRESS,
                   P_BRAND,
                   P_VEHICLE_TYPE,
                   P_VEHICLE_CATEGORY,
                   P_MODEL_NO,
                   P_MANUFACTURING_YEAR,
                   P_CYCLINDER_CAPACITY,
                   P_FRAME_NUMBER,
                   P_ENGINE_NUMBER,
                   P_VEHICLE_COLOR,
                   P_FUEL_TYPE,
                   P_LICENCE_PLATE_COLOR,
                   P_REGISTRATION_YEAR,
                   P_VEHICLE_OWNERSHIP_NO,
                   P_LOCATION_CODE,
                   P_REG_QUEUE_NO,
                   P_VEHICLEIMAGE_FRONT,
                   P_VEHICLEIMAGE_REAR,
                   P_VEHICLEIMAGE_RIGHT,
                   P_VEHICLEIMAGE_LEFT,
                   P_VEHICLE_RC_NO_PATH,
                   P_EXCEPTION_FLAG,
                   P_STATUS,
                   P_VALID_UNTIL,
                   P_TID_FRONT,
                   P_TID_REAR,
                   P_ACCOUNT_BALANCE,
                   P_REGISTRATION_THROUGH,
                   P_IS_DOC_VERIFIED,
                   P_QUEUE_STATUS);



      P_ENTRY_ID := CUSTOMER_VEHICLE_SEQ.CURRVAL;
   END CUSTOMER_VEHICLE_INSERT;



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
      P_QUEUE_STATUS           IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_CUSTOMER_VEHICLE
         SET TMS_ID = P_TMS_ID,
             ENTRY_ID = P_ENTRY_ID,
             ACCOUNT_ID = P_ACCOUNT_ID,
             VEH_REG_NO = P_VEH_REG_NO,
             TAG_ID = P_TAG_ID,
             VEHICLE_CLASS_ID = P_VEHICLE_CLASS_ID,
             MODIFICATION_DATE = P_MODIFICATION_DATE,
             MODIFIED_BY = P_MODIFIED_BY,
             TRANSFER_STATUS = P_TRANSFER_STATUS,
             VEHICLE_RC_NO = P_VEHICLE_RC_NO,
             OWNER_NAME = P_OWNER_NAME,
             OWNER_ADDRESS = P_OWNER_ADDRESS,
             BRAND = P_BRAND,
             VEHICLE_TYPE = P_VEHICLE_TYPE,
             VEHICLE_CATEGORY = P_VEHICLE_CATEGORY,
             MODEL_NO = P_MODEL_NO,
             MANUFACTURING_YEAR = P_MANUFACTURING_YEAR,
             CYCLINDER_CAPACITY = P_CYCLINDER_CAPACITY,
             FRAME_NUMBER = P_FRAME_NUMBER,
             ENGINE_NUMBER = P_ENGINE_NUMBER,
             VEHICLE_COLOR = P_VEHICLE_COLOR,
             FUEL_TYPE = P_FUEL_TYPE,
             LICENCE_PLATE_COLOR = P_LICENCE_PLATE_COLOR,
             REGISTRATION_YEAR = P_REGISTRATION_YEAR,
             VEHICLE_OWNERSHIP_NO = P_VEHICLE_OWNERSHIP_NO,
             LOCATION_CODE = P_LOCATION_CODE,
             REG_QUEUE_NO = P_REG_QUEUE_NO,
             VEHICLEIMAGE_FRONT = P_VEHICLEIMAGE_FRONT,
             VEHICLEIMAGE_REAR = P_VEHICLEIMAGE_REAR,
             VEHICLEIMAGE_RIGHT = P_VEHICLEIMAGE_RIGHT,
             VEHICLEIMAGE_LEFT = P_VEHICLEIMAGE_LEFT,
             VEHICLE_RC_NO_PATH = P_VEHICLE_RC_NO_PATH,
             EXCEPTION_FLAG = P_EXCEPTION_FLAG,
             STATUS = P_STATUS,
             VALID_UNTIL = P_VALID_UNTIL,
             TID_FRONT = P_TID_FRONT,
             TID_REAR = P_TID_REAR,
             ACCOUNT_BALANCE = P_ACCOUNT_BALANCE,
             IS_DOC_VERIFIED = P_IS_DOC_VERIFIED,
             QUEUE_STATUS = P_QUEUE_STATUS
       WHERE TMS_ID = P_TMS_ID AND ENTRY_ID = P_ENTRY_ID;
   END CUSTOMER_VEHICLE_UPDATE;



   PROCEDURE VEHICLE_BALANCE_UPDATE (P_ENTRY_ID          IN     NUMBER,
                                     P_VEH_REG_NO        IN     NVARCHAR2,
                                     P_AMOUNT            IN     NUMBER,
                                     P_ACCOUNT_BALANCE      OUT NUMBER)
   AS
   BEGIN
      P_ACCOUNT_BALANCE := 0;



      UPDATE TBL_CUSTOMER_VEHICLE
         SET ACCOUNT_BALANCE = ACCOUNT_BALANCE + P_AMOUNT
       WHERE ENTRY_ID = P_ENTRY_ID AND VEH_REG_NO = P_VEH_REG_NO;



      SELECT ACCOUNT_BALANCE
        INTO P_ACCOUNT_BALANCE
        FROM TBL_CUSTOMER_VEHICLE
       WHERE ENTRY_ID = P_ENTRY_ID AND VEH_REG_NO = P_VEH_REG_NO;
   END VEHICLE_BALANCE_UPDATE;



   PROCEDURE CUSTOMER_VEHICLE_GETBYID (P_VEHICLE_CLASS_ID   IN     NUMBER,
                                       P_TMS_ID             IN     NUMBER,
                                       P_ACCOUNT_ID         IN     NUMBER,
                                       CUR_OUT                 OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                CV.LICENCE_PLATE_COLOR,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS
           FROM    TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN
                   TBL_VEHICLE_CLASS VC
                ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
          WHERE CV.TMS_ID = P_TMS_ID AND CV.ACCOUNT_ID = P_ACCOUNT_ID;
   END CUSTOMER_VEHICLE_GETBYID;



   PROCEDURE CUSTOMERVEHICLE_GETBYID (P_ENTRY_ID   IN     NUMBER,
                                      CUR_OUT         OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER,
                CA.ADDRESS
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE CV.ENTRY_ID = P_ENTRY_ID;
   END CUSTOMERVEHICLE_GETBYID;



   PROCEDURE CUSTOMERVEHICLE_GETBYACCOUNTID (P_ACCOUNT_ID   IN     NUMBER,
                                             CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER,
                CA.ADDRESS
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE CV.ACCOUNT_ID = P_ACCOUNT_ID;
   END CUSTOMERVEHICLE_GETBYACCOUNTID;


   PROCEDURE VEHICLE_GETALLCSV (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CA.ACCOUNT_ID AS "Customer ID",
                  CA.FIRST_NAME AS "Name",
                  CA.MOB_NUMBER AS "Mobile Phone",
                  CA.RESIDENT_ID AS "Resident ID",
                  CA.ADDRESS AS "Address",
                  CA.EMAIL_ID AS "Email",
                  CV.ENTRY_ID AS "Vehicle ID",
                  CV.VEH_REG_NO AS "Registration Num",
                  CV.OWNER_ADDRESS AS "Owner Address",
                  CV.VEHICLE_TYPE AS "Type",
                  CV.MODEL_NO AS "Model",
                  CV.CYCLINDER_CAPACITY AS "Cylinder Capacity",
                  CV.ENGINE_NUMBER AS "Engine Num",
                  (CASE CV.FUEL_TYPE
                      WHEN 1 THEN 'GASOLINE'
                      WHEN 2 THEN 'DIESEL'
                      WHEN 3 THEN 'ELECTRIC'
                      ELSE 'Unknown'
                   END)
                     AS "Fuel Type",
                  CV.REGISTRATION_YEAR AS "Registration Year",
                  CV.LOCATION_CODE AS "Location Code",
                  CV.VALID_UNTIL AS "Valid Until",
                  CV.VEHICLE_RC_NO AS "Certificate Num",
                  CV.OWNER_NAME AS "Owner Name",
                  CV.BRAND AS "Brand",
                  CV.VEHICLE_CATEGORY AS "Category",
                  CV.MANUFACTURING_YEAR AS "Manufacturing Year",
                  CV.FRAME_NUMBER AS "Frame Num",
                  CV.VEHICLE_COLOR AS "Color",
                  (CASE CV.LICENCE_PLATE_COLOR
                      WHEN 1 THEN 'BLACK'
                      WHEN 2 THEN 'BLUE'
                      WHEN 3 THEN 'GREEN'
                      WHEN 4 THEN 'RED'
                      WHEN 5 THEN 'WHITE'
                      WHEN 6 THEN 'YELLOW'
                      ELSE 'Unknown'
                   END)
                     AS "Licence Plate Color",
                  CV.VEHICLE_OWNERSHIP_NO AS "Ownership Document Num",
                  CV.REG_QUEUE_NO AS "Registration Queue Num",
                  (CASE CV.EXCEPTION_FLAG
                      WHEN 1 THEN 'CHARGED'
                      WHEN 2 THEN 'NOT CHARGED'
                      WHEN 3 THEN 'BLACK LISTED'
                      ELSE 'Unknown'
                   END)
                     AS "Exception Flag",
                  CV.TID_FRONT AS "TID Front",
                  CV.TID_REAR AS "TID Rear",
                  CV.TAG_ID AS "EPC",
                  (CASE CV.QUEUE_STATUS
                      WHEN 1 THEN 'OPEN'
                      WHEN 2 THEN 'POSTPONED'
                      WHEN 3 THEN 'PROCESSED'
                      ELSE 'Unknown'
                   END)
                     AS "Status",
                  VC.VEHICLE_CLASS_NAME AS "Vehicle Class",
                  CV.ACCOUNT_BALANCE AS "Account Balance"
             FROM TBL_CUSTOMER_VEHICLE CV
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
         ORDER BY CV.CREATION_DATE DESC, CV.MODIFICATION_DATE DESC;
   END VEHICLE_GETALLCSV;

   PROCEDURE VEHICLE_CSVWITHFILTER (P_FILTER   IN     NVARCHAR2,
                                    CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT CA.ACCOUNT_ID AS "Customer ID",
                  CA.FIRST_NAME AS "Name",
                  CA.MOB_NUMBER AS "Mobile Phone",
                  CA.RESIDENT_ID AS "Resident ID",
                  CA.ADDRESS AS "Address",
                  CA.EMAIL_ID AS "Email",
                  CV.ENTRY_ID AS "Vehicle ID",
                  CV.VEH_REG_NO AS "Registration Num",
                  CV.OWNER_ADDRESS AS "Owner Address",
                  CV.VEHICLE_TYPE AS "Type",
                  CV.MODEL_NO AS "Model",
                  CV.CYCLINDER_CAPACITY AS "Cylinder Capacity",
                  CV.ENGINE_NUMBER AS "Engine Num",
                  (CASE CV.FUEL_TYPE
                       WHEN 1 THEN ''GASOLINE''
                      WHEN 2 THEN ''DIESEL''
                      WHEN 3 THEN ''ELECTRIC''
                      ELSE ''Unknown''
                   END)
                     AS "Fuel Type",
                  CV.REGISTRATION_YEAR AS "Registration Year",
                  CV.LOCATION_CODE AS "Location Code",
                  CV.VALID_UNTIL AS "Valid Until",
                  CV.VEHICLE_RC_NO AS "Certificate Num",
                  CV.OWNER_NAME AS "Owner Name",
                  CV.BRAND AS "Brand",
                  CV.VEHICLE_CATEGORY AS "Category",
                  CV.MANUFACTURING_YEAR AS "Manufacturing Year",
                  CV.FRAME_NUMBER AS "Frame Num",
                  CV.VEHICLE_COLOR AS "Color",
                  (CASE CV.LICENCE_PLATE_COLOR
                       WHEN 1 THEN ''BLACK''
                      WHEN 2 THEN ''BLUE''
                      WHEN 3 THEN ''GREEN''
                      WHEN 4 THEN ''RED''
                      WHEN 5 THEN ''WHITE''
                      WHEN 6 THEN ''YELLOW''
                      ELSE ''Unknown''
                   END)
                     AS "Licence Plate Color",
                  CV.VEHICLE_OWNERSHIP_NO AS "Ownership Document Num",
                  CV.REG_QUEUE_NO AS "Registration Queue Num",
                  (CASE CV.EXCEPTION_FLAG
                      WHEN 1 THEN ''CHARGED''
                    WHEN 2 THEN ''NOT CHARGED''
                    WHEN 3 THEN ''BLACK LISTED''
                      ELSE ''Unknown''
                   END)
                     AS "Exception Flag",
                  CV.TID_FRONT AS "TID Front",
                  CV.TID_REAR AS "TID Rear",
                  CV.TAG_ID AS "EPC",
                  (CASE CV.QUEUE_STATUS
                       WHEN 1 THEN ''OPEN''
                    WHEN 2 THEN ''POSTPONED''
                    WHEN 3 THEN ''PROCESSED''
                      ELSE ''Unknown''
                   END)
                     AS "Status",
                  VC.VEHICLE_CLASS_NAME AS "Vehicle Class",
                  CV.ACCOUNT_BALANCE AS "Account Balance"
    FROM TBL_CUSTOMER_VEHICLE CV
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID '
         || P_FILTER
         || ' ORDER BY CV.CREATION_DATE DESC, CV.MODIFICATION_DATE DESC ';

      OPEN CUR_OUT FOR SQLQUERY;
   END VEHICLE_CSVWITHFILTER;

   PROCEDURE CV_GET_BY_TRANCTPENTRYID (P_TRAN_CT_EN_ID   IN     NUMBER,
                                       CUR_OUT              OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                CV.LICENCE_PLATE_COLOR,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS
           FROM    TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN
                   TBL_VEHICLE_CLASS VC
                ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
          WHERE CV.TAG_ID = (SELECT OBJECT_ID
                               FROM TBL_CROSSTALK_PACKET CT
                              WHERE CT.ENTRY_ID = P_TRAN_CT_EN_ID);
   END CV_GET_BY_TRANCTPENTRYID;



   PROCEDURE CUSTOMERVEHICLE_GETBYACCOUNTID (P_TMS_ID       IN     NUMBER,
                                             P_ACCOUNT_ID   IN     NUMBER,
                                             CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE CV.TMS_ID = P_TMS_ID AND CV.ACCOUNT_ID = P_ACCOUNT_ID;
   END CUSTOMERVEHICLE_GETBYACCOUNTID;



   PROCEDURE CUSTOMER_VEHICLE_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CV.TMS_ID,
                  CV.ENTRY_ID,
                  CV.ACCOUNT_ID,
                  CV.VEH_REG_NO,
                  CV.TAG_ID,
                  CV.VEHICLE_CLASS_ID,
                  VC.VEHICLE_CLASS_NAME,
                  CV.CREATION_DATE,
                  CV.MODIFICATION_DATE,
                  CV.MODIFIED_BY,
                  CV.TRANSFER_STATUS,
                  CV.VEHICLE_RC_NO,
                  CV.OWNER_NAME,
                  CV.OWNER_ADDRESS,
                  CV.BRAND,
                  CV.VEHICLE_TYPE,
                  CV.VEHICLE_CATEGORY,
                  CV.MODEL_NO,
                  CV.MANUFACTURING_YEAR,
                  CV.CYCLINDER_CAPACITY,
                  CV.FRAME_NUMBER,
                  CV.ENGINE_NUMBER,
                  CV.VEHICLE_COLOR,
                  CV.FUEL_TYPE,
                  (CASE CV.FUEL_TYPE
                      WHEN 1 THEN 'GASOLINE'
                      WHEN 2 THEN 'DIESEL'
                      WHEN 3 THEN 'ELECTRIC'
                      ELSE 'Unknown'
                   END)
                     FUEL_TYPE_NAME,
                  CV.LICENCE_PLATE_COLOR,
                  (CASE CV.LICENCE_PLATE_COLOR
                      WHEN 1 THEN 'BLACK'
                      WHEN 2 THEN 'BLUE'
                      WHEN 3 THEN 'GREEN'
                      WHEN 4 THEN 'RED'
                      WHEN 5 THEN 'WHITE'
                      WHEN 6 THEN 'YELLOW'
                      ELSE 'Unknown'
                   END)
                     LICENCE_PLATE_COLOR_NAME,
                  CV.REGISTRATION_YEAR,
                  CV.VEHICLE_OWNERSHIP_NO,
                  CV.LOCATION_CODE,
                  CV.REG_QUEUE_NO,
                  CV.VEHICLEIMAGE_FRONT,
                  CV.VEHICLEIMAGE_REAR,
                  CV.VEHICLEIMAGE_RIGHT,
                  CV.VEHICLEIMAGE_LEFT,
                  CV.VEHICLE_RC_NO_PATH,
                  CV.EXCEPTION_FLAG,
                  (CASE CV.EXCEPTION_FLAG
                      WHEN 1 THEN 'CHARGED'
                      WHEN 2 THEN 'NOT CHARGED'
                      WHEN 3 THEN 'BLACK LISTED'
                      ELSE 'Unknown'
                   END)
                     EXCEPTION_FLAG_NAME,
                  CV.STATUS,
                  CV.VALID_UNTIL,
                  CV.TID_FRONT,
                  CV.TID_REAR,
                  CV.ACCOUNT_BALANCE,
                  CV.REGISTRATION_THROUGH,
                  CV.IS_DOC_VERIFIED,
                  CV.QUEUE_STATUS,
                  (CASE CV.QUEUE_STATUS
                      WHEN 1 THEN 'OPEN'
                      WHEN 2 THEN 'POSTPONED'
                      WHEN 3 THEN 'PROCESSED'
                      ELSE 'Unknown'
                   END)
                     QUEUE_STATUS_NAME,
                  CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                  CA.RESIDENT_ID,
                  CA.EMAIL_ID,
                  CA.MOB_NUMBER
             FROM TBL_CUSTOMER_VEHICLE CV
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
         ORDER BY CV.CREATION_DATE DESC, CV.MODIFICATION_DATE DESC;
   END CUSTOMER_VEHICLE_GETALL;



   PROCEDURE CUSTOMER_VEHICLE_LAZYLOAD (P_PAGE_INDEX   IN     NUMBER,
                                        P_PAGE_SIZE    IN     NUMBER,
                                        CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_VEHICLE_HISTORY
              AS (  SELECT ROW_NUMBER ()
                           OVER (
                              ORDER BY
                                 CV.CREATION_DATE DESC,
                                 CV.MODIFICATION_DATE DESC)
                              AS ROWNUMBER,
                           CV.TMS_ID,
                           CV.ENTRY_ID,
                           CV.ACCOUNT_ID,
                           CV.VEH_REG_NO,
                           CV.TAG_ID,
                           CV.VEHICLE_CLASS_ID,
                           VC.VEHICLE_CLASS_NAME,
                           CV.CREATION_DATE,
                           CV.MODIFICATION_DATE,
                           CV.MODIFIED_BY,
                           CV.TRANSFER_STATUS,
                           CV.VEHICLE_RC_NO,
                           CV.OWNER_NAME,
                           CV.OWNER_ADDRESS,
                           CV.BRAND,
                           CV.VEHICLE_TYPE,
                           CV.VEHICLE_CATEGORY,
                           CV.MODEL_NO,
                           CV.MANUFACTURING_YEAR,
                           CV.CYCLINDER_CAPACITY,
                           CV.FRAME_NUMBER,
                           CV.ENGINE_NUMBER,
                           CV.VEHICLE_COLOR,
                           CV.FUEL_TYPE,
                           (CASE CV.FUEL_TYPE
                               WHEN 1 THEN 'GASOLINE'
                               WHEN 2 THEN 'DIESEL'
                               WHEN 3 THEN 'ELECTRIC'
                               ELSE 'Unknown'
                            END)
                              FUEL_TYPE_NAME,
                           CV.LICENCE_PLATE_COLOR,
                           (CASE CV.LICENCE_PLATE_COLOR
                               WHEN 1 THEN 'BLACK'
                               WHEN 2 THEN 'BLUE'
                               WHEN 3 THEN 'GREEN'
                               WHEN 4 THEN 'RED'
                               WHEN 5 THEN 'WHITE'
                               WHEN 6 THEN 'YELLOW'
                               ELSE 'Unknown'
                            END)
                              LICENCE_PLATE_COLOR_NAME,
                           CV.REGISTRATION_YEAR,
                           CV.VEHICLE_OWNERSHIP_NO,
                           CV.LOCATION_CODE,
                           CV.REG_QUEUE_NO,
                           CV.VEHICLEIMAGE_FRONT,
                           CV.VEHICLEIMAGE_REAR,
                           CV.VEHICLEIMAGE_RIGHT,
                           CV.VEHICLEIMAGE_LEFT,
                           CV.VEHICLE_RC_NO_PATH,
                           CV.EXCEPTION_FLAG,
                           (CASE CV.EXCEPTION_FLAG
                               WHEN 1 THEN 'CHARGED'
                               WHEN 2 THEN 'NOT CHARGED'
                               WHEN 3 THEN 'BLACK LISTED'
                               ELSE 'Unknown'
                            END)
                              EXCEPTION_FLAG_NAME,
                           CV.STATUS,
                           CV.VALID_UNTIL,
                           CV.TID_FRONT,
                           CV.TID_REAR,
                           NVL (CV.ACCOUNT_BALANCE, 0) ACCOUNT_BALANCE,
                           CV.REGISTRATION_THROUGH,
                           CV.IS_DOC_VERIFIED,
                           CV.QUEUE_STATUS,
                           (CASE CV.QUEUE_STATUS
                               WHEN 1 THEN 'OPEN'
                               WHEN 2 THEN 'POSTPONED'
                               WHEN 3 THEN 'PROCESSED'
                               ELSE 'Unknown'
                            END)
                              QUEUE_STATUS_NAME,
                           CA.FIRST_NAME || ' ' || CA.LAST_NAME
                              AS CUSTOMER_NAME,
                           CA.RESIDENT_ID,
                           CA.EMAIL_ID,
                           CA.MOB_NUMBER
                      FROM TBL_CUSTOMER_VEHICLE CV
                           LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                              ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                           LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                              ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
                  ORDER BY CV.CREATION_DATE DESC, CV.MODIFICATION_DATE DESC)
           SELECT ROWNUMBER,
                  TMS_ID,
                  ENTRY_ID,
                  ACCOUNT_ID,
                  VEH_REG_NO,
                  TAG_ID,
                  VEHICLE_CLASS_ID,
                  VEHICLE_CLASS_NAME,
                  CREATION_DATE,
                  MODIFICATION_DATE,
                  MODIFIED_BY,
                  TRANSFER_STATUS,
                  VEHICLE_RC_NO,
                  OWNER_NAME,
                  OWNER_ADDRESS,
                  BRAND,
                  VEHICLE_TYPE,
                  VEHICLE_CATEGORY,
                  MODEL_NO,
                  MANUFACTURING_YEAR,
                  CYCLINDER_CAPACITY,
                  FRAME_NUMBER,
                  ENGINE_NUMBER,
                  VEHICLE_COLOR,
                  FUEL_TYPE,
                  FUEL_TYPE_NAME,
                  LICENCE_PLATE_COLOR,
                  LICENCE_PLATE_COLOR_NAME,
                  REGISTRATION_YEAR,
                  VEHICLE_OWNERSHIP_NO,
                  LOCATION_CODE,
                  REG_QUEUE_NO,
                  VEHICLEIMAGE_FRONT,
                  VEHICLEIMAGE_REAR,
                  VEHICLEIMAGE_RIGHT,
                  VEHICLEIMAGE_LEFT,
                  VEHICLE_RC_NO_PATH,
                  EXCEPTION_FLAG,
                  EXCEPTION_FLAG_NAME,
                  STATUS,
                  VALID_UNTIL,
                  TID_FRONT,
                  TID_REAR,
                  ACCOUNT_BALANCE,
                  REGISTRATION_THROUGH,
                  IS_DOC_VERIFIED,
                  QUEUE_STATUS,
                  QUEUE_STATUS_NAME,
                  CUSTOMER_NAME,
                  RESIDENT_ID,
                  EMAIL_ID,
                  MOB_NUMBER,
                  (SELECT COUNT (1) FROM CTE_VEHICLE_HISTORY) AS RECORDCOUNT
             FROM CTE_VEHICLE_HISTORY
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END CUSTOMER_VEHICLE_LAZYLOAD;



   PROCEDURE CUSTOMER_VEHICLE_GETBYTAGID (P_TAG_ID   IN     NVARCHAR2,
                                          CUR_OUT       OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE CV.TAG_ID = P_TAG_ID;
   END CUSTOMER_VEHICLE_GETBYTAGID;



   PROCEDURE CUSTOMERVEHICLE_GETBYVEHREGNO (P_VEH_REG_NO   IN     NVARCHAR2,
                                            CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE CV.VEH_REG_NO = P_VEH_REG_NO;
   END CUSTOMERVEHICLE_GETBYVEHREGNO;



   PROCEDURE VALIDATE_VEHICLE_DETAILS (P_VEH_REG_NO      IN     NVARCHAR2,
                                       P_RESIDENT_ID     IN     NVARCHAR2,
                                       P_VEHICLE_RC_NO   IN     NVARCHAR2,
                                       CUR_OUT              OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE     LOWER (CV.VEH_REG_NO) = LOWER (P_VEH_REG_NO)
                AND LOWER (CV.VEHICLE_RC_NO) = LOWER (P_VEHICLE_RC_NO)
                AND LOWER (CA.RESIDENT_ID) = LOWER (P_RESIDENT_ID);
   END VALIDATE_VEHICLE_DETAILS;

   PROCEDURE VALIDATE_VEHICLE_DETAILS_M (P_VEH_REG_NO      IN     NVARCHAR2,
                                         P_VEHICLE_RC_NO   IN     NVARCHAR2,
                                         CUR_OUT              OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE     LOWER (CV.VEH_REG_NO) = LOWER (P_VEH_REG_NO)
                AND LOWER (CV.VEHICLE_RC_NO) = LOWER (P_VEHICLE_RC_NO);
   END VALIDATE_VEHICLE_DETAILS_M;

   PROCEDURE VEHICLE_DETAILS_RESIDENTID (P_RESIDENT_ID   IN     NVARCHAR2,
                                         CUR_OUT            OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE LOWER (CA.RESIDENT_ID) = LOWER (P_RESIDENT_ID);
   END VEHICLE_DETAILS_RESIDENTID;

   PROCEDURE VEHICLE_DETAILS_TID (P_TID           IN     NVARCHAR2,
                                  P_RESIDENT_ID   IN     NVARCHAR2,
                                  CUR_OUT            OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE (   LOWER (CV.TID_FRONT) = LOWER (P_TID)
                 OR LOWER (CV.TID_REAR) = LOWER (P_TID));
   END VEHICLE_DETAILS_TID;

   PROCEDURE VALIDATE_TID (P_TID IN NVARCHAR2, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT CV.TMS_ID,
                CV.ENTRY_ID,
                CV.ACCOUNT_ID,
                CV.VEH_REG_NO,
                CV.TAG_ID,
                CV.VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME,
                CV.CREATION_DATE,
                CV.MODIFICATION_DATE,
                CV.MODIFIED_BY,
                CV.TRANSFER_STATUS,
                CV.VEHICLE_RC_NO,
                CV.OWNER_NAME,
                CV.OWNER_ADDRESS,
                CV.BRAND,
                CV.VEHICLE_TYPE,
                CV.VEHICLE_CATEGORY,
                CV.MODEL_NO,
                CV.MANUFACTURING_YEAR,
                CV.CYCLINDER_CAPACITY,
                CV.FRAME_NUMBER,
                CV.ENGINE_NUMBER,
                CV.VEHICLE_COLOR,
                CV.FUEL_TYPE,
                (CASE CV.FUEL_TYPE
                    WHEN 1 THEN 'GASOLINE'
                    WHEN 2 THEN 'DIESEL'
                    WHEN 3 THEN 'ELECTRIC'
                    ELSE 'Unknown'
                 END)
                   FUEL_TYPE_NAME,
                CV.LICENCE_PLATE_COLOR,
                (CASE CV.LICENCE_PLATE_COLOR
                    WHEN 1 THEN 'BLACK'
                    WHEN 2 THEN 'BLUE'
                    WHEN 3 THEN 'GREEN'
                    WHEN 4 THEN 'RED'
                    WHEN 5 THEN 'WHITE'
                    WHEN 6 THEN 'YELLOW'
                    ELSE 'Unknown'
                 END)
                   LICENCE_PLATE_COLOR_NAME,
                CV.REGISTRATION_YEAR,
                CV.VEHICLE_OWNERSHIP_NO,
                CV.LOCATION_CODE,
                CV.REG_QUEUE_NO,
                CV.VEHICLEIMAGE_FRONT,
                CV.VEHICLEIMAGE_REAR,
                CV.VEHICLEIMAGE_RIGHT,
                CV.VEHICLEIMAGE_LEFT,
                CV.VEHICLE_RC_NO_PATH,
                CV.EXCEPTION_FLAG,
                (CASE CV.EXCEPTION_FLAG
                    WHEN 1 THEN 'CHARGED'
                    WHEN 2 THEN 'NOT CHARGED'
                    WHEN 3 THEN 'BLACK LISTED'
                    ELSE 'Unknown'
                 END)
                   EXCEPTION_FLAG_NAME,
                CV.STATUS,
                CV.VALID_UNTIL,
                CV.TID_FRONT,
                CV.TID_REAR,
                CV.ACCOUNT_BALANCE,
                CV.REGISTRATION_THROUGH,
                CV.IS_DOC_VERIFIED,
                CV.QUEUE_STATUS,
                (CASE CV.QUEUE_STATUS
                    WHEN 1 THEN 'OPEN'
                    WHEN 2 THEN 'POSTPONED'
                    WHEN 3 THEN 'PROCESSED'
                    ELSE 'Unknown'
                 END)
                   QUEUE_STATUS_NAME,
                CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                CA.RESIDENT_ID,
                CA.EMAIL_ID,
                CA.MOB_NUMBER
           FROM TBL_CUSTOMER_VEHICLE CV
                LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                   ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
          WHERE    LOWER (CV.TID_FRONT) = LOWER (P_TID)
                OR LOWER (CV.TID_REAR) = LOWER (P_TID);
   END VALIDATE_TID;

   PROCEDURE VEHICLE_LATEST_GETALL (P_LAST_UPDATE_TIME   IN     DATE,
                                    CUR_OUT                 OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CV.TMS_ID,
                  CV.ENTRY_ID,
                  CV.ACCOUNT_ID,
                  CV.VEH_REG_NO,
                  CV.TAG_ID,
                  CV.VEHICLE_CLASS_ID,
                  VC.VEHICLE_CLASS_NAME,
                  CV.CREATION_DATE,
                  CV.MODIFICATION_DATE,
                  CV.MODIFIED_BY,
                  CV.TRANSFER_STATUS,
                  CV.VEHICLE_RC_NO,
                  CV.OWNER_NAME,
                  CV.OWNER_ADDRESS,
                  CV.BRAND,
                  CV.VEHICLE_TYPE,
                  CV.VEHICLE_CATEGORY,
                  CV.MODEL_NO,
                  CV.MANUFACTURING_YEAR,
                  CV.CYCLINDER_CAPACITY,
                  CV.FRAME_NUMBER,
                  CV.ENGINE_NUMBER,
                  CV.VEHICLE_COLOR,
                  CV.FUEL_TYPE,
                  (CASE CV.FUEL_TYPE
                      WHEN 1 THEN 'GASOLINE'
                      WHEN 2 THEN 'DIESEL'
                      WHEN 3 THEN 'ELECTRIC'
                      ELSE 'Unknown'
                   END)
                     FUEL_TYPE_NAME,
                  CV.LICENCE_PLATE_COLOR,
                  (CASE CV.LICENCE_PLATE_COLOR
                      WHEN 1 THEN 'BLACK'
                      WHEN 2 THEN 'BLUE'
                      WHEN 3 THEN 'GREEN'
                      WHEN 4 THEN 'RED'
                      WHEN 5 THEN 'WHITE'
                      WHEN 6 THEN 'YELLOW'
                      ELSE 'Unknown'
                   END)
                     LICENCE_PLATE_COLOR_NAME,
                  CV.REGISTRATION_YEAR,
                  CV.VEHICLE_OWNERSHIP_NO,
                  CV.LOCATION_CODE,
                  CV.REG_QUEUE_NO,
                  CV.VEHICLEIMAGE_FRONT,
                  CV.VEHICLEIMAGE_REAR,
                  CV.VEHICLEIMAGE_RIGHT,
                  CV.VEHICLEIMAGE_LEFT,
                  CV.VEHICLE_RC_NO_PATH,
                  CV.EXCEPTION_FLAG,
                  (CASE CV.EXCEPTION_FLAG
                      WHEN 1 THEN 'CHARGED'
                      WHEN 2 THEN 'NOT CHARGED'
                      WHEN 3 THEN 'BLACK LISTED'
                      ELSE 'Unknown'
                   END)
                     EXCEPTION_FLAG_NAME,
                  CV.STATUS,
                  CV.VALID_UNTIL,
                  CV.TID_FRONT,
                  CV.TID_REAR,
                  CV.ACCOUNT_BALANCE,
                  CV.REGISTRATION_THROUGH,
                  CV.IS_DOC_VERIFIED,
                  CV.QUEUE_STATUS,
                  (CASE CV.QUEUE_STATUS
                      WHEN 1 THEN 'OPEN'
                      WHEN 2 THEN 'POSTPONED'
                      WHEN 3 THEN 'PROCESSED'
                      ELSE 'Unknown'
                   END)
                     QUEUE_STATUS_NAME,
                  CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                  CA.RESIDENT_ID,
                  CA.EMAIL_ID,
                  CA.MOB_NUMBER
             FROM TBL_CUSTOMER_VEHICLE CV
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
            WHERE CV.CREATION_DATE > P_LAST_UPDATE_TIME
         ORDER BY CV.VEHICLE_CLASS_ID;
   END VEHICLE_LATEST_GETALL;



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
      P_TRANSFER_STATUS             IN     NUMBER,
      P_OPENING_BALANCE             IN     NUMBER,
      P_CLOSING_BALANCE             IN     NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_ACCOUNT_HISTORY (TMS_ID,
                                       ACCOUNT_ID,
                                       ENTRY_ID,
                                       CUSTOMER_VEHICLE_ENTRY_ID,
                                       TRANSACTION_TYPE,
                                       TRANSACTION_ID,
                                       AMOUNT,
                                       SMS_SENT,
                                       EMAIL_SENT,
                                       CREATION_DATE,
                                       TRANSFER_STATUS,
                                       OPENING_BALANCE,
                                       CLOSING_BALANCE,
                                       SENT_STATUS)
           VALUES (P_TMS_ID,
                   P_ACCOUNT_ID,
                   ACCOUNT_HISTORY_SEQ.NEXTVAL,
                   P_CUSTOMER_VEHICLE_ENTRY_ID,
                   P_TRANSACTION_TYPE,
                   P_TRANSACTION_ID,
                   P_AMOUNT,
                   P_IS_SMS_SENT,
                   P_IS_EMAIL_SENT,
                   P_CREATION_DATE,
                   P_TRANSFER_STATUS,
                   P_OPENING_BALANCE,
                   P_CLOSING_BALANCE,
                   1);



      P_ENTRY_ID := ACCOUNT_HISTORY_SEQ.CURRVAL;
   END ACCOUNT_HISTORY_INSERT;



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
                                     P_TRANSFER_STATUS             IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_ACCOUNT_HISTORY
         SET TRANSACTION_ID = P_TRANSACTION_ID,
             ACCOUNT_ID = P_ACCOUNT_ID,
             CUSTOMER_VEHICLE_ENTRY_ID = P_CUSTOMER_VEHICLE_ENTRY_ID,
             TRANSACTION_TYPE = P_TRANSACTION_TYPE,
             AMOUNT = P_AMOUNT,
             SMS_SENT = P_IS_SMS_SENT,
             EMAIL_SENT = P_IS_EMAIL_SENT,
             MODIFIED_BY = P_MODIFIED_BY,
             TRANSFER_STATUS = P_TRANSFER_STATUS
       WHERE TMS_ID = P_TMS_ID AND ENTRY_ID = P_ENTRY_ID;
   END ACCOUNT_HISTORY_UPDATE;



   PROCEDURE ACCOUNT_HISTORY_DELETE (P_TMS_ID IN NUMBER, P_ENTRY_ID NUMBER)
   AS
   BEGIN
      DELETE TBL_ACCOUNT_HISTORY
       WHERE TMS_ID = P_TMS_ID AND ENTRY_ID = P_ENTRY_ID;
   END ACCOUNT_HISTORY_DELETE;



   PROCEDURE ACCOUNT_HISTORY_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT AH.TMS_ID,
                AH.ACCOUNT_ID,
                AH.ENTRY_ID,
                AH.CUSTOMER_VEHICLE_ENTRY_ID,
                AH.TRANSACTION_TYPE AH,
                AH.TRANSACTION_ID,
                AH.AMOUNT,
                AH.SMS_SENT,
                AH.EMAIL_SENT,
                AH.MODIFIED_BY,
                AH.CREATION_DATE
           FROM TBL_ACCOUNT_HISTORY AH;
   END ACCOUNT_HISTORY_GETALL;



   PROCEDURE ACCOUNT_HISTORY_BYACCOUNTID (
      P_ACCOUNT_ID         IN     NUMBER,
      P_TRANSACTION_TYPE   IN     NUMBER,
      CUR_OUT                 OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT AH.TMS_ID,
                  AH.ACCOUNT_ID,
                  AH.ENTRY_ID,
                  AH.CUSTOMER_VEHICLE_ENTRY_ID,
                  (CASE AH.TRANSACTION_TYPE
                      WHEN 1 THEN 'Sale'
                      WHEN 2 THEN 'Top-Up'
                      WHEN 3 THEN 'Refund'
                      WHEN 4 THEN 'Charge'
                   END)
                     TRANSACTION_TYPE,
                  AH.TRANSACTION_ID,
                  AH.AMOUNT,
                  AH.SMS_SENT,
                  AH.EMAIL_SENT,
                  AH.MODIFIED_BY,
                  AH.CREATION_DATE,
                  CA.FIRST_NAME,
                  CA.LAST_NAME,
                  CA.MOB_NUMBER,
                  CA.EMAIL_ID,
                  CA.ACCOUNT_BALANCE,
                  CA.ADDRESS,
                  CV.TAG_ID,
                  CV.VEH_REG_NO,
                  VC.VEHICLE_CLASS_NAME
             FROM TBL_ACCOUNT_HISTORY AH
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON AH.ACCOUNT_ID = CA.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                     ON AH.CUSTOMER_VEHICLE_ENTRY_ID = CV.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
            WHERE     AH.ACCOUNT_ID = P_ACCOUNT_ID
                  AND AH.TRANSACTION_TYPE = P_TRANSACTION_TYPE
         ORDER BY ENTRY_ID DESC;
   END ACCOUNT_HISTORY_BYACCOUNTID;



   PROCEDURE ACCOUNTHISTORY_ACCOUNTID_LAZY (P_ACCOUNT_ID   IN     NUMBER,
                                            P_PAGE_INDEX   IN     NUMBER,
                                            P_PAGE_SIZE    IN     NUMBER,
                                            CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH TRANS_HISTORY
              AS (  SELECT ROW_NUMBER () OVER (ORDER BY CREATION_DATE DESC)
                              AS ROWNUMBER,
                           AH.TMS_ID,
                           AH.ACCOUNT_ID,
                           AH.ENTRY_ID,
                           AH.CUSTOMER_VEHICLE_ENTRY_ID,
                           (CASE AH.TRANSACTION_TYPE
                               WHEN 1 THEN 'Sale'
                               WHEN 2 THEN 'Top-Up'
                               WHEN 3 THEN 'Refund'
                               WHEN 4 THEN 'Charge'
                            END)
                              TRANSACTION_TYPE_NAME,
                           AH.TRANSACTION_ID,
                           AH.AMOUNT,
                           AH.CREATION_DATE
                      FROM TBL_ACCOUNT_HISTORY AH
                     WHERE AH.ACCOUNT_ID = P_ACCOUNT_ID
                  ORDER BY CREATION_DATE DESC)
           SELECT ROWNUMBER,
                  AH.TRANSACTION_TYPE_NAME,
                  AH.ENTRY_ID,
                  AH.TRANSACTION_ID,
                  (CASE NVL (AH.TRANSACTION_ID, 0)
                      WHEN 0 THEN AH.ENTRY_ID
                      ELSE AH.TRANSACTION_ID
                   END)
                     AS MYTRANID,
                  AH.AMOUNT,
                  (CASE AH.TRANSACTION_ID
                      WHEN 0
                      THEN
                         TO_CHAR (AH.CREATION_DATE, 'DD-Mon-YYYY HH:MI:SS AM')
                      ELSE
                         TO_CHAR (T.TRANSACTION_DATETIME,
                                  'DD-Mon-YYYY HH:MI:SS AM')
                   END)
                     CREATION_DATE,
                  P.PLAZA_NAME,
                  CV.VEH_REG_NO,
                  VC.VEHICLE_CLASS_NAME
             FROM TRANS_HISTORY AH
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON AH.ACCOUNT_ID = CA.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                     ON AH.CUSTOMER_VEHICLE_ENTRY_ID = CV.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_TRANSACTION T
                     ON AH.TRANSACTION_ID = T.TRANSACTION_ID
                  LEFT OUTER JOIN TBL_PLAZA P
                     ON T.PLAZA_ID = P.PLAZA_ID
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END ACCOUNTHISTORY_ACCOUNTID_LAZY;



   PROCEDURE ACCOUNTHISTORY_VEHICLEID_LAZY (P_ACCOUNT_ID   IN     NUMBER,
                                            P_VEHICLE_ID   IN     NUMBER,
                                            P_PAGE_INDEX   IN     NUMBER,
                                            P_PAGE_SIZE    IN     NUMBER,
                                            CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH TRANS_HISTORY
              AS (  SELECT ROW_NUMBER () OVER (ORDER BY CREATION_DATE DESC)
                              AS ROWNUMBER,
                           AH.TMS_ID,
                           AH.ACCOUNT_ID,
                           AH.ENTRY_ID,
                           AH.CUSTOMER_VEHICLE_ENTRY_ID,
                           (CASE AH.TRANSACTION_TYPE
                               WHEN 1 THEN 'Sale'
                               WHEN 2 THEN 'Top-Up'
                               WHEN 3 THEN 'Refund'
                               WHEN 4 THEN 'Charge'
                            END)
                              TRANSACTION_TYPE_NAME,
                           AH.TRANSACTION_ID,
                           AH.AMOUNT,
                           AH.CREATION_DATE
                      FROM TBL_ACCOUNT_HISTORY AH
                     WHERE     AH.ACCOUNT_ID = P_ACCOUNT_ID
                           AND AH.CUSTOMER_VEHICLE_ENTRY_ID = P_VEHICLE_ID
                  ORDER BY CREATION_DATE DESC)
           SELECT ROWNUMBER,
                  AH.TRANSACTION_TYPE_NAME,
                  AH.ENTRY_ID,
                  AH.TRANSACTION_ID,
                  (CASE AH.TRANSACTION_ID
                      WHEN 0 THEN AH.ENTRY_ID
                      ELSE AH.TRANSACTION_ID
                   END)
                     AS MYTRANID,
                  AH.AMOUNT,
                  (CASE AH.TRANSACTION_ID
                      WHEN 0
                      THEN
                         TO_CHAR (AH.CREATION_DATE, 'DD-Mon-YYYY HH:MI:SS AM')
                      ELSE
                         TO_CHAR (T.TRANSACTION_DATETIME,
                                  'DD-Mon-YYYY HH:MI:SS AM')
                   END)
                     CREATION_DATE,
                  P.PLAZA_NAME,
                  CV.VEH_REG_NO,
                  VC.VEHICLE_CLASS_NAME
             FROM TRANS_HISTORY AH
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON AH.ACCOUNT_ID = CA.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                     ON AH.CUSTOMER_VEHICLE_ENTRY_ID = CV.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_TRANSACTION T
                     ON AH.TRANSACTION_ID = T.TRANSACTION_ID
                  LEFT OUTER JOIN TBL_PLAZA P
                     ON T.PLAZA_ID = P.PLAZA_ID
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END ACCOUNTHISTORY_VEHICLEID_LAZY;



   /*CSV*/



   PROCEDURE TRAN_CSV_GETNORMALTRAN (P_START_TIME   IN     DATE,
                                     P_END_TIME     IN     DATE,
                                     CUR_OUT           OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE
              AS (  SELECT *
                      FROM TBL_TRANSACTION
                     WHERE     TRANSACTION_DATETIME >= P_START_TIME
                           AND TRANSACTION_DATETIME <= P_END_TIME
                           AND IS_REGISTERED = 1
                  ORDER BY TRANSACTION_ID)
           SELECT TRAN.TRANSACTION_ID,
                  TRAN.TRANSACTION_DATETIME AS TIME_STAMP,
                  UPPER (CTP.PLATE_NUMBER) AS EVI_VRN_FRONT,
                  UPPER (CTP_REAR.PLATE_NUMBER) AS EVI_VRN_REAR,
                  UPPER (NFP.PLATE_NUMBER) AS FRONT_VRN,
                  UPPER (NFP1.PLATE_NUMBER) AS REAR_VRN,
                  (CASE NVL (CTP.VEHICLE_CLASS_ID, 0)
                      WHEN 1 THEN 'Two-wheeled'
                      WHEN 2 THEN 'Small'
                      WHEN 3 THEN 'Medium'
                      WHEN 4 THEN 'Large'
                      ELSE 'blank'
                   END)
                     EVI_CLASS_FRONT,
                  (CASE NVL (CTP_REAR.VEHICLE_CLASS_ID, 0)
                      WHEN 1 THEN 'Two-wheeled'
                      WHEN 2 THEN 'Small'
                      WHEN 3 THEN 'Medium'
                      WHEN 4 THEN 'Large'
                      ELSE 'blank'
                   END)
                     EVI_CLASS_REAR,
                  (CASE NVL (NFP.VEHICLE_CLASS_ID, 0)
                      WHEN 1 THEN 'Two-wheeled'
                      WHEN 2 THEN 'Small'
                      WHEN 3 THEN 'Medium'
                      WHEN 4 THEN 'Large'
                      ELSE 'blank'
                   END)
                     ANPR_CLASS_FRONT,
                  (CASE NVL (NFP1.VEHICLE_CLASS_ID, 0)
                      WHEN 1 THEN 'Two-wheeled'
                      WHEN 2 THEN 'Small'
                      WHEN 3 THEN 'Medium'
                      WHEN 4 THEN 'Large'
                      ELSE 'blank'
                   END)
                     ANPR_CLASS_REAR,
                  TRAN.LANE_ID AS LANE_ID,
                  CTP.OBJECT_ID AS EVI_ID,
                  CUST_ACC.RESIDENT_ID AS RESIDENT_ID,
                  (CUST_ACC.FIRST_NAME) AS VEHICLE_OWNER,
                  NVL (ACC_HIST.AMOUNT, 0) AS AMOUNT_CHARGED,
                  NVL (CUST_ACC.ACCOUNT_BALANCE, 0) AS BALANCE,
                  (CASE NVL (ACC_HIST.SMS_SENT, 0)
                      WHEN 1 THEN 'SENT'
                      ELSE 'NOT SENT'
                   END)
                     AS SMS_NOTIFICATION,
                  NFP.PLATE_THUMBNAIL AS FRONT_PLATE_IMAGE,
                  NFP.VEHICLE_THUMBNAIL AS FRONT_VEHICLE_IMAGE,
                  NFP.VIDEO_URL AS FRONT_VIDEO_URL,
                  NFP1.PLATE_THUMBNAIL AS REAR_PLATE_IMAGE,
                  NFP1.VEHICLE_THUMBNAIL AS REAR_VEHICLE_IMAGE,
                  NFP1.VIDEO_URL AS REAR_VIDEO_URL
             FROM CTE TRAN
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON TRAN.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP_REAR
                     ON TRAN.CT_ENTRY_ID_REAR = CTP_REAR.ENTRY_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP
                     ON TRAN.NF_ENTRY_ID_FRONT = NFP.ENTRY_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP1
                     ON TRAN.NF_ENTRY_ID_REAR = NFP1.ENTRY_ID
                  LEFT OUTER JOIN TBL_ACCOUNT_HISTORY ACC_HIST
                     ON TRAN.TRANSACTION_ID = ACC_HIST.TRANSACTION_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CUST_VEH
                     ON UPPER (CTP.OBJECT_ID) = UPPER (CUST_VEH.TAG_ID)
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CUST_ACC
                     ON CUST_VEH.ACCOUNT_ID = CUST_ACC.ACCOUNT_ID
         ORDER BY TRAN.TRANSACTION_DATETIME;
   END TRAN_CSV_GETNORMALTRAN;



   PROCEDURE TRAN_CSV_GETVIOTRAN (P_START_TIME   IN     DATE,
                                  P_END_TIME     IN     DATE,
                                  CUR_OUT           OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE
              AS (  SELECT *
                      FROM TBL_TRANSACTION
                     WHERE     TRANSACTION_DATETIME >= P_START_TIME
                           AND TRANSACTION_DATETIME <= P_END_TIME
                           AND IS_REGISTERED = 2
                  ORDER BY TRANSACTION_ID)
           SELECT TRAN.TRANSACTION_ID,
                  TRAN.TRANSACTION_DATETIME AS TIME_STAMP,
                  NFP.PLATE_NUMBER AS FRONT_VRN,
                  NFP1.PLATE_NUMBER AS REAR_VRN,
                  (CASE NVL (NFP.VEHICLE_CLASS_ID, 0)
                      WHEN 1 THEN 'Two-wheeled'
                      WHEN 2 THEN 'Small'
                      WHEN 3 THEN 'Medium'
                      WHEN 4 THEN 'Large'
                      ELSE 'blank'
                   END)
                     ANPR_CLASS_FRONT,
                  (CASE NVL (NFP1.VEHICLE_CLASS_ID, 0)
                      WHEN 1 THEN 'Two-wheeled'
                      WHEN 2 THEN 'Small'
                      WHEN 3 THEN 'Medium'
                      WHEN 4 THEN 'Large'
                      ELSE 'blank'
                   END)
                     ANPR_CLASS_REAR,
                  TRAN.LANE_ID AS LANE_ID,
                  NFP.PLATE_THUMBNAIL AS FRONT_PLATE_IMAGE,
                  NFP.VEHICLE_THUMBNAIL AS FRONT_VEHICLE_IMAGE,
                  NFP.VIDEO_URL AS FRONT_VIDEO_URL,
                  NFP1.PLATE_THUMBNAIL AS REAR_PLATE_IMAGE,
                  NFP1.VEHICLE_THUMBNAIL AS REAR_VEHICLE_IMAGE,
                  NFP1.VIDEO_URL AS REAR_VIDEO_URL
             FROM CTE TRAN
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP
                     ON TRAN.NF_ENTRY_ID_FRONT = NFP.ENTRY_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP1
                     ON TRAN.NF_ENTRY_ID_REAR = NFP1.ENTRY_ID
         ORDER BY TRAN.TRANSACTION_DATETIME;
   END TRAN_CSV_GETVIOTRAN;



   PROCEDURE TRAN_CSV_GETTRANWITHOUTTAG (P_START_TIME   IN     DATE,
                                         P_END_TIME     IN     DATE,
                                         CUR_OUT           OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT TRAN.TRANSACTION_ID,
                  TRAN.TRANSACTION_DATETIME AS TIME_STAMP,
                  CUST_VEH.VEH_REG_NO AS EVI_VEH_NO,
                  NFP.PLATE_NUMBER AS FRONT_VRN,
                  NFP1.PLATE_NUMBER AS REAR_VRN,
                  VC1.VEHICLE_CLASS_NAME AS VEH_NAME_EVI,
                  VC2.VEHICLE_CLASS_NAME AS VEH_NAME_NODEFLUX,
                  TRAN.LANE_ID AS LANE_ID,
                  CTP.OBJECT_ID AS EVI_ID,
                  CUST_ACC.ACCOUNT_ID AS OWNER_ID,
                  (CUST_ACC.FIRST_NAME || CUST_ACC.LAST_NAME) AS VEHICLE_OWNER,
                  ACC_HIST.AMOUNT AS AMOUNT_CHARGED,
                  CUST_ACC.ACCOUNT_BALANCE AS BALANCE,
                  ACC_HIST.SMS_SENT AS SMS_NOTIFICATION
             FROM TBL_TRANSACTION TRAN
                  LEFT OUTER JOIN TBL_CROSSTALK_PACKET CTP
                     ON TRAN.CT_ENTRY_ID = CTP.ENTRY_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CUST_VEH
                     ON CTP.OBJECT_ID = CUST_VEH.TAG_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP
                     ON TRAN.NF_ENTRY_ID_FRONT = NFP.ENTRY_ID
                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP1
                     ON TRAN.NF_ENTRY_ID_REAR = NFP1.ENTRY_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CUST_ACC
                     ON CUST_VEH.ACCOUNT_ID = CUST_ACC.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_ACCOUNT_HISTORY ACC_HIST
                     ON TRAN.TRANSACTION_ID = ACC_HIST.TRANSACTION_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC1
                     ON CUST_VEH.VEHICLE_CLASS_ID = VC1.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC2
                     ON NFP.VEHICLE_CLASS_ID = VC2.VEHICLE_CLASS_ID
            WHERE     TRAN.TRANSACTION_DATETIME >= P_START_TIME
                  AND TRAN.TRANSACTION_DATETIME <= P_END_TIME
         --AND TRAN.CROSSTALK_VRN = "abc"



         ORDER BY TRAN.TRANSACTION_ID;
   END TRAN_CSV_GETTRANWITHOUTTAG;



   PROCEDURE JOIN_AUDIT_TRANSACTIONS (
      P_PARENT_TRANSACTION_ID      IN NUMBER := NULL,
      P_CHILD_1_TRANSACTION_ID     IN NUMBER := NULL,
      P_CHILD_2_TRANSACTION_ID     IN NUMBER := NULL,
      P_AUDITED_VRN                IN NVARCHAR2,
      P_AUDITED_VEHICLE_CLASS_ID   IN NUMBER,
      P_AUDITOR_ID                 IN NUMBER)
   AS
      CT_ENTRYID         NUMBER := NULL;



      NF_ENTRYID_FRONT   NUMBER := NULL;



      NF_ENTRYID_REAR    NUMBER := NULL;
   BEGIN
      --Update Child 1 Transaction



      IF (P_CHILD_1_TRANSACTION_ID != 0)
      THEN
         UPDATE TBL_TRANSACTION
            SET AUDITOR_ID = P_AUDITOR_ID,
                AUDIT_STATUS = 1,
                AUDIT_DATE = SYSDATE,
                AUDITED_VRN = P_AUDITED_VRN,
                AUDITED_VEHICLE_CLASS_ID = P_AUDITED_VEHICLE_CLASS_ID
          WHERE TRANSACTION_ID = P_CHILD_1_TRANSACTION_ID;



         SELECT NVL (TC.CT_ENTRY_ID, 0)
           INTO CT_ENTRYID
           FROM TBL_TRANSACTION TC
          WHERE TC.TRANSACTION_ID = P_CHILD_1_TRANSACTION_ID;



         SELECT NVL (NF_ENTRY_ID_REAR, 0)
           INTO NF_ENTRYID_REAR
           FROM TBL_TRANSACTION
          WHERE TRANSACTION_ID = P_CHILD_1_TRANSACTION_ID;



         SELECT NVL (NF_ENTRY_ID_FRONT, 0)
           INTO NF_ENTRYID_FRONT
           FROM TBL_TRANSACTION
          WHERE TRANSACTION_ID = P_CHILD_1_TRANSACTION_ID;
      END IF;



      --Update Child 2 Two Transaction If Exists



      IF (P_CHILD_2_TRANSACTION_ID != 0)
      THEN
         UPDATE TBL_TRANSACTION
            SET AUDITOR_ID = P_AUDITOR_ID,
                AUDIT_STATUS = 1,
                AUDIT_DATE = SYSDATE,
                AUDITED_VRN = P_AUDITED_VRN,
                AUDITED_VEHICLE_CLASS_ID = P_AUDITED_VEHICLE_CLASS_ID
          WHERE TRANSACTION_ID = P_CHILD_2_TRANSACTION_ID;



         IF (NVL (CT_ENTRYID, 0) = 0)
         THEN
            SELECT NVL (TC.CT_ENTRY_ID, 0)
              INTO CT_ENTRYID
              FROM TBL_TRANSACTION TC
             WHERE TC.TRANSACTION_ID = P_CHILD_2_TRANSACTION_ID;
         END IF;



         IF (NVL (NF_ENTRYID_REAR, 0) = 0)
         THEN
            SELECT NVL (NF_ENTRY_ID_REAR, 0)
              INTO NF_ENTRYID_REAR
              FROM TBL_TRANSACTION
             WHERE TRANSACTION_ID = P_CHILD_2_TRANSACTION_ID;
         END IF;



         IF (NVL (NF_ENTRYID_FRONT, 0) = 0)
         THEN
            SELECT NVL (NF_ENTRY_ID_FRONT, 0)
              INTO NF_ENTRYID_FRONT
              FROM TBL_TRANSACTION
             WHERE TRANSACTION_ID = P_CHILD_2_TRANSACTION_ID;
         END IF;
      END IF;



      --Update Parent Transaction for CT_ENTRY IF EXITS



      IF (CT_ENTRYID != 0)
      THEN
         UPDATE TBL_TRANSACTION
            SET CT_ENTRY_ID = CT_ENTRYID
          WHERE TRANSACTION_ID = P_PARENT_TRANSACTION_ID;
      END IF;



      --Update Parent Transaction for NF_ENTRY_ID_REAR IF EXITS



      IF (NF_ENTRYID_REAR != 0)
      THEN
         UPDATE TBL_TRANSACTION
            SET NF_ENTRY_ID_REAR = NF_ENTRYID_REAR
          WHERE TRANSACTION_ID = P_PARENT_TRANSACTION_ID;
      END IF;



      --Update Parent Transaction for NF_ENTRY_ID_REAR IF EXITS



      IF (NF_ENTRYID_FRONT != 0)
      THEN
         UPDATE TBL_TRANSACTION
            SET NF_ENTRY_ID_FRONT = NF_ENTRYID_FRONT
          WHERE TRANSACTION_ID = P_PARENT_TRANSACTION_ID;
      END IF;



      UPDATE TBL_TRANSACTION
         SET AUDITOR_ID = P_AUDITOR_ID,
             AUDIT_STATUS = 1,
             AUDIT_DATE = SYSDATE,
             AUDITED_VRN = P_AUDITED_VRN,
             AUDITED_VEHICLE_CLASS_ID = P_AUDITED_VEHICLE_CLASS_ID
       WHERE TRANSACTION_ID = P_PARENT_TRANSACTION_ID;
   END JOIN_AUDIT_TRANSACTIONS;



   /*VEHICLE CLASS MAPPING*/



   PROCEDURE VEHICLE_CLASS_MAPPING_INSERT (
      P_TMS_ID                     IN NUMBER,
      P_MAPPING_VEHICLE_CLASS_ID   IN NUMBER,
      P_MLFF_VEHICLE_CLASS_ID      IN NUMBER,
      P_ANPR_VEHICLE_CLASS_ID      IN NUMBER,
      P_ANPR_VEHICLE_CLASS_NAME    IN NVARCHAR2,
      P_CREATION_DATE              IN DATE,
      P_TRANSFER_STATUS            IN NUMBER)
   AS
   BEGIN
      INSERT INTO TBL_VEHICLE_CLASS_MAPPING (TMS_ID,
                                             MAPPING_VEHICLE_CLASS_ID,
                                             MLFF_VEHICLE_CLASS_ID,
                                             ANPR_VEHICLE_CLASS_ID,
                                             ANPR_VEHICLE_CLASS_NAME,
                                             CREATION_DATE,
                                             TRANSFER_STATUS)
           VALUES (P_TMS_ID,
                   P_MAPPING_VEHICLE_CLASS_ID,
                   P_MLFF_VEHICLE_CLASS_ID,
                   P_ANPR_VEHICLE_CLASS_ID,
                   P_ANPR_VEHICLE_CLASS_NAME,
                   P_CREATION_DATE,
                   P_TRANSFER_STATUS);
   END VEHICLE_CLASS_MAPPING_INSERT;



   PROCEDURE VEHICLE_CLASS_MAPPING_UPDATE (
      P_TMS_ID                     IN NUMBER,
      P_MAPPING_VEHICLE_CLASS_ID   IN NUMBER,
      P_MLFF_VEHICLE_CLASS_ID      IN NUMBER,
      P_ANPR_VEHICLE_CLASS_ID      IN NUMBER,
      P_ANPR_VEHICLE_CLASS_NAME    IN NVARCHAR2,
      P_MODIFIED_BY                IN NUMBER,
      P_MODIFICATION_DATE          IN DATE,
      P_TRANSFER_STATUS            IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_VEHICLE_CLASS_MAPPING
         SET MLFF_VEHICLE_CLASS_ID = P_MLFF_VEHICLE_CLASS_ID,
             ANPR_VEHICLE_CLASS_ID = P_ANPR_VEHICLE_CLASS_ID,
             ANPR_VEHICLE_CLASS_NAME = P_ANPR_VEHICLE_CLASS_NAME,
             MODIFIED_BY = P_MODIFIED_BY,
             MODIFICATION_DATE = MODIFICATION_DATE,
             TRANSFER_STATUS = P_TRANSFER_STATUS
       WHERE     TMS_ID = P_TMS_ID
             AND MAPPING_VEHICLE_CLASS_ID = P_MAPPING_VEHICLE_CLASS_ID;
   END VEHICLE_CLASS_MAPPING_UPDATE;



   PROCEDURE VEHICLE_CLASS_MAPPING_GETALL (CUR_OUT OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT VCM.TMS_ID,
                  VCM.MAPPING_VEHICLE_CLASS_ID,
                  VCM.MLFF_VEHICLE_CLASS_ID,
                  VC.VEHICLE_CLASS_NAME AS MLFF_VEHICLE_CLASS_NAME,
                  VCM.ANPR_VEHICLE_CLASS_ID,
                  VCM.ANPR_VEHICLE_CLASS_NAME,
                  VCM.CREATION_DATE,
                  VCM.MODIFIED_BY,
                  VCM.MODIFICATION_DATE,
                  VCM.TRANSFER_STATUS
             FROM    TBL_VEHICLE_CLASS_MAPPING VCM
                  LEFT OUTER JOIN
                     TBL_VEHICLE_CLASS VC
                  ON VCM.MLFF_VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
         ORDER BY MAPPING_VEHICLE_CLASS_ID;
   END VEHICLE_CLASS_MAPPING_GETALL;



   PROCEDURE VEHICLE_CLASS_MAPPING_GETBYID (
      P_MAPPING_VEHICLE_CLASS_ID   IN     NUMBER,
      CUR_OUT                         OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT VCM.TMS_ID,
                VCM.MAPPING_VEHICLE_CLASS_ID,
                VCM.MLFF_VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME AS MLFF_VEHICLE_CLASS_NAME,
                VCM.ANPR_VEHICLE_CLASS_ID,
                VCM.ANPR_VEHICLE_CLASS_NAME,
                VCM.CREATION_DATE,
                VCM.MODIFIED_BY,
                VCM.MODIFICATION_DATE,
                VCM.TRANSFER_STATUS
           FROM    TBL_VEHICLE_CLASS_MAPPING VCM
                LEFT OUTER JOIN
                   TBL_VEHICLE_CLASS VC
                ON VCM.MLFF_VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
          WHERE MAPPING_VEHICLE_CLASS_ID = P_MAPPING_VEHICLE_CLASS_ID;
   END VEHICLE_CLASS_MAPPING_GETBYID;



   PROCEDURE VEHICLECLASS_MAPPING_GETBYNAME (
      P_ANPR_VEHICLE_CLASS_NAME   IN     NVARCHAR2,
      CUR_OUT                        OUT T_CURSOR)
   AS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT VCM.TMS_ID,
                VCM.MAPPING_VEHICLE_CLASS_ID,
                VCM.MLFF_VEHICLE_CLASS_ID,
                VC.VEHICLE_CLASS_NAME AS MLFF_VEHICLE_CLASS_NAME,
                VCM.ANPR_VEHICLE_CLASS_ID,
                VCM.ANPR_VEHICLE_CLASS_NAME,
                VCM.CREATION_DATE,
                VCM.MODIFIED_BY,
                VCM.MODIFICATION_DATE,
                VCM.TRANSFER_STATUS
           FROM    TBL_VEHICLE_CLASS_MAPPING VCM
                LEFT OUTER JOIN
                   TBL_VEHICLE_CLASS VC
                ON VCM.MLFF_VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
          WHERE LOWER (ANPR_VEHICLE_CLASS_NAME) =
                   LOWER (P_ANPR_VEHICLE_CLASS_NAME);
   END VEHICLECLASS_MAPPING_GETBYNAME;



   /*PROVINCE*/



   PROCEDURE PROVINCE_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_PROVINCE P
         ORDER BY P.PROVINCE_ID;
   END PROVINCE_GETALL;


   PROCEDURE PROVINCE_GETBYID (P_PROVINCE_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_PROVINCE
            WHERE PROVINCE_ID = P_PROVINCE_ID
         ORDER BY PROVINCE_ID;
   END PROVINCE_GETBYID;


   PROCEDURE PROVINCE_INSERT (P_TMS_ID              IN     NUMBER,
                              P_PROVINCE_NAME       IN     NVARCHAR2,
                              P_CREATION_DATE       IN     DATE,
                              P_MODIFIER_ID         IN     NUMBER,
                              P_MODIFICATION_DATE   IN     DATE,
                              P_RETURNMSG              OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_PROVINCE
       WHERE PROVINCE_NAME = P_PROVINCE_NAME;

      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'PROVINCE FOUND';
      ELSE
         INSERT INTO TBL_PROVINCE (TMS_ID,
                                   PROVINCE_ID,
                                   PROVINCE_NAME,
                                   MODIFIER_ID,
                                   CREATION_DATE,
                                   TRANSFER_STATUS)
              VALUES (
                        P_TMS_ID,
                          NVL ( (SELECT MAX (PROVINCE_ID) FROM TBL_PROVINCE),
                               0)
                        + 1,
                        P_PROVINCE_NAME,
                        P_MODIFIER_ID,
                        P_CREATION_DATE,
                        1);

         P_RETURNMSG := 'PROVINCE CREATED';
      END IF;
   END PROVINCE_INSERT;



   PROCEDURE PROVINCE_UPDATE (P_PROVINCE_ID         IN     NUMBER,
                              P_PROVINCE_NAME       IN     NVARCHAR2,
                              P_MODIFIER_ID         IN     NUMBER,
                              P_MODIFICATION_DATE   IN     DATE,
                              P_RETURNMSG              OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_PROVINCE
       WHERE PROVINCE_NAME = P_PROVINCE_NAME;

      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'PROVINCE FOUND';
      ELSE
         UPDATE TBL_PROVINCE
            SET PROVINCE_NAME = P_PROVINCE_NAME,
                MODIFIER_ID = P_MODIFIER_ID,
                MODIFICATION_DATE = P_MODIFICATION_DATE
          WHERE PROVINCE_ID = P_PROVINCE_ID;

         P_RETURNMSG := 'PROVINCE UPDATED';
      END IF;
   END PROVINCE_UPDATE;

   /*CITY*/



   PROCEDURE CITY_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT C.*, P.PROVINCE_NAME
             FROM TBL_CITY C, TBL_PROVINCE P
            WHERE C.PROVINCE_ID = P.PROVINCE_ID
         ORDER BY CITY_ID;
   END CITY_GETALL;


   PROCEDURE CITY_GETBYID (P_CITY_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_CITY
            WHERE CITY_ID = P_CITY_ID
         ORDER BY CITY_ID;
   END CITY_GETBYID;


   PROCEDURE CITY_INSERT (P_TMS_ID          IN     NUMBER,
                          P_PROVINCE_ID     IN     NUMBER,
                          P_CITY_NAME       IN     NVARCHAR2,
                          P_CREATION_DATE   IN     DATE,
                          P_MODIFIER_ID     IN     NUMBER,
                          P_RETURNMSG          OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_CITY
       WHERE CITY_NAME = P_CITY_NAME;

      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'CITY FOUND';
      ELSE
         INSERT INTO TBL_CITY (TMS_ID,
                               CITY_ID,
                               PROVINCE_ID,
                               CITY_NAME,
                               MODIFIER_ID,
                               CREATION_DATE,
                               TRANSFER_STATUS)
              VALUES (P_TMS_ID,
                      NVL ( (SELECT MAX (CITY_ID) FROM TBL_CITY), 0) + 1,
                      P_PROVINCE_ID,
                      P_CITY_NAME,
                      P_MODIFIER_ID,
                      P_CREATION_DATE,
                      1);

         P_RETURNMSG := 'CITY CREATED';
      END IF;
   END CITY_INSERT;



   PROCEDURE CITY_UPDATE (P_CITY_ID             IN     NUMBER,
                          P_PROVINCE_ID         IN     NVARCHAR2,
                          P_CITY_NAME           IN     NVARCHAR2,
                          P_MODIFIER_ID         IN     NUMBER,
                          P_MODIFICATION_DATE   IN     DATE,
                          P_RETURNMSG              OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_CITY
       WHERE CITY_NAME = P_CITY_NAME AND CITY_ID <> P_CITY_ID;

      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'CITY FOUND';
      ELSE
         UPDATE TBL_CITY
            SET CITY_NAME = P_CITY_NAME,
                PROVINCE_ID = P_PROVINCE_ID,
                MODIFIER_ID = P_MODIFIER_ID,
                MODIFICATION_DATE = P_MODIFICATION_DATE
          WHERE CITY_ID = P_CITY_ID;

         P_RETURNMSG := 'CITY UPDATED';
      END IF;
   END CITY_UPDATE;



   PROCEDURE CITY_GETBYPROVINCEID (P_TMS_ID        IN     NUMBER,
                                   P_PROVINCE_ID   IN     NUMBER,
                                   CUR_OUT            OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_CITY
            WHERE TMS_ID = P_TMS_ID AND PROVINCE_ID = P_PROVINCE_ID
         ORDER BY CITY_ID;
   END CITY_GETBYPROVINCEID;



   /*DISTRICT*/



   PROCEDURE DISTRICT_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT D.*,
                  C.CITY_NAME,
                  P.PROVINCE_ID,
                  P.PROVINCE_NAME
             FROM TBL_DISTRICT D, TBL_CITY C, TBL_PROVINCE P
            WHERE D.CITY_ID = C.CITY_ID AND P.PROVINCE_ID = C.PROVINCE_ID
         ORDER BY DISTRICT_ID;
   END DISTRICT_GETALL;



   PROCEDURE DISTRICT_GETBYCITYID (P_TMS_ID    IN     NUMBER,
                                   P_CITY_ID   IN     NUMBER,
                                   CUR_OUT        OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_DISTRICT
            WHERE TMS_ID = P_TMS_ID AND CITY_ID = P_CITY_ID
         ORDER BY DISTRICT_ID;
   END DISTRICT_GETBYCITYID;


   PROCEDURE DISTRICT_GETBYID (P_DISTRICT_ID IN NUMBER, CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_DISTRICT
            WHERE DISTRICT_ID = P_DISTRICT_ID
         ORDER BY DISTRICT_ID;
   END DISTRICT_GETBYID;

   PROCEDURE DISTRICT_INSERT (P_TMS_ID          IN     NUMBER,
                              P_CITY_ID         IN     NUMBER,
                              P_DISTRICT_NAME   IN     NVARCHAR2,
                              P_CREATION_DATE   IN     DATE,
                              P_MODIFIER_ID     IN     NUMBER,
                              P_RETURNMSG          OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_DISTRICT
       WHERE DISTRICT_NAME = P_DISTRICT_NAME;

      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'DISTRICT FOUND';
      ELSE
         INSERT INTO TBL_DISTRICT (TMS_ID,
                                   DISTRICT_ID,
                                   CITY_ID,
                                   DISTRICT_NAME,
                                   MODIFIER_ID,
                                   CREATION_DATE,
                                   TRANSFER_STATUS)
              VALUES (
                        P_TMS_ID,
                          NVL ( (SELECT MAX (DISTRICT_ID) FROM TBL_DISTRICT),
                               0)
                        + 1,
                        P_CITY_ID,
                        P_DISTRICT_NAME,
                        P_MODIFIER_ID,
                        P_CREATION_DATE,
                        1);

         P_RETURNMSG := 'DISTRICT CREATED';
      END IF;
   END DISTRICT_INSERT;



   PROCEDURE DISTRICT_UPDATE (P_DISTRICT_ID         IN     NUMBER,
                              P_CITY_ID             IN     NUMBER,
                              P_DISTRICT_NAME       IN     NVARCHAR2,
                              P_MODIFIER_ID         IN     NUMBER,
                              P_MODIFICATION_DATE   IN     DATE,
                              P_RETURNMSG              OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_DISTRICT
       WHERE DISTRICT_NAME = P_DISTRICT_NAME AND DISTRICT_ID <> P_DISTRICT_ID;

      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'DISTRICT FOUND';
      ELSE
         UPDATE TBL_DISTRICT
            SET DISTRICT_NAME = P_DISTRICT_NAME,
                CITY_ID = P_CITY_ID,
                MODIFIER_ID = P_MODIFIER_ID,
                MODIFICATION_DATE = P_MODIFICATION_DATE
          WHERE DISTRICT_ID = P_DISTRICT_ID;

         P_RETURNMSG := 'DISTRICT UPDATED';
      END IF;
   END DISTRICT_UPDATE;

   /*SUB DISTRICT*/



   PROCEDURE SUBDISTRICT_GETALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT SD.*,
                  D.DISTRICT_NAME,
                  C.CITY_ID,
                  C.CITY_NAME,
                  P.PROVINCE_ID,
                  P.PROVINCE_NAME
             FROM TBL_SUB_DISTRICT SD,
                  TBL_DISTRICT D,
                  TBL_CITY C,
                  TBL_PROVINCE P
            WHERE     P.PROVINCE_ID = C.PROVINCE_ID
                  AND C.CITY_ID = D.CITY_ID
                  AND SD.DISTRICT_ID = D.DISTRICT_ID
         ORDER BY SUB_DISTRICT_ID;
   END SUBDISTRICT_GETALL;



   PROCEDURE SUBDISTRICT_GETBYDISTRICTID (P_TMS_ID        IN     NUMBER,
                                          P_DISTRICT_ID   IN     NUMBER,
                                          CUR_OUT            OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_SUB_DISTRICT
            WHERE TMS_ID = P_TMS_ID AND DISTRICT_ID = P_DISTRICT_ID
         ORDER BY SUB_DISTRICT_ID;
   END SUBDISTRICT_GETBYDISTRICTID;


   PROCEDURE SUBDISTRICT_GETBYID (P_SUBDISTRICT_ID   IN     NUMBER,
                                  CUR_OUT               OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM TBL_SUB_DISTRICT
            WHERE SUB_DISTRICT_ID = P_SUBDISTRICT_ID
         ORDER BY SUB_DISTRICT_ID;
   END SUBDISTRICT_GETBYID;



   PROCEDURE SUBDISTRICT_INSERT (P_TMS_ID              IN     NUMBER,
                                 P_DISTRICT_ID         IN     NUMBER,
                                 P_SUB_DISTRICT_NAME   IN     NVARCHAR2,
                                 P_CREATION_DATE       IN     DATE,
                                 P_MODIFIER_ID         IN     NUMBER,
                                 P_RETURNMSG              OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_SUB_DISTRICT
       WHERE SUB_DISTRICT_NAME = P_SUB_DISTRICT_NAME;

      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'SUBDISTRICT FOUND';
      ELSE
         INSERT INTO TBL_SUB_DISTRICT (TMS_ID,
                                       SUB_DISTRICT_ID,
                                       DISTRICT_ID,
                                       SUB_DISTRICT_NAME,
                                       MODIFIER_ID,
                                       CREATION_DATE,
                                       TRANSFER_STATUS)
              VALUES (
                        P_TMS_ID,
                          NVL (
                             (SELECT MAX (SUB_DISTRICT_ID)
                                FROM TBL_SUB_DISTRICT),
                             0)
                        + 1,
                        P_DISTRICT_ID,
                        P_SUB_DISTRICT_NAME,
                        P_MODIFIER_ID,
                        P_CREATION_DATE,
                        1);

         P_RETURNMSG := 'SUBDISTRICT CREATED';
      END IF;
   END SUBDISTRICT_INSERT;



   PROCEDURE SUBDISTRICT_UPDATE (P_DISTRICT_ID         IN     NUMBER,
                                 P_SUB_DISTRICT_ID     IN     NUMBER,
                                 P_SUB_DISTRICT_NAME   IN     NVARCHAR2,
                                 P_MODIFIER_ID         IN     NUMBER,
                                 P_MODIFICATION_DATE   IN     DATE,
                                 P_RETURNMSG              OUT NVARCHAR2)
   AS
      C_COUNT   NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_SUB_DISTRICT
       WHERE     SUB_DISTRICT_NAME = P_SUB_DISTRICT_NAME
             AND SUB_DISTRICT_ID <> P_SUB_DISTRICT_ID;

      IF (C_COUNT > 0)
      THEN
         P_RETURNMSG := 'SUBDISTRICT FOUND';
      ELSE
         UPDATE TBL_SUB_DISTRICT
            SET SUB_DISTRICT_NAME = P_SUB_DISTRICT_NAME,
                DISTRICT_ID = P_DISTRICT_ID,
                MODIFIER_ID = P_MODIFIER_ID,
                MODIFICATION_DATE = P_MODIFICATION_DATE
          WHERE SUB_DISTRICT_ID = P_SUB_DISTRICT_ID;

         P_RETURNMSG := 'SUBDISTRICT UPDATED';
      END IF;
   END SUBDISTRICT_UPDATE;



   PROCEDURE TRANSCATION_HISTORY_DETAILS (P_VEH_REG_NO      IN     NVARCHAR2,
                                          P_RESIDENT_ID     IN     NVARCHAR2,
                                          P_VEHICLE_RC_NO   IN     NVARCHAR2,
                                          CUR_OUT              OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT AH.ENTRY_ID,
                CA.RESIDENT_ID,
                CV.VEH_REG_NO,
                CV.VEHICLE_RC_NO,
                (CASE AH.TRANSACTION_ID
                    WHEN 0 THEN AH.ENTRY_ID
                    ELSE AH.TRANSACTION_ID
                 END)
                   TRANSACTION_ID,
                AH.TRANSACTION_TYPE,
                (CASE AH.TRANSACTION_TYPE
                    WHEN 1 THEN 'Sale'
                    WHEN 2 THEN 'Top-Up'
                    WHEN 3 THEN 'Refund'
                    WHEN 4 THEN 'Charge'
                    ELSE 'Unknown'
                 END)
                   TRANSACTION_TYPE_NAME,
                AH.CREATION_DATE,
                AH.AMOUNT,
                L.LANE_NAME,
                P.PLAZA_NAME
           FROM TBL_CUSTOMER_ACCOUNT CA
                LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                   ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
                   ON     CA.ACCOUNT_ID = AH.ACCOUNT_ID
                      AND AH.CUSTOMER_VEHICLE_ENTRY_ID = CV.ENTRY_ID
                LEFT OUTER JOIN TBL_TRANSACTION T
                   ON AH.TRANSACTION_ID = T.TRANSACTION_ID
                LEFT OUTER JOIN TBL_LANE L
                   ON T.LANE_ID = L.LANE_ID
                LEFT OUTER JOIN TBL_PLAZA P
                   ON T.PLAZA_ID = P.PLAZA_ID
          WHERE     LOWER (CV.VEH_REG_NO) = LOWER (P_VEH_REG_NO)
                AND LOWER (CV.VEHICLE_RC_NO) = LOWER (P_VEHICLE_RC_NO)
                AND LOWER (CA.RESIDENT_ID) = LOWER (P_RESIDENT_ID);
   END TRANSCATION_HISTORY_DETAILS;



   PROCEDURE TRANS_HISTORY_DETAILS_PAGING (
      P_VEH_REG_NO      IN     NVARCHAR2,
      P_RESIDENT_ID     IN     NVARCHAR2,
      P_VEHICLE_RC_NO   IN     NVARCHAR2,
      P_PAGE_INDEX      IN     NUMBER,
      P_PAGE_SIZE       IN     NUMBER,
      CUR_OUT              OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_TRANS_HISTORY
              AS (  SELECT ROW_NUMBER () OVER (ORDER BY AH.CREATION_DATE DESC)
                              AS ROWNUMBER,
                           AH.ENTRY_ID,
                           CA.RESIDENT_ID,
                           CV.VEH_REG_NO,
                           CV.VEHICLE_RC_NO,
                           (CASE AH.TRANSACTION_ID
                               WHEN 0 THEN AH.ENTRY_ID
                               ELSE AH.TRANSACTION_ID
                            END)
                              TRANSACTION_ID,
                           AH.TRANSACTION_TYPE,
                           (CASE AH.TRANSACTION_TYPE
                               WHEN 1 THEN 'Sale'
                               WHEN 2 THEN 'Top-Up'
                               WHEN 3 THEN 'Refund'
                               WHEN 4 THEN 'Charge'
                               ELSE 'Unknown'
                            END)
                              TRANSACTION_TYPE_NAME,
                           AH.CREATION_DATE,
                           AH.AMOUNT,
                           L.LANE_NAME,
                           P.PLAZA_NAME
                      FROM TBL_CUSTOMER_ACCOUNT CA
                           LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                              ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                           LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
                              ON     CA.ACCOUNT_ID = AH.ACCOUNT_ID
                                 AND AH.CUSTOMER_VEHICLE_ENTRY_ID = CV.ENTRY_ID
                           LEFT OUTER JOIN TBL_TRANSACTION T
                              ON AH.TRANSACTION_ID = T.TRANSACTION_ID
                           LEFT OUTER JOIN TBL_LANE L
                              ON T.LANE_ID = L.LANE_ID
                           LEFT OUTER JOIN TBL_PLAZA P
                              ON T.PLAZA_ID = P.PLAZA_ID
                     WHERE     LOWER (CV.VEH_REG_NO) = LOWER (P_VEH_REG_NO)
                           AND LOWER (CV.VEHICLE_RC_NO) =
                                  LOWER (P_VEHICLE_RC_NO)
                           AND LOWER (CA.RESIDENT_ID) = LOWER (P_RESIDENT_ID)
                  ORDER BY AH.CREATION_DATE DESC)
           SELECT ROWNUMBER,
                  ENTRY_ID,
                  RESIDENT_ID,
                  VEH_REG_NO,
                  VEHICLE_RC_NO,
                  TRANSACTION_ID,
                  TRANSACTION_TYPE,
                  TRANSACTION_TYPE_NAME,
                  CREATION_DATE,
                  AMOUNT,
                  LANE_NAME,
                  PLAZA_NAME,
                  (SELECT COUNT (1) FROM CTE_TRANS_HISTORY) AS RECORDCOUNT
             FROM CTE_TRANS_HISTORY
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END TRANS_HISTORY_DETAILS_PAGING;



   PROCEDURE MEARGED_AUDIT_TRANSACTIONS_OLD (
      P_PARENT_TRANSACTION_ID      IN NUMBER,
      P_IKEENTRYID                 IN NUMBER,
      P_ANPRFRONTENTRYID           IN NUMBER,
      P_ANPRREARENTRYID            IN NUMBER,
      P_AUDITED_VRN                IN NVARCHAR2,
      P_AUDITED_VEHICLE_CLASS_ID   IN NUMBER,
      P_AUDITOR_ID                 IN NUMBER,
      P_TRANS_STATUS               IN NUMBER)
   AS
   BEGIN
      --Update Child 1 Transaction



      IF (P_IKEENTRYID != 0)
      THEN
         UPDATE TBL_TRANSACTION
            SET AUDITOR_ID = P_AUDITOR_ID,
                AUDIT_STATUS = 1,
                AUDIT_DATE = SYSDATE,
                AUDITED_VRN = P_AUDITED_VRN,
                AUDITED_VEHICLE_CLASS_ID = P_AUDITED_VEHICLE_CLASS_ID,
                TRANS_STATUS = P_TRANS_STATUS,
                MEARGED_TRAN_ID = P_PARENT_TRANSACTION_ID
          WHERE     CT_ENTRY_ID = P_IKEENTRYID
                AND TRANSACTION_ID <> P_PARENT_TRANSACTION_ID;
      END IF;



      IF (P_ANPRFRONTENTRYID != 0)
      THEN
         UPDATE TBL_TRANSACTION
            SET AUDITOR_ID = P_AUDITOR_ID,
                AUDIT_STATUS = 1,
                AUDIT_DATE = SYSDATE,
                AUDITED_VRN = P_AUDITED_VRN,
                AUDITED_VEHICLE_CLASS_ID = P_AUDITED_VEHICLE_CLASS_ID,
                TRANS_STATUS = P_TRANS_STATUS,
                MEARGED_TRAN_ID = P_PARENT_TRANSACTION_ID
          WHERE     NF_ENTRY_ID_FRONT = P_ANPRFRONTENTRYID
                AND TRANSACTION_ID <> P_PARENT_TRANSACTION_ID;
      END IF;



      IF (P_ANPRREARENTRYID != 0)
      THEN
         UPDATE TBL_TRANSACTION
            SET AUDITOR_ID = P_AUDITOR_ID,
                AUDIT_STATUS = 1,
                AUDIT_DATE = SYSDATE,
                AUDITED_VRN = P_AUDITED_VRN,
                AUDITED_VEHICLE_CLASS_ID = P_AUDITED_VEHICLE_CLASS_ID,
                TRANS_STATUS = P_TRANS_STATUS,
                MEARGED_TRAN_ID = P_PARENT_TRANSACTION_ID
          WHERE     NF_ENTRY_ID_REAR = P_ANPRREARENTRYID
                AND TRANSACTION_ID <> P_PARENT_TRANSACTION_ID;
      END IF;
   END MEARGED_AUDIT_TRANSACTIONS_OLD;

   PROCEDURE MEARGED_AUDIT_TRANSACTIONS (P_PARENT_TRANSACTION_ID   IN NUMBER,
                                         P_CHILD_TRANSACTION_ID    IN NUMBER,
                                         P_TRANS_STATUS            IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_TRANSACTION
         SET MEARGED_TRAN_ID = P_PARENT_TRANSACTION_ID
       WHERE TRANSACTION_ID = P_CHILD_TRANSACTION_ID;
   END MEARGED_AUDIT_TRANSACTIONS;



   PROCEDURE TOPUP_TRANS_LAZYLOAD (P_PAGE_INDEX   IN     NUMBER,
                                   P_PAGE_SIZE    IN     NUMBER,
                                   CUR_OUT           OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH TRANS_TOPUP
              AS (  SELECT ROW_NUMBER () OVER (ORDER BY CREATION_DATE DESC)
                              AS ROWNUMBER,
                           ENTRY_ID,
                           ACCOUNT_ID,
                           CUSTOMER_VEHICLE_ENTRY_ID,
                           AMOUNT,
                           CREATION_DATE
                      FROM TBL_ACCOUNT_HISTORY
                     WHERE     TRANSACTION_TYPE = 2
                           AND NVL (CUSTOMER_VEHICLE_ENTRY_ID, 0) <> 0
                           AND ROWNUM <= (P_PAGE_INDEX * P_PAGE_SIZE)
                  ORDER BY CREATION_DATE DESC)
           SELECT ROWNUMBER,
                  T.ENTRY_ID,
                  TO_CHAR (T.CREATION_DATE, 'DD-Mon-YYYY HH:MI:SS AM')
                     CREATION_DATE,
                  CV.VEH_REG_NO,
                  VC.VEHICLE_CLASS_NAME,
                  CA.FIRST_NAME,
                  T.AMOUNT
             FROM TRANS_TOPUP T
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON T.ACCOUNT_ID = CA.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                     ON T.CUSTOMER_VEHICLE_ENTRY_ID = CV.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID
            WHERE ROWNUMBER BETWEEN (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1
                                AND   (  ( (P_PAGE_INDEX - 1) * P_PAGE_SIZE + 1)
                                       + P_PAGE_SIZE)
                                    - 1
         ORDER BY ROWNUMBER ASC;
   END TOPUP_TRANS_LAZYLOAD;

   PROCEDURE TOPUP_TRANS_FILTER (P_FILTER IN NVARCHAR2, CUR_OUT OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT ROW_NUMBER () OVER (ORDER BY T.CREATION_DATE DESC) AS ROWNUMBER,
                  T.ENTRY_ID,
                  T.F_CREATION_DATE, 
                  T.CREATION_DATE,
                  CV.VEH_REG_NO,
                  VC.VEHICLE_CLASS_NAME,
                  CA.FIRST_NAME,
                  T.AMOUNT
             FROM TRANS_TOPUP T
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON T.ACCOUNT_ID = CA.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_CUSTOMER_VEHICLE CV
                     ON T.CUSTOMER_VEHICLE_ENTRY_ID = CV.ENTRY_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON CV.VEHICLE_CLASS_ID = VC.VEHICLE_CLASS_ID '
         || P_FILTER
         || 'ORDER BY T.CREATION_DATE DESC';

      OPEN CUR_OUT FOR SQLQUERY;
   END TOPUP_TRANS_FILTER;

   PROCEDURE CUSTOMERACCOUNT_FILTERED (P_FILTER   IN     NVARCHAR2,
                                       CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT ROWNUM AS ROWNUMBER,
       ACCOUNT_ID,
       FIRST_NAME,
       MOB_NUMBER,
       EMAIL_ID,
       ADDRESS,
       CUSTOMER_IMAGE_PATH,
       RESIDENT_ID,
       RESIDENTIDCARDIMAGE
  FROM (  SELECT CA.ACCOUNT_ID,
                 CA.FIRST_NAME,
                 CA.MOB_NUMBER,
                 CA.EMAIL_ID,
                 CA.ADDRESS,
                 CA.CUSTOMER_IMAGE_PATH,
                 CA.RESIDENT_ID,
                 CA.RESIDENTIDCARDIMAGE,
                 MIN (CV.ENTRY_ID) AS VEHICLE_ID
            FROM    TBL_CUSTOMER_ACCOUNT CA
                 LEFT OUTER JOIN
                    TBL_CUSTOMER_VEHICLE CV
                 ON CA.ACCOUNT_ID = CV.ACCOUNT_ID '
         || P_FILTER
         || ' GROUP BY CA.ACCOUNT_ID,
                 CA.FIRST_NAME,
                 CA.MOB_NUMBER,
                 CA.EMAIL_ID,
                 CA.ADDRESS,
                 CA.CUSTOMER_IMAGE_PATH,
                 CA.RESIDENT_ID,
                 CA.RESIDENTIDCARDIMAGE,
                 CA.CREATION_DATE,
                 CA.MODIFICATION_DATE
        ORDER BY CA.CREATION_DATE DESC, CA.MODIFICATION_DATE DESC)';

      OPEN CUR_OUT FOR SQLQUERY;
   END CUSTOMERACCOUNT_FILTERED;



   PROCEDURE CUSTOMERVEHICLE_FILTERED (P_FILTER   IN     NVARCHAR2,
                                       CUR_OUT       OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (5000);
   BEGIN
      SQLQUERY :=
            ' SELECT ROW_NUMBER ()
          OVER (ORDER BY CV.CREATION_DATE DESC, CV.MODIFICATION_DATE DESC)
          AS ROWNUMBER,
       CV.TMS_ID,
       CV.ENTRY_ID,
       CV.ACCOUNT_ID,
       CV.VEH_REG_NO,
       CV.TAG_ID,
       CV.VEHICLE_CLASS_ID,
       VC.VEHICLE_CLASS_NAME,
       CV.CREATION_DATE,
       CV.MODIFICATION_DATE,
       CV.MODIFIED_BY,
       CV.TRANSFER_STATUS,
       CV.VEHICLE_RC_NO,
       CV.OWNER_NAME,
       CV.OWNER_ADDRESS,
       CV.BRAND,
       CV.VEHICLE_TYPE,
       CV.VEHICLE_CATEGORY,
       CV.MODEL_NO,
       CV.MANUFACTURING_YEAR,
       CV.CYCLINDER_CAPACITY,
       CV.FRAME_NUMBER,
       CV.ENGINE_NUMBER,
       CV.VEHICLE_COLOR,
       CV.FUEL_TYPE,
       CV.LICENCE_PLATE_COLOR,
       CV.REGISTRATION_YEAR,
       CV.VEHICLE_OWNERSHIP_NO,
       CV.LOCATION_CODE,
       CV.REG_QUEUE_NO,
       CV.VEHICLEIMAGE_FRONT,
       CV.VEHICLEIMAGE_REAR,
       CV.VEHICLEIMAGE_RIGHT,
       CV.VEHICLEIMAGE_LEFT,
       CV.VEHICLE_RC_NO_PATH,
       CV.EXCEPTION_FLAG,
       CV.STATUS,
       CV.VALID_UNTIL,
       CV.TID_FRONT,
       CV.TID_REAR,
       CV.ACCOUNT_BALANCE,
       CV.REGISTRATION_THROUGH,
       CV.IS_DOC_VERIFIED,
       CV.QUEUE_STATUS,
       CA.FIRST_NAME AS CUSTOMER_NAME,
       CA.ACCOUNT_ID,
       CA.MOB_NUMBER,
       CA.EMAIL_ID,
       CA.ADDRESS,
       CA.CUSTOMER_IMAGE_PATH,
       CA.RESIDENT_ID,
       CA.RESIDENTIDCARDIMAGE
  FROM TBL_CUSTOMER_VEHICLE CV
       LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
          ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
       LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
          ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID'
         || P_FILTER
         || ' ORDER BY CV.CREATION_DATE DESC, CV.MODIFICATION_DATE DESC';

      OPEN CUR_OUT FOR SQLQUERY;
   END CUSTOMERVEHICLE_FILTERED;

   PROCEDURE ACTIVE_ANPR_GET (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         SELECT ANPR_NAME
           FROM TBL_ANPR_SYSTEM
          WHERE STATUS = 1;
   END ACTIVE_ANPR_GET;

   PROCEDURE VEHICLE_BALANCE_REPORT (P_VEHICLE_ID   IN     NUMBER,
                                     P_MONTH_ID     IN     NUMBER,
                                     P_YEAR_ID      IN     NUMBER,
                                     P_PMONTH_ID    IN     NUMBER,
                                     P_PYEAR_ID     IN     NUMBER,
                                     CUR_OUT           OUT T_CURSOR)
   IS
      C_COUNT   NUMBER;
      C_BAL     NUMBER;
   BEGIN
      SELECT COUNT (*)
        INTO C_COUNT
        FROM TBL_ACCOUNT_HISTORY
       WHERE     CUSTOMER_VEHICLE_ENTRY_ID = P_VEHICLE_ID
             AND TO_CHAR (CREATION_DATE, 'MM') = P_MONTH_ID
             AND TO_CHAR (CREATION_DATE, 'YYYY') = P_YEAR_ID;


      SELECT SUM (AMOUNT)
        INTO C_BAL
        FROM (SELECT NVL (
                        SUM (
                           (CASE TRANSACTION_TYPE
                               WHEN 1 THEN AMOUNT
                               WHEN 2 THEN AMOUNT
                               WHEN 3 THEN -1 * AMOUNT
                               WHEN 4 THEN -1 * AMOUNT
                            END)),
                        0)
                        AMOUNT
                FROM TBL_ACCOUNT_HISTORY
               WHERE     CUSTOMER_VEHICLE_ENTRY_ID = P_VEHICLE_ID
                     AND TO_CHAR (CREATION_DATE, 'MM') = P_PMONTH_ID
                     AND TO_CHAR (CREATION_DATE, 'YYYY') = P_PYEAR_ID
              UNION ALL
              SELECT NVL (
                        SUM (
                           (CASE TRANSACTION_TYPE
                               WHEN 1 THEN AMOUNT
                               WHEN 2 THEN AMOUNT
                               WHEN 3 THEN -1 * AMOUNT
                               WHEN 4 THEN -1 * AMOUNT
                            END)),
                        0)
                        AMOUNT
                FROM TBL_ACCOUNT_HISTORY
               WHERE     CUSTOMER_VEHICLE_ENTRY_ID = P_VEHICLE_ID
                     AND TO_CHAR (CREATION_DATE, 'MM') = P_MONTH_ID
                     AND TO_CHAR (CREATION_DATE, 'YYYY') = P_YEAR_ID) TAB1;

      OPEN CUR_OUT FOR
           SELECT ROWNUMBER,
                  CREATION_DATE,
                  TRANSACTION_ID,
                  TRANSACTION_TYPE,
                  PLAZA_NAME,
                  LANE_ID,
                  AMOUNT,
                  OPENING_BALANCE,
                  CLOSING_BALANCE,
                  FRONT_IMAGE,
                  FRONT_VIDEO_URL,
                  REAR_IMAGE,
                  REAR_VIDEO_URL
             FROM (SELECT ROWNUMBER,
                          CREATION_DATE,
                          TRANSACTION_ID,
                          TRANSACTION_TYPE,
                          PLAZA_NAME,
                          LANE_ID,
                          AMOUNT,
                          OPENING_BALANCE,
                          CLOSING_BALANCE,
                          FRONT_IMAGE,
                          FRONT_VIDEO_URL,
                          REAR_IMAGE,
                          REAR_VIDEO_URL
                     FROM (SELECT 1 AS ROWNUMBER,
                                  NULL AS CREATION_DATE,
                                  NULL TRANSACTION_ID,
                                  'Beginning' TRANSACTION_TYPE,
                                  NULL AS PLAZA_NAME,
                                  NULL LANE_ID,
                                  NVL (
                                     SUM (
                                        (CASE TRANSACTION_TYPE
                                            WHEN 1 THEN AMOUNT
                                            WHEN 2 THEN AMOUNT
                                            WHEN 3 THEN -1 * AMOUNT
                                            WHEN 4 THEN -1 * AMOUNT
                                         END)),
                                     0)
                                     AMOUNT,
                                  0 AS OPENING_BALANCE,
                                  0 AS CLOSING_BALANCE,
                                  NULL AS FRONT_IMAGE,
                                  NULL AS FRONT_VIDEO_URL,
                                  NULL AS REAR_IMAGE,
                                  NULL AS REAR_VIDEO_URL
                             FROM TBL_ACCOUNT_HISTORY
                            WHERE     CUSTOMER_VEHICLE_ENTRY_ID = P_VEHICLE_ID
                                  AND TO_CHAR (CREATION_DATE, 'MM') =
                                         P_PMONTH_ID
                                  AND TO_CHAR (CREATION_DATE, 'YYYY') =
                                         P_PYEAR_ID) TABL1
                   UNION
                   SELECT ROWNUMBER,
                          CREATION_DATE,
                          TRANSACTION_ID,
                          TRANSACTION_TYPE,
                          PLAZA_NAME,
                          LANE_ID,
                          AMOUNT,
                          OPENING_BALANCE,
                          CLOSING_BALANCE,
                          FRONT_IMAGE,
                          FRONT_VIDEO_URL,
                          REAR_IMAGE,
                          REAR_VIDEO_URL
                     FROM (SELECT (  ROW_NUMBER ()
                                     OVER (
                                        ORDER BY AH.CREATION_DATE, AH.ENTRY_ID)
                                   + 1)
                                     AS ROWNUMBER,
                                  (CASE AH.TRANSACTION_ID
                                      WHEN 0
                                      THEN
                                         TO_CHAR (AH.CREATION_DATE,
                                                  'DD-Mon-YYYY HH:MI:SS AM')
                                      ELSE
                                         TO_CHAR (T.TRANSACTION_DATETIME,
                                                  'DD-Mon-YYYY HH:MI:SS AM')
                                   END)
                                     CREATION_DATE,
                                  (CASE AH.TRANSACTION_ID
                                      WHEN 0 THEN AH.ENTRY_ID
                                      ELSE AH.TRANSACTION_ID
                                   END)
                                     TRANSACTION_ID,
                                  (CASE AH.TRANSACTION_TYPE
                                      WHEN 1 THEN 'Sale'
                                      WHEN 2 THEN 'Top-Up'
                                      WHEN 3 THEN 'Refund'
                                      WHEN 4 THEN 'Charge'
                                   END)
                                     TRANSACTION_TYPE,
                                  P.PLAZA_NAME,
                                  T.LANE_ID,
                                  NVL (
                                     (CASE TRANSACTION_TYPE
                                         WHEN 1 THEN AH.AMOUNT
                                         WHEN 2 THEN AH.AMOUNT
                                         WHEN 3 THEN -1 * AH.AMOUNT
                                         WHEN 4 THEN -1 * AH.AMOUNT
                                      END),
                                     0)
                                     AMOUNT,
                                  AH.OPENING_BALANCE,
                                  AH.CLOSING_BALANCE,
                                  NFP.PLATE_THUMBNAIL AS FRONT_IMAGE,
                                  NFP.VIDEO_URL AS FRONT_VIDEO_URL,
                                  NFP1.PLATE_THUMBNAIL AS REAR_IMAGE,
                                  NFP1.VIDEO_URL AS REAR_VIDEO_URL
                             FROM TBL_ACCOUNT_HISTORY AH
                                  LEFT OUTER JOIN TBL_TRANSACTION T
                                     ON AH.TRANSACTION_ID = T.TRANSACTION_ID
                                  LEFT OUTER JOIN TBL_PLAZA P
                                     ON T.PLAZA_ID = P.PLAZA_ID
                                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP
                                     ON T.NF_ENTRY_ID_FRONT = NFP.ENTRY_ID
                                  LEFT OUTER JOIN TBL_NODEFLUX_PACKET NFP1
                                     ON T.NF_ENTRY_ID_REAR = NFP1.ENTRY_ID
                            WHERE     CUSTOMER_VEHICLE_ENTRY_ID = P_VEHICLE_ID
                                  AND TO_CHAR (AH.CREATION_DATE, 'MM') =
                                         P_MONTH_ID
                                  AND TO_CHAR (AH.CREATION_DATE, 'YYYY') =
                                         P_YEAR_ID) TABL2
                   UNION
                   SELECT ROWNUMBER,
                          CREATION_DATE,
                          TRANSACTION_ID,
                          TRANSACTION_TYPE,
                          PLAZA_NAME,
                          LANE_ID,
                          AMOUNT,
                          OPENING_BALANCE,
                          CLOSING_BALANCE,
                          FRONT_IMAGE,
                          FRONT_VIDEO_URL,
                          REAR_IMAGE,
                          REAR_VIDEO_URL
                     FROM (SELECT (C_COUNT + 2) AS ROWNUMBER,
                                  NULL AS CREATION_DATE,
                                  NULL TRANSACTION_ID,
                                  'Ending' TRANSACTION_TYPE,
                                  NULL AS PLAZA_NAME,
                                  NULL LANE_ID,
                                  C_BAL AS AMOUNT,
                                  0 AS OPENING_BALANCE,
                                  0 AS CLOSING_BALANCE,
                                  NULL AS FRONT_IMAGE,
                                  NULL AS FRONT_VIDEO_URL,
                                  NULL AS REAR_IMAGE,
                                  NULL AS REAR_VIDEO_URL
                             FROM TBL_ACCOUNT_HISTORY
                            WHERE     CUSTOMER_VEHICLE_ENTRY_ID = P_VEHICLE_ID
                                  AND TO_CHAR (CREATION_DATE, 'MM') =
                                         P_MONTH_ID
                                  AND TO_CHAR (CREATION_DATE, 'YYYY') =
                                         P_YEAR_ID) TABL3) TABL
         ORDER BY ROWNUMBER;
   END VEHICLE_BALANCE_REPORT;

   PROCEDURE CUSTOMER_VEHICLE_BALANCE (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT CV.TMS_ID,
                  CV.ENTRY_ID AS VEHICLEID,
                  AH.ENTRY_ID,
                  CV.ACCOUNT_ID,
                  CV.VEH_REG_NO,
                  CV.TAG_ID,
                  CV.VEHICLE_CLASS_ID,
                  VC.VEHICLE_CLASS_NAME,
                  CV.CREATION_DATE,
                  CV.MODIFICATION_DATE,
                  CV.MODIFIED_BY,
                  CV.TRANSFER_STATUS,
                  CV.VEHICLE_RC_NO,
                  CV.OWNER_NAME,
                  CV.OWNER_ADDRESS,
                  CV.BRAND,
                  CV.VEHICLE_TYPE,
                  CV.VEHICLE_CATEGORY,
                  CV.MODEL_NO,
                  CV.MANUFACTURING_YEAR,
                  CV.CYCLINDER_CAPACITY,
                  CV.FRAME_NUMBER,
                  CV.ENGINE_NUMBER,
                  CV.VEHICLE_COLOR,
                  CV.FUEL_TYPE,
                  (CASE CV.FUEL_TYPE
                      WHEN 1 THEN 'GASOLINE'
                      WHEN 2 THEN 'DIESEL'
                      WHEN 3 THEN 'ELECTRIC'
                      ELSE 'Unknown'
                   END)
                     FUEL_TYPE_NAME,
                  CV.LICENCE_PLATE_COLOR,
                  (CASE CV.LICENCE_PLATE_COLOR
                      WHEN 1 THEN 'BLACK'
                      WHEN 2 THEN 'BLUE'
                      WHEN 3 THEN 'GREEN'
                      WHEN 4 THEN 'RED'
                      WHEN 5 THEN 'WHITE'
                      WHEN 6 THEN 'YELLOW'
                      ELSE 'Unknown'
                   END)
                     LICENCE_PLATE_COLOR_NAME,
                  CV.REGISTRATION_YEAR,
                  CV.VEHICLE_OWNERSHIP_NO,
                  CV.LOCATION_CODE,
                  CV.REG_QUEUE_NO,
                  CV.VEHICLEIMAGE_FRONT,
                  CV.VEHICLEIMAGE_REAR,
                  CV.VEHICLEIMAGE_RIGHT,
                  CV.VEHICLEIMAGE_LEFT,
                  CV.VEHICLE_RC_NO_PATH,
                  CV.EXCEPTION_FLAG,
                  (CASE CV.EXCEPTION_FLAG
                      WHEN 1 THEN 'CHARGED'
                      WHEN 2 THEN 'NOT CHARGED'
                      WHEN 3 THEN 'BLACK LISTED'
                      ELSE 'Unknown'
                   END)
                     EXCEPTION_FLAG_NAME,
                  CV.STATUS,
                  CV.VALID_UNTIL,
                  CV.TID_FRONT,
                  CV.TID_REAR,
                  AH.CLOSING_BALANCE AS ACCOUNT_BALANCE,
                  CV.REGISTRATION_THROUGH,
                  CV.IS_DOC_VERIFIED,
                  CV.QUEUE_STATUS,
                  (CASE CV.QUEUE_STATUS
                      WHEN 1 THEN 'OPEN'
                      WHEN 2 THEN 'POSTPONED'
                      WHEN 3 THEN 'PROCESSED'
                      ELSE 'Unknown'
                   END)
                     QUEUE_STATUS_NAME,
                  CA.FIRST_NAME || ' ' || CA.LAST_NAME AS CUSTOMER_NAME,
                  CA.RESIDENT_ID,
                  CA.EMAIL_ID,
                  CA.MOB_NUMBER
             FROM TBL_CUSTOMER_VEHICLE CV
                  LEFT OUTER JOIN TBL_CUSTOMER_ACCOUNT CA
                     ON CA.ACCOUNT_ID = CV.ACCOUNT_ID
                  LEFT OUTER JOIN TBL_VEHICLE_CLASS VC
                     ON VC.VEHICLE_CLASS_ID = CV.VEHICLE_CLASS_ID
                  LEFT OUTER JOIN TBL_ACCOUNT_HISTORY AH
                     ON     CA.ACCOUNT_ID = AH.ACCOUNT_ID
                        AND CV.ENTRY_ID = AH.CUSTOMER_VEHICLE_ENTRY_ID
            WHERE NVL (AH.SENT_STATUS, 0) = 1 AND NVL (AH.AMOUNT, 0) <> 0
         ORDER BY AH.CREATION_DATE;
   END CUSTOMER_VEHICLE_BALANCE;

   PROCEDURE ACCOUNT_HISTORY_BALANCEUPDATE (P_ENTRY_ID      IN NUMBER,
                                            P_SENT_STATUS   IN NUMBER,
                                            P_RESPONSE      IN NVARCHAR2)
   AS
   BEGIN
      UPDATE TBL_ACCOUNT_HISTORY
         SET SENT_STATUS = P_SENT_STATUS, RESPONSE = P_RESPONSE
       WHERE ENTRY_ID = P_ENTRY_ID;
   END ACCOUNT_HISTORY_BALANCEUPDATE;

   PROCEDURE SMS_HISTORY_SENDALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM SMS_SEND_GATWAY
         ORDER BY CREATION_DATE;
   END SMS_HISTORY_SENDALL;

   PROCEDURE SMS_HISTORY_PENDIND_STATUS (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM SMS_PENDING_GATWAY_STATUS
         ORDER BY MESSAGE_SEND_TIME;
   END SMS_HISTORY_PENDIND_STATUS;

   PROCEDURE MOBILE_PENDING_NOTI_ALL (CUR_OUT OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
           SELECT *
             FROM MOBILE_PENDING_NOTIFICATION
         ORDER BY CREATION_DATE;
   END MOBILE_PENDING_NOTI_ALL;

   PROCEDURE MOBILE_NOTI_STATUS_UPDATE (P_ENTRY_ID      IN NUMBER,
                                        P_SENT_STATUS   IN NUMBER)
   AS
   BEGIN
      UPDATE TBL_SMS_COMM_HISTORY
         SET MOBILE_SEND_STATUS = P_SENT_STATUS
       WHERE ENTRY_ID = P_ENTRY_ID;
   END MOBILE_NOTI_STATUS_UPDATE;

   PROCEDURE MOBILE_CUST_STATUS_UPDATE (P_ACCOUNT_ID    IN NUMBER,
                                        P_SENT_STATUS   IN NUMBER,
                                        P_RESPONSE      IN NVARCHAR2)
   AS
   BEGIN
      UPDATE TBL_CUSTOMER_ACCOUNT
         SET MOBILE_SEND_STATUS = P_SENT_STATUS, MOBILE_RESPONSE = P_RESPONSE
       WHERE ACCOUNT_ID = P_ACCOUNT_ID;
   END MOBILE_CUST_STATUS_UPDATE;

   PROCEDURE TRAN_DEATILS (P_FILTER IN NVARCHAR2, CUR_OUT OUT T_CURSOR)
   IS
      SQLQUERY   VARCHAR2 (20000);
   BEGIN
      SQLQUERY :=
         ' SELECT *
  FROM TRANS_DEATILS ' || P_FILTER || '
ORDER BY CREATION_DATE DESC ';



      OPEN CUR_OUT FOR SQLQUERY;
   END TRAN_DEATILS;


   PROCEDURE TRAN_DEATILSALL (P_STARTDATE   IN     NVARCHAR2,
                              P_ENDDATE     IN     NVARCHAR2,
                              CUR_OUT          OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_DETAILS
              AS (SELECT TMS_ID,
                         PLAZA_ID,
                         LANE_ID,
                         TRANSACTION_ID,
                         VEHICLESPEED,
                         RFID_FRONT_ID,
                         RFID_FRONT_TIMESTAMP,
                         --RFID_FRONT_TAG_ID,
                         RFID_FRONT_CLASS_ID,
                         RFID_FRONT_VRN,
                         RFID_REAR_ID,
                         RFID_REAR_TIMESTAMP,
                         --RFID_REAR_TAG_ID,
                         RFID_REAR_CLASS_ID,
                         RFID_REAR_VRN,
                         ANPR_FRONT_ID,
                         ANPR_FRONT_VRN,
                         ANPR_FRONT_CLASS_ID,
                         --ANPR_FRONT_IMAGE,
                         --ANPR_FRONT_VIDEO_URL,
                         ANPR_FRONT_SPEED,
                         ANPR_REAR_ID,
                         ANPR_REAR_VRN,
                         ANPR_REAR_CLASS_ID,
                         --ANPR_REAR_IMAGE,
                         --ANPR_REAR_VIDEO_URL,
                         ANPR_REAR_SPEED,
                         IS_BALANCE_UPDATED,
                         IS_VIOLATION,
                         IS_REGISTERED,
                         AUDIT_STATUS,
                         AUDITOR_ID,
                         AUDIT_DATE,
                         AUDITED_VEHICLE_CLASS_ID,
                         AUDITED_VRN,
                         AMOUNT,
                         CLOSING_BALANCE,
                         ParentId,
                         TRANS_STATUS,
                         GATEWAY_RESPONSE_CODE,
                         OPERATOR_RESPONSE_CODE,
                         TRANSACTION_DATETIME,
                         CREATION_DATE,
                         CREATION_DATE SMS_CREATION,
                         MESSAGE_SEND_TIME,
                         MESSAGE_RECEIVE_TIME,
                         SECONDS
                    FROM TRANS_DEATILS
                   WHERE TRANSACTION_DATETIME BETWEEN TO_DATE (
                                                         P_STARTDATE,
                                                         'DD/MM/YYYY HH24:MI:SS')
                                                  AND TO_DATE (
                                                         P_ENDDATE,
                                                         'DD/MM/YYYY HH24:MI:SS')),
              CTE_REPORT
              AS (SELECT 1 AS SNO,
                         'TOTAL VEHICLE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                  UNION ALL
                  SELECT 2 AS SNO,
                         'TOTAL REGISTERED PASS' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE NVL (IS_REGISTERED, 0) = 1
                  UNION ALL
                  SELECT 3 AS SNO,
                         'TOTAL UNREGISTERED' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE NVL (IS_REGISTERED, 0) <> 1
                  UNION ALL
                  SELECT 4 AS SNO,
                         'TOTAL IKE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE (RFID_FRONT_ID > 0 OR RFID_REAR_ID > 0)
                  UNION ALL
                  SELECT 5 AS SNO,
                         'TOTAL FRONT IKE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE (RFID_FRONT_ID > 0)
                  UNION ALL
                  SELECT 6 AS SNO,
                         'TOTAL REAR IKE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE (RFID_REAR_ID > 0)
                  UNION ALL
                  SELECT 7 AS SNO,
                         'TOTAL ANPR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE (ANPR_FRONT_ID > 0 OR ANPR_REAR_ID > 0)
                  UNION ALL
                  SELECT 8 AS SNO,
                         'TOTAL UNIDENTIFIED VRN' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE (   UPPER (ANPR_FRONT_VRN) = 'UNIDENTIFIED'
                          OR UPPER (ANPR_REAR_VRN) = 'UNIDENTIFIED')
                  UNION ALL
                  SELECT 9 AS SNO,
                         'TOTAL FRONT UNIDENTIFIED VRN' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE UPPER (ANPR_FRONT_VRN) = 'UNIDENTIFIED'
                  UNION ALL
                  SELECT 10 AS SNO,
                         'TOTAL REAR UNIDENTIFIED VRN' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE UPPER (ANPR_REAR_VRN) = 'UNIDENTIFIED'
                  UNION ALL
                  SELECT 11 AS SNO,
                         'TOTAL DETECTED VRN' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE (   UPPER (ANPR_FRONT_VRN) <> 'UNIDENTIFIED'
                          OR UPPER (ANPR_REAR_VRN) <> 'UNIDENTIFIED')
                  UNION ALL
                  SELECT 12 AS SNO,
                         'TOTAL FRONT DETECTED VRN' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE UPPER (ANPR_FRONT_VRN) <> 'UNIDENTIFIED'
                  UNION ALL
                  SELECT 13 AS SNO,
                         'TOTAL REAR DETECTED VRN' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE UPPER (ANPR_REAR_VRN) <> 'UNIDENTIFIED'
                  UNION ALL
                  SELECT 14 AS SNO,
                         'TOTAL TWO-WHEELED -IKE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     (RFID_FRONT_ID > 0 OR RFID_REAR_ID > 0)
                         AND (   RFID_FRONT_CLASS_ID = 1
                              OR RFID_REAR_CLASS_ID = 1)
                  UNION ALL
                  SELECT 15 AS SNO,
                         'TOTAL SMALL -IKE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     (RFID_FRONT_ID > 0 OR RFID_REAR_ID > 0)
                         AND (   RFID_FRONT_CLASS_ID = 2
                              OR RFID_REAR_CLASS_ID = 2)
                  UNION ALL
                  SELECT 16 AS SNO,
                         'TOTAL MEDIUM -IKE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     (RFID_FRONT_ID > 0 OR RFID_REAR_ID > 0)
                         AND (   RFID_FRONT_CLASS_ID = 3
                              OR RFID_REAR_CLASS_ID = 3)
                  UNION ALL
                  SELECT 17 AS SNO,
                         'TOTAL LARGE -IKE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     (RFID_FRONT_ID > 0 OR RFID_REAR_ID > 0)
                         AND (   RFID_FRONT_CLASS_ID = 4
                              OR RFID_REAR_CLASS_ID = 4)
                  UNION ALL
                  SELECT 18 AS SNO,
                         'TOTAL TWO-WHEELED -IKE-FRONT' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE RFID_FRONT_ID > 0 AND RFID_FRONT_CLASS_ID = 1
                  UNION ALL
                  SELECT 19 AS SNO,
                         'TOTAL SMALL -IKE-FRONT' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE RFID_FRONT_ID > 0 AND RFID_FRONT_CLASS_ID = 2
                  UNION ALL
                  SELECT 20 AS SNO,
                         'TOTAL MEDIUM -IKE-FRONT' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE RFID_FRONT_ID > 0 AND RFID_FRONT_CLASS_ID = 3
                  UNION ALL
                  SELECT 21 AS SNO,
                         'TOTAL LARGE -IKE-FRONT' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE RFID_FRONT_ID > 0 AND RFID_FRONT_CLASS_ID = 4
                  UNION ALL
                  SELECT 22 AS SNO,
                         'TOTAL TWO-WHEELED -IKE-REAR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE RFID_REAR_ID > 0 AND RFID_REAR_CLASS_ID = 1
                  UNION ALL
                  SELECT 23 AS SNO,
                         'TOTAL SMALL -IKE-REAR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE RFID_REAR_ID > 0 AND RFID_REAR_CLASS_ID = 2
                  UNION ALL
                  SELECT 24 AS SNO,
                         'TOTAL MEDIUM -IKE-REAR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE RFID_REAR_ID > 0 AND RFID_REAR_CLASS_ID = 3
                  UNION ALL
                  SELECT 25 AS SNO,
                         'TOTAL LARGE -IKE-REAR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE RFID_REAR_ID > 0 AND RFID_REAR_CLASS_ID = 4
                  UNION ALL
                  SELECT 26 AS SNO,
                         'TOTAL TWO-WHEELED -ANPR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     (ANPR_FRONT_ID > 0 OR ANPR_REAR_ID > 0)
                         AND (   ANPR_FRONT_CLASS_ID = 1
                              OR ANPR_REAR_CLASS_ID = 1)
                  UNION ALL
                  SELECT 27 AS SNO,
                         'TOTAL SMALL -ANPR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     (ANPR_FRONT_ID > 0 OR ANPR_REAR_ID > 0)
                         AND (   ANPR_FRONT_CLASS_ID = 2
                              OR ANPR_REAR_CLASS_ID = 2)
                  UNION ALL
                  SELECT 28 AS SNO,
                         'TOTAL MEDIUM -ANPR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     (ANPR_FRONT_ID > 0 OR ANPR_REAR_ID > 0)
                         AND (   ANPR_FRONT_CLASS_ID = 3
                              OR ANPR_REAR_CLASS_ID = 3)
                  UNION ALL
                  SELECT 29 AS SNO,
                         'TOTAL LARGE -ANPR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     (ANPR_FRONT_ID > 0 OR ANPR_REAR_ID > 0)
                         AND (   ANPR_FRONT_CLASS_ID = 4
                              OR ANPR_REAR_CLASS_ID = 4)
                  UNION ALL
                  SELECT 30 AS SNO,
                         'TOTAL TWO-WHEELED -ANPR-FRONT' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE ANPR_FRONT_ID > 0 AND ANPR_FRONT_CLASS_ID = 1
                  UNION ALL
                  SELECT 31 AS SNO,
                         'TOTAL SMALL -ANPR-FRONT' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE ANPR_FRONT_ID > 0 AND ANPR_FRONT_CLASS_ID = 2
                  UNION ALL
                  SELECT 32 AS SNO,
                         'TOTAL MEDIUM -ANPR-FRONT' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE ANPR_FRONT_ID > 0 AND ANPR_FRONT_CLASS_ID = 3
                  UNION ALL
                  SELECT 33 AS SNO,
                         'TOTAL LARGE -ANPR-FRONT' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE ANPR_FRONT_ID > 0 AND ANPR_FRONT_CLASS_ID = 4
                  UNION ALL
                  SELECT 34 AS SNO,
                         'TOTAL TWO-WHEELED -ANPR-REAR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE ANPR_REAR_ID > 0 AND ANPR_REAR_CLASS_ID = 1
                  UNION ALL
                  SELECT 35 AS SNO,
                         'TOTAL SMALL -ANPR-REAR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE ANPR_REAR_ID > 0 AND ANPR_REAR_CLASS_ID = 2
                  UNION ALL
                  SELECT 36 AS SNO,
                         'TOTAL MEDIUM -ANPR-REAR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE ANPR_REAR_ID > 0 AND ANPR_REAR_CLASS_ID = 3
                  UNION ALL
                  SELECT 37 AS SNO,
                         'TOTAL LARGE -ANPR-REAR' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE ANPR_REAR_ID > 0 AND ANPR_REAR_CLASS_ID = 4
                  UNION ALL
                  SELECT 38 AS SNO,
                         'TOTAL AUTO CHARGED' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     NVL (IS_BALANCE_UPDATED, 0) = 1
                         AND NVL (AUDIT_STATUS, 0) <> 1
                  UNION ALL
                  SELECT 39 AS SNO,
                         'TOTAL NOT CHARGED' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     NVL (IS_BALANCE_UPDATED, 0) = 0
                         AND NVL (AUDIT_STATUS, 0) <> 1
                         AND NVL (IS_REGISTERED, 0) = 1
                  UNION ALL
                  SELECT 40 AS SNO,
                         'TOTAL MANUAL CHARGED' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     NVL (IS_BALANCE_UPDATED, 0) = 1
                         AND NVL (AUDIT_STATUS, 0) = 1
                         AND NVL (IS_BALANCE_UPDATED, 0) = 1
                  UNION ALL
                  SELECT 41 AS SNO,
                         'TOTAL REVIEWED' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE NVL (AUDIT_STATUS, 0) = 1
                  UNION ALL
                  SELECT 42 AS SNO,
                         'TOTAL VIOLATION' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE NVL (AUDIT_STATUS, 0) = 1 AND TRANS_STATUS = 3
                  UNION ALL
                  SELECT 43 AS SNO,
                         'TOTAL UNIDENTIFIED' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE NVL (AUDIT_STATUS, 0) = 1 AND TRANS_STATUS = 4
                  UNION ALL
                  SELECT 44 AS SNO,
                         'TOTAL MERGED' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE NVL (AUDIT_STATUS, 0) = 1 AND TRANS_STATUS = 2
                  UNION ALL
                  SELECT 45 AS SNO,
                         'TOTAL SMS SEND' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     NVL (GATEWAY_RESPONSE_CODE, 0) <> 0
                         AND AUDIT_STATUS <> 1
                  UNION ALL
                  SELECT 46 AS SNO,
                         'TOTAL SUCCESSFUL SEND' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     NVL (OPERATOR_RESPONSE_CODE, 0) <> 0
                         AND AUDIT_STATUS <> 1
                  UNION ALL
                  SELECT 47 AS SNO,
                         'TOTAL AVERAGE SMS TIME IN SECONDS' MESSAGE,
                         ROUND (NVL ( (  (SELECT SUM (SECONDS)
                                            FROM CTE_DETAILS
                                           WHERE SECONDS <> 0)
                                       / (SELECT COUNT (SECONDS)
                                            FROM CTE_DETAILS
                                           WHERE SECONDS <> 0)),
                                     0),
                                0)
                            AS TOTALVEHICLE
                    FROM DUAL
                  UNION ALL
                  SELECT 48 AS SNO,
                         'TOTAL REGISTERED VEHICLE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM TBL_CUSTOMER_VEHICLE
                  UNION ALL
                  SELECT 49 AS SNO,
                         'TOTAL PROCESSED VEHICLE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM TBL_CUSTOMER_VEHICLE
                   WHERE QUEUE_STATUS = 3
                  UNION ALL
                  SELECT 50 AS SNO,
                         'TOTAL CHARGEABLE VEHICLE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM TBL_CUSTOMER_VEHICLE
                   WHERE EXCEPTION_FLAG = 1
                  UNION ALL
                  SELECT 51 AS SNO,
                         'TOTAL NON CHARGEABLE VEHICLE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM TBL_CUSTOMER_VEHICLE
                   WHERE EXCEPTION_FLAG = 2
                  UNION ALL
                  SELECT 52 AS SNO,
                         'TOTAL BLACK LISTED VEHICLE' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM TBL_CUSTOMER_VEHICLE
                   WHERE EXCEPTION_FLAG = 3
                  UNION ALL
                  SELECT 53 AS SNO,
                         'TOTAL MANUAL SMS SEND' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     NVL (GATEWAY_RESPONSE_CODE, 0) <> 0
                         AND AUDIT_STATUS = 1
                  UNION ALL
                  SELECT 54 AS SNO,
                         'TOTAL MANUAL SUCCESSFUL SEND' MESSAGE,
                         COUNT (1) TOTALVEHICLE
                    FROM CTE_DETAILS
                   WHERE     NVL (OPERATOR_RESPONSE_CODE, 0) <> 0
                         AND AUDIT_STATUS = 1)
         SELECT *
           FROM CTE_REPORT;
   END TRAN_DEATILSALL;


   PROCEDURE TRAN_GETSTACKCHART (P_STARTDATE   IN     NVARCHAR2,
                                 CUR_OUT          OUT T_CURSOR)
   IS
   BEGIN
      OPEN CUR_OUT FOR
         WITH CTE_DETAILS
              AS (SELECT TMS_ID,
                         PLAZA_ID,
                         LANE_ID,
                         TRANSACTION_ID,
                         VEHICLESPEED,
                         RFID_FRONT_ID,
                         RFID_FRONT_TIMESTAMP,
                         --RFID_FRONT_TAG_ID,
                         RFID_FRONT_CLASS_ID,
                         RFID_FRONT_VRN,
                         RFID_REAR_ID,
                         RFID_REAR_TIMESTAMP,
                         --RFID_REAR_TAG_ID,
                         RFID_REAR_CLASS_ID,
                         RFID_REAR_VRN,
                         ANPR_FRONT_ID,
                         ANPR_FRONT_VRN,
                         ANPR_FRONT_CLASS_ID,
                         ANPR_FRONT_SPEED,
                         ANPR_REAR_ID,
                         ANPR_REAR_VRN,
                         ANPR_REAR_CLASS_ID,
                         ANPR_REAR_SPEED,
                         IS_BALANCE_UPDATED,
                         IS_VIOLATION,
                         IS_REGISTERED,
                         AUDIT_STATUS,
                         AUDITOR_ID,
                         AUDIT_DATE,
                         AUDITED_VEHICLE_CLASS_ID,
                         AUDITED_VRN,
                         AMOUNT,
                         CLOSING_BALANCE,
                         ParentId,
                         TRANS_STATUS,
                         GATEWAY_RESPONSE_CODE,
                         OPERATOR_RESPONSE_CODE,
                         TRANSACTION_DATETIME,
                         CREATION_DATE,
                         CREATION_DATE SMS_CREATION,
                         MESSAGE_SEND_TIME,
                         MESSAGE_RECEIVE_TIME,
                         SECONDS,
                         TRANSACTION_TYPE
                    FROM TRANS_DEATILS
                   WHERE TRANSACTION_DATETIME BETWEEN TO_DATE (P_STARTDATE,
                                                               'DD/MM/YYYY')-7
                                                  AND TO_DATE (P_STARTDATE,
                                                               'DD/MM/YYYY')-1),
              CTE_STACKCHARTREPORTDATA
              AS (SELECT 1 AS SNO,
                         'TOTAL VEHICLE PASS' MESSAGE,
                         COUNT (1) TOTALDETAILS
                    FROM CTE_DETAILS
                  UNION ALL
                  SELECT 2 AS SNO,
                         'TOTAL REGISTERED' MESSAGE,
                         COUNT (1) TOTALDETAILS
                    FROM CTE_DETAILS
                   WHERE NVL (IS_REGISTERED, 0) = 1
                  UNION ALL
                  SELECT 3 AS SNO,
                         'TOTAL UNREGISTERED' MESSAGE,
                         COUNT (1) TOTALDETAILS
                    FROM CTE_DETAILS
                   WHERE NVL (IS_REGISTERED, 0) <> 1
                  UNION ALL
                  SELECT 4 AS SNO,
                         'TOTAL UNIDENTIFIED VRN' MESSAGE,
                         COUNT (1) TOTALDETAILS
                    FROM CTE_DETAILS
                   WHERE (   UPPER (ANPR_FRONT_VRN) = 'UNIDENTIFIED'
                          OR UPPER (ANPR_REAR_VRN) = 'UNIDENTIFIED')
                  UNION ALL
                  SELECT 5 AS SNO,
                         'TOPUP_AMOUNT' MESSAGE,
                         NVL (SUM (AMOUNT), 0) TOTALDETAILS
                    FROM CTE_DETAILS
                   WHERE TRANSACTION_TYPE = 2
                  UNION ALL
                  SELECT 6 AS SNO,
                         'CHARGED_AMOUNT' MESSAGE,
                         NVL (SUM (AMOUNT), 0) TOTALDETAILS
                    FROM CTE_DETAILS
                   WHERE TRANSACTION_TYPE = 4)
         SELECT *
           FROM CTE_STACKCHARTREPORTDATA;
   END TRAN_GETSTACKCHART;
END MLFF_PACKAGE;
/
/