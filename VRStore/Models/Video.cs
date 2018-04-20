using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VRStore.Models
{
    public class Video : Entity
    {
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public VideoAge Age {
            get {
                if (ReleaseDate>DateTime.UtcNow.AddDays(0-GlobalValues.NewReleaseAge)) 
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

        public List<VideoCopy> Copies { get; set; } = new List<VideoCopy>();
    }

    public enum VideoAge
    {
        Old,
        Regular,
        New
    }
}
