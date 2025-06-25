using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Social_Network_Project_BE.Models;

namespace Social_Network_Project_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Registration")]

        public Response Registration(Registration registration)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SNCon").ToString());

            Dal dal = new Dal();
            response = dal.Registartion(registration, connection);

            return response;
        }

        [HttpPost]
        [Route("Login")]

        public Response Login(Registration registration)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SNCon").ToString());
            Dal dal = new Dal();
            response = dal.Login(registration, connection);
            return response;
        }

        [HttpPost]
        [Route("UserApproval")]

        public Response UserApproval(Registration registration, int Id)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SNCon").ToString());
            Dal dal = new Dal();
            response = dal.UserApproval(registration, connection);
            return response;
        }

        [HttpPost]
        [Route("StaffRegistration")]

        public Response StaffRegistration(Registration staff)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SNCon").ToString());

            Dal dal = new Dal();
            response = dal.StaffRegistartion(staff, connection);

            return response;
        }

        [HttpPost]
        [Route("DeleteStaff")]

        public Response DeleteStaff(Staff staff)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SNCon").ToString());

            Dal dal = new Dal();
            response = dal.DeleteStaff(staff, connection);

            return response;
        }

        [HttpPost]
        [Route("RegistrationList")]

        public Response RegistrationList(Registration registration)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SNCon").ToString());

            Dal dal = new Dal();
            response = dal.RegistrationList(registration, connection);

            return response;
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile FormFile, [FromForm] string FileName)
        {
            try
            {
                if (FormFile == null || FormFile.Length == 0)
                {
                    return BadRequest(new Response
                    {
                        StatusCode = 400,
                        StatusMessage = "No file received"
                    });
                }

                // Folder path to save uploaded files
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await FormFile.CopyToAsync(stream);
                }

                return Ok(new Response
                {
                    StatusCode = 200,
                    StatusMessage = "FIle uploaded successfully",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    StatusCode = 500,
                    StatusMessage = $"Error uploading file: {ex.Message}"
                });
            }
        }

    }
}
