namespace WebThoiTrang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            BinhLuan = new HashSet<BinhLuan>();
            Cart = new HashSet<Cart>();
            ChiTietDH = new HashSet<ChiTietDH>();
            DanhGia = new HashSet<DanhGia>();
        }

        [Key]
        public int MaSP { get; set; }

        public int MaHSX { get; set; }

        public int MaLoai { get; set; }

        [Required]
        [StringLength(100)]
        public string TenSP { get; set; }

        [Required]
        [StringLength(10)]
        public string KichCo { get; set; }

        public int GiaBan { get; set; }

        [StringLength(100)]
        public string AnhSP { get; set; }

        public string MoTa { get; set; }

        public int? PhanTramKM { get; set; }

        public DateTime NgayTao { get; set; }

        public int? SoLuong { get; set; }

        public string proDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Cart { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDH> ChiTietDH { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGia { get; set; }

        public virtual HangSanXuat HangSanXuat { get; set; }

        public virtual LoaiSP LoaiSP { get; set; }
    }
}
