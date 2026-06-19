using System;
using System.ServiceModel;
using WaterQuality.Contracts.Services;

namespace WaterQuality.InformationSystem.Services
{
    public class WcfHostService
    {
        private ServiceHost _serviceHost;

        public void Start()
        {
            if (_serviceHost != null)
            {
                return;
            }

            Uri baseAddress = new Uri("net.pipe://localhost/WaterQualityMonitoring");

            _serviceHost = new ServiceHost(typeof(WaterQualityDataService), baseAddress);

            NetNamedPipeBinding binding = new NetNamedPipeBinding();

            _serviceHost.AddServiceEndpoint(
                typeof(IWaterQualityDataService),
                binding,
                "WaterQualityDataService");

            _serviceHost.Open();
        }

        public void Stop()
        {
            if (_serviceHost == null)
            {
                return;
            }

            if (_serviceHost.State == CommunicationState.Opened)
            {
                _serviceHost.Close();
            }
            else
            {
                _serviceHost.Abort();
            }

            _serviceHost = null;
        }
    }
}