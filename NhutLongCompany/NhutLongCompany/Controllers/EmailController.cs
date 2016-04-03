using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;
using NhutLongCompany.Attribute;

namespace NhutLongCompany.Controllers
{
    [RedirectOnError]
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        // GET: Email/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Email/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Email/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Email/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Email/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Email/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Email/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SendEmail sendEmail)
        {

            if (ModelState.IsValid)
            {//info@nhutlong.com
                MailMessage mail = new MailMessage();
                mail.To.Add("info@nhutlong.com");
                mail.From = new MailAddress("mailorderthung@gmail.com");
                mail.Subject = "Tin Nhắn từ khách hàng";
                string Body = ("Tên khách hàng: <br><h1>" + sendEmail.hovaten + "</h1><br> Emai khách hàng : <h1>" + sendEmail.Email + "</h1><br>" + " Số điện thoại khách hàng : <h1>" + sendEmail.sodienthoai + "</h1><br>" + " Nộ dung tin nhắn : <h1>" + sendEmail.content + "</h1>");
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("mailorderthung@gmail.com", "a1234@1234");
                smtp.EnableSsl = true;
                smtp.Send(mail);


            }

            return View("success");
        }

        public ActionResult success()
        {
            return View();
        }

    }
}
