    select  EMPWH.WHID
           ,WH.WHNAME
      from  EMPWH
           ,WH
     where  EMPWH.EMPID = :EMPID
       and  EMPWH.WHID  = WH.WHID
       order by WH.WHNAME
