using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityWebApplnUsingAsp.Net.Models.DTO;

namespace UniversityWebApplnUsingAsp.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProtectedController : ControllerBase
    {
        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            //var status = new Status();
            //status.StatusCode = 1;
            //status.Message = "Data from protected controller";
            return Ok("Data from protected controller");
        }
    }
}
