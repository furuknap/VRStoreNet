using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VRStore.Models;

namespace VRStore.api
{
    public class VideoCopiesController : VRStoreBaseApiController
    {

        public IHttpActionResult Create(Guid videoID)
        {
            Video video = db.Videos.Find(videoID);
            if (video == null)
            {
                return NotFound();
            }

            // Shortcut, because we don't need to add any data to a new copy.
            VideoCopy copy = new VideoCopy();
            copy.Video = video;
            copy.ID = Guid.NewGuid();
            db.VideoCopies.Add(copy);
            db.SaveChanges();
            return Ok<VideoCopy>(copy);
        }

        public IHttpActionResult Return(Guid id)
        {
            VideoCopy copy = db.VideoCopies.Include(c => c.Video).Where(c => c.ID == id && c.RentedDate != null).SingleOrDefault();
            if (copy == null) // Copy not rented out
            {
                return NotFound();
            }

            copy.RentedDate = null;
            copy.RenterID = null;
            copy.RentedDays = 0;
            db.Entry(copy).State = EntityState.Modified;

            UserHistory historyEntry = db.UserHistory.Where(h => h.CopyID == copy.ID && h.ReturnedDate == null).SingleOrDefault();
            if (historyEntry == null)
            {
                // Not rented out to this user
                return NotFound();
            }
            historyEntry.ReturnedDate = DateTime.UtcNow;
            db.SaveChanges();
            return Ok();
        }
    }
}
