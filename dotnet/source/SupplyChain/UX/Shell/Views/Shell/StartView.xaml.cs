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
using System.Windows.Media.Animation;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : UserControl
    {
        Storyboard fadeStoryBoard = null;
                
        public StartView()
        {
            InitializeComponent();
            this.IsVisibleChanged += IsVisibleChangedEventHandler;
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(DataContextChangedEventHandler);
        }
                
        private void DataContextChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => 
            {
                Animate();
            }));
        }

        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                Animate();
            }
        }
                
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            fadeStoryBoard = this.FindResource("fadeText") as Storyboard;
        }
                
        private void Animate()
        {
            if (fadeStoryBoard != null)
            {
                fadeStoryBoard.Seek(TimeSpan.Zero);
                fadeStoryBoard.Begin();
            }
        }
    }
}
