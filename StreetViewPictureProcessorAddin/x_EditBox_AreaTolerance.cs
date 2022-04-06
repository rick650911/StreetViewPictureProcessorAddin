//using ArcGIS.Desktop.Framework.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetViewPictureProcessorAddin
//{
//    class EditBox_AreaTolerance : ArcGIS.Desktop.Framework.Contracts.EditBox
//    {
//        public EditBox_AreaTolerance()
//        {
//            Module1.Current.EditBox_AreaTolerance1 = this;
//            Text = LandUpdateManager._strAreaTolerance; ;// "";

//            this.PropertyChanged += EditBox_AreaTolerance_PropertyChanged;
//        }

//        private void EditBox_AreaTolerance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            EditBox txtBox = sender as EditBox;

//            try
//            {
//                double dblAreaTolerance;

//                bool blParseOk = double.TryParse(txtBox.Text,out dblAreaTolerance);

//                if (blParseOk)
//                {
//                    LandUpdateManager._strAreaTolerance = txtBox.Text;
//                    LandUpdateManager._dblAreaTolerance = dblAreaTolerance;

//                    LandUpdateManager._strAreaTolerance_分割_合併 = txtBox.Text;
//                    LandUpdateManager._dblAreaTolerance_分割_合併 = dblAreaTolerance;

//                }
                
//            }
//            catch (Exception ex)
//            {
//                txtBox.Text = LandUpdateManager._strAreaTolerance;// "0.01";
//                //LandUpdateManager._strAreaTolerance = "0.01";
//                //LandUpdateManager._dblAreaTolerance = 0.01;
//            }

//        }

//    }
//}
