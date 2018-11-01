create table EXT_BALANCE_ANSWER(
  HAPIRCV_ID                varchar2(35) /* HAPIRCV.HAPIRCV_ID                            */
 ,OPCODE                    varchar2(1)  /* .                                             */
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
 ,PickedQuantity            number(20,6) /* HAPI_BALANCE_ANSWER.PICKEDQUANTITY            */
 ,TopickQuantity            number(20,6) /* HAPI_BALANCE_ANSWER.TOPICKQUANTITY            */
 ,CustomerReservedQuantity  number(20,6) /* HAPI_BALANCE_ANSWER.CUSTOMERRESERVEDQUANTITY  */
 ,ReservedForReplenQuantity number(20,6) /* HAPI_BALANCE_ANSWER.RESERVEDFORREPLENQUANTITY */
 ,MessageNumber             number(14)   /* HAPI_BALANCE_ANSWER.MESSAGENUMBER             */
 ,LastBalanceAnswer         varchar2(1)  /* HAPI_BALANCE_ANSWER.LASTBALANCEANSWER         */
 ,UPDDTM                    date         /* HAPIRCV.UPDDTM                                */
 ,PROID                     varchar2(35) /* HAPIRCV.PROID                                 */
);

create table EXT_DLVRY_RECEIPT_HEAD(
  HAPIRCV_ID         varchar2(35) /* HAPIRCV.HAPIRCV_ID                         */
 ,OPCODE             varchar2(1)  /* .                                          */
 ,ClientIdentity     varchar2(17) /* HAPI_DLVRY_RECEIPT_HEAD.CLIENTIDENTITY     */
 ,DeliveryIdentity   number(8)    /* HAPI_DLVRY_RECEIPT_HEAD.DELIVERYIDENTITY   */
 ,ArrivalDateTime    date         /* HAPI_DLVRY_RECEIPT_HEAD.ARRIVALDATETIME    */
 ,WarehouseIdentity  varchar2(4)  /* HAPI_DLVRY_RECEIPT_HEAD.WAREHOUSEIDENTITY  */
 ,EmployeeIdentity   varchar2(35) /* HAPI_DLVRY_RECEIPT_HEAD.EMPLOYEEIDENTITY   */
 ,ReceiveType        varchar2(2)  /* HAPI_DLVRY_RECEIPT_HEAD.RECEIVETYPE        */
 ,PackingSlipNumber  varchar2(35) /* HAPI_DLVRY_RECEIPT_HEAD.PACKINGSLIPNUMBER  */
 ,BillOfLadingNumber varchar2(35) /* HAPI_DLVRY_RECEIPT_HEAD.BILLOFLADINGNUMBER */
 ,VehicleIdentity    varchar2(17) /* HAPI_DLVRY_RECEIPT_HEAD.VEHICLEIDENTITY    */
 ,UPDDTM             date         /* HAPIRCV.UPDDTM                             */
 ,PROID              varchar2(35) /* HAPIRCV.PROID                              */
);

create table EXT_DLVRY_RECEIPT_HEAD_TEXT(
  HAPIRCV_ID       varchar2(35)  /* HAPIRCV.HAPIRCV_ID                            */
 ,OPCODE           varchar2(1)   /* .                                             */
 ,ClientIdentity   varchar2(17)  /* HAPI_DLVRY_RECEIPT_HEAD_TEXT.CLIENTIDENTITY   */
 ,DeliveryIdentity number(8)     /* HAPI_DLVRY_RECEIPT_HEAD_TEXT.DELIVERYIDENTITY */
 ,ArrivalDateTime  date          /* HAPI_DLVRY_RECEIPT_HEAD_TEXT.ARRIVALDATETIME  */
 ,TextFunction     varchar2(3)   /* HAPI_DLVRY_RECEIPT_HEAD_TEXT.TEXTFUNCTION     */
 ,Text             varchar2(400) /* HAPI_DLVRY_RECEIPT_HEAD_TEXT.TEXT             */
 ,UPDDTM           date          /* HAPIRCV.UPDDTM                                */
 ,PROID            varchar2(35)  /* HAPIRCV.PROID                                 */
);

create table EXT_DLVRY_RECEIPT_LINE(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                    */
 ,OPCODE                        varchar2(1)  /* .                                                     */
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
 ,InventoryStatusCode           varchar2(8)  /* HAPI_DLVRY_RECEIPT_LINE.INVENTORYSTATUSCODE           */
 ,SerialNumber                  varchar2(21) /* HAPI_DLVRY_RECEIPT_LINE.SERIALNUMBER                  */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                        */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                         */
);

create table EXT_DLVRY_RECEIPT_LINE_TEXT(
  HAPIRCV_ID           varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                */
 ,OPCODE               varchar2(1)   /* .                                                 */
 ,ClientIdentity       varchar2(17)  /* HAPI_DLVRY_RECEIPT_LINE_TEXT.CLIENTIDENTITY       */
 ,DeliveryIdentity     number(8)     /* HAPI_DLVRY_RECEIPT_LINE_TEXT.DELIVERYIDENTITY     */
 ,DeliveryidentityLine number(5)     /* HAPI_DLVRY_RECEIPT_LINE_TEXT.DELIVERYIDENTITYLINE */
 ,ArrivalDateTime      date          /* HAPI_DLVRY_RECEIPT_LINE_TEXT.ARRIVALDATETIME      */
 ,TextFunction         varchar2(3)   /* HAPI_DLVRY_RECEIPT_LINE_TEXT.TEXTFUNCTION         */
 ,Text                 varchar2(400) /* HAPI_DLVRY_RECEIPT_LINE_TEXT.TEXT                 */
 ,UPDDTM               date          /* HAPIRCV.UPDDTM                                    */
 ,PROID                varchar2(35)  /* HAPIRCV.PROID                                     */
);

create table EXT_DLVRY_RECEIPT_PM(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                  */
 ,OPCODE                        varchar2(1)  /* .                                                   */
 ,ClientIdentity                varchar2(17) /* HAPI_DLVRY_RECEIPT_PM.CLIENTIDENTITY                */
 ,DeliveryIdentity              number(8)    /* HAPI_DLVRY_RECEIPT_PM.DELIVERYIDENTITY              */
 ,ArrivalDateTime               date         /* HAPI_DLVRY_RECEIPT_PM.ARRIVALDATETIME               */
 ,PackingMaterialType           varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.PACKINGMATERIALTYPE           */
 ,PackageIdentity               varchar2(17) /* HAPI_DLVRY_RECEIPT_PM.PACKAGEIDENTITY               */
 ,DeliveredQuantity             number(20,6) /* HAPI_DLVRY_RECEIPT_PM.DELIVEREDQUANTITY             */
 ,VendorIdentity                varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.VENDORIDENTITY                */
 ,PurchaseOrderType             varchar2(2)  /* HAPI_DLVRY_RECEIPT_PM.PURCHASEORDERTYPE             */
 ,PurchaseOrderNumber           varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.PURCHASEORDERNUMBER           */
 ,PurchaseOrderSequence         number(3)    /* HAPI_DLVRY_RECEIPT_PM.PURCHASEORDERSEQUENCE         */
 ,PurchaseOrderLinePosition     number(4)    /* HAPI_DLVRY_RECEIPT_PM.PURCHASEORDERLINEPOSITION     */
 ,PurchaseOrderLineSequence     number(3)    /* HAPI_DLVRY_RECEIPT_PM.PURCHASEORDERLINESEQUENCE     */
 ,CustomerOrderNumber           varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERORDERNUMBER           */
 ,CustomerOrderSequence         number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERORDERSEQUENCE         */
 ,CustomerOrderLinePosition     number(5)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERORDERLINEPOSITION     */
 ,CustomerOrderLineKitPosition  number(2)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERORDERLINEKITPOSITION  */
 ,CustomerOrderLineSequence     number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERORDERLINESEQUENCE     */
 ,FromPartyId                   varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.FROMPARTYID                   */
 ,FromPartyQualifier            varchar2(3)  /* HAPI_DLVRY_RECEIPT_PM.FROMPARTYQUALIFIER            */
 ,CustomerReturnOrderNumber     varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERNUMBER     */
 ,CustomerReturnOrderSequence   number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERSEQUENCE   */
 ,CustomerReturnOrderLinePos    number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERLINEPOS    */
 ,CustomerReturnOrderLineKitPos number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERLINEKITPOS */
 ,CustomerReturnOrderLineSeq    number(3)    /* HAPI_DLVRY_RECEIPT_PM.CUSTOMERRETURNORDERLINESEQ    */
 ,DiscrepancyCode               varchar2(3)  /* HAPI_DLVRY_RECEIPT_PM.DISCREPANCYCODE               */
 ,DiscrepancyActionCode         varchar2(2)  /* HAPI_DLVRY_RECEIPT_PM.DISCREPANCYACTIONCODE         */
 ,ShipFromPartyId               varchar2(35) /* HAPI_DLVRY_RECEIPT_PM.SHIPFROMPARTYID               */
 ,ShipFromPartyQualifier        varchar2(3)  /* HAPI_DLVRY_RECEIPT_PM.SHIPFROMPARTYQUALIFIER        */
 ,PackingMaterialLine           number(5)    /* HAPI_DLVRY_RECEIPT_PM.PACKINGMATERIALLINE           */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                      */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                       */
);

create table EXT_INSPECTION_RECEIPT_HEAD(
  HAPIRCV_ID            varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                varchar2(1)  /* .                                                  */
 ,ClientIdentity        varchar2(17) /* HAPI_INSPECTION_RECEIPT_HEAD.CLIENTIDENTITY        */
 ,WarehouseIdentity     varchar2(4)  /* HAPI_INSPECTION_RECEIPT_HEAD.WAREHOUSEIDENTITY     */
 ,EmployeeIdentity      varchar2(35) /* HAPI_INSPECTION_RECEIPT_HEAD.EMPLOYEEIDENTITY      */
 ,ReturnhandleddateTime date         /* HAPI_INSPECTION_RECEIPT_HEAD.RETURNHANDLEDDATETIME */
 ,UPDDTM                date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                 varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT_INSPECTION_RECEIPT_LINE(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                         */
 ,OPCODE                        varchar2(1)  /* .                                                          */
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
 ,ProductionSubLotIdentity      varchar2(40) /* HAPI_INSPECTION_RECEIPT_LINE.PRODUCTIONSUBLOTIDENTITY      */
 ,MarketingLotIdentity          varchar2(20) /* HAPI_INSPECTION_RECEIPT_LINE.MARKETINGLOTIDENTITY          */
 ,QualityLotIdentity            varchar2(20) /* HAPI_INSPECTION_RECEIPT_LINE.QUALITYLOTIDENTITY            */
 ,ItemLoadIdentity              varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.ITEMLOADIDENTITY              */
 ,ExpiryDate                    date         /* HAPI_INSPECTION_RECEIPT_LINE.EXPIRYDATE                    */
 ,FromPartyId                   varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.FROMPARTYID                   */
 ,FromPartyQualifier            varchar2(3)  /* HAPI_INSPECTION_RECEIPT_LINE.FROMPARTYQUALIFIER            */
 ,ShipToCustomerIdentity        varchar2(35) /* HAPI_INSPECTION_RECEIPT_LINE.SHIPTOCUSTOMERIDENTITY        */
 ,ShipToCustomerQualifier       varchar2(3)  /* HAPI_INSPECTION_RECEIPT_LINE.SHIPTOCUSTOMERQUALIFIER       */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                             */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                              */
);

create table EXT_INSPECTION_RECEIPT_LINEPM(
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

create table EXT_INVENTORY_CHANGE(
  HAPIRCV_ID                   varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                       varchar2(1)  /* .                                                  */
 ,ClientIdentity               varchar2(17) /* HAPI_INVENTORY_CHANGE.CLIENTIDENTITY               */
 ,Employeeid                   varchar2(35) /* HAPI_INVENTORY_CHANGE.EMPLOYEEID                   */
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
 ,VendorIdentity2              varchar2(35) /* HAPI_INVENTORY_CHANGE.VENDORIDENTITY2              */
 ,OwnerIdentity                varchar2(35) /* HAPI_INVENTORY_CHANGE.OWNERIDENTITY                */
 ,OwnerIdentity2               varchar2(35) /* HAPI_INVENTORY_CHANGE.OWNERIDENTITY2               */
 ,ProductNumber                varchar2(35) /* HAPI_INVENTORY_CHANGE.PRODUCTNUMBER                */
 ,ProductNumber2               varchar2(35) /* HAPI_INVENTORY_CHANGE.PRODUCTNUMBER2               */
 ,ProductDate                  date         /* HAPI_INVENTORY_CHANGE.PRODUCTDATE                  */
 ,ProductDate2                 date         /* HAPI_INVENTORY_CHANGE.PRODUCTDATE2                 */
 ,PackageIdentity              varchar2(17) /* HAPI_INVENTORY_CHANGE.PACKAGEIDENTITY              */
 ,PackageIdentity2             varchar2(17) /* HAPI_INVENTORY_CHANGE.PACKAGEIDENTITY2             */
 ,Quantity                     number(20,6) /* HAPI_INVENTORY_CHANGE.QUANTITY                     */
 ,Quantity2                    number(20,6) /* HAPI_INVENTORY_CHANGE.QUANTITY2                    */
 ,MeasuredQty                  number(20,6) /* HAPI_INVENTORY_CHANGE.MEASUREDQTY                  */
 ,MeasuredQty2                 number(20,6) /* HAPI_INVENTORY_CHANGE.MEASUREDQTY2                 */
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
 ,Freetext                     varchar2(45) /* HAPI_INVENTORY_CHANGE.FREETEXT                     */
 ,SerialNumber                 varchar2(21) /* HAPI_INVENTORY_CHANGE.SERIALNUMBER                 */
 ,SerialNumber2                varchar2(21) /* HAPI_INVENTORY_CHANGE.SERIALNUMBER2                */
 ,UPDDTM                       date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                        varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT_PICK_RECEIPT_HEAD(
  HAPIRCV_ID               varchar2(35) /* HAPIRCV.HAPIRCV_ID                              */
 ,OPCODE                   varchar2(1)  /* .                                               */
 ,ClientIdentity           varchar2(17) /* HAPI_PICK_RECEIPT_HEAD.CLIENTIDENTITY           */
 ,CustomerOrderNumber      varchar2(35) /* HAPI_PICK_RECEIPT_HEAD.CUSTOMERORDERNUMBER      */
 ,CustomerOrderSequence    number(3)    /* HAPI_PICK_RECEIPT_HEAD.CUSTOMERORDERSEQUENCE    */
 ,CustomerOrderSubSequence number(4)    /* HAPI_PICK_RECEIPT_HEAD.CUSTOMERORDERSUBSEQUENCE */
 ,LastOndeparture          varchar2(1)  /* HAPI_PICK_RECEIPT_HEAD.LASTONDEPARTURE          */
 ,WarehouseIdentity        varchar2(4)  /* HAPI_PICK_RECEIPT_HEAD.WAREHOUSEIDENTITY        */
 ,AssembleToStock          varchar2(1)  /* HAPI_PICK_RECEIPT_HEAD.ASSEMBLETOSTOCK          */
 ,UPDDTM                   date         /* HAPIRCV.UPDDTM                                  */
 ,PROID                    varchar2(35) /* HAPIRCV.PROID                                   */
);

create table EXT_PICK_RECEIPT_HEAD_PM(
  HAPIRCV_ID               varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                   varchar2(1)  /* .                                                  */
 ,ClientIdentity           varchar2(17) /* HAPI_PICK_RECEIPT_HEAD_PM.CLIENTIDENTITY           */
 ,CustomerOrderNumber      varchar2(35) /* HAPI_PICK_RECEIPT_HEAD_PM.CUSTOMERORDERNUMBER      */
 ,CustomerOrderSequence    number(3)    /* HAPI_PICK_RECEIPT_HEAD_PM.CUSTOMERORDERSEQUENCE    */
 ,CustomerOrderSubSequence number(4)    /* HAPI_PICK_RECEIPT_HEAD_PM.CUSTOMERORDERSUBSEQUENCE */
 ,PackingMaterialType      varchar2(35) /* HAPI_PICK_RECEIPT_HEAD_PM.PACKINGMATERIALTYPE      */
 ,Quantity                 number(20,6) /* HAPI_PICK_RECEIPT_HEAD_PM.QUANTITY                 */
 ,UPDDTM                   date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                    varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT_PICK_RECEIPT_LINE(
  HAPIRCV_ID                   varchar2(35) /* HAPIRCV.HAPIRCV_ID                                  */
 ,OPCODE                       varchar2(1)  /* .                                                   */
 ,ClientIdentity               varchar2(17) /* HAPI_PICK_RECEIPT_LINE.CLIENTIDENTITY               */
 ,CustomerOrderNumber          varchar2(35) /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERNUMBER          */
 ,CustomerOrderSequence        number(3)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERSEQUENCE        */
 ,CustomerOrderSubSequence     number(4)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERSUBSEQUENCE     */
 ,CustomerOrderLinePosition    number(5)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERLINEPOSITION    */
 ,CustomerOrderLineKitPosition number(2)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERLINEKITPOSITION */
 ,CustomerOrderLineSequence    number(3)    /* HAPI_PICK_RECEIPT_LINE.CUSTOMERORDERLINESEQUENCE    */
 ,PickOrderLineIdentity        varchar2(35) /* HAPI_PICK_RECEIPT_LINE.PICKORDERLINEIDENTITY        */
 ,OwnerIdentity                varchar2(35) /* HAPI_PICK_RECEIPT_LINE.OWNERIDENTITY                */
 ,ProductIdentity              varchar2(35) /* HAPI_PICK_RECEIPT_LINE.PRODUCTIDENTITY              */
 ,PackageIdentity              varchar2(17) /* HAPI_PICK_RECEIPT_LINE.PACKAGEIDENTITY              */
 ,PickQuantity                 number(20,6) /* HAPI_PICK_RECEIPT_LINE.PICKQUANTITY                 */
 ,MeasuredQty                  number(20,6) /* HAPI_PICK_RECEIPT_LINE.MEASUREDQTY                  */
 ,RestCode                     varchar2(3)  /* HAPI_PICK_RECEIPT_LINE.RESTCODE                     */
 ,DepartureIdentity            varchar2(35) /* HAPI_PICK_RECEIPT_LINE.DEPARTUREIDENTITY            */
 ,DepartureweekDay             varchar2(1)  /* HAPI_PICK_RECEIPT_LINE.DEPARTUREWEEKDAY             */
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
 ,AssembleToStock              varchar2(1)  /* HAPI_PICK_RECEIPT_LINE.ASSEMBLETOSTOCK              */
 ,ItemLoadIdentity             varchar2(35) /* HAPI_PICK_RECEIPT_LINE.ITEMLOADIDENTITY             */
 ,UPDDTM                       date         /* HAPIRCV.UPDDTM                                      */
 ,PROID                        varchar2(35) /* HAPIRCV.PROID                                       */
);

create table EXT_PICK_RECEIPT_LINE_TEXT(
  HAPIRCV_ID                   varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                       */
 ,OPCODE                       varchar2(1)   /* .                                                        */
 ,ClientIdentity               varchar2(17)  /* HAPI_PICK_RECEIPT_LINE_TEXT.CLIENTIDENTITY               */
 ,CustomerOrderNumber          varchar2(35)  /* HAPI_PICK_RECEIPT_LINE_TEXT.CUSTOMERORDERNUMBER          */
 ,CustomerOrderSequence        number(3)     /* HAPI_PICK_RECEIPT_LINE_TEXT.CUSTOMERORDERSEQUENCE        */
 ,CustomerOrderSubSequence     number(4)     /* HAPI_PICK_RECEIPT_LINE_TEXT.CUSTOMERORDERSUBSEQUENCE     */
 ,CustomerOrderLinePosition    number(5)     /* HAPI_PICK_RECEIPT_LINE_TEXT.CUSTOMERORDERLINEPOSITION    */
 ,CustomerOrderLineKitPosition number(2)     /* HAPI_PICK_RECEIPT_LINE_TEXT.CUSTOMERORDERLINEKITPOSITION */
 ,CustomerOrderLineSequence    number(3)     /* HAPI_PICK_RECEIPT_LINE_TEXT.CUSTOMERORDERLINESEQUENCE    */
 ,PickOrderLineIdentity        varchar2(35)  /* HAPI_PICK_RECEIPT_LINE_TEXT.PICKORDERLINEIDENTITY        */
 ,TextFunction                 varchar2(3)   /* HAPI_PICK_RECEIPT_LINE_TEXT.TEXTFUNCTION                 */
 ,Text                         varchar2(400) /* HAPI_PICK_RECEIPT_LINE_TEXT.TEXT                         */
 ,UPDDTM                       date          /* HAPIRCV.UPDDTM                                           */
 ,PROID                        varchar2(35)  /* HAPIRCV.PROID                                            */
);

create table EXT_PICK_RECEIPT_LINE_PM(
  HAPIRCV_ID                   varchar2(35) /* HAPIRCV.HAPIRCV_ID                                     */
 ,OPCODE                       varchar2(1)  /* .                                                      */
 ,ClientIdentity               varchar2(17) /* HAPI_PICK_RECEIPT_LINE_PM.CLIENTIDENTITY               */
 ,CustomerOrderNumber          varchar2(35) /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERNUMBER          */
 ,CustomerOrderSequence        number(3)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERSEQUENCE        */
 ,CustomerOrderSubSequence     number(4)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERSUBSEQUENCE     */
 ,CustomerOrderLinePosition    number(5)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERLINEPOSITION    */
 ,CustomerOrderLineKitPosition number(2)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERLINEKITPOSITION */
 ,CustomerOrderLineSequence    number(3)    /* HAPI_PICK_RECEIPT_LINE_PM.CUSTOMERORDERLINESEQUENCE    */
 ,PickOrderLineIdentity        varchar2(35) /* HAPI_PICK_RECEIPT_LINE_PM.PICKORDERLINEIDENTITY        */
 ,PackingMaterialType          varchar2(35) /* HAPI_PICK_RECEIPT_LINE_PM.PACKINGMATERIALTYPE          */
 ,Quantity                     number(20,6) /* HAPI_PICK_RECEIPT_LINE_PM.QUANTITY                     */
 ,UPDDTM                       date         /* HAPIRCV.UPDDTM                                         */
 ,PROID                        varchar2(35) /* HAPIRCV.PROID                                          */
);

create table EXT_PICK_RECEIPT_SERVICE(
  HAPIRCV_ID               varchar2(35) /* HAPIRCV.HAPIRCV_ID                                 */
 ,OPCODE                   varchar2(1)  /* .                                                  */
 ,ClientIdentity           varchar2(17) /* HAPI_PICK_RECEIPT_SERVICE.CLIENTIDENTITY           */
 ,CustomerOrderNumber      varchar2(35) /* HAPI_PICK_RECEIPT_SERVICE.CUSTOMERORDERNUMBER      */
 ,CustomerOrderSequence    number(3)    /* HAPI_PICK_RECEIPT_SERVICE.CUSTOMERORDERSEQUENCE    */
 ,CustomerOrderSubSequence number(4)    /* HAPI_PICK_RECEIPT_SERVICE.CUSTOMERORDERSUBSEQUENCE */
 ,ServiceQualifier         varchar2(3)  /* HAPI_PICK_RECEIPT_SERVICE.SERVICEQUALIFIER         */
 ,ServiceCode              varchar2(17) /* HAPI_PICK_RECEIPT_SERVICE.SERVICECODE              */
 ,ServiceQty               number(20,6) /* HAPI_PICK_RECEIPT_SERVICE.SERVICEQTY               */
 ,UPDDTM                   date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                    varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT_PICK_RECEIPT_TEXT(
  HAPIRCV_ID               varchar2(35)  /* HAPIRCV.HAPIRCV_ID                              */
 ,OPCODE                   varchar2(1)   /* .                                               */
 ,ClientIdentity           varchar2(17)  /* HAPI_PICK_RECEIPT_TEXT.CLIENTIDENTITY           */
 ,CustomerOrderNumber      varchar2(35)  /* HAPI_PICK_RECEIPT_TEXT.CUSTOMERORDERNUMBER      */
 ,CustomerOrderSequence    number(3)     /* HAPI_PICK_RECEIPT_TEXT.CUSTOMERORDERSEQUENCE    */
 ,CustomerOrderSubSequence number(4)     /* HAPI_PICK_RECEIPT_TEXT.CUSTOMERORDERSUBSEQUENCE */
 ,TextFunction             varchar2(3)   /* HAPI_PICK_RECEIPT_TEXT.TEXTFUNCTION             */
 ,Text                     varchar2(400) /* HAPI_PICK_RECEIPT_TEXT.TEXT                     */
 ,UPDDTM                   date          /* HAPIRCV.UPDDTM                                  */
 ,PROID                    varchar2(35)  /* HAPIRCV.PROID                                   */
);

create table EXT_RETURNED_PM_HEAD(
  HAPIRCV_ID varchar2(35) /* HAPIRCV.HAPIRCV_ID */
 ,OPCODE     varchar2(1)  /* .                  */
 ,UPDDTM     date         /* HAPIRCV.UPDDTM     */
 ,PROID      varchar2(35) /* HAPIRCV.PROID      */
);

create table EXT_RETURNED_PM_LINE(
  HAPIRCV_ID varchar2(35) /* HAPIRCV.HAPIRCV_ID */
 ,OPCODE     varchar2(1)  /* .                  */
 ,UPDDTM     date         /* HAPIRCV.UPDDTM     */
 ,PROID      varchar2(35) /* HAPIRCV.PROID      */
);

create table EXT_RETURN_RECEIPT_HEAD(
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

create table EXT_RETURN_RECEIPT_HEAD_PM(
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
 ,WarehouseIdentity      varchar2(4)  /* HAPI_RETURN_RECEIPT_HEAD_PM.WAREHOUSEIDENTITY      */
 ,UPDDTM                 date         /* HAPIRCV.UPDDTM                                     */
 ,PROID                  varchar2(35) /* HAPIRCV.PROID                                      */
);

create table EXT_RETURN_RECEIPT_LINE(
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
 ,MeasuredQty              number(20,6) /* HAPI_RETURN_RECEIPT_LINE.MEASUREDQTY              */
 ,OwnerIdentity            varchar2(35) /* HAPI_RETURN_RECEIPT_LINE.OWNERIDENTITY            */
 ,SerialNumber             varchar2(21) /* HAPI_RETURN_RECEIPT_LINE.SERIALNUMBER             */
 ,PickOrderLineIdentity    varchar2(35) /* HAPI_RETURN_RECEIPT_LINE.PICKORDERLINEIDENTITY    */
 ,ItemLoadIdentity         varchar2(35) /* HAPI_RETURN_RECEIPT_LINE.ITEMLOADIDENTITY         */
 ,UPDDTM                   date         /* HAPIRCV.UPDDTM                                    */
 ,PROID                    varchar2(35) /* HAPIRCV.PROID                                     */
);

create table EXT_SHIPMENT_REPORT_HEAD(
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
 ,ShipToPartyIdentity          varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.SHIPTOPARTYIDENTITY          */
 ,ShipToPartyQualifier         varchar2(3)   /* HAPI_SHIPMENT_REPORT_HEAD.SHIPTOPARTYQUALIFIER         */
 ,ForwarderIdentity            varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.FORWARDERIDENTITY            */
 ,Instructions                 varchar2(400) /* HAPI_SHIPMENT_REPORT_HEAD.INSTRUCTIONS                 */
 ,NumberOfLoadCarriers         number(8)     /* HAPI_SHIPMENT_REPORT_HEAD.NUMBEROFLOADCARRIERS         */
 ,VehicleIdentity              varchar2(17)  /* HAPI_SHIPMENT_REPORT_HEAD.VEHICLEIDENTITY              */
 ,EstimatedVolume              number(19,9)  /* HAPI_SHIPMENT_REPORT_HEAD.ESTIMATEDVOLUME              */
 ,VolumeUOMIdentity            varchar2(17)  /* HAPI_SHIPMENT_REPORT_HEAD.VOLUMEUOMIDENTITY            */
 ,BillOfLadingNumber           varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.BILLOFLADINGNUMBER           */
 ,PackingSlipNumber            varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.PACKINGSLIPNUMBER            */
 ,ShippedFromWarehouseIdentity varchar2(4)   /* HAPI_SHIPMENT_REPORT_HEAD.SHIPPEDFROMWAREHOUSEIDENTITY */
 ,ShippedOnDepartureIdentity   varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD.SHIPPEDONDEPARTUREIDENTITY   */
 ,UPDDTM                       date          /* HAPIRCV.UPDDTM                                         */
 ,PROID                        varchar2(35)  /* HAPIRCV.PROID                                          */
);

create table EXT_SHIPMENT_REPORT_CARRIER(
  HAPIRCV_ID                 varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                      */
 ,OPCODE                     varchar2(1)   /* .                                                       */
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
 ,ShipToPartyIdentity        varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOPARTYIDENTITY        */
 ,ShipToPartyQualifier       varchar2(3)   /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOPARTYQUALIFIER       */
 ,ShipToCustomerNodeIdentity varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOCUSTOMERNODEIDENTITY */
 ,ShipToCustomerIdentity     varchar2(35)  /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOCUSTOMERIDENTITY     */
 ,ShipToCustomerQualifier    varchar2(3)   /* HAPI_SHIPMENT_REPORT_CARRIER.SHIPTOCUSTOMERQUALIFIER    */
 ,Instructions               varchar2(400) /* HAPI_SHIPMENT_REPORT_CARRIER.INSTRUCTIONS               */
 ,TotalWeight                number(16,6)  /* HAPI_SHIPMENT_REPORT_CARRIER.TOTALWEIGHT                */
 ,WeightUOMIdentity          varchar2(17)  /* HAPI_SHIPMENT_REPORT_CARRIER.WEIGHTUOMIDENTITY          */
 ,TotalVolume                number(19,9)  /* HAPI_SHIPMENT_REPORT_CARRIER.TOTALVOLUME                */
 ,VolumeUOMIdentity          varchar2(17)  /* HAPI_SHIPMENT_REPORT_CARRIER.VOLUMEUOMIDENTITY          */
 ,TotalHeight                number(9,4)   /* HAPI_SHIPMENT_REPORT_CARRIER.TOTALHEIGHT                */
 ,HeightUOMIdentity          varchar2(17)  /* HAPI_SHIPMENT_REPORT_CARRIER.HEIGHTUOMIDENTITY          */
 ,FirstPlannedDeliveryDtm    date          /* HAPI_SHIPMENT_REPORT_CARRIER.FIRSTPLANNEDDELIVERYDTM    */
 ,LastPlannedDeliveryDtm     date          /* HAPI_SHIPMENT_REPORT_CARRIER.LASTPLANNEDDELIVERYDTM     */
 ,UPDDTM                     date          /* HAPIRCV.UPDDTM                                          */
 ,PROID                      varchar2(35)  /* HAPIRCV.PROID                                           */
);

create table EXT_SHIPMENT_REPORT_HEAD_TEXT(
  HAPIRCV_ID            varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                   */
 ,OPCODE                varchar2(1)   /* .                                                    */
 ,ASNIdentity           varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD_TEXT.ASNIDENTITY           */
 ,ASNSequenceNumber     number(3)     /* HAPI_SHIPMENT_REPORT_HEAD_TEXT.ASNSEQUENCENUMBER     */
 ,CustomerOrderNumber   varchar2(35)  /* HAPI_SHIPMENT_REPORT_HEAD_TEXT.CUSTOMERORDERNUMBER   */
 ,CustomerOrderSequence number(3)     /* HAPI_SHIPMENT_REPORT_HEAD_TEXT.CUSTOMERORDERSEQUENCE */
 ,TextFunction          varchar2(3)   /* HAPI_SHIPMENT_REPORT_HEAD_TEXT.TEXTFUNCTION          */
 ,Text                  varchar2(400) /* HAPI_SHIPMENT_REPORT_HEAD_TEXT.TEXT                  */
 ,UPDDTM                date          /* HAPIRCV.UPDDTM                                       */
 ,PROID                 varchar2(35)  /* HAPIRCV.PROID                                        */
);

create table EXT_SHIPMENT_REPORT_LINE(
  HAPIRCV_ID                    varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                      */
 ,OPCODE                        varchar2(1)   /* .                                                       */
 ,ASNIdentity                   varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.ASNIDENTITY                   */
 ,ASNSequenceNumber             number(3)     /* HAPI_SHIPMENT_REPORT_LINE.ASNSEQUENCENUMBER             */
 ,ShipFromPartyNodeIdentity     varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPFROMPARTYNODEIDENTITY     */
 ,ASNLineNumber                 number(5)     /* HAPI_SHIPMENT_REPORT_LINE.ASNLINENUMBER                 */
 ,ASNLineSequenceNumber         number(3)     /* HAPI_SHIPMENT_REPORT_LINE.ASNLINESEQUENCENUMBER         */
 ,LoadCarrierIdentity           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.LOADCARRIERIDENTITY           */
 ,ClientIdentity                varchar2(17)  /* HAPI_SHIPMENT_REPORT_LINE.CLIENTIDENTITY                */
 ,ShipFromPartyIdentity         varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPFROMPARTYIDENTITY         */
 ,ShipToPartyNodeIdentity       varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOPARTYNODEIDENTITY       */
 ,ShipToPartyIdentity           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOPARTYIDENTITY           */
 ,ShipToPartyQualifier          varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOPARTYQUALIFIER          */
 ,ShipToCustomerNodeIdentity    varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOCUSTOMERNODEIDENTITY    */
 ,ShipToCustomerIdentity        varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOCUSTOMERIDENTITY        */
 ,ShipToCustomerQualifier       varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOCUSTOMERQUALIFIER       */
 ,SellToCustomerIdentity        varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SELLTOCUSTOMERIDENTITY        */
 ,SellToCustomerQualifier       varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.SELLTOCUSTOMERQUALIFIER       */
 ,OwnerIdentity                 varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.OWNERIDENTITY                 */
 ,OwnerIdentityAtShipToCustomer varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.OWNERIDENTITYATSHIPTOCUSTOMER */
 ,VendorIdentity                varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.VENDORIDENTITY                */
 ,VendorPartyNodeIdentity       varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.VENDORPARTYNODEIDENTITY       */
 ,ProductNumber                 varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTNUMBER                 */
 ,ProductDescription            varchar2(256) /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTDESCRIPTION            */
 ,ProductNumberType             varchar2(1)   /* HAPI_SHIPMENT_REPORT_LINE.PRODUCTNUMBERTYPE             */
 ,AlternativeProductNumber      varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.ALTERNATIVEPRODUCTNUMBER      */
 ,AlternativeProductDescription varchar2(256) /* HAPI_SHIPMENT_REPORT_LINE.ALTERNATIVEPRODUCTDESCRIPTION */
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
 ,PackingMaterial               varchar2(1)   /* HAPI_SHIPMENT_REPORT_LINE.PACKINGMATERIAL               */
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
 ,ShipToVendorIdentity          varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOVENDORIDENTITY          */
 ,ShipToVendorQualifier         varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.SHIPTOVENDORQUALIFIER         */
 ,DiscrepancyQuantity           number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.DISCREPANCYQUANTITY           */
 ,DiscrepancyCode               varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE.DISCREPANCYCODE               */
 ,DiscrepancyText               varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE.DISCREPANCYTEXT               */
 ,QuantityUpperTolerance        number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.QUANTITYUPPERTOLERANCE        */
 ,CatchMeasureLowerTolerance    number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.CATCHMEASURELOWERTOLERANCE    */
 ,CatchMeasureUpperTolerance    number(20,6)  /* HAPI_SHIPMENT_REPORT_LINE.CATCHMEASUREUPPERTOLERANCE    */
 ,ReserveForAssembly            varchar2(1)   /* HAPI_SHIPMENT_REPORT_LINE.RESERVEFORASSEMBLY            */
 ,UPDDTM                        date          /* HAPIRCV.UPDDTM                                          */
 ,PROID                         varchar2(35)  /* HAPIRCV.PROID                                           */
);

create table EXT_SHIPMENT_REPORT_LINE_TEXT(
  HAPIRCV_ID            varchar2(35)  /* HAPIRCV.HAPIRCV_ID                                   */
 ,OPCODE                varchar2(1)   /* .                                                    */
 ,ASNIdentity           varchar2(35)  /* HAPI_SHIPMENT_REPORT_LINE_TEXT.ASNIDENTITY           */
 ,ASNSequenceNumber     number(3)     /* HAPI_SHIPMENT_REPORT_LINE_TEXT.ASNSEQUENCENUMBER     */
 ,ASNLineNumber         number(5)     /* HAPI_SHIPMENT_REPORT_LINE_TEXT.ASNLINENUMBER         */
 ,ASNLineSequenceNumber number(3)     /* HAPI_SHIPMENT_REPORT_LINE_TEXT.ASNLINESEQUENCENUMBER */
 ,TextFunction          varchar2(3)   /* HAPI_SHIPMENT_REPORT_LINE_TEXT.TEXTFUNCTION          */
 ,Text                  varchar2(400) /* HAPI_SHIPMENT_REPORT_LINE_TEXT.TEXT                  */
 ,UPDDTM                date          /* HAPIRCV.UPDDTM                                       */
 ,PROID                 varchar2(35)  /* HAPIRCV.PROID                                        */
);

create table EXT_CONF_OF_RECEIPT_HEAD(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                      */
 ,OPCODE                        varchar2(1)  /* .                                                       */
 ,AcknowledgeInboundAsnIdentity varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.ACKNOWLEDGEINBOUNDASNIDENTITY */
 ,ArrivalDateTime               date         /* HAPI_CONF_OF_RECEIPT_HEAD.ARRIVALDATETIME               */
 ,ShipToPartyNodeIdentity       varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPTOPARTYNODEIDENTITY       */
 ,ShipToPartyIdentity           varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPTOPARTYIDENTITY           */
 ,ShipToPartyQualifier          varchar2(3)  /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPTOPARTYQUALIFIER          */
 ,ShipFromPartyNodeIdentity     varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPFROMPARTYNODEIDENTITY     */
 ,ShipFromPartyIdentity         varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPFROMPARTYIDENTITY         */
 ,ShipFromPartyQualifier        varchar2(3)  /* HAPI_CONF_OF_RECEIPT_HEAD.SHIPFROMPARTYQUALIFIER        */
 ,InboundAsnIdentity            varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.INBOUNDASNIDENTITY            */
 ,InboundAsnSequenceNumber      number(3)    /* HAPI_CONF_OF_RECEIPT_HEAD.INBOUNDASNSEQUENCENUMBER      */
 ,ClientIdentity                varchar2(17) /* HAPI_CONF_OF_RECEIPT_HEAD.CLIENTIDENTITY                */
 ,ForwarderIdentity             varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.FORWARDERIDENTITY             */
 ,ReceiptType                   varchar2(2)  /* HAPI_CONF_OF_RECEIPT_HEAD.RECEIPTTYPE                   */
 ,PackingSlipNumber             varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.PACKINGSLIPNUMBER             */
 ,BillOfLadingNumber            varchar2(35) /* HAPI_CONF_OF_RECEIPT_HEAD.BILLOFLADINGNUMBER            */
 ,VehicleIdentity               varchar2(17) /* HAPI_CONF_OF_RECEIPT_HEAD.VEHICLEIDENTITY               */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                          */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                           */
);

create table EXT_CONF_OF_RECEIPT_CARRIER(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                         */
 ,OPCODE                        varchar2(1)  /* .                                                          */
 ,AcknowledgeInboundAsnIdentity varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.ACKNOWLEDGEINBOUNDASNIDENTITY */
 ,LoadCarrierIdentity           varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.LOADCARRIERIDENTITY           */
 ,ParentLoadCarrierIdentity     varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.PARENTLOADCARRIERIDENTITY     */
 ,LoadCarrierQualifier          varchar2(4)  /* HAPI_CONF_OF_RECEIPT_CARRIER.LOADCARRIERQUALIFIER          */
 ,ArrivalDateTime               date         /* HAPI_CONF_OF_RECEIPT_CARRIER.ARRIVALDATETIME               */
 ,ShipToPartyNodeIdentity       varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPTOPARTYNODEIDENTITY       */
 ,ShipToPartyIdentity           varchar2(35) /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPTOPARTYIDENTITY           */
 ,ShipToPartyQualifier          varchar2(3)  /* HAPI_CONF_OF_RECEIPT_CARRIER.SHIPTOPARTYQUALIFIER          */
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

create table EXT_CONF_OF_RECEIPT_LINE(
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
 ,PackageIdentity               varchar2(17) /* HAPI_CONF_OF_RECEIPT_LINE.PACKAGEIDENTITY               */
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
 ,CustomerOrderNumber           varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERORDERNUMBER           */
 ,CustomerOrderSequence         number(3)    /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERORDERSEQUENCE         */
 ,CustomerOrderLinePosition     number(5)    /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERORDERLINEPOSITION     */
 ,CustomerOrderLineKitPosition  number(2)    /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERORDERLINEKITPOSITION  */
 ,CustomerOrderLineSequence     number(3)    /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERORDERLINESEQUENCE     */
 ,ReturnsFinished               varchar2(1)  /* HAPI_CONF_OF_RECEIPT_LINE.RETURNSFINISHED               */
 ,DecidedActionCode             varchar2(2)  /* HAPI_CONF_OF_RECEIPT_LINE.DECIDEDACTIONCODE             */
 ,FromPartyId                   varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.FROMPARTYID                   */
 ,FromPartyQualifier            varchar2(3)  /* HAPI_CONF_OF_RECEIPT_LINE.FROMPARTYQUALIFIER            */
 ,CustomerReturnOrderNumber     varchar2(35) /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERRETURNORDERNUMBER     */
 ,CustomerReturnOrderSequence   number(3)    /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERRETURNORDERSEQUENCE   */
 ,CustomerReturnOrderLinePos    number(5)    /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERRETURNORDERLINEPOS    */
 ,CustomerReturnOrderLineKitPos number(2)    /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERRETURNORDERLINEKITPOS */
 ,CustomerReturnOrderLineSeq    number(3)    /* HAPI_CONF_OF_RECEIPT_LINE.CUSTOMERRETURNORDERLINESEQ    */
 ,DiscrepancyCode               varchar2(3)  /* HAPI_CONF_OF_RECEIPT_LINE.DISCREPANCYCODE               */
 ,DiscrepancyActionCode         varchar2(2)  /* HAPI_CONF_OF_RECEIPT_LINE.DISCREPANCYACTIONCODE         */
 ,MarketingLotIdentity          varchar2(20) /* HAPI_CONF_OF_RECEIPT_LINE.MARKETINGLOTIDENTITY          */
 ,UPDDTM                        date         /* HAPIRCV.UPDDTM                                          */
 ,PROID                         varchar2(35) /* HAPIRCV.PROID                                           */
);

create table EXT_INB_ORDER_COMPLETED(
  HAPIRCV_ID                    varchar2(35) /* HAPIRCV.HAPIRCV_ID                                         */
 ,OPCODE                        varchar2(1)  /* .                                                          */
 ,ClientIdentity                varchar2(17) /* HAPI_INBOUND_ORDER_COMPLETED.CLIENTIDENTITY                */
 ,WarehouseIdentity             varchar2(4)  /* HAPI_INBOUND_ORDER_COMPLETED.WAREHOUSEIDENTITY             */
 ,EmployeeIdentity              varchar2(35) /* HAPI_INBOUND_ORDER_COMPLETED.EMPLOYEEIDENTITY              */
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

create table EXT_TRPINSTR(
  HAPIRCV_ID                  varchar2(35)  /* HAPIRCV.HAPIRCV_ID                        */
 ,OPCODE                      varchar2(1)   /* .                                         */
 ,DepartureIdentity           varchar2(35)  /* HAPI_TRPINSTR.DEPARTUREIDENTITY           */
 ,Route_id                    varchar2(17)  /* HAPI_TRPINSTR.ROUTE_ID                    */
 ,Deliverymeth_id             varchar2(17)  /* HAPI_TRPINSTR.DELIVERYMETH_ID             */
 ,ShipDateTime                date          /* HAPI_TRPINSTR.SHIPDATETIME                */
 ,Departure_dtm               date          /* HAPI_TRPINSTR.DEPARTURE_DTM               */
 ,ShipFromPartyIdentity       varchar2(4)   /* HAPI_TRPINSTR.SHIPFROMPARTYIDENTITY       */
 ,ShipFromParty_Name          varchar2(175) /* HAPI_TRPINSTR.SHIPFROMPARTY_NAME          */
 ,ShipFromParty_Name2         varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_NAME2         */
 ,ShipFromParty_Name3         varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_NAME3         */
 ,ShipFromParty_Name4         varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_NAME4         */
 ,ShipFromParty_Name5         varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_NAME5         */
 ,ShipFromParty_Adr           varchar2(140) /* HAPI_TRPINSTR.SHIPFROMPARTY_ADR           */
 ,ShipFromParty_Adr2          varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_ADR2          */
 ,ShipFromParty_Adr3          varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_ADR3          */
 ,ShipFromParty_Adr4          varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_ADR4          */
 ,ShipFromParty_PostCode      varchar2(12)  /* HAPI_TRPINSTR.SHIPFROMPARTY_POSTCODE      */
 ,ShipFromParty_City          varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_CITY          */
 ,ShipFromParty_Country       varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTY_COUNTRY       */
 ,ShipFromParty_CountryCode   varchar2(6)   /* HAPI_TRPINSTR.SHIPFROMPARTY_COUNTRYCODE   */
 ,ShipFromPartyNodeIdentity   varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMPARTYNODEIDENTITY   */
 ,FreighterIdentity           varchar2(35)  /* HAPI_TRPINSTR.FREIGHTERIDENTITY           */
 ,FreName                     varchar2(175) /* HAPI_TRPINSTR.FRENAME                     */
 ,Fre_Adr                     varchar2(140) /* HAPI_TRPINSTR.FRE_ADR                     */
 ,Fre_PostCode                varchar2(12)  /* HAPI_TRPINSTR.FRE_POSTCODE                */
 ,Fre_City                    varchar2(35)  /* HAPI_TRPINSTR.FRE_CITY                    */
 ,Fre_Country                 varchar2(35)  /* HAPI_TRPINSTR.FRE_COUNTRY                 */
 ,Fre_CountryCode             varchar2(6)   /* HAPI_TRPINSTR.FRE_COUNTRYCODE             */
 ,Fre_Georef                  varchar2(35)  /* HAPI_TRPINSTR.FRE_GEOREF                  */
 ,ShipFromCompanyIdentity     varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANYIDENTITY     */
 ,ShipFromCompany_Name        varchar2(175) /* HAPI_TRPINSTR.SHIPFROMCOMPANY_NAME        */
 ,ShipFromCompany_Name2       varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_NAME2       */
 ,ShipFromCompany_Name3       varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_NAME3       */
 ,ShipFromCompany_Name4       varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_NAME4       */
 ,ShipFromCompany_Name5       varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_NAME5       */
 ,ShipFromCompany_Adr         varchar2(140) /* HAPI_TRPINSTR.SHIPFROMCOMPANY_ADR         */
 ,ShipFromCompany_Adr2        varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_ADR2        */
 ,ShipFromCompany_Adr3        varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_ADR3        */
 ,ShipFromCompany_Adr4        varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_ADR4        */
 ,Shipfromcompany_PostCode    varchar2(12)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_POSTCODE    */
 ,ShipFromCompany_City        varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_CITY        */
 ,ShipFromCompany_Country     varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_COUNTRY     */
 ,ShipFromCompany_CountryCode varchar2(6)   /* HAPI_TRPINSTR.SHIPFROMCOMPANY_COUNTRYCODE */
 ,ShipFromCompanyNodeIdentity varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANYNODEIDENTITY */
 ,ShipFromCompany_Phone       varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_PHONE       */
 ,ShipFromCompany_Email       varchar2(50)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_EMAIL       */
 ,ShipFromCompany_ContactName varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_CONTACTNAME */
 ,ShipFromCompany_TypeOfGoods varchar2(35)  /* HAPI_TRPINSTR.SHIPFROMCOMPANY_TYPEOFGOODS */
 ,UPDDTM                      date          /* HAPIRCV.UPDDTM                            */
 ,PROID                       varchar2(35)  /* HAPIRCV.PROID                             */
);

create table EXT_TRPINSTR_CONSIGNMENT(
  HAPIRCV_ID              varchar2(35) /* HAPIRCV.HAPIRCV_ID                                */
 ,OPCODE                  varchar2(1)  /* .                                                 */
 ,DepartureIdentity       varchar2(35) /* HAPI_TRPINSTR_CONSIGNMENT.DEPARTUREIDENTITY       */
 ,Consignment_id          varchar2(35) /* HAPI_TRPINSTR_CONSIGNMENT.CONSIGNMENT_ID          */
 ,ClientIdentity          varchar2(17) /* HAPI_TRPINSTR_CONSIGNMENT.CLIENTIDENTITY          */
 ,ShipToPartyIdentity     varchar2(35) /* HAPI_TRPINSTR_CONSIGNMENT.SHIPTOPARTYIDENTITY     */
 ,ShipToPartyNodeIdentity varchar2(35) /* HAPI_TRPINSTR_CONSIGNMENT.SHIPTOPARTYNODEIDENTITY */
 ,Fre_ConsignmentIdentity varchar2(35) /* HAPI_TRPINSTR_CONSIGNMENT.FRE_CONSIGNMENTIDENTITY */
 ,UPDDTM                  date         /* HAPIRCV.UPDDTM                                    */
 ,PROID                   varchar2(35) /* HAPIRCV.PROID                                     */
);

create table EXT_TRPINSTR_PARTY(
  HAPIRCV_ID                  varchar2(35)  /* HAPIRCV.HAPIRCV_ID                              */
 ,OPCODE                      varchar2(1)   /* .                                               */
 ,DepartureIdentity           varchar2(35)  /* HAPI_TRPINSTR_PARTY.DEPARTUREIDENTITY           */
 ,Consignment_id              varchar2(35)  /* HAPI_TRPINSTR_PARTY.CONSIGNMENT_ID              */
 ,ClientIdentity              varchar2(17)  /* HAPI_TRPINSTR_PARTY.CLIENTIDENTITY              */
 ,ShipToPartyIdentity         varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTYIDENTITY         */
 ,ShipToPartyNodeIdentity     varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTYNODEIDENTITY     */
 ,Arrival_dtm                 date          /* HAPI_TRPINSTR_PARTY.ARRIVAL_DTM                 */
 ,ShipToParty_Name            varchar2(175) /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_NAME            */
 ,ShipToParty_Name2           varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_NAME2           */
 ,ShipToParty_Name3           varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_NAME3           */
 ,ShipToParty_Name4           varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_NAME4           */
 ,ShipToParty_Name5           varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_NAME5           */
 ,ShipToParty_Adr             varchar2(140) /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_ADR             */
 ,ShipToParty_Adr2            varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_ADR2            */
 ,ShipToParty_Adr3            varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_ADR3            */
 ,ShipToParty_Adr4            varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_ADR4            */
 ,ShipToParty_PostCode        varchar2(12)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_POSTCODE        */
 ,ShipToParty_City            varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_CITY            */
 ,ShipToParty_Country         varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_COUNTRY         */
 ,ShipToParty_CountryCode     varchar2(6)   /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_COUNTRYCODE     */
 ,ShipToParty_Phone           varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_PHONE           */
 ,ShipToParty_Email           varchar2(50)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_EMAIL           */
 ,ShipToParty_ContactName     varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPTOPARTY_CONTACTNAME     */
 ,ShipFromCompanyIdentity     varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANYIDENTITY     */
 ,ShipFromCompany_Name        varchar2(175) /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_NAME        */
 ,ShipFromCompany_Name2       varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_NAME2       */
 ,ShipFromCompany_Name3       varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_NAME3       */
 ,ShipFromCompany_Name4       varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_NAME4       */
 ,ShipFromCompany_Name5       varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_NAME5       */
 ,ShipFromCompany_Adr         varchar2(140) /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_ADR         */
 ,ShipFromCompany_Adr2        varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_ADR2        */
 ,ShipFromCompany_Adr3        varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_ADR3        */
 ,ShipFromCompany_Adr4        varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_ADR4        */
 ,Shipfromcompany_PostCode    varchar2(12)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_POSTCODE    */
 ,ShipFromCompany_City        varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_CITY        */
 ,ShipFromCompany_Country     varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_COUNTRY     */
 ,ShipFromCompany_CountryCode varchar2(6)   /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_COUNTRYCODE */
 ,ShipFromCompanyNodeIdentity varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANYNODEIDENTITY */
 ,ShipFromCompany_Phone       varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_PHONE       */
 ,ShipFromCompany_Email       varchar2(50)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_EMAIL       */
 ,ShipFromCompany_ContactName varchar2(35)  /* HAPI_TRPINSTR_PARTY.SHIPFROMCOMPANY_CONTACTNAME */
 ,UPDDTM                      date          /* HAPIRCV.UPDDTM                                  */
 ,PROID                       varchar2(35)  /* HAPIRCV.PROID                                   */
);

create table EXT_TRPINSTR_CAR(
  HAPIRCV_ID                  varchar2(35)  /* HAPIRCV.HAPIRCV_ID                            */
 ,OPCODE                      varchar2(1)   /* .                                             */
 ,DepartureIdentity           varchar2(35)  /* HAPI_TRPINSTR_CAR.DEPARTUREIDENTITY           */
 ,Consignment_id              varchar2(35)  /* HAPI_TRPINSTR_CAR.CONSIGNMENT_ID              */
 ,ClientIdentity              varchar2(17)  /* HAPI_TRPINSTR_CAR.CLIENTIDENTITY              */
 ,ShipToPartyIdentity         varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOPARTYIDENTITY         */
 ,ShipToPartyNodeIdentity     varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOPARTYNODEIDENTITY     */
 ,LoadCarrierIdentity         varchar2(35)  /* HAPI_TRPINSTR_CAR.LOADCARRIERIDENTITY         */
 ,LoadCarrierQualifier        varchar2(4)   /* HAPI_TRPINSTR_CAR.LOADCARRIERQUALIFIER        */
 ,LoadCarrierType             varchar2(3)   /* HAPI_TRPINSTR_CAR.LOADCARRIERTYPE             */
 ,ExternalLoadCarrierType     varchar2(20)  /* HAPI_TRPINSTR_CAR.EXTERNALLOADCARRIERTYPE     */
 ,ParentLoadCarrierIdentity   varchar2(35)  /* HAPI_TRPINSTR_CAR.PARENTLOADCARRIERIDENTITY   */
 ,NetWeight                   number(16,6)  /* HAPI_TRPINSTR_CAR.NETWEIGHT                   */
 ,NetVolume                   number(16,6)  /* HAPI_TRPINSTR_CAR.NETVOLUME                   */
 ,TotalWeight                 number(16,6)  /* HAPI_TRPINSTR_CAR.TOTALWEIGHT                 */
 ,TotalVolume                 number(16,6)  /* HAPI_TRPINSTR_CAR.TOTALVOLUME                 */
 ,BottomArea                  number(10,6)  /* HAPI_TRPINSTR_CAR.BOTTOMAREA                  */
 ,LoadingMeter                number(16,6)  /* HAPI_TRPINSTR_CAR.LOADINGMETER                */
 ,Piknopaks                   number(6)     /* HAPI_TRPINSTR_CAR.PIKNOPAKS                   */
 ,ProductTransportIdentity    varchar2(5)   /* HAPI_TRPINSTR_CAR.PRODUCTTRANSPORTIDENTITY    */
 ,Fre_Party_Id                varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_ID                */
 ,Fre_Party_Name              varchar2(175) /* HAPI_TRPINSTR_CAR.FRE_PARTY_NAME              */
 ,Fre_Party_Name2             varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_NAME2             */
 ,Fre_Party_Name3             varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_NAME3             */
 ,Fre_Party_Name4             varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_NAME4             */
 ,Fre_Party_Name5             varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_NAME5             */
 ,Fre_Party_Adr               varchar2(140) /* HAPI_TRPINSTR_CAR.FRE_PARTY_ADR               */
 ,Fre_Party_Adr2              varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_ADR2              */
 ,Fre_Party_Adr3              varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_ADR3              */
 ,Fre_Party_Adr4              varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_ADR4              */
 ,Fre_Party_PostCode          varchar2(12)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_POSTCODE          */
 ,Fre_Party_City              varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_CITY              */
 ,Fre_Party_Country           varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_COUNTRY           */
 ,Fre_Party_CountryCode       varchar2(6)   /* HAPI_TRPINSTR_CAR.FRE_PARTY_COUNTRYCODE       */
 ,Fre_Party_Georef            varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_PARTY_GEOREF            */
 ,Freight_payment_code        varchar2(1)   /* HAPI_TRPINSTR_CAR.FREIGHT_PAYMENT_CODE        */
 ,Fre_Customer_Name_other     varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_CUSTOMER_NAME_OTHER     */
 ,NumberOfLoadCarriers        number(6)     /* HAPI_TRPINSTR_CAR.NUMBEROFLOADCARRIERS        */
 ,Numberofconsignmentpackages number(6)     /* HAPI_TRPINSTR_CAR.NUMBEROFCONSIGNMENTPACKAGES */
 ,CustomerOrderNumber         varchar2(35)  /* HAPI_TRPINSTR_CAR.CUSTOMERORDERNUMBER         */
 ,CustomerOrderSequence       number(3)     /* HAPI_TRPINSTR_CAR.CUSTOMERORDERSEQUENCE       */
 ,ShippingInstruction         varchar2(400) /* HAPI_TRPINSTR_CAR.SHIPPINGINSTRUCTION         */
 ,TransportInstruction        varchar2(400) /* HAPI_TRPINSTR_CAR.TRANSPORTINSTRUCTION        */
 ,Fre_Customer_Id             varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_CUSTOMER_ID             */
 ,Fre_Customer_Id_Eur         varchar2(35)  /* HAPI_TRPINSTR_CAR.FRE_CUSTOMER_ID_EUR         */
 ,ASNIdentity                 varchar2(35)  /* HAPI_TRPINSTR_CAR.ASNIDENTITY                 */
 ,ASNIdentitySequence         number(3)     /* HAPI_TRPINSTR_CAR.ASNIDENTITYSEQUENCE         */
 ,ShipFromCompanyIdentity     varchar2(17)  /* HAPI_TRPINSTR_CAR.SHIPFROMCOMPANYIDENTITY     */
 ,ShipToCustomerIdentity      varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMERIDENTITY      */
 ,ShipToCustomer_Name         varchar2(175) /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_NAME         */
 ,ShipToCustomer_Name2        varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_NAME2        */
 ,ShipToCustomer_Name3        varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_NAME3        */
 ,ShipToCustomer_Name4        varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_NAME4        */
 ,ShipToCustomer_Name5        varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_NAME5        */
 ,ShipToCustomer_Adr          varchar2(140) /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_ADR          */
 ,ShipToCustomer_Adr2         varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_ADR2         */
 ,ShipToCustomer_Adr3         varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_ADR3         */
 ,ShipToCustomer_Adr4         varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_ADR4         */
 ,ShipToCustomer_PostCode     varchar2(12)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_POSTCODE     */
 ,ShipToCustomer_City         varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_CITY         */
 ,ShipToCustomer_Country      varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_COUNTRY      */
 ,ShipToCustomer_CountryCode  varchar2(6)   /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_COUNTRYCODE  */
 ,ShipToCustomerNodeIdentity  varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMERNODEIDENTITY  */
 ,ShipToCustomer_Phone        varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_PHONE        */
 ,ShipToCustomer_Email        varchar2(50)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_EMAIL        */
 ,ShipToCustomer_ContactName  varchar2(35)  /* HAPI_TRPINSTR_CAR.SHIPTOCUSTOMER_CONTACTNAME  */
 ,VehicleIdentity             varchar2(35)  /* HAPI_TRPINSTR_CAR.VEHICLEIDENTITY             */
 ,VLUIdentity                 varchar2(35)  /* HAPI_TRPINSTR_CAR.VLUIDENTITY                 */
 ,UPDDTM                      date          /* HAPIRCV.UPDDTM                                */
 ,PROID                       varchar2(35)  /* HAPIRCV.PROID                                 */
);

create table EXT_TRPINSTR_ART(
  HAPIRCV_ID              varchar2(35) /* HAPIRCV.HAPIRCV_ID                        */
 ,OPCODE                  varchar2(1)  /* .                                         */
 ,DepartureIdentity       varchar2(35) /* HAPI_TRPINSTR_ART.DEPARTUREIDENTITY       */
 ,Consignment_id          varchar2(35) /* HAPI_TRPINSTR_ART.CONSIGNMENT_ID          */
 ,ClientIdentity          varchar2(17) /* HAPI_TRPINSTR_ART.CLIENTIDENTITY          */
 ,ShipToPartyIdentity     varchar2(35) /* HAPI_TRPINSTR_ART.SHIPTOPARTYIDENTITY     */
 ,ShipToPartyNodeIdentity varchar2(35) /* HAPI_TRPINSTR_ART.SHIPTOPARTYNODEIDENTITY */
 ,LoadCarrierIdentity     varchar2(35) /* HAPI_TRPINSTR_ART.LOADCARRIERIDENTITY     */
 ,ProductIdentity         varchar2(35) /* HAPI_TRPINSTR_ART.PRODUCTIDENTITY         */
 ,UPDDTM                  date         /* HAPIRCV.UPDDTM                            */
 ,PROID                   varchar2(35) /* HAPIRCV.PROID                             */
);

create table EXT_TRPINSTR_ARTCOD(
  HAPIRCV_ID              varchar2(35)  /* HAPIRCV.HAPIRCV_ID                           */
 ,OPCODE                  varchar2(1)   /* .                                            */
 ,DepartureIdentity       varchar2(35)  /* HAPI_TRPINSTR_ARTCOD.DEPARTUREIDENTITY       */
 ,Consignment_id          varchar2(35)  /* HAPI_TRPINSTR_ARTCOD.CONSIGNMENT_ID          */
 ,ClientIdentity          varchar2(17)  /* HAPI_TRPINSTR_ARTCOD.CLIENTIDENTITY          */
 ,ShipToPartyIdentity     varchar2(35)  /* HAPI_TRPINSTR_ARTCOD.SHIPTOPARTYIDENTITY     */
 ,ShipToPartyNodeIdentity varchar2(35)  /* HAPI_TRPINSTR_ARTCOD.SHIPTOPARTYNODEIDENTITY */
 ,LoadCarrierIdentity     varchar2(35)  /* HAPI_TRPINSTR_ARTCOD.LOADCARRIERIDENTITY     */
 ,ProductIdentity         varchar2(35)  /* HAPI_TRPINSTR_ARTCOD.PRODUCTIDENTITY         */
 ,Type                    varchar2(17)  /* HAPI_TRPINSTR_ARTCOD.TYPE                    */
 ,Description             varchar2(255) /* HAPI_TRPINSTR_ARTCOD.DESCRIPTION             */
 ,Code                    varchar2(255) /* HAPI_TRPINSTR_ARTCOD.CODE                    */
 ,UPDDTM                  date          /* HAPIRCV.UPDDTM                               */
 ,PROID                   varchar2(35)  /* HAPIRCV.PROID                                */
);

create table EXT_TRANSPORT_PLAN_HEAD(
  HAPIRCV_ID            varchar2(35) /* HAPIRCV.HAPIRCV_ID                             */
 ,OPCODE                varchar2(1)  /* .                                              */
 ,TransportOrderNo      varchar2(35) /* HAPI_TRANSPORT_PLAN_HEAD.TRANSPORTORDERNO      */
 ,ClientIdentity        varchar2(17) /* HAPI_TRANSPORT_PLAN_HEAD.CLIENTIDENTITY        */
 ,CustomerOrderNumber   varchar2(35) /* HAPI_TRANSPORT_PLAN_HEAD.CUSTOMERORDERNUMBER   */
 ,CustomerOrderSequence number(3)    /* HAPI_TRANSPORT_PLAN_HEAD.CUSTOMERORDERSEQUENCE */
 ,Custypid              varchar2(8)  /* HAPI_TRANSPORT_PLAN_HEAD.CUSTYPID              */
 ,UPDDTM                date         /* HAPIRCV.UPDDTM                                 */
 ,PROID                 varchar2(35) /* HAPIRCV.PROID                                  */
);

create table EXT_TRANSPORT_PLAN_LINE(
  HAPIRCV_ID                   varchar2(35) /* HAPIRCV.HAPIRCV_ID                                    */
 ,OPCODE                       varchar2(1)  /* .                                                     */
 ,TransportOrderNo             varchar2(35) /* HAPI_TRANSPORT_PLAN_LINE.TRANSPORTORDERNO             */
 ,ClientIdentity               varchar2(17) /* HAPI_TRANSPORT_PLAN_LINE.CLIENTIDENTITY               */
 ,CustomerOrderNumber          varchar2(35) /* HAPI_TRANSPORT_PLAN_LINE.CUSTOMERORDERNUMBER          */
 ,CustomerOrderSequence        number(3)    /* HAPI_TRANSPORT_PLAN_LINE.CUSTOMERORDERSEQUENCE        */
 ,CustomerOrderLinePosition    number(5)    /* HAPI_TRANSPORT_PLAN_LINE.CUSTOMERORDERLINEPOSITION    */
 ,CustomerOrderLineKitPosition number(2)    /* HAPI_TRANSPORT_PLAN_LINE.CUSTOMERORDERLINEKITPOSITION */
 ,CustomerOrderLineSequence    number(3)    /* HAPI_TRANSPORT_PLAN_LINE.CUSTOMERORDERLINESEQUENCE    */
 ,CustomerOrderLineType        varchar2(1)  /* HAPI_TRANSPORT_PLAN_LINE.CUSTOMERORDERLINETYPE        */
 ,LoadCarrierIdentity          varchar2(35) /* HAPI_TRANSPORT_PLAN_LINE.LOADCARRIERIDENTITY          */
 ,LoadCarrierType              varchar2(3)  /* HAPI_TRANSPORT_PLAN_LINE.LOADCARRIERTYPE              */
 ,PalletPackage                varchar2(1)  /* HAPI_TRANSPORT_PLAN_LINE.PALLETPACKAGE                */
 ,LoadCarrierEnclosed          varchar2(1)  /* HAPI_TRANSPORT_PLAN_LINE.LOADCARRIERENCLOSED          */
 ,LoadCarrierVolume            number(19,9) /* HAPI_TRANSPORT_PLAN_LINE.LOADCARRIERVOLUME            */
 ,LoadCarrierWeight            number(16,6) /* HAPI_TRANSPORT_PLAN_LINE.LOADCARRIERWEIGHT            */
 ,LoadCarrierHeight            number(9,4)  /* HAPI_TRANSPORT_PLAN_LINE.LOADCARRIERHEIGHT            */
 ,ProductNumber                varchar2(35) /* HAPI_TRANSPORT_PLAN_LINE.PRODUCTNUMBER                */
 ,OrderedQuantity              number(20,6) /* HAPI_TRANSPORT_PLAN_LINE.ORDEREDQUANTITY              */
 ,Volume                       number(19,9) /* HAPI_TRANSPORT_PLAN_LINE.VOLUME                       */
 ,Weight                       number(16,6) /* HAPI_TRANSPORT_PLAN_LINE.WEIGHT                       */
 ,Height                       number(9,4)  /* HAPI_TRANSPORT_PLAN_LINE.HEIGHT                       */
 ,UPDDTM                       date         /* HAPIRCV.UPDDTM                                        */
 ,PROID                        varchar2(35) /* HAPIRCV.PROID                                         */
);

