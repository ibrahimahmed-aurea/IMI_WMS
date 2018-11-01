create table EXT_MSG_OUT_DEPARTURE(
  MSG_IN_ID                    varchar2(35) /* MSG_IN.MSG_IN_ID                               */
 ,OPCODE                       varchar2(1)  /* .                                              */
 ,DepartureIdentity            varchar2(35) /* MSG_OUT_DEPARTURE.DEPARTUREIDENTITY            */
 ,DepartureIdentityReference   varchar2(35) /* MSG_OUT_DEPARTURE.DEPARTUREIDENTITYREFERENCE   */
 ,FromNodeIdentity             varchar2(35) /* MSG_OUT_DEPARTURE.FROMNODEIDENTITY             */
 ,DeliveryMethod               varchar2(17) /* MSG_OUT_DEPARTURE.DELIVERYMETHOD               */
 ,RouteIdentity                varchar2(17) /* MSG_OUT_DEPARTURE.ROUTEIDENTITY                */
 ,RouteDescription             varchar2(35) /* MSG_OUT_DEPARTURE.ROUTEDESCRIPTION             */
 ,CustomerOrderReleaseDateTime date         /* MSG_OUT_DEPARTURE.CUSTOMERORDERRELEASEDATETIME */
 ,CustomerOrderStopDateTime    date         /* MSG_OUT_DEPARTURE.CUSTOMERORDERSTOPDATETIME    */
 ,TransitStopDateTime          date         /* MSG_OUT_DEPARTURE.TRANSITSTOPDATETIME          */
 ,PlannedDepartureDateTime     date         /* MSG_OUT_DEPARTURE.PLANNEDDEPARTUREDATETIME     */
 ,CheckProductTransportType    varchar2(1)  /* MSG_OUT_DEPARTURE.CHECKPRODUCTTRANSPORTTYPE    */
 ,VehicleIdentity              varchar2(35) /* MSG_OUT_DEPARTURE.VEHICLEIDENTITY              */
 ,UPDDTM                       date         /* MSG_IN.UPDDTM                                  */
 ,PROID                        varchar2(35) /* MSG_IN.PROID                                   */
);

create table EXT_MSG_OUT_DEPARTURE_NODE(
  MSG_IN_ID                varchar2(35) /* MSG_IN.MSG_IN_ID                                */
 ,OPCODE                   varchar2(1)  /* .                                               */
 ,DepartureIdentity        varchar2(35) /* MSG_OUT_DEPARTURE_NODE.DEPARTUREIDENTITY        */
 ,SEQNUM                   number(5)    /* MSG_OUT_DEPARTURE_NODE.SEQNUM                   */
 ,ToNodeIdentity           varchar2(35) /* MSG_OUT_DEPARTURE_NODE.TONODEIDENTITY           */
 ,EstimatedArrivalDateTime date         /* MSG_OUT_DEPARTURE_NODE.ESTIMATEDARRIVALDATETIME */
 ,ReceiveTransitGoods      varchar2(1)  /* MSG_OUT_DEPARTURE_NODE.RECEIVETRANSITGOODS      */
 ,UPDDTM                   date         /* MSG_IN.UPDDTM                                   */
 ,PROID                    varchar2(35) /* MSG_IN.PROID                                    */
);

create table EXT_MSG_OUT_DEPARTURE_TRP(
  MSG_IN_ID                varchar2(35) /* MSG_IN.MSG_IN_ID                               */
 ,OPCODE                   varchar2(1)  /* .                                              */
 ,DepartureIdentity        varchar2(35) /* MSG_OUT_DEPARTURE_TRP.DEPARTUREIDENTITY        */
 ,ProductTransportIdentity varchar2(5)  /* MSG_OUT_DEPARTURE_TRP.PRODUCTTRANSPORTIDENTITY */
 ,UPDDTM                   date         /* MSG_IN.UPDDTM                                  */
 ,PROID                    varchar2(35) /* MSG_IN.PROID                                   */
);

create table EXT_MSG_OUT_DEPLOAD(
  MSG_IN_ID                    varchar2(35)   /* MSG_IN.MSG_IN_ID                             */
 ,OPCODE                       varchar2(1)    /* .                                            */
 ,DepartureLoadIdentity        varchar2(35)   /* MSG_OUT_DEPLOAD.DEPARTURELOADIDENTITY        */
 ,CustomerOrderSequence        number(10)     /* MSG_OUT_DEPLOAD.CUSTOMERORDERSEQUENCE        */
 ,DepartureIdentity            varchar2(35)   /* MSG_OUT_DEPLOAD.DEPARTUREIDENTITY            */
 ,RouteIdentity                varchar2(17)   /* MSG_OUT_DEPLOAD.ROUTEIDENTITY                */
 ,RouteDescription             varchar2(35)   /* MSG_OUT_DEPLOAD.ROUTEDESCRIPTION             */
 ,CustomerOrderReleaseDateTime date           /* MSG_OUT_DEPLOAD.CUSTOMERORDERRELEASEDATETIME */
 ,CustomerOrderStopDateTime    date           /* MSG_OUT_DEPLOAD.CUSTOMERORDERSTOPDATETIME    */
 ,PlannedDepartureDateTime     date           /* MSG_OUT_DEPLOAD.PLANNEDDEPARTUREDATETIME     */
 ,EstimatedArrivalDateTime     date           /* MSG_OUT_DEPLOAD.ESTIMATEDARRIVALDATETIME     */
 ,LastChainDepartureIdentity   varchar2(35)   /* MSG_OUT_DEPLOAD.LASTCHAINDEPARTUREIDENTITY   */
 ,WhyNoDepartureMessage        varchar2(2000) /* MSG_OUT_DEPLOAD.WHYNODEPARTUREMESSAGE        */
 ,UPDDTM                       date           /* MSG_IN.UPDDTM                                */
 ,PROID                        varchar2(35)   /* MSG_IN.PROID                                 */
);

create table EXT_MSG_OUT_MODIFY_DEPLOAD(
  MSG_IN_ID                    varchar2(35) /* MSG_IN.MSG_IN_ID                                    */
 ,OPCODE                       varchar2(1)  /* .                                                   */
 ,DepartureLoadIdentity        varchar2(35) /* MSG_OUT_MODIFY_DEPLOAD.DEPARTURELOADIDENTITY        */
 ,FromNodeIdentity             varchar2(35) /* MSG_OUT_MODIFY_DEPLOAD.FROMNODEIDENTITY             */
 ,DepartureIdentity            varchar2(35) /* MSG_OUT_MODIFY_DEPLOAD.DEPARTUREIDENTITY            */
 ,RouteIdentity                varchar2(17) /* MSG_OUT_MODIFY_DEPLOAD.ROUTEIDENTITY                */
 ,RouteDescription             varchar2(35) /* MSG_OUT_MODIFY_DEPLOAD.ROUTEDESCRIPTION             */
 ,CustomerOrderReleaseDateTime date         /* MSG_OUT_MODIFY_DEPLOAD.CUSTOMERORDERRELEASEDATETIME */
 ,CustomerOrderStopDateTime    date         /* MSG_OUT_MODIFY_DEPLOAD.CUSTOMERORDERSTOPDATETIME    */
 ,PlannedDepartureDateTime     date         /* MSG_OUT_MODIFY_DEPLOAD.PLANNEDDEPARTUREDATETIME     */
 ,EstimatedArrivalDateTime     date         /* MSG_OUT_MODIFY_DEPLOAD.ESTIMATEDARRIVALDATETIME     */
 ,UPDDTM                       date         /* MSG_IN.UPDDTM                                       */
 ,PROID                        varchar2(35) /* MSG_IN.PROID                                        */
);

create table EXT_MSG_OUT_REMOVE_DEP(
  MSG_IN_ID         varchar2(35) /* MSG_IN.MSG_IN_ID                     */
 ,OPCODE            varchar2(1)  /* .                                    */
 ,DepartureIdentity varchar2(35) /* MSG_OUT_REMOVE_DEP.DEPARTUREIDENTITY */
 ,UPDDTM            date         /* MSG_IN.UPDDTM                        */
 ,PROID             varchar2(35) /* MSG_IN.PROID                         */
);

create table EXT_MSG_OUT_REMOVE_DEP_NODE(
  MSG_IN_ID         varchar2(35) /* MSG_IN.MSG_IN_ID                          */
 ,OPCODE            varchar2(1)  /* .                                         */
 ,DepartureIdentity varchar2(35) /* MSG_OUT_REMOVE_DEP_NODE.DEPARTUREIDENTITY */
 ,SEQNUM            number(5)    /* MSG_OUT_REMOVE_DEP_NODE.SEQNUM            */
 ,UPDDTM            date         /* MSG_IN.UPDDTM                             */
 ,PROID             varchar2(35) /* MSG_IN.PROID                              */
);

create table EXT_MSG_OUT_REMOVE_DEP_TRP(
  MSG_IN_ID                varchar2(35) /* MSG_IN.MSG_IN_ID                                */
 ,OPCODE                   varchar2(1)  /* .                                               */
 ,DepartureIdentity        varchar2(35) /* MSG_OUT_REMOVE_DEP_TRP.DEPARTUREIDENTITY        */
 ,ProductTransportIdentity varchar2(5)  /* MSG_OUT_REMOVE_DEP_TRP.PRODUCTTRANSPORTIDENTITY */
 ,UPDDTM                   date         /* MSG_IN.UPDDTM                                   */
 ,PROID                    varchar2(35) /* MSG_IN.PROID                                    */
);

create table EXT_MSG_OUT_CONFIRM(
  MSG_IN_ID          varchar2(35)   /* MSG_IN.MSG_IN_ID                   */
 ,OPCODE             varchar2(1)    /* .                                  */
 ,Transaction_id     varchar2(35)   /* MSG_OUT_CONFIRM.TRANSACTION_ID     */
 ,Transaction_Status varchar2(1)    /* MSG_OUT_CONFIRM.TRANSACTION_STATUS */
 ,ErrCode            varchar2(12)   /* MSG_OUT_CONFIRM.ERRCODE            */
 ,Errmsg             varchar2(1024) /* MSG_OUT_CONFIRM.ERRMSG             */
 ,UPDDTM             date           /* MSG_IN.UPDDTM                      */
 ,PROID              varchar2(35)   /* MSG_IN.PROID                       */
);

