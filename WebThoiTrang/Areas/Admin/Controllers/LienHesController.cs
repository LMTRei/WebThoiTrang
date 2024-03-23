using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Models;

namespace WebThoiTrang.Areas.Admin.Controllers
{
    public class LienHesController : BaseController
    {
        private KDTShop db = new KDTShop();

        //TRANG CHỦ LIÊN HỆ
        public ActionResult Index()
        {
            List<LienHe> lienHes = new List<LienHe>();
            try
            {
                lienHes = db.LienHe.ToList();
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            return View(lienHes);
        }

        //THÊM
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLH,DiaChi,Email,DienThoai,ThoiGianLamViec")] LienHe lienHe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string error = ValidLienHe(lienHe);
                    if (error.Length > 0)
                    {
                        ViewBag.Error = error;
                        return View(lienHe);
                    }
                    db.LienHe.Add(lienHe);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(lienHe);
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        //HÀM KIỂM TRA TÍNH HỢP LỆ DỮ LIỆU
        public string ValidLienHe(LienHe lienHe)
        {
            if (string.IsNullOrEmpty(lienHe.DiaChi))
            {
                return "Vui lòng nhập địa chỉ!";
            }
            if (string.IsNullOrEmpty(lienHe.Email))
            {
                return "Vui lòng nhập email!";
            }
            if (string.IsNullOrEmpty(lienHe.DienThoai))
            {
                return "Vui lòng nhập số điện thoại!";
            }
            if (string.IsNullOrEmpty(lienHe.ThoiGianLamViec))
            {
                return "Vui lòng nhập thời gian làm việc!";
            }

            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (!Regex.IsMatch(lienHe.Email, expression))
            {
                return "Email chưa đúng định dạng!";
            }
            string regex = @"^([\+]?61[-]?|[0])?[1-9][0-9]{8}$";

            if (!Regex.IsMatch(lienHe.DienThoai, regex))
            {
                return "Số điện thoại không hợp lệ!";
            }
            return "";
        }

        //CHI TIẾT
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }

            LienHe lienHe = new LienHe();
            try
            {
                lienHe = db.LienHe.Find(id);
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

            if (lienHe == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(lienHe);
        }


        //CHỈNH SỬA
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            LienHe lienHe = new LienHe();
            try
            {
                lienHe = db.LienHe.Find(id);
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

            if (lienHe == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(lienHe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLH,DiaChi,Email,DienThoai,ThoiGianLamViec")] LienHe lienHe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string error = ValidLienHe(lienHe);
                    if (error.Length > 0)
                    {
                        ViewBag.Error = error;
                        return View(lienHe);
                    }
                    db.Entry(lienHe).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(lienHe);
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        //XÓA
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            LienHe lienHe = new LienHe();
            try
            {
                lienHe = db.LienHe.Find(id);
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

            if (lienHe == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(lienHe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LienHe lienHe = new LienHe();
            try
            {
                lienHe = db.LienHe.Find(id);
                if (lienHe == null)
                {
                    return RedirectToAction("Index");
                }
                db.LienHe.Remove(lienHe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}