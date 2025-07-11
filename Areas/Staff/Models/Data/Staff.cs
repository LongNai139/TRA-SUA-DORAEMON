using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIHA.Areas.Staff.Models.Data
{
    [Table("Staff")]
    public class Staff
    {
        [Key]
        public string IdStaff { get; set; }

        [StringLength(256)]
        public string StaffName { get; set; }

        [StringLength(256)]
        public string NormalizedStaffName { get; set; }
        
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
