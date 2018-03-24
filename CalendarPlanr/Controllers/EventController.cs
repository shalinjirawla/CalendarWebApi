using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CalendarPlanr.DomainModel;
using CalendarPlanr.Repo;
using Microsoft.AspNetCore.Cors;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace CalendarPlanr.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class EventController : Controller
    {
        
        private readonly IEventRepo reo;
        public EventController(IEventRepo repo)
        {
            reo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var data = await reo.GetAllEvents();
            return Json(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var data = await reo.GetById(id);
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEvent([FromBody] CEvent model)
        {
            return Json(await reo.SaveEvent(model));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, [FromBody] CEvent model)
        {
            return Json(await reo.PutEvent(id, model));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            return Json(await reo.DeleteEventByID(id));
        }

        [HttpGet("{id}")]
        [Produces("text/csv")]
        public async Task<IActionResult> Getcsv(int id)
        {
            var data = await reo.GetCSV(id);
            return Json(data);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Storecsv([FromBody] List<CEvent> model, int id)
        {
            var data = await reo.importCSV(model, id);
            return Json(data);
        }
        [HttpGet("{pin}")]
        public async Task<IActionResult> Login(int pin)
        {
            var a = await reo.Login(pin);
            return Json(a);
        }
        [HttpPost]
        public async Task<IActionResult> FilePost(List<IFormFile> files)
        {
            var sb = await reo.Upload(files);
            return Ok(sb);
        }
        
    }
}