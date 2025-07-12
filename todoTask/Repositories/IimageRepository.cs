using System.Net;
using todoTask.Models.Domain;

namespace todoTask.Repositories
{
    public interface IimageRepository
    {
        Task <Image>Upload(Image image);
    }
}
