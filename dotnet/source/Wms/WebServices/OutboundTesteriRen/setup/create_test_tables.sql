create table EXT2_BALANCE_ANSWER(
  HAPIRCV_ID                varchar2(35) /* HAPIRCV.HAPIRCV_ID                            */
 ,OPCODE                    varchar2(1)  /* .                                             */
 ,LastBalanceAnswer         varchar2(1)  /* HAPI_BALANCE_ANSWER.LASTBALANCEANSWER         */
 ,WarehouseIdentity         varchar2(4)  /* HAPI_BALANCE_ANSWER.WAREHOUSEIDENTITY         */
 ,OwnerIdentity             varchar2(35) /* HAPI_BALANCE_ANSWER.OWNERIDENTITY             */
 ,ClientIdentity            varchar2(17) /* HAPI_BALANCE_ANSWER.CLIENTIDENTITY            */
 ,ProductNumber             varchar2(35) /* HAPI_BALANCE_ANSWER.PRODUCTNUMBER             */
 ,ProductionLotIdentity     varchar2(40) /* HAPI_BALANCE_ANSWER.PRODUCTIONLOTIDENTITY     */
 ,ProductionSubLotIdentity  varchar2(40) /* HAPI_BALANCE_ANSWER.PRODUCTIONSUBLOTIDENTITY  */
 ,MarketingLotIdentity      varchar2(20) /* HAPI_BALANCE_ANSWER.MARKETINGLOTIDENTITY      */
 ,QualityLotIdentity        varchar2(20) /* HAPI_BALANCE_ANSWER.QUALITYLOTIDENTITY        */
 ,PackageIdentity           varchar2(17) /* HAPI_BALANCE_ANSWER.PACKAGEIDENTITY           */
 ,InventoryStatusCode       varchar2(8)  /* HAPI_BALANCE_ANSWER.INVENTORYSTATUSCODE       */
 ,FreeQuantity              number(20,6) /* HAPI_BALANCE_ANSWER.FREEQUANTITY              */
 ,PickLocationQuantity      number(20,6) /* HAPI_BALANCE_ANSWER.PICKLOCATIONQUANTITY      */
 ,ReservedForReplenQuantity number(20,6) /* HAPI_BALANCE_ANSWER.RESERVEDFORREPLENQUANTITY */
 ,PickedQuantity            number(20,6) /* HAPI_BALANCE_ANSWER.PICKEDQUANTITY            */
 ,TopickQuantity            number(20,6) /* HAPI_BALANCE_ANSWER.TOPICKQUANTITY            */
 ,CustomerReservedQuantity  number(20,6) /* HAPI_BALANCE_ANSWER.CUSTOMERRESERVEDQUANTITY  */
 ,MessageNumber             number(14)   /* HAPI_BALANCE_ANSWER.MESSAGENUMBER             */
 ,UPDDTM                    date         /* HAPIRCV.UPDDTM                                */
 ,PROID                     varchar2(35) /* HAPIRCV.PROID                                 */
);

create table EXT2_DLVRY_RECEIPT_HEAD(
  HAPIRCV_ID         varchar2(35) /* HAPIRCV.HAPIRCV_ID                         */
 ,OPCODE             varchar2(1)  /* .                                          */
 ,ClientIdentity     varchar2(17) /* HAPI_DLVRY_RECEIPT_HEAD.CLIENTIDENTITY     */
 ,DeliveryIdentity   number(8)    /* HAPI_DLVRY_RECEIPT_HEAD.DELIVERYIDENTITY   */
 ,ArrivalDateTime    date         /* HAPI_DLVRY_RECEIPT_HEAD.ARRIVALDATETIME    */
 ,WarehouseIdentity  varchar2(4)  /* HAPI_DLVRY_RECEIPT_HEAD.WAREHOUSEIDENTITY  */
 ,EmployeeIdentity   varchar2(8)  /* HAPI_DLVRY_RECEIPT_HEAD.EMPLOYEEIDENTITY   */
 ,ReceiveType        varchar2(2)  /* HAPI_DLVRY_RECEIPT_HEAD.RECEIVETYPE        */
 ,PackingSlipNumber  varchar2(35) /* HAPI_DLVRY_RECEIPT_HEAD.PACKINGSLIPNUMBER  */
 ,BillOfLadingNumber varchar2(35) /* HAPI_DLVRY_RECEIPT_HEAD.BILLOFLADINGNUMBER */
 ,VehicleIdentity    varchar2(17) /* HAPI_DLVRY_RECEIPT_HEAD.VEHICLEIDENTITY    */
 ,UPDDTM             date         /* HAPIRCV.UPDDTM                             */
 ,PROID              varchar2(35) /* HAPIRCV.PROID                              */
);

create table EXT2_DLVRY_RECEIPT_LINE(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                    */
 ,OPCODE                        varchar2(1)  /* .                                                     */
 ,InventoryStatusCode           varchar2(8)  /* HAPI_DLVRY_RECEIPT_LINE.INVENTORYSTATUSCODE           */
 ,PurchaseOrderLineSequence     number(3)    /* HAPI_DLVRY_RECEIPT_LINE.PURCHASEORDERLINESEQUENCE     */
 ,PurchaseOrderType             varchar2(2)  /* HAPI_DLVRY_RECEIPT_LINE.PURCHASEORDERTYPE             */
 ,CustomerOrderNumber           varchar2(35) /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERORDERNUMBER           */
 ,CustomerOrderSequence         number(3)    /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERORDERSEQUENCE         */
 ,CustomerOrderLinePosition     number(5)    /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERORDERLINEPOSITION     */
 ,CustomerOrderLineKitPosition  number(2)    /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERORDERLINEKITPOSITION  */
 ,CustomerOrderLineSequence     number(3)    /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERORDERLINESEQUENCE     */
 ,DespatchadvicenoticeIdentity  varchar2(35) /* HAPI_DLVRY_RECEIPT_LINE.DESPATCHADVICENOTICEIDENTITY  */
 ,ItemLoadIdentity              varchar2(35) /* HAPI_DLVRY_RECEIPT_LINE.ITEMLOADIDENTITY              */
 ,ExpiryDate                    date         /* HAPI_DLVRY_RECEIPT_LINE.EXPIRYDATE                    */
 ,ReturnsFinished               varchar2(1)  /* HAPI_DLVRY_RECEIPT_LINE.RETURNSFINISHED               */
 ,DecidedActionCode             varchar2(2)  /* HAPI_DLVRY_RECEIPT_LINE.DECIDEDACTIONCODE             */
 ,FromPartyId                   varchar2(35) /* HAPI_DLVRY_RECEIPT_LINE.FROMPARTYID                   */
 ,FromPartyQualifier            varchar2(3)  /* HAPI_DLVRY_RECEIPT_LINE.FROMPARTYQUALIFIER            */
 ,CustomerReturnOrderNumber     varchar2(35) /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERRETURNORDERNUMBER     */
 ,CustomerReturnOrderSequence   number(3)    /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERRETURNORDERSEQUENCE   */
 ,CustomerReturnOrderLinePos    number(5)    /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERRETURNORDERLINEPOS    */
 ,CustomerReturnOrderLineKitPos number(2)    /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERRETURNORDERLINEKITPOS */
 ,CustomerReturnOrderLineSeq    number(3)    /* HAPI_DLVRY_RECEIPT_LINE.CUSTOMERRETURNORDERLINESEQ    */
 ,DiscrepancyCode               varchar2(3)  /* HAPI_DLVRY_RECEIPT_LINE.DISCREPANCYCODE               */
 ,DiscrepancyActionCode         varchar2(2)  /* HAPI_DLVRY_RECEIPT_LINE.DISCREPANCYACTIONCODE         */
 ,ManufacturingDate             date         /* HAPI_DLVRY_RECEIPT_LINE.MANUFACTURINGDATE             */
 ,MeasureQualifier              varchar2(4)  /* HAPI_DLVRY_RECEIPT_LINE.MEASUREQUALIFIER              */
 ,ClientIdentity                varchar2(17) /* HAPI_DLVRY_RECEIPT_LINE.CLIENTIDENTITY                */
 ,DeliveryIdentity              number(8)    /* HAPI_DLVRY_RECEIPT_LINE.DELIVERYIDENTITY              */
 ,DeliveryidentityLine          number(5)    /* HAPI_DLVRY_RECEIPT_LINE.DELIVERYIDENTITYLINE          */
 ,ArrivalDateTime               date         /* HAPI_DLVRY_RECEIPT_LINE.ARRIVALDATETIME               */
 ,ProductIdentity               varchar2(35) /* HAPI_DLVRY_RECEIPT_LINE.PRODUCTIDENTITY               */
 ,PackageIdentity               varchar2(17) /* HAPI_DLVRY_RECEIPT_LINE.PACKAGEIDENTITY               */
 ,DeliveredQuantity             number(20,6) /* HAPI_DLVRY_RECEIPT_LINE.DELIVEREDQUANTITY             */
 ,MeasuredQuantity              number(20,6) /* HAPI_DLVRY_RECEIPT_LINE.MEASUREDQUANTITY              */
 ,ProductionLotIdentity         varchar2(40) /* HAPI_DLVRY_RECEIPT_LINE.PRODUCTIONLOTIDENTITY         */
 ,ProductionSubLotIdentity      varchar2(40) /* HAPI_DLVRY_RECEIPT_LINE.PRODUCTIONSUBLOTIDENTITY      */
 ,MarketingLotIdentity          varchar2(20) /* HAPI_DLVRY_RECEIPT_LINE.MARKETINGLOTIDENTITY          */
 ,QualityLotIdentity            varchar2(20) /* HAPI_DLVRY_RECEIPT_LINE.QUALITYLOTIDENTITY            */
 ,PurchaseOrderNumber           varchar2(35) /* HAPI_DLVRY_RECEIPT_LINE.PURCHASEORDERNUMBER           */
 ,PurchaseOrderSequence         number(3)    /* HAPI_DLVRY_RECEIPT_LINE.PURCHASEORDERSEQUENCE         */
 ,PurchaseOrderLinePosition     number(4)    /* HAPI_DLVRY_RECEIPT_LINE.PURCHASEORDERLINEPOSITION     */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                        */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                         */
);

create table EXT2_DLVRY_RECEIPT_PM(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                  */
 ,OPCODE                        varchar2(1)  /* .                                                   */
 ,PackingMaterialType           varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.PACKINGMATERIALTYPE           */
 ,VendorIdentity                varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.VENDORIDENTITY                */
 ,PurchaseOrderType             varchar2(2)  /* HAPI_DLVRY_RECEIPT_PM.PURCHASEORDERTYPE             */
 ,CustomerReturnOrderSequence   number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERSEQUENCE   */
 ,CustomerReturnOrderLinePos    number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERLINEPOS    */
 ,CustomerReturnOrderLineKitPos number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERLINEKITPOS */
 ,CustomerReturnOrderLineSeq    number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERLINESEQ    */
 ,ClientIdentity                varchar2(17) /* HAPI_DLVRY_RECEIPT_PM.CLIENTIDENTITY                */
 ,DeliveryIdentity              number(8)    /* HAPI_DLVRY_RECEIPT_PM.DELIVERYIDENTITY              */
 ,ArrivalDateTime               date         /* HAPI_DLVRY_RECEIPT_PM.ARRIVALDATETIME               */
 ,DeliveryidentityLine          number(5)    /* HAPI_DLVRY_RECEIPT_PM.DELIVERYIDENTITYLINE          */
 ,PackageIdentity               varchar2(17) /* HAPI_DLVRY_RECEIPT_PM.PACKAGEIDENTITY               */
 ,DeliveredQuantity             number(20,6) /* HAPI_DLVRY_RECEIPT_PM.DELIVEREDQUANTITY             */
 ,PurchaseOrderNumber           varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.PURCHASEORDERNUMBER           */
 ,PurchaseOrderSequence         number(3)    /* HAPI_DLVRY_RECEIPT_PM.PURCHASEORDERSEQUENCE         */
 ,CustomerOrderNumber           varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERORDERNUMBER           */
 ,CustomerOrderSequence         number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERORDERSEQUENCE         */
 ,FromPartyId                   varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.FROMPARTYID                   */
 ,FromPartyQualifier            varchar2(3)  /* HAPI_DLVRY_RECEIPT_PM.FROMPARTYQUALIFIER            */
 ,CustomerReturnOrderNumber     varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERNUMBER     */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                      */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                       */
);

create table EXT2_INSPECTION_RECEIPT_HEAD(
  HAPIRCV_ID            varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                varchar2(1)  /* .                                                  */
 ,ClientIdentity        varchar2(17) /* HAPI_INSPECTION_RECEIPT_HEAD.CLIENTIDENTITY        */
 ,WarehouseIdentity     varchar2(4)  /* HAPI_INSPECTION_RECEIPT_HEAD.WAREHOUSEIDENTITY     */
 ,EmployeeIdentity      varchar2(8)  /* HAPI_INSPECTION_RECEIPT_HEAD.EMPLOYEEIDENTITY      */
 ,ReturnhandleddateTime date         /* HAPI_INSPECTION_RECEIPT_HEAD.RETURNHANDLEDDATETIME */
 ,UPDDTM                date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                 varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT2_INSPECTION_RECEIPT_LINE(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                         */
 ,OPCODE                        varchar2(1)  /* .                                                          */
 ,ProductionSubLotIdentity      varchar2(40) /* HAPI_INSPECTION_RECEIPT_LINE.PRODUCTIONSUBLOTIDENTITY      */
 ,MarketingLotIdentity          varchar2(20) /* HAPI_INSPECTION_RECEIPT_LINE.MARKETINGLOTIDENTITY          */
 ,QualityLotIdentity            varchar2(20) /* HAPI_INSPECTION_RECEIPT_LINE.QUALITYLOTIDENTITY            */
 ,ItemLoadIdentity              varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.ITEMLOADIDENTITY              */
 ,ExpiryDate                    date         /* HAPI_INSPECTION_RECEIPT_LINE.EXPIRYDATE                    */
 ,FromPartyId                   varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.FROMPARTYID                   */
 ,FromPartyQualifier            varchar2(3)  /* HAPI_INSPECTION_RECEIPT_LINE.FROMPARTYQUALIFIER            */
 ,ShiptoCustomerIdentity        varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.SHIPTOCUSTOMERIDENTITY        */
 ,ShiptoCustomerQualifier       varchar2(3)  /* HAPI_INSPECTION_RECEIPT_LINE.SHIPTOCUSTOMERQUALIFIER       */
 ,ClientIdentity                varchar2(17) /* HAPI_INSPECTION_RECEIPT_LINE.CLIENTIDENTITY                */
 ,WorkOrderIdentity             varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.WORKORDERIDENTITY             */
 ,WorkOrderLine                 number(4)    /* HAPI_INSPECTION_RECEIPT_LINE.WORKORDERLINE                 */
 ,WorkOrderLineSeq              number(4)    /* HAPI_INSPECTION_RECEIPT_LINE.WORKORDERLINESEQ              */
 ,ProductIdentity               varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.PRODUCTIDENTITY               */
 ,PackageIdentity               varchar2(17) /* HAPI_INSPECTION_RECEIPT_LINE.PACKAGEIDENTITY               */
 ,InspectedQuantity             number(20,6) /* HAPI_INSPECTION_RECEIPT_LINE.INSPECTEDQUANTITY             */
 ,DecidedActionCode             varchar2(3)  /* HAPI_INSPECTION_RECEIPT_LINE.DECIDEDACTIONCODE             */
 ,CustomerReturnOrderNumber     varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.CUSTOMERRETURNORDERNUMBER     */
 ,CustomerReturnOrderSequence   number(3)    /* HAPI_INSPECTION_RECEIPT_LINE.CUSTOMERRETURNORDERSEQUENCE   */
 ,CustomerReturnOrderLinePos    number(4)    /* HAPI_INSPECTION_RECEIPT_LINE.CUSTOMERRETURNORDERLINEPOS    */
 ,CustomerReturnOrderLineKitPos number(3)    /* HAPI_INSPECTION_RECEIPT_LINE.CUSTOMERRETURNORDERLINEKITPOS */
 ,CustomerReturnOrderLineSeq    number(3)    /* HAPI_INSPECTION_RECEIPT_LINE.CUSTOMERRETURNORDERLINESEQ    */
 ,DeliveryIdentity              number(8)    /* HAPI_INSPECTION_RECEIPT_LINE.DELIVERYIDENTITY              */
 ,DeliveryidentityLine          number(5)    /* HAPI_INSPECTION_RECEIPT_LINE.DELIVERYIDENTITYLINE          */
 ,MeasuredQuantity              number(20,6) /* HAPI_INSPECTION_RECEIPT_LINE.MEASUREDQUANTITY              */
 ,InventoryStatusCode           varchar2(8)  /* HAPI_INSPECTION_RECEIPT_LINE.INVENTORYSTATUSCODE           */
 ,ProductionLotIdentity         varchar2(40) /* HAPI_INSPECTION_RECEIPT_LINE.PRODUCTIONLOTIDENTITY         */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                             */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                              */
);

create table EXT2_INSPECTION_RECEIPT_LINEPM(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                           */
 ,OPCODE                        varchar2(1)  /* .                                                            */
 ,ClientIdentity                varchar2(17) /* HAPI_INSPECTION_RECEIPT_LINEPM.CLIENTIDENTITY                */
 ,WorkOrderIdentity             varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINEPM.WORKORDERIDENTITY             */
 ,WorkOrderLine                 number(4)    /* HAPI_INSPECTION_RECEIPT_LINEPM.WORKORDERLINE                 */
 ,WorkOrderLineSeq              number(4)    /* HAPI_INSPECTION_RECEIPT_LINEPM.WORKORDERLINESEQ              */
 ,WorkOrderLinePmSeq            number(4)    /* HAPI_INSPECTION_RECEIPT_LINEPM.WORKORDERLINEPMSEQ            */
 ,PackingMaterialIdentity       varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINEPM.PACKINGMATERIALIDENTITY       */
 ,PackageIdentity               varchar2(17) /* HAPI_INSPECTION_RECEIPT_LINEPM.PACKAGEIDENTITY               */
 ,InspectedPmQuantity           number(20,6) /* HAPI_INSPECTION_RECEIPT_LINEPM.INSPECTEDPMQUANTITY           */
 ,DecidedActionCode             varchar2(3)  /* HAPI_INSPECTION_RECEIPT_LINEPM.DECIDEDACTIONCODE             */
 ,CustomerReturnOrderNumber     varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINEPM.CUSTOMERRETURNORDERNUMBER     */
 ,CustomerReturnOrderSequence   number(3)    /* HAPI_INSPECTION_RECEIPT_LINEPM.CUSTOMERRETURNORDERSEQUENCE   */
 ,CustomerReturnOrderLinePos    number(4)    /* HAPI_INSPECTION_RECEIPT_LINEPM.CUSTOMERRETURNORDERLINEPOS    */
 ,CustomerReturnOrderLineKitPos number(3)    /* HAPI_INSPECTION_RECEIPT_LINEPM.CUSTOMERRETURNORDERLINEKITPOS */
 ,CustomerReturnOrderLineSeq    number(3)    /* HAPI_INSPECTION_RECEIPT_LINEPM.CUSTOMERRETURNORDERLINESEQ    */
 ,DeliveryIdentity              number(8)    /* HAPI_INSPECTION_RECEIPT_LINEPM.DELIVERYIDENTITY              */
 ,DeliveryidentityLine          number(5)    /* HAPI_INSPECTION_RECEIPT_LINEPM.DELIVERYIDENTITYLINE          */
 ,FromPartyId                   varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINEPM.FROMPARTYID                   */
 ,FromPartyQualifier            varchar2(3)  /* HAPI_INSPECTION_RECEIPT_LINEPM.FROMPARTYQUALIFIER            */
 ,CustomerOrderNumber           varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINEPM.CUSTOMERORDERNUMBER           */
 ,CustomerOrderSequence         varchar2(3)  /* HAPI_INSPECTION_RECEIPT_LINEPM.CUSTOMERORDERSEQUENCE         */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                               */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                                */
);

create table EXT2_INVENTORY_CHANGE(
  HAPIRCV_ID                   varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                       varchar2(1)  /* .                                                  */
 ,MeasuredQty                  number(20,6) /* HAPI_INVENTORY_CHANGE.MEASUREDQTY                  */
 ,MeasuredQty2                 number(20,6) /* HAPI_INVENTORY_CHANGE.MEASUREDQTY2                 */
 ,ProductDate                  date         /* HAPI_INVENTORY_CHANGE.PRODUCTDATE                  */
 ,ProductDate2                 date         /* HAPI_INVENTORY_CHANGE.PRODUCTDATE2                 */
 ,VendorIdentity2              varchar2(35) /* HAPI_INVENTORY_CHANGE.VENDORIDENTITY2              */
 ,ProductNumber2               varchar2(35) /* HAPI_INVENTORY_CHANGE.PRODUCTNUMBER2               */
 ,PackageIdentity              varchar2(17) /* HAPI_INVENTORY_CHANGE.PACKAGEIDENTITY              */
 ,PackageIdentity2             varchar2(17) /* HAPI_INVENTORY_CHANGE.PACKAGEIDENTITY2             */
 ,Quantity                     number(20,6) /* HAPI_INVENTORY_CHANGE.QUANTITY                     */
 ,Quantity2                    number(20,6) /* HAPI_INVENTORY_CHANGE.QUANTITY2                    */
 ,InventoryStatusCode          varchar2(8)  /* HAPI_INVENTORY_CHANGE.INVENTORYSTATUSCODE          */
 ,InventoryStatusCode2         varchar2(8)  /* HAPI_INVENTORY_CHANGE.INVENTORYSTATUSCODE2         */
 ,ProductionLotIdentity        varchar2(40) /* HAPI_INVENTORY_CHANGE.PRODUCTIONLOTIDENTITY        */
 ,ProductionLotIdentity2       varchar2(40) /* HAPI_INVENTORY_CHANGE.PRODUCTIONLOTIDENTITY2       */
 ,ProductionSubLotIdentity     varchar2(40) /* HAPI_INVENTORY_CHANGE.PRODUCTIONSUBLOTIDENTITY     */
 ,ProductionSublotIdentity2    varchar2(40) /* HAPI_INVENTORY_CHANGE.PRODUCTIONSUBLOTIDENTITY2    */
 ,MarketingLotIdentity         varchar2(20) /* HAPI_INVENTORY_CHANGE.MARKETINGLOTIDENTITY         */
 ,MarketingLotIdentity2        varchar2(20) /* HAPI_INVENTORY_CHANGE.MARKETINGLOTIDENTITY2        */
 ,QualityLotIdentity           varchar2(20) /* HAPI_INVENTORY_CHANGE.QUALITYLOTIDENTITY           */
 ,QualityLotIdentity2          varchar2(20) /* HAPI_INVENTORY_CHANGE.QUALITYLOTIDENTITY2          */
 ,WarehouseIdentity            varchar2(4)  /* HAPI_INVENTORY_CHANGE.WAREHOUSEIDENTITY            */
 ,CustomerOrderNumber          varchar2(35) /* HAPI_INVENTORY_CHANGE.CUSTOMERORDERNUMBER          */
 ,CustomerOrderSequence        number(3)    /* HAPI_INVENTORY_CHANGE.CUSTOMERORDERSEQUENCE        */
 ,CustomerOrderLinePosition    number(6)    /* HAPI_INVENTORY_CHANGE.CUSTOMERORDERLINEPOSITION    */
 ,CustomerOrderLineKitPosition number(2)    /* HAPI_INVENTORY_CHANGE.CUSTOMERORDERLINEKITPOSITION */
 ,CustomerOrderLineSequence    number(3)    /* HAPI_INVENTORY_CHANGE.CUSTOMERORDERLINESEQUENCE    */
 ,ReasonCode                   varchar2(6)  /* HAPI_INVENTORY_CHANGE.REASONCODE                   */
 ,FreeText                     varchar2(45) /* HAPI_INVENTORY_CHANGE.FREETEXT                     */
 ,ClientIdentity               varchar2(17) /* HAPI_INVENTORY_CHANGE.CLIENTIDENTITY               */
 ,EmployeeId                   varchar2(8)  /* HAPI_INVENTORY_CHANGE.EMPLOYEEID                   */
 ,TimeStamp                    date         /* HAPI_INVENTORY_CHANGE.TIMESTAMP                    */
 ,ItemLoadIdentity             varchar2(35) /* HAPI_INVENTORY_CHANGE.ITEMLOADIDENTITY             */
 ,OperationCode                varchar2(1)  /* HAPI_INVENTORY_CHANGE.OPERATIONCODE                */
 ,InventoryChangeCode          varchar2(2)  /* HAPI_INVENTORY_CHANGE.INVENTORYCHANGECODE          */
 ,InventoryChangeText          varchar2(35) /* HAPI_INVENTORY_CHANGE.INVENTORYCHANGETEXT          */
 ,DespatchadviceIdentity       varchar2(35) /* HAPI_INVENTORY_CHANGE.DESPATCHADVICEIDENTITY       */
 ,PurchaseOrderNumber          varchar2(35) /* HAPI_INVENTORY_CHANGE.PURCHASEORDERNUMBER          */
 ,PurchaseOrderSequence        number(3)    /* HAPI_INVENTORY_CHANGE.PURCHASEORDERSEQUENCE        */
 ,PurchaseOrderLinePosition    number(6)    /* HAPI_INVENTORY_CHANGE.PURCHASEORDERLINEPOSITION    */
 ,PurchaseOrderLineSequence    number(3)    /* HAPI_INVENTORY_CHANGE.PURCHASEORDERLINESEQUENCE    */
 ,VendorIdentity               varchar2(35) /* HAPI_INVENTORY_CHANGE.VENDORIDENTITY               */
 ,OwnerIdentity                varchar2(35) /* HAPI_INVENTORY_CHANGE.OWNERIDENTITY                */
 ,OwnerIdentity2               varchar2(35) /* HAPI_INVENTORY_CHANGE.OWNERIDENTITY2               */
 ,ProductNumber                varchar2(35) /* HAPI_INVENTORY_CHANGE.PRODUCTNUMBER                */
 ,UPDDTM                       date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                        varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT2_PICK_RECEIPT_HEAD(
  HAPIRCV_ID               varchar2(35) /* HAPIRCV.HAPIRCV_ID                              */
 ,OPCODE                   varchar2(1)  /* .                                               */
 ,LastOndeparture          varchar2(1)  /* HAPI_PICK_RECEIPT_HEAD.LASTONDEPARTURE          */
 ,ClientIdentity           varchar2(17) /* HAPI_PICK_RECEIPT_HEAD.CLIENTIDENTITY           */
 ,CustomerOrderNumber      varchar2(35) /* HAPI_PICK_RECEIPT_HEAD.CUSTOMERORDERNUMBER      */
 ,CustomerOrderSequence    number(3)    /* HAPI_PICK_RECEIPT_HEAD.CUSTOMERORDERSEQUENCE    */
 ,CustomerOrderSubSequence number(2)    /* HAPI_PICK_RECEIPT_HEAD.CUSTOMERORDERSUBSEQUENCE */
 ,WarehouseIdentity        varchar2(4)  /* HAPI_PICK_RECEIPT_HEAD.WAREHOUSEIDENTITY        */
 ,UPDDTM                   date         /* HAPIRCV.UPDDTM                                  */
 ,PROID                    varchar2(35) /* HAPIRCV.PROID                                   */
);

create table EXT2_PICK_RECEIPT_HEAD_PM(
  HAPIRCV_ID               varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                   varchar2(1)  /* .                                                  */
 ,PackingMaterialType      varchar2(35) /* HAPI_PICK_RECEIPT_HEAD_PM.PACKINGMATERIALTYPE      */
 ,Quantity                 number(20,6) /* HAPI_PICK_RECEIPT_HEAD_PM.QUANTITY                 */
 ,ClientIdentity           varchar2(17) /* HAPI_PICK_RECEIPT_HEAD_PM.CLIENTIDENTITY           */
 ,CustomerOrderNumber      varchar2(35) /* HAPI_PICK_RECEIPT_HEAD_PM.CUSTOMERORDERNUMBER      */
 ,CustomerOrderSequence    number(3)    /* HAPI_PICK_RECEIPT_HEAD_PM.CUSTOMERORDERSEQUENCE    */
 ,CustomerOrderSubSequence number(2)    /* HAPI_PICK_RECEIPT_HEAD_PM.CUSTOMERORDERSUBSEQUENCE */
 ,UPDDTM                   date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                    varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT2_PICK_RECEIPT_LINE(
  HAPIRCV_ID                   varchar2(35) /* HAPIRCV.HAPIRCV_ID                                  */
 ,OPCODE                       varchar2(1)  /* .                                                   */
 ,CustomerOrderLineKitPosition number(2)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERLINEKITPOSITION */
 ,CustomerOrderLineSequence    number(3)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERLINESEQUENCE    */
 ,OwnerIdentity                varchar2(35) /* HAPI_PICK_RECEIPT_LINE.OWNERIDENTITY                */
 ,ProductIdentity              varchar2(35) /* HAPI_PICK_RECEIPT_LINE.PRODUCTIDENTITY              */
 ,PackageIdentity              varchar2(17) /* HAPI_PICK_RECEIPT_LINE.PACKAGEIDENTITY              */
 ,PickQuantity                 number(20,6) /* HAPI_PICK_RECEIPT_LINE.PICKQUANTITY                 */
 ,MeasuredQty                  number(20,6) /* HAPI_PICK_RECEIPT_LINE.MEASUREDQTY                  */
 ,RestCode                     varchar2(3)  /* HAPI_PICK_RECEIPT_LINE.RESTCODE                     */
 ,DepartureIdentity            varchar2(35) /* HAPI_PICK_RECEIPT_LINE.DEPARTUREIDENTITY            */
 ,DepartureWeekday             varchar2(1)  /* HAPI_PICK_RECEIPT_LINE.DEPARTUREWEEKDAY             */
 ,MarketingLotIdentity         varchar2(20) /* HAPI_PICK_RECEIPT_LINE.MARKETINGLOTIDENTITY         */
 ,QualityLotIdentity           varchar2(20) /* HAPI_PICK_RECEIPT_LINE.QUALITYLOTIDENTITY           */
 ,FreighterIdentity            varchar2(35) /* HAPI_PICK_RECEIPT_LINE.FREIGHTERIDENTITY            */
 ,ShipDate                     date         /* HAPI_PICK_RECEIPT_LINE.SHIPDATE                     */
 ,ConsignmentNote              varchar2(35) /* HAPI_PICK_RECEIPT_LINE.CONSIGNMENTNOTE              */
 ,PackingSlipIdentity          varchar2(20) /* HAPI_PICK_RECEIPT_LINE.PACKINGSLIPIDENTITY          */
 ,OrderedQuantity              number(20,6) /* HAPI_PICK_RECEIPT_LINE.ORDEREDQUANTITY              */
 ,UnderQuantity                number(20,6) /* HAPI_PICK_RECEIPT_LINE.UNDERQUANTITY                */
 ,OverQuantity                 number(20,6) /* HAPI_PICK_RECEIPT_LINE.OVERQUANTITY                 */
 ,ProductionLotIdentity        varchar2(40) /* HAPI_PICK_RECEIPT_LINE.PRODUCTIONLOTIDENTITY        */
 ,ProductionSubLotIdentity     varchar2(40) /* HAPI_PICK_RECEIPT_LINE.PRODUCTIONSUBLOTIDENTITY     */
 ,InventoryStatusCode          varchar2(8)  /* HAPI_PICK_RECEIPT_LINE.INVENTORYSTATUSCODE          */
 ,SerialNumber                 varchar2(21) /* HAPI_PICK_RECEIPT_LINE.SERIALNUMBER                 */
 ,ClientIdentity               varchar2(17) /* HAPI_PICK_RECEIPT_LINE.CLIENTIDENTITY               */
 ,CustomerOrderNumber          varchar2(35) /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERNUMBER          */
 ,CustomerOrderSequence        number(3)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERSEQUENCE        */
 ,CustomerOrderSubSequence     number(2)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERSUBSEQUENCE     */
 ,CustomerOrderLinePosition    number(5)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERLINEPOSITION    */
 ,UPDDTM                       date         /* HAPIRCV.UPDDTM                                      */
 ,PROID                        varchar2(35) /* HAPIRCV.PROID                                       */
);

create table EXT2_PICK_RECEIPT_LINE_PM(
  HAPIRCV_ID                   varchar2(35) /* HAPIRCV.HAPIRCV_ID                                     */
 ,OPCODE                       varchar2(1)  /* .                                                      */
 ,PackingMaterialType          varchar2(35) /* HAPI_PICK_RECEIPT_LINE_PM.PACKINGMATERIALTYPE          */
 ,Quantity                     number(20,6) /* HAPI_PICK_RECEIPT_LINE_PM.QUANTITY                     */
 ,ClientIdentity               varchar2(17) /* HAPI_PICK_RECEIPT_LINE_PM.CLIENTIDENTITY               */
 ,CustomerOrderNumber          varchar2(35) /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERNUMBER          */
 ,CustomerOrderSequence        number(3)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERSEQUENCE        */
 ,CustomerOrderSubSequence     number(2)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERSUBSEQUENCE     */
 ,CustomerOrderLinePosition    number(5)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERLINEPOSITION    */
 ,CustomerOrderLineKitPosition number(2)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERLINEKITPOSITION */
 ,CustomerOrderLineSequence    number(3)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERLINESEQUENCE    */
 ,UPDDTM                       date         /* HAPIRCV.UPDDTM                                         */
 ,PROID                        varchar2(35) /* HAPIRCV.PROID                                          */
);

create table EXT2_PICK_RECEIPT_SERVICE(
  HAPIRCV_ID               varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                   varchar2(1)  /* .                                                  */
 ,ClientIdentity           varchar2(17) /* HAPI_PICK_RECEIPT_SERVICE.CLIENTIDENTITY           */
 ,CustomerOrderNumber      varchar2(35) /* HAPI_PICK_RECEIPT_SERVICE.CUSTOMERORDERNUMBER      */
 ,CustomerOrderSequence    number(3)    /* HAPI_PICK_RECEIPT_SERVICE.CUSTOMERORDERSEQUENCE    */
 ,CustomerOrderSubSequence number(2)    /* HAPI_PICK_RECEIPT_SERVICE.CUSTOMERORDERSUBSEQUENCE */
 ,ServiceQualifier         varchar2(3)  /* HAPI_PICK_RECEIPT_SERVICE.SERVICEQUALIFIER         */
 ,ServiceCode              varchar2(17) /* HAPI_PICK_RECEIPT_SERVICE.SERVICECODE              */
 ,ServiceQty               number(20,6) /* HAPI_PICK_RECEIPT_SERVICE.SERVICEQTY               */
 ,UPDDTM                   date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                    varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT2_PICK_RECEIPT_TEXT(
  HAPIRCV_ID               varchar2(35)  /* HAPIRCV.HAPIRCV_ID                              */
 ,OPCODE                   varchar2(1)   /* .                                               */
 ,ClientIdentity           varchar2(17)  /* HAPI_PICK_RECEIPT_TEXT.CLIENTIDENTITY           */
 ,CustomerOrderNumber      varchar2(35)  /* HAPI_PICK_RECEIPT_TEXT.CUSTOMERORDERNUMBER      */
 ,CustomerOrderSequence    number(3)     /* HAPI_PICK_RECEIPT_TEXT.CUSTOMERORDERSEQUENCE    */
 ,CustomerOrderSubSequence number(2)     /* HAPI_PICK_RECEIPT_TEXT.CUSTOMERORDERSUBSEQUENCE */
 ,TextFunction             varchar2(3)   /* HAPI_PICK_RECEIPT_TEXT.TEXTFUNCTION             */
 ,Text                     varchar2(400) /* HAPI_PICK_RECEIPT_TEXT.TEXT                     */
 ,UPDDTM                   date          /* HAPIRCV.UPDDTM                                  */
 ,PROID                    varchar2(35)  /* HAPIRCV.PROID                                   */
);

create table EXT2_RETURNED_PM_HEAD(
  HAPIRCV_ID        varchar2(35)  /* HAPIRCV.HAPIRCV_ID                      */
 ,OPCODE            varchar2(1)   /* .                                       */
 ,Rpmid             number        /* HAPI_RETURNED_PM_HEAD.RPMID             */
 ,ClientIdentity    varchar2(17)  /* HAPI_RETURNED_PM_HEAD.CLIENTIDENTITY    */
 ,Customer_id       varchar2(35)  /* HAPI_RETURNED_PM_HEAD.CUSTOMER_ID       */
 ,Referense         varchar2(200) /* HAPI_RETURNED_PM_HEAD.REFERENSE         */
 ,ArrivalDateTime   date          /* HAPI_RETURNED_PM_HEAD.ARRIVALDATETIME   */
 ,WarehouseIdentity varchar2(4)   /* HAPI_RETURNED_PM_HEAD.WAREHOUSEIDENTITY */
 ,UPDDTM            date          /* HAPIRCV.UPDDTM                          */
 ,PROID             varchar2(35)  /* HAPIRCV.PROID                           */
);

create table EXT2_RETURNED_PM_LINE(
  HAPIRCV_ID      varchar2(35) /* HAPIRCV.HAPIRCV_ID                    */
 ,OPCODE          varchar2(1)  /* .                                     */
 ,Rpmid           number       /* HAPI_RETURNED_PM_LINE.RPMID           */
 ,Seqnum          number       /* HAPI_RETURNED_PM_LINE.SEQNUM          */
 ,ProductIdentity varchar2(35) /* HAPI_RETURNED_PM_LINE.PRODUCTIDENTITY */
 ,Quantity        number(20,6) /* HAPI_RETURNED_PM_LINE.QUANTITY        */
 ,UPDDTM          date         /* HAPIRCV.UPDDTM                        */
 ,PROID           varchar2(35) /* HAPIRCV.PROID                         */
);

create table EXT2_RETURN_RECEIPT_HEAD(
  HAPIRCV_ID             varchar2(35) /* HAPIRCV.HAPIRCV_ID                              */
 ,OPCODE                 varchar2(1)  /* .                                               */
 ,ClientIdentity         varchar2(17) /* HAPI_RETURN_RECEIPT_HEAD.CLIENTIDENTITY         */
 ,ReturnOrderNumber      varchar2(35) /* HAPI_RETURN_RECEIPT_HEAD.RETURNORDERNUMBER      */
 ,ReturnOrderSequence    number(3)    /* HAPI_RETURN_RECEIPT_HEAD.RETURNORDERSEQUENCE    */
 ,ReturnOrderSubSequence number(2)    /* HAPI_RETURN_RECEIPT_HEAD.RETURNORDERSUBSEQUENCE */
 ,WarehouseIdentity      varchar2(4)  /* HAPI_RETURN_RECEIPT_HEAD.WAREHOUSEIDENTITY      */
 ,ShipDate               date         /* HAPI_RETURN_RECEIPT_HEAD.SHIPDATE               */
 ,SpontaneousReturn      varchar2(1)  /* HAPI_RETURN_RECEIPT_HEAD.SPONTANEOUSRETURN      */
 ,VendorIdentity         varchar2(35) /* HAPI_RETURN_RECEIPT_HEAD.VENDORIDENTITY         */
 ,UPDDTM                 date         /* HAPIRCV.UPDDTM                                  */
 ,PROID                  varchar2(35) /* HAPIRCV.PROID                                   */
);

create table EXT2_RETURN_RECEIPT_HEAD_PM(
  HAPIRCV_ID             varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                 varchar2(1)  /* .                                                  */
 ,ClientIdentity         varchar2(17) /* HAPI_RETURN_RECEIPT_HEAD_PM.CLIENTIDENTITY         */
 ,ReturnOrderNumber      varchar2(35) /* HAPI_RETURN_RECEIPT_HEAD_PM.RETURNORDERNUMBER      */
 ,ReturnOrderSequence    number(3)    /* HAPI_RETURN_RECEIPT_HEAD_PM.RETURNORDERSEQUENCE    */
 ,ReturnOrderSubSequence number(2)    /* HAPI_RETURN_RECEIPT_HEAD_PM.RETURNORDERSUBSEQUENCE */
 ,ShipDate               date         /* HAPI_RETURN_RECEIPT_HEAD_PM.SHIPDATE               */
 ,ProductIdentity        varchar2(35) /* HAPI_RETURN_RECEIPT_HEAD_PM.PRODUCTIDENTITY        */
 ,PickQuantity           number(20,6) /* HAPI_RETURN_RECEIPT_HEAD_PM.PICKQUANTITY           */
 ,SpontaneousReturn      varchar2(1)  /* HAPI_RETURN_RECEIPT_HEAD_PM.SPONTANEOUSRETURN      */
 ,UPDDTM                 date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                  varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT2_RETURN_RECEIPT_LINE(
  HAPIRCV_ID               varchar2(35) /* HAPIRCV.HAPIRCV_ID                                */
 ,OPCODE                   varchar2(1)  /* .                                                 */
 ,ClientIdentity           varchar2(17) /* HAPI_RETURN_RECEIPT_LINE.CLIENTIDENTITY           */
 ,ReturnOrderNumber        varchar2(35) /* HAPI_RETURN_RECEIPT_LINE.RETURNORDERNUMBER        */
 ,ReturnOrderSequence      number(3)    /* HAPI_RETURN_RECEIPT_LINE.RETURNORDERSEQUENCE      */
 ,ReturnOrderSubSequence   number(2)    /* HAPI_RETURN_RECEIPT_LINE.RETURNORDERSUBSEQUENCE   */
 ,ShipDate                 date         /* HAPI_RETURN_RECEIPT_LINE.SHIPDATE                 */
 ,ReturnOrderLinePosition  number(5)    /* HAPI_RETURN_RECEIPT_LINE.RETURNORDERLINEPOSITION  */
 ,ReturnOrderLineSequence  number(5)    /* HAPI_RETURN_RECEIPT_LINE.RETURNORDERLINESEQUENCE  */
 ,ProductIdentity          varchar2(35) /* HAPI_RETURN_RECEIPT_LINE.PRODUCTIDENTITY          */
 ,PackageIdentity          varchar2(17) /* HAPI_RETURN_RECEIPT_LINE.PACKAGEIDENTITY          */
 ,ProductionLotIdentity    varchar2(40) /* HAPI_RETURN_RECEIPT_LINE.PRODUCTIONLOTIDENTITY    */
 ,ProductionSubLotIdentity varchar2(40) /* HAPI_RETURN_RECEIPT_LINE.PRODUCTIONSUBLOTIDENTITY */
 ,MarketingLotIdentity     varchar2(20) /* HAPI_RETURN_RECEIPT_LINE.MARKETINGLOTIDENTITY     */
 ,PickQuantity             number(20,6) /* HAPI_RETURN_RECEIPT_LINE.PICKQUANTITY             */
 ,Qtyback                  number(20,6) /* HAPI_RETURN_RECEIPT_LINE.QTYBACK                  */
 ,Qtycancel                number(20,6) /* HAPI_RETURN_RECEIPT_LINE.QTYCANCEL                */
 ,QtySurplusPick           number(20,6) /* HAPI_RETURN_RECEIPT_LINE.QTYSURPLUSPICK           */
 ,InventoryStatusCode      varchar2(8)  /* HAPI_RETURN_RECEIPT_LINE.INVENTORYSTATUSCODE      */
 ,UPDDTM                   date         /* HAPIRCV.UPDDTM                                    */
 ,PROID                    varchar2(35) /* HAPIRCV.PROID                                     */
);

create table EXT2_SHIPMENT_REPORT_HEAD(
  HAPIRCV_ID                   varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                     */
 ,OPCODE                       varchar2(1)   /* .                                                      */
 ,ASNIdentity                  varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.ASNIDENTITY                  */
 ,ASNSequenceNumber            number(3)     /* HAPI_SHIPMENT_REPORT_HEAD.ASNSEQUENCENUMBER            */
 ,ShipFromPartyNodeIdentity    varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.SHIPFROMPARTYNODEIDENTITY    */
 ,ClientIdentity               varchar2(17)  /* HAPI_SHIPMENT_REPORT_HEAD.CLIENTIDENTITY               */
 ,ASNLevel                     varchar2(1)   /* HAPI_SHIPMENT_REPORT_HEAD.ASNLEVEL                     */
 ,ShipDateTime                 date          /* HAPI_SHIPMENT_REPORT_HEAD.SHIPDATETIME                 */
 ,DocumentDateTime             date          /* HAPI_SHIPMENT_REPORT_HEAD.DOCUMENTDATETIME             */
 ,DeliveryWindowFirst          date          /* HAPI_SHIPMENT_REPORT_HEAD.DELIVERYWINDOWFIRST          */
 ,DeliveryWindowLast           date          /* HAPI_SHIPMENT_REPORT_HEAD.DELIVERYWINDOWLAST           */
 ,ScheduledArrivalDateTime     date          /* HAPI_SHIPMENT_REPORT_HEAD.SCHEDULEDARRIVALDATETIME     */
 ,ShipFromPartyIdentity        varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.SHIPFROMPARTYIDENTITY        */
 ,ShipFromPartyQualifier       varchar2(3)   /* HAPI_SHIPMENT_REPORT_HEAD.SHIPFROMPARTYQUALIFIER       */
 ,ShipToPartyNodeIdentity      varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.SHIPTOPARTYNODEIDENTITY      */
 ,ShiptoPartyIdentity          varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.SHIPTOPARTYIDENTITY          */
 ,ShiptoPartyQualifier         varchar2(3)   /* HAPI_SHIPMENT_REPORT_HEAD.SHIPTOPARTYQUALIFIER         */
 ,ForwarderIdentity            varchar2(17)  /* HAPI_SHIPMENT_REPORT_HEAD.FORWARDERIDENTITY            */
 ,Instructions                 varchar2(400) /* HAPI_SHIPMENT_REPORT_HEAD.INSTRUCTIONS                 */
 ,NumberOfLoadCarriers         number(8)     /* HAPI_SHIPMENT_REPORT_HEAD.NUMBEROFLOADCARRIERS         */
 ,VehicleIdentity              varchar2(17)  /* HAPI_SHIPMENT_REPORT_HEAD.VEHICLEIDENTITY              */
 ,EstimatedVolume              number(16,6)  /* HAPI_SHIPMENT_REPORT_HEAD.ESTIMATEDVOLUME              */
 ,VolumeUOMIdentity            varchar2(17)  /* HAPI_SHIPMENT_REPORT_HEAD.VOLUMEUOMIDENTITY            */
 ,BillOfLadingNumber           varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.BILLOFLADINGNUMBER           */
 ,PackingSlipNumber            varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.PACKINGSLIPNUMBER            */
 ,ShippedFromWarehouseIdentity varchar2(4)   /* HAPI_SHIPMENT_REPORT_HEAD.SHIPPEDFROMWAREHOUSEIDENTITY */
 ,ShippedOnDepartureIdentity   varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.SHIPPEDONDEPARTUREIDENTITY   */
 ,UPDDTM                       date          /* HAPIRCV.UPDDTM                                         */
 ,PROID                        varchar2(35)  /* HAPIRCV.PROID                                          */
);

create table EXT2_SHIPMENT_REPORT_CARRIER(
  HAPIRCV_ID                 varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                      */
 ,OPCODE                     varchar2(1)   /* .                                                       */
 ,VolumeUOMIdentity          varchar2(17)  /* HAPI_SHIPMENT_REPORT_CARRIER.VOLUMEUOMIDENTITY          */
 ,TotalHeight                number(9,4)   /* HAPI_SHIPMENT_REPORT_CARRIER.TOTALHEIGHT                */
 ,HeightUOMIdentity          varchar2(17)  /* HAPI_SHIPMENT_REPORT_CARRIER.HEIGHTUOMIDENTITY          */
 ,ASNIdentity                varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.ASNIDENTITY                */
 ,ASNSequenceNumber          number(3)     /* HAPI_SHIPMENT_REPORT_CARRIER.ASNSEQUENCENUMBER          */
 ,ShipFromPartyNodeIdentity  varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPFROMPARTYNODEIDENTITY  */
 ,LoadCarrierIdentity        varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.LOADCARRIERIDENTITY        */
 ,LoadCarrierQualifier       varchar2(4)   /* HAPI_SHIPMENT_REPORT_CARRIER.LOADCARRIERQUALIFIER       */
 ,ClientIdentity             varchar2(17)  /* HAPI_SHIPMENT_REPORT_CARRIER.CLIENTIDENTITY             */
 ,LoadCarrierType            varchar2(20)  /* HAPI_SHIPMENT_REPORT_CARRIER.LOADCARRIERTYPE            */
 ,ExternalLoadCarrierType    varchar2(20)  /* HAPI_SHIPMENT_REPORT_CARRIER.EXTERNALLOADCARRIERTYPE    */
 ,ParentLoadCarrierIdentity  varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.PARENTLOADCARRIERIDENTITY  */
 ,ProductTransportIdentity   varchar2(5)   /* HAPI_SHIPMENT_REPORT_CARRIER.PRODUCTTRANSPORTIDENTITY   */
 ,ShipFromPartyIdentity      varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPFROMPARTYIDENTITY      */
 ,ShipToPartyNodeIdentity    varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOPARTYNODEIDENTITY    */
 ,ShiptoPartyIdentity        varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOPARTYIDENTITY        */
 ,ShiptoPartyQualifier       varchar2(3)   /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOPARTYQUALIFIER       */
 ,ShipToCustomerNodeIdentity varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOCUSTOMERNODEIDENTITY */
 ,ShiptoCustomerIdentity     varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOCUSTOMERIDENTITY     */
 ,ShiptoCustomerQualifier    varchar2(3)   /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOCUSTOMERQUALIFIER    */
 ,Instructions               varchar2(400) /* HAPI_SHIPMENT_REPORT_CARRIER.INSTRUCTIONS               */
 ,TotalWeight                number(16,6)  /* HAPI_SHIPMENT_REPORT_CARRIER.TOTALWEIGHT                */
 ,WeightUOMIdentity          varchar2(17)  /* HAPI_SHIPMENT_REPORT_CARRIER.WEIGHTUOMIDENTITY          */
 ,TotalVolume                number(16,6)  /* HAPI_SHIPMENT_REPORT_CARRIER.TOTALVOLUME                */
 ,UPDDTM                     date          /* HAPIRCV.UPDDTM                                          */
 ,PROID                      varchar2(35)  /* HAPIRCV.PROID                                           */
);

create table EXT2_SHIPMENT_REPORT_LINE(
  HAPIRCV_ID                    varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                      */
 ,OPCODE                        varchar2(1)   /* .                                                       */
 ,CustomerOrderLineSequence     number(3)     /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERORDERLINESEQUENCE     */
 ,CustomerOrderLineKitPosition  number(2)     /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERORDERLINEKITPOSITION  */
 ,OriginalPurchaseOrderNumber   varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.ORIGINALPURCHASEORDERNUMBER   */
 ,OriginalPurchaseOrderSequence number(3)     /* HAPI_SHIPMENT_REPORT_LINE.ORIGINALPURCHASEORDERSEQUENCE */
 ,OriginalPurchaseOrderLinePos  number(16)    /* HAPI_SHIPMENT_REPORT_LINE.ORIGINALPURCHASEORDERLINEPOS  */
 ,OriginalPurchaseOrderLineSeq  number(3)     /* HAPI_SHIPMENT_REPORT_LINE.ORIGINALPURCHASEORDERLINESEQ  */
 ,OriginalCustomerReference     varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.ORIGINALCUSTOMERREFERENCE     */
 ,InventoryStatusCode           varchar2(8)   /* HAPI_SHIPMENT_REPORT_LINE.INVENTORYSTATUSCODE           */
 ,InventoryStatusDays           number(5)     /* HAPI_SHIPMENT_REPORT_LINE.INVENTORYSTATUSDAYS           */
 ,InventorystatusKey            varchar2(12)  /* HAPI_SHIPMENT_REPORT_LINE.INVENTORYSTATUSKEY            */
 ,InventorystatusText           varchar2(200) /* HAPI_SHIPMENT_REPORT_LINE.INVENTORYSTATUSTEXT           */
 ,InventorystatusAlarmDate      date          /* HAPI_SHIPMENT_REPORT_LINE.INVENTORYSTATUSALARMDATE      */
 ,Instructions                  varchar2(400) /* HAPI_SHIPMENT_REPORT_LINE.INSTRUCTIONS                  */
 ,PredefinedItemLoadIdentity    varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.PREDEFINEDITEMLOADIDENTITY    */
 ,ProductNumberShipFromPartner  varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTNUMBERSHIPFROMPARTNER  */
 ,ProductNumberShipToPartner    varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTNUMBERSHIPTOPARTNER    */
 ,GlobalProductNumber           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.GLOBALPRODUCTNUMBER           */
 ,AdditionalPackingMaterial     varchar2(1)   /* HAPI_SHIPMENT_REPORT_LINE.ADDITIONALPACKINGMATERIAL     */
 ,CustomerOrderType             varchar2(2)   /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERORDERTYPE             */
 ,PurchaseOrderNumber           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.PURCHASEORDERNUMBER           */
 ,PurchaseOrderSequence         number(3)     /* HAPI_SHIPMENT_REPORT_LINE.PURCHASEORDERSEQUENCE         */
 ,PurchaseOrderLinePos          number(4)     /* HAPI_SHIPMENT_REPORT_LINE.PURCHASEORDERLINEPOS          */
 ,PurchaseOrderLineSeq          number(3)     /* HAPI_SHIPMENT_REPORT_LINE.PURCHASEORDERLINESEQ          */
 ,StockedProductNumber          varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.STOCKEDPRODUCTNUMBER          */
 ,GlobalStockedProductNumber    varchar2(20)  /* HAPI_SHIPMENT_REPORT_LINE.GLOBALSTOCKEDPRODUCTNUMBER    */
 ,FromPartyIdentity             varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.FROMPARTYIDENTITY             */
 ,FromPartyQualifier            varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.FROMPARTYQUALIFIER            */
 ,CustomerReturnOrderNumber     varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERRETURNORDERNUMBER     */
 ,CustomerReturnOrderSequence   number(3)     /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERRETURNORDERSEQUENCE   */
 ,CustomerReturnOrderLinePos    number(5)     /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERRETURNORDERLINEPOS    */
 ,CustomerReturnOrderLineKitPos number(2)     /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERRETURNORDERLINEKITPOS */
 ,CustomerReturnOrderLineSeq    number(3)     /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERRETURNORDERLINESEQ    */
 ,ActionCode                    varchar2(2)   /* HAPI_SHIPMENT_REPORT_LINE.ACTIONCODE                    */
 ,ActionCodeRequirement         varchar2(1)   /* HAPI_SHIPMENT_REPORT_LINE.ACTIONCODEREQUIREMENT         */
 ,ReasonCode                    varchar2(2)   /* HAPI_SHIPMENT_REPORT_LINE.REASONCODE                    */
 ,ReasonText                    varchar2(400) /* HAPI_SHIPMENT_REPORT_LINE.REASONTEXT                    */
 ,ShiptoVendorIdentity          varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOVENDORIDENTITY          */
 ,ShiptoVendorQualifier         varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOVENDORQUALIFIER         */
 ,DiscrepancyQuantity           number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.DISCREPANCYQUANTITY           */
 ,DiscrepancyCode               varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.DISCREPANCYCODE               */
 ,DiscrepancyText               varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.DISCREPANCYTEXT               */
 ,QuantityUpperTolerance        number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.QUANTITYUPPERTOLERANCE        */
 ,CatchMeasureLowerTolerance    number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.CATCHMEASURELOWERTOLERANCE    */
 ,CatchMeasureUpperTolerance    number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.CATCHMEASUREUPPERTOLERANCE    */
 ,ASNIdentity                   varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.ASNIDENTITY                   */
 ,ASNSequenceNumber             number(3)     /* HAPI_SHIPMENT_REPORT_LINE.ASNSEQUENCENUMBER             */
 ,ShipFromPartyNodeIdentity     varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPFROMPARTYNODEIDENTITY     */
 ,ASNLineNumber                 number(5)     /* HAPI_SHIPMENT_REPORT_LINE.ASNLINENUMBER                 */
 ,ASNLineSequenceNumber         number(3)     /* HAPI_SHIPMENT_REPORT_LINE.ASNLINESEQUENCENUMBER         */
 ,LoadCarrierIdentity           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.LOADCARRIERIDENTITY           */
 ,ClientIdentity                varchar2(17)  /* HAPI_SHIPMENT_REPORT_LINE.CLIENTIDENTITY                */
 ,ShipFromPartyIdentity         varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPFROMPARTYIDENTITY         */
 ,ShipToPartyNodeIdentity       varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOPARTYNODEIDENTITY       */
 ,ShiptoPartyIdentity           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOPARTYIDENTITY           */
 ,ShiptoPartyQualifier          varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOPARTYQUALIFIER          */
 ,ShipToCustomerNodeIdentity    varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOCUSTOMERNODEIDENTITY    */
 ,ShiptoCustomerIdentity        varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOCUSTOMERIDENTITY        */
 ,ShiptoCustomerQualifier       varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOCUSTOMERQUALIFIER       */
 ,SelltoCustomerIdentity        varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SELLTOCUSTOMERIDENTITY        */
 ,SelltoCustomerQualifier       varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.SELLTOCUSTOMERQUALIFIER       */
 ,OwnerIdentity                 varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.OWNERIDENTITY                 */
 ,OwnerIdentityAtShipToCustomer varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.OWNERIDENTITYATSHIPTOCUSTOMER */
 ,VendorIdentity                varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.VENDORIDENTITY                */
 ,VendorPartyNodeIdentity       varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.VENDORPARTYNODEIDENTITY       */
 ,ProductNumber                 varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTNUMBER                 */
 ,ProductDescription            varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTDESCRIPTION            */
 ,ProductNumberType             varchar2(1)   /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTNUMBERTYPE             */
 ,AlternativeProductNumber      varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.ALTERNATIVEPRODUCTNUMBER      */
 ,AlternativeProductDescription varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.ALTERNATIVEPRODUCTDESCRIPTION */
 ,ProductDate                   date          /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTDATE                   */
 ,ExpiryDate                    date          /* HAPI_SHIPMENT_REPORT_LINE.EXPIRYDATE                    */
 ,ManufacturingDate             date          /* HAPI_SHIPMENT_REPORT_LINE.MANUFACTURINGDATE             */
 ,VendorProductNumber           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.VENDORPRODUCTNUMBER           */
 ,PurchasePrice                 number(22,7)  /* HAPI_SHIPMENT_REPORT_LINE.PURCHASEPRICE                 */
 ,PackageIdentity               varchar2(17)  /* HAPI_SHIPMENT_REPORT_LINE.PACKAGEIDENTITY               */
 ,ProductionLotIdentity         varchar2(40)  /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTIONLOTIDENTITY         */
 ,ProductionSubLotIdentity      varchar2(40)  /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTIONSUBLOTIDENTITY      */
 ,ManufacturingUnit             varchar2(20)  /* HAPI_SHIPMENT_REPORT_LINE.MANUFACTURINGUNIT             */
 ,MarketingLotIdentity          varchar2(20)  /* HAPI_SHIPMENT_REPORT_LINE.MARKETINGLOTIDENTITY          */
 ,SerialNumber                  varchar2(21)  /* HAPI_SHIPMENT_REPORT_LINE.SERIALNUMBER                  */
 ,StorageLot                    varchar2(20)  /* HAPI_SHIPMENT_REPORT_LINE.STORAGELOT                    */
 ,ShippedQuantity               number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPPEDQUANTITY               */
 ,MeasuredQuantity              number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.MEASUREDQUANTITY              */
 ,MeasureQualifier              varchar2(4)   /* HAPI_SHIPMENT_REPORT_LINE.MEASUREQUALIFIER              */
 ,Volume                        number(16,6)  /* HAPI_SHIPMENT_REPORT_LINE.VOLUME                        */
 ,VolumeUOMIdentity             varchar2(17)  /* HAPI_SHIPMENT_REPORT_LINE.VOLUMEUOMIDENTITY             */
 ,Weight                        number(16,6)  /* HAPI_SHIPMENT_REPORT_LINE.WEIGHT                        */
 ,WeightUOMIdentity             varchar2(17)  /* HAPI_SHIPMENT_REPORT_LINE.WEIGHTUOMIDENTITY             */
 ,CustomerOrderNumber           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERORDERNUMBER           */
 ,CustomerOrderSequence         number(3)     /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERORDERSEQUENCE         */
 ,CustomerOrderLinePosition     number(5)     /* HAPI_SHIPMENT_REPORT_LINE.CUSTOMERORDERLINEPOSITION     */
 ,UPDDTM                        date          /* HAPIRCV.UPDDTM                                          */
 ,PROID                         varchar2(35)  /* HAPIRCV.PROID                                           */
);

create table EXT2_CONF_OF_RECEIPT_HEAD(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                      */
 ,OPCODE                        varchar2(1)  /* .                                                       */
 ,AcknowledgeInboundAsnIdentity varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.ACKNOWLEDGEINBOUNDASNIDENTITY */
 ,ArrivalDateTime               date         /* HAPI_CONF_OF_RECEIPT_HEAD.ARRIVALDATETIME               */
 ,ShipToPartyNodeIdentity       varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPTOPARTYNODEIDENTITY       */
 ,ShiptoPartyIdentity           varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPTOPARTYIDENTITY           */
 ,ShiptoPartyQualifier          varchar2(3)  /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPTOPARTYQUALIFIER          */
 ,ShipFromPartyNodeIdentity     varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPFROMPARTYNODEIDENTITY     */
 ,ShipFromPartyIdentity         varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPFROMPARTYIDENTITY         */
 ,ShipFromPartyQualifier        varchar2(3)  /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPFROMPARTYQUALIFIER        */
 ,InboundAsnIdentity            varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.INBOUNDASNIDENTITY            */
 ,InboundAsnSequenceNumber      number(3)    /* HAPI_CONF_OF_RECEIPT_HEAD.INBOUNDASNSEQUENCENUMBER      */
 ,ClientIdentity                varchar2(17) /* HAPI_CONF_OF_RECEIPT_HEAD.CLIENTIDENTITY                */
 ,AcknowledgeInboundAsnType     varchar2(1)  /* HAPI_CONF_OF_RECEIPT_HEAD.ACKNOWLEDGEINBOUNDASNTYPE     */
 ,ForwarderIdentity             varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.FORWARDERIDENTITY             */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                          */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                           */
);

create table EXT2_CONF_OF_RECEIPT_CARRIER(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                         */
 ,OPCODE                        varchar2(1)  /* .                                                          */
 ,AcknowledgeInboundAsnIdentity varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.ACKNOWLEDGEINBOUNDASNIDENTITY */
 ,LoadCarrierIdentity           varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.LOADCARRIERIDENTITY           */
 ,LoadCarrierQualifier          varchar2(4)  /* HAPI_CONF_OF_RECEIPT_CARRIER.LOADCARRIERQUALIFIER          */
 ,ArrivalDateTime               date         /* HAPI_CONF_OF_RECEIPT_CARRIER.ARRIVALDATETIME               */
 ,ShipToPartyNodeIdentity       varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPTOPARTYNODEIDENTITY       */
 ,ShiptoPartyIdentity           varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPTOPARTYIDENTITY           */
 ,ShiptoPartyQualifier          varchar2(3)  /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPTOPARTYQUALIFIER          */
 ,ShipFromPartyNodeIdentity     varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPFROMPARTYNODEIDENTITY     */
 ,ShipFromPartyIdentity         varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPFROMPARTYIDENTITY         */
 ,ShipFromPartyQualifier        varchar2(3)  /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPFROMPARTYQUALIFIER        */
 ,InboundAsnIdentity            varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.INBOUNDASNIDENTITY            */
 ,InboundAsnSequenceNumber      number(3)    /* HAPI_CONF_OF_RECEIPT_CARRIER.INBOUNDASNSEQUENCENUMBER      */
 ,DeliveryIdentity              number(8)    /* HAPI_CONF_OF_RECEIPT_CARRIER.DELIVERYIDENTITY              */
 ,ClientIdentity                varchar2(17) /* HAPI_CONF_OF_RECEIPT_CARRIER.CLIENTIDENTITY                */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                             */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                              */
);

create table EXT2_CONF_OF_RECEIPT_LINE(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                      */
 ,OPCODE                        varchar2(1)  /* .                                                       */
 ,AcknowledgeInboundAsnIdentity varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.ACKNOWLEDGEINBOUNDASNIDENTITY */
 ,AcknowledgeInboundAsnLinenum  number(5)    /* HAPI_CONF_OF_RECEIPT_LINE.ACKNOWLEDGEINBOUNDASNLINENUM  */
 ,ArrivalDateTime               date         /* HAPI_CONF_OF_RECEIPT_LINE.ARRIVALDATETIME               */
 ,ShipFromPartyNodeIdentity     varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.SHIPFROMPARTYNODEIDENTITY     */
 ,ShipFromPartyIdentity         varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.SHIPFROMPARTYIDENTITY         */
 ,ShipFromPartyQualifier        varchar2(3)  /* HAPI_CONF_OF_RECEIPT_LINE.SHIPFROMPARTYQUALIFIER        */
 ,InboundAsnIdentity            varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.INBOUNDASNIDENTITY            */
 ,InboundAsnSequenceNumber      number(3)    /* HAPI_CONF_OF_RECEIPT_LINE.INBOUNDASNSEQUENCENUMBER      */
 ,InboundAsnLineNumber          number(5)    /* HAPI_CONF_OF_RECEIPT_LINE.INBOUNDASNLINENUMBER          */
 ,InboundAsnLineSequenceNumber  number(3)    /* HAPI_CONF_OF_RECEIPT_LINE.INBOUNDASNLINESEQUENCENUMBER  */
 ,ProductNumber                 varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.PRODUCTNUMBER                 */
 ,ClientIdentity                varchar2(17) /* HAPI_CONF_OF_RECEIPT_LINE.CLIENTIDENTITY                */
 ,AsnLineQuantity               number(20,6) /* HAPI_CONF_OF_RECEIPT_LINE.ASNLINEQUANTITY               */
 ,ArrivedQuantity               number(20,6) /* HAPI_CONF_OF_RECEIPT_LINE.ARRIVEDQUANTITY               */
 ,ReceiptQuantity               number(20,6) /* HAPI_CONF_OF_RECEIPT_LINE.RECEIPTQUANTITY               */
 ,ProductionLotIdentity         varchar2(40) /* HAPI_CONF_OF_RECEIPT_LINE.PRODUCTIONLOTIDENTITY         */
 ,ProductionSubLotIdentity      varchar2(40) /* HAPI_CONF_OF_RECEIPT_LINE.PRODUCTIONSUBLOTIDENTITY      */
 ,SerialNumber                  varchar2(21) /* HAPI_CONF_OF_RECEIPT_LINE.SERIALNUMBER                  */
 ,DeliveryIdentity              number(8)    /* HAPI_CONF_OF_RECEIPT_LINE.DELIVERYIDENTITY              */
 ,LoadCarrierIdentity           varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.LOADCARRIERIDENTITY           */
 ,PurchaseOrderNumber           varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.PURCHASEORDERNUMBER           */
 ,PurchaseOrderSequence         number(3)    /* HAPI_CONF_OF_RECEIPT_LINE.PURCHASEORDERSEQUENCE         */
 ,PurchaseOrderLinePos          number(4)    /* HAPI_CONF_OF_RECEIPT_LINE.PURCHASEORDERLINEPOS          */
 ,PurchaseOrderLineSeq          number(3)    /* HAPI_CONF_OF_RECEIPT_LINE.PURCHASEORDERLINESEQ          */
 ,CustomerOrderType             varchar2(2)  /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERORDERTYPE             */
 ,ExpiryDate                    date         /* HAPI_CONF_OF_RECEIPT_LINE.EXPIRYDATE                    */
 ,ManufacturingDate             date         /* HAPI_CONF_OF_RECEIPT_LINE.MANUFACTURINGDATE             */
 ,InventoryStatusCode           varchar2(8)  /* HAPI_CONF_OF_RECEIPT_LINE.INVENTORYSTATUSCODE           */
 ,MeasuredQuantity              number(20,6) /* HAPI_CONF_OF_RECEIPT_LINE.MEASUREDQUANTITY              */
 ,MeasureQualifier              varchar2(4)  /* HAPI_CONF_OF_RECEIPT_LINE.MEASUREQUALIFIER              */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                          */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                           */
);

create table EXT2_INB_ORDER_COMPLETED(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                         */
 ,OPCODE                        varchar2(1)  /* .                                                          */
 ,ClientIdentity                varchar2(17) /* HAPI_INBOUND_ORDER_COMPLETED.CLIENTIDENTITY                */
 ,WarehouseIdentity             varchar2(4)  /* HAPI_INBOUND_ORDER_COMPLETED.WAREHOUSEIDENTITY             */
 ,EmployeeIdentity              varchar2(8)  /* HAPI_INBOUND_ORDER_COMPLETED.EMPLOYEEIDENTITY              */
 ,PurchaseOrderNumber           varchar2(35) /* HAPI_INBOUND_ORDER_COMPLETED.PURCHASEORDERNUMBER           */
 ,PurchaseOrderSequence         number(3)    /* HAPI_INBOUND_ORDER_COMPLETED.PURCHASEORDERSEQUENCE         */
 ,PurchaseOrderLinePosition     number(4)    /* HAPI_INBOUND_ORDER_COMPLETED.PURCHASEORDERLINEPOSITION     */
 ,PurchaseOrderLineSequence     number(3)    /* HAPI_INBOUND_ORDER_COMPLETED.PURCHASEORDERLINESEQUENCE     */
 ,CustomerReturnOrderNumber     varchar2(35) /* HAPI_INBOUND_ORDER_COMPLETED.CUSTOMERRETURNORDERNUMBER     */
 ,CustomerReturnOrderSequence   number(3)    /* HAPI_INBOUND_ORDER_COMPLETED.CUSTOMERRETURNORDERSEQUENCE   */
 ,CustomerReturnOrderLinePos    number(5)    /* HAPI_INBOUND_ORDER_COMPLETED.CUSTOMERRETURNORDERLINEPOS    */
 ,CustomerReturnOrderLineKitPos number(2)    /* HAPI_INBOUND_ORDER_COMPLETED.CUSTOMERRETURNORDERLINEKITPOS */
 ,CustomerReturnOrderLineSeq    number(3)    /* HAPI_INBOUND_ORDER_COMPLETED.CUSTOMERRETURNORDERLINESEQ    */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                             */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                              */
);

