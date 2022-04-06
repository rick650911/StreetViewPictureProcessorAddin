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
//    internal class x_btnRasterize : Button
//    {
//        protected override async void OnClick()
//        {
//            //string strCompleOrCancal = "完成";

//            RasterlizeManager._CancelableProgressorSource = new ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource("密度圖計算", "取消");
//            RasterlizeManager._CancelableProgressorSource.Progressor.Max = (uint)100;

//            await QueuedTask.Run(async () =>
//            {
//                // Check for an active mapview, if not, then prompt and exit.
//                if (MapView.Active == null)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("沒有作用中的地圖. 離開...", "Info");
//                    return;
//                }

//                if (MapView.Active.Map.SpatialReference == null ||
//                   (MapView.Active.Map.SpatialReference != null && MapView.Active.Map.SpatialReference.Wkid != 3826))
//                {
//                    //Name = "TWD_1997_TM_Taiwan"
//                    //Wkid = 3826
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("地圖坐標系統需為 TWD_1997_TM_Taiwan. 離開...", "Info");
//                    return;
//                }

//                // Get the layer(s) selected in the Contents pane, if there is not just one, then prompt then exit.
//                if (MapView.Active.GetSelectedLayers().Count != 1)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇一個向量面圖層. 離開...", "Info");
//                    return;
//                }
//                // Check to see if the selected layer is a feature layer, if not, then prompt and exit.
//                var featLayer_input = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

//                if (featLayer_input == null)
//                {
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("請於內容面版選擇一個向量面圖層. 離開...", "Info");
//                    return;
//                }

//                try
//                {
//                    ////產製1000公尺fishnet
//                    //string strOutFeatureClass = System.IO.Path.GetFullPath(@"D:\COA\地圖服務\arcgis pro project\rasterize\rasterize.gdb" + @"\twd97\abc_1000");
//                    //string strTemplateExtent = "147522.2188 2406567.9796 351690.1143 2799148.9622";
//                    //string strCellSize = "1000";

//                    //List<IGPResult> list_output_GpResult = new List<IGPResult>();
//                    //List<string> list_output_ErrorMsg = new List<string>();

//                    //GpTools.CreateFishnet(strOutFeatureClass,
//                    //                           strCellSize,
//                    //                           strTemplateExtent,
//                    //                          list_output_GpResult,
//                    //                           list_output_ErrorMsg);



//                    string strFieldName_權重 = RasterlizeManager._strWeightField.Trim();// "權重";


//                    ////用input layer extent產製 fishnet (1000 200 40m)
//                    //Envelope pEnv_input = featLayer_input.QueryExtent();

//                    //用台澎金馬範圍曘25k fishnet
//                    double pEnv_inputXMin = -36116.6512573572;
//                    double pEnv_inputXMax = 345858.141456109;
//                    double pEnv_inputYMin = 2424556.04058704;
//                    double pEnv_inputYMax = 2897248.58387447;

//                    //1.產製n公尺fishnet
//                    //string strOutFeatureClass_fishnet = System.IO.Path.GetFullPath(@"D:\COA\地圖服務\arcgis pro project\rasterize\rasterize.gdb" + @"\twd97\abc_200");
//                    double dblCellSize = double.Parse(RasterlizeManager._strFishnet_size);// 1000;
//                    string strCellSize = dblCellSize.ToString();
//                    string strCellSize_Largest = "25000";//25k

//                    string strLayerName_1_fishnet = "fishnet_1_" + strCellSize_Largest;// RasterlizeManager._strFishnet_size;
//                    //FeatureLayer featureLayer_1_fishnet=null;

//                    //對齊地政司dtm extent的左下角
//                    double dblPrvot_XMin = 214530.0000001;
//                    double dblPrvot_YMin = 2701129.9999974;

//                    //找出要shift fishnet的距離
//                    double dblDiff_X = Math.Abs(dblPrvot_XMin - pEnv_inputXMin) % 20;  //如果要建的fishnet的extent左下角坐標x > 對齊地政司dtm extent的左下角x坐標
//                    double dblDiff_Y = Math.Abs(dblPrvot_YMin - pEnv_inputYMin) % 20;  //如果要建的fishnet的extent左下角坐標y > 對齊地政司dtm extent的左下角y坐標


//                    if (dblPrvot_XMin > pEnv_inputXMin)//如果要建的fishnet的extent左下角坐標x < 對齊地政司dtm extent的左下角x坐標
//                    {
//                        dblDiff_X = 20 - dblDiff_X;
//                    }

//                    if (dblPrvot_YMin > pEnv_inputYMin)//如果要建的fishnet的extent左下角坐標y < 對齊地政司dtm extent的左下角y坐標
//                    {
//                        dblDiff_Y = 20 - dblDiff_Y;
//                    }

//                    string strTemplateExtent = (pEnv_inputXMin - dblDiff_X).ToString() + " " + (pEnv_inputYMin - dblDiff_Y).ToString() + " "
//                                                + pEnv_inputXMax.ToString() + " " + pEnv_inputYMax.ToString();


//                    List<IGPResult> list_output_GpResult = new List<IGPResult>();
//                    List<string> list_output_ErrorMsg = new List<string>();

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }

//                    GpTools.CreateFishnet(strLayerName_1_fishnet,
//                                           strCellSize_Largest,
//                                               strTemplateExtent,
//                                              list_output_GpResult,
//                                               list_output_ErrorMsg);


//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("產製 fishnet 發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }

//                        //featureLayer_1_fishnet = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == strLayerName_1_fishnet).FirstOrDefault() as FeatureLayer;

//                        //if (featureLayer_1_fishnet == null)
//                        //{
//                        //    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("取得fishnet圖層失敗: ");
//                        //    return;
//                        //}
//                    }

//                    //依選擇大小分割fishne處理-------------------------------------
//                    double dblCellSize_divid = 25000;

//                    for (int i = 0; i < 4; i++)//分25k 5k 1k 200m 40m分割網格
//                    {
//                        dblCellSize_divid = dblCellSize_divid / 5;

//                        if (dblCellSize > dblCellSize_divid)
//                        {
//                            break;
//                        }

//                        string strLayerName_1_fishnet_div_h = "fishnet_1_" + dblCellSize_divid.ToString() + "_h";
//                        string strLayerName_1_fishnet_div_v = "fishnet_1_" + dblCellSize_divid.ToString() + "_v";

//                        //依input 圖資圍圍選取fishnet
//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                            return;
//                        }

//                        GpTools.SelectLayerByLocation(strLayerName_1_fishnet,
//                                                 featLayer_input,
//                                                 "INTERSECT",
//                                                 "0 Meters",
//                                                 list_output_GpResult,
//                                                 list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("select by location (fishnet)發生錯誤: " + gpResult.ErrorCode.ToString());
//                                return;
//                            }
//                        }

//                        //水平分割
//                        // string strLayerName_1_fishnet_div_h = strLayerName_1_fishnet + "_div_h";

//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                            return;
//                        }

//                        GpTools.SubdividePolygon(strLayerName_1_fishnet,
//                                                 strLayerName_1_fishnet_div_h,
//                                                 5,
//                                                 0,
//                                                 list_output_GpResult,
//                                                 list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("分割 fishnet 發生錯誤: " + gpResult.ErrorCode.ToString());
//                                return;
//                            }
//                        }

//                        //垂直分割
//                        //string strLayerName_1_fishnet_div_v = strLayerName_1_fishnet + "_div_v";

//                        list_output_GpResult = new List<IGPResult>();
//                        list_output_ErrorMsg = new List<string>();

//                        if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                            return;
//                        }

//                        GpTools.SubdividePolygon(strLayerName_1_fishnet_div_h,
//                                                 strLayerName_1_fishnet_div_v,
//                                                 5,
//                                                 90,
//                                                 list_output_GpResult,
//                                                 list_output_ErrorMsg);

//                        if (list_output_GpResult.Count == 1)
//                        {
//                            IGPResult gpResult = list_output_GpResult[0];

//                            if (gpResult.IsFailed == true)
//                            {
//                                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("分割 fishnet 發生錯誤: " + gpResult.ErrorCode.ToString());
//                                return;
//                            }
//                        }

//                        strLayerName_1_fishnet = strLayerName_1_fishnet_div_v;



//                    }//for (int i=0;i<4 ;i++ )//分25k 5k 1k 200m 40m分割網格

//                    //end 依選擇大小切割fishne處理-------------------------------------

//                    //2.輸入圖層先作 dissolve 
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    string strLayerName_2_dis = featLayer_input.Name + "_2_dis";

//                    List<string> listDis_Fields;//= new List<string>() { strFieldName_權重 };
//                    //List<List<string>> listSta_Fields;
//                    List<string> listSta_Fields;

//                    if (strFieldName_權重.Equals(string.Empty))
//                    {
//                        listDis_Fields = new List<string>();
//                    }
//                    else
//                    {
//                        listDis_Fields = new List<string>() { strFieldName_權重 };
//                    }

//                    //listSta_Fields = new List<List<string>>();
//                    listSta_Fields = new List<string>();

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }

//                    GpTools.Dissolve(featLayer_input,
//                                     strLayerName_2_dis,// @"D:\COA\地圖服務\arcgis pro project\rasterize\rasterize.gdb" + @"\twd97\merge_test_dis",
//                                     listDis_Fields,
//                                     listSta_Fields,
//                                       "MULTI_PART",
//                                     list_output_GpResult,
//                                     list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("輸入圖層 dissolve 發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    //3.輸入圖層與fishnet交集運算
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    var strLayers_instersect = new List<string>() { strLayerName_1_fishnet, strLayerName_2_dis };

//                    string strLayerName_3_int = featLayer_input.Name + "_3_int";

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }

//                    GpTools.Intersect(strLayers_instersect,
//                             strLayerName_3_int,// @"D:\COA\地圖服務\arcgis pro project\rasterize\rasterize.gdb" + @"\twd97\merge_test_dis",

//                             list_output_GpResult,
//                             list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("輸入圖層與fishnet交集運算發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    //4.同個fishnet ID dissolve(將同個fishnet網格交集的面圖徵融合)
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    string strLayerName_4_int_dis = featLayer_input.Name + "_4_int_dis";

//                    string FieldName_FID_fishnetLayeName = "FID_" + strLayerName_1_fishnet;


//                    if (strFieldName_權重.Equals(string.Empty))
//                    {
//                        listDis_Fields = new List<string>() { FieldName_FID_fishnetLayeName };
//                    }
//                    else
//                    {
//                        listDis_Fields = new List<string>() { FieldName_FID_fishnetLayeName, strFieldName_權重 };
//                    }

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }

//                    GpTools.Dissolve(strLayerName_3_int,
//                                     strLayerName_4_int_dis,// @"D:\COA\地圖服務\arcgis pro project\rasterize\rasterize.gdb" + @"\twd97\merge_test_dis",
//                                     listDis_Fields,
//                                     listSta_Fields,
//                                     "MULTI_PART",
//                                     list_output_GpResult,
//                                     list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("同個fishnet ID dissolve(將同個fishnet網格交集的面圖徵融合)發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }



//                    //5. input layer add join fishnet
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }

//                    GpTools.AddJoin(strLayerName_4_int_dis,
//                                    FieldName_FID_fishnetLayeName,// "OID",//"OBJECTID",// @"D:\COA\地圖服務\arcgis pro project\rasterize\rasterize.gdb" + @"\twd97\merge_test_dis",
//                                     strLayerName_1_fishnet,
//                                     "OBJECTID",//OID
//                                     list_output_GpResult,
//                                     list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("input layer add join fishnet 發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    //6.input layer 新增面積比欄位
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }

//                    GpTools.AddField(strLayerName_4_int_dis, "面積比", "DOUBLE",
//                                "", "", "",
//                               "面積比", "NULLABLE",
//                               list_output_GpResult,
//                               list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("fishnet 新增面積比欄位發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    //7.面積比欄位 calculate field
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    double dblfishnet_area = dblCellSize * dblCellSize;

//                    string strExp;//= "getClass(float(!" + strLayerName_4_int_dis + ".Shape_Area!),float(!" + strLayerName_4_int_dis + ".權重!))";// "!Merge_test_4_int_dis.Shape_Area!/" + dblfishnet_area.ToString();
//                    string strCodeBlock = string.Empty;// = "def getClass(area,weight):\r\n"
//                                                       //+ "    if area >= 0:\r\n"
//                                                       //+ "        return area*weight/" + dblfishnet_area .ToString()+ "\r\n"
//                                                       //+ "    else:\r\n"
//                                                       //+ "        return 0\r\n";

//                    if (strFieldName_權重.Equals(string.Empty))
//                    {
//                        //strExp = "getClass(float(!" + strLayerName_4_int_dis + ".Shape_Area!))";// "!Merge_test_4_int_dis.Shape_Area!/" + dblfishnet_area.ToString();
//                        //strCodeBlock = "def getClass(area):\r\n"
//                        //                        + "    if area >= 0:\r\n"
//                        //                        + "        return area/" + dblfishnet_area.ToString() + "\r\n"
//                        //                        + "    else:\r\n"
//                        //                        + "        return 0\r\n";

//                        strExp = "$feature['" + strLayerName_4_int_dis + ".Shape_Area']/ $feature['" + strLayerName_1_fishnet + ".Shape_Area']";
//                    }
//                    else
//                    {
//                        //strExp = "getClass(float(!" + strLayerName_4_int_dis + ".Shape_Area!),float(!" + strLayerName_4_int_dis + ".權重!))";// "!Merge_test_4_int_dis.Shape_Area!/" + dblfishnet_area.ToString();
//                        //strCodeBlock = "def getClass(area,weight):\r\n"
//                        //                        + "    if area >= 0:\r\n"
//                        //                        + "        return area*weight/" + dblfishnet_area.ToString() + "\r\n"
//                        //                        + "    else:\r\n"
//                        //                        + "        return 0\r\n";
//                        strExp = "$feature['" + strLayerName_4_int_dis + ".Shape_Area'] * $feature['" + strLayerName_4_int_dis + "." + strFieldName_權重 + "'] / $feature['" + strLayerName_1_fishnet + ".Shape_Area']";
//                    }

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }

//                    GpTools.CalculateField(strLayerName_4_int_dis, "面積比", strExp, "Arcade", strCodeBlock,
//                                  list_output_GpResult,
//                                     list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("面積比欄位 calculate field 發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    //remove join 
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }
//                    GpTools.RemoveJoin(strLayerName_4_int_dis,
//                                    strLayerName_1_fishnet,
//                                     list_output_GpResult,
//                                     list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("fishnet remove join input layer 發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    //同個fishnet ID dissolve(將同個fishnet網格中面圖徵融合)
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    string strLayerName_5_int_dis_dis = featLayer_input.Name + "_5_int_dis_dis";

//                    listDis_Fields = new List<string>() { FieldName_FID_fishnetLayeName };

//                    listSta_Fields = new List<string>();
//                    listSta_Fields.Add("面積比 SUM");//統計欄位名稱 統計類型

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }
//                    GpTools.Dissolve(strLayerName_4_int_dis,
//                                     strLayerName_5_int_dis_dis,
//                                     listDis_Fields,
//                                     listSta_Fields,//listSta_Fields,
//                                     "MULTI_PART",
//                                     list_output_GpResult,
//                                     list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("同個fishnet ID dissolve(將同個fishnet網格中的面圖徵融合)發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    //取小數n位四捨五入
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    //if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    //{
//                    //    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                    //    return;
//                    //}

//                    int intDecimalPointDigitNum = int.Parse(RasterlizeManager._strDecimalPointDigitNum);// 0,1,2,3,4;

//                    int intRoundDigits = 2 + intDecimalPointDigitNum;
//                    double dblIgnore = 1;

//                    if (intRoundDigits > 2)
//                    {
//                        int intDiffDigits = intRoundDigits - 2;

//                        dblIgnore = dblIgnore / (intDiffDigits * 10);
//                    }

//                    strExp = "round_x100(!SUM_面積比!)";
//                    strCodeBlock = "def round_x100(SUM_面積比):\r\n" +
//                                   "    x100 = round(SUM_面積比, " + intRoundDigits.ToString() + ") * 100\r\n" +
//                                   "    if x100 < " + dblIgnore.ToString() + ":\r\n" +
//                                   "        return None\r\n" +
//                                   "    else:\r\n" +
//                                   "        return x100";

//                    GpTools.CalculateField(strLayerName_5_int_dis_dis, "SUM_面積比", strExp, "Python 3", strCodeBlock,
//                                           list_output_GpResult,
//                                           list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("SUM_面積比欄位 calculate field 發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }



//                    //被join 圖層欄位加index, 否則會超級慢
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    List<string> pFields = new List<string>();
//                    pFields.Add(FieldName_FID_fishnetLayeName);

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }
//                    GpTools.AddIndex(strLayerName_5_int_dis_dis, pFields,
//                                         list_output_GpResult,
//                                         list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("交集結果圖層加index發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }


//                    //fishnet add join input layer
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }
//                    GpTools.AddJoin(strLayerName_1_fishnet,
//                                    "OBJECTID",// "OID",//"OBJECTID",// @"D:\COA\地圖服務\arcgis pro project\rasterize\rasterize.gdb" + @"\twd97\merge_test_dis",
//                                     strLayerName_5_int_dis_dis,
//                                     FieldName_FID_fishnetLayeName,
//                                     list_output_GpResult,
//                                     list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("fishnet add join 交集結果圖層發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    ////export add join to featureclass, for perfomance
//                    //list_output_GpResult = new List<IGPResult>();
//                    //list_output_ErrorMsg = new List<string>();

//                    //string strLayerName_6_fishnet_add_join_exp = "fishnet_6_" + strCellSize +"_add_join_exp";
//                    //GpTools.FeatureClassToFeatureClass(strLayerName_1_fishnet,
//                    //                              Project.Current.DefaultGeodatabasePath,
//                    //                              strLayerName_6_fishnet_add_join_exp,
//                    //                              "1=1",
//                    //                              list_output_GpResult,
//                    //                              list_output_ErrorMsg);

//                    //if (list_output_GpResult.Count == 1)
//                    //{
//                    //    IGPResult gpResult = list_output_GpResult[0];

//                    //    if (gpResult.IsFailed == true)
//                    //    {
//                    //        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("fishnet export 發生錯誤: " + gpResult.ErrorCode.ToString());
//                    //        return;
//                    //    }
//                    //}

//                    //8.polygon to raster
//                    list_output_GpResult = new List<IGPResult>();
//                    list_output_ErrorMsg = new List<string>();

//                    string strLayerName_6_polygon2raster = strLayerName_1_fishnet + "_6_poygon2raster";

//                    if (RasterlizeManager._CancelableProgressorSource.Progressor.CancellationToken.IsCancellationRequested)
//                    {
//                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("密度圖計算" + "取消", "資訊");
//                        return;
//                    }
//                    GpTools.PolygonToRaster(strLayerName_1_fishnet, strLayerName_5_int_dis_dis + ".SUM_面積比", strLayerName_6_polygon2raster,
//                    //GpTools.PolygonToRaster(strLayerName_6_fishnet_add_join_exp,  "SUM_面積比", strLayerName_7_polygon2raster,
//                                 "Cell center", "NONE", "40",
//                                  list_output_GpResult,
//                                  list_output_ErrorMsg);

//                    if (list_output_GpResult.Count == 1)
//                    {
//                        IGPResult gpResult = list_output_GpResult[0];

//                        if (gpResult.IsFailed == true)
//                        {
//                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("polygon to raster發生錯誤: " + gpResult.ErrorCode.ToString());
//                            return;
//                        }
//                    }

//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("運算完成.");

//                }
//                catch (Exception exc)
//                {
//                    // Catch any exception found and display in a message box
//                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
//                    return;
//                }
//            }, RasterlizeManager._CancelableProgressorSource.Progressor);
//        }
//    }
//}
