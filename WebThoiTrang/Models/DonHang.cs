namespace WebThoiTrang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            ChiTietDH = new HashSet<ChiTietDH>();
        }

        [Key]
        public int SoDH { get; set; }

        public DateTime NgayDat { get; set; }

        [Required]
        [StringLength(70)]
        public string DiaChiGH { get; set; }

        public string GhiChu { get; set; }

        public int TongTien { get; set; }

        public int? MaTK { get; set; }

        public int? TinhTrang { get; set; }

        public int? HinhThucTT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDH> ChiTietDH { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
