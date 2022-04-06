//using ArcGIS.Core.CIM;
//using ArcGIS.Core.Data;
//using ArcGIS.Core.Geometry;
//using ArcGIS.Desktop.Catalog;
//using ArcGIS.Desktop.Core;
//using ArcGIS.Desktop.Core.Geoprocessing;
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

//namespace StreetViewPictureProcessorAddin
//{
//    internal class x_btnLandUpdate : Button
//    {
//        protected override async void OnClick()
//        {
//            try
//            {
//                    // Check for an active mapview, if not, then prompt and exit.
//                    if (MapView.Active == null)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("No MapView currently active. Exiting...", "Info");
//                        return;
//                    }
//                    // Get the layer(s) selected in the Contents pane, if there is not just one, then prompt then exit.
//                    if (MapView.Active.GetSelectedLayers().Count != 2)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇2個向量面圖層. 離開...", "Info");
//                        return;
//                    }
//                    // Check to see if the selected layer is a feature layer, if not, then prompt and exit.
//                    LandUpdateManager._featurelayer_old = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

//                    if (LandUpdateManager._featurelayer_old == null)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇向量面圖層. 離開...", "Info");
//                        return;
//                    }

//                    LandUpdateManager._featurelayer_new = MapView.Active.GetSelectedLayers()[1] as FeatureLayer;

//                    if (LandUpdateManager._featurelayer_new == null)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇向量面圖層. 離開...", "Info");
//                        return;
//                    }
             
//                //try
//                //{


//                LandUpdateManager._CancelableProgressorSource = new ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource("地籍更新", "取消");
//                LandUpdateManager._CancelableProgressorSource.Progressor.Max = (uint)100;

//                await QueuedTask.Run(async  () =>
//                {
//                    //檢查是否在同一個gdb，否則add join 會有問題
//                    string strPath_old = LandUpdateManager._featurelayer_old.GetFeatureClass().GetPath().ToString();
//                    string strPath_new = LandUpdateManager._featurelayer_new.GetFeatureClass().GetPath().ToString();

//                    int intIdx_gdb_old = strPath_old.IndexOf(".gdb");
//                    int intIdx_gdb_new = strPath_new.IndexOf(".gdb");

//                    if (intIdx_gdb_old==-1 || intIdx_gdb_new == -1 || (intIdx_gdb_old!= intIdx_gdb_new))
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新舊版地籍圖資需放在同一file geodatabase。" );
//                        return;
//                    }

//                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("開始在選擇的第2個圖層中尋找所選擇第1個圖層的資料.");
//                    TableDefinition tableDefinition_old = LandUpdateManager._featurelayer_old.GetTable().GetDefinition();
//                    TableDefinition tableDefinition_new = LandUpdateManager._featurelayer_new.GetTable().GetDefinition();

//                    IReadOnlyList<Field> fields_old = tableDefinition_old.GetFields();
//                    IReadOnlyList<Field> fields_new = tableDefinition_new.GetFields();

//                    LandUpdateManager._strFieldName_OID_old = fields_old.FirstOrDefault(a => a.FieldType == FieldType.OID).Name;
//                    LandUpdateManager._strFieldName_OID_new = fields_new.FirstOrDefault(a => a.FieldType == FieldType.OID).Name;

//                    LandUpdateManager._intFldIdx_段號_old = tableDefinition_old.FindField(LandUpdateManager._strFieldName_段號);
//                    LandUpdateManager._intFldIdx_段號_new = tableDefinition_new.FindField(LandUpdateManager._strFieldName_段號);

//                    LandUpdateManager._intFldIdx_登記面積_old = tableDefinition_old.FindField(LandUpdateManager._strFieldName_登記面積);
//                    LandUpdateManager._intFldIdx_登記面積_new = tableDefinition_new.FindField(LandUpdateManager._strFieldName_登記面積);

//                    LandUpdateManager._intFldIdx_lu_found_old = tableDefinition_old.FindField(LandUpdateManager._strFieldName_lu_found);

//                    LandUpdateManager._intFldIdx_lu_type_old = tableDefinition_old.FindField(LandUpdateManager._strFieldName_lu_type);
//                    LandUpdateManager._intFldIdx_lu_type_new = tableDefinition_new.FindField(LandUpdateManager._strFieldName_lu_type);

//                    LandUpdateManager._intFldIdx_lu_ar_rt_old = tableDefinition_old.FindField(LandUpdateManager._strFieldName_lu_ar_rt);

//                    if (LandUpdateManager._intFldIdx_段號_old == -1)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("舊版地籍欄位不存在：" + LandUpdateManager._strFieldName_段號);
//                        return;
//                    }

//                    if (LandUpdateManager._intFldIdx_段號_new == -1)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新版地籍欄位不存在：" + LandUpdateManager._strFieldName_段號);
//                        return;
//                    }

//                    if (LandUpdateManager._intFldIdx_登記面積_old == -1)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("舊版地籍欄位不存在：" + LandUpdateManager._strFieldName_登記面積);
//                        return;
//                    }

//                    if (LandUpdateManager._intFldIdx_登記面積_new == -1)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新版地籍欄位不存在：" + LandUpdateManager._strFieldName_登記面積);
//                        return;
//                    }

//                    //LandUpdateManager._intFldIdx_lu_log_old = tableDefinition_old.FindField(LandUpdateManager._strFieldName_lu_log);
//                    //LandUpdateManager._intFldIdx_lu_log_new = tableDefinition_new.FindField(LandUpdateManager._strFieldName_lu_log);

//                    List<IGPResult> list_output_GpResult = new List<IGPResult>();
//                    List<string> list_output_ErrorMsg = new List<string>();

//                    //產生結果圖層         
//                    string strSerialNo = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;//流水號

//                    string strLayerName_result = "result_" + strSerialNo;
//                    FeatureLayer featureLayer_result = null;
//                    string strExp_new = LandUpdateManager._strFieldName_OID_new + "=-999";//"1=2";//1=2 在shapefile會失效

//                    GpTools.FeatureClassToFeatureClass(LandUpdateManager._featurelayer_new,
//                                                   Project.Current.DefaultGeodatabasePath,
//                                                   strLayerName_result,
//                                                   strExp_new,
//                                                   list_output_GpResult,
//                                                   list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("產生結果圖層發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }

//                        featureLayer_result = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == strLayerName_result).FirstOrDefault() as FeatureLayer;

//                        if (featureLayer_result == null)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("取得結果圖層失敗: ");
//                            return;
//                        }
//                    }

//                    //建立欄位-lu_log
//                    //if (LandUpdateManager._intFldIdx_lu_log == -1)
//                    //{
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    //GpTools.AddField(LandUpdateManager._featurelayer_new, LandUpdateManager._strFieldName_lu_log, "TEXT",
//                    GpTools.AddField(strLayerName_result, LandUpdateManager._strFieldName_lu_log, "TEXT",
//                                        "", "", "90",
//                                       LandUpdateManager._strFieldName_lu_log, "NULLABLE",
//                                       list_output_GpResult,
//                                       list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[lu_log]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }
//                    //}

//                    //建立欄位-lu_type
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    GpTools.AddField(strLayerName_result, LandUpdateManager._strFieldName_lu_type, "SHORT",
//                                    "", "", "",
//                                   LandUpdateManager._strFieldName_lu_type, "NULLABLE",
//                                   list_output_GpResult,
//                                   list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[lu_type]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    //欄位不存在建立欄位-原段號
//                    //if (LandUpdateManager._intFldIdx_lu_log_new == -1)
//                    //{
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    //GpTools.AddField(LandUpdateManager._featurelayer_new, LandUpdateManager._strFieldName_lu_log, "TEXT",
//                    GpTools.AddField(strLayerName_result, LandUpdateManager._strFieldName_原段號, "TEXT",
//                                "", "", "700",
//                               LandUpdateManager._strFieldName_原段號, "NULLABLE",
//                               list_output_GpResult,
//                               list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[原段號]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }
//                    //}

//                    //欄位不存在建立欄位-lu_found
//                    if (LandUpdateManager._intFldIdx_lu_found_old == -1)
//                    {
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        GpTools.AddField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_found, "TEXT",
//                                "", "", "5",
//                               LandUpdateManager._strFieldName_lu_found, "NULLABLE",
//                               list_output_GpResult,
//                               list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[lu_found]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
//                                return;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        //清空欄位
//                        //field calculate-lu_found
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        string strExp = "'" + "" + "'";
//                        string strCodeBlock = string.Empty;
//                        GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_found, strExp, "Arcade", strCodeBlock,
//                    list_output_GpResult,
//                    list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + LandUpdateManager._featurelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_found + " exp:" + strExp + " code block:" + strCodeBlock);
//                                return;
//                            }
//                        }
//                    }

//                    //欄位不存在建立欄位-lu_type
//                    if (LandUpdateManager._intFldIdx_lu_type_old == -1)
//                    {
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        GpTools.AddField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_type, "SHORT",
//                                "", "", "",
//                               LandUpdateManager._strFieldName_lu_type, "NULLABLE",
//                               list_output_GpResult,
//                               list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[lu_type]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
//                                return;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        //清空欄位
//                        //field calculate-lu_type
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        string strExp = "0";
//                        string strCodeBlock = string.Empty;
//                        GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                    list_output_GpResult,
//                    list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + LandUpdateManager._featurelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                                return;
//                            }
//                        }
//                    }

//                    //欄位不存在建立欄位-lu_ar_rt
//                    if (LandUpdateManager._intFldIdx_lu_ar_rt_old == -1)
//                    {
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        //GpTools.AddField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_ar_rt, "DOUBLE",//"DOUBLE",
//                        //        "", "", "",
//                        //       LandUpdateManager._strFieldName_lu_ar_rt, "NULLABLE",
//                        //       list_output_GpResult,
//                        //       list_output_ErrorMsg);

//                        GpTools.AddField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_ar_rt, "DOUBLE",//"面積比"
//                                        "", "", "",
//                                        LandUpdateManager._strFieldName_lu_ar_rt, "NULLABLE",
//                                        list_output_GpResult,
//                                        list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[lu_ar_rt]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
//                                return;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        //清空欄位
//                        //field calculate-lu_ar_rt
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        string strExp = "999";
//                        string strCodeBlock = string.Empty;
//                        GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_ar_rt, strExp, "Arcade", strCodeBlock,
//                    list_output_GpResult,
//                    list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + LandUpdateManager._featurelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_ar_rt + " exp:" + strExp + " code block:" + strCodeBlock);
//                                return;
//                            }
//                        }
//                    }

//                    ////欄位不存在建立欄位-lu_type
//                    //if (LandUpdateManager._intFldIdx_lu_type_new == -1)
//                    //{
//                    //    list_output_GpResult = new List<IGPResult>();
//                    //    list_output_ErrorMsg = new List<string>();

//                    //    GpTools.AddField(LandUpdateManager._featurelayer_new, LandUpdateManager._strFieldName_lu_type, "SHORT",
//                    //                "", "", "",
//                    //               LandUpdateManager._strFieldName_lu_type, "NULLABLE",
//                    //               list_output_GpResult,
//                    //               list_output_ErrorMsg);

//                    //    if (list_output_GpResult.Count == 1)
//                    //    {
//                    //        IGPResult gpResult = list_output_GpResult[0];

//                    //        if (gpResult.IsFailed == true)
//                    //        {
//                    //            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[lu_type]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
//                    //            return;
//                    //        }
//                    //    }
//                    //}
//                    //else
//                    //{
//                    //    //清空欄位
//                    //    //field calculate-lu_type
//                    //    list_output_GpResult = new List<IGPResult>();
//                    //    list_output_ErrorMsg = new List<string>();

//                    //    string strExp = "0";
//                    //    string strCodeBlock = string.Empty;
//                    //    GpTools.CalculateField(LandUpdateManager._featurelayer_new, LandUpdateManager._strFieldName_lu_type, strExp, strCodeBlock,
//                    //    list_output_GpResult,
//                    //    list_output_ErrorMsg);

//                    //    if (list_output_GpResult.Count == 1)
//                    //    {
//                    //        IGPResult gpResult = list_output_GpResult[0];

//                    //        if (gpResult.IsFailed == true)
//                    //        {
//                    //            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                    //            return;
//                    //        }
//                    //    }
//                    //}

           
              
//                    var featSelectionOIDs = LandUpdateManager._featurelayer_old.GetSelection().GetObjectIDs();

//                    bool blNoSelect_old = false;//原圖層是否沒有選取圖徵

//                    if (featSelectionOIDs.Count < 1)
//                    {
//                        //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請選取要更新的圖徵, " + LandUpdateManager._featurelayer_old.Name + "...", "資訊");
//                        //return;

//                        LandUpdateManager._featurelayer_old.Select(null);
//                        blNoSelect_old = true;
//                    }

//                    //重置已找到的OID清單
//                    LandUpdateManager._listFound_OID_old.Clear();
//                    //LandUpdateManager._listNotFound_OID_old.Clear();
//                    //LandUpdateManager._listFound_OID_new.Clear();
//                    //LandUpdateManager._listFound_Land_old.Clear();
//                    LandUpdateManager._listPossibleFound_Land_old_合併.Clear();
//                    LandUpdateManager._listPossibleFound_Land_old_分割.Clear();
//                    LandUpdateManager._listPossibleFound_Land_old.Clear();

//                    IReadOnlyList<long> readOnlyList_OID_sel_old = LandUpdateManager._featurelayer_old.GetSelection().GetObjectIDs();
//                    List<long> list_OID_sel_ini = new List<long>();

//                    string strOIDs_sel_ini = string.Empty;

//                    foreach (long lngOID in readOnlyList_OID_sel_old)
//                    {
//                        strOIDs_sel_ini += lngOID + ",";
//                        list_OID_sel_ini.Add(lngOID);
//                    }

//                    if (strOIDs_sel_ini.EndsWith(","))
//                    {
//                        strOIDs_sel_ini = strOIDs_sel_ini.Substring(0, strOIDs_sel_ini.Length - 1);
//                    }

//                    QueryFilter pQf_old_sel_ini = new QueryFilter();
               
//                    RowCursor rowCursor = null;

//                    if (blNoSelect_old)
//                    {
//                        rowCursor = LandUpdateManager._featurelayer_old.Search(null);
//                    }
//                    else
//                    {
                      
//                        rowCursor = LandUpdateManager._featurelayer_old.GetSelection().Search(null);
//                        pQf_old_sel_ini.WhereClause = LandUpdateManager._strFieldName_OID_old + " in (" + strOIDs_sel_ini + ")";

//                    }

//                    //#1 修正地籍圖版次因接合對位造成偏移差異 -->找14碼、登記面積都一樣(無變動) --> 批次處理
//                    LandUpdateFunctiions.search_14code_area_same_段號相同_batch(LandUpdateManager._featurelayer_old,
//                                                                               LandUpdateManager._featurelayer_new,
//                                                                               featureLayer_result);

//                    using (rowCursor)//(Feature)rowCursor.Current)  //處理每一筆舊版地籍
//                    {
//                        while (rowCursor.MoveNext())
//                        {
//                            using (Feature pFeature_old = (Feature)rowCursor.Current)  //處理每一筆舊版地籍
//                            {
//                                bool blFound = false;
//                                string str_log = "";

//                                if (LandUpdateManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                                {
//                                    //strCompleOrCancal = "取消";
//                                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地籍更新" + "取消", "資訊");
//                                    return;
//                                }

//                                //LandUpdateManager.search_14code_area_same_段號相同(pFeature_old, featureLayer_result, out str_log, out blFound);

//                                //#1 修正地籍圖版次因接合對位造成偏移差異 -->找14碼、登記面積都一樣    (無變動)
//                                object obj_lu_type = pFeature_old[LandUpdateManager._intFldIdx_lu_type_old];

//                                string str_lu_type = null;
//                                short sht_lu_type = -1;

//                                if (obj_lu_type != null )
//                                {
//                                    str_lu_type = obj_lu_type.ToString().Trim();

//                                    bool blParseOk = short.TryParse(str_lu_type, out sht_lu_type);

//                                    if (sht_lu_type == 1)
//                                    {
//                                        blFound = true;
//                                    }
//                                }

//                                if (blFound)
//                                {
//                                    LandUpdateManager._listFound_OID_old.Add(pFeature_old.GetObjectID());
//                                    continue;
//                                }

//                                if (LandUpdateManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                                {
//                                    //strCompleOrCancal = "取消";
//                                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地籍更新" + "取消", "資訊");
//                                    return;
//                                }

//                                //#2 比對因土地重劃之段號變更 -->找14碼變、center in 、登記面積變化<1% (重劃)
//                                LandUpdateManager.search_new_center_in_area_same_重劃(pFeature_old, featureLayer_result, out str_log, out blFound);

//                                if (blFound)
//                                {
//                                    continue;
//                                }

//                                if (LandUpdateManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                                {
//                                    //strCompleOrCancal = "取消";
//                                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地籍更新" + "取消", "資訊");
//                                    return;
//                                }
//                                //#4 比對地籍分割 -->可能分割(新版center in舊版，新版面積變小)
//                                LandUpdateManager.search_new_center_in_area_samller_possible_分割(pFeature_old, featureLayer_result, out str_log, out blFound);

//                                if (blFound)
//                                {
//                                    continue;
//                                }

//                                if (LandUpdateManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                                {
//                                    //strCompleOrCancal = "取消";
//                                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地籍更新" + "取消", "資訊");
//                                    return;
//                                }
//                                //#3 比對地籍合併 -->可能合併(舊版center in新版，新版面積變大，反向intersect找出被合併的地籍)
//                                LandUpdateManager.search_old_center_in_area_larger_possible_合併(pFeature_old, featureLayer_result, out str_log, out blFound);

//                                //if (!blFound)
//                                //{
//                                //    //記錄not found
//                                //    LandUpdateManager._listNotFound_OID_old.Add(pFeature_old.GetObjectID());
//                                //}

                                
//                            }
//                        }//while (rowCursor.MoveNext())
//                    }

//                    //#3 #4 檢查分割或合併之前後面積和是否等於 -->start /////////////////                    
//                    Dictionary<long, List<mdlLand>> dicOID_newLand_分割 = new Dictionary<long, List<mdlLand>>();//1對多
//                    Dictionary<long, List<mdlLand>> dicOID_newLand_合併 = new Dictionary<long, List<mdlLand>>();//1對多

//                    foreach (mdlLand mdlLand_possible in LandUpdateManager._listPossibleFound_Land_old)
//                    {
//                        //mdlLand mdlLand_possible = kvp.Value;

//                        if (mdlLand_possible.新版地籍狀態.Equals(enumTypeUpdateStatus.分割))
//                        {
//                            List<mdlLand> listLand_new = null;

//                            if (dicOID_newLand_分割.ContainsKey(mdlLand_possible.原OID))
//                            {
//                                listLand_new = dicOID_newLand_分割[mdlLand_possible.原OID];
//                                listLand_new.Add(mdlLand_possible);
//                            }
//                            else
//                            {
//                                listLand_new = new List<mdlLand>();
//                                listLand_new.Add(mdlLand_possible);
//                                dicOID_newLand_分割.Add(mdlLand_possible.原OID, listLand_new);
//                            }
//                        }
//                        else if (mdlLand_possible.新版地籍狀態.Equals(enumTypeUpdateStatus.合併))
//                        {
//                            List<mdlLand> listLand_old = null;

//                            if (dicOID_newLand_合併.ContainsKey(mdlLand_possible.新OID))
//                            {
//                                listLand_old = dicOID_newLand_合併[mdlLand_possible.新OID];
//                                listLand_old.Add(mdlLand_possible);
//                            }
//                            else
//                            {
//                                listLand_old = new List<mdlLand>();
//                                listLand_old.Add(mdlLand_possible);
//                                dicOID_newLand_合併.Add(mdlLand_possible.新OID, listLand_old);
//                            }
//                        }
//                    }

//                    LandUpdateManager._listPossibleFound_Land_old.Clear();

//                    if (LandUpdateManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        //strCompleOrCancal = "取消";
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地籍更新" + "取消", "資訊");
//                        return;
//                    }
//                    //#4 比對地籍分割
//                    LandUpdateManager.check_sum_area_分割(dicOID_newLand_分割, featureLayer_result);

//                    if (LandUpdateManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        //strCompleOrCancal = "取消";
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地籍更新" + "取消", "資訊");
//                        return;
//                    }
//                    //#3 比對地籍合併
//                    LandUpdateManager.check_sum_area_合併(dicOID_newLand_合併, featureLayer_result);

//                    //#3 #4 檢查分割或合併之前後面積和是否等於 end /////////////////

//                    dicOID_newLand_分割.Clear();
//                    dicOID_newLand_合併.Clear();

//                    //#5 比對地籍{合併後分割}或{分割後合併}

//                    //找出目前未找到原圖層圖徵ObjectID清單
//                    List<long> list_notFound_to_dissolve = new List<long>();

//                    foreach (long lngOID_sel_ini in list_OID_sel_ini)//原圖層圖徵ObjectID清單
//                    {
//                        if (!LandUpdateManager._listFound_OID_old.Contains(lngOID_sel_ini))//找到的清單中未包含這個ObjectID
//                        {
//                            list_notFound_to_dissolve.Add(lngOID_sel_ini);
//                        }
//                    }

//                    if (list_notFound_to_dissolve.Count > 0)
//                    {
//                        if (LandUpdateManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                        {
//                            //strCompleOrCancal = "取消";
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地籍更新" + "取消", "資訊");
//                            return;
//                        }

//                        try
//                        {
//                            LandUpdateManager.dissolve_not_found_then_search_center_in_old_new(featureLayer_result, list_notFound_to_dissolve);
//                        }
//                        catch (Exception ex)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("比對地籍{合併後分割}或{分割後合併}發生錯誤: " + ex.Message);
//                            return;
//                        }
//                    }

                    
//                    //#5 比對地籍{合併後分割}或{分割後合併} end

//                    //記錄最後 not found
//                    foreach (long lngOID_found_old in LandUpdateManager._listFound_OID_old)
//                    {
//                        if (list_OID_sel_ini.Contains(lngOID_found_old))
//                        {
//                            list_OID_sel_ini.Remove(lngOID_found_old);
//                        }
//                    }

//                    string strOID_NotFounds = string.Empty;

//                    foreach (long lngOIDNotFond in list_OID_sel_ini)
//                    {
//                        strOID_NotFounds += lngOIDNotFond + ",";
//                    }

//                    if (strOID_NotFounds.EndsWith(","))
//                    {
//                        strOID_NotFounds = strOID_NotFounds.Substring(0, strOID_NotFounds.Length - 1);
//                    }

//                    strOID_NotFounds = strOID_NotFounds.Trim();

//                    if (strOID_NotFounds.Length > 0)
//                    {
//                        //select not found 
//                        QueryFilter pQF_old_not_found = new QueryFilter();
//                        pQF_old_not_found.WhereClause = LandUpdateManager._strFieldName_OID_old + " in (" + strOID_NotFounds + ")";

//                        LandUpdateManager._featurelayer_old.Select(pQF_old_not_found);

//                        //field calculate-lu_found
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        string strExp = "'" + "false" + "'";
//                        string strCodeBlock = string.Empty;
//                        GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_found, strExp, "Arcade", strCodeBlock,
//                    list_output_GpResult,
//                    list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + LandUpdateManager._featurelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_found + " exp:" + strExp + " code block:" + strCodeBlock);
//                                return;
//                            }
//                        }
//                    }//    if (strOID_NotFounds.Length > 0)

//                    //恢復一開始選取的圖徵
//                    if (blNoSelect_old == true)
//                    {
//                        LandUpdateManager._featurelayer_old.ClearSelection();

//                    }
//                    else
//                    {
//                        LandUpdateManager._featurelayer_old.Select(pQf_old_sel_ini);
//                    }


//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("運算完成.");

//                    //}
//                    //catch (Exception exc)
//                    //{
//                    //    // Catch any exception found and display in a message box
//                    //    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
//                    //    return;
//                    //}
//                }, LandUpdateManager._CancelableProgressorSource.Progressor);

//            }//try
//            catch (Exception exc)
//            {
//                // Catch any exception found and display in a message box
//                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
//                return;
//            }
//        }
//    }
//}
