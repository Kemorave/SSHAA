using System.Threading.Tasks;
using Xamarin.Forms;

namespace SSHAA.Services
{
    public interface IQRCodeServices
    {
        Task<bool> SaveImage(string v,string filename);

        ImageSource GetImage(string content);
    }
}
