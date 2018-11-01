using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.BusinessLogic
{
    //Delegates used by BusinessLogic classes
    //--------------------------------------------------------------------------------------------------------------------------------------------------------
    public delegate void DomainObjectDeletedDelegate(Guid Id, Type objectType);
    public delegate void DomainObjectAddedDelegate(Guid Id, Type objectType);
    public delegate void DomainObjectChangedDelegate(Guid Id, Type objectType);
    //--------------------------------------------------------------------------------------------------------------------------------------------------------



    //Delegates used by GUI 
    //--------------------------------------------------------------------------------------------------------------------------------------------------------
    public enum ObjectChangeTypes
    {
        Changed,
        Added,
        Deleted
    }

    public delegate void DomainObjectChangeDelegate(Guid Id, Type objectType, ObjectChangeTypes objectChangeType);
    //--------------------------------------------------------------------------------------------------------------------------------------------------------

    public interface IModelChangeNotificationService
    {
        event DomainObjectChangeDelegate DomainObjectChange; 
    }
}
