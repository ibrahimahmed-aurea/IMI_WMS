using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;


namespace Cdc.MetaManager.DataAccess.Domain
{

    [ClassStorageHint(TableName = "Dialog")]
    public class Dialog : IMappableUXObject, ILocalizable, IVersionControlled, IXmlSerializable
    {
        public Dialog()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)] 
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [Browsable(false)]
        [PropertyStorageHint(Column = "RootViewId", IsMandatory = false, Cascade = CascadeAssociationOperation.SaveUpdate , Lazy = true, ForeignKey = "FK_Dialog_ViewNode")]
        public virtual ViewNode RootViewNode { get; set; }

        private IList<ViewNode> viewNodes;

        [Browsable(false)]
        [DataAccess.DomainXmlById]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<ViewNode> ViewNodes
        {
            get
            {
                if (viewNodes == null)
                {
                    viewNodes = new List<ViewNode>();
                }
                return viewNodes;
            }
            set { viewNodes = value; }

        }

        [PropertyStorageHint(Length = 100, IsMandatory = true, UniqueKey="UNQ_Dialog_Name_Module")]
        public virtual string Name { get; set; }

        [PropertyStorageHint(Length = 100, IsMandatory = true)]
        [Localizable]
        public virtual string Title { get; set; }

        [Browsable(false)]
        [Search(SearchType=SearchTypes.FreeText)]
        [PropertyStorageHint(Length = 100, IsMandatory = false)]
        public virtual string OriginalDialogName { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ModuleId", IsMandatory = true, ForeignKey = "FK_Dialog_Module", UniqueKey = "UNQ_Dialog_Name_Module")]
        public virtual Module Module { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "InterfaceViewId", IsMandatory = false, ForeignKey = "FK_Dialog_View")]
        public virtual View InterfaceView { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "SearchPanelViewId", IsMandatory = false, ForeignKey = "FK_Dialog_SearchView", Cascade = CascadeAssociationOperation.None)]
        public virtual View SearchPanelView { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual DialogType Type { get; set; }


        [PropertyStorageHint(Length = 40, IsMandatory = false)]
        public virtual string CreatorName { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual UXActionType ActionType
        {
            get
            {
                return UXActionType.Dialog;
            }
        }

        [PropertyStorageHint(Column = "EnableImport", IsMandatory = false)]
        public virtual bool EnableImport { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        #region ILocalizable Members
        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string MetaId
        {
            get
            {
                return Id.ToString();
            }
        }

        #endregion

        #region IVersionControlled Members

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool IsLocked { get; set; }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Length = 40, IsMandatory = false)]
        public virtual string LockedBy { get; set; }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = false)]
        public virtual DateTime? LockedDate { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string RepositoryFileName
        {
            get { return this.GetType().Name + "_" + this.Id.ToString(); }
        }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = true)]
        public virtual VersionControlledObjectStat State { get; set; }

        #endregion

        #region IXmlSerializable Members

        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            DataAccess.DomainXmlSerializationHelper.ReadXML(this, reader);
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            DataAccess.DomainXmlSerializationHelper.WriteXML(this, writer);
        }

        #endregion

        #region IEquatable<IDomainObject> Members

        public virtual bool Equals(IDomainObject other)
        {
            if (other == null) { return false; }

            if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(this) == NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(other))
            {
                if (this.Id == other.Id)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
