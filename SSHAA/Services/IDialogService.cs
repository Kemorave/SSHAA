namespace SSHAA.Services
{
    public interface IDialogService
    {
        void ShowDialog(string title, string message);
        bool ShowDialog(string title, string message, string confirmButton, string rejectButton);
    }
}
