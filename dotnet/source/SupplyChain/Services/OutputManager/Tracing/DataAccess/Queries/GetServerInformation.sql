select SYS_CONTEXT('USERENV','SERVER_HOST') SERVER_HOST
      ,logg_output.Get_Logg_Path LOGG_PATH
from DUAL