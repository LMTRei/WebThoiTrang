using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Common;
using WebThoiTrang.Models;

namespace WebThoiTrang.Areas.Admin.Controllers
{
    public class HangSanXuatsController : BaseController
    {
        private KDTShop db = new KDTShop();
        // GET: Admin/HangSanXuats
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
                var hangSanXuats = db.HangSanXuat.Select(t => t);
                if (!String.IsNullOrEmpty(searchString))
                {
                    hangSanXuats = hangSanXuats.Where(t => t.TenHSX.Contains(searchString));
                }
                switch (sortOder)
                {
                    case "name_desc":
                        hangSanXuats = hangSanXuats.OrderByDescending(t => t.TenHSX);
                        break;

                    //case "TenDM":
                    //    loaiSPs = loaiSPs.OrderBy(t => t.DanhMuc.TenDM);
                    //    break;

                    //case "dm_desc":
                    //    loaiSPs = loaiSPs.OrderByDescending(t => t.DanhMuc.TenDM);
                    //    break;

                    default:
                        hangSanXuats = hangSanXuats.OrderBy(t => t.TenHSX);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    pageSize = hangSanXuats.ToList().Count;
                    if (pageSize == 0)
                    {
                        pageSize = 1;
                    }
                    pageNumber = (page ?? 1);
                    return View(hangSanXuats.ToPagedList(pageNumber, pageSize));
                }
                pageSize = 5;
                pageNumber = (page ?? 1);
                return View(hangSanXuats.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

        }
        //THÊM SẢN PHẨM
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "MaHSX,TenHSX,DienThoaiHSX,EmailHSX,DiaChiHSX,AnhHSX,ThongTinHSX")] HangSanXuat hangSanXuat)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    hangSanXuat.AnhHSX = "";
                    var f = Request.Files["ImageFile"];
                    if (string.IsNullOrEmpty(hangSanXuat.TenHSX))
                    {
                        hangSanXuat.TenHSX = "";
                    }
                    if (string.IsNullOrEmpty(hangSanXuat.DienThoaiHSX))
                    {
                        hangSanXuat.DienThoaiHSX = "";
                    }
                    if (string.IsNullOrEmpty(hangSanXuat.EmailHSX))
                    {
                        hangSanXuat.EmailHSX = "";
                    }
                    if (string.IsNullOrEmpty(hangSanXuat.DiaChiHSX))
                    {
                        hangSanXuat.DiaChiHSX = "";
                    }
                    if (string.IsNullOrEmpty(hangSanXuat.ThongTinHSX))
                    {
                        hangSanXuat.ThongTinHSX = "";
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
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                        f.SaveAs(UploadPath);
                        hangSanXuat.AnhHSX = FileName;
                    }
                    //sanPham.NgayTao = DateTime.Now;
                    db.HangSanXuat.Add(hangSanXuat);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    //ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                    return View(hangSanXuat);
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                //ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                return View(hangSanXuat);
            }
        }

        //CHI TIẾT SẢN PHẨM
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            HangSanXuat hangSanXuat = new HangSanXuat();
            try
            {
                hangSanXuat = db.HangSanXuat.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (hangSanXuat == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(hangSanXuat);
        }

        //CHỈNH SỬA
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            HangSanXuat hangSanXuat = new HangSanXuat();
            try
            {
                hangSanXuat = db.HangSanXuat.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

            if (hangSanXuat == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            //ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
            return View(hangSanXuat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MaHSX,TenHSX,DienThoaiHSX,EmailHSX,DiaChiHSX,AnhHSX,ThongTinHSX")] HangSanXuat hangSanXuat)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(hangSanXuat.TenHSX))
                    {
                        hangSanXuat.TenHSX = "";
                    }
                    if (string.IsNullOrEmpty(hangSanXuat.DienThoaiHSX))
                    {
                        hangSanXuat.DienThoaiHSX = "";
                    }
                    if (string.IsNullOrEmpty(hangSanXuat.EmailHSX))
                    {
                        hangSanXuat.EmailHSX = "";
                    }
                    if (string.IsNullOrEmpty(hangSanXuat.DiaChiHSX))
                    {
                        hangSanXuat.DiaChiHSX = "";
                    }
                    if (string.IsNullOrEmpty(hangSanXuat.ThongTinHSX))
                    {
                        hangSanXuat.ThongTinHSX = "";
                    }

                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                        f.SaveAs(UploadPath);
                        hangSanXuat.AnhHSX = FileName;
                    }
                    else
                    {
                        hangSanXuat.AnhHSX = Request["oldImage"];
                    }
                    db.Entry(hangSanXuat).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    //ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                    return View(hangSanXuat);
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi: " + e.Message;
                //ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                return View(hangSanXuat);
            }
        }

        //XÓA
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            HangSanXuat hangSanXuat = new HangSanXuat();
            try
            {
                hangSanXuat = db.HangSanXuat.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (hangSanXuat == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(hangSanXuat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HangSanXuat hangSanXuat = new HangSanXuat();
            try
            {
                hangSanXuat = db.HangSanXuat.Find(id);
                if (hangSanXuat == null)
                {
                    return RedirectToAction("NotFoundPage", "ErrorAdmin");
                }
                db.HangSanXuat.Remove(hangSanXuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                //ViewData["Error"] = "Sản phẩm liên kết với các mục khác vui lòng xóa chúng trước khi xóa sản phẩm!";
                //DanhMuc danhMuc = db.DanhMucs.Find(sanPham.MaDM);
                //ViewData["MaDM"] = danhMuc.TenDM;
                return View(hangSanXuat);
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