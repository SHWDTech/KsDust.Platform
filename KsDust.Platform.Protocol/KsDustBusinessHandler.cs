using System;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;

namespace KsDust.Platform.Protocol
{
    public class KsDustBusinessHandler : IBuinessHandler
    {
        public string BusinessName { get; } = Properties.Resource.BusinessName;

        public void RunHandler(IProtocolPackage package)
        {
            throw new NotImplementedException();
        }
    }
}
