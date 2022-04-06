//using ArcGIS.Desktop.Mapping;
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
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetViewPictureProcessorAddin
//{
//    public class LandUpdateFunctiions
//    {
//        public static void search_14code_area_same_段號相同_batch(FeatureLayer aFeaturelayer_old,
//                                                                FeatureLayer aFeaturelayer_new,
//                                                                FeatureLayer aFeaturelayer_result)
//        //out string outStr_log, out bool outBlFound)
//        {
//            //outStr_log = string.Empty;
//            //outBlFound = false;

//            //LandUpdateManager._featurelayer_old.ClearSelection();
//            //LandUpdateManager._featurelayer_new.ClearSelection();

//            List<IGPResult> list_output_GpResult = new List<IGPResult>();
//            List<string> list_output_ErrorMsg = new List<string>();

//            //add join 
//            GpTools.AddJoin(aFeaturelayer_old,
//                            LandUpdateManager._strFieldName_段號,// "OID",//"OBJECTID",// @"D:\COA\地圖服務\arcgis pro project\rasterize\rasterize.gdb" + @"\twd97\merge_test_dis",
//                            aFeaturelayer_new,
//                            LandUpdateManager._strFieldName_段號,//OID
//                            list_output_GpResult,
//                            list_output_ErrorMsg);

//            if (list_output_GpResult.Count == 1)
//            {
//                IGPResult gpResult = list_output_GpResult[0];

//                if (gpResult.IsFailed == true)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("add join 發生錯誤: " + gpResult.ErrorCode.ToString());
//                    return;
//                }
//            }

//            //to doadd index:檢查段號欄位是否有add index， 否則add index

//            //calculate field:面積比
//            list_output_GpResult = new List<IGPResult>();
//            list_output_ErrorMsg = new List<string>();

//            //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("old name: " + aFeaturelayer_old.GetFeatureClass().GetFeatureDataset().GetName());

//            string strExp = "abs((!" + aFeaturelayer_old.Name + "." + LandUpdateManager._strFieldName_登記面積 + "! -" + "!" + aFeaturelayer_new.Name + "." + LandUpdateManager._strFieldName_登記面積 + "!" +
//                                ") / !" + aFeaturelayer_new.Name + "." + LandUpdateManager._strFieldName_登記面積 + "!)";
//            string strCodeBlock = string.Empty;

//            string strFieldName_lu_ar_rt_withLayerName_old = aFeaturelayer_old.Name + "." + LandUpdateManager._strFieldName_lu_ar_rt;
//            string strFieldName_newOID_withLayerName_old = aFeaturelayer_new.Name + "." + LandUpdateManager._strFieldName_OID_new;//join 後在舊版中的新版地籍ObjectId欄位名稱

//            GpTools.CalculateField(aFeaturelayer_old, strFieldName_lu_ar_rt_withLayerName_old, strExp, "Python 3", strCodeBlock,
//                                  list_output_GpResult,
//                                  list_output_ErrorMsg);

//            if (list_output_GpResult.Count == 1)
//            {
//                IGPResult gpResult = list_output_GpResult[0];

//                if (gpResult.IsFailed == true)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("面積比欄位 calculate field 發生錯誤: " + gpResult.ErrorCode.ToString() + aFeaturelayer_old.Name + " field:" + strFieldName_lu_ar_rt_withLayerName_old + " exp:" + strExp + " code block:" + strCodeBlock);
//                    return;
//                }
//            }

//            //select old layer 面積比 <= 1% 
//            QueryFilter pQueryFilter = new QueryFilter();
//            pQueryFilter.WhereClause = strFieldName_lu_ar_rt_withLayerName_old + "<=" + "0";//段號相同面積需完全相同 // LandUpdateManager._dblAreaTolerance;

//            aFeaturelayer_old.Select(pQueryFilter,SelectionCombinationMethod.And);

//            //calculate field:面積比 <= 1% -->lu_type=1 (old layer)
//            list_output_GpResult = new List<IGPResult>();
//            list_output_ErrorMsg = new List<string>();

//            strExp = "1";
//            strCodeBlock = string.Empty;
//            GpTools.CalculateField(aFeaturelayer_old, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//            list_output_GpResult,
//            list_output_ErrorMsg);

//            if (list_output_GpResult.Count == 1)
//            {
//                IGPResult gpResult = list_output_GpResult[0];

//                if (gpResult.IsFailed == true)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeaturelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                    return;
//                }
//            }

//            //get 面積比 <= 1%  new OID
//            TableDefinition tableDefinition_old = LandUpdateManager._featurelayer_old.GetTable().GetDefinition();

//            int intFldIdx_newOID_oldLayer = tableDefinition_old.FindField(strFieldName_newOID_withLayerName_old);

//            string str_newOIDs_sel = string.Empty;

//            using (RowCursor rowCursor = aFeaturelayer_old.GetSelection().Search(null))//(Feature)rowCursor.Current)  //處理每一筆舊版地籍
//            {
//                while (rowCursor.MoveNext())
//                {
//                    using (Feature pFeature_old_sel = (Feature)rowCursor.Current)  //處理每一筆舊版地籍
//                    {
//                        object obj_newOID = pFeature_old_sel[intFldIdx_newOID_oldLayer];
//                        string str_newOID = null;
//                        long lng_newOID = -1;

//                        if (obj_newOID != null)
//                        {
//                            str_newOID = obj_newOID.ToString().Trim();

//                            bool blParseOk = long.TryParse(str_newOID, out lng_newOID);

//                            str_newOIDs_sel += lng_newOID + ",";
//                        }
//                    }
//                }
//            }

//            if (str_newOIDs_sel.EndsWith(","))
//            {
//                str_newOIDs_sel = str_newOIDs_sel.Substring(0, str_newOIDs_sel.Length - 1);
//            }

//            if (str_newOIDs_sel.Length > 0)
//            {
//                //select new layer 面積比 <= 1%  new OID
//                pQueryFilter.WhereClause = LandUpdateManager._strFieldName_OID_new + " in(" + str_newOIDs_sel + ")";

//                aFeaturelayer_new.Select(pQueryFilter);


//                //calculate field:lu_log、lu_type (result layer)            
//                //append 新版圖徵加到結果圖層
//                List<object> listFeatureLayer_ToAppend = new List<object>();
//                listFeatureLayer_ToAppend.Add(aFeaturelayer_new);

//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                GpTools.Append(listFeatureLayer_ToAppend, aFeaturelayer_result, list_output_GpResult, list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                        return;
//                    }
//                }

//                //LandUpdateManager._listFound_OID_new.Add(pFeature_new.GetObjectID());

//                //select lu_log is null
//                QueryFilter pQF_new_lu_log = new QueryFilter();
//                pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//                aFeaturelayer_result.Select(pQF_new_lu_log);

//                //field calculate-lu_log
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                string str_log = "找到段號相同;登記面積變化 <= " + 0 * 100 + " %"; //段號相同面積需完全相同 //+ LandUpdateManager._dblAreaTolerance * 100 + " %";
//                strExp = "'" + str_log + "'";
//                strCodeBlock = string.Empty;

//                GpTools.CalculateField(aFeaturelayer_result, LandUpdateManager._strFieldName_lu_log, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeaturelayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_log + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return;
//                    }
//                }

//                //field calculate-lu_type
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                strExp = "1";
//                strCodeBlock = string.Empty;
//                GpTools.CalculateField(aFeaturelayer_result, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeaturelayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return;
//                    }
//                }
//            }//    if (str_newOIDs_sel.Length > 0)

//            //remove join
//            list_output_GpResult = new List<IGPResult>();
//            list_output_ErrorMsg = new List<string>();

//            GpTools.RemoveJoin(aFeaturelayer_old,
//                               aFeaturelayer_new,
//                               list_output_GpResult,
//                               list_output_ErrorMsg);

//            if (list_output_GpResult.Count == 1)
//            {
//                IGPResult gpResult = list_output_GpResult[0];

//                if (gpResult.IsFailed == true)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("RemoveJoin處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                    return;
//                }
//            }


//        }

//    }
//}
