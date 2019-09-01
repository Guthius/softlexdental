namespace OpenDentBusiness
{
    public interface IProgressHandler
    {
        void UpdateBytesRead(long numBytes);

        void DisplayError(string error);

        void CloseProgress();
    }
}