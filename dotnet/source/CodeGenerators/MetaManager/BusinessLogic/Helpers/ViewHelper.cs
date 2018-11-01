using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;
using Cdc.MetaManager.DataAccess;
using System.Diagnostics;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Reflection;
using Domain = Cdc.MetaManager.DataAccess.Domain;
using Spring.Data.NHibernate.Support;
using Spring.Data.NHibernate;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class ViewHelper : IViewHelper
    {
        private IModelService ModelService { get; set; }

        [Transaction(ReadOnly = false)]
        public void InitializeUXComponent(UXComponent component)
        {
            InitializeUXComponent(component, new Dictionary<Guid, IDomainObject>());
        }

        [Transaction(ReadOnly = false)]
        public void InitializeUXComponent(UXComponent component, Dictionary<Guid, IDomainObject> loadedObjects)
        {
            if (component != null)
            {
                ResolveDomainReferences(component, loadedObjects);

                if ((component is IBindable) && (component.Hint == null))
                {
                    IBindable bindable = component as IBindable;
                    component.Hint = GetHintForMappedComponent(bindable);

                    if (component.Hint != null)
                    {
                        NHibernateUtil.Initialize(component.Hint.Text);
                    }
                }

                UXContainer container = null;

                if (component is UXGroupBox)
                    container = (component as UXGroupBox).Container;
                else
                    container = component as UXContainer;

                if (container != null)
                {
                    foreach (UXComponent child in container.Children)
                        InitializeUXComponent(child, loadedObjects);
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public void SetHints(View view)
        {
            foreach (UXComponent component in GetAllComponents<UXComponent>(view.VisualTree))
            {
                SetHint(component);
            }
        }

        [Transaction(ReadOnly = false)]
        private void SetHint(UXComponent component)
        {
            
            if (component is IBindable)
            {
                IBindable bindable = component as IBindable;
                Hint propertyHint = GetHintForMappedComponent(bindable);

                if (component.Hint != null && propertyHint != null)
                {
                    if ((propertyHint.Id == component.HintId) || (propertyHint.Id != component.HintId && !ExistsInHintCollection(component)))
                    {
                        component.Hint = null;
                        component.HintId = Guid.Empty;
                    }
                }

            }
        }

        [Transaction(ReadOnly = false)]
        private bool ExistsInHintCollection(UXComponent comp)
        {
            HintCollection hintCollection = ModelService.GetDomainObject<HintCollection>(comp.Hint.HintCollection.Id);
              
            foreach (Hint hint in hintCollection.Hints)
            {
                if (hint.Id == comp.HintId)
                    return true;
            }
            
            return false;
        }


        [Transaction(ReadOnly = false)]
        public void FindComponentServiceReferences(UXComponent component, IDictionary<Guid, Service> references)
        {
            if (component != null)
            {
                foreach (PropertyInfo info in component.GetType().GetProperties())
                {
                    if (info.GetCustomAttributes(typeof(DomainReferenceAttribute), false).Count() > 0)
                    {
                        if (info.PropertyType == typeof(ServiceMethod))
                        {
                            ServiceMethod method = info.GetValue(component, null) as ServiceMethod;

                            if (method != null)
                            {
                                if (method.Service != null)
                                {
                                    if (!references.ContainsKey(method.Service.Id))
                                        references.Add(method.Service.Id, method.Service);
                                }
                            }
                        }
                    }
                }

                UXContainer container = null;

                if (component is UXGroupBox)
                    container = (component as UXGroupBox).Container;
                else
                    container = component as UXContainer;

                if (container != null)
                {
                    foreach (UXComponent child in container.Children)
                        FindComponentServiceReferences(child, references);
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public View ConnectServiceMethodToView(Guid viewId, Guid serviceMethodId)
        {
            View theView = ModelService.GetDomainObject<View>(viewId);
            ServiceMethod theServiceMethod = ModelService.GetDomainObject<ServiceMethod>(serviceMethodId);

            if (theView.ServiceMethod != null)
            {
                if (theView.ServiceMethod.Id == theServiceMethod.Id)
                {
                    return theView;
                }
                else
                {
                    List<IDomainObject> mappedPropertiesToDelete = new List<IDomainObject>();
                    mappedPropertiesToDelete.AddRange(theView.RequestMap.MappedProperties);
                    mappedPropertiesToDelete.AddRange(theView.ResponseMap.MappedProperties);

                    ModelService.StartSynchronizePropertyMapsInObjects(theView, null , mappedPropertiesToDelete);
                }
            }


            theView.ServiceMethod = theServiceMethod;

            ModelService.CreateAndSynchronizePropertyMaps(theServiceMethod, theView);

            //Spara vyn

            return theView;
        }

        private Hint GetHintForMappedComponent(IBindable bindable)
        {
            return MappedPropertyHelper.GetHintForMappedProperty(bindable.MappedProperty);
        }

        private void ResolveDomainReferences(UXComponent component, Dictionary<Guid, IDomainObject> loadedObjects)
        {
            //New model
            foreach (PropertyInfo info in component.GetType().GetProperties())
            {

                if (info.GetCustomAttributes(typeof(DomainReferenceAttribute), true).Count() > 0)
                {
                    PropertyInfo idProperty = component.GetType().GetProperty(info.Name + "Id");

                    if (idProperty == null)
                        throw new Exception("Id property not found.");

                    Guid id = (Guid)idProperty.GetValue(component, null);

                    if (id != Guid.Empty)
                    {
                        IDomainObject refObj = null;
                        if (DataAccess.DomainXmlSerializationHelper.DontUseDBorSession)
                        {
                            if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.ContainsKey(info.PropertyType))
                            {
                                if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[info.PropertyType].ContainsKey(id))
                                {
                                    refObj = DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[info.PropertyType][id];
                                }
                            }
                        }
                        else
                        {
                            if (loadedObjects.ContainsKey(id))
                            {
                                refObj = loadedObjects[id];
                            }
                            else
                            {
                                refObj = ModelService.GetDomainObject(id, info.PropertyType);
                            }
                        }

                        if (refObj != null)
                        {
                            info.SetValue(component, refObj, null);
                        }
                        else
                        {
                            info.SetValue(component, null, null);
                            idProperty.SetValue(component, Guid.Empty, null);
                        }
                    }
                }
            }
        }



        public static void ReplaceComponentInVisualTree(UXComponent oldComponent, UXComponent newComponent)
        {
            UXLayoutGrid grid = oldComponent.Parent as UXLayoutGrid;

            if (grid != null)
            {
                UXLayoutGridCell[] cells = grid.Cells;

                foreach (UXLayoutGridCell cell in cells)
                {
                    if (cell.Component == oldComponent)
                        cell.Component = newComponent;
                }

                grid.Cells = cells;
            }
            else
            {
                UXContainer container = oldComponent.Parent as UXContainer;

                if (newComponent == null)
                {
                    container.Children.Remove(oldComponent);
                }
                else
                {
                    int index = container.Children.IndexOf(oldComponent);
                    container.Children[index] = newComponent;
                }
            }
        }
        
        [Transaction(ReadOnly = false)]
        public View CopyView(View view)
        {
            View copy = new View();
            copy.Id = Guid.NewGuid();
            copy.Name = view.Name + "_Copy";
            copy.VisualTree = null;
            copy.VisualTreeXml = view.VisualTreeXml;
            copy.Title = view.Title;
            copy.RequestMap = new PropertyMap();
            copy.ResponseMap = new PropertyMap();
            copy.BusinessEntity = view.BusinessEntity;
            copy.Application = view.Application;
            copy.State = VersionControlledObjectStat.New;
            copy.IsLocked = true;
            copy.LockedBy = Environment.UserName;
            copy.LockedDate = DateTime.Now;
            copy.Type = view.Type;
            
            foreach (UXComponent component in GetAllComponents<UXComponent>(copy.VisualTree))
            {
                PropertyInfo metaInfo = component.GetType().GetProperty("MetaId");

                if (metaInfo != null)
                {
                    metaInfo.SetValue(component, Guid.NewGuid().ToString(), null);
                }

                foreach (PropertyInfo pi in component.GetType().GetProperties())
                {
                    if (pi.PropertyType == typeof(PropertyMap))
                    {
                        pi.SetValue(component, null, null);
                    }
                    else if (pi.PropertyType == typeof(DataSource))
                    {
                        pi.SetValue(component, null, null);
                    }
                    else if (pi.PropertyType == typeof(MappedProperty))
                    {
                        pi.SetValue(component, null, null);
                    }
                }
            }

            copy = (View)ModelService.SaveDomainObject(copy);

            ConnectServiceMethodToView(copy.Id, view.ServiceMethod.Id);

            copy = ModelService.GetDomainObject<View>(copy.Id);

            foreach (UXComponent component in GetAllComponents<UXComponent>(copy.VisualTree))
            { 
                var originalComponent = (from c in GetAllComponents<UXComponent>(view.VisualTree)
                                  where c.Name == component.Name
                                  select c).FirstOrDefault();

                if (originalComponent != null)
                {
                    IBindable bindable = component as IBindable;

                    if (bindable != null && bindable.DataSource == null)
                    {
                        MappedProperty originalProperty = ((IBindable)originalComponent).MappedProperty;

                        if (originalProperty != null)
                        {
                            var target = (from m in copy.ResponseMap.MappedProperties
                                              where m.Name == originalProperty.Name
                                              select m).FirstOrDefault();

                            bindable.MappedProperty = target;
                        }
                    }
                }
            }

            copy = (View)ModelService.SaveDomainObject(copy);

            return copy;
        }

        public static IList<TType> GetAllComponents<TType>(UXComponent root)
            where TType : class
        {
            IList<TType> components = new List<TType>();
            GetAllComponents<TType>(components, root);

            return components;
        }

        private static void GetAllComponents<TType>(IList<TType> components, UXComponent component)
            where TType : class
        {
            if (component is TType)
                components.Add(component as TType);

            UXContainer container = null;

            if (component is UXGroupBox)
                container = (component as UXGroupBox).Container;
            else if (component is UXViewBox)
                container = (component as UXViewBox).View.VisualTree;
            else
                container = component as UXContainer;

            if (container != null)
            {
                foreach (UXComponent child in container.Children)
                    GetAllComponents(components, child);
            }
        }

        public static string GetDefaultComponentName(IEnumerable<string> existingNames, Type componentType)
        {
            string componentName = null;

            VisualDesignerAttribute attribute = Attribute.GetCustomAttribute(componentType, typeof(VisualDesignerAttribute)) as VisualDesignerAttribute;

            if (attribute != null)
            {
                if (!string.IsNullOrEmpty(attribute.ComponentName))
                {
                    componentName = attribute.ComponentName;
                }
            }

            if (string.IsNullOrEmpty(componentName))
            {
                if (componentType.Name.StartsWith("UX"))
                {
                    componentName = componentType.Name.Replace("UX", "");
                }
                else
                {
                    componentName = componentType.Name;
                }
            }

            string name = null;
            Random rnd = new Random();

            do
            {
                name = string.Format("{0}{1}", componentName, rnd.Next(1, 9999).ToString());
            }
            while (existingNames.Any(n => n.ToLower() == name.ToLower()));

            return name;
        }

    }

}
