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
using System.Text;
using System.Threading.Tasks;

namespace StreetViewPictureProcessorAddin
{
    public class StreetViewModule
    {
        //public static string strFolderPath_in = @"D:\COA_StreetView\Data\吳欣玲\110GOPRO";
        //public static string strLayerName_PhotosToPoints_out = "吳欣玲_110GOPRO2";
        public static string _strFileName_exif2 = "exif2.txt";


        //public static string _strFolderPath_web_virtual_folder_root = @"D:\COA\Layer\street_view_pic";

        ////IDT 237
        //public static string _strFolderPath_upload_root = @"D:\COA_StreetView\Data";
        //public static string _strFolderPath_web_virtual_folder_root_unc_to_record_db = @"\\\\10.1.100.237\\streetview_pic";
        //public static string _strFolderPath_web_virtual_folder_root_unc = @"\\10.1.100.237\streetview_pic";
        //public static string _strSDEFilePath = @"D:\COA_StreetView\connections\sde104.sde";

        ////NCHC 193
        //public static string _strFolderPath_upload_root = @"D:\COA_StreetView\Data";
        //public static string _strFolderPath_web_virtual_folder_root_unc_to_record_db = @"\\\\10.3.161.101\\streetview_pic";
        //public static string _strFolderPath_web_virtual_folder_root_unc = @"\\10.3.161.101\streetview_pic";
        //public static string _strSDEFilePath = @"D:\COA_StreetView\connections\sde106.sde";

        //NCHC 106
        public static string _strFolderPath_upload_root = @"\\10.3.161.102\Data";
        public static string _strFolderPath_web_virtual_folder_root_unc_to_record_db = @"\\\\10.3.161.101\\streetview_pic";
        public static string _strFolderPath_web_virtual_folder_root_unc = @"\\10.3.161.101\streetview_pic";
        public static string _strSDEFilePath = @"D:\COA_StreetView\connections\sde106.sde";


        public static string _strFGDBPath_template = @"D:\COA_StreetView\streetview_empty.gdb";
        //public static string strFGDBPath_out = @"D:\COA_StreetView\streetview_empty_process.gdb";
        public static string _strFGDBDatasetName_out = @"twd97";

        public static string _strFieldName_PhotosToPoints_DateTime = "DateTime";
        public static string _strFieldName_PhotosToPoints_st_dist = "st_dist";
        public static string _strFieldName_PhotosToPoints_jobid = "jobid";
        public static string _strFieldName_PhotosToPoints_Direction_camara = "Direction_camara";


        public static string _strFieldName_PhotosToPoints_X1 = "X1";
        public static string _strFieldName_PhotosToPoints_Y1 = "Y1";
        public static string _strFieldName_PhotosToPoints_Path = "Path";
        public static string _strFieldName_PhotosToPoints_Project = "Project";
        public static string _strFieldName_PhotosToPoints_project_name = "project_name";
        public static string _strFieldName_PhotosToPoints_investigator = "investigator";

        public static string _strFieldName_PhotosToPoints_X = "X";
        public static string _strFieldName_PhotosToPoints_Y = "Y";

        public static string _strFieldName_PhotosToPoints_X2 = "X2";
        public static string _strFieldName_PhotosToPoints_Y2 = "Y2";

        public static string _strFieldName_PhotosToPoints_Direction = "direction";
        public static string _strFieldName_PhotosToPoints_turn_angle = "turn_angle";

        //public static string _strFieldName_PhotosToPoints_d_X_X1 = "d_X_X1";
        //public static string _strFieldName_PhotosToPoints_d_Y_Y1 = "d_Y_Y1";
        //public static string _strFieldName_PhotosToPoints_d_X_Y = "d_X_Y";

        public static double _dblturn_angle_min = 35;//45;//35;
        public static double _dblturn_angle_max = 145;//135;//145;

        public static double _intUpdate_Time_Extent_pre_post_sec = 30;//15;
        public static double _intUpdate_Time_Extend_line_length_m = 50;

        public static double _dblDistanceTolerance = 3;

        
        public static string _strTableName_StreetViewPhotoProcessInfom = "DBO.StreetViewPhotoProcessInfom";//"sde104.DBO.StreetViewPhotoProcessInfom"
        public static string _strFieldName_StreetViewPhotoProcessInfom_job_type_code = "job_type_code";
        public static string _strFieldName_StreetViewPhotoProcessInfom_job_type_name = "job_type_name";
        public static string _strFieldName_StreetViewPhotoProcessInfom_jobid = "jobid";
        public static string _strLayerName_StreetViewPhoto = "DBO.StreetViewPhoto";//"sde104.DBO.StreetViewPhoto";
        public static string _strDatasetName_StreetViewPhoto = "DBO.twd97";// "sde104.DBO.twd97";

        public static bool AddField(object layerName, string fieldName, string strType, string strLength)
        {
            List<IGPResult> list_output_GpResult = new List<IGPResult>();
            List<string> list_output_ErrorMsg = new List<string>();

            GpTools.AddField(layerName, fieldName, strType,
                            "", "", strLength,
                            fieldName, "NULLABLE",
                            list_output_GpResult,
                            list_output_ErrorMsg);

            if (list_output_GpResult.Count == 1)
            {
                IGPResult gpResult = list_output_GpResult[0];

                if (gpResult.IsFailed == true)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + fieldName + "]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
                    return false;
                }
            }

            return true;
        }

        public static bool CalculateDistanceWithBasePoint_EachPoint(FeatureLayer featureLayer_PhotosToPoints)
        {
            bool blOk = false;

            try
            {
                QueryFilter pQf = new ArcGIS.Core.Data.QueryFilter();
                pQf.WhereClause = "";
                pQf.PostfixClause = "ORDER BY " + _strFieldName_PhotosToPoints_DateTime;
                RowCursor rowCursor = featureLayer_PhotosToPoints.Search(pQf);

                MapPoint p_pre = null;

                using (rowCursor)
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Feature pFeature_now = (Feature)rowCursor.Current)  //處理每一筆點位
                        {
                            MapPoint p_now = (MapPoint)pFeature_now.GetShape();

                            if (p_now != null)
                            {
                                if (p_pre != null)
                                {
                                    double dblDistance = GeometryEngine.Instance.Distance(p_pre, p_now);//取得和基準點的距離 

                                    if (dblDistance < _dblDistanceTolerance)//後續小於3公尺篩掉這個相片點位
                                    {
                                        pFeature_now[_strFieldName_PhotosToPoints_st_dist] = dblDistance;// - 999;
                                    }
                                    else
                                    {
                                        pFeature_now[_strFieldName_PhotosToPoints_st_dist] = dblDistance;
                                        p_pre = p_now;//大於等於3公尺才改變基準點
                                    }

                                    pFeature_now.Store();
                                }
                                else
                                {
                                    p_pre = p_now;//還沒有基準點，把目前的點設為基準點
                                }
                            }
                        }
                    }
                }

                blOk = true;
            }
            catch (Exception ex)
            {

            }

            return blOk;
        }

        public static void GetJobIdBy_job_type_code(string aStrCondition_job_type_code, out mdlProcessItem out_mdlProcessItem)//out string out_str_jobid, out string out_strFolderPath_in,out  Row out_row)
        {
            //out_str_jobid = null;
            //out_strFolderPath_in = null;
            //out_row = null;
            out_mdlProcessItem = null;

            using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri(StreetViewModule._strSDEFilePath))))
            {
                // Use the geodatabase.
                QueryDef queryDef = new QueryDef();
                queryDef.Tables = StreetViewModule._strTableName_StreetViewPhotoProcessInfom;// "sde104.DBO.StreetViewPhotoProcessInfom";
                QueryTableDescription queryTableDescription = new QueryTableDescription(queryDef);

                using (Table table_StreetViewPhotoProcessInfom = geodatabase.OpenQueryTable(queryTableDescription))
                {
                    TableDefinition tableDefinition_StreetViewPhotoProcessInfom_ = table_StreetViewPhotoProcessInfom.GetDefinition();

                    int intFldIdx_StreetViewPhotoProcessInfom_jobid = tableDefinition_StreetViewPhotoProcessInfom_.FindField(StreetViewModule._strFieldName_StreetViewPhotoProcessInfom_jobid);

                    QueryFilter pQf = new ArcGIS.Core.Data.QueryFilter();
                    table_StreetViewPhotoProcessInfom.Search(pQf);
                    pQf.WhereClause = StreetViewModule._strFieldName_StreetViewPhotoProcessInfom_job_type_code + aStrCondition_job_type_code;// " is null";

                    RowCursor rowCursor = table_StreetViewPhotoProcessInfom.Search(pQf);

                    using (rowCursor)
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row pRow_now = rowCursor.Current)  //處理每一筆點位
                            {
                                //取得jobid
                                object obj_jobid = pRow_now[intFldIdx_StreetViewPhotoProcessInfom_jobid];

                                string str_jobid = obj_jobid.ToString().Trim();


                                if (!string.IsNullOrEmpty(str_jobid))
                                {
                                    out_mdlProcessItem = new mdlProcessItem(pRow_now, str_jobid);

                                    //out_mdlProcessItem.str_jobid = out_str_jobid;
                                    // out_strFolderPath_in = StreetViewModule._strFolderPath_upload_root + @"\" + out_str_jobid;

                                    // out_row = pRow_now;



                                    break;//取得jobid 即可先進行處理
                                }
                            }
                        }
                    }

                }
            }
        }

        public static void  AddJobInform(int aInt_job_type_code, string aStr_job_type_name, mdlProcessItem aMdlProcessItem)//out string out_str_jobid, out string out_strFolderPath_in,out  Row out_row)
        {
            
            ////string message = String.Empty;
            ////bool creationResult = false;

            ////  EditOperation editOperation = new EditOperation();

            //await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            //{

            using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri(StreetViewModule._strSDEFilePath))))
            {
                // Use the geodatabase.

                using (Table table_StreetViewPhotoProcessInfom = geodatabase.OpenDataset<Table>(StreetViewModule._strTableName_StreetViewPhotoProcessInfom))
                {
                    ////    editOperation.Callback(context => {

                    //TableDefinition tableDefinition_StreetViewPhotoProcessInfom_ = table_StreetViewPhotoProcessInfom.GetDefinition();
                    //int intFldIdx_StreetViewPhotoProcessInfom_jobid = tableDefinition_StreetViewPhotoProcessInfom_.FindField(StreetViewModule._strFieldName_StreetViewPhotoProcessInfom_jobid);

                    using (RowBuffer rowBuffer = table_StreetViewPhotoProcessInfom.CreateRowBuffer())
                    {
                        // Either the field index or the field name can be used in the indexer.
                        rowBuffer[StreetViewModule._strFieldName_StreetViewPhotoProcessInfom_jobid] = aMdlProcessItem._str_jobid;
                        rowBuffer[StreetViewModule._strFieldName_StreetViewPhotoProcessInfom_job_type_code] = aInt_job_type_code;
                        rowBuffer[StreetViewModule._strFieldName_StreetViewPhotoProcessInfom_job_type_name] = aStr_job_type_name;

                        using (Row row = table_StreetViewPhotoProcessInfom.CreateRow(rowBuffer))
                        {
                            // To Indicate that the attribute table has to be updated.
                            ////  context.Invalidate(row);
                        }
                    }

                    ////       }, table_StreetViewPhotoProcessInfom);

                    ////try
                    ////{
                    ////    creationResult = editOperation.Execute();

                    ////    if (!creationResult)
                    ////    {
                    ////        message = editOperation.ErrorMessage;
                    ////    }
                    ////}
                    ////catch (GeodatabaseException exObj)
                    ////{
                    ////    message = exObj.Message;
                    ////}

                }
            }
            //});


            ////if (!string.IsNullOrEmpty(message))
            ////    MessageBox.Show(message);

        }


        public static MapPoint GetCrossPoint(RowCursor rowCursor_ToUpdateGPS, string strFieldName_PhotosToPoints_X, string strFieldName_PhotosToPoints_Y)
        {
            MapPoint mp_xy_cross = null;
            MapPoint mp_xy_pre = null;
            // LineSegment lineFromMapPoint_pre = null;

            double dblAgnle_deg_xy_pre = -1;
            List<Feature> listFeatures_ToUpdateGPS = new List<Feature>();
            List<MapPoint> listMapPoint_ToUpdateGPS = new List<MapPoint>();

            int intIdxPoint = -1;
            int intIdxTurnPoint = -1;

            using (rowCursor_ToUpdateGPS)
            {
                while (rowCursor_ToUpdateGPS.MoveNext())
                {
                    using (Feature pFeature_now = (Feature)rowCursor_ToUpdateGPS.Current)
                    {
                        intIdxPoint++;

                        listFeatures_ToUpdateGPS.Add(pFeature_now);

                        //計算XY 的移動角度變化，找到轉彎點
                        double dblX = Convert.ToDouble(pFeature_now[strFieldName_PhotosToPoints_X]);//StreetViewModule._strFieldName_PhotosToPoints_X
                        double dblY = Convert.ToDouble(pFeature_now[strFieldName_PhotosToPoints_Y]);//StreetViewModule._strFieldName_PhotosToPoints_Y

                        MapPoint mp_xy = MapPointBuilder.CreateMapPoint(dblX, dblY, SpatialReferences.WGS84);

                        listMapPoint_ToUpdateGPS.Add(mp_xy);

                        if (mp_xy_pre != null)
                        {
                            LineSegment lineFromMapPoint = LineBuilder.CreateLineSegment(mp_xy_pre, mp_xy, SpatialReferences.WGS84);

                            double dblAgnle_xy_deg = (Math.Abs(lineFromMapPoint.Angle) * 360) / (2 * Math.PI);

                            ////test
                            //System.Diagnostics.Debug.WriteLine(intIdxPoint + "  point:" + Convert.ToDateTime(pFeature_now[_strFieldName_PhotosToPoints_DateTime])+ " angle######"+ dblAgnle_xy_deg);
                            //MapPoint mp_shape =(MapPoint) pFeature_now.GetShape();
                            //System.Diagnostics.Debug.WriteLine( "xy:" + dblX +","+dblY + "shape xy:" + mp_shape.X + "," + mp_shape.Y);

                            ////test end
                            if (dblAgnle_deg_xy_pre != -1)
                            {
                                double dblAngel_deg_xy_change = Math.Abs(dblAgnle_xy_deg - dblAgnle_deg_xy_pre);

                                if (dblAngel_deg_xy_change > 180)
                                {
                                    dblAngel_deg_xy_change = 360 - dblAngel_deg_xy_change;
                                }

                                if (dblAngel_deg_xy_change >= StreetViewModule._dblturn_angle_min &&
                                    dblAngel_deg_xy_change <= StreetViewModule._dblturn_angle_max)//目前的點為轉彎點
                                {
                                    intIdxTurnPoint = intIdxPoint;
                                    //  System.Diagnostics.Debug.WriteLine(intIdxTurnPoint+" turn point:" +Convert.ToDateTime(pFeature_now[_strFieldName_PhotosToPoints_DateTime]));
                                }
                            }

                            dblAgnle_deg_xy_pre = dblAgnle_xy_deg;

                        }

                        mp_xy_pre = mp_xy;
                    }
                }
            }

            //延伸目前線段和前一線段找到交點
            if ((intIdxTurnPoint - 2 - 1 - 2) >= 0 && (intIdxTurnPoint + 1 + 2) < listMapPoint_ToUpdateGPS.Count)
            {
                int idxLine_1_FromPoint = intIdxTurnPoint - 2 - 1 - 2;  //轉彎點可能被誤認為實際的轉彎點的下一點，所以往前找一點比較保險. 再往後找一點 ，離路口比較遠比較準確
                int idxLine_1_ToPoint = intIdxTurnPoint - 1 - 1 - 2;//轉彎點可能被誤認為實際的轉彎點的下一點，所以往前找一點比較保險. 再往後找一點 ，離路口比較遠比較準確

                int idxLine_2_FromPoint = intIdxTurnPoint + 2;//往前找一點 ，離路口比較遠比較準確
                int idxLine_2_ToPoint = intIdxTurnPoint + 1 + 2;//往前找一點 ，離路口比較遠比較準確

                MapPoint pLine_1_FromPoint = listMapPoint_ToUpdateGPS[idxLine_1_FromPoint];
                MapPoint pLine_1_ToPoint = listMapPoint_ToUpdateGPS[idxLine_1_ToPoint];

                MapPoint pLine_2_FromPoint = listMapPoint_ToUpdateGPS[idxLine_2_FromPoint];
                MapPoint pLine_2_ToPoint = listMapPoint_ToUpdateGPS[idxLine_2_ToPoint];

                List<MapPoint> point_collection_1 = new List<MapPoint>
                        {
                            pLine_1_FromPoint,
                            pLine_1_ToPoint
                        };

                Polyline polyline_1 = PolylineBuilder.CreatePolyline(point_collection_1);//要用polyline不能用 line，否則Intersect會錯誤

                MapPoint point_extended_1_to = GeometryEngine.Instance.QueryPoint(polyline_1, SegmentExtension.ExtendTangents, _intUpdate_Time_Extend_line_length_m, AsRatioOrLength.AsLength);
                MapPoint point_extended_1_from = GeometryEngine.Instance.QueryPoint(polyline_1, SegmentExtension.ExtendTangents, -_intUpdate_Time_Extend_line_length_m, AsRatioOrLength.AsLength);

                point_collection_1 = new List<MapPoint>
                        {
                            point_extended_1_from,
                            pLine_1_FromPoint,
                            pLine_1_ToPoint,
                            point_extended_1_to
                        };

                polyline_1 = PolylineBuilder.CreatePolyline(point_collection_1);//要用polyline不能用 line，否則Intersect會錯誤

                List<MapPoint> point_collection_2 = new List<MapPoint>
                        {
                            pLine_2_FromPoint,
                            pLine_2_ToPoint
                        };

                Polyline polyline_2 = PolylineBuilder.CreatePolyline(point_collection_2);//要用polyline不能用 line，否則Intersect會錯誤

                MapPoint point_extended_2_from = GeometryEngine.Instance.QueryPoint(polyline_2, SegmentExtension.ExtendTangents, -_intUpdate_Time_Extend_line_length_m, AsRatioOrLength.AsLength);
                MapPoint point_extended_2_to = GeometryEngine.Instance.QueryPoint(polyline_2, SegmentExtension.ExtendTangents, _intUpdate_Time_Extend_line_length_m, AsRatioOrLength.AsLength);

                point_collection_2 = new List<MapPoint>
                        {
                            point_extended_2_from,
                            pLine_2_FromPoint,
                            pLine_2_ToPoint,
                            point_extended_2_to
                        };

                polyline_2 = PolylineBuilder.CreatePolyline(point_collection_2);//要用polyline不能用 line，否則Intersect會錯誤

                Geometry pGeo_intersect_line_1_2 = GeometryEngine.Instance.Intersection(polyline_1, polyline_2, GeometryDimension.esriGeometry0Dimension);

                Multipoint pPoints_intersect_line_1_2 = null;

                pPoints_intersect_line_1_2 = pGeo_intersect_line_1_2 as Multipoint;

                if (pPoints_intersect_line_1_2.PointCount >= 1)
                {
                    //   System.Diagnostics.Debug.WriteLine("??????????" + pPoints_intersect_line_1_2.Points[0].X + "" + pPoints_intersect_line_1_2.Points[0].Y);
                    mp_xy_cross = MapPointBuilder.CreateMapPoint(pPoints_intersect_line_1_2.Points[0].X, pPoints_intersect_line_1_2.Points[0].Y, SpatialReferences.WGS84);
                }

            }

            return mp_xy_cross;
        }

        public static bool GetExif2(string strFolderPath_in, out double out_dbl_direction_camara,
                                                           out string out_str_project_name,
                                                           out string out_str_調查員,
                                                           //out string out_str_調查修正,
                                                           out int out_int_day, out int out_int_hour)//, out int out_int_minute)
        {
            out_dbl_direction_camara = -1;
            out_str_project_name = "";
            out_str_調查員 = "";
            //out_str_調查修正 = "";
            out_int_day = 0;
            out_int_hour = 0;
            //   out_int_minute = 0;


            try
            {
                string strReadLine;
                int intReadCount = 0;

                //System.Diagnostics.Debug.WriteLine("$$$"+System.Text.Encoding.Default.HeaderName);

                System.IO.StreamReader file = new System.IO.StreamReader(strFolderPath_in + @"\" + StreetViewModule._strFileName_exif2, System.Text.Encoding.GetEncoding(950));//big5

                while ((strReadLine = file.ReadLine()) != null)
                {
                    strReadLine = strReadLine.Trim().ToLower();

                    if (strReadLine.StartsWith("dextrorotation:"))//ex.dextrorotation:90
                    {
                        string str_dextrorotation = strReadLine.Trim("dextrorotation:".ToCharArray());

                        bool blOk_parse = double.TryParse(str_dextrorotation, out out_dbl_direction_camara);

                        if (blOk_parse == false)
                        {
                            return false;
                        }
                    }
                    else if (strReadLine.StartsWith("project_name:"))//ex.project_name:(街景專案)高雄市路竹區
                    {
                        out_str_project_name = strReadLine.Trim("project_name:".ToCharArray());
                    }
                    else if (strReadLine.StartsWith("調查員:"))//ex.調查員:蘇學鎮
                    {
                        out_str_調查員 = strReadLine.Trim("調查員:".ToCharArray());
                    }
                    //else if (strReadLine.StartsWith("調查修正:"))//ex.調查修正:否
                    //{
                    //    out_str_調查修正 = strReadLine.Trim("調查修正:".ToCharArray());
                    //}
                    else if (strReadLine.StartsWith("修正時間:")) //ex.修正時間:0/0 ex.修正時間:46/2:13  --> 取消分鐘
                    {
                        string str_修正時間 = strReadLine.Trim("修正時間:".ToCharArray());

                        int int_idx_slash = str_修正時間.IndexOf("/");

                        if (int_idx_slash >= 0)
                        {
                            string[] array_shift_day_hour = str_修正時間.Split("/".ToCharArray());

                            if (array_shift_day_hour.Length == 2)
                            {
                                string str_day = array_shift_day_hour[0];
                                string str_hour = array_shift_day_hour[1];

                                bool blOk_parse = int.TryParse(str_day, out out_int_day);

                                if (blOk_parse == false)
                                {
                                    return false;
                                }

                                blOk_parse = int.TryParse(str_hour, out out_int_hour);

                                if (blOk_parse == false)
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    intReadCount++;
                }//   while ((strReadLine = file.ReadLine()) != null)

                file.Close();
            }
            catch (Exception ex)
            {
                return false;

            }

            return true;


        }

        public static bool addFields_featureLayer_PhotosToPoints(FeatureLayer featureLayer_PhotosToPoints)
        {
            //加st_dist欄位 --> 取消，改由圖霸處理
            //bool blOk = StreetViewModule.AddField(featureLayer_PhotosToPoints, StreetViewModule.strFieldName_PhotosToPoints_st_dist, "DOUBLE", "");

            //if (!blOk)
            //{
            //    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule.strFieldName_PhotosToPoints_st_dist + "]欄位發生錯誤 ");
            //    return;
            //}

            bool blOk = StreetViewModule.AddField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_Direction_camara, "DOUBLE", "");

            if (!blOk)
            {
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_Direction_camara + "]欄位發生錯誤 ");
                return false;
            }

            blOk = StreetViewModule.AddField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_jobid, "TEXT", "40");

            if (!blOk)
            {
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_jobid + "]欄位發生錯誤 ");
                return false;
            }

            blOk = StreetViewModule.AddField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_Project, "TEXT", "40");

            if (!blOk)
            {
               // ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_Project + "]欄位發生錯誤 ");
                return false;
            }

            blOk = StreetViewModule.AddField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_project_name, "TEXT", "40");

            if (!blOk)
            {
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_project_name + "]欄位發生錯誤 ");
                return false;
            }

            blOk = StreetViewModule.AddField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_investigator, "TEXT", "10");

            if (!blOk)
            {
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_investigator + "]欄位發生錯誤 ");
                return false;
            }

            return true;
        }

        public static bool calculateFields_featureLayer_PhotosToPoints(FeatureLayer featureLayer_PhotosToPoints,
                                            string strFolderPath_web_virtual_folder_pic_unc_to_record_db,
                                            mdlProcessItem pmdlProcessItem,
                                            string str_project_name,
                                            string str_調查員,
                                            double dbl_direction_camara,
                                            int int_day,
                                            int int_hour,
                                            out string outErrorFieldName,
                                            out string outErrorExpression)
        {
            outErrorFieldName = null;
            outErrorExpression = null;

            //field calculate-Path
            List<IGPResult> list_output_GpResult = new List<IGPResult>();
            List<string> list_output_ErrorMsg = new List<string>();

            string strExp = "'" + strFolderPath_web_virtual_folder_pic_unc_to_record_db + "'";
            string strCodeBlock = string.Empty;
            GpTools.CalculateField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_Path, strExp, "Arcade", strCodeBlock,
            list_output_GpResult,
            list_output_ErrorMsg);

            if (list_output_GpResult.Count == 1)
            {
                IGPResult gpResult = list_output_GpResult[0];

                if (gpResult.IsFailed == true)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + featureLayer_PhotosToPoints.Name + " field:" + StreetViewModule._strFieldName_PhotosToPoints_Path + " exp:" + strExp + " code block:" + strCodeBlock);
                    outErrorFieldName = StreetViewModule._strFieldName_PhotosToPoints_Path;
                    outErrorExpression = strExp;
                    return false;
                }
            }


            //field calculate-jobid
            list_output_GpResult = new List<IGPResult>();
            list_output_ErrorMsg = new List<string>();

            strExp = "'" + pmdlProcessItem._str_jobid + "'";
            strCodeBlock = string.Empty;
            GpTools.CalculateField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_jobid, strExp, "Arcade", strCodeBlock,
            list_output_GpResult,
            list_output_ErrorMsg);

            if (list_output_GpResult.Count == 1)
            {
                IGPResult gpResult = list_output_GpResult[0];

                if (gpResult.IsFailed == true)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + featureLayer_PhotosToPoints.Name + " field:" + StreetViewModule._strFieldName_PhotosToPoints_jobid + " exp:" + strExp + " code block:" + strCodeBlock);
                    outErrorFieldName = StreetViewModule._strFieldName_PhotosToPoints_jobid;
                    outErrorExpression = strExp;
                    return false;
                }
            }

            //field calculate-Project
            list_output_GpResult = new List<IGPResult>();
            list_output_ErrorMsg = new List<string>();

            strExp = "'" + str_project_name + "'";
            strCodeBlock = string.Empty;
            GpTools.CalculateField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_Project, strExp, "Arcade", strCodeBlock,
            list_output_GpResult,
            list_output_ErrorMsg);

            if (list_output_GpResult.Count == 1)
            {
                IGPResult gpResult = list_output_GpResult[0];

                if (gpResult.IsFailed == true)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + featureLayer_PhotosToPoints.Name + " field:" + StreetViewModule._strFieldName_PhotosToPoints_Project + " exp:" + strExp + " code block:" + strCodeBlock);
                    outErrorFieldName = StreetViewModule._strFieldName_PhotosToPoints_Project;
                    outErrorExpression = strExp;
                    return false;
                }
            }

            //field calculate-project_name
            list_output_GpResult = new List<IGPResult>();
            list_output_ErrorMsg = new List<string>();

            strExp = "'" + str_project_name + "'";
            strCodeBlock = string.Empty;
            GpTools.CalculateField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_project_name, strExp, "Arcade", strCodeBlock,
            list_output_GpResult,
            list_output_ErrorMsg);

            if (list_output_GpResult.Count == 1)
            {
                IGPResult gpResult = list_output_GpResult[0];

                if (gpResult.IsFailed == true)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + featureLayer_PhotosToPoints.Name + " field:" + StreetViewModule._strFieldName_PhotosToPoints_project_name + " exp:" + strExp + " code block:" + strCodeBlock);
                    outErrorFieldName = StreetViewModule._strFieldName_PhotosToPoints_project_name;
                    outErrorExpression = strExp; 
                    return false;
                }
            }

            //field calculate-調查員
            list_output_GpResult = new List<IGPResult>();
            list_output_ErrorMsg = new List<string>();

            strExp = "'" + str_調查員 + "'";
            strCodeBlock = string.Empty;
            GpTools.CalculateField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_investigator, strExp, "Arcade", strCodeBlock,
            list_output_GpResult,
            list_output_ErrorMsg);

            if (list_output_GpResult.Count == 1)
            {
                IGPResult gpResult = list_output_GpResult[0];

                if (gpResult.IsFailed == true)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + featureLayer_PhotosToPoints.Name + " field:" + StreetViewModule._strFieldName_PhotosToPoints_investigator + " exp:" + strExp + " code block:" + strCodeBlock);
                    outErrorFieldName = StreetViewModule._strFieldName_PhotosToPoints_investigator;
                    outErrorExpression = strExp; 
                    return false;
                }
            }

            //field calculate-Direction_camara
            list_output_GpResult = new List<IGPResult>();
            list_output_ErrorMsg = new List<string>();

            strExp = dbl_direction_camara.ToString();
            strCodeBlock = string.Empty;
            GpTools.CalculateField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_Direction_camara, strExp, "Arcade", strCodeBlock,
            list_output_GpResult,
            list_output_ErrorMsg);

            if (list_output_GpResult.Count == 1)
            {
                IGPResult gpResult = list_output_GpResult[0];

                if (gpResult.IsFailed == true)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + featureLayer_PhotosToPoints.Name + " field:" + StreetViewModule._strFieldName_PhotosToPoints_Direction_camara + " exp:" + strExp + " code block:" + strCodeBlock);
                    outErrorFieldName = StreetViewModule._strFieldName_PhotosToPoints_Direction_camara;
                    outErrorExpression = strExp; 
                    return false;
                }
            }

            //field calculate-調整拍照時間(小時)
            int intTotalHours = int_day * 24 + int_hour;

            list_output_GpResult = new List<IGPResult>();
            list_output_ErrorMsg = new List<string>();

            strExp = "!" + StreetViewModule._strFieldName_PhotosToPoints_DateTime + "! + " + "datetime.timedelta(hours = " + intTotalHours + ")";
            strCodeBlock = string.Empty;
            GpTools.CalculateField(featureLayer_PhotosToPoints, StreetViewModule._strFieldName_PhotosToPoints_DateTime, strExp, "Python 3", strCodeBlock,
            list_output_GpResult,
            list_output_ErrorMsg);

            if (list_output_GpResult.Count == 1)
            {
                IGPResult gpResult = list_output_GpResult[0];

                if (gpResult.IsFailed == true)
                {
                    //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("CalculateField處理發生錯誤: " + gpResult.ErrorCode.ToString() + featureLayer_PhotosToPoints.Name + " field:" + StreetViewModule._strFieldName_PhotosToPoints_Direction_camara + " exp:" + strExp + " code block:" + strCodeBlock);
                    outErrorFieldName = StreetViewModule._strFieldName_PhotosToPoints_DateTime;
                    outErrorExpression = strExp; 
                    return false;
                }
            }

            return true;
        }
    }
}
