//using ArcGIS.Desktop.Framework.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetViewPictureProcessorAddin
//{
//    class EditBox_段號Field : ArcGIS.Desktop.Framework.Contracts.EditBox
//    {
//        public EditBox_段號Field()
//        {
//            Module1.Current.EditBox_SecNoField1 = this;
//            Text = LandUpdateManager._strFieldName_段號;// "";

//            this.PropertyChanged += EditBox_段號Field_PropertyChanged;
//        }

//        private void EditBox_段號Field_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            EditBox txtBox = sender as EditBox;

//            try
//            {
//                LandUpdateManager._strFieldName_段號 = txtBox.Text;
//            }
//            catch (Exception ex)
//            {
//                txtBox.Text = LandUpdateManager._strFieldName_段號;
//            }

//        }

//    }
//}
