//using ArcGIS.Core.CIM;
//using ArcGIS.Core.Data;
//using ArcGIS.Core.Geometry;
//using ArcGIS.Desktop.Catalog;
//using ArcGIS.Desktop.Core;
//using ArcGIS.Desktop.Editing;
//using ArcGIS.Desktop.Extensions;
//using ArcGIS.Desktop.Framework;
//using ArcGIS.Desktop.Framework.Contracts;
//using ArcGIS.Desktop.Framework.Dialogs;
//using ArcGIS.Desktop.Framework.Threading.Tasks;
//using ArcGIS.Desktop.Layouts;
//using ArcGIS.Desktop.Mapping;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;

//namespace StreetViewPictureProcessorAddin
//{
//    internal class Module1 : Module
//    {
//        private static Module1 _this = null;

//        public combox_fishnet_size combox_fishnet_size1 { get; set; }

//        public combox_DecimalPointDigitNum combox_DecimalPointDigitNum1 { get; set; }

//        public EditBox_WeightField EditBox_WeightField1 { get; set; }
      
//        public EditBox_AreaTolerance EditBox_AreaTolerance1 { get; set; }

//        public EditBox_BufferSearchTolerance EditBox_BufferSearchTolerance1 { get; set; }

//        public EditBox_BufferSearchDistance EditBox_BufferSearchDistance1 { get; set; }


//        public EditBox_段號Field EditBox_SecNoField1 { get; set; }

//        public EditBox_登記面積Field EditBox_登記面積Field1 { get; set; }

//        /// <summary>
//        /// Retrieve the singleton instance to this module here
//        /// </summary>
//        public static Module1 Current
//        {
//            get
//            {
//                return _this ?? (_this = (Module1)FrameworkApplication.FindModule("StreetViewPictureProcessorAddin_Module"));
//            }
//        }

//        #region Overrides
//        /// <summary>
//        /// Called by Framework when ArcGIS Pro is closing
//        /// </summary>
//        /// <returns>False to prevent Pro from closing, otherwise True</returns>
//        protected override bool CanUnload()
//        {
//            //TODO - add your business logic
//            //return false to ~cancel~ Application close
//            return true;
//        }

//        #endregion Overrides

//    }
//}
