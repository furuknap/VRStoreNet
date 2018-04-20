using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VRStore.Models;

namespace VRStore.ViewModels
{
    public class VideoDetailViewModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public VideoAge Age
        {
            get
            {
                if (ReleaseDate > DateTime.UtcNow.AddDays(0 - GlobalValues.NewReleaseAge))
                {
                    return VideoAge.New;
                }
                else if (ReleaseDate > DateTime.UtcNow.AddDays(0 - GlobalValues.RegularReleaseAge))
                {
                    return VideoAge.Regular;
                }
                else
                {
                    return VideoAge.Old;
                }

            }

        }
        public List<VideoDetailCopyViewModel> Copies { get; set; }

    }

    public class VideoDetailCopyViewModel
    {
        public Guid ID { get; set; }
        public DateTime? RentedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public double DueLateRent { get; set; }
        public Guid RenterID { get; set; }

    }
}