using System.ComponentModel;

namespace todoTask.Models.DTO
{
    public class AddTodotaskDto
    {
        //public Guid Id { get; set; }
        public string taskName { get; set; }
        [DefaultValue("Pending")]
        public string taskStatus { get; set; }
    }
}
