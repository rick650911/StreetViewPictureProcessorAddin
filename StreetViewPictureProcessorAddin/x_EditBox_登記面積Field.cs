//using ArcGIS.Desktop.Framework.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetViewPictureProcessorAddin
//{
//    class EditBox_登記面積Field : ArcGIS.Desktop.Framework.Contracts.EditBox
//    {
//        public EditBox_登記面積Field()
//        {
//            Module1.Current.EditBox_登記面積Field1 = this;
//            Text = LandUpdateManager._strFieldName_登記面積;// "";

//            this.PropertyChanged += EditBox_登記面積Field_PropertyChanged;
//        }

//        private void EditBox_登記面積Field_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            EditBox txtBox = sender as EditBox;

//            try
//            {
//                LandUpdateManager._strFieldName_登記面積 = txtBox.Text;
//            }
//            catch (Exception ex)
//            {
//                txtBox.Text = LandUpdateManager._strFieldName_登記面積;


//            }

//        }

//    }
//}
