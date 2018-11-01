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
using System.Xml;
using System.Xml.XPath;
using ActiproSoftware.Windows.Controls.Ribbon;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Views
{
    public partial class MessageHandlerBoxView : RibbonWindow, IMessageHandlerBoxView
    {
        MessageHandlerBoxResult result;

        private IList<MessageHandlerDataItem> messageList { get; set; }

        [ServiceDependency]
        public IShellInteractionService ShellInteractionService { get; set; }

        public MessageHandlerBoxView()
        {
            InitializeComponent();
            
            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;

            cancelButton.Click += new RoutedEventHandler(CancelButtonClickEventHandler);
            yesButton.Click += new RoutedEventHandler(YesButtonClickEventHandler);
            noButton.Click += new RoutedEventHandler(NoButtonClickEventHandler);
            okButton.Click += new RoutedEventHandler(OkButtonClickEventHandler);

            messageList = new List<MessageHandlerDataItem>();
        }

        void OkButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            result = MessageHandlerBoxResult.Ok;
            Close();
        }

        void NoButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            result = MessageHandlerBoxResult.No;
            Close();
        }

        void YesButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            result = MessageHandlerBoxResult.Yes;
            Close();
        }

        void CancelButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            result = MessageHandlerBoxResult.Cancel;
            Close();
        }
                        
        [ServiceDependency]
        public WorkItem WorkItem
        {
            get;
            set;
        }
    
        #region IMessageBoxView Members

        public MessageHandlerBoxResult Show(string messageXml)
        {
            this.Owner = Application.Current.MainWindow;
            
            yesButton.Visibility = Visibility.Collapsed;
            noButton.Visibility = Visibility.Collapsed;
            okButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;

            // Parse the messages
            ParseMessageXML(messageXml);

            // Check what severe states we have
            if (messageList.Where(i => i.Type == MessageHandlerItemType.Error).Count() > 0)
            {
                // Got errors
                // Set visibility on buttons
                okButton.Visibility = Visibility.Visible;
                okButton.IsCancel = true;
                okButton.IsDefault = true;

                messageTextBlock.Visibility = Visibility.Collapsed;

                // Check if also have warnings
                if (messageList.Where(i => i.Type == MessageHandlerItemType.Warning).Count() > 0)
                {
                    this.Title = StringResources.MessageHandlerBox_ErrorWarning_Headline;
                    captionLabel.Text = StringResources.MessageHandlerBox_ErrorWarning_Headline;
                }
                else
                {
                    this.Title = StringResources.MessageHandlerBox_Error_Headline;
                    captionLabel.Text = StringResources.MessageHandlerBox_Error_Headline;
                }
            }
            else if (messageList.Where(i => i.Type == MessageHandlerItemType.Warning).Count() > 0)
            {
                // Got warnings
                // Set visibility on buttons
                yesButton.Visibility = Visibility.Visible;
                yesButton.IsDefault = true;
                noButton.Visibility = Visibility.Visible;
                noButton.IsCancel = true;

                this.Title = StringResources.MessageHandlerBox_Warning_Headline;
                captionLabel.Text = StringResources.MessageHandlerBox_Warning_Headline;
                messageTextBlock.Text = StringResources.MessageHandlerBox_Warning_Question;
            }
            else
            {
                // Got information, show as notification
                
                foreach (MessageHandlerDataItem item in messageList)
                {
                    ShellNotification notification = new ShellNotification(item.Text, null);
                    ShellInteractionService.ShowNotification(notification);
                }
                
                WorkItem.SmartParts.Remove(this);

                return MessageHandlerBoxResult.Ok;
            }

            // Set the items source for the listbox so it gets populated.
            this.messageListBox.ItemsSource = messageList;

            // Show dialog
            base.ShowDialog();

            // Remove from smart parts. Added from where it's called!
            WorkItem.SmartParts.Remove(this);

            ShellInteractionService.ShowProgress();
            return result;
        }

        // Only possible to create this window using the XML from the backends
        // MessageFault that initially comes from the database call to 
        // UIMessageHandler.GetErrorWarningXML or UIMessageHandler.GetInformationXML
        // The XML should be on the form:
        //  <?xml version="1.0"?>
        //  <MessageList>
        //      <Messages>
        //          <Message Type="E" Seq="1" Count="1" AlarmId="ALM002" AlarmText="Alarmtext for ALM002"/>
        //          <Message Type="W" Seq="2" Count="1" AlarmId="ALM001" AlarmText="Alarmtext for ALM001"/>
        //          ...
        //          or
        //          <Message Type="I" Seq="1" Count="1" AlarmId="ALM001" AlarmText="Alarmtext for ALM001"/>
        //          ...
        //      </Messages>
        //      <Ignores>
        //          <Ignore AlarmId="ALM002"/>
        //          ...
        //      </Ignores>
        //  </MessageList>
        private void ParseMessageXML(string messageXml)
        {
            XmlTextReader xmlReader = new XmlTextReader(messageXml, XmlNodeType.Document, null);
            XPathDocument xmlDoc = new XPathDocument(xmlReader);
            XPathNavigator xmlNav = xmlDoc.CreateNavigator();
            XPathNodeIterator nodeIterator = null;

            // Find all messages within the MessageList/Messages
            nodeIterator = xmlNav.Select("/MessageList/Messages/Message");

            string messageText = string.Empty;
            messageList.Clear();

            while (nodeIterator.MoveNext())
            {
                MessageHandlerDataItem item = new MessageHandlerDataItem();

                // Get the type of the message
                if (nodeIterator.Current.GetAttribute("Type", string.Empty) == "E")
                {
                    item.TypeBrush = GetBrushFromMessageType(MessageHandlerItemType.Error);
                    item.Type = MessageHandlerItemType.Error;
                }
                else if (nodeIterator.Current.GetAttribute("Type", string.Empty) == "W")
                {
                    item.TypeBrush = GetBrushFromMessageType(MessageHandlerItemType.Warning);
                    item.Type = MessageHandlerItemType.Warning;
                }
                else if (nodeIterator.Current.GetAttribute("Type", string.Empty) == "I")
                {
                    item.TypeBrush = GetBrushFromMessageType(MessageHandlerItemType.Information);
                    item.Type = MessageHandlerItemType.Information;
                }

                int Count = 0;

                try
                {
                    Count = int.Parse(nodeIterator.Current.GetAttribute("Count", string.Empty));
                }
                catch
                {
                    Count = 0;
                }

                if (Count <= 1)
                {
                    item.Text = string.Format("{0} [{1}]",
                                               nodeIterator.Current.GetAttribute("AlarmText", string.Empty),
                                               nodeIterator.Current.GetAttribute("AlarmId", string.Empty));
                }
                else
                {
                    item.Text = string.Format("{0} [{1}] ({2})",
                                               nodeIterator.Current.GetAttribute("AlarmText", string.Empty),
                                               nodeIterator.Current.GetAttribute("AlarmId", string.Empty),
                                               nodeIterator.Current.GetAttribute("Count", string.Empty));
                }

                // Add item to list
                messageList.Add(item);
            }
        }

        private Brush GetBrushFromMessageType(MessageHandlerItemType messageType)
        {
            switch (messageType)
            {
                case MessageHandlerItemType.Warning: return new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Imi.SupplyChain.UX;component/Resources/Images/Warning32.png")));
                case MessageHandlerItemType.Error: return new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Imi.SupplyChain.UX;component/Resources/Images/Error32.png")));
                default:
                    return new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Imi.SupplyChain.UX;component/Resources/Images/Info32.png")));
            }
        }

        #endregion
    }
}
