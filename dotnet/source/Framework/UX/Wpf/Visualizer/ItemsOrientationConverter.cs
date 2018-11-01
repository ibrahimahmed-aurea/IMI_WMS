using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace Imi.Framework.UX.Wpf.Visualizer
{
    [ValueConversion( typeof( ItemsPresenter ), typeof( Orientation ) )]
	public class ItemsOrientationConverter : IValueConverter
	{		
		public object Convert( 
			object		value, 
			Type		targetType, 
			object		parameter, 
			CultureInfo culture )
		{
			ItemsPresenter itemsPresenter = value as ItemsPresenter;
			if( itemsPresenter == null )
				return Binding.DoNothing;

			TreeViewItem item = itemsPresenter.TemplatedParent as TreeViewItem;			
			
            if( item == null )
				return Binding.DoNothing;

			
			return Orientation.Horizontal;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}