using System.Net.NetworkInformation;
using Readinizer.Backend.Business.Interfaces;


namespace Readinizer.Backend.Business.Services
{
    public class PingService : IPingService
    {
        public bool IsPingable(string ipAddress)
        {
            if (ipAddress == null)
            {
                return false;
            }

            var isPingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                var reply = pinger.Send(ipAddress, 500);
                if (reply != null) isPingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
            finally
            {
                pinger?.Dispose();
            }

            return isPingable;
        }

    }
}