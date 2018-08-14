using AutofacMultipleImpTest.Logger;
using AutofacMultipleImpTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutofacMultipleImpTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ISampleLog _log;
        private readonly IService _defaultService;

        public DefaultController(IService defaultService, ISampleLog log)
        {
            _defaultService = defaultService;
            _log = log;
        }

        [HttpGet]
        public ActionResult<string> GetDefault()
        {
            return new JsonResult(_defaultService.Process());
        }
    }
}