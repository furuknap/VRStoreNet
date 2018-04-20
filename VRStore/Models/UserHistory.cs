using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRStore.Models
{
    public class UserHistory : Entity
    {
        public Guid UserID { get; set; }
        public VideoCopy Copy { get; set; }
        public Guid CopyID { get; set; }
        public DateTime RentedDate { get; set; }
        public int RentedDays { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int PointsEarned { get; set; } 

    }
}
