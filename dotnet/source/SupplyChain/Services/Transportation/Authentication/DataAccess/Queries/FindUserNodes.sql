select   NODE.NODE_ID,
               NODE.NODE_NAME
        from   NODE,
               EMP_NODE
        where  NODE.NODE_ID   = EMP_NODE.NODE_ID
          and  EMP_NODE.EMPID = :EMPID