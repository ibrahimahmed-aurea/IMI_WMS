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
using QuickGraph;
using Cdc.MetaManager.DataAccess;
using GraphSharp.Controls;

namespace Cdc.MetaManager.GUI
{
    public class DependencyGraphLayout : GraphLayout<IDomainObject, IEdge<IDomainObject>, BidirectionalGraph<IDomainObject, IEdge<IDomainObject>>> { }

    public class VertexClickedEventArgs : EventArgs
    {
        public object Vertex { get; set; }
        public MouseButton Button { get; set; }
        public MouseDevice MouseDevice { get; set; }
    }
   
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DependencyGraph : UserControl
    {
        public event EventHandler<VertexClickedEventArgs> VertexClicked;
                
        public IBidirectionalGraph<IDomainObject, IEdge<IDomainObject>> Graph
        {
            get { return (IBidirectionalGraph<IDomainObject, IEdge<IDomainObject>>)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Graph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(IBidirectionalGraph<IDomainObject, IEdge<IDomainObject>>), typeof(DependencyGraph), new UIPropertyMetadata(null));
                
        public DependencyGraph()
        {
            InitializeComponent();

            graphLayout.PreviewMouseUp += (s, e) =>
            {
                VertexControl vc = e.Source as VertexControl;
                
                if (vc != null)
                {
                    HighlightVertex(vc.Vertex as IDomainObject);
                    
                    if (VertexClicked != null)
                    {
                        VertexClicked(vc, new VertexClickedEventArgs() { Vertex = vc.Vertex, Button = e.ChangedButton, MouseDevice = e.MouseDevice });
                    }
                }
            };
        }

        public void HighlightVertex(IDomainObject vertex)
        {
            foreach (IDomainObject v in graphLayout.HighlightedVertices)
            {
                graphLayout.RemoveHighlightFromVertex(v);
            }

            graphLayout.HighlightVertex(vertex, null);
        }
    }
}
