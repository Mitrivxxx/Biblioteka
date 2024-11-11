using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotekaPro
{
    public interface ILightSensorService
    {
        void StartListening();
        void StopListening();
        event EventHandler<float> LightSensorChanged;
    }
}
