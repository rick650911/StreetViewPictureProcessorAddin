//using ArcGIS.Desktop.Framework.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetViewPictureProcessorAddin
//{
//    class EditBox_BufferSearchDistance : ArcGIS.Desktop.Framework.Contracts.EditBox
//    {
//        public EditBox_BufferSearchDistance()
//        {
//            Module1.Current.EditBox_BufferSearchDistance1 = this;
//            Text = LandUpdateManager._strBufferSearchDistance; ;// "";

//            this.PropertyChanged += EditBox_BufferSearchDistance_PropertyChanged;
//        }

//        private void EditBox_BufferSearchDistance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            EditBox txtBox = sender as EditBox;

//            try
//            {
//                double dblBufferSearchDistance;

//                bool blParseOk = double.TryParse(txtBox.Text,out dblBufferSearchDistance);

//                if (blParseOk)
//                {
//                    LandUpdateManager._strBufferSearchDistance = txtBox.Text;
//                    LandUpdateManager._dblBufferSearchDistance = dblBufferSearchDistance;

//                }
                
//            }
//            catch (Exception ex)
//            {
//                txtBox.Text = LandUpdateManager._strBufferSearchDistance;// "0.01";
//                //LandUpdateManager._strBufferSearchDistance = "0.01";
//                //LandUpdateManager._dblBufferSearchDistance = 0.01;
//            }

//        }

//    }
//}
