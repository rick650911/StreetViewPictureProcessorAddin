//using ArcGIS.Desktop.Framework.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetViewPictureProcessorAddin
//{
//    class EditBox_WeightField : ArcGIS.Desktop.Framework.Contracts.EditBox
//    {
//        public EditBox_WeightField()
//        {
//            Module1.Current.EditBox_WeightField1 = this;
//            Text = RasterlizeManager._strWeightField;// "";

//            this.PropertyChanged += EditBox_WeightField_PropertyChanged;
//        }

//        private void EditBox_WeightField_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            EditBox txtBox = sender as EditBox;

//            try
//            {
//                RasterlizeManager._strWeightField = txtBox.Text;
//            }
//            catch (Exception ex)
//            {
//                txtBox.Text = RasterlizeManager._strWeightField;
                
//            }

//        }

//    }
//}
