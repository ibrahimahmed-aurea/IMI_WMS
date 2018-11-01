select  PRTID
       ,PRT_DEVICE
	   ,PRT_TYPE
	   ,UPDDTM
  from  PRT
  where OMID = :OMID