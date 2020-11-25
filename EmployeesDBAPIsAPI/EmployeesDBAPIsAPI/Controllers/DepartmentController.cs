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
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
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
            string query = @"select ID, DepartmentName from dbo.Department";
            return new JsonResult(GetData(query));
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"insert into dbo.Department (DepartmentName) values ('" + department.DepartmentName + "')";
            if (ModelState.IsValid)
            {
                GetData(query);
                return new JsonResult("Success_Post");
            }
            else
            {
                return new JsonResult("Error");
            }
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"update dbo.Department set DepartmentName = ('" + department.DepartmentName + "') " +
                "where ID = ('" + department.ID + "')";
            if (ModelState.IsValid)
            {
                GetData(query);
                return new JsonResult("Success_Put");
            }
            else
            {
                return new JsonResult("Error");
            }
        }

        [HttpDelete(("{ID}"))]
        public JsonResult Delete(int ID)
        {
            string query = @"delete from dbo.Department where ID = ('" + ID + "')";
            if (ModelState.IsValid)
            {
                GetData(query);
                return new JsonResult("Success_Delete");
            }
            else
            {
                return new JsonResult("Error");
            }
        }
    }
}
