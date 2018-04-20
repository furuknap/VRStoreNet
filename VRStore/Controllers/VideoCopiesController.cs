using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VRStore.Models;
using VRStore.ViewModels;

namespace VRStore.Controllers
{
    public class VideoCopiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VideoCopies
        public ActionResult Index()
        {
            return View(db.VideoCopies.ToList());
        }


        public ActionResult Return(Guid id)
        {
            VideoCopy copy = db.VideoCopies.Include(c=>c.Video).Where(c => c.RentedDate != null).SingleOrDefault();
            if (copy== null) // Copy not rented out
            {
                return HttpNotFound();
            }
            Video video = db.Videos.Find(copy.Video.ID);
            if (video == null)
            {
                return HttpNotFound();
            }

            UserHistory historyEntry = db.UserHistory.Where(h => h.CopyID == copy.ID && h.ReturnedDate == null).SingleOrDefault();
            if (historyEntry == null)
            {
                return HttpNotFound();
            }

            


            return RedirectToAction("Details", "Videos", new { id = video.ID });
            
        }


        // GET: VideoCopies/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoCopy videoCopy = db.VideoCopies.Find(id);
            if (videoCopy == null)
            {
                return HttpNotFound();
            }
            return View(videoCopy);
        }

        // GET: VideoCopies/Create
        public ActionResult Create(Guid videoID)
        {
            Video video = db.Videos.Find(videoID);
            if (video==null)
            {
                return HttpNotFound();
            }

            // Shortcut, because we don't need to add any data to a new copy.
            VideoCopy copy = new VideoCopy();
            copy.Video = video;
            copy.ID = Guid.NewGuid();
            db.VideoCopies.Add(copy);
            db.SaveChanges();
            return RedirectToAction("Details", "Videos", new { id = video.ID });

            // This shows as not reachable because of the above shortcut.
            var model = new VideoFrontPageViewModel();

            return View();
        }

        // POST: VideoCopies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RentedDate,RentedDays")] VideoCopy videoCopy)
        {
            if (ModelState.IsValid)
            {
                videoCopy.ID = Guid.NewGuid();
                db.VideoCopies.Add(videoCopy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(videoCopy);
        }

        // GET: VideoCopies/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoCopy videoCopy = db.VideoCopies.Find(id);
            if (videoCopy == null)
            {
                return HttpNotFound();
            }
            return View(videoCopy);
        }

        // POST: VideoCopies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RentedDate,RentedDays")] VideoCopy videoCopy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(videoCopy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(videoCopy);
        }

        // GET: VideoCopies/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoCopy videoCopy = db.VideoCopies.Find(id);
            if (videoCopy == null)
            {
                return HttpNotFound();
            }
            return View(videoCopy);
        }

        // POST: VideoCopies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VideoCopy videoCopy = db.VideoCopies.Find(id);
            db.VideoCopies.Remove(videoCopy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
