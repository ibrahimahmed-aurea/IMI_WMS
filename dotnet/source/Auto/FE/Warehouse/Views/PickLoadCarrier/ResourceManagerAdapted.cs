// Generated from template: .\UX\ResourceManagerTemplate.cst


namespace Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier {
    using System;
        	
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    public class ResourceManager : ResourceManagerBase {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        		
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceManager() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager Instance {
            get {
                if (object.ReferenceEquals(resourceMan, null)) 
				{
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.Resources", typeof(ResourceManager).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string str_FromLoadCarrier_Caption
        {
            get
            {
                return Instance.GetString("str_FromLoadCarrier_Caption", resourceCulture);
            }
        }
        public static string str_ToLoadCarrier_Caption
        {
            get
            {
                return Instance.GetString("str_ToLoadCarrier_Caption", resourceCulture);
            }
        }
        public static string str_NewLoadCarrier_Caption
        {
            get
            {
                return Instance.GetString("str_NewLoadCarrier_Caption", resourceCulture);
            }
        }
        public static string str_ResetPackStatio_Title
        {
            get
            {
                return Instance.GetString("str_ResetPackStatio_Title", resourceCulture);
            }
        }
        public static string str_EAN_Caption
        {
            get
            {
                return Instance.GetString("str_EAN_Caption", resourceCulture);
            }
        }
        public static string str_EANPack_Error_Title
        {
            get
            {
                return Instance.GetString("str_EANPack_Error_Title", resourceCulture);
            }
        }
        public static string hint_ScanableField
        {
            get
            {
                return Instance.GetString("hint_ScanableField", resourceCulture);
            }
        }
    }
}
