namespace WebThoiTrang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhGia")]
    public partial class DanhGia
    {
        public int MaSP { get; set; }

        [Key]
        [Column("DanhGia")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DanhGia1 { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
