using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Controllers;
using WebThoiTrang.Models;

namespace WebThoiTrang.Areas.Admin.Controllers
{
    public class LoaiSPsController : BaseController
    {
        private KDTShop db = new KDTShop();
        // GET: Admin/LoaiSPs
        public ActionResult Index(string sortOder, string searchString, string currenFilter, int? page)
        {
            int pageSize = 0;
            int pageNumber = 0;
            ViewBag.CurrentSort = sortOder;
            ViewBag.SapTheoTen = string.IsNullOrEmpty(sortOder) ? "name_desc" : "";
            ViewBag.SapTheoDanhMuc = sortOder == "TenDM" ? "dm_desc" : "TenDM";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currenFilter;
            }
            ViewBag.CurrentFilter = searchString;
            try
            {
                var loaiSPs = db.LoaiSP.Select(t => t);
                if (!String.IsNullOrEmpty(searchString))
                {
                    loaiSPs = loaiSPs.Where(t => t.TenLoai.Contains(searchString));
                }
                switch (sortOder)
                {
                    case "name_desc":
                        loaiSPs = loaiSPs.OrderByDescending(t => t.TenLoai);
                        break;

                    case "TenDM":
                        loaiSPs = loaiSPs.OrderBy(t => t.DanhMuc.TenDM);
                        break;

                    case "dm_desc":
                        loaiSPs = loaiSPs.OrderByDescending(t => t.DanhMuc.TenDM);
                        break;

                    default:
                        loaiSPs = loaiSPs.OrderBy(t => t.TenLoai);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    pageSize = loaiSPs.ToList().Count;
                    if (pageSize == 0)
                    {
                        pageSize = 1;
                    }
                    pageNumber = (page ?? 1);
                    return View(loaiSPs.ToPagedList(pageNumber, pageSize));
                }
                pageSize = 5;
                pageNumber = (page ?? 1);
                return View(loaiSPs.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

        }

        //THÊM SẢN PHẨM
        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DanhMuc, "MaDM", "TenDM");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "MaLoai,MaDM,TenLoai")] LoaiSP loaiSP)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //sanPham.AnhSP = "";
                    //var f = Request.Files["ImageFile"];
                    if (string.IsNullOrEmpty(loaiSP.TenLoai))
                    {
                        ViewBag.Error = "Vui lòng nhập tên loại sản phẩm!";
                        return View(loaiSP);
                    }
                    //if (sanPham.SoLuong == null)
                    //{
                    //    sanPham.SoLuong = 0;
                    //}
                    //if (sanPham.PhanTramKM == null)
                    //{
                    //    sanPham.PhanTramKM = 0;
                    //}
                    //if (sanPham.SoLuong < 0 || sanPham.PhanTramKM < 0 || sanPham.GiaBan < 0 || sanPham.PhanTramKM > 100)
                    //{
                    //    ViewBag.Error = "Giá sản phẩm, khuyến mãi, giá bán phải lớn hơn 0 và khuyến mãi nhỏ hơn 100!";
                    //    ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                    //    return View(sanPham);
                    //}
                    //if (f != null && f.ContentLength > 0)
                    //{
                    //    string FileName = System.IO.Path.GetFileName(f.FileName);
                    //    string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                    //    f.SaveAs(UploadPath);
                    //    sanPham.AnhSP = FileName;
                    //}
                    //sanPham.NgayTao = DateTime.Now;
                    db.LoaiSP.Add(loaiSP);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MaDM = new SelectList(db.DanhMuc, "MaDM", "TenDM", loaiSP.MaDM);
                    return View(loaiSP);
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                ViewBag.MaDM = new SelectList(db.DanhMuc, "MaDM", "TenDM", loaiSP.MaDM);
                return View(loaiSP);
            }
        }

        //CHI TIẾT SẢN PHẨM
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            LoaiSP loaiSP = new LoaiSP();
            try
            {
                loaiSP = db.LoaiSP.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (loaiSP == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(loaiSP);
        }

        //CHỈNH SỬA
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            LoaiSP loaiSP = new LoaiSP();
            try
            {
                loaiSP = db.LoaiSP.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

            if (loaiSP == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            ViewBag.MaDM = new SelectList(db.DanhMuc, "MaDM", "TenDM", loaiSP.MaDM);
            return View(loaiSP);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MaLoai,MaDM,TenLoai")] LoaiSP loaiSP)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(loaiSP.TenLoai))
                    {
                        ViewBag.Error = "Vui lòng nhập loại sản phẩm!";
                        return View(loaiSP);
                    }
                    //if (loaiSP.SoLuong == null)
                    //{
                    //    loaiSP.SoLuong = 0;
                    //}
                    //if (loaiSP.PhanTramKM == null)
                    //{
                    //    sanPham.PhanTramKM = 0;
                    //}
                    //if (sanPham.SoLuong < 0 || sanPham.PhanTramKM < 0 || sanPham.GiaBan < 0)
                    //{
                    //    ViewBag.Error = "Giá sản phẩm, Khuyến mãi, giá bán phải lớn hơn 0!";
                    //    ViewBag.MaDM = new SelectList(db.DanhMuc, "MaDM", "TenDM", sanPham.MaDM);
                    //    return View(sanPham);
                    //}

                    //var f = Request.Files["ImageFile"];
                    //if (f != null && f.ContentLength > 0)
                    //{
                    //    string FileName = System.IO.Path.GetFileName(f.FileName);
                    //    string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                    //    f.SaveAs(UploadPath);
                    //    sanPham.AnhSP = FileName;
                    //}
                    //else
                    //{
                    //    sanPham.AnhSP = Request["oldImage"];
                    //}
                    db.Entry(loaiSP).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MaDM = new SelectList(db.DanhMuc, "MaDM", "TenDM", loaiSP.MaDM);
                    return View(loaiSP);
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi: " + e.Message;
                ViewBag.MaDM = new SelectList(db.DanhMuc, "MaDM", "TenDM", loaiSP.MaDM);
                return View(loaiSP);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            LoaiSP loaiSP = new LoaiSP();
            try
            {
                loaiSP = db.LoaiSP.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (loaiSP == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(loaiSP);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoaiSP loaiSP = new LoaiSP();
            try
            {
                loaiSP = db.LoaiSP.Find(id);
                if (loaiSP == null)
                {   
                    return RedirectToAction("NotFoundPage", "ErrorAdmin");
                }
                db.LoaiSP.Remove(loaiSP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Sản phẩm liên kết với các mục khác vui lòng xóa chúng trước khi xóa sản phẩm!";
                DanhMuc danhMuc = db.DanhMuc.Find(loaiSP.MaDM);
                ViewData["MaDM"] = danhMuc.TenDM;
                return View(loaiSP);
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