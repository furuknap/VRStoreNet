using System;

namespace VRStore.Models
{
    public class VideoCopy : Entity
    {
        public virtual Video Video { get; set; }
        public Guid VideoID { get; set; }
        public DateTime? RentedDate { get; set; }
        public int RentedDays { get; set; }
        public Guid? RenterID { get; set; }

        // I'm explicitly not adding history here to avoid complexity. 
        //
        // History is already included in the user history so we can query that to 
        // get specific history for a copy

    }
}