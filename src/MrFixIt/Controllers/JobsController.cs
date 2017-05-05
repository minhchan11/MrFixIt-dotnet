using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MrFixIt.Controllers
{
    public class JobsController : Controller
    {
        private MrFixItContext db = new MrFixItContext();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(db.Jobs.Include(i => i.Worker).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            db.Jobs.Add(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Change all 'item' to 'job'
        public IActionResult Claim(int id)
        {
            var thisJob = db.Jobs.FirstOrDefault(jobs => jobs.JobId == id);
            return View(thisJob);
        }

        [HttpPost]
        public IActionResult Claim(int jobId, string userName)
        {

            //job.Worker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);
            //Since Ajax is used, the parameters should be changed to int and string
            var thisJob = db.Jobs.FirstOrDefault(jobs => jobs.JobId == jobId);
            thisJob.Worker = db.Workers.FirstOrDefault(i => i.UserName == userName);
            db.Entry(thisJob).State = EntityState.Modified;
            db.SaveChanges();
            return Json(thisJob.Worker);
        }

        //Create new get method for detail page
        public IActionResult Details(int id)
        {
            var thisJob = db.Jobs.FirstOrDefault(jobs => jobs.JobId == id);
            return View(thisJob);
        }

        //Create new post methods for detail page using Ajax
        [HttpPost]
        public IActionResult TogglePending(int jobId)
        {
            var thisJob = db.Jobs.FirstOrDefault(jobs => jobs.JobId == jobId);
            if (thisJob.Pending == true)
            {
                thisJob.Pending = false;
            } else { thisJob.Pending = true; };
            db.Entry(thisJob).State = EntityState.Modified;
            db.SaveChanges();
            var editedJob = db.Jobs.FirstOrDefault(jobs => jobs.JobId == jobId);
            return Json(editedJob);
        }

        [HttpPost]
        public IActionResult ToggleComplete(int jobId)
        {
            var thisJob = db.Jobs.FirstOrDefault(jobs => jobs.JobId == jobId);
            if (thisJob.Completed == true)
            {
                thisJob.Completed = false;
            }
            else { thisJob.Completed = true; };
            db.Entry(thisJob).State = EntityState.Modified;
            db.SaveChanges();
            var editedJob = db.Jobs.FirstOrDefault(jobs => jobs.JobId == jobId);
            return Json(editedJob);
        }
    }
}
