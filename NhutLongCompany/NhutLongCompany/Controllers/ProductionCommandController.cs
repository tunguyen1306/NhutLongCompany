using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Attribute;
using NhutLongCompany.Models;

namespace NhutLongCompany.Controllers
{
    [RedirectOnError]
    public class ProductionCommandController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        //
        // GET: /ProductionCommand/
        public ActionResult Edit(int? id,double? khoGiay,int? buhao)
        {
          
         
            ProductionCommand command=new ProductionCommand();
            command.Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            command.Product = db.tbl_Products.Find(command.Detail.sanpam_id);
            command.Command = db.tbl_ProductionCommand.Where(T => T.product_id == command.Product.ID_Products).FirstOrDefault();
            if (command.Command==null)
            {
                if (khoGiay == null)
                {
                    khoGiay = 1;
                }
                if (buhao == null)
                {
                    buhao = 0;
                }
                command.Command = new tbl_ProductionCommand { KhoGiay = khoGiay.HasValue ? khoGiay.Value :1 , NgayLap = DateTime.Now, BuHao = buhao.HasValue?buhao.Value: 0};

                String[] quyCach = command.Product.QuyCachProducts.Trim().Split('x').Where(T => T.Trim().Length > 0).Select(T => T.Trim()).ToArray();

                System.Nullable<double> D = (quyCach.Length == 2 ? double.Parse(quyCach[0]) : ((double.Parse(quyCach[0]) + double.Parse(quyCach[1])) * 2 + 5));

                System.Nullable<double> R1 = ((double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) % 5 == 0 ? (((double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) / 5 * 5) : (((double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) / 5 * 5 + 5);
                
                System.Nullable<double> R = quyCach.Length == 3 ? ((double.Parse(quyCach[0]) + double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) % 5 == 0 ? (((double.Parse(quyCach[0]) + double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) / 5 * 5) : (((double.Parse(quyCach[0]) + double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) / 5 * 5 + 5) : R1;
               
                String[] loaiGiay = command.Product.LoaigiayProducts.Trim().Split('-').Where(T => T.Trim().Length > 0).Select(T => T.Trim()).ToArray();

                command.Command.Loai7 = loaiGiay[loaiGiay.Length - 1];
                command.Command.Loai1 = loaiGiay.Length > 0 && (0 < loaiGiay.Length - 1) ?loaiGiay[0]:"";
                command.Command.Loai2 = loaiGiay.Length > 1 && (1 < loaiGiay.Length - 1) ? loaiGiay[1]:"";
                command.Command.Loai3 =  loaiGiay.Length > 2 && (2 < loaiGiay.Length - 1) ?loaiGiay[2]:"";
                command.Command.Loai4 =  loaiGiay.Length > 3 && (3 < loaiGiay.Length - 1) ?loaiGiay[3]:"";
                command.Command.Loai5 =  loaiGiay.Length > 4 && (4 < loaiGiay.Length - 1) ?loaiGiay[4]:"";
                command.Command.Loai6 = loaiGiay.Length > 5 && (5 < loaiGiay.Length - 1) ? loaiGiay[5] : "";

                command.Command.Dai = D;
                command.Command.Rong = R;

                command.Command.KhoCuon7 = command.Command.Rong;
                command.Command.KhoCuon1 = loaiGiay.Length > 0 && (0 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                command.Command.KhoCuon2 = loaiGiay.Length > 1 && (1 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                command.Command.KhoCuon3 = loaiGiay.Length > 2 && (2 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                command.Command.KhoCuon4 = loaiGiay.Length > 3 && (3 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                command.Command.KhoCuon5 = loaiGiay.Length > 4 && (4 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                command.Command.KhoCuon6 = loaiGiay.Length > 5 && (5 < loaiGiay.Length - 1) ? command.Command.Rong : null;

            }
            else
            {
                if (khoGiay != null && buhao!=null)
                {
                    command.Command.KhoGiay = khoGiay;
                    command.Command.BuHao = buhao;
                    String[] quyCach = command.Product.QuyCachProducts.Trim().Split('x').Where(T => T.Trim().Length > 0).Select(T => T.Trim()).ToArray();

                    System.Nullable<double> D = (quyCach.Length == 2 ? double.Parse(quyCach[0]) : ((double.Parse(quyCach[0]) + double.Parse(quyCach[1])) * 2 + 5));

                    System.Nullable<double> R1 = ((double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) % 5 == 0 ? (((double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) / 5 * 5) : (((double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) / 5 * 5 + 5);

                    System.Nullable<double> R = quyCach.Length == 3 ? ((double.Parse(quyCach[0]) + double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) % 5 == 0 ? (((double.Parse(quyCach[0]) + double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) / 5 * 5) : (((double.Parse(quyCach[0]) + double.Parse(quyCach[1]) + 1) * command.Command.KhoGiay) / 5 * 5 + 5) : R1;

                    String[] loaiGiay = command.Product.LoaigiayProducts.Trim().Split('-').Where(T => T.Trim().Length > 0).Select(T => T.Trim()).ToArray();

                    command.Command.Loai7 = loaiGiay[loaiGiay.Length - 1];
                    command.Command.Loai1 = loaiGiay.Length > 0 && (0 < loaiGiay.Length - 1) ? loaiGiay[0] : "";
                    command.Command.Loai2 = loaiGiay.Length > 1 && (1 < loaiGiay.Length - 1) ? loaiGiay[1] : "";
                    command.Command.Loai3 = loaiGiay.Length > 2 && (2 < loaiGiay.Length - 1) ? loaiGiay[2] : "";
                    command.Command.Loai4 = loaiGiay.Length > 3 && (3 < loaiGiay.Length - 1) ? loaiGiay[3] : "";
                    command.Command.Loai5 = loaiGiay.Length > 4 && (4 < loaiGiay.Length - 1) ? loaiGiay[4] : "";
                    command.Command.Loai6 = loaiGiay.Length > 5 && (5 < loaiGiay.Length - 1) ? loaiGiay[5] : "";

                    command.Command.Dai = D;
                    command.Command.Rong = R;

                    command.Command.KhoCuon7 = command.Command.Rong;
                    command.Command.KhoCuon1 = loaiGiay.Length > 0 && (0 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                    command.Command.KhoCuon2 = loaiGiay.Length > 1 && (1 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                    command.Command.KhoCuon3 = loaiGiay.Length > 2 && (2 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                    command.Command.KhoCuon4 = loaiGiay.Length > 3 && (3 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                    command.Command.KhoCuon5 = loaiGiay.Length > 4 && (4 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                    command.Command.KhoCuon6 = loaiGiay.Length > 5 && (5 < loaiGiay.Length - 1) ? command.Command.Rong : null;
                }
            }
            return View(command);
        }
      
        [HttpPost]
        public ActionResult Edit(int dataID,tbl_ProductionCommand model)
        {
            if (model.Id != null && model.Id>0)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.tbl_ProductionCommand.Add(model);
                db.SaveChanges();
            }
            return Edit(dataID, model.KhoGiay, model.BuHao);
        }
    }
}