update EMP
       set   EMP.RECENT_NODE_ID = :NODE_ID
            ,EMP.LASTLOGONDTM = :LASTLOGONDTM
      where  EMP.EMPID   = :EMPID