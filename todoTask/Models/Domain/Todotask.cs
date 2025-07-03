using System.ComponentModel;

namespace todoTask.Models.Domain
{
    public class Todotask
    {
        public Guid Id { get; set; }
        public string taskName {  get; set; }

        [DefaultValue("Pending")]
        public string taskStatus { get; set; }

    }
}
