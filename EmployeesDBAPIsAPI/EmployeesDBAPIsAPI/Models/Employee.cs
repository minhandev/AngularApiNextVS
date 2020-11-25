using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesDBAPIsAPI.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
        public string Department { get; set; }
    }
}
