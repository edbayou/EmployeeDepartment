using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDepartment.Models
{
    class EmployeeDepartmentEntities : DbContext
    {
        public EmployeeDepartmentEntities() : base("DefaultConnection")
        {

        }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
    }
}
