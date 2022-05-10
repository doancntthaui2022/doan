
using HoangNV.HotelBooking.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoangNV.HotelBooking.Entities.Entities
{
    public class Users : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int Active { get; set; }
        public string RoleId { get; set; }

        public int? CustomerId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Roles Roles { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customers Customer { get; set; }
    }
}
