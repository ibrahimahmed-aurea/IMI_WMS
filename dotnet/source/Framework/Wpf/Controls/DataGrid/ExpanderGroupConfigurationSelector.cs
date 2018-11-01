using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.DataGrid;
using System.Windows.Data;
using System.ComponentModel;

namespace Imi.Framework.Wpf.Controls
{
    public class ExpanderGroupConfigurationSelector : GroupConfigurationSelector
    {
        private DataGridControl grid;

        public ExpanderGroupConfigurationSelector(DataGridControl grid)
        {
            this.grid = grid;
        }

        public override GroupConfiguration SelectGroupConfiguration(int groupLevel, CollectionViewGroup collectionViewGroup, GroupDescription groupDescription)
        {
            GroupConfiguration groupConfiguration = base.SelectGroupConfiguration(groupLevel, collectionViewGroup, groupDescription);

            if (groupConfiguration == null)
            {
                groupConfiguration = new GroupConfiguration();
                groupConfiguration.InitiallyExpanded = false;
            }

            if (collectionViewGroup != null)
            {
                groupConfiguration.InitiallyExpanded = (!collectionViewGroup.IsBottomLevel);
            }


            return groupConfiguration;
        }

    }
}
