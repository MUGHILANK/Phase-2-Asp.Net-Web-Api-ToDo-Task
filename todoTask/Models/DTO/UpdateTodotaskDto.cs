using System.ComponentModel;

namespace todoTask.Models.DTO
{
    public class UpdateTodotaskDto
    {
        //public Guid Id { get; set; }
        public string taskName { get; set; }

        public string taskStatus { get; set; }
    }
}
