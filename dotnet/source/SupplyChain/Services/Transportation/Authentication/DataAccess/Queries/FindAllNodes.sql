      select   NODE.NODE_ID,
               NODE.NODE_NAME
        from   NODE
	   where   NODE.NODE_TYPE = 'SF'
         order by NODE_ID