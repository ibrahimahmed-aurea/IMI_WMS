using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class HintCollectionHelper : Cdc.MetaManager.BusinessLogic.Helpers.IHintCollectionHelper
    {
        private IModelService ModelService { get; set; }
        private IConfigurationManagementService ConfigurationManagementService { get; set; }

        public void UpdateHints(DataModelChanges detectedChanges, HintCollection hintCollection)
        {
            hintCollection = ModelService.GetDomainObject<HintCollection>(hintCollection.Id);

            if (detectedChanges.Where(c => c.Value.ContainDataModelChangeType(DataModelChangeType.NewHint) || c.Value.ContainDataModelChangeType(DataModelChangeType.ModifiedHint)
                                        || c.Value.ContainDataModelChangeType(DataModelChangeType.DeletedHint)).Count() > 0)
            {
                ConfigurationManagementService.CheckOutDomainObject(hintCollection.Id, typeof(HintCollection));

                // Check if ther is any New Hint that neads to be saved before the BusinessEntities
                foreach (KeyValuePair<object, DataModelChange> keyValue in detectedChanges)
                {

                    if (keyValue.Value.ContainDataModelChangeType(DataModelChangeType.NewHint) && keyValue.Value.Apply)
                    {
                        if (keyValue.Key is Property)
                        {
                            Property property = (Property)keyValue.Key;

                            if (property.Hint != null && property.Hint.Id == Guid.Empty)
                            {
                                Hint hint = property.Hint;

                                string newText = hint.Text;

                               IList<Hint> hintList = hintCollection.Hints.Where<Hint>(h => h.Text == newText).ToList<Hint>() ;

                                if (hintList.Count > 0)
                                {
                                    Hint oldHint = (Hint)hintList.First<Hint>() ;

                                    property.Hint = oldHint;
                                }
                                else
                                {
                                    hint.HintCollection = hintCollection;

                                    ModelService.SaveDomainObject(hint);
                                    hintCollection.Hints.Add(hint);
                                }
                                keyValue.Value.AddChange(DataModelChangeType.Modified, "");
                            }
                        }
                    }
                    else if (keyValue.Value.ContainDataModelChangeType(DataModelChangeType.ModifiedHint) && keyValue.Value.Apply)
                    {
                        if (keyValue.Key is Property)
                        {
                            Property property = (Property)keyValue.Key;

                            if (property.Hint != null)
                            {
                                Hint newHint = property.Hint;

                                ModelService.MergeSaveDomainObject(newHint);
                                                                
                            }
                        }
                    }
                    else if (keyValue.Value.ContainDataModelChangeType(DataModelChangeType.DeletedHint) && keyValue.Value.Apply)
                    {
                        if (keyValue.Key is Property)
                        {
                            Property property = (Property)keyValue.Key;

                            Property localProperty = ModelService.GetDomainObject<Property>(property.Id);
                            Hint hintToDelete = property.Hint;

                            property.Hint = null;
                            localProperty.Hint = null;

                            ModelService.MergeSaveDomainObject(localProperty);

                            Dictionary<string, object> dic = new Dictionary<string,object>();
                            dic.Add("Hint.Id", hintToDelete.Id);

                            IList<Property> propertyList = ModelService.GetAllDomainObjectsByPropertyValues<Property>(dic);

                            if (propertyList.Count == 0)
                            {
                                ModelService.DeleteDomainObject(hintToDelete);
                            }

                        }
                    }
                }
            }
        }
    }
}
