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

namespace Imi.Framework.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for LayoutComboBox.xaml
    /// </summary>
    public partial class LayoutSelector : UserControl
    {
        public LayoutSelector()
        {
            InitializeComponent();
            comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);
        }

        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedColumns = comboBox.SelectedIndex + 1;
        }
                
        public int SelectedColumns
        {
            get { return (int)GetValue(SelectedColumnsProperty); }
            set 
            {
                SetValue(SelectedColumnsProperty, value); 
            }
        }
                
        // Using a DependencyProperty as the backing store for SelectedColumns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColumnsProperty =
            DependencyProperty.Register("SelectedColumns", typeof(int), typeof(LayoutSelector), new UIPropertyMetadata(0, SelectedColumnsChangedCallback, CoerceSelectedColumnsCallback));
        
        private static object CoerceSelectedColumnsCallback(DependencyObject d, object value)
        {
            return value;
        }

        private static void SelectedColumnsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LayoutSelector)d).comboBox.SelectedIndex = (int)e.NewValue -1;
        }

    }
}
