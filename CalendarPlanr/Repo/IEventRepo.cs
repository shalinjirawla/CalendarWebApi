using CalendarPlanr.DomainModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarPlanr.Repo
{
    public interface IEventRepo
    {
        Task<List<CEvent>> GetAllEvents();
        Task<bool> SaveEvent(CEvent model);
        // if only want to use single method for update and create the use SaveContact else use PutContact for Update
        Task<bool> PutEvent(int id, CEvent model);
        Task<bool> DeleteEventByID(int id);
        Task<CEvent> GetById(int id);
        Task<IEnumerable<CEvent>> GetCSV(int id);
        Task<IEnumerable<CEvent>> importCSV(List<CEvent> model,int id);
        Task<CLogin> Login(int pin);
        Task<string> Upload(List<IFormFile> files);
    }
}
