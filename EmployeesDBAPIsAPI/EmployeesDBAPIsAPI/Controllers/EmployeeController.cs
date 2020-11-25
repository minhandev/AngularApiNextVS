using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EmployeesDBAPIsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmployeesDBAPIsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataTable GetData(string query)
        {
            SqlDataReader sqlDataReader;
            DataTable table = new DataTable();
            string sqlDbSource = _configuration.GetConnectionString("ConnectString");
            using (SqlConnection sqlConnection = new SqlConnection(sqlDbSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return table;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select ID, EmployeeName, DateOfJoining, PhotoFileName, Department from dbo.Employee";
            if (ModelState.IsValid)
            {
                return new JsonResult(GetData(query));
            }
            else
            {
                return new JsonResult("Error");
            }
        }
      
    }
}
