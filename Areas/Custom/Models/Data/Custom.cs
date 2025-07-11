using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIHA.Areas.Custom.Models.Data
{
    [Table("Custom")]
    public class Custom
    {
        [Key]
        public string IdCustom { get; set; }

        [StringLength(256)]
        public string CustomName { get; set; }

        [StringLength(256)]
        public string NormalizedCustomName { get; set; }
        
        [Column(TypeName = "datetime2(7)")]
        public DateTime? BirthDate { get; set; }

        [StringLength(256)]
        public string Email { get; set; }
        
        [StringLength(400)]
        public string HomeAdress { get; set; }
        
        [StringLength(400)]
        public string PhoneNumber { get; set; }

    }
}