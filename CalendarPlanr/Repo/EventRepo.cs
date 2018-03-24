using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalendarPlanr.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.IO;
using CalendarPlanr.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace CalendarPlanr.Repo
{
    public class EventRepo : IEventRepo
    {
        private readonly DataContext db;
        private readonly IHostingEnvironment _appEnvironment;
        private static readonly HttpContext context;
        private readonly IFileProvider _fileProvider;
        public EventRepo(DataContext dataContext, IHostingEnvironment appEnvironment, IFileProvider fileProvider)
        {
            db = dataContext;
            _appEnvironment = appEnvironment;
            _fileProvider = fileProvider;
        }
        public async Task<bool> DeleteEventByID(int id)
        {
            using (db)
            {
                CEvent ce = await db.CEvents.FirstOrDefaultAsync(x => x.Id == id);
                if(ce != null)
                {
                    db.CEvents.Remove(ce);
                }
                return await db.SaveChangesAsync() >= 1;
            }
        }
        public async Task<List<CEvent>> GetAllEvents()
        {
            using (db)
            {
                return await (from a in db.CEvents
                              select new CEvent
                              {
                                  Id = a.Id,
                                  Title = a.Title,
                                  Description = a.Description,
                                  ImagePath = a.ImagePath,
                                  LoginID = a.LoginID,
                                  StartDate = a.StartDate,
                                  EndDate = a.EndDate
                              }).ToListAsync();
            }
        }
        public async Task<bool> SaveEvent(CEvent cEvent)
        {
            using (db)
            {
                var CEve = new CEvent();
                
                CEve.Title = cEvent.Title;
                CEve.Description = cEvent.Description;
                CEve.LoginID = cEvent.LoginID;
                CEve.ImagePath = cEvent.ImagePath;
                CEve.StartDate = cEvent.StartDate;
                CEve.EndDate = cEvent.EndDate;
                db.CEvents.Add(CEve);

                return await db.SaveChangesAsync() >= 1;
            }
        }
        public async Task<bool> PutEvent(int id, CEvent modl)
        {
            using (db)
            {
                var query = await db.CEvents.FirstOrDefaultAsync(x => x.Id == id);
                if(query != null)
                {
                    query.Title = modl.Title;
                    query.ImagePath = modl.ImagePath;
                    query.Description = modl.Description;
                    query.StartDate = modl.StartDate;
                    query.EndDate = modl.EndDate;
                    query.LoginID = modl.LoginID;
                }
                return await db.SaveChangesAsync() >= 1;
            }
        }
        public async Task<CEvent> GetById(int id)
        {
            using (db)
            {
                var query = await db.CEvents.FirstOrDefaultAsync(x => x.Id == id);
                
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                foreach (string file in Directory.EnumerateFiles(
                                  path,
                                     "*",
                              SearchOption.AllDirectories))
                {
                    if (file.Split('\\')[5] == query.ImagePath)
                    {
                        var contents = _fileProvider.GetDirectoryContents(file);
                        var pathc = file;
                        byte[] bytes = System.IO.File.ReadAllBytes(pathc);
                        var newFile = Convert.ToBase64String(bytes);
                        query.ImagePath = newFile;

                        return query;
                    }
                }
                return null;
            }
        }
        public async Task<IEnumerable<CEvent>> GetCSV(int id)
        {
            var data = await DummyData(id);
            return data;
        }
        public async Task<IEnumerable<CEvent>> DummyData(int id)
        {
            List<CEvent> model = new List<CEvent>();
            using (db)
            {
                List<CEvent> query = db.CEvents.Where(x=>x.LoginID == id).ToList();

                foreach (var item in query)
                {
                    CEvent models = new CEvent();
                    models.Title = item.Title;
                    models.Description = item.Description;
                    models.StartDate = item.StartDate;
                    models.EndDate = item.EndDate;
                    models.ImagePath = item.ImagePath;
                    model.Add(models);
                };
            }
            return model;  
        }

        public async Task<IEnumerable<CEvent>> importCSV(List<CEvent> model,int id)
        {
            List<CEvent> data = model;
            using (db)
            {
                foreach (var item in data)
                {
                    CEvent obj = new CEvent();
                    obj.Title = item.Title;
                    obj.Description = item.Description;
                    obj.StartDate = item.StartDate;
                    obj.EndDate = item.EndDate;
                    obj.ImagePath = item.ImagePath;
                    obj.LoginID = item.LoginID;
                    db.CEvents.Add(obj);
                }
                db.SaveChanges();
                string ac = "Completed";
                return null;
            }
           
        }
        public async Task<CLogin> Login(int pin)
        {
            using (db)
            {
                var query = db.CLogins.Where(x => x.Pin == pin).FirstOrDefault();
                if(query != null)
                {
                    return query;
                }
                return null;
            }
        }
        public async Task<string> Upload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var FileNa = string.Empty;
            StringBuilder sb = new StringBuilder();

            foreach (var Image in files)
            {
                if (Image != null && Image.Length > 0)
                {
                    var file = Image;

                
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    if (file.Length > 0)
                    {
                        var fil = DateTime.Now.Ticks;
                        var fileName = fil + Path.GetExtension(file.FileName);
                        sb.Append(fileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                    }
                }
            }
            var ac = Convert.ToString(sb);
            return ac;
        }
    }
}
