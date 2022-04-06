using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityClassLibrary;

namespace StreetViewPictureProcessorAddin
{
    internal class btnStreeviewPicProcess : Button
    {
        protected override async void OnClick()
        {
            //RasterlizeManager._CancelableProgressorSource = new ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource("街景照片處理", "取消");
            //RasterlizeManager._CancelableProgressorSource.Progressor.Max = (uint)100;

            await QueuedTask.Run(async () =>
            {
                //(1)農工通知互動"照片轉點位處理"
                //1 檢查農工通知互動"照片轉點位處理"============================================
                //1.0 檢查資料表中job_type_code是null 的
                //DBO.StreetViewPhotoProcessInfom

                mdlProcessItem pmdlProcessItem = null;

                StreetViewModule.GetJobIdBy_job_type_code(" is null", out pmdlProcessItem);//out str_jobid, out strFolderPath_in,out row_job);

                if (pmdlProcessItem == null) //string.IsNullOrEmpty(strFolderPath_in) || string.IsNullOrEmpty(str_jobid))
                {
                    return;//沒有待處理的資料
                }

                //複製處理的file geodatabase
                string str_jodid_reg = pmdlProcessItem._str_jobid.Replace("(", "_").Replace(")", "_");
                string strFGDBPath_out = StreetViewModule._strFGDBPath_template.Replace("empty", str_jodid_reg);

                bool blOk = IoUtility.DirectoryCopy(StreetViewModule._strFGDBPath_template, strFGDBPath_out, true);

                if (!blOk)
                {
                    //新增訊息通知
                    StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-複製處理的file geodatabase", pmdlProcessItem);
                                        
                    return;
                }

                //1.1 已完成的資料夾照片轉點位==================================================
                string strLayerName_PhotosToPoints_out = "PhotosToPoints_" + str_jodid_reg;//圖層名稱不能有特殊字元 

                FeatureLayer featureLayer_PhotosToPoints = null;
                List<IGPResult> list_output_GpResult = new List<IGPResult>();
                List<string> list_output_ErrorMsg = new List<string>();

                GpTools.GeoTaggedPhotosToPoints(pmdlProcessItem._strFolderPath_in, strFGDBPath_out + @"\" + strLayerName_PhotosToPoints_out, list_output_GpResult, list_output_ErrorMsg);

                if (list_output_GpResult.Count == 1)
                {
                    IGPResult gpResult = list_output_GpResult[0];

                    if (gpResult.IsFailed == true)
                    {
                        //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("GeoTaggedPhotosToPoints處理發生錯誤: " + gpResult.ErrorCode.ToString());
                        
                        //新增訊息通知
                        StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-GeoTaggedPhotosToPoints處理", pmdlProcessItem);

                        return;
                    }

                    //取得點位圖層
                    featureLayer_PhotosToPoints = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == strLayerName_PhotosToPoints_out).FirstOrDefault() as FeatureLayer;

                    if (featureLayer_PhotosToPoints == null)
                    {
                        //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("取得相片轉點圖層失敗: ");

                        //新增訊息通知
                        StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-取得相片轉點圖層", pmdlProcessItem);

                        return;
                    }
                }

                //1.2 加欄位：加st_dist、jobid、Direction_camara等欄位
                blOk = StreetViewModule.addFields_featureLayer_PhotosToPoints(featureLayer_PhotosToPoints);

                if (!blOk)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增欄位發生錯誤 ");

                    //新增訊息通知
                    StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-新增欄位", pmdlProcessItem);

                    return;
                }

                //照片複製到圖台虛擬目錄(圖8處理完後，宸訊即會取用)  ，並於Path欄位記錄
                //to do 連線unc路徑 (連線帳號需有目的路徑寫入權限)
                string strFolderPath_web_virtual_folder_pic = StreetViewModule._strFolderPath_web_virtual_folder_root_unc + @"\" + pmdlProcessItem._str_jobid;
                string strFolderPath_web_virtual_folder_pic_unc_to_record_db = StreetViewModule._strFolderPath_web_virtual_folder_root_unc_to_record_db + @"\" + pmdlProcessItem._str_jobid;

                blOk = IoUtility.DirectoryCopy(pmdlProcessItem._strFolderPath_in, strFolderPath_web_virtual_folder_pic, true);

                if (blOk==false)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("照片複製到圖台虛擬目錄發生錯誤 ");

                    //新增訊息通知
                    StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-照片複製到圖台虛擬目錄", pmdlProcessItem);

                    return;
                }

                //讀取說明檔exif2.txt，取得Direction_camara、時間調整天/小時
                double dbl_direction_camara = -1;
                int int_day = 0;
                int int_hour = 0;
                string str_project_name;
                string str_調查員;
                //string str_調查修正;

                blOk = StreetViewModule.GetExif2(pmdlProcessItem._strFolderPath_in,
                                out dbl_direction_camara,
                                out str_project_name,
                                out str_調查員,
                                //out str_調查修正, 
                                out int_day,
                                out int_hour);

                if (blOk == false)//未解析出說明檔中的鏡頭角度等資訊
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("解析說明檔發生錯誤 ");

                    //新增訊息通知
                    StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-解析說明檔", pmdlProcessItem);

                    return;
                }

                //field caculate：加上Path、jobid、Direction_camara欄位值、Path欄位值、調整時間欄位值、project_name欄位值、調查員欄位值
                string strErrorFieldName = null;
                string strErrorExpression = null;

                blOk = StreetViewModule.calculateFields_featureLayer_PhotosToPoints(featureLayer_PhotosToPoints,
                                                         strFolderPath_web_virtual_folder_pic_unc_to_record_db,
                                                         pmdlProcessItem,
                                                         str_project_name,
                                                         str_調查員,
                                                         dbl_direction_camara,
                                                         int_day,
                                                         int_hour,
                                                         out strErrorFieldName,
                                                         out strErrorExpression);
         
                if (blOk == false)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("欄位計算發生錯誤["+ strErrorFieldName+"] expression["+strErrorExpression+"]");

                    //新增訊息通知
                    StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-"+ "欄位計算[" + strErrorFieldName + "] expression[" + strErrorExpression + "]", pmdlProcessItem);

                    return;
                }

                //1.3 篩選處理(3M內只留一筆)==================================================
                //圖層匯到twd97 dataset將坐標系統改為twd97
                string strLayerName_PhotosToPoints_twd97_out = strLayerName_PhotosToPoints_out + "_twd97";

                list_output_GpResult = new List<IGPResult>();
                list_output_ErrorMsg = new List<string>();

                GpTools.FeatureClassToFeatureClass(featureLayer_PhotosToPoints,
                                             strFGDBPath_out + @"\" + StreetViewModule._strFGDBDatasetName_out,
                                             strLayerName_PhotosToPoints_twd97_out,
                                             "",
                                             list_output_GpResult,
                                             list_output_ErrorMsg);

                if (list_output_GpResult.Count == 1)
                {
                    IGPResult gpResult = list_output_GpResult[0];

                    if (gpResult.IsFailed == true)
                    {
                        //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("FeatureClassToFeatureClass發生錯誤: " + gpResult.ErrorCode.ToString());

                        //新增訊息通知
                        StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-FeatureClassToFeatureClass", pmdlProcessItem);

                        return;
                    }

                    featureLayer_PhotosToPoints = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == strLayerName_PhotosToPoints_twd97_out).FirstOrDefault() as FeatureLayer;

                    if (featureLayer_PhotosToPoints == null)
                    {
                        //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("取得相片轉點圖層(twd97)失敗: ");

                        //新增訊息通知
                        StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-取得相片轉點圖層(twd97)", pmdlProcessItem);

                        return;
                    }
                }

                //計算每點跟其基準點的距離 --> 取消，改由圖霸處理
                //blOk = StreetViewModule.CalculateDistanceWithBasePoint_EachPoint(featureLayer_PhotosToPoints);

                //if (!blOk)
                //{
                //    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("計算每點跟其基準點的距離發生錯誤 ");
                //    return;
                //}


                //1.4 寫入照片點位feature service 的 sde圖層 (append)
                //to do 寫入前先檢查是否有該jobid資料

                //選取 st_dist >= 3M 的圖徵 --> 取消，改由圖霸處理
                //QueryFilter pQF_3m = new QueryFilter();
                //pQF_3m.WhereClause = StreetViewModule.strFieldName_PhotosToPoints_st_dist + ">=" + StreetViewModule.dblDistanceTolerance;

                //featureLayer_PhotosToPoints.Select(pQF_3m);

                list_output_GpResult = new List<IGPResult>();
                list_output_ErrorMsg = new List<string>();

                List<object> listFeatureLayer_ToAppend = new List<object>();
                listFeatureLayer_ToAppend.Add(featureLayer_PhotosToPoints);

                GpTools.Append(listFeatureLayer_ToAppend, StreetViewModule._strSDEFilePath + @"\" + StreetViewModule._strDatasetName_StreetViewPhoto + @"\" + StreetViewModule._strLayerName_StreetViewPhoto, list_output_GpResult, list_output_ErrorMsg);

                if (list_output_GpResult.Count == 1)
                {
                    IGPResult gpResult = list_output_GpResult[0];

                    if (gpResult.IsFailed == true)
                    {
                        //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());

                        //新增訊息通知
                        StreetViewModule.AddJobInform(11, "錯誤：照片轉點位-append處理", pmdlProcessItem);

                        return;
                    }
                }

                //通知圖霸進行點位修正                
                StreetViewModule.AddJobInform(2, "待處理：加拍照點位行進方向", pmdlProcessItem);

                //(2)圖霸通知"加拍照點位行進方向處理"完成
                //2 檢查圖霸通知"加拍照點位行進方向處理"完成
                //2.1 篩選處理(未算出Direction 9999)、匯出到處理中圖層
                //2.2 加欄位Direction_heading,X_old,Y_old
                //2.3 欄位計算
                //2.3.1 Direction_heading = Direction
                //2.3.2 Direction = Direction + Direction_camara
                //2.3.3 if direction_new > 360  Direction = Direction - 360
                //2.3.4 Project = jobid
                //2.3.5 X_old = X
                //2.3.6 Y_old = Y
                //2.3.7 X = X1
                //2.3.8 Y = Y1

                //2.4 匯入到上線圖層
                //2.5 照片資料夾(jobid)放入虛擬目錄


                //////if (MapView.Active.Map.SpatialReference == null || 
                //////   (MapView.Active.Map.SpatialReference!=null && MapView.Active.Map.SpatialReference.Wkid!=3826))
                //////{
                //////    //Name = "TWD_1997_TM_Taiwan"
                //////    //Wkid = 3826
                //////    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地圖坐標系統需為 TWD_1997_TM_Taiwan. 離開...", "Info");
                //////    return;
                //////}

                //////// Get the layer(s) selected in the Contents pane, if there is not just one, then prompt then exit.
                //////if (MapView.Active.GetSelectedLayers().Count != 1)
                //////{
                //////    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇一個向量面圖層. 離開...", "Info");
                //////    return;
                //////}
                //////// Check to see if the selected layer is a feature layer, if not, then prompt and exit.
                //////var featLayer_input = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

                //////if (featLayer_input == null)
                //////{
                //////    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇一個向量面圖層. 離開...", "Info");
                //////    return;
                //////}

                //////List<IGPResult> list_output_GpResult = new List<IGPResult>();
                //////List<string> list_output_ErrorMsg = new List<string>();







            }/*, RasterlizeManager._CancelableProgressorSource.Progressor*/);
        }
    }
}
