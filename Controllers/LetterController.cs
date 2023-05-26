using AlticeApi.BusinessObjects;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace AlticeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LetterController : ControllerBase
    {
        private readonly IUserBusinessObject _userBusinessObject;

        public LetterController(IUserBusinessObject userBusinessObject)
        {
            _userBusinessObject = userBusinessObject;
        }

        [EnableQuery]
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var users = await _userBusinessObject.GetUsers();
            return Ok(users);
        }
    }
}