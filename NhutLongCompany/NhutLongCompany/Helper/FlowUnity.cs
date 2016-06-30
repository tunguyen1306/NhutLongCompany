using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Antlr.Runtime.Tree;
using NhutLongCompany.Models;

namespace NhutLongCompany.Helper
{
    public class FlowUnity
    {
        private static NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        public static tbl_FlowPauseTime FlowPauseTime(int id, int idflow)
        {
            tbl_FlowPauseTime time = db.tbl_FlowPauseTime.Where(T => T.id_flow == idflow && T.baoGia_detail_id == id && T.status == 1).FirstOrDefault();
            return time;
        }
        public static tbl_QuyTrinh QuyTrinhByID(int idflow)
        {
            tbl_QuyTrinh flow = db.tbl_QuyTrinh.Find(idflow);
            return flow;
        }
        public static int getStatusNextStep(int index, List<tbl_QuyTrinh> listQyTrinh)
        {
            tbl_QuyTrinh cuurenQuyTrinh = listQyTrinh[index];
            tbl_QuyTrinh prevQuyTrinh = null;
            int indexQTFlowOne = -1;
            if (index > 0)
            {
                for (int jj = index - 1; jj >= 0; jj--)
                {
                    if (listQyTrinh[jj].SongSong == 0)
                    {
                        prevQuyTrinh = listQyTrinh[jj];
                        break;
                    }
                    else
                    {
                        indexQTFlowOne = jj;
                    }
                }
            }
            if (prevQuyTrinh==null)
            {
                return 2;
            }
            else
            {
                if (prevQuyTrinh.TrangThai==2)
                {
                    bool checkSuccess = true;
                    if (indexQTFlowOne == -1)
                    {
                        return 2;
                    }
                    else
                    {
                        for (int i = indexQTFlowOne; i < index; i++)
                        {
                            tbl_QuyTrinh itemTrinh = listQyTrinh[i];
                            if (itemTrinh.TrangThai != 2)
                            {
                                checkSuccess = false;
                                break;
                            }
                        }
                        if (checkSuccess || cuurenQuyTrinh.SongSong == 1)
                        {
                            return 2;
                        }
                        else
                        {
                            return 1;
                        }

                    }
                  
                }
                else
                {
                    if (indexQTFlowOne==-1)
                    {
                        return 1;
                    }
                    else
                    {
                        if (cuurenQuyTrinh.SongSong==1)
                        {
                            return 2;
                        }
                    }
             
                }
            }
           

            return 1;
        }
    }
}