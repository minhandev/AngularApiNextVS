using System;
using System.Data;
using System.Data.SqlClient;
using EmployeesDBAPIsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EmployeesDBAPIsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment evn)
        {
            _configuration = configuration;
            _env = evn;
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

        [HttpPost]
        public JsonResult Post(Employee em)
        {
            string query = @"insert into dbo.Employee (EmployeeName, DateOfJoining, PhotoFileName, Department)
            values
            ('" + em.EmployeeName +"', " +
            "'"+ em.DateOfJoining.Value.ToString("yyyyMMdd") + "', " +
            "'"+ em.PhotoFileName +"', " +
            "'" + em.Department + "'" +
            ")";
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
        public JsonResult Put(Employee em)
        {
            string query = @"
                            update dbo.Employee set 
                            EmployeeName = '" + em.EmployeeName + "',"+
                            "Department = '" + em.Department + "',"+
                            "DateOfJoining = '" + em.DateOfJoining.Value.ToString("yyyyMMdd") + "'," +
                            "PhotoFileName = '" + em.PhotoFileName + "'" +
                            "Where ID = '" + em.ID + "'";

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
            string query = @"delete from  dbo.Employee where ID = ('" + ID + "')";
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

        [Route("Save")]
        [HttpPost]
        public JsonResult Save()
        {
            try
            {
                var httpRequest = Request.Form;
                var posFile = httpRequest.Files[0];
                string fileName = posFile.FileName;
                var path = _env.ContentRootPath + "/Photos/" + fileName;
                using(var stream = new FileStream(path, FileMode.Create))
                {
                    posFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("___.png");
            }
        }

        [Route("Getall")]
        public JsonResult GetAll()
        {
            string query = @"select ID, DepartmentName from dbo.Department";
            try
            {
                if (ModelState.IsValid)
                {
                    return new JsonResult(GetData(query));
                }
                else
                {
                    return new JsonResult("Not_Connect");
                }
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }
    }
}
