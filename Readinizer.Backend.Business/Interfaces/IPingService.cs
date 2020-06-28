namespace Readinizer.Backend.Business.Interfaces
{
    public interface IPingService
    {
        bool IsPingable(string ipAddress);
    }
}
