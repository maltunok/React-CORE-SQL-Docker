using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReactBackend.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ReactBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select EmployeeId, EmployeeName, Department, DateOfJoining, PhotoFileName 
                            from dbo.Employee";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
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

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"insert into dbo.Employee(
                EmployeeName, Department, DateOfJoining, PhotoFileName)
                values (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName) ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();

                }
            }

            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"update dbo.Employee set EmployeeName = @EmployeeName,
                                Department = @Department,
                                DateOfJoining = @DateOfJoining,
                                PhotoFileName = @PhotoFileName
                            where EmployeeId = @EmployeeId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();

                }
            }

            return new JsonResult(table);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.Employee
                            where EmployeeId = @EmployeeId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();

                }
            }

            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]

        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using( var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }
    }
}
