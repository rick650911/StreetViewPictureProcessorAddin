//using ArcGIS.Desktop.Framework.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetViewPictureProcessorAddin
//{
//    public class combox_fishnet_size : ArcGIS.Desktop.Framework.Contracts.ComboBox
//    {
//        private bool _isInitialized;


//        public combox_fishnet_size()
//        {
//            UpdateCombo();

//            Module1.Current.combox_fishnet_size1 = this;

//        }

//        protected override void OnSelectionChange(ComboBoxItem item)
//        {
//            RasterlizeManager._strFishnet_size = SelectedItem.ToString();
//        }

//        private void UpdateCombo()
//        {
//            string strSelectedItem = RasterlizeManager._strFishnet_size;// "1000";


//            if (_isInitialized)
//            {
//                //設定SelectedItem
//                var query1 = from selected in ItemCollection
//                             where selected.ToString() == strSelectedItem
//                             select selected;

//                foreach (var q in query1)
//                {
//                    SelectedItem = q;//直接設定字串會無效，要取ItemCollection進行設定
//                }
//            }

//            if (!_isInitialized)
//            {
//                Clear();

//                Add(new ComboBoxItem("25000"));
//                Add(new ComboBoxItem("5000"));
//                Add(new ComboBoxItem("1000"));
//                Add(new ComboBoxItem("200"));
//                Add(new ComboBoxItem("40"));

//                _isInitialized = true;
//            }

//            Enabled = true; //enables the ComboBox

//            //設定SelectedItem
//            var query2 = from selected in ItemCollection
//                         where selected.ToString() == strSelectedItem
//                         select selected;

//            foreach (var q in query2)
//            {
//                SelectedItem = q;  //直接設定字串會無效，要取ItemCollection進行設定
//            }

//        }
//    }
//}
