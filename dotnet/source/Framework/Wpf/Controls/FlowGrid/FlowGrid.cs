using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Imi.Framework.Wpf.Controls
{
    public class FlowGrid : Panel
    {
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
                
        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(FlowGrid), new UIPropertyMetadata(2, new PropertyChangedCallback(ColumnsPropertyChangedCallback)));

        private double[] columnDefinitions;
        private double[] rowDefinitions;
        
        public FlowGrid()
        {
            columnDefinitions = new double[Columns];
        }

        private static void ColumnsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlowGrid grid = (FlowGrid)d;
            grid.columnDefinitions = new double[grid.Columns];
            grid.InvalidateMeasure();
            grid.InvalidateArrange();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size childSize = availableSize;

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(childSize);
            }

            int col = 0;
            int row = 0;
            int rows = (int)Math.Ceiling(InternalChildren.Count / (double)Columns);

            rowDefinitions = new double[Columns];
           
            foreach (UIElement child in InternalChildren)
            {
                columnDefinitions[col] = Math.Max(columnDefinitions[col], child.DesiredSize.Width);

                rowDefinitions[col] += child.DesiredSize.Height;

                row++;

                if (row % rows == 0)
                {
                    row = 0;
                    col++;
                }
            }

            double width = 0;

            for (int i = 0; i < Columns; i++)
                width += columnDefinitions[i];

            double height = 0;

            for (int i = 0; i < Columns; i++)
                height = Math.Max(height, rowDefinitions[i]);

            return new Size(width, height);
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            rowDefinitions = new double[Columns];
            
            int col = 0;
            int row = 0;
            int rows = (int)Math.Ceiling(InternalChildren.Count / (double)Columns);

            foreach (UIElement child in InternalChildren)
            {
                double x = 0;
                
                for (int i = 0; i < col; i++)
                    x += columnDefinitions[i];
                                
                child.Arrange(new Rect(new Point(x, rowDefinitions[col]), child.DesiredSize));
                rowDefinitions[col] += child.DesiredSize.Height;

                row++;

                if (row % rows == 0)
                {
                    row = 0;
                    col++;
                }
            }

            return base.ArrangeOverride(finalSize);
        }        
    }
}
