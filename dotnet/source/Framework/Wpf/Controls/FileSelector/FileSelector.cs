using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Imi.Framework.Wpf.Controls
{
    public class FileSelector : TextBox
    {
        public event EventHandler FileSelected;

        public byte[] FileData
        {
            get { return (byte[])GetValue(FileDataProperty); }
            set { SetValue(FileDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileDataProperty =
            DependencyProperty.Register("FileData", typeof(byte[]), typeof(FileSelector), new UIPropertyMetadata(null));

        public DateTime? FileLastModified
        {
            get { return (DateTime?)GetValue(FileLastModifiedProperty); }
            set { SetValue(FileLastModifiedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileLastModified.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileLastModifiedProperty =
            DependencyProperty.Register("FileLastModified", typeof(DateTime?), typeof(FileSelector), new UIPropertyMetadata(null));


        public long? FileSize
        {
            get { return (long?)GetValue(FileSizeProperty); }
            set { SetValue(FileSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileSizeProperty =
            DependencyProperty.Register("FileSize", typeof(long?), typeof(FileSelector), new UIPropertyMetadata(null));

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(FileSelector), new UIPropertyMetadata(null));

        public bool OnlyReturnPath
        {
            get { return (bool)GetValue(OnlyReturnPathProperty); }
            set { SetValue(OnlyReturnPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnlyReturnPathProperty =
            DependencyProperty.Register("OnlyReturnPath", typeof(bool), typeof(FileSelector), new UIPropertyMetadata(null));


        public string FileExtensions { get; set; }

        static FileSelector()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileSelector), new FrameworkPropertyMetadata(typeof(FileSelector)));
        }

        public FileSelector()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button clearButton = Template.FindName("PART_ClearButton", this) as Button;

            if (clearButton != null)
            {
                clearButton.Click += new RoutedEventHandler(clearButton_Click);
            }

            Button browseButton = Template.FindName("PART_BrowseButton", this) as Button;

            if (browseButton != null)
            {
                browseButton.Click += new RoutedEventHandler(browseButton_Click);
            }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
             Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

             openFileDialog.DefaultExt = FileExtensions;

             if (openFileDialog.ShowDialog() == true)
             {
                 if (!string.IsNullOrEmpty(openFileDialog.FileName))
                 {
                     FileInfo fi = new FileInfo(openFileDialog.FileName);
                     if (fi.Exists)
                     {
                         if (OnlyReturnPath)
                         {
                             FilePath = openFileDialog.FileName;
                             this.Text = openFileDialog.FileName;
                         }
                         else
                         {
                             FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                             byte[] fileData = new byte[fs.Length];
                             fs.Read(fileData, 0, System.Convert.ToInt32(fs.Length));
                             fs.Close();

                             FileData = fileData;
                             FileLastModified = fi.LastWriteTime;
                             FileSize = fi.Length;

                             this.Text = openFileDialog.SafeFileName;
                         }

                         if (FileSelected != null)
                         {
                             FileSelected(this, null);
                         }
                     }
                 }
             }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            FileData = null;
            FileLastModified = null;
            FileSize = null;
            this.Text = string.Empty;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            BindingExpression expression = BindingOperations.GetBindingExpression(this, TextBox.TextProperty);

            if (expression != null)
            {
                expression.UpdateSource();
            }
        }
    }
}
