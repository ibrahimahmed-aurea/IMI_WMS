// Generated from template: .\UX\ResourceManagerTemplate.cst


namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    using System;

    // Containerclasses user in views with chart control
    public class durationItem
    {
        public durationItem(string text, int duration)
        {
            Text = text;
            Duration = duration;
        }

        public string Text;
        public int Duration;

        public override string ToString()
        {
            return Text;
        }
    }

    public class typeItem
    {
        public typeItem(string text, string valueBinding, Type valueType)
        {
            Text = text;
            ValueBinding = valueBinding;
            ValueType = valueType;
        }

        public string Text;
        public string ValueBinding;
        public Type ValueType;

        public override string ToString()
        {
            return Text;
        }
    }

    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    public class ResourceManager : ResourceManagerBase
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceManager()
        {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager Instance
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.Resources", typeof(ResourceManager).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }


        public static string str_notOnTime_Caption
        {
            get
            {
                return Instance.GetString("str_notOnTime_Caption", resourceCulture);
            }
        }

        public static string str_onTime_Caption
        {
            get
            {
                return Instance.GetString("str_onTime_Caption", resourceCulture);
            }
        }


        public static string str_capacity_Caption
        {
            get
            {
                return Instance.GetString("str_capacity_Caption", resourceCulture);
            }
        }


        public static string str_lagging_Caption
        {
            get
            {
                return Instance.GetString("str_lagging_Caption", resourceCulture);
            }
        }

        public static string str_lastUpdated_Caption
        {
            get
            {
                return Instance.GetString("str_lastUpdated_Caption", resourceCulture);
            }
        }

        public static string str_timeResolution_Caption
        {
            get
            {
                return Instance.GetString("str_timeResolution_Caption", resourceCulture);
            }
        }

        public static string str_workload_Caption
        {
            get
            {
                return Instance.GetString("str_workload_Caption", resourceCulture);
            }
        }


        public static string str_dur60_Caption
        {
            get
            {
                return Instance.GetString("str_dur60_Caption", resourceCulture);
            }
        }

        public static string str_dur30_Caption
        {
            get
            {
                return Instance.GetString("str_dur30_Caption", resourceCulture);
            }
        }

        public static string str_dur15_Caption
        {
            get
            {
                return Instance.GetString("str_dur15_Caption", resourceCulture);
            }
        }

        public static string str_dur10_Caption
        {
            get
            {
                return Instance.GetString("str_dur10_Caption", resourceCulture);
            }
        }

        public static string str_dur5_Caption
        {
            get
            {
                return Instance.GetString("str_dur5_Caption", resourceCulture);
            }
        }

        public static string str_orders_Caption
        {
            get
            {
                return Instance.GetString("str_orders_Caption", resourceCulture);
            }
        }

        public static string str_cars_Caption
        {
            get
            {
                return Instance.GetString("str_cars_Caption", resourceCulture);
            }
        }

        public static string str_paks_Caption
        {
            get
            {
                return Instance.GetString("str_paks_Caption", resourceCulture);
            }
        }

        public static string str_rows_Caption
        {
            get
            {
                return Instance.GetString("str_rows_Caption", resourceCulture);
            }
        }

        public static string str_weight_Caption
        {
            get
            {
                return Instance.GetString("str_weight_Caption", resourceCulture);
            }
        }

        public static string str_volume_Caption
        {
            get
            {
                return Instance.GetString("str_volume_Caption", resourceCulture);
            }
        }

        public static string str_kits_Caption
        {
            get
            {
                return Instance.GetString("str_kits_Caption", resourceCulture);
            }
        }

        public static string str_comp_Caption
        {
            get
            {
                return Instance.GetString("str_comp_Caption", resourceCulture);
            }
        }

        public static string str_staged_Caption
        {
            get
            {
                return Instance.GetString("str_staged_Caption", resourceCulture);
            }
        }

        public static string str_inProgress_Caption
        {
            get
            {
                return Instance.GetString("str_inProgress_Caption", resourceCulture);
            }
        }

        public static string str_planned_Caption
        {
            get
            {
                return Instance.GetString("str_planned_Caption", resourceCulture);
            }
        }

        public static string str_manHours_Caption
        {
            get
            {
                return Instance.GetString("str_manHours_Caption", resourceCulture);
            }
        }

        public static string str_pickedQuantity_Caption
        {
            get
            {
                return Instance.GetString("str_pickedQuantity_Caption", resourceCulture);
            }
        }

        public static string str_currentPace_Caption
        {
            get
            {
                return Instance.GetString("str_currentPace_Caption", resourceCulture);
            }
        }

        public static string str_averagePace_Caption
        {
            get
            {
                return Instance.GetString("str_averagePace_Caption", resourceCulture);
            }
        }

        public static string str_remaining_Caption
        {
            get
            {
                return Instance.GetString("str_remaining_Caption", resourceCulture);
            }
        }

        public static string str_ESTFinish_Caption
        {
            get
            {
                return Instance.GetString("str_ESTFinish_Caption", resourceCulture);
            }
        }

        public static string str_measurePerHour_Caption
        {
            get
            {
                return Instance.GetString("str_measurePerHour_Caption", resourceCulture);
            }
        }

        public static string str_meanPace_Caption
        {
            get
            {
                return Instance.GetString("str_meanPace_Caption", resourceCulture);
            }
        }


        public static string str_started_Caption
        {
            get
            {
                return Instance.GetString("str_started_Caption", resourceCulture);
            }
        }

        public static string str_finished_Caption
        {
            get
            {
                return Instance.GetString("str_finished_Caption", resourceCulture);
            }
        }

        public static string str_end_Caption
        {
            get
            {
                return Instance.GetString("str_end_Caption", resourceCulture);
            }
        }

        public static string str_start_Caption
        {
            get
            {
                return Instance.GetString("str_start_Caption", resourceCulture);
            }
        }

        public static string str_status_Caption
        {
            get
            {
                return Instance.GetString("str_status_Caption", resourceCulture);
            }
        }

        public static string str_value_Caption
        {
            get
            {
                return Instance.GetString("str_value_Caption", resourceCulture);
            }
        }

        public static string str_carSize_Caption
        {
            get
            {
                return Instance.GetString("str_carSize_Caption", resourceCulture);
            }
        }

        public static string str_loadMeter_Caption
        {
            get
            {
                return Instance.GetString("str_loadMeter_Caption", resourceCulture);
            }
        }
        //----
        public static string str_notOnShipLocOther_Caption
        {
            get
            {
                return Instance.GetString("str_notOnShipLocOther_Caption", resourceCulture);
            }
        }

        public static string str_notOnShipLocPall_Caption
        {
            get
            {
                return Instance.GetString("str_notOnShipLocPall_Caption", resourceCulture);
            }
        }

        public static string str_notOnShipLocPick_Caption
        {
            get
            {
                return Instance.GetString("str_notOnShipLocPick_Caption", resourceCulture);
            }
        }

        public static string str_notOnShipLocTrans_Caption
        {
            get
            {
                return Instance.GetString("str_notOnShipLocTrans_Caption", resourceCulture);
            }
        }

        public static string str_onShipLoc_Caption
        {
            get
            {
                return Instance.GetString("str_onShipLoc_Caption", resourceCulture);
            }
        }

        public static string str_methodOfShipment_Caption
        {
            get
            {
                return Instance.GetString("str_methodOfShipment_Caption", resourceCulture);
            }
        }

        public static string str_notComposedOrders_Caption
        {
            get
            {
                return Instance.GetString("str_notComposedOrders_Caption", resourceCulture);
            }
        }

        public static string str_notReceivedTransits_Caption
        {
            get
            {
                return Instance.GetString("str_notReceivedTransits_Caption", resourceCulture);
            }
        }

        public static string str_route_Caption
        {
            get
            {
                return Instance.GetString("str_route_Caption", resourceCulture);
            }
        }

        public static string str_shippingLocations_Caption
        {
            get
            {
                return Instance.GetString("str_shippingLocations_Caption", resourceCulture);
            }
        }

        public static string str_transportConditions_Caption
        {
            get
            {
                return Instance.GetString("str_transportConditions_Caption", resourceCulture);
            }
        }

        public static string str_departure_Caption
        {
            get
            {
                return Instance.GetString("str_departure_Caption", resourceCulture);
            }
        }

        public static string str_departureTime_Caption
        {
            get
            {
                return Instance.GetString("str_departureTime_Caption", resourceCulture);
            }
        }
    }
}
