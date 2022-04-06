//using ArcGIS.Desktop.Framework.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetViewPictureProcessorAddin
//{
//    public class combox_DecimalPointDigitNum : ArcGIS.Desktop.Framework.Contracts.ComboBox
//    {
//        private bool _isInitialized;


//        public combox_DecimalPointDigitNum()
//        {
//            UpdateCombo();

//            Module1.Current.combox_DecimalPointDigitNum1 = this;

//        }

//        protected override void OnSelectionChange(ComboBoxItem item)
//        {
//            RasterlizeManager._strDecimalPointDigitNum = SelectedItem.ToString();
//        }

//        private void UpdateCombo()
//        {
//            string strSelectedItem = RasterlizeManager._strDecimalPointDigitNum;// "0";


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

//                Add(new ComboBoxItem("0"));
//                Add(new ComboBoxItem("1"));
//                Add(new ComboBoxItem("2"));
//                Add(new ComboBoxItem("3"));
//                Add(new ComboBoxItem("4"));

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
