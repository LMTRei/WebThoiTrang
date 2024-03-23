using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebThoiTrang.Models
{
    public class CartItem
    {
        public SanPham Product { get; set; }
        public Cart Cart { get; set; }
        public string Hinh { get; set; }
    }
}