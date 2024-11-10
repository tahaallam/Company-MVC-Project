using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department
    {

        public int Id { get; set; }
        [Required]
        public string Name  { get; set; }
        [Required]
        public string Code  { get; set; }
        public DateTime DateOfCreation { get; set; }
        public ICollection<Employee> employees { get; set; } = new HashSet<Employee>();

    }
}
