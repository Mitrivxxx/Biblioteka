using Android.Content;
using Android.Hardware;
using Android.Runtime;
using BibliotekaPro;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(LightSensorService))]
namespace BibliotekaPro
{
    public class LightSensorService : Java.Lang.Object, ILightSensorService, ISensorEventListener
    {
        private readonly SensorManager _sensorManager;
        private readonly Sensor _lightSensor;

        public event EventHandler<float> LightSensorChanged;

        public LightSensorService()
        {
            _sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService(Context.SensorService);
            _lightSensor = _sensorManager.GetDefaultSensor(SensorType.Light);
        }

        public void StartListening()
        {
            _sensorManager.RegisterListener(this, _lightSensor, SensorDelay.Normal);
        }

        public void StopListening()
        {
            _sensorManager.UnregisterListener(this, _lightSensor);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            // Możesz zignorować tę metodę lub dodać obsługę zmian dokładności czujnika
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == SensorType.Light)
            {
                var lightLevel = e.Values[0]; // Uzyskanie wartości natężenia światła
                LightSensorChanged?.Invoke(this, lightLevel);
            }
        }
    }
}
