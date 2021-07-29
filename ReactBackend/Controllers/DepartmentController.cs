using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReactBackend.Models;
using System.Data;
using System.Data.SqlClient;

namespace ReactBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmentId, DepartmentName from dbo.Department";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            SqlDataReader sqlDataReader;
            using(SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                    using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
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
        public JsonResult Post(Department department)
        {
            string query = @"insert into dbo.Department values(@DepartmentName)";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();

                }
            }

            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"update dbo.Department set DepartmentName = @DepartmentName
                            where DepartmentId = @DepartmentId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
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
            string query = @"delete from dbo.Department
                            where DepartmentId = @DepartmentId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", id);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();

                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
