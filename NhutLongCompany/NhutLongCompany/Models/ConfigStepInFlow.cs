using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Models
{
  
    public class ConfigStepInFlow
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        public List<tbl_Config_FlowName> ListFlow { get; set; }
        public List<tbl_Config_Step> ListStep { get; set; }
        public List<tbl_Config_StepInFlow> ListConfig { get; set; }

        public bool CheckExists(int idflow,int idstep)
        {
           List<tbl_Config_StepInFlow> list = db.tbl_Config_StepInFlow.Where(T => T.ID_Flow == idflow && T.ID_Step == idstep && T.TrangThai==1).ToList();
           if (list.Count>0)
           {
               return true;
           }
            return false;
        }
    }
}