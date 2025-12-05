namespace Framework.Controller.Interfaces
{
    public interface IInterfaceController
    {
        bool IsOpen { get; }
        bool CanOpen();
        void OpenPanel();
        void ClosePanel();
    }
}