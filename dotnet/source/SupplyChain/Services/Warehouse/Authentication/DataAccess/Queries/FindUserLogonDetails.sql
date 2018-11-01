select 
      EMPID
      ,EMPNAME
      ,WHID
      ,LASTLOGONDTM
      ,COMPANY_ID
  from (
        select  EMP.EMPID,EMP.EMPNAME,EMPWH.WHID, EMP.LASTLOGONDTM, EMP.COMPANY_ID
          from  EMP
               ,EMPWH
         where  (EMP.MAPPED_EMPID = :MAPPED_EMPID or (NOT EXISTS(SELECT EMPID FROM EMP WHERE MAPPED_EMPID = :MAPPED_EMPID) and  EMP.EMPID = :EMPID))
           and  EMP.EMPID = EMPWH.EMPID(+)
           order by EMPWH.UPDDTM desc )
      where rownum < 2
