using System.Threading.Tasks;

namespace SSHAA.Services
{
    public interface IShare
    {
        Task Show(string title, string message, string filePath);
    }
}
