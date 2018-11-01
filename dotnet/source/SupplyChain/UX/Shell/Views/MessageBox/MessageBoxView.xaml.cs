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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using ActiproSoftware.Windows.Controls.Ribbon;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public partial class MessageBoxView : RibbonWindow, IMessageBoxView
    {
        private Infrastructure.MessageBoxResult result;
        private FrameworkElement focusElement;
        
        public MessageBoxView()
        {
            InitializeComponent();

            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;

            cancelButton.Click += new RoutedEventHandler(CancelButtonClickEventHandler);
            yesButton.Click += new RoutedEventHandler(YesButtonClickEventHandler);
            noButton.Click += new RoutedEventHandler(NoButtonClickEventHandler);
            okButton.Click += new RoutedEventHandler(OkButtonClickEventHandler);
                        
            this.IsVisibleChanged += (s, e) =>
                {
                    if ((bool)e.NewValue)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            focusElement.Focus();
                        }));
                    }
                };
        }
                
        void OkButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            result = Infrastructure.MessageBoxResult.Ok;
            Close();
        }

        void NoButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            result = Infrastructure.MessageBoxResult.No;
            Close();
        }

        void YesButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            result = Infrastructure.MessageBoxResult.Yes;
            Close();
        }

        void CancelButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            result = Infrastructure.MessageBoxResult.Cancel;
            Close();
        }
                        
        [ServiceDependency]
        public WorkItem WorkItem
        {
            get;
            set;
        }
    
        #region IMessageBoxView Members

        public Infrastructure.MessageBoxResult Show(string caption, string message, string details, Infrastructure.MessageBoxButton button, Infrastructure.MessageBoxImage image)
        {
            this.Owner = Application.Current.MainWindow;
            
            yesButton.Visibility = Visibility.Collapsed;
            noButton.Visibility = Visibility.Collapsed;
            okButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;

            if (button == Infrastructure.MessageBoxButton.Ok)
            {
                okButton.Visibility = Visibility.Visible;
                focusElement = okButton;
                okButton.IsCancel = true;
                okButton.IsDefault = true;
            }
            else if (button == Infrastructure.MessageBoxButton.OkCancel)
            {
                okButton.Visibility = Visibility.Visible;
                okButton.IsDefault = true;
                cancelButton.Visibility = Visibility.Visible;
                cancelButton.IsCancel = true;
                focusElement = okButton;
            }
            else if (button == Infrastructure.MessageBoxButton.YesNo)
            {
                yesButton.Visibility = Visibility.Visible;
                yesButton.IsDefault = true;
                noButton.Visibility = Visibility.Visible;
                noButton.IsCancel = true;
                focusElement = yesButton;
            }
            else if (button == Infrastructure.MessageBoxButton.YesNoCancel)
            {
                yesButton.Visibility = Visibility.Visible;
                yesButton.IsDefault = true;
                noButton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
                cancelButton.IsCancel = true;
                focusElement = yesButton;
            }

            if (image == Infrastructure.MessageBoxImage.Error)
                iconImage.Source = new BitmapImage(new Uri("pack://application:,,,/Imi.SupplyChain.UX.Shell;component/Resources/Images/Error32.png"));
            else if (image == Infrastructure.MessageBoxImage.Warning)
                iconImage.Source = new BitmapImage(new Uri("pack://application:,,,/Imi.SupplyChain.UX.Shell;component/Resources/Images/Warning32.png"));
            else if (image == Infrastructure.MessageBoxImage.Information)
                iconImage.Source = new BitmapImage(new Uri("pack://application:,,,/Imi.SupplyChain.UX.Shell;component/Resources/Images/Info32.png"));
            else if (image == Infrastructure.MessageBoxImage.Question)
                iconImage.Source = new BitmapImage(new Uri("pack://application:,,,/Imi.SupplyChain.UX.Shell;component/Resources/Images/Help32.png"));

            this.Title = caption;
            captionLabel.Text = caption;
            messageTextBlock.Text = message;

            if (string.IsNullOrEmpty(details))
                detailsExpander.Visibility = Visibility.Collapsed;
            else
                detailsExpander.Visibility = Visibility.Visible;

            detailsTextBlock.Text = details;

            base.ShowDialog();

            WorkItem.SmartParts.Remove(this);

            return result;
        }

        #endregion

        private void CopyInfoButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(captionLabel.Text);
            sb.AppendLine();
            sb.AppendLine(messageTextBlock.Text);
            sb.AppendLine();
            sb.AppendLine(detailsTextBlock.Text);

            Clipboard.SetText(sb.ToString(), TextDataFormat.UnicodeText);
        }
    }
}
