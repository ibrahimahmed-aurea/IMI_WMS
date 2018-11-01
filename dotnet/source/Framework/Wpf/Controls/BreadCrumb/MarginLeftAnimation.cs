using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace Imi.Framework.Wpf.Controls
{
    public enum AnimationDirection  { Left, Right };

    public class MarginLeftAnimation : AnimationTimeline
    {
        private bool isCompleted;

        /// <summary>
        /// Marks the animation as completed
        /// </summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        // TranslationDistance
        // Move left or move right
        // Property to animate (i.e. what Margin value)

        /// <summary>
        /// Sets the distance the animation should move
        /// </summary>
        public double TranslationDistance
        {
            get { return (double)GetValue(TranslationDistanceProperty); }
            set { SetValue(TranslationDistanceProperty, value); }
        }


        /// <summary>
        /// Dependency property. Sets the reverse value for the second animation
        /// </summary>
        public static readonly DependencyProperty TranslationDistanceProperty =
            DependencyProperty.Register("TranslationDistance", typeof(double), typeof(MarginLeftAnimation), new UIPropertyMetadata(0.0));


        /// <summary>
        /// Sets the direction in which the animation should go.
        /// </summary>
        public AnimationDirection Direction
        {
            get { return (AnimationDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(AnimationDirection), typeof(MarginLeftAnimation), new UIPropertyMetadata(AnimationDirection.Right));


        private double To
        {
            get
            {
                if (Direction == AnimationDirection.Left)
                {
                    return -TranslationDistance;
                }
                else
                {
                    return 0;
                }
            }
        }

        private double From
        {
            get
            {
                if (Direction == AnimationDirection.Left)
                {
                    return 0;
                }
                else
                {
                    return -TranslationDistance;
                }
            }
        }

        /// <summary>
        /// Returns the type of object to animate
        /// </summary>
        public override Type TargetPropertyType
        {
            get
            {
                return typeof(Thickness);
            }
        }

        /// <summary>
        /// Creates an instance of the animation object
        /// </summary>
        /// <returns>Returns the instance of the MarginLeftAnimation</returns>
        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new MarginLeftAnimation();
        }

        AnimationClock clock;

        /// <summary>
        /// registers to the completed event of the animation clock
        /// </summary>
        /// <param name="clock">the animation clock to notify completion status</param>
        void VerifyAnimationCompletedStatus(AnimationClock clock)
        {
            if (this.clock == null)
            {
                this.clock = clock;
                this.clock.Completed += new EventHandler(delegate(object sender, EventArgs e) { isCompleted = true; });
            }
        }

        /// <summary>
        /// Animates the ....
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate</param>
        /// <param name="defaultDestinationValue">The final value</param>
        /// <param name="animationClock">The animation clock (timer)</param>
        /// <returns>Returns the new grid length to set</returns>
        public override object GetCurrentValue(object defaultOriginValue,
            object defaultDestinationValue, AnimationClock animationClock)
        {
            //check the animation clock event
            VerifyAnimationCompletedStatus(animationClock);

            //check if the animation was completed
            if (isCompleted)
                return (Thickness)defaultDestinationValue;

            //check if the value is already collapsed
            Thickness thick = (Thickness)defaultOriginValue;

            //check to see if this is the last tick of the animation clock.
            if (animationClock.CurrentProgress.Value == 1.0) 
            {
                thick.Left = To;
                return thick;
            }

            double newValue = thick.Left;
            double fromVal = this.From;
            double toVal = this.To;

            if (fromVal > toVal) 
            {
                newValue = ((1 - animationClock.CurrentProgress.Value) *
                    (fromVal - toVal) + toVal);
            }
            else
            {
                newValue = (animationClock.CurrentProgress.Value *
                    (toVal - fromVal) + fromVal);
            }

            thick.Left = newValue;

            return thick;
        }
    }

}
