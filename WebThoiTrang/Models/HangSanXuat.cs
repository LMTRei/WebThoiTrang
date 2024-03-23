namespace WebThoiTrang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangSanXuat")]
    public partial class HangSanXuat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HangSanXuat()
        {
            SanPham = new HashSet<SanPham>();
        }

        [Key]
        public int MaHSX { get; set; }

        [StringLength(100)]
        public string TenHSX { get; set; }

        [StringLength(20)]
        public string DienThoaiHSX { get; set; }

        [StringLength(50)]
        public string EmailHSX { get; set; }

        public string DiaChiHSX { get; set; }

        public string AnhHSX { get; set; }

        public string ThongTinHSX { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPham { get; set; }
    }
}
