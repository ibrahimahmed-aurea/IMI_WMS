﻿select  OM_ADAPTER.ADAPTER_ID
       ,OM_ADAPTER_CONFIG.PARAM_NAME
       ,OM_ADAPTER_CONFIG.PARAM_VALUE
	   ,OM_ADAPTER.UPDDTM UPDDTMADT
	   ,OM_ADAPTER_CONFIG.UPDDTM UPDDTMCONF
  from  OM_ADAPTER
       ,OM_ADAPTER_CONFIG
  where OM_ADAPTER.OMID = :OMID
    and OM_ADAPTER_CONFIG.ADAPTER_ID(+) = OM_ADAPTER.ADAPTER_ID
    and OM_ADAPTER_CONFIG.OMID(+)       = OM_ADAPTER.OMID
  order by OM_ADAPTER.ADAPTER_ID