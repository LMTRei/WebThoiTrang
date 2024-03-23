using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Models;

namespace WebThoiTrang.Areas.Admin.Controllers
{
    public class SanPhamsController : BaseController
    {
        private KDTShop db = new KDTShop();
        // GET: Admin/SanPhams
        //TRANG CHỦ TRANG SẢN PHẨM (TÌM KIẾM THEO TÊN SP, SẮP XẾP THEO TÊN SP, TÊN HÃNG)
        public ActionResult Index(string sortOder, string searchString, string currenFilter, int? page)
        {
            int pageSize = 0;
            int pageNumber = 0;
            ViewBag.CurrentSort = sortOder;
            ViewBag.SapTheoTen = string.IsNullOrEmpty(sortOder) ? "name_desc" : "";
            ViewBag.SapTheoHang = sortOder == "TenHSX" ? "dm_desc" : "TenHSX";
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
                var sanPhams = db.SanPham.Select(t => t);
                if (!String.IsNullOrEmpty(searchString))
                {
                    sanPhams = sanPhams.Where(t => t.TenSP.Contains(searchString));
                }
                switch (sortOder)
                {
                    case "name_desc":
                        sanPhams = sanPhams.OrderByDescending(t => t.TenSP);
                        break;

                    case "TenDM":
                        sanPhams = sanPhams.OrderBy(t => t.HangSanXuat.TenHSX);
                        break;

                    case "dm_desc":
                        sanPhams = sanPhams.OrderByDescending(t => t.HangSanXuat.TenHSX);
                        break;

                    default:
                        sanPhams = sanPhams.OrderBy(t => t.TenSP);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    pageSize = sanPhams.ToList().Count;
                    if (pageSize == 0)
                    {
                        pageSize = 1;
                    }
                    pageNumber = (page ?? 1);
                    return View(sanPhams.ToPagedList(pageNumber, pageSize));
                }
                pageSize = 5;
                pageNumber = (page ?? 1);
                return View(sanPhams.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        //THÊM SẢN PHẨM
        public ActionResult Create()
        {
            ViewBag.MaHSX = new SelectList(db.HangSanXuat, "MaHSX", "TenHSX");
            ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "MaSP,MaHSX,MaLoai,TenSP,KichCo,GiaBan,AnhSP,MoTa,PhanTramKM,SoLuong,proDescription")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sanPham.AnhSP = "";
                    var f = Request.Files["ImageFile"];
                    if (string.IsNullOrEmpty(sanPham.MoTa))
                    {
                        sanPham.MoTa = "";
                    }
                    if (string.IsNullOrEmpty(sanPham.KichCo))
                    {
                        sanPham.KichCo = "";
                    }
                    if (string.IsNullOrEmpty(sanPham.proDescription))
                    {
                        sanPham.proDescription = "";
                    }
                    if (sanPham.SoLuong == null)
                    {
                        sanPham.SoLuong = 0;
                    }
                    if (sanPham.PhanTramKM == null)
                    {
                        sanPham.PhanTramKM = 0;
                    }
                    if (sanPham.SoLuong < 0 || sanPham.PhanTramKM < 0 || sanPham.GiaBan < 0 || sanPham.PhanTramKM > 100)
                    {
                        ViewBag.Error = "Giá sản phẩm, khuyến mãi, giá bán phải lớn hơn 0 và khuyến mãi nhỏ hơn 100!";
                        ViewBag.MaHSX = new SelectList(db.HangSanXuat, "MaHSX", "TenHSX", sanPham.MaHSX);
                        ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
                        return View(sanPham);
                    }
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.AnhSP = FileName;
                    }
                    sanPham.NgayTao = DateTime.Now;
                    db.SanPham.Add(sanPham);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MaHSX = new SelectList(db.HangSanXuat, "MaHSX", "TenHSX", sanPham.MaHSX);
                    ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
                    return View(sanPham);
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                ViewBag.MaHSX = new SelectList(db.HangSanXuat, "MaHSX", "TenHSX", sanPham.MaHSX);
                ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
                return View(sanPham);
            }
        }

        //CHI TIẾT SẢN PHẨM
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            SanPham sanPham = new SanPham();
            try
            {
                sanPham = db.SanPham.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (sanPham == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(sanPham);
        }

        //CHỈNH SỬA
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            SanPham sanPham = new SanPham();
            try
            {
                sanPham = db.SanPham.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

            if (sanPham == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            ViewBag.MaHSX = new SelectList(db.HangSanXuat, "MaHSX", "TenHSX", sanPham.MaHSX);
            ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MaSP,MaHSX,MaLoai,TenSP,KichCo,GiaBan,AnhSP,MoTa,PhanTramKM,SoLuong,proDescription")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(sanPham.MoTa))
                    {
                        sanPham.MoTa = "";
                    }
                    if (string.IsNullOrEmpty(sanPham.KichCo))
                    {
                        sanPham.KichCo = "";
                    }
                    if (string.IsNullOrEmpty(sanPham.proDescription))
                    {
                        sanPham.proDescription = "";
                    }
                    if (sanPham.SoLuong == null)
                    {
                        sanPham.SoLuong = 0;
                    }
                    if (sanPham.PhanTramKM == null)
                    {
                        sanPham.PhanTramKM = 0;
                    }
                    if (sanPham.SoLuong < 0 || sanPham.PhanTramKM < 0 || sanPham.GiaBan < 0)
                    {
                        ViewBag.Error = "Giá sản phẩm, Khuyến mãi, giá bán phải lớn hơn 0!";
                        ViewBag.MaHSX = new SelectList(db.HangSanXuat, "MaHSX", "TenHSX", sanPham.MaHSX);
                        ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
                        return View(sanPham);
                    }

                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.AnhSP = FileName;
                    }
                    else
                    {
                        sanPham.AnhSP = Request["oldImage"];
                    }
                    sanPham.NgayTao = DateTime.Now;
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MaHSX = new SelectList(db.HangSanXuat, "MaHSX", "TenHSX", sanPham.MaHSX);
                    ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
                    return View(sanPham);
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi: " + e.Message;
                ViewBag.MaHSX = new SelectList(db.HangSanXuat, "MaHSX", "TenHSX", sanPham.MaHSX);
                ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
                return View(sanPham);
            }
        }

        //XÓA
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            SanPham sanPham = new SanPham();
            try
            {
                sanPham = db.SanPham.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (sanPham == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(sanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = new SanPham();
            try
            {
                sanPham = db.SanPham.Find(id);
                if (sanPham == null)
                {
                    return RedirectToAction("NotFoundPage", "ErrorAdmin");
                }
                db.SanPham.Remove(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Sản phẩm liên kết với các mục khác vui lòng xóa chúng trước khi xóa sản phẩm!";
                HangSanXuat hangSanXuat = db.HangSanXuat.Find(sanPham.MaHSX);
                ViewData["MaHSX"] = hangSanXuat.TenHSX;
                LoaiSP loaiSP = db.LoaiSP.Find(sanPham.MaLoai);
                ViewData["MaLoai"] = loaiSP.TenLoai;
                return View(sanPham);
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