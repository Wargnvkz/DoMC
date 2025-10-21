using DoMCLib.Classes.Module.ArchiveDB;
using DoMCModuleControl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.API.Controllers
{
    [Route("api/[controller]")] // Маршрут: /api/status
    [ApiController] // Упрощает обработку APIModule (валидация, JSON)
    public class StatusController : ControllerBase
    {

        private readonly Func<DoMCApplicationContext> _getContext;
        private readonly Func<IMainController> _getController;

        public StatusController(Func<DoMCApplicationContext> getContext, Func<IMainController> getController)
        {
            _getContext = getContext;
            _getController = getController;
        }

        [HttpGet] // GET /api/status
        public async Task<IActionResult> Get()
        {
            var context = _getContext();
            var response = new APIStatusResponse()
            {
                WorkingState = context.WorkingState,
                LastDefects = await context.WorkingState.GetDefectesCycles(_getController(), 2)
            };
            return Ok(response);
        }

        [HttpPost("reset-statistics")] // POST /api/status
        public IActionResult ResetStatistics()
        {
            var context = _getContext();
            context.WorkingState.ResetStatistics();
            return Ok();
        }

        [HttpPost("reset-cycles")] // POST /api/status
        public IActionResult ResetCycles()
        {
            var context = _getContext();
            context.WorkingState.ResetTotalDefectCyles();

            return Ok();
        }
    }

    [Route("api/[controller]")] // Маршрут: /api/control
    [ApiController] // Упрощает обработку APIModule (валидация, JSON)
    public class WorkingController : ControllerBase
    {

        private readonly Func<DoMCApplicationContext> _getContext;
        private readonly Func<IMainController> _getController;

        public WorkingController(Func<DoMCApplicationContext> getContext, Func<IMainController> getController)
        {
            _getContext = getContext;
            _getController = getController;
        }

        [HttpPost("start")] // POST /api/control
        public IActionResult Start()
        {
            var context = _getContext();
            context.WorkingState.StartWork();
            return Ok();
        }

        [HttpPost("stop")] // POST /api/control
        public IActionResult Stop()
        {
            var context = _getContext();
            context.WorkingState.StartWork();
            return Ok();
        }

    }

    public class APIStatusResponse
    {
        public WorkingState WorkingState;
        public List<DefectedCycleSockets> LastDefects;
    }
}
