using ArcGIS.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetViewPictureProcessorAddin
{
    public class mdlProcessItem
    {
        public long _lngOId = -1;
        public string _strFolderPath_in = null;
        public string _str_jobid = null;
        public Row _row_job = null;

        public mdlProcessItem(Row aRow,string aJobid)
        {
            _row_job = aRow;
            _str_jobid = aJobid;
            _lngOId = aRow.GetObjectID();

            _strFolderPath_in =StreetViewModule._strFolderPath_upload_root + @"\" + _str_jobid;
        }
    }
}
