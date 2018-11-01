        update  EMP
           set  LASTLOGONDTM = :LASTLOGONDTM
               ,COMPANY_ID = nvl(:COMPANY_ID, EMP.COMPANY_ID)
         where  EMPID = :EMPID
           