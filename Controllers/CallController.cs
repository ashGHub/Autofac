using AutofacMultipleImpTest.Logger;
using AutofacMultipleImpTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutofacMultipleImpTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly ISampleLog _log;
        private readonly IService _callService;

        public CallController(IService callService, ISampleLog log)
        {
            _callService = callService;
            _log = log;
        }

        [HttpGet]
        public ActionResult<string> GetCall()
        {
            return new JsonResult(_callService.Process());
        }
    }
}