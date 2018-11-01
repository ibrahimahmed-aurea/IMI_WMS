using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Imi.Framework.UX.Wpf
{
    public class GridLenghtAnimationHelper
    {
        private RowDefinition row;

        private double desiredSpeed = (300.0 / 200.0); // pixels per millisecond
        private int longestTime = 2000;  // milliseconds
        private GridLength SaveHeight;

        public double DesiredSpeed
        {
            get { return desiredSpeed; }
            set { desiredSpeed = value; }
        }

        public int LongestTime
        {
            get { return longestTime; }
            set { longestTime = value; }
        }

        public GridLenghtAnimationHelper(RowDefinition row)
        {
            this.row = row;
        }

        private Duration GetDuration(double distance)
        {
            int timeMilliseconds = Convert.ToInt32(Math.Round(distance / desiredSpeed));

            if (timeMilliseconds > longestTime)
                timeMilliseconds = longestTime;

            return new Duration(new TimeSpan(0, 0, 0, 0, timeMilliseconds));
        }

        public void ToggleHeightAnimation()
        {
            GridLengthAnimation a = new GridLengthAnimation();

            if (row.ActualHeight == 0)
            {
                // Going up
                a.From = new GridLength(0, GridUnitType.Pixel);
                a.To = SaveHeight;
                a.Duration = GetDuration(SaveHeight.Value);
            }
            else
            {
                // Going down
                a.From = new GridLength(row.ActualHeight, GridUnitType.Pixel);
                SaveHeight = a.From;
                a.To = new GridLength(0, GridUnitType.Pixel);
                a.Duration = GetDuration(SaveHeight.Value);
            }

            row.BeginAnimation(RowDefinition.HeightProperty, a);
        }
    }
}
