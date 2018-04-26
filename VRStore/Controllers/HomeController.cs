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
    public class HomeController : VRStoreBaseController
    {
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();
            model.Videos = db.Videos.Include(v => v.Copies).ToList()
                .Select(v => new VideoFrontPageViewModel
                {
                    AvailableCopies = v.Copies.Where(c => c.RentedDate == null).Count(),
                    ID = v.ID,
                    Title = v.Title

                }).ToList();

            return View(model);
        }

        public ActionResult ReadMe()
        {
            return View();
        }



    }
}