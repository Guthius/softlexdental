using System.Collections.Generic;

namespace OpenDentBusiness
{
    public interface ISignalProcessor
    {
        void ProcessObjects(List<Signalod> signals);
    }
}
