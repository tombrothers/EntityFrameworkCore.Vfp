using EntityFrameworkCore.Vfp.Storage;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCore.Vfp.Tests.Data.AllTypes {
    [Table("AllTypesTables")]
    public class AllTypesTable {
        public int Id { get; set; }

        [Column(TypeName = VfpStorageTypes.Character)]
        public string Char { get; set; }

        [Column(TypeName = VfpStorageTypes.BinaryCharacter)]
        public string BinaryChar { get; set; }

        [Column(TypeName = VfpStorageTypes.Varchar)]
        public string VarChar { get; set; }

        [Column(TypeName = VfpStorageTypes.BinaryVarchar)]
        public string BinaryVarChar { get; set; }

        public string Memo { get; set; }

        [Column(TypeName = VfpStorageTypes.BinaryMemo)]
        public string BinaryMemo { get; set; }

        [Column(TypeName = VfpStorageTypes.Currency)]
        public decimal? Currency { get; set; }

        public decimal? Decimal { get; set; }

        public int? Integer { get; set; }

        public bool? Logical { get; set; }

        public long? Long { get; set; }

        public float? Float { get; set; }

        public double? Double { get; set; }
        ////public byte[] Blob { get; set; }

        [Column(TypeName = VfpStorageTypes.Date)]
        public DateTime? Date { get; set; }

        public DateTime? DateTime { get; set; }
        public Guid? Guid { get; set; }
    }
}
