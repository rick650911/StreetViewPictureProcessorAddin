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
using UtilityClassLibrary;

namespace StreetViewPictureProcessorAddin
{
    internal class btnGpsCorrect : Button
    {
        protected override async void OnClick()
        {
            //RasterlizeManager._CancelableProgressorSource = new ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource("街景照片處理", "取消");
            //RasterlizeManager._CancelableProgressorSource.Progressor.Max = (uint)100;

            await QueuedTask.Run(async () =>
            {
                //Check for an active mapview, if not, then prompt and exit.
                if (MapView.Active == null)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("沒有作用中的地圖. 離開...", "Info");
                    return;
                }

                // Get the layer(s) selected in the Contents pane, if there is not just one, then prompt then exit.
                if (MapView.Active.GetSelectedLayers().Count != 1)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇1個向量點圖層. 離開...", "Info");
                    return;
                }

                // Check to see if the selected layer is a feature layer, if not, then prompt and exit.
                FeatureLayer featurelayer_point_map8 = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

                if (featurelayer_point_map8 == null)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇向量點圖層. 離開...", "Info");
                    return;
                }

                //找出轉彎點
                //加記錄轉彎角度欄位
                bool blOk = StreetViewModule.AddField(featurelayer_point_map8, StreetViewModule._strFieldName_PhotosToPoints_turn_angle, "DOUBLE", "");

                if (!blOk)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_turn_angle + "]欄位發生錯誤 ");
                    return;
                }

                blOk = StreetViewModule.AddField(featurelayer_point_map8, StreetViewModule._strFieldName_PhotosToPoints_X2, "DOUBLE", "");

                if (!blOk)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_X2 + "]欄位發生錯誤 ");
                    return;
                }

                blOk = StreetViewModule.AddField(featurelayer_point_map8, StreetViewModule._strFieldName_PhotosToPoints_Y2, "DOUBLE", "");

                if (!blOk)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_Y2 + "]欄位發生錯誤 ");
                    return;
                }

                //blOk = StreetViewModule.AddField(featurelayer_point_map8, StreetViewModule._strFieldName_PhotosToPoints_d_Y_Y1, "DOUBLE", "");

                //if (!blOk)
                //{
                //    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_d_Y_Y1 + "]欄位發生錯誤 ");
                //    return;

                //}

                //blOk = StreetViewModule.AddField(featurelayer_point_map8, StreetViewModule._strFieldName_PhotosToPoints_d_X_Y, "DOUBLE", "");

                //if (!blOk)
                //{
                //    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + StreetViewModule._strFieldName_PhotosToPoints_d_X_Y + "]欄位發生錯誤 ");
                //    return;

                //}

                List<mdlTurnPoint> listTurnPoint = new List<mdlTurnPoint>();

                QueryFilter pQf = new ArcGIS.Core.Data.QueryFilter();
                pQf.WhereClause = "";
                pQf.PostfixClause = "ORDER BY " + StreetViewModule._strFieldName_PhotosToPoints_DateTime;

                RowCursor rowCursor = featurelayer_point_map8.Search(pQf);

                //MapPoint p_pre = null;
                double dbl_dir_pre = -1;

                using (rowCursor)
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Feature pFeature_now = (Feature)rowCursor.Current)  //處理每一筆點位行進方向改變角度
                        {
                            // MapPoint p_now = (MapPoint)pFeature_now.GetShape();

                            try
                            {
                                object obj_dir = pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Direction];
                                double dbl_dir = -1;

                                bool blParse = double.TryParse(obj_dir.ToString(), out dbl_dir);

                                if (blParse && dbl_dir >= 0 && dbl_dir != 9999)//目前點有方向
                                {
                                    double dbl_dir_change = 0;

                                    if (dbl_dir_pre != -1)//有前一點的方向
                                    {
                                        dbl_dir_change = Math.Abs(dbl_dir_pre - dbl_dir);

                                        if (dbl_dir_change > 180)
                                        {
                                            dbl_dir_change = 360 - dbl_dir_change;
                                        }
                                    }
                                    //else 
                                    //{

                                    //}

                                    //if (pFeature_now.GetObjectID() ==98)//dbl_dir_change == 46)//temp  271(89) // 46  //96(264)  pFeature_now.GetObjectID()==122)//
                                    if (dbl_dir_change >= StreetViewModule._dblturn_angle_min &&
                                    dbl_dir_change <= StreetViewModule._dblturn_angle_max)
                                    {
                                        mdlTurnPoint pmdlTurnPoint = new mdlTurnPoint();
                                        pmdlTurnPoint._ObjectID = pFeature_now.GetObjectID();
                                        pmdlTurnPoint._dateTime = Convert.ToDateTime(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_DateTime]);
                                        pmdlTurnPoint._dblX = Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X]);
                                        pmdlTurnPoint._dblY = Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y]);
                                        pmdlTurnPoint._dblX1 = Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X1]);
                                        pmdlTurnPoint._dblY1 = Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y1]);

                                        listTurnPoint.Add(pmdlTurnPoint);
                                    }

                               

                                    pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_turn_angle] = dbl_dir_change;

                                    pFeature_now.Store();

                                    dbl_dir_pre = dbl_dir;
                                }

                                //temp  把xy 設為原始
                                //if (pFeature_now.GetObjectID() == 121)
                                //{
                                //    System.Diagnostics.Debug.WriteLine("121......"+Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X])+","+ Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y]));
                                //}

                                //MapPoint mp_xy = MapPointBuilder.CreateMapPoint(Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X]), Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y]), SpatialReferences.WGS84);

                                //pFeature_now.SetShape(mp_xy);

                                //pFeature_now.Store();

                                //temp end

                                //把X2Y2設為X1Y1
                                pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X2] = pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X1];
                                pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y2] = pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y1];

                                pFeature_now.Store();
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("以" + StreetViewModule._strFieldName_PhotosToPoints_Direction + "檢出轉彎點發生錯誤" + ex.Message);
                            }



                            




                            ////記錄每一點的xy修正量
                            //double dblX = Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X1]) - Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X]); ;
                            //double dblY = Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y1]) - Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y]);
                            //dblX = Math.Abs(dblX);
                            //dblY = Math.Abs(dblY);

                            //pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_d_X_X1] = dblX;
                            //pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_d_Y_Y1] = dblY;
                            //pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_d_X_Y] = Math.Sqrt(dblX * dblX + dblY * dblY);

                            //pFeature_now.Store();

                            //System.Diagnostics.Debug.WriteLine(obj_dir.ToString());

                        }
                    }
                }


              //  return;

                //轉彎點附近(15秒內)的點修正GPS-->X
                //同方向的點連成線，延伸取得轉彎點，由XY轉彎點和X1Y1轉彎點取得位移向量，轉彎點附近(15秒內)的點修正GPS
                foreach (mdlTurnPoint pmdlTurnPoint in listTurnPoint)
                {
                    try
                    {

                        DateTime dtTime_from = pmdlTurnPoint._dateTime.AddSeconds(-StreetViewModule._intUpdate_Time_Extent_pre_post_sec);
                        DateTime dtTime_to = pmdlTurnPoint._dateTime.AddSeconds(StreetViewModule._intUpdate_Time_Extent_pre_post_sec);

                        string strTime_from = dtTime_from.ToString("yyyy/MM/dd HH:mm:ss");
                        string strTime_to = dtTime_to.ToString("yyyy/MM/dd HH:mm:ss");

                        //double dblDelta_x = pmdlTurnPoint._dblX1 - pmdlTurnPoint._dblX;
                        //double dblDelta_y = pmdlTurnPoint._dblY1 - pmdlTurnPoint._dblY;

                        pQf = new ArcGIS.Core.Data.QueryFilter();

                        //DateTime >= '2021/9/29 16:41:30' And DateTime <='2021/9/29 16:42:00'
                        pQf.WhereClause = StreetViewModule._strFieldName_PhotosToPoints_DateTime + " >= timestamp'" + strTime_from + "' And " +
                                          StreetViewModule._strFieldName_PhotosToPoints_DateTime + " <= timestamp'" + strTime_to + "'";

                        pQf.PostfixClause = "ORDER BY " + StreetViewModule._strFieldName_PhotosToPoints_DateTime;

                        //以XY欄位的軌跡找到轉彎交叉點
                        rowCursor = featurelayer_point_map8.Search(pQf);

                        MapPoint pCross_XY = StreetViewModule.GetCrossPoint(rowCursor, StreetViewModule._strFieldName_PhotosToPoints_X, StreetViewModule._strFieldName_PhotosToPoints_Y);

                        if (pCross_XY == null)
                        {
                            continue;//沒有找到轉彎交叉點
                        }

                        //以X1Y1欄位的軌跡找到轉彎交叉點
                        rowCursor = featurelayer_point_map8.Search(pQf);

                        MapPoint pCross_X1Y1 = StreetViewModule.GetCrossPoint(rowCursor, StreetViewModule._strFieldName_PhotosToPoints_X1, StreetViewModule._strFieldName_PhotosToPoints_Y1);

                        if (pCross_X1Y1 == null)
                        {
                            continue;//沒有找到轉彎交叉點
                        }

                        double dblDelta_X = pCross_X1Y1.X - pCross_XY.X;
                        double dblDelta_Y = pCross_X1Y1.Y - pCross_XY.Y;

                        //修正GPS
                        rowCursor = featurelayer_point_map8.Search(pQf);

                        using (rowCursor)
                        {
                            while (rowCursor.MoveNext())
                            {
                                using (Feature pFeature_now = (Feature)rowCursor.Current)
                                {
                                    System.Diagnostics.Debug.WriteLine("修正…" + pmdlTurnPoint._ObjectID);

                                    double dblX = Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X]);
                                    double dblY = Convert.ToDouble(pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y]);

                                    dblX = dblX + dblDelta_X;
                                    dblY = dblY + dblDelta_Y;

                                    pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_X2] = dblX;
                                    pFeature_now[StreetViewModule._strFieldName_PhotosToPoints_Y2] = dblY;

                                    MapPoint mp_xy = MapPointBuilder.CreateMapPoint(dblX, dblY, SpatialReferences.WGS84);

                                    pFeature_now.SetShape(mp_xy);
                                    pFeature_now.Store();
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("同方向的點連成線，延伸取得轉彎點，由XY轉彎點和X1Y1轉彎點取得位移向量，轉彎點附近(15秒內)的點修正GPS處理發生錯誤" + ex.Message);
                    }

                }



            }/*, RasterlizeManager._CancelableProgressorSource.Progressor*/);
        }
    }
}
