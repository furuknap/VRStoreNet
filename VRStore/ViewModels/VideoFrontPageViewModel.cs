using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRStore.ViewModels
{
    public class VideoFrontPageViewModel
    {
        public string Title { get; set; }
        public Guid ID { get; set; }
        public int AvailableCopies { get; set; }

    }
}