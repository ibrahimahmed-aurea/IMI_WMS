using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataCollection;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using Imi.Framework.Wpf.Controls;

namespace Imi.Framework.UX.Wpf.Services
{
    public class TextControlScannerService : ScannerService
    {

        private struct ElementProperties
        {
            public string ApplicationIdentifierToGet;
            public Key CompleteScanKey;
        }

        private Dictionary<FrameworkElement, ElementProperties> Controls = new Dictionary<FrameworkElement, ElementProperties>();

        public TextControlScannerService()
            : base(new RegexBarcodeDecoder())
        {

        }

        public void AddControl(FrameworkElement frameworkElement, string applicationIdentifierToGet, Key completeScanKey)
        {

            if (frameworkElement is UIElement)
            {
                ElementProperties newProp = new ElementProperties();
                newProp.ApplicationIdentifierToGet = applicationIdentifierToGet;
                newProp.CompleteScanKey = completeScanKey;

                Controls.Add(frameworkElement, newProp);

                //((ComboDialog)frameworkElement).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(TextControlScannerService_PreviewTextInput);
                ((UIElement)frameworkElement).PreviewKeyDown += new KeyEventHandler(TextControlScannerService_PreviewKeyDown);
            }
        }



        public void RemoveControl(FrameworkElement frameworkElement)
        {
            if (Controls.ContainsKey(frameworkElement))
            {
                if (frameworkElement is UIElement)
                {
                    //((ComboDialog)frameworkElement).PreviewTextInput -= TextControlScannerService_PreviewTextInput;
                    ((UIElement)frameworkElement).PreviewKeyDown -= TextControlScannerService_PreviewKeyDown;
                }

                Controls.Remove(frameworkElement);
            }
        }

        void TextControlScannerService_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                if (Controls.ContainsKey(sender as FrameworkElement))
                {
                    ElementProperties senderProps = Controls[sender as FrameworkElement];

                    TextInputBarcodeScannerSection section = ConfigurationManager.GetSection(TextInputBarcodeScannerSection.SectionKey) as TextInputBarcodeScannerSection;

                    char eofChar = section.EOF_char;

                    if (((char)KeyInterop.VirtualKeyFromKey(e.Key)) == eofChar)
                    {
                        if (sender is ComboDialog)
                        {
                            ((ComboDialog)sender).Text = DecodeTextIfScanned(((ComboDialog)sender).Text, senderProps.ApplicationIdentifierToGet);
                        }

                        if (sender is TextBox)
                        {
                            ((TextBox)sender).Text = DecodeTextIfScanned(((TextBox)sender).Text, senderProps.ApplicationIdentifierToGet);
                        }

                        if (senderProps.CompleteScanKey != Key.None)
                        {
                            var routedEvent = Keyboard.PreviewKeyDownEvent; // Event to send 
                            ((FrameworkElement)sender).RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual((FrameworkElement)sender), 0, senderProps.CompleteScanKey) { RoutedEvent = routedEvent });
                        }

                    }
                }
            }
        }

        private string DecodeTextIfScanned(string text, string applicationIdentifierToGet)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(applicationIdentifierToGet))
            {

                IList<BarcodeSegment> foundSegments = OnScanCompleted(text);
                List<string> applicationIdentifiersToGet = applicationIdentifierToGet.Split(';').ToList();

                List<BarcodeSegment> GetSegments = foundSegments.Where(s => applicationIdentifiersToGet.Contains(s.ApplicationIdentifier)).ToList();
                if (GetSegments.Count > 0)
                {
                    return GetSegments[0].Text;
                }

            }

            return string.Empty;
        }
    }
}
