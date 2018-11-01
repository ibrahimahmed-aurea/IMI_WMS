using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using ActiproSoftware.Windows.Controls.Ribbon;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Export;
using Imi.Framework.Wpf.Controls;

namespace Imi.SupplyChain.UX.Views
{
    [SmartPart]
    public partial class EntityView : RibbonWindow, IEntityView, IBuilderAware
    {
        private EntityPresenter presenter;

        private IDictionary<string, List<string>> _propertiesUsedByImport;

        [CreateNew]
        public EntityPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

        [ServiceDependency]
        public WorkItem WorkItem
        {
            get;
            set;
        }

        public EntityView()
        {
            InitializeComponent();
            RefreshDataOnShow = true;
            IsDetailView = true;
            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;
            Owner = System.Windows.Application.Current.MainWindow;
            
            EntityGrid.ClipboardExporters.Clear();
            
            UnicodeCsvClipboardExporter exporter = new UnicodeCsvClipboardExporter();
            exporter.IncludeColumnHeaders = false;
            exporter.FormatSettings.TextQualifier = '\0';
            EntityGrid.ClipboardExporters.Add("UnicodeText", exporter);
        }

        private void Grid_DataExport(object sender, DataExportEventArgs e)
        {
            if (EntityGrid.DataContext != null)
            {
                List<object> data = (List<object>)typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(typeof(object)).Invoke(null, new object[] { EntityGrid.DataContext });

                EntityGrid.AppendDataForExport(data, _propertiesUsedByImport);
                EntityGrid.DoExport();
            }
        }

        #region IDataView Members

        
        public void PresentData(object data)
        {
            PresentData(data, null);
        }


        public void PresentData(object data, IDictionary<string, List<string>> propertiesUsedByImport = null)
        {
            _propertiesUsedByImport = propertiesUsedByImport;

            bool isList = false;

            if (data == null) { return; }

            Type dataType;

            if (data.GetType().Name == typeof(List<object>).Name)
            {
                if (((System.Collections.ICollection) data).Count == 0) { return; }

                dataType = data.GetType().GetGenericArguments()[0];

                isList = true;
            }
            else
            {
                dataType = data.GetType();
            }

            foreach (PropertyInfo pi in dataType.GetProperties())
            {
                if (pi.GetGetMethod().GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Count() == 0)
                {
                    string headerText;
                    string bindingName;

                    headerText = pi.Name;
                    bindingName = pi.Name;

                    if (pi.GetCustomAttributes(typeof(DisplayNameAttribute), false).Count() > 0)
                    {
                        headerText += "\n\r[" + ((DisplayNameAttribute)pi.GetCustomAttributes(typeof(DisplayNameAttribute), false)[0]).DisplayName + "]";
                    }

                    Column newColumn = new Column();
                    newColumn.Title = headerText;
                    newColumn.FieldName = bindingName;

                    EntityGrid.Columns.Add(newColumn);
                }
            }

            if (!isList)
            {
                data = new List<object>() { data };
            }
            
            EntityGrid.DataContext = data;
            
            Dispatcher.BeginInvoke(new Action(() => 
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(data);

                if (collectionView != null && collectionView.CurrentItem != null)
                {
                    object item = Presenter.WorkItem.Items.FindByType(collectionView.CurrentItem.GetType()).LastOrDefault();
                    int index = EntityGrid.Items.IndexOf(item);
                    
                    if (index > -1)
                    {
                        EntityGrid.SelectedCellRanges.Add(new SelectionCellRange(index, 0));
                        EntityGrid.CurrentItem = item;
                    }
                }

                EntityGrid.Focus();
            }));

            base.ShowDialog();

            WorkItem.SmartParts.Remove(this);
        }
                
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }

            base.OnPreviewKeyDown(e);
        }

        public void Update(object parameters)
        {

        }

        public new bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                if (value)
                    Visibility = Visibility.Visible;
                else
                    Visibility = Visibility.Collapsed;
            }
        }

        public bool IsDetailView { get; set; }

        public bool RefreshDataOnShow
        {
            get;
            set;
        }

        public void SetFocus()
        {
            Focus();
        }

        public void UpdateProgress(int progressPercentage)
        {

        }

        #endregion

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
        }

        public void OnTearingDown()
        {
            Presenter.WorkItem.Items.Remove(Presenter);
        }

        #endregion
    }
}
