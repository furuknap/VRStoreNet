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
    [Authorize]
    public class VideosController : VRStoreBaseController
    {
        // GET: Videos
        public ActionResult Index()
        {
            return View(db.Videos.Include(e => e.Copies).ToList());
        }

        public ActionResult Rent(Guid id)
        {
            // For the data context (database), get the videos, 
            // including the copies, and get the one which has an 
            // id matching the id we're requesting and has copies
            // that aren't rented out.
            Video video = db
                .Videos
                .Include(v => v.Copies)
                .SingleOrDefault(v => v.ID == id && v.Copies.Where(c => c.RentedDate == null).Count() > 0);

            if (video == null)
            {
                // If there are no such videos, we've likely just rented the last copy.
                return HttpNotFound();
            }
            return View(video);
        }
        [HttpPost]
        public ActionResult Rent(VideoRentViewModel model)
        {
            Video video = db.Videos.Include(v => v.Copies)
                .SingleOrDefault(v => v.ID == model.ID && v.Copies.Where(c => c.RentedDate == null).Count() > 0);
            if (video == null)
            {
                return HttpNotFound();
            }

            DateTime now = DateTime.UtcNow;
            VideoCopy copy = video.Copies.Where(c => c.RentedDate == null).First();
            copy.RentedDate = now;
            copy.RenterID = UserID;
            copy.RentedDays = model.Days;
            db.Entry(copy).State = EntityState.Modified;

            UserHistory history = new UserHistory();
            history.Copy = copy;
            history.ID = Guid.NewGuid();
            history.PointsEarned = (video.Age == VideoAge.New ? 2 : 1);
            history.RentedDate = now;
            history.RentedDays = model.Days;
            history.UserID = UserID;
            db.UserHistory.Add(history);
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        // I'm overriding authorization to allow external apps to query the price.
        [OverrideAuthorization]
        public ActionResult GetPrice(Guid videoID, int days)
        {
            return Json(CalculatePrice(videoID, days), JsonRequestBehavior.AllowGet);
        }

        private object CalculatePrice(Guid videoID, int days)
        {
            double price = 0;
            int daysToCharge = days;
            var video = db.Videos.Find(videoID);
            if (video.Age == VideoAge.Old) // Normalize old movies days to charge.
            {
                price += GlobalValues.RegularReleasePrice;
                daysToCharge -= 5;
                if (daysToCharge < 0)
                {
                    daysToCharge = 0;
                }
            }

            price += daysToCharge * (video.Age == VideoAge.New ? GlobalValues.NewReleasePrice : GlobalValues.RegularReleasePrice);

            return price;
        }



        // GET: Videos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos
                .Include(v => v.Copies)
                .SingleOrDefault(v => v.ID == id);
            if (video == null)
            {
                return HttpNotFound();
            }

            var model = new VideoDetailViewModel
            {
                ReleaseDate = video.ReleaseDate,
                ID = video.ID,
                Title = video.Title,
                Copies = video.Copies.Select(c =>
                new VideoDetailCopyViewModel
                {
                    ID = c.ID,
                    RentedDate = c.RentedDate,
                    DueDate = c.RentedDate != null ? ((DateTime)c.RentedDate).AddDays(c.RentedDays) : default(DateTime),
                    DueLateRent = GetLateFee(c),
                    RenterID = c.RenterID
                }).ToList()
            };

            return View(model);
        }

        private double GetLateFee(VideoCopy copy)
        {
            double lateFee = 0;
            DateTime dueDate = copy.RentedDate != null ? ((DateTime)copy.RentedDate).AddDays(copy.RentedDays) : default(DateTime);
            if (dueDate != default(DateTime) && dueDate < DateTime.UtcNow)
            {
                // Late return

                // I'm assuming that for old films, we will still charge 
                // late rent even if their rental period is shorter than the 
                // minimum rental time. IE, if they say the want to rent for 2 days,
                // they still pay the minimum of 5 days, and if they return after 4 days, 
                // they get charged for 2 days late rent.
                TimeSpan overdue = DateTime.UtcNow - dueDate;


                lateFee = (overdue.Days + 1) * (copy.Video.Age == VideoAge.New ? GlobalValues.NewReleasePrice : GlobalValues.RegularReleasePrice);
            }
            return lateFee;
        }

        // GET: Videos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,ReleaseDate")] Video video)
        {
            if (ModelState.IsValid)
            {
                video.ID = Guid.NewGuid();
                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(video);
        }

        // GET: Videos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,ReleaseDate")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(video);
        }

        // GET: Videos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
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
