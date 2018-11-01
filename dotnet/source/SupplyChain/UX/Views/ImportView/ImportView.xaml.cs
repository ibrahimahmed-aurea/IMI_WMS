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
using Imi.SupplyChain.UX.Infrastructure;


namespace Imi.SupplyChain.UX.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : UserControl, IImportView, IBuilderAware
    {
        

        private ImportPresenter presenter;
        private string title;

        [CreateNew]
        public ImportPresenter Presenter
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

        public ImportView()
        {
            InitializeComponent();

            this.contextMenu.Opened += (s, e) =>
            {
                contextMenu.Items.Clear();


                foreach (ShellAction action in Presenter.GetActions())
                {
                    MenuItem item = new MenuItem();
                    item.Header = action.Caption;
                    item.IsEnabled = action.IsEnabled && action.IsAuthorized;
                    item.Tag = action.Id;

                    item.Click += (sender, a) =>
                    {
                        System.Windows.Controls.TextBox textBox = FocusManager.GetFocusedElement(Application.Current.MainWindow) as System.Windows.Controls.TextBox;

                        if (textBox != null)
                        {
                            BindingExpression expression = textBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty);

                            if (expression != null)
                            {
                                expression.UpdateSource();
                            }
                        }

                        Presenter.ActionCatalog.Execute((string)((MenuItem)sender).Tag, Presenter.WorkItem, this, null);
                    };

                    contextMenu.Items.Add(item);

                }
            };
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        #region IDataView Members

        public void PresentData(object data)
        {
            if (DataType != null)
            {
                foreach (PropertyInfo pi in DataType.GetProperties())
                {
                    if (ImportParameters.Contains(pi.Name))
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

                            ImportGrid.Columns.Add(newColumn);

                            if (!Presenter.PropertyToExcelColumnIIndexDic.ContainsKey(bindingName))
                            {
                                Presenter.PropertyToExcelColumnIIndexDic.Add(bindingName, -1);
                            }
                        }
                    }
                }

                LoadDataToGrid();
            }

            if (ShowOpenFile)
            {
                OpenFileComponent.FileExtensions = FileExtension;
                OpenFileComponent.FileSelected += new EventHandler(OpenFileComponent_FileSelected);
            }
            else
            {
                OpenFileComponent.Visibility = System.Windows.Visibility.Collapsed;
            }

        }

        void OpenFileComponent_FileSelected(object sender, EventArgs e)
        {
            Presenter.ShellInteractionService.ShowProgress();
            Data = Presenter.ReadExcelData(OpenFileComponent.FilePath, DataType);
            LoadDataToGrid();
            Presenter.OnViewUpdated(Data);
            Presenter.ShellInteractionService.HideProgress();
        }

        public void Update(object parameters)
        {

        }

        public new bool IsVisible
        {
            get;
            set;
        }

        public bool IsDetailView
        {
            get;
            set;
        }

        public bool RefreshDataOnShow
        {
            get;
            set;
        }

        public void SetFocus()
        {

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

        #region IImportView Members

        public Type DataType { get; set; }
        public object Data { get; set; }
        public bool ShowOpenFile { get; set; }
        public string FileExtension { get; set; }
        public List<string> ImportParameters { get; set; }

        #endregion

        private void LoadDataToGrid()
        {
            bool isList = false;

            if (Data != null)
            {
                if (Data.GetType().Name == typeof(List<object>).Name)
                {
                    isList = true;
                }

                if (!isList)
                {
                    Data = new List<object>() { Data };
                }

                ImportGrid.DataContext = Data;
            }
        }

        
    }
}
