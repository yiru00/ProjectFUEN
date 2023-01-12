using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string EventName { get; set; }

        public string Photo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
    public static partial class EventExts
    {
        public static EventDto ToDto(this Event source)
        {
            return new EventDto
            {
                Id = source.Id,
                EventName = source.EventName,
                Photo = source.Photo,
                StartDate = source.StartDate,
                EndDate = source.EndDate
            };
        }

        public static EventDto ToDto(this EventVM source)
        {
            return new EventDto
            {
                Id = source.Id,
                EventName = source.EventName,
                Photo = source.Photo,
                StartDate = source.StartDate,
                EndDate = source.EndDate
            };
        }
    }

}
