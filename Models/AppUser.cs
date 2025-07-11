using DIHA.Areas.Staff.Models.Data;
using DIHA.Areas.Custom.Models.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class AppUser : IdentityUser
{
    [Column(TypeName = "nvarchar")]
    [StringLength(400)]
    public string? HomeAdress { get; set; }

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    public string? IdStaff { get; set; }

    [ForeignKey("IdStaff")]
    public virtual Staff? Staff { get; set; }

    public string? IdCustom { get; set; }

    [ForeignKey("IdCustom")]
    public virtual Custom? Custom { get; set; }
}
