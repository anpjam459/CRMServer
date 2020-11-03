using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMServer.Api.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int EmployeId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }             
    }
}
