using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Models;

namespace WebThoiTrang.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private KDTShop db = new KDTShop();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            List<TaiKhoan> taiKhoans = new List<TaiKhoan>();
            List<DanhMuc> danhMucs = new List<DanhMuc>();
            List<LoaiSP> loaiSPs = new List<LoaiSP>();
            List<HangSanXuat> hangSanXuats = new List<HangSanXuat>();
            List<SanPham> sanPhams = new List<SanPham>();
            List<DonHang> donHangs = new List<DonHang>();
            try
            {
                taiKhoans = db.TaiKhoan.ToList();
                danhMucs = db.DanhMuc.ToList();
                loaiSPs = db.LoaiSP.ToList();
                hangSanXuats = db.HangSanXuat.ToList();
                sanPhams = db.SanPham.ToList();
                donHangs = db.DonHang.ToList();
            }
            catch (Exception e)
            {
                return View("ServerError", "Error");
            }
            ViewData["taikhoan"] = taiKhoans.Count;
            ViewData["danhmuc"] = danhMucs.Count;
            ViewData["loaisp"] = loaiSPs.Count;
            ViewData["hangsanxuat"] = hangSanXuats.Count;
            ViewData["sanpham"] = sanPhams.Count;
            ViewData["donhang"] = donHangs.Count;
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home", new { area = "" });
        }
    }
}