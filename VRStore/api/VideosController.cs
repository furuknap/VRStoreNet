
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VRStore.Models;
using VRStore.ViewModels;

namespace VRStore.api
{
    public class VideosController : VRStoreBaseApiController
    {
        /// <summary>
        /// Rent a particual video, if available.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Rent(VideoRentViewModel model)
        {
            // Note that I would prefer to have a service deal with the renting 
            // and other operations on videos or video copies because 
            // this code is duplicated in the api service. I'm not doing that here
            // to avoid complexity.
            //
            // The call the could be service.Rent(model.ID, UserID, days); for both 
            // uses.
            Video video = db.Videos.Include(v => v.Copies)
                .SingleOrDefault(v => v.ID == model.ID && v.Copies.Where(c => c.RentedDate == null).Count() > 0);
            if (video == null)
            {
                return NotFound();
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

            return Json(history);
        }
        // GET: api/Videos
        public IQueryable<Video> GetVideos()
        {
            return db.Videos.Include(v => v.Copies);
        }

        // GET: api/Videos/5
        [ResponseType(typeof(Video))]
        public IHttpActionResult GetVideo(Guid id)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return NotFound();
            }

            return Ok(video);
        }

        // PUT: api/Videos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVideo(Guid id, Video video)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != video.ID)
            {
                return BadRequest();
            }

            db.Entry(video).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Videos
        [ResponseType(typeof(Video))]
        public IHttpActionResult PostVideo(Video video)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Videos.Add(video);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VideoExists(video.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = video.ID }, video);
        }

        // DELETE: api/Videos/5
        [ResponseType(typeof(Video))]
        public IHttpActionResult DeleteVideo(Guid id)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return NotFound();
            }

            db.Videos.Remove(video);
            db.SaveChanges();

            return Ok(video);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VideoExists(Guid id)
        {
            return db.Videos.Count(e => e.ID == id) > 0;
        }
    }
}