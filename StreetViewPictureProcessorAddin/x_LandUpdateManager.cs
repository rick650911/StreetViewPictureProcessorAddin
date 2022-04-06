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
//    public class LandUpdateManager
//    {

//        public static FeatureLayer _featurelayer_old;
//        public static FeatureLayer _featurelayer_new;

//        public static string _strFieldName_OID_old = "OBJECTID";
//        public static string _strFieldName_OID_new = "OBJECTID";


//        public static string _strFieldName_登記面積 = "AA10";//"登記面積_平方公尺";
//        public static string _strFieldName_段號 = "段號";

//        public static int _intFldIdx_段號_old;
//        public static int _intFldIdx_段號_new;

//        public static int _intFldIdx_登記面積_old;
//        public static int _intFldIdx_登記面積_new;

//        //public static int _intFldIdx_lu_log_old;
//        //public static int _intFldIdx_lu_log_new;
//        public static int _intFldIdx_lu_found_old;

//        public static int _intFldIdx_lu_type_old;
//        public static int _intFldIdx_lu_type_new;

//        public static int _intFldIdx_lu_ar_rt_old;

//        public static string _strFieldName_lu_found = "lu_found";
//        public static string _strFieldName_lu_log = "lu_log";
//        public static string _strFieldName_lu_type = "lu_type";
//        public static string _strFieldName_原段號 = "原段號";
//        public static string _strFieldName_lu_ar_rt = "lu_ar_rt";

//        public static double _dblAreaTolerance = 0.01;
//        public static string _strAreaTolerance = "0.01";

//        public static double _dblAreaTolerance_分割_合併 = 0.01;
//        public static string _strAreaTolerance_分割_合併 = "0.01";

//        public static double _dblBufferSearchTolerance = 0.1;
//        public static string _strBufferSearchTolerance = "0.1";

//        public static double _dblBufferSearchDistance = 5;
//        public static string _strBufferSearchDistance = "5";

//        public static List<long> _listFound_OID_old = new List<long>();
//        //public static List<long> _listFound_OID_new = new List<long>();

//        //public static List<mdlLand> _listFound_Land_old = new List<mdlLand>();
//        public static List<mdlLand> _listPossibleFound_Land_old = new List<mdlLand>();

//        //public static Dictionary<string, mdlLand> _dicPossibleFound_Land_old_合併 = new Dictionary<string, mdlLand>();
//        //public static Dictionary<string, mdlLand> _dicPossibleFound_Land_old_分割 = new Dictionary<string, mdlLand>();

//        public static List<string> _listPossibleFound_Land_old_合併 = new List<string>();
//        public static List<string> _listPossibleFound_Land_old_分割 = new List<string>();

//        //public static List<long> _listNotFound_OID_old = new List<long>();

//        public static ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource _CancelableProgressorSource = null;

//        /// <summary>
//        /// #1 修正地籍圖版次因接合對位造成偏移差異
//        /// </summary>
//        /// <param name="aFeature_old"></param>
//        /// <param name="aFeatureLayer_result"></param>
//        /// <param name="outStr_log"></param>
//        /// <param name="outBlFound"></param>
//        //public static void search_14code_area_same_段號相同(Feature aFeature_old, FeatureLayer aFeatureLayer_result, out string outStr_log, out bool outBlFound)
//        //{
//        //    outStr_log = string.Empty;
//        //    outBlFound = false;

//        //    LandUpdateManager._featurelayer_old.ClearSelection();
//        //    LandUpdateManager._featurelayer_new.ClearSelection();

//        //    List<IGPResult> list_output_GpResult = new List<IGPResult>();
//        //    List<string> list_output_ErrorMsg = new List<string>();

//        //    object obj_段號_old = aFeature_old[_intFldIdx_段號_old];
//        //    string str_段號_old = null;

//        //    if (obj_段號_old == null)
//        //    {
//        //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("原段號為空值");
//        //        return;
//        //    }

//        //    str_段號_old = obj_段號_old.ToString().Trim();

//        //    object obj_登記面積_old = aFeature_old[_intFldIdx_登記面積_old];
//        //    string str_登記面積_old = null;
//        //    double dbl_登記面積_old = -1;

//        //    if (obj_登記面積_old != null)
//        //    {
//        //        str_登記面積_old = obj_登記面積_old.ToString().Trim();
//        //        bool blParseOk_old = double.TryParse(str_登記面積_old, out dbl_登記面積_old);
//        //    }

//        //    if (str_段號_old != null && str_段號_old.Length > 0)
//        //    {
//        //        QueryFilter pQF_new = new QueryFilter();
//        //        pQF_new.WhereClause = LandUpdateManager._strFieldName_段號 + "='" + str_段號_old + "'";

//        //        using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.Search(pQF_new))
//        //        {
//        //            while (rowCursor_new.MoveNext())
//        //            {
//        //                using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍
//        //                {
//        //                    bool blFound = false;

//        //                    object obj_登記面積_new = pFeature_new[_intFldIdx_登記面積_new];
//        //                    string str_登記面積_new = null;
//        //                    double dbl_登記面積_new = -1;

//        //                    object obj_段號_new = pFeature_new[_intFldIdx_段號_new];
//        //                    string str_段號_new = null;

//        //                    if (obj_段號_new == null)
//        //                    {
//        //                        continue;//處理下一筆
//        //                    }
//        //                    str_段號_new = obj_段號_new.ToString().Trim();

//        //                    outStr_log = "找到段號相同";

//        //                    if (obj_登記面積_new != null)
//        //                    {
//        //                        str_登記面積_new = obj_登記面積_new.ToString().Trim();
//        //                        bool blParseOk_new = double.TryParse(str_登記面積_new, out dbl_登記面積_new);

//        //                        if (dbl_登記面積_old != -1 && dbl_登記面積_new != -1)
//        //                        {
//        //                            double dblDiff_登記面積 = Math.Abs(dbl_登記面積_old - dbl_登記面積_new);
//        //                            double dblDiff_Ratio = dblDiff_登記面積 / dbl_登記面積_old;

//        //                            if (dblDiff_登記面積 == 0 || dblDiff_Ratio <= _dblAreaTolerance)//面積不變
//        //                            {
//        //                                outStr_log += ";登記面積變化 <= " + LandUpdateManager._dblAreaTolerance * 100 + " %";
//        //                                blFound = true;
//        //                                outBlFound = true;

//        //                                LandUpdateManager._listFound_OID_old.Add(aFeature_old.GetObjectID());

//        //                                mdlLand pmdlLand = new mdlLand();

//        //                                pmdlLand.原OID = aFeature_old.GetObjectID();
//        //                                pmdlLand.原登記面積 = dbl_登記面積_old;
//        //                                pmdlLand.原段號 = str_段號_old;

//        //                                pmdlLand.新OID = pFeature_new.GetObjectID();
//        //                                pmdlLand.新登記面積 = dbl_登記面積_new;
//        //                                pmdlLand.新段號 = str_段號_new;
//        //                                pmdlLand.新版地籍狀態 = enumTypeUpdateStatus.段號_登記面積一致;

//        //                                //LandUpdateManager._listFound_Land_old.Add(pmdlLand);

//        //                            }
//        //                            else if (dblDiff_Ratio == double.NaN)
//        //                            {
//        //                                outStr_log += ";比對登記面積失敗";
//        //                            }
//        //                            else//面積改變
//        //                            {
//        //                                outStr_log += ";登記面積變化 > " + LandUpdateManager._dblAreaTolerance * 100 + " %";
//        //                            }
//        //                        }
//        //                        else
//        //                        {
//        //                            outStr_log += ";比對登記面積失敗";
//        //                        }
//        //                    }
//        //                    else
//        //                    {
//        //                        outStr_log += ";比對登記面積失敗";
//        //                    }


//        //                    if (!blFound)
//        //                    {
//        //                        continue;//處理下一筆
//        //                    }

//        //                    //有比對成功，把新版圖徵加到結果圖層
//        //                    //append
//        //                    QueryFilter pQF_new_sel = new QueryFilter();
//        //                    pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + "=" + pFeature_new.GetObjectID().ToString();

//        //                    LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//        //                    List<object> listFeatureLayer_ToAppend = new List<object>();
//        //                    listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//        //                    list_output_GpResult = new List<IGPResult>();
//        //                    list_output_ErrorMsg = new List<string>();

//        //                    GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//        //                    if (list_output_GpResult.Count == 1)
//        //                    {
//        //                        IGPResult gpResult = list_output_GpResult[0];

//        //                        if (gpResult.IsFailed == true)
//        //                        {
//        //                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//        //                            return;
//        //                        }
//        //                    }

//        //                    LandUpdateManager._listFound_OID_new.Add(pFeature_new.GetObjectID());

//        //                    //select lu_log is null
//        //                    QueryFilter pQF_new_lu_log = new QueryFilter();
//        //                    pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//        //                    aFeatureLayer_result.Select(pQF_new_lu_log);

//        //                    //field calculate-lu_log
//        //                    list_output_GpResult = new List<IGPResult>();
//        //                    list_output_ErrorMsg = new List<string>();

//        //                    string strExp = "'" + outStr_log + "'";
//        //                    string strCodeBlock = string.Empty;
//        //                    GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, "Arcade", strCodeBlock,
//        //                    list_output_GpResult,
//        //                    list_output_ErrorMsg);

//        //                    if (list_output_GpResult.Count == 1)
//        //                    {
//        //                        IGPResult gpResult = list_output_GpResult[0];

//        //                        if (gpResult.IsFailed == true)
//        //                        {
//        //                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//        //                            return;
//        //                        }
//        //                    }

//        //                    //field calculate-lu_type
//        //                    list_output_GpResult = new List<IGPResult>();
//        //                    list_output_ErrorMsg = new List<string>();

//        //                    strExp = "1";
//        //                    strCodeBlock = string.Empty;
//        //                    GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//        //                    list_output_GpResult,
//        //                    list_output_ErrorMsg);

//        //                    if (list_output_GpResult.Count == 1)
//        //                    {
//        //                        IGPResult gpResult = list_output_GpResult[0];

//        //                        if (gpResult.IsFailed == true)
//        //                        {
//        //                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//        //                            return;
//        //                        }
//        //                    }

//        //                    //field calculate-lu_type --> old layer
//        //                    //選取舊版圖層中要找的圖徵
//        //                    QueryFilter pQF_old = new QueryFilter();
//        //                    pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + "=" + aFeature_old.GetObjectID();

//        //                    LandUpdateManager._featurelayer_old.Select(pQF_old);

//        //                    list_output_GpResult = new List<IGPResult>();
//        //                    list_output_ErrorMsg = new List<string>();

//        //                    //strExp = "1";
//        //                    //strCodeBlock = string.Empty;
//        //                    GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//        //                    list_output_GpResult,
//        //                    list_output_ErrorMsg);

//        //                    if (list_output_GpResult.Count == 1)
//        //                    {
//        //                        IGPResult gpResult = list_output_GpResult[0];

//        //                        if (gpResult.IsFailed == true)
//        //                        {
//        //                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//        //                            return;
//        //                        }
//        //                    }

//        //                    //field calculate-原段號
//        //                    list_output_GpResult = new List<IGPResult>();
//        //                    list_output_ErrorMsg = new List<string>();

//        //                    strExp = "'" + str_段號_old + "'";
//        //                    strCodeBlock = string.Empty;
//        //                    GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, "Arcade", strCodeBlock,
//        //                    list_output_GpResult,
//        //                    list_output_ErrorMsg);

//        //                    if (list_output_GpResult.Count == 1)
//        //                    {
//        //                        IGPResult gpResult = list_output_GpResult[0];

//        //                        if (gpResult.IsFailed == true)
//        //                        {
//        //                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//        //                            return;
//        //                        }
//        //                    }

//        //                }//  using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍
//        //            }// while (rowCursor_new.MoveNext())
//        //        }// using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.Search(pQF_new))
//        //    }//if (str_段號_old != null && str_段號_old.Length > 0)
//        //}

//        /// <summary>
//        /// #2 比對因土地重劃之段號變更
//        /// </summary>
//        /// <param name="aFeature_old"></param>
//        /// <param name="aFeatureLayer_result"></param>
//        /// <param name="outStr_log"></param>
//        /// <param name="outBlFound"></param>
//        public static void search_new_center_in_area_same_重劃(Feature aFeature_old, FeatureLayer aFeatureLayer_result, out string outStr_log, out bool outBlFound)
//        {
//            outStr_log = string.Empty;
//            outBlFound = false;

//            LandUpdateManager._featurelayer_old.ClearSelection();
//            LandUpdateManager._featurelayer_new.ClearSelection();

//            List<IGPResult> list_output_GpResult = new List<IGPResult>();
//            List<string> list_output_ErrorMsg = new List<string>();

//            object obj_段號_old = aFeature_old[_intFldIdx_段號_old];
//            string str_段號_old = null;

//            if (obj_段號_old == null)
//            {
//                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("原段號為空值");
//                return;
//            }

//            str_段號_old = obj_段號_old.ToString().Trim();

//            object obj_登記面積_old = aFeature_old[_intFldIdx_登記面積_old];
//            string str_登記面積_old = null;
//            double dbl_登記面積_old = -1;

//            if (obj_登記面積_old != null)
//            {
//                str_登記面積_old = obj_登記面積_old.ToString().Trim();
//                bool blParseOk_old = double.TryParse(str_登記面積_old, out dbl_登記面積_old);
//            }

//            if (dbl_登記面積_old == 0 || dbl_登記面積_old == -1)
//            {
//                outStr_log += ";比對登記面積失敗(原地籍登記面積有誤)";
//                return;
//            }

//            //if (str_段號_old != null && str_段號_old.Length > 0)
//            //{
//            //SpatialQueryFilter pQF_new = new SpatialQueryFilter();
//            //pQF_new.FilterGeometry = aFeature_old.GetShape().;
//            //pQF_new.SpatialRelationship = SpatialRelationship.ha;
//            //    pQF_new.WhereClause = LandUpdateManager._strFieldName_段號 + "='" + str_段號_old + "'";

//            //選取舊版圖層中要找的圖徵
//            QueryFilter pQF_old = new QueryFilter();
//            pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + "=" + aFeature_old.GetObjectID();

//            LandUpdateManager._featurelayer_old.Select(pQF_old);

//            //依舊版圖徵選取新版圖徵(新版center in舊版)
//            list_output_GpResult = new List<IGPResult>();
//            list_output_ErrorMsg = new List<string>();

//            GpTools.SelectLayerByLocation(LandUpdateManager._featurelayer_new,
//                                     LandUpdateManager._featurelayer_old,
//                                     "HAVE_THEIR_CENTER_IN",
//                                     "0 Meters",
//                                     list_output_GpResult,
//                                     list_output_ErrorMsg);

//            if (list_output_GpResult.Count == 1)
//            {
//                IGPResult gpResult = list_output_GpResult[0];

//                if (gpResult.IsFailed == true)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("select by location (新版圖資)發生錯誤: " + gpResult.ErrorCode.ToString());
//                    return;
//                }
//            }

//            using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.GetSelection().Search(null, false))
//            {
//                while (rowCursor_new.MoveNext())
//                {
//                    using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍(每個新版center in舊版圖徵)
//                    {
//                        bool blFound = false;

//                        object obj_登記面積_new = pFeature_new[_intFldIdx_登記面積_new];
//                        string str_登記面積_new = null;
//                        double dbl_登記面積_new = -1;

//                        object obj_段號_new = pFeature_new[_intFldIdx_段號_new];
//                        string str_段號_new = null;

//                        if (obj_段號_new == null)
//                        {
//                            continue;//處理下一筆
//                        }

//                        str_段號_new = obj_段號_new.ToString().Trim();

//                        outStr_log = "找到 center in";

//                        if (obj_登記面積_new != null)
//                        {
//                            str_登記面積_new = obj_登記面積_new.ToString().Trim();
//                            bool blParseOk_new = double.TryParse(str_登記面積_new, out dbl_登記面積_new);

//                            if (dbl_登記面積_old != -1 && dbl_登記面積_new != -1)
//                            {
//                                double dblDiff_登記面積 = Math.Abs(dbl_登記面積_old - dbl_登記面積_new);
//                                double dblDiff_Ratio = dblDiff_登記面積 / dbl_登記面積_old;

//                                if (dblDiff_Ratio <= _dblAreaTolerance)//面積不變
//                                {
//                                    outStr_log += "(重劃);登記面積變化 <= " + LandUpdateManager._dblAreaTolerance * 100 + " %";
//                                    blFound = true;
//                                    outBlFound = true;

//                                    LandUpdateManager._listFound_OID_old.Add(aFeature_old.GetObjectID());

//                                    mdlLand pmdlLand = new mdlLand();

//                                    pmdlLand.原OID = aFeature_old.GetObjectID();
//                                    pmdlLand.原登記面積 = dbl_登記面積_old;
//                                    pmdlLand.原段號 = str_段號_old;

//                                    pmdlLand.新OID = pFeature_new.GetObjectID();
//                                    pmdlLand.新登記面積 = dbl_登記面積_new;
//                                    pmdlLand.新段號 = str_段號_new;
//                                    pmdlLand.新版地籍狀態 = enumTypeUpdateStatus.重劃;

//                                    //LandUpdateManager._listFound_Land_old.Add(pmdlLand);
//                                }
//                                else //面積改變
//                                {
//                                    outStr_log += ";登記面積變化 > " + LandUpdateManager._dblAreaTolerance * 100 + " %";
//                                }
//                            }
//                            else
//                            {
//                                outStr_log += ";比對登記面積失敗";
//                            }
//                        }
//                        else
//                        {
//                            outStr_log += ";比對登記面積失敗";
//                        }

//                        if (!blFound)
//                        {
//                            continue;
//                        }

//                        //有比對成功，把新版圖徵加到結果圖層
//                        //append
//                        QueryFilter pQF_new_sel = new QueryFilter();
//                        pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + "=" + pFeature_new.GetObjectID().ToString();

//                        LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//                        List<object> listFeatureLayer_ToAppend = new List<object>();
//                        listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                                return;
//                            }
//                        }

//                        //LandUpdateManager._listFound_OID_new.Add(pFeature_new.GetObjectID());

//                        //select lu_log is null
//                        QueryFilter pQF_new_lu_log = new QueryFilter();
//                        pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//                        aFeatureLayer_result.Select(pQF_new_lu_log);

//                        //field calculate-lu_log
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        string strExp = "'" + outStr_log + "'";
//                        string strCodeBlock = string.Empty;
//                        GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, "Arcade", strCodeBlock,
//                        list_output_GpResult,
//                        list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_log + " exp:" + strExp + " code block:" + strCodeBlock);
//                                return;
//                            }
//                        }

//                        //field calculate-lu_type
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        strExp = "2";
//                        strCodeBlock = string.Empty;
//                        GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                        list_output_GpResult,
//                        list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                                return;
//                            }
//                        }

//                        //field calculate-lu_type --> old layer
//                        //選取舊版圖層中要找的圖徵
//                        pQF_old = new QueryFilter();
//                        pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + "=" + aFeature_old.GetObjectID();

//                        LandUpdateManager._featurelayer_old.Select(pQF_old);

//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        //strExp = "2";
//                        //strCodeBlock = string.Empty;
//                        GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                        list_output_GpResult,
//                        list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + LandUpdateManager._featurelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                                return;
//                            }
//                        }

//                        //field calculate-原段號
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        strExp = "'" + str_段號_old + "'";
//                        strCodeBlock = string.Empty;
//                        GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, "Arcade", strCodeBlock,
//                        list_output_GpResult,
//                        list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_原段號 + " exp:" + strExp + " code block:" + strCodeBlock);
//                                return;
//                            }
//                        }

//                    }//  using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍
//                }// while (rowCursor_new.MoveNext())
//            }// using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.Search(pQF_new))
//             // }//if (str_段號_old != null && str_段號_old.Length > 0)
//        }

//        /// <summary>
//        /// #4 比對地籍分割 (可能的分割)
//        /// </summary>
//        /// <param name="aFeature_old"></param>
//        /// <param name="aFeatureLayer_result"></param>
//        /// <param name="outStr_log"></param>
//        /// <param name="outBlFound"></param>
//        public static void search_new_center_in_area_samller_possible_分割(Feature aFeature_old, FeatureLayer aFeatureLayer_result, out string outStr_log, out bool outBlFound)
//        {
//            outStr_log = string.Empty;
//            outBlFound = false;

//            LandUpdateManager._featurelayer_old.ClearSelection();
//            LandUpdateManager._featurelayer_new.ClearSelection();

//            List<IGPResult> list_output_GpResult = new List<IGPResult>();
//            List<string> list_output_ErrorMsg = new List<string>();

//            object obj_段號_old = aFeature_old[_intFldIdx_段號_old];
//            string str_段號_old = null;

//            if (obj_段號_old == null)
//            {
//                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("原段號為空值");
//                return;
//            }

//            str_段號_old = obj_段號_old.ToString().Trim();

//            object obj_登記面積_old = aFeature_old[_intFldIdx_登記面積_old];
//            string str_登記面積_old = null;
//            double dbl_登記面積_old = -1;

//            if (obj_登記面積_old != null)
//            {
//                str_登記面積_old = obj_登記面積_old.ToString().Trim();
//                bool blParseOk_old = double.TryParse(str_登記面積_old, out dbl_登記面積_old);
//            }

//            //if (str_段號_old != null && str_段號_old.Length > 0)
//            //{
//            //SpatialQueryFilter pQF_new = new SpatialQueryFilter();
//            //pQF_new.FilterGeometry = aFeature_old.GetShape().;
//            //pQF_new.SpatialRelationship = SpatialRelationship.ha;
//            //    pQF_new.WhereClause = LandUpdateManager._strFieldName_段號 + "='" + str_段號_old + "'";

//            //選取舊版圖層中要找的圖徵
//            QueryFilter pQF_old = new QueryFilter();
//            pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + "=" + aFeature_old.GetObjectID();

//            LandUpdateManager._featurelayer_old.Select(pQF_old);

//            search_new_center_in_larger_old_buffer(aFeature_old, dbl_登記面積_old, str_段號_old, false, out outStr_log, out outBlFound);

//            if (_dblBufferSearchDistance > 0)
//            {
//                search_new_center_in_larger_old_buffer(aFeature_old, dbl_登記面積_old, str_段號_old, true, out outStr_log, out outBlFound);
//            }



//            // }//if (str_段號_old != null && str_段號_old.Length > 0)
//        }

//        public static void search_new_center_in_larger_old_buffer(Feature aFeature_old, double dbl_登記面積_old, string str_段號_old, bool aBlBufferSearch, out string outStr_log, out bool outBlFound)
//        {
//            outStr_log = null;
//            outBlFound = false;

//            string strSearchDistance = "0 Meters";

//            if (aBlBufferSearch)
//            {
//                strSearchDistance = _strBufferSearchDistance + " Meters";
//            }


//            //依舊版圖徵選取新版圖徵(新版center in舊版)
//            List<IGPResult> list_output_GpResult = new List<IGPResult>();
//            List<string> list_output_ErrorMsg = new List<string>();

//            GpTools.SelectLayerByLocation(LandUpdateManager._featurelayer_new,
//                                     LandUpdateManager._featurelayer_old,
//                                     "HAVE_THEIR_CENTER_IN",
//                                     strSearchDistance,
//                                     list_output_GpResult,
//                                     list_output_ErrorMsg);

//            if (list_output_GpResult.Count == 1)
//            {
//                IGPResult gpResult = list_output_GpResult[0];

//                if (gpResult.IsFailed == true)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("select by location (新版圖資)發生錯誤: " + gpResult.ErrorCode.ToString());
//                    return;
//                }
//            }

//            using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.GetSelection().Search(null, false))
//            {
//                while (rowCursor_new.MoveNext())
//                {
//                    using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍(每個新版center in舊版圖徵)
//                    {
//                        bool blFound = false;

//                        object obj_登記面積_new = pFeature_new[_intFldIdx_登記面積_new];
//                        string str_登記面積_new = null;
//                        double dbl_登記面積_new = -1;

//                        object obj_段號_new = pFeature_new[_intFldIdx_段號_new];
//                        string str_段號_new = null;

//                        if (obj_段號_new == null)
//                        {
//                            continue;//處理下一筆
//                        }

//                        str_段號_new = obj_段號_new.ToString().Trim();

//                        outStr_log = "找到 center in";

//                        if (obj_登記面積_new != null)
//                        {
//                            str_登記面積_new = obj_登記面積_new.ToString().Trim();
//                            bool blParseOk_new = double.TryParse(str_登記面積_new, out dbl_登記面積_new);

//                            if (dbl_登記面積_old != -1 && dbl_登記面積_new != -1)
//                            {
//                                //double dblDiff_登記面積 = Math.Abs(dbl_登記面積_old - dbl_登記面積_new);
//                                //double dblDiff_Ratio = dblDiff_登記面積 / dbl_登記面積_old;

//                                //if (dblDiff_Ratio <= 0.01)//面積不變
//                                //{
//                                //    outStr_log += ";登記面積變化 <= 1%";

//                                //}
//                                //else //面積改變
//                                //{
//                                if (dbl_登記面積_new < dbl_登記面積_old)  //面積變小
//                                {
//                                    outStr_log += "(分割);登記面積變小";

//                                    blFound = true;
//                                    outBlFound = true;

//                                    //LandUpdateManager._listFound_OID_old.Add(aFeature_old.GetObjectID());

//                                    mdlLand pmdlLand = new mdlLand();

//                                    pmdlLand.原OID = aFeature_old.GetObjectID();
//                                    pmdlLand.原登記面積 = dbl_登記面積_old;
//                                    pmdlLand.原段號 = str_段號_old;

//                                    pmdlLand.新OID = pFeature_new.GetObjectID();
//                                    pmdlLand.新登記面積 = dbl_登記面積_new;
//                                    pmdlLand.新段號 = str_段號_new;
//                                    pmdlLand.新版地籍狀態 = enumTypeUpdateStatus.分割;

//                                    if (aBlBufferSearch)
//                                    {
//                                        pmdlLand.SearchedByBuffer = true;
//                                    }

//                                    string strOID_old_new = pmdlLand.原OID + "-" + pmdlLand.新OID;

//                                    if (!LandUpdateManager._listPossibleFound_Land_old_分割.Contains(strOID_old_new))
//                                    {
//                                        LandUpdateManager._listPossibleFound_Land_old.Add(pmdlLand);

//                                        LandUpdateManager._listPossibleFound_Land_old_分割.Add(strOID_old_new);
//                                    }
//                                    ////

//                                    //LandUpdateManager._dicPossibleFound_Land_old_分割.Add(pmdlLand.原OID, pmdlLand);
//                                }

//                                //}
//                            }
//                            else
//                            {
//                                outStr_log += ";比對登記面積失敗";
//                            }
//                        }
//                        else
//                        {
//                            outStr_log += ";比對登記面積失敗";
//                        }

//                        if (!blFound)
//                        {
//                            continue;
//                        }

//                        ////有比對成功，把新版圖徵加到結果圖層
//                        ////append
//                        //QueryFilter pQF_new_sel = new QueryFilter();
//                        //pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + "=" + pFeature_new.GetObjectID().ToString();

//                        //LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//                        //List<object> listFeatureLayer_ToAppend = new List<object>();
//                        //listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//                        //list_output_GpResult = new List<IGPResult>();
//                        //list_output_ErrorMsg = new List<string>();

//                        //GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//                        //if (list_output_GpResult.Count == 1)
//                        //{
//                        //    IGPResult gpResult = list_output_GpResult[0];

//                        //    if (gpResult.IsFailed == true)
//                        //    {
//                        //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                        //        return;
//                        //    }
//                        //}

//                        //LandUpdateManager._listFound_OID_new.Add(pFeature_new.GetObjectID());

//                        ////select lu_log is null
//                        //QueryFilter pQF_new_lu_log = new QueryFilter();
//                        //pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//                        //aFeatureLayer_result.Select(pQF_new_lu_log);

//                        ////field calculate-lu_log
//                        //list_output_GpResult = new List<IGPResult>();
//                        //list_output_ErrorMsg = new List<string>();

//                        //string strExp = "'" + outStr_log + "'";
//                        //string strCodeBlock = string.Empty;
//                        //GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, strCodeBlock,
//                        //list_output_GpResult,
//                        //list_output_ErrorMsg);

//                        //if (list_output_GpResult.Count == 1)
//                        //{
//                        //    IGPResult gpResult = list_output_GpResult[0];

//                        //    if (gpResult.IsFailed == true)
//                        //    {
//                        //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                        //        return;
//                        //    }
//                        //}

//                        ////field calculate-原段號
//                        //list_output_GpResult = new List<IGPResult>();
//                        //list_output_ErrorMsg = new List<string>();

//                        //strExp = "'" + str_段號_old + "'";
//                        //strCodeBlock = string.Empty;
//                        //GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, strCodeBlock,
//                        //list_output_GpResult,
//                        //list_output_ErrorMsg);

//                        //if (list_output_GpResult.Count == 1)
//                        //{
//                        //    IGPResult gpResult = list_output_GpResult[0];

//                        //    if (gpResult.IsFailed == true)
//                        //    {
//                        //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                        //        return;
//                        //    }
//                        //}

//                    }//  using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍
//                }// while (rowCursor_new.MoveNext())
//            }// using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.Search(pQF_new))
//        }

//        /// <summary>
//        /// #3 比對地籍合併 (可能的合併)
//        /// </summary>
//        /// <param name="aFeature_old"></param>
//        /// <param name="aFeatureLayer_result"></param>
//        /// <param name="outStr_log"></param>
//        /// <param name="outBlFound"></param>
//        public static void search_old_center_in_area_larger_possible_合併(Feature aFeature_old, FeatureLayer aFeatureLayer_result, out string outStr_log, out bool outBlFound)
//        {
//            outStr_log = string.Empty;
//            outBlFound = false;

//            LandUpdateManager._featurelayer_old.ClearSelection();
//            LandUpdateManager._featurelayer_new.ClearSelection();

//            List<IGPResult> list_output_GpResult = new List<IGPResult>();
//            List<string> list_output_ErrorMsg = new List<string>();

//            object obj_段號_old = aFeature_old[_intFldIdx_段號_old];
//            string str_段號_old = null;

//            if (obj_段號_old == null)
//            {
//                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("原段號為空值");
//                return;
//            }

//            str_段號_old = obj_段號_old.ToString().Trim();

//            object obj_登記面積_old = aFeature_old[_intFldIdx_登記面積_old];
//            string str_登記面積_old = null;
//            double dbl_登記面積_old = -1;

//            if (obj_登記面積_old != null)
//            {
//                str_登記面積_old = obj_登記面積_old.ToString().Trim();
//                bool blParseOk_old = double.TryParse(str_登記面積_old, out dbl_登記面積_old);
//            }

//            //if (str_段號_old != null && str_段號_old.Length > 0)
//            //{
//            //SpatialQueryFilter pQF_new = new SpatialQueryFilter();
//            //pQF_new.FilterGeometry = aFeature_old.GetShape().;
//            //pQF_new.SpatialRelationship = SpatialRelationship.ha;
//            //    pQF_new.WhereClause = LandUpdateManager._strFieldName_段號 + "='" + str_段號_old + "'";

//            //選取舊版圖層中要找的圖徵
//            QueryFilter pQF_old = new QueryFilter();
//            pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + "=" + aFeature_old.GetObjectID();

//            LandUpdateManager._featurelayer_old.Select(pQF_old);

//            //依舊版圖徵選取新版圖徵(新版交集舊版)
//            list_output_GpResult = new List<IGPResult>();
//            list_output_ErrorMsg = new List<string>();

//            GpTools.SelectLayerByLocation(LandUpdateManager._featurelayer_new,
//                                          LandUpdateManager._featurelayer_old,
//                                          "INTERSECT",//"HAVE_THEIR_CENTER_IN",//"INTERSECT",
//                                          "0 Meters",
//                                          list_output_GpResult,
//                                          list_output_ErrorMsg);

//            if (list_output_GpResult.Count == 1)
//            {
//                IGPResult gpResult = list_output_GpResult[0];

//                if (gpResult.IsFailed == true)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("select by location (新版圖資)發生錯誤: " + gpResult.ErrorCode.ToString());
//                    return;
//                }
//            }

//            //記錄面積變大的 new 地籍的 OID
//            List<mdlLand> listLand_new_larger = new List<mdlLand>();

//            using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.GetSelection().Search(null, false))
//            {
//                while (rowCursor_new.MoveNext())
//                {
//                    using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍(每個新版intersect到舊版圖徵)
//                    {
//                        //bool blFound = false;

//                        object obj_登記面積_new = pFeature_new[_intFldIdx_登記面積_new];
//                        string str_登記面積_new = null;
//                        double dbl_登記面積_new = -1;
//                        //outStr_log = str_段號_old + " 找到 center in";

//                        object obj_段號_new = pFeature_new[_intFldIdx_段號_new];
//                        string str_段號_new = null;

//                        if (obj_段號_new == null)
//                        {
//                            continue;//處理下一筆
//                        }

//                        str_段號_new = obj_段號_new.ToString().Trim();

//                        if (obj_登記面積_new != null)
//                        {
//                            str_登記面積_new = obj_登記面積_new.ToString().Trim();
//                            bool blParseOk_new = double.TryParse(str_登記面積_new, out dbl_登記面積_new);

//                            if (dbl_登記面積_old != -1 && dbl_登記面積_new != -1)
//                            {
//                                if (dbl_登記面積_new > dbl_登記面積_old)  //面積變大
//                                {
//                                    //outStr_log += ";登記面積變大(合併)";
//                                    mdlLand mdlLand_new = new mdlLand();
//                                    mdlLand_new.新OID = pFeature_new.GetObjectID();
//                                    mdlLand_new.新登記面積 = dbl_登記面積_new;
//                                    mdlLand_new.新段號 = str_段號_new;

//                                    listLand_new_larger.Add(mdlLand_new);

//                                    //blFound = true;
//                                    //outBlFound = true;
//                                }
//                            }
//                            else
//                            {
//                                //outStr_log += ";比對登記面積失敗";
//                            }
//                        }
//                        else
//                        {
//                            //outStr_log += ";比對登記面積失敗";
//                        }
//                    }//  using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍
//                }// while (rowCursor_new.MoveNext())
//            }// using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.Search(pQF_new))

//            //再看old 那些地center in這些new OID------------------------------------
//            if (listLand_new_larger.Count == 0)
//            {
//                return;
//            }

//            search_old_center_in_larger_new_buffer(listLand_new_larger, aFeature_old, dbl_登記面積_old, str_段號_old, false, out outStr_log, out outBlFound);

//            if (_dblBufferSearchDistance > 0)
//            {
//                search_old_center_in_larger_new_buffer(listLand_new_larger, aFeature_old, dbl_登記面積_old, str_段號_old, true, out outStr_log, out outBlFound);
//            }

//            ////選取新版圖層中要找的圖徵
//            //foreach (mdlLand pmdlLand in listLand_new_larger)
//            //{
//            //    long lng_OID_new = pmdlLand.新OID;
//            //    double dbl_登記面積_new = pmdlLand.新登記面積;
//            //    string str_段號_new = pmdlLand.新段號;

//            //    QueryFilter pQF_new = new QueryFilter();
//            //    pQF_new.WhereClause = LandUpdateManager._strFieldName_OID_new + "= " + lng_OID_new;

//            //    LandUpdateManager._featurelayer_new.Select(pQF_new);

//            //    //依新增版圖徵選取舊版圖徵(舊版center in新版)
//            //    list_output_GpResult = new List<IGPResult>();
//            //    list_output_ErrorMsg = new List<string>();

//            //    GpTools.SelectLayerByLocation(LandUpdateManager._featurelayer_old,
//            //                                 LandUpdateManager._featurelayer_new,
//            //                                 "HAVE_THEIR_CENTER_IN",
//            //                                 "0 Meters",
//            //                                 list_output_GpResult,
//            //                                 list_output_ErrorMsg);

//            //    if (list_output_GpResult.Count == 1)
//            //    {
//            //        IGPResult gpResult = list_output_GpResult[0];

//            //        if (gpResult.IsFailed == true)
//            //        {
//            //            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("select by location (舊版圖資)發生錯誤: " + gpResult.ErrorCode.ToString());
//            //            return;
//            //        }
//            //    }

//            //    using (RowCursor rowCursor_old = LandUpdateManager._featurelayer_old.GetSelection().Search(null, false))
//            //    {
//            //        while (rowCursor_old.MoveNext())
//            //        {
//            //            using (Feature pFeature_old = (Feature)rowCursor_old.Current)  //處理每一筆對應到的舊版地籍(每個舊版center in新版圖徵)
//            //            {
//            //                long lngOID_Old = aFeature_old.GetObjectID();//test

//            //                if (!pFeature_old.GetObjectID().Equals(aFeature_old.GetObjectID()))//只處理input 的地籍，可能有其他是center in其他舊版intersect到的新版地籍
//            //                {
//            //                    continue;
//            //                }

//            //                bool blFound = false;

//            //                object obj_段號_old_re = pFeature_old[_intFldIdx_段號_old];
//            //                string str_段號_old_re = null;

//            //                if (obj_段號_old_re != null)
//            //                {
//            //                    str_段號_old_re = obj_段號_old_re.ToString().Trim();
//            //                }
//            //                else
//            //                {
//            //                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("取段號發生錯誤: ");
//            //                    return;
//            //                }

//            //                object obj_登記面積_old_re = pFeature_old[_intFldIdx_登記面積_old];
//            //                string str_登記面積_old_re = null;
//            //                double dbl_登記面積_old_re = -1;

//            //                outStr_log =  "找到 center in";

//            //                if (obj_登記面積_old_re != null)
//            //                {
//            //                    str_登記面積_old_re = obj_登記面積_old_re.ToString().Trim();
//            //                    bool blParseOk_old_re = double.TryParse(str_登記面積_old_re, out dbl_登記面積_old_re);

//            //                    if (dbl_登記面積_new != -1 && dbl_登記面積_old_re != -1)
//            //                    {
//            //                        //double dblDiff_登記面積 = Math.Abs(dbl_登記面積_old - dbl_登記面積_new);
//            //                        //double dblDiff_Ratio = dblDiff_登記面積 / dbl_登記面積_old;

//            //                        //if (dblDiff_Ratio <= 0.01)//面積不變
//            //                        //{
//            //                        //    outStr_log += ";登記面積變化 <= 1%";

//            //                        //}
//            //                        //else //面積改變
//            //                        //{
//            //                        if (dbl_登記面積_old_re < dbl_登記面積_new)  //面積變大
//            //                        {
//            //                            if (_listFound_OID_old.Contains(pFeature_old.GetObjectID()))//檢查是否在之前的搜索已找到
//            //                            {
//            //                                continue;
//            //                            }

//            //                            outStr_log += "(合併);登記面積變大";

//            //                            blFound = true;
//            //                            outBlFound = true;

//            //                            //LandUpdateManager._listFound_OID_old.Add(pFeature_old.GetObjectID());

//            //                            mdlLand pmdlLand_old = new mdlLand();

//            //                            //object obj_段號_new = pFeature_new[_intFldIdx_段號_new];
//            //                            //string str_段號_new = null;

//            //                            //if (obj_段號_new == null)
//            //                            //{
//            //                            //    continue;//處理下一筆
//            //                            //}

//            //                            //str_段號_new = obj_段號_new.ToString().Trim();

//            //                            pmdlLand.原OID = aFeature_old.GetObjectID();
//            //                            pmdlLand.原登記面積 = dbl_登記面積_old;
//            //                            pmdlLand.原段號 = str_段號_old;

//            //                            pmdlLand.新OID = lng_OID_new;
//            //                            pmdlLand.新登記面積 = dbl_登記面積_new;
//            //                            pmdlLand.新段號 = str_段號_new;
//            //                            pmdlLand.新版地籍狀態 = enumTypeUpdateStatus.合併;

//            //                            LandUpdateManager._listPossibleFound_Land_old.Add(pmdlLand);
//            //                        }

//            //                        //}
//            //                    }
//            //                    else
//            //                    {
//            //                        outStr_log += ";比對登記面積失敗";
//            //                    }
//            //                }
//            //                else
//            //                {
//            //                    outStr_log += ";比對登記面積失敗";
//            //                }

//            //                if (!blFound)
//            //                {
//            //                    continue;
//            //                }

//            //                ////有比對成功，把新版圖徵加到結果圖層
//            //                ////append
//            //                ////QueryFilter pQF_new_sel = new QueryFilter();
//            //                ////pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + "=" + lng_OID_new.ToString();

//            //                ////LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//            //                //List<object> listFeatureLayer_ToAppend = new List<object>();
//            //                //listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//            //                //list_output_GpResult = new List<IGPResult>();
//            //                //list_output_ErrorMsg = new List<string>();

//            //                //GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//            //                //if (list_output_GpResult.Count == 1)
//            //                //{
//            //                //    IGPResult gpResult = list_output_GpResult[0];

//            //                //    if (gpResult.IsFailed == true)
//            //                //    {
//            //                //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//            //                //        return;
//            //                //    }
//            //                //}

//            //                //LandUpdateManager._listFound_OID_new.Add(lng_OID_new);

//            //                ////select lu_log is null
//            //                //QueryFilter pQF_new_lu_log = new QueryFilter();
//            //                //pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//            //                //aFeatureLayer_result.Select(pQF_new_lu_log);

//            //                ////field calculate-lu_log
//            //                //list_output_GpResult = new List<IGPResult>();
//            //                //list_output_ErrorMsg = new List<string>();

//            //                //string strExp = "'" + outStr_log + "'";
//            //                //string strCodeBlock = string.Empty;
//            //                //GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, strCodeBlock,
//            //                //list_output_GpResult,
//            //                //list_output_ErrorMsg);

//            //                //if (list_output_GpResult.Count == 1)
//            //                //{
//            //                //    IGPResult gpResult = list_output_GpResult[0];

//            //                //    if (gpResult.IsFailed == true)
//            //                //    {
//            //                //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//            //                //        return;
//            //                //    }
//            //                //}

//            //                ////field calculate-原段號
//            //                //list_output_GpResult = new List<IGPResult>();
//            //                //list_output_ErrorMsg = new List<string>();

//            //                //strExp = "'" + str_段號_old + "'";
//            //                //strCodeBlock = string.Empty;
//            //                //GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, strCodeBlock,
//            //                //list_output_GpResult,
//            //                //list_output_ErrorMsg);

//            //                //if (list_output_GpResult.Count == 1)
//            //                //{
//            //                //    IGPResult gpResult = list_output_GpResult[0];

//            //                //    if (gpResult.IsFailed == true)
//            //                //    {
//            //                //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//            //                //        return;
//            //                //    }
//            //                //}

//            //            }//  using (Feature pFeature_old = (Feature)rowCursor_old.Current)  //處理每一筆對應到的舊版地籍
//            //        }// while (rowCursor_old.MoveNext())
//            //    }// using (RowCursor rowCursor_old = LandUpdateManager._featurelayer_old.Search(pQF_old))

//            //}//foreach (mdlLand pmdlLand in listLand_new_larger)



//        }

//        public static void search_old_center_in_larger_new_buffer(List<mdlLand> listLand_new_larger, Feature aFeature_old, double dbl_登記面積_old, string str_段號_old, bool aBlBufferSearch, out string outStr_log, out bool outBlFound)
//        {
//            outStr_log = null;
//            outBlFound = false;

//            //選取新版圖層中要找的圖徵
//            foreach (mdlLand pmdlLand in listLand_new_larger)
//            {
//                long lng_OID_new = pmdlLand.新OID;
//                double dbl_登記面積_new = pmdlLand.新登記面積;
//                string str_段號_new = pmdlLand.新段號;

//                QueryFilter pQF_new = new QueryFilter();
//                pQF_new.WhereClause = LandUpdateManager._strFieldName_OID_new + "= " + lng_OID_new;

//                LandUpdateManager._featurelayer_new.Select(pQF_new);

//                //依新增版圖徵選取舊版圖徵(舊版center in新版)
//                List<IGPResult> list_output_GpResult = new List<IGPResult>();
//                List<string> list_output_ErrorMsg = new List<string>();

//                string strSearchDistance = "0 Meters";

//                if (aBlBufferSearch)
//                {
//                    strSearchDistance = _strBufferSearchDistance + " Meters";
//                }

//                GpTools.SelectLayerByLocation(LandUpdateManager._featurelayer_old,
//                                             LandUpdateManager._featurelayer_new,
//                                             "HAVE_THEIR_CENTER_IN",
//                                             strSearchDistance,
//                                             list_output_GpResult,
//                                             list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("select by location (舊版圖資)發生錯誤: " + gpResult.ErrorCode.ToString());
//                        return;
//                    }
//                }

//                using (RowCursor rowCursor_old = LandUpdateManager._featurelayer_old.GetSelection().Search(null, false))
//                {
//                    while (rowCursor_old.MoveNext())
//                    {
//                        using (Feature pFeature_old = (Feature)rowCursor_old.Current)  //處理每一筆對應到的舊版地籍(每個舊版center in新版圖徵)
//                        {
//                            long lngOID_Old = aFeature_old.GetObjectID();//test

//                            if (!pFeature_old.GetObjectID().Equals(aFeature_old.GetObjectID()))//只處理input 的地籍，可能有其他是center in其他舊版intersect到的新版地籍
//                            {
//                                continue;
//                            }

//                            bool blFound = false;

//                            object obj_段號_old_re = pFeature_old[_intFldIdx_段號_old];
//                            string str_段號_old_re = null;

//                            if (obj_段號_old_re != null)
//                            {
//                                str_段號_old_re = obj_段號_old_re.ToString().Trim();
//                            }
//                            else
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("取段號發生錯誤: ");
//                                return;
//                            }

//                            object obj_登記面積_old_re = pFeature_old[_intFldIdx_登記面積_old];
//                            string str_登記面積_old_re = null;
//                            double dbl_登記面積_old_re = -1;

//                            outStr_log = "找到 center in";

//                            if (obj_登記面積_old_re != null)
//                            {
//                                str_登記面積_old_re = obj_登記面積_old_re.ToString().Trim();
//                                bool blParseOk_old_re = double.TryParse(str_登記面積_old_re, out dbl_登記面積_old_re);

//                                if (dbl_登記面積_new != -1 && dbl_登記面積_old_re != -1)
//                                {
//                                    //double dblDiff_登記面積 = Math.Abs(dbl_登記面積_old - dbl_登記面積_new);
//                                    //double dblDiff_Ratio = dblDiff_登記面積 / dbl_登記面積_old;

//                                    //if (dblDiff_Ratio <= 0.01)//面積不變
//                                    //{
//                                    //    outStr_log += ";登記面積變化 <= 1%";

//                                    //}
//                                    //else //面積改變
//                                    //{
//                                    if (dbl_登記面積_old_re < dbl_登記面積_new)  //面積變大
//                                    {

//                                        if (_listFound_OID_old.Contains(pFeature_old.GetObjectID()))//檢查是否在之前的搜索已找到
//                                        {
//                                            continue;
//                                        }

//                                        outStr_log += "(合併);登記面積變大";

//                                        blFound = true;
//                                        outBlFound = true;

//                                        //LandUpdateManager._listFound_OID_old.Add(pFeature_old.GetObjectID());

//                                        mdlLand pmdlLand_old = new mdlLand();

//                                        //object obj_段號_new = pFeature_new[_intFldIdx_段號_new];
//                                        //string str_段號_new = null;

//                                        //if (obj_段號_new == null)
//                                        //{
//                                        //    continue;//處理下一筆
//                                        //}

//                                        //str_段號_new = obj_段號_new.ToString().Trim();

//                                        pmdlLand_old.原OID = aFeature_old.GetObjectID();
//                                        pmdlLand_old.原登記面積 = dbl_登記面積_old;
//                                        pmdlLand_old.原段號 = str_段號_old;

//                                        pmdlLand_old.新OID = lng_OID_new;
//                                        pmdlLand_old.新登記面積 = dbl_登記面積_new;
//                                        pmdlLand_old.新段號 = str_段號_new;
//                                        pmdlLand_old.新版地籍狀態 = enumTypeUpdateStatus.合併;

//                                        if (aBlBufferSearch)
//                                        {
//                                            pmdlLand_old.SearchedByBuffer = true;
//                                        }

//                                        string strOID_old_new = pmdlLand_old.原OID + "-" + pmdlLand_old.新OID;

//                                        if (!LandUpdateManager._listPossibleFound_Land_old_合併.Contains(strOID_old_new))
//                                        {
//                                            LandUpdateManager._listPossibleFound_Land_old.Add(pmdlLand_old);

//                                            LandUpdateManager._listPossibleFound_Land_old_合併.Add(strOID_old_new);
//                                        }


//                                    }

//                                    //}
//                                }
//                                else
//                                {
//                                    outStr_log += ";比對登記面積失敗";
//                                }
//                            }
//                            else
//                            {
//                                outStr_log += ";比對登記面積失敗";
//                            }

//                            if (!blFound)
//                            {
//                                continue;
//                            }

//                            ////有比對成功，把新版圖徵加到結果圖層
//                            ////append
//                            ////QueryFilter pQF_new_sel = new QueryFilter();
//                            ////pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + "=" + lng_OID_new.ToString();

//                            ////LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//                            //List<object> listFeatureLayer_ToAppend = new List<object>();
//                            //listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//                            //list_output_GpResult = new List<IGPResult>();
//                            //list_output_ErrorMsg = new List<string>();

//                            //GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//                            //if (list_output_GpResult.Count == 1)
//                            //{
//                            //    IGPResult gpResult = list_output_GpResult[0];

//                            //    if (gpResult.IsFailed == true)
//                            //    {
//                            //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                            //        return;
//                            //    }
//                            //}

//                            //LandUpdateManager._listFound_OID_new.Add(lng_OID_new);

//                            ////select lu_log is null
//                            //QueryFilter pQF_new_lu_log = new QueryFilter();
//                            //pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//                            //aFeatureLayer_result.Select(pQF_new_lu_log);

//                            ////field calculate-lu_log
//                            //list_output_GpResult = new List<IGPResult>();
//                            //list_output_ErrorMsg = new List<string>();

//                            //string strExp = "'" + outStr_log + "'";
//                            //string strCodeBlock = string.Empty;
//                            //GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, strCodeBlock,
//                            //list_output_GpResult,
//                            //list_output_ErrorMsg);

//                            //if (list_output_GpResult.Count == 1)
//                            //{
//                            //    IGPResult gpResult = list_output_GpResult[0];

//                            //    if (gpResult.IsFailed == true)
//                            //    {
//                            //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                            //        return;
//                            //    }
//                            //}

//                            ////field calculate-原段號
//                            //list_output_GpResult = new List<IGPResult>();
//                            //list_output_ErrorMsg = new List<string>();

//                            //strExp = "'" + str_段號_old + "'";
//                            //strCodeBlock = string.Empty;
//                            //GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, strCodeBlock,
//                            //list_output_GpResult,
//                            //list_output_ErrorMsg);

//                            //if (list_output_GpResult.Count == 1)
//                            //{
//                            //    IGPResult gpResult = list_output_GpResult[0];

//                            //    if (gpResult.IsFailed == true)
//                            //    {
//                            //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                            //        return;
//                            //    }
//                            //}

//                        }//  using (Feature pFeature_old = (Feature)rowCursor_old.Current)  //處理每一筆對應到的舊版地籍
//                    }// while (rowCursor_old.MoveNext())
//                }// using (RowCursor rowCursor_old = LandUpdateManager._featurelayer_old.Search(pQF_old))

//            }
//        }

//        public static bool check_sum_area_分割_one(Dictionary<long, List<mdlLand>> aDicOID_newLand_合併,
//           KeyValuePair<long, List<mdlLand>> item, FeatureLayer aFeatureLayer_result, bool aBlSearchBuffer, out double outDblDiff_Ratio)
//        {
//            bool blFound = false;
//            outDblDiff_Ratio = 999;

//            long lng原OID = item.Key;
//            List<mdlLand> listLand_分割 = item.Value;

//            double dbl原登記面積 = 0;
//            double dbl登記面積_sum_of_new = 0;
//            string str原段號 = string.Empty;

//            string strOIDs_new_分割 = string.Empty;

//            int intLandCount_分割 = 0;

//            foreach (mdlLand land_分割 in listLand_分割)
//            {
//                if (!aBlSearchBuffer)
//                {
//                    if (land_分割.SearchedByBuffer == true)
//                    {
//                        continue;
//                    }
//                }

//                if (dbl原登記面積 == 0)
//                {
//                    dbl原登記面積 = land_分割.原登記面積;
//                }

//                if (str原段號.Equals(string.Empty))
//                {
//                    str原段號 = land_分割.原段號;
//                }

//                dbl登記面積_sum_of_new += land_分割.新登記面積;

//                strOIDs_new_分割 += land_分割.新OID + ",";

//                intLandCount_分割++;
//            }

//            if (strOIDs_new_分割.EndsWith(","))
//            {
//                strOIDs_new_分割 = strOIDs_new_分割.Substring(0, strOIDs_new_分割.Length - 1);
//            }

//            if (strOIDs_new_分割.Length == 0)//沒有要處理的新地籍
//            {
//                return false;
//            }

//            if (dbl原登記面積 == 0)//dbl原登記面積==0 無法算變化比例
//            {
//                return false;
//            }

//            double dblDiff_登記面積 = Math.Abs(dbl原登記面積 - dbl登記面積_sum_of_new);
//            outDblDiff_Ratio = dblDiff_登記面積 / dbl原登記面積;

//            if (dblDiff_登記面積 == 0 || outDblDiff_Ratio <= _dblAreaTolerance_分割_合併)//檢查面積和符合
//            {
//                string str_log = "找到 center in;(分割)登記面積總和變化 <= " + _dblAreaTolerance_分割_合併 * 100 + " %";

//                if (intLandCount_分割 == 1)
//                {
//                    str_log = str_log.Replace("分割", "重劃");
//                }

//                if (aBlSearchBuffer)
//                {
//                    str_log = "buffer " + str_log;
//                }

//                LandUpdateManager._listFound_OID_old.Add(lng原OID);

//                //有比對成功，把新版圖徵加到結果圖層
//                //append
//                QueryFilter pQF_new_sel = new QueryFilter();
//                pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + " in (" + strOIDs_new_分割 + ")";

//                LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//                List<object> listFeatureLayer_ToAppend = new List<object>();
//                listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//                List<IGPResult> list_output_GpResult = new List<IGPResult>();
//                List<string> list_output_ErrorMsg = new List<string>();


//                GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                        return blFound;
//                    }
//                }

//                //todo? LandUpdateManager._listFound_OID_new.Add(pFeature_new.GetObjectID());

//                //select lu_log is null
//                QueryFilter pQF_new_lu_log = new QueryFilter();
//                pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//                aFeatureLayer_result.Select(pQF_new_lu_log);

//                //field calculate-lu_log
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                string strExp = "'" + str_log + "'";
//                string strCodeBlock = string.Empty;
//                GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_log + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return blFound;
//                    }
//                }

//                //field calculate-lu_type
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                strExp = "4";

//                if (intLandCount_分割 == 1)
//                {
//                    strExp = "2";
//                }

//                strCodeBlock = string.Empty;
//                GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return blFound;
//                    }
//                }

//                //field calculate-lu_type --> old layer
//                //選取舊版圖層中要找的圖徵
//                QueryFilter pQF_old = new QueryFilter();
//                pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + "=" + lng原OID;

//                LandUpdateManager._featurelayer_old.Select(pQF_old);

//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                //strExp = "4";
//                //strCodeBlock = string.Empty;
//                GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + LandUpdateManager._featurelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return blFound;
//                    }
//                }

//                //field calculate-原段號
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                strExp = "'" + str原段號 + "'";
//                strCodeBlock = string.Empty;
//                GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_原段號 + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return blFound;
//                    }
//                }

//                blFound = true;
//            }//     if (dblDiff_登記面積 ==0 || dblDiff_Ratio <= 0.01)//檢查面積和符合

//            return blFound;
//        }

//        /// <summary>
//        /// #4 比對地籍分割
//        /// </summary>
//        /// <param name="aDicOID_newLand_分割"></param>
//        /// <param name="aFeatureLayer_result"></param>
//        public static void check_sum_area_分割(Dictionary<long, List<mdlLand>> aDicOID_newLand_分割, FeatureLayer aFeatureLayer_result)
//        {
//            foreach (KeyValuePair<long, List<mdlLand>> item in aDicOID_newLand_分割)//處理每1個被分割(原)地籍
//            {

//                double dblDiff_Ratio;

//                bool blFound = check_sum_area_分割_one(aDicOID_newLand_分割, item, aFeatureLayer_result, false, out dblDiff_Ratio);

//                if (!blFound && (dblDiff_Ratio > 0 && dblDiff_Ratio < _dblBufferSearchTolerance))
//                {
//                    if (_dblBufferSearchDistance > 0)
//                    {
//                        blFound = check_sum_area_分割_one(aDicOID_newLand_分割, item, aFeatureLayer_result, true, out dblDiff_Ratio);
//                    }

//                }

//            }
//        }

//        public static bool check_sum_area_合併_one(Dictionary<long, List<mdlLand>> aDicOID_newLand_合併,
//            KeyValuePair<long, List<mdlLand>> item, FeatureLayer aFeatureLayer_result, bool aBlSearchBuffer, out double outDblDiff_Ratio)
//        {
//            bool blFound = false;
//            outDblDiff_Ratio = 999;

//            long lng新OID = item.Key;
//            List<mdlLand> listLand_old = item.Value;

//            double dbl新登記面積 = 0;
//            double dbl登記面積_sum_of_old = 0;
//            string str新段號 = string.Empty;

//            string strOIDs_old = string.Empty;
//            string str段號s_old = string.Empty;

//            int intLandCount_合併 = 0;

//            foreach (mdlLand land_old in listLand_old)
//            {
//                if (!aBlSearchBuffer)
//                {
//                    if (land_old.SearchedByBuffer == true)
//                    {
//                        continue;
//                    }
//                }

//                if (dbl新登記面積 == 0)
//                {
//                    dbl新登記面積 = land_old.新登記面積;
//                }

//                if (str新段號.Equals(string.Empty))
//                {
//                    str新段號 = land_old.新段號;
//                }

//                dbl登記面積_sum_of_old += land_old.原登記面積;

//                strOIDs_old += land_old.原OID + ",";
//                str段號s_old += land_old.原段號 + ",";

//                intLandCount_合併++;
//            }

//            if (strOIDs_old.EndsWith(","))
//            {
//                strOIDs_old = strOIDs_old.Substring(0, strOIDs_old.Length - 1);
//            }

//            if (strOIDs_old.Length == 0)//沒有要處理得舊地籍
//            {
//                return false;
//            }

//            if (dbl新登記面積 == 0)//dbl新登記面積==0 無法算變化比例
//            {
//                return false;
//            }

//            if (str段號s_old.EndsWith(","))
//            {
//                str段號s_old = str段號s_old.Substring(0, str段號s_old.Length - 1);
//            }

//            double dblDiff_登記面積 = Math.Abs(dbl新登記面積 - dbl登記面積_sum_of_old);
//            outDblDiff_Ratio = dblDiff_登記面積 / dbl新登記面積;

//            if (dblDiff_登記面積 == 0 || outDblDiff_Ratio <= _dblAreaTolerance_分割_合併)//檢查面積和符合
//            {
//                string str_log = "找到 center in;(合併)登記面積總和變化 <= " + _dblAreaTolerance_分割_合併 * 100 + " %";

//                if (intLandCount_合併 == 1)
//                {
//                    str_log = str_log.Replace("合併", "重劃");
//                }

//                if (aBlSearchBuffer)
//                {
//                    str_log = "buffer " + str_log;
//                }

//                foreach (mdlLand land_old in listLand_old)
//                {
//                    if (!aBlSearchBuffer)
//                    {
//                        if (land_old.SearchedByBuffer == true)
//                        {
//                            continue;
//                        }
//                    }

//                    LandUpdateManager._listFound_OID_old.Add(land_old.原OID);
//                }

//                //有比對成功，把新版圖徵加到結果圖層
//                //append
//                QueryFilter pQF_new_sel = new QueryFilter();
//                pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + " = " + lng新OID;

//                LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//                List<object> listFeatureLayer_ToAppend = new List<object>();
//                listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//                List<IGPResult> list_output_GpResult = new List<IGPResult>();
//                List<string> list_output_ErrorMsg = new List<string>();

//                GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                        return blFound;
//                    }
//                }

//                //todo? LandUpdateManager._listFound_OID_new.Add(pFeature_new.GetObjectID());

//                //select lu_log is null
//                QueryFilter pQF_new_lu_log = new QueryFilter();
//                pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//                aFeatureLayer_result.Select(pQF_new_lu_log);

//                //field calculate-lu_log
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                string strExp = "'" + str_log + "'";
//                string strCodeBlock = string.Empty;
//                GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_log + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return blFound;
//                    }
//                }

//                //field calculate-lu_type
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                strExp = "3";

//                if (intLandCount_合併 == 1)
//                {
//                    strExp = "2";
//                }

//                strCodeBlock = string.Empty;
//                GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return blFound;
//                    }
//                }

//                //field calculate-lu_type --> old layer
//                //選取舊版圖層中要找的圖徵
//                QueryFilter pQF_old = new QueryFilter();
//                pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + " in (" + strOIDs_old + ")";

//                LandUpdateManager._featurelayer_old.Select(pQF_old);

//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                //strExp = "4";
//                //strCodeBlock = string.Empty;
//                GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + LandUpdateManager._featurelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return blFound;
//                    }
//                }

//                //field calculate-原段號
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                strExp = "'" + str段號s_old + "'";
//                strCodeBlock = string.Empty;
//                GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, "Arcade", strCodeBlock,
//                list_output_GpResult,
//                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_原段號 + " exp:" + strExp + " code block:" + strCodeBlock);
//                        return blFound;
//                    }
//                }

//                blFound = true;
//                //aDicOID_newLand_合併.Remove(item.Key);

//            }//     if (dblDiff_登記面積 ==0 || dblDiff_Ratio <= 0.01)//檢查面積和符合

//            return blFound;

//        }

//        /// <summary>
//        /// #3 比對地籍合併
//        /// </summary>
//        /// <param name="aDicOID_newLand_合併"></param>
//        /// <param name="aFeatureLayer_result"></param>
//        public static void check_sum_area_合併(Dictionary<long, List<mdlLand>> aDicOID_newLand_合併, FeatureLayer aFeatureLayer_result)
//        {
//            foreach (KeyValuePair<long, List<mdlLand>> item in aDicOID_newLand_合併)//處理每1個被合併(新)地籍，(原地籍多個被合併成一個)
//            {
//                double dblDiff_Ratio;

//                bool blFound = check_sum_area_合併_one(aDicOID_newLand_合併, item, aFeatureLayer_result, false, out dblDiff_Ratio);

//                if (!blFound && (dblDiff_Ratio > 0 && dblDiff_Ratio < _dblBufferSearchTolerance))
//                {
//                    if (_dblBufferSearchDistance > 0)
//                    {
//                        blFound = check_sum_area_合併_one(aDicOID_newLand_合併, item, aFeatureLayer_result, true, out dblDiff_Ratio);
//                    }
//                }

//                //long lng新OID = item.Key;
//                //List<mdlLand> listLand_old_分割 = item.Value;

//                //double dbl新登記面積 = 0;
//                //double dbl登記面積_sum_of_old = 0;
//                //string str新段號 = string.Empty;

//                //string strOIDs_old_分割 = string.Empty;
//                //string str段號s_old_分割 = string.Empty;

//                //foreach (mdlLand land_old_分割 in listLand_old_分割)
//                //{
//                //    if (dbl新登記面積 == 0)
//                //    {
//                //        dbl新登記面積 = land_old_分割.新登記面積;
//                //    }

//                //    if (str新段號.Equals(string.Empty))
//                //    {
//                //        str新段號 = land_old_分割.新段號;
//                //    }

//                //    dbl登記面積_sum_of_old += land_old_分割.原登記面積;

//                //    strOIDs_old_分割 += land_old_分割.原OID + ",";
//                //    str段號s_old_分割 += land_old_分割.原段號 + ",";
//                //}

//                //if (strOIDs_old_分割.EndsWith(","))
//                //{
//                //    strOIDs_old_分割 = strOIDs_old_分割.Substring(0, strOIDs_old_分割.Length - 1);
//                //}

//                //if (str段號s_old_分割.EndsWith(","))
//                //{
//                //    str段號s_old_分割 = str段號s_old_分割.Substring(0, str段號s_old_分割.Length - 1);
//                //}

//                //double dblDiff_登記面積 = Math.Abs(dbl新登記面積 - dbl登記面積_sum_of_old);
//                //double dblDiff_Ratio = dblDiff_登記面積 / dbl新登記面積;

//                //if (dblDiff_登記面積 == 0 || dblDiff_Ratio <= _dblAreaTolerance_分割_合併)//檢查面積和符合
//                //{
//                //    string str_log = "找到 center in;(合併)登記面積總和變化 <= " + _dblAreaTolerance_分割_合併 * 100 + " %";

//                //    foreach (mdlLand land_old_分割 in listLand_old_分割)
//                //    {
//                //        LandUpdateManager._listFound_OID_old.Add(land_old_分割.原OID);                        
//                //    }

//                //    //有比對成功，把新版圖徵加到結果圖層
//                //    //append
//                //    QueryFilter pQF_new_sel = new QueryFilter();
//                //    pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + " = " + lng新OID ;

//                //    LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//                //    List<object> listFeatureLayer_ToAppend = new List<object>();
//                //    listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//                //    List<IGPResult> list_output_GpResult = new List<IGPResult>();
//                //    List<string> list_output_ErrorMsg = new List<string>();


//                //    GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//                //    if (list_output_GpResult.Count == 1)
//                //    {
//                //        IGPResult gpResult = list_output_GpResult[0];

//                //        if (gpResult.IsFailed == true)
//                //        {
//                //            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                //            return;
//                //        }
//                //    }

//                //    //todo? LandUpdateManager._listFound_OID_new.Add(pFeature_new.GetObjectID());

//                //    //select lu_log is null
//                //    QueryFilter pQF_new_lu_log = new QueryFilter();
//                //    pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//                //    aFeatureLayer_result.Select(pQF_new_lu_log);

//                //    //field calculate-lu_log
//                //    list_output_GpResult = new List<IGPResult>();
//                //    list_output_ErrorMsg = new List<string>();

//                //    string strExp = "'" + str_log + "'";
//                //    string strCodeBlock = string.Empty;
//                //    GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, "Arcade", strCodeBlock,
//                //    list_output_GpResult,
//                //    list_output_ErrorMsg);

//                //    if (list_output_GpResult.Count == 1)
//                //    {
//                //        IGPResult gpResult = list_output_GpResult[0];

//                //        if (gpResult.IsFailed == true)
//                //        {
//                //            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                //            return;
//                //        }
//                //    }

//                //    //field calculate-lu_type
//                //    list_output_GpResult = new List<IGPResult>();
//                //    list_output_ErrorMsg = new List<string>();

//                //    strExp = "3";
//                //    strCodeBlock = string.Empty;
//                //    GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                //    list_output_GpResult,
//                //    list_output_ErrorMsg);

//                //    if (list_output_GpResult.Count == 1)
//                //    {
//                //        IGPResult gpResult = list_output_GpResult[0];

//                //        if (gpResult.IsFailed == true)
//                //        {
//                //            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                //            return;
//                //        }
//                //    }

//                //    //field calculate-lu_type --> old layer
//                //    //選取舊版圖層中要找的圖徵
//                //    QueryFilter pQF_old = new QueryFilter();
//                //    pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + " in (" + strOIDs_old_分割 +")";

//                //    LandUpdateManager._featurelayer_old.Select(pQF_old);

//                //    list_output_GpResult = new List<IGPResult>();
//                //    list_output_ErrorMsg = new List<string>();

//                //    //strExp = "4";
//                //    //strCodeBlock = string.Empty;
//                //    GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                //    list_output_GpResult,
//                //    list_output_ErrorMsg);

//                //    if (list_output_GpResult.Count == 1)
//                //    {
//                //        IGPResult gpResult = list_output_GpResult[0];

//                //        if (gpResult.IsFailed == true)
//                //        {
//                //            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                //            return;
//                //        }
//                //    }

//                //    //field calculate-原段號
//                //    list_output_GpResult = new List<IGPResult>();
//                //    list_output_ErrorMsg = new List<string>();

//                //    strExp = "'" + str段號s_old_分割 + "'";
//                //    strCodeBlock = string.Empty;
//                //    GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, "Arcade", strCodeBlock,
//                //    list_output_GpResult,
//                //    list_output_ErrorMsg);

//                //    if (list_output_GpResult.Count == 1)
//                //    {
//                //        IGPResult gpResult = list_output_GpResult[0];

//                //        if (gpResult.IsFailed == true)
//                //        {
//                //            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                //            return;
//                //        }
//                //    }
//                //}//     if (dblDiff_登記面積 ==0 || dblDiff_Ratio <= 0.01)//檢查面積和符合

//            }//foreach (KeyValuePair<long, List<mdlLand>> item in aDicOID_newLand_合併)//處理每1個被合併(新)地籍，(原地籍多個被合併成一個)



//        }

//        public static void dissolve_not_found_then_search_center_in_old_new(FeatureLayer aFeatureLayer_result, List<long> aList_notFound_to_dissolve)
//        {
//            //#5.1 原地籍剩下未找到的dissolve (single part)
//            //select not found
//            //foreach (long lngOID_found_old in LandUpdateManager._listFound_OID_old)
//            //{
//            //    if (list_OID_sel_old.Contains(lngOID_found_old))
//            //    {
//            //        list_OID_sel_old.Remove(lngOID_found_old);
//            //    }
//            //}

//            LandUpdateManager._featurelayer_old.ClearSelection();
//            LandUpdateManager._featurelayer_new.ClearSelection();

//            string strOID_NotFounds = string.Empty;

//            foreach (long lngOIDNotFond in aList_notFound_to_dissolve)
//            {
//                strOID_NotFounds += lngOIDNotFond + ",";
//            }

//            if (strOID_NotFounds.EndsWith(","))
//            {
//                strOID_NotFounds = strOID_NotFounds.Substring(0, strOID_NotFounds.Length - 1);
//            }

//            strOID_NotFounds = strOID_NotFounds.Trim();

//            //select not found 
//            QueryFilter pQF_old_not_found = new QueryFilter();
//            pQF_old_not_found.WhereClause = LandUpdateManager._strFieldName_OID_old + " in (" + strOID_NotFounds + ")";

//            LandUpdateManager._featurelayer_old.Select(pQF_old_not_found);

//            string strLayerName_notFound_dissolve = LandUpdateManager._featurelayer_old.Name + "_notFound_dis";
//            FeatureLayer featureLayer_notFound_dissolve = null;
//            List<string> listDis_Fields = new List<string>();
//            List<string> listSta_Fields = new List<string>();

//            // listDis_Fields.Add(LandUpdateManager._strFieldName_OID_old);
//            // listSta_Fields.Add(LandUpdateManager._strFieldName_OID_old + " COUNT");//因dissovle 非指定屬性欄位進行，dissolve 沒有針對被dissolve的圖徵統計

//            List<IGPResult> list_output_GpResult = new List<IGPResult>();
//            List<string> list_output_ErrorMsg = new List<string>();

//            GpTools.Dissolve(LandUpdateManager._featurelayer_old,
//                             strLayerName_notFound_dissolve,
//                             listDis_Fields,
//                             listSta_Fields,
//                               "SINGLE_PART",
//                             list_output_GpResult,
//                             list_output_ErrorMsg);

//            if (list_output_GpResult.Count == 1)
//            {
//                IGPResult gpResult = list_output_GpResult[0];

//                if (gpResult.IsFailed == true)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Dissolve 發生錯誤: " + gpResult.ErrorCode.ToString());
//                    return;
//                }

//                featureLayer_notFound_dissolve = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == strLayerName_notFound_dissolve).FirstOrDefault() as FeatureLayer;

//                if (featureLayer_notFound_dissolve == null)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("取得 Dissolve 圖層失敗: ");
//                    return;
//                }
//            }

//            //取得dissolve圖層ObjectID 欄位名
//            TableDefinition tableDefinition_dis = featureLayer_notFound_dissolve.GetTable().GetDefinition();
//            IReadOnlyList<Field> fields_dis = tableDefinition_dis.GetFields();
//            string strFieldName_OID_dis = fields_dis.FirstOrDefault(a => a.FieldType == FieldType.OID).Name;

//            featureLayer_notFound_dissolve.Search();

//            bool bl處理dissolve地籍發生錯誤 = false;

//            using (RowCursor rowCursor_notFound_dissolve = featureLayer_notFound_dissolve.Search(null))
//            {
//                while (rowCursor_notFound_dissolve.MoveNext())
//                {
//                    using (Feature pFeature_notFound_dissolve = (Feature)rowCursor_notFound_dissolve.Current)  //處理每一筆dissolve地籍
//                    {
//                        try
//                        {
//                            if (LandUpdateManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                            {
//                                //strCompleOrCancal = "取消";
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地籍更新" + "取消", "資訊");
//                                return;
//                            }

//                            //#5.2 找出 dissovle count OID > 1 再找原地籍center in 這個disolve 範圍的原地籍OID、段號用分號分隔，登記面積和
//                            //#5.3 找出 dissovle count OID > 1 再找新地籍center in 這個disolve 範圍的分割新地籍 (登記面積<=這個disolve 範圍)

//                            //找出 dissovle count OID > 1
//                            //select 這1筆dissolve圖徵
//                            QueryFilter pQF_notFound_dissolve = new QueryFilter();
//                            pQF_notFound_dissolve.WhereClause = strFieldName_OID_dis + "=" + pFeature_notFound_dissolve.GetObjectID() + "";
//                            featureLayer_notFound_dissolve.Select(pQF_notFound_dissolve);

//                            //依dissolve圖徵選取舊版圖徵(舊版center in dissolve圖徵)
//                            list_output_GpResult = new List<IGPResult>();
//                            list_output_ErrorMsg = new List<string>();

//                            GpTools.SelectLayerByLocation(LandUpdateManager._featurelayer_old,
//                                                     featureLayer_notFound_dissolve,
//                                                     "HAVE_THEIR_CENTER_IN",
//                                                     "0 Meters",
//                                                     list_output_GpResult,
//                                                     list_output_ErrorMsg);

//                            if (list_output_GpResult.Count == 1)
//                            {
//                                IGPResult gpResult = list_output_GpResult[0];

//                                if (gpResult.IsFailed == true)
//                                {
//                                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("select by location (舊版圖資)發生錯誤: " + gpResult.ErrorCode.ToString());
//                                    return;
//                                }
//                            }

//                            if (LandUpdateManager._featurelayer_old.SelectionCount > 1)// dissovle count OID > 1
//                            {
//                                //依dissolve圖徵選取新版圖徵(新版center in dissolve圖徵)
//                                list_output_GpResult = new List<IGPResult>();
//                                list_output_ErrorMsg = new List<string>();

//                                GpTools.SelectLayerByLocation(LandUpdateManager._featurelayer_new,
//                                                         featureLayer_notFound_dissolve,
//                                                         "HAVE_THEIR_CENTER_IN",
//                                                         "0 Meters",
//                                                         list_output_GpResult,
//                                                         list_output_ErrorMsg);

//                                if (list_output_GpResult.Count == 1)
//                                {
//                                    IGPResult gpResult = list_output_GpResult[0];

//                                    if (gpResult.IsFailed == true)
//                                    {
//                                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("select by location (新版圖資)發生錯誤: " + gpResult.ErrorCode.ToString());
//                                        return;
//                                    }
//                                }

//                                if (LandUpdateManager._featurelayer_new.SelectionCount >= 1)
//                                {
//                                    List<mdlLand> listLand_舊版 = new List<mdlLand>();//舊版center in dissolve圖徵的清單
//                                    List<mdlLand> listLand_新版 = new List<mdlLand>();//新版center in dissolve圖徵的清單

//                                    //取得舊版center in dissolve圖徵的清單
//                                    using (RowCursor rowCursor_old = LandUpdateManager._featurelayer_old.GetSelection().Search(null, false))
//                                    {
//                                        while (rowCursor_old.MoveNext())
//                                        {
//                                            using (Feature pFeature_old = (Feature)rowCursor_old.Current)  //處理每一筆對應到的新版地籍
//                                            {
//                                                object obj_登記面積_old = pFeature_old[LandUpdateManager._intFldIdx_登記面積_old];
//                                                string str_登記面積_old = null;
//                                                double dbl_登記面積_old = -1;

//                                                if (obj_登記面積_old != null)
//                                                {
//                                                    str_登記面積_old = obj_登記面積_old.ToString().Trim();
//                                                    bool blParseOk_old = double.TryParse(str_登記面積_old, out dbl_登記面積_old);
//                                                }

//                                                object obj_段號_old = pFeature_old[LandUpdateManager._intFldIdx_段號_old];
//                                                string str_段號_old = null;

//                                                if (dbl_登記面積_old == -1 || obj_段號_old == null)
//                                                {
//                                                    continue;//處理下一筆
//                                                }

//                                                str_段號_old = obj_段號_old.ToString().Trim();

//                                                mdlLand pmdlLand = new mdlLand();
//                                                pmdlLand.原OID = pFeature_old.GetObjectID();
//                                                pmdlLand.原段號 = str_段號_old;
//                                                pmdlLand.原登記面積 = dbl_登記面積_old;

//                                                listLand_舊版.Add(pmdlLand);

//                                            }
//                                        }
//                                        /////////
//                                    }

//                                    //取得新版center in dissolve圖徵的清單
//                                    using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.GetSelection().Search(null, false))
//                                    {
//                                        while (rowCursor_new.MoveNext())
//                                        {
//                                            using (Feature pFeature_new = (Feature)rowCursor_new.Current)  //處理每一筆對應到的新版地籍
//                                            {
//                                                object obj_登記面積_new = pFeature_new[LandUpdateManager._intFldIdx_登記面積_new];
//                                                string str_登記面積_new = null;
//                                                double dbl_登記面積_new = -1;

//                                                if (obj_登記面積_new != null)
//                                                {
//                                                    str_登記面積_new = obj_登記面積_new.ToString().Trim();
//                                                    bool blParseOk_new = double.TryParse(str_登記面積_new, out dbl_登記面積_new);
//                                                }

//                                                object obj_段號_new = pFeature_new[LandUpdateManager._intFldIdx_段號_new];
//                                                string str_段號_new = null;

//                                                if (dbl_登記面積_new == -1 || obj_段號_new == null)
//                                                {
//                                                    continue;//處理下一筆
//                                                }

//                                                str_段號_new = obj_段號_new.ToString().Trim();

//                                                mdlLand pmdlLand = new mdlLand();
//                                                pmdlLand.新OID = pFeature_new.GetObjectID();
//                                                pmdlLand.新段號 = str_段號_new;
//                                                pmdlLand.新登記面積 = dbl_登記面積_new;

//                                                listLand_新版.Add(pmdlLand);

//                                            }
//                                        }
//                                    }//using (RowCursor rowCursor_new = LandUpdateManager._featurelayer_new.GetSelection().Search(null, false))

//                                    //計算面積和
//                                    double dblArea_sum_old = 0;
//                                    string str段號s_old = string.Empty;
//                                    string strOIDs_old = string.Empty;

//                                    foreach (mdlLand pLand_old in listLand_舊版)
//                                    {
//                                        dblArea_sum_old += pLand_old.原登記面積;
//                                        str段號s_old += pLand_old.原段號 + ",";
//                                        strOIDs_old += pLand_old.原OID + ",";
//                                    }

//                                    if (dblArea_sum_old == 0)
//                                    {
//                                        continue;
//                                    }

//                                    if (str段號s_old.EndsWith(","))
//                                    {
//                                        str段號s_old = str段號s_old.Substring(0, str段號s_old.Length - 1);
//                                    }

//                                    if (strOIDs_old.EndsWith(","))
//                                    {
//                                        strOIDs_old = strOIDs_old.Substring(0, strOIDs_old.Length - 1);
//                                    }

//                                    double dblArea_sum_new = 0;

//                                    foreach (mdlLand pLand_new in listLand_新版)
//                                    {
//                                        dblArea_sum_new += pLand_new.新登記面積;
//                                    }

//                                    double dblDiff_登記面積 = Math.Abs(dblArea_sum_old - dblArea_sum_new);
//                                    double dblDiff_Ratio = dblDiff_登記面積 / dblArea_sum_old;

//                                    if (dblDiff_登記面積 == 0 || dblDiff_Ratio <= LandUpdateManager._dblAreaTolerance)//面積不變
//                                    {
//                                        //將舊地籍屬性(段號清單)更新到每個新地籍

//                                        string str_log = "找到{合併後分割}或{分割後合併}登記面積總和變化 <= " + LandUpdateManager._dblAreaTolerance_分割_合併 * 100 + " %";

//                                        foreach (mdlLand pLand_old in listLand_舊版)
//                                        {
//                                            LandUpdateManager._listFound_OID_old.Add(pLand_old.原OID);
//                                        }

//                                        //有比對成功，把新版圖徵加到結果圖層
//                                        //append
//                                        string strOIDs_new_分割 = string.Empty;

//                                        foreach (mdlLand pLand_new in listLand_新版)
//                                        {
//                                            strOIDs_new_分割 += pLand_new.新OID + ",";
//                                        }

//                                        if (strOIDs_new_分割.EndsWith(","))
//                                        {
//                                            strOIDs_new_分割 = strOIDs_new_分割.Substring(0, strOIDs_new_分割.Length - 1);
//                                        }

//                                        QueryFilter pQF_new_sel = new QueryFilter();
//                                        pQF_new_sel.WhereClause = LandUpdateManager._strFieldName_OID_new + " in (" + strOIDs_new_分割 + ")";

//                                        LandUpdateManager._featurelayer_new.Select(pQF_new_sel);

//                                        List<object> listFeatureLayer_ToAppend = new List<object>();
//                                        listFeatureLayer_ToAppend.Add(LandUpdateManager._featurelayer_new);

//                                        list_output_GpResult = new List<IGPResult>();
//                                        list_output_ErrorMsg = new List<string>();


//                                        GpTools.Append(listFeatureLayer_ToAppend, aFeatureLayer_result, list_output_GpResult, list_output_ErrorMsg);

//                                        if (list_output_GpResult.Count == 1)
//                                        {
//                                            IGPResult gpResult = list_output_GpResult[0];

//                                            if (gpResult.IsFailed == true)
//                                            {
//                                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("append處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                                                return;
//                                            }
//                                        }

//                                        //todo? LandUpdateManager._listFound_OID_new.Add(pFeature_new.GetObjectID());

//                                        //select lu_log is null
//                                        QueryFilter pQF_new_lu_log = new QueryFilter();
//                                        pQF_new_lu_log.WhereClause = LandUpdateManager._strFieldName_lu_log + " is null";

//                                        aFeatureLayer_result.Select(pQF_new_lu_log);

//                                        //field calculate-lu_log
//                                        list_output_GpResult = new List<IGPResult>();
//                                        list_output_ErrorMsg = new List<string>();

//                                        string strExp = "'" + str_log + "'";
//                                        string strCodeBlock = string.Empty;
//                                        GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_log, strExp, "Arcade", strCodeBlock,
//                                        list_output_GpResult,
//                                        list_output_ErrorMsg);

//                                        if (list_output_GpResult.Count == 1)
//                                        {
//                                            IGPResult gpResult = list_output_GpResult[0];

//                                            if (gpResult.IsFailed == true)
//                                            {
//                                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_log + " exp:" + strExp + " code block:" + strCodeBlock);
//                                                return;
//                                            }
//                                        }

//                                        //field calculate-lu_type
//                                        list_output_GpResult = new List<IGPResult>();
//                                        list_output_ErrorMsg = new List<string>();

//                                        strExp = "5";
//                                        strCodeBlock = string.Empty;
//                                        GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                                        list_output_GpResult,
//                                        list_output_ErrorMsg);

//                                        if (list_output_GpResult.Count == 1)
//                                        {
//                                            IGPResult gpResult = list_output_GpResult[0];

//                                            if (gpResult.IsFailed == true)
//                                            {
//                                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                                                return;
//                                            }
//                                        }

//                                        //field calculate-lu_type --> old layer
//                                        //選取舊版圖層中要找的圖徵
//                                        QueryFilter pQF_old = new QueryFilter();
//                                        pQF_old.WhereClause = LandUpdateManager._strFieldName_OID_old + " in (" + strOIDs_old + ")";

//                                        LandUpdateManager._featurelayer_old.Select(pQF_old);


//                                        list_output_GpResult = new List<IGPResult>();
//                                        list_output_ErrorMsg = new List<string>();

//                                        //strExp = "5";
//                                        //strCodeBlock = string.Empty;
//                                        GpTools.CalculateField(LandUpdateManager._featurelayer_old, LandUpdateManager._strFieldName_lu_type, strExp, "Arcade", strCodeBlock,
//                                        list_output_GpResult,
//                                        list_output_ErrorMsg);

//                                        if (list_output_GpResult.Count == 1)
//                                        {
//                                            IGPResult gpResult = list_output_GpResult[0];

//                                            if (gpResult.IsFailed == true)
//                                            {
//                                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + " layer:" + LandUpdateManager._featurelayer_old.Name + " field:" + LandUpdateManager._strFieldName_lu_type + " exp:" + strExp + " code block:" + strCodeBlock);
//                                                return;
//                                            }
//                                        }

//                                        //field calculate-原段號
//                                        list_output_GpResult = new List<IGPResult>();
//                                        list_output_ErrorMsg = new List<string>();

//                                        strExp = "'" + str段號s_old + "'";
//                                        strCodeBlock = string.Empty;
//                                        GpTools.CalculateField(aFeatureLayer_result, LandUpdateManager._strFieldName_原段號, strExp, "Arcade", strCodeBlock,
//                                        list_output_GpResult,
//                                        list_output_ErrorMsg);

//                                        if (list_output_GpResult.Count == 1)
//                                        {
//                                            IGPResult gpResult = list_output_GpResult[0];

//                                            if (gpResult.IsFailed == true)
//                                            {
//                                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + " layer:" + aFeatureLayer_result.Name + " field:" + LandUpdateManager._strFieldName_原段號 + " exp:" + strExp + " code block:" + strCodeBlock);
//                                                return;
//                                            }
//                                        }

//                                    }// if (dblDiff_登記面積 == 0 || dblDiff_Ratio <= LandUpdateManager._dblAreaTolerance)//面積不變

//                                }//if (LandUpdateManager._featurelayer_new.SelectionCount >= 1)
//                            }// if (LandUpdateManager._featurelayer_old.SelectionCount > 1)// dissovle count OID > 1
//                        }
//                        catch (Exception ex)
//                        {

//                            //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("處理dissolve地籍發生錯誤, OID: " + pFeature_notFound_dissolve.GetObjectID() + ex.Message);
//                            bl處理dissolve地籍發生錯誤 = true;

//                        }

//                    }//using (Feature pFeature_notFound_dissolve = (Feature)rowCursor_notFound_dissolve.Current)  //處理每一筆dissolve地籍
//                }
//            }//using (RowCursor rowCursor_notFound_dissolve = LandUpdateManager._featurelayer_new.Search(null))

//            //MapView.Active.Map.RemoveLayer(featureLayer_notFound_dissolve);

//            if (bl處理dissolve地籍發生錯誤 == false)//沒發生錯誤才將dissolve圖層刪除，有發生錯誤將dissolve圖層留著，以查看錯誤
//            {
//                //Delete Feature Class
//                list_output_GpResult = new List<IGPResult>();
//                list_output_ErrorMsg = new List<string>();

//                GpTools.Delete(featureLayer_notFound_dissolve.GetFeatureClass(),
//                                list_output_GpResult,
//                                list_output_ErrorMsg);

//                if (list_output_GpResult.Count == 1)
//                {
//                    IGPResult gpResult = list_output_GpResult[0];

//                    if (gpResult.IsFailed == true)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Delete處理發生錯誤: " + gpResult.ErrorCode.ToString());
//                        return;
//                    }
//                }
//            }

//        }

//    }
//}
