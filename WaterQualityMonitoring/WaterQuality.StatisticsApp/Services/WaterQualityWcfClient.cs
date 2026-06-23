using System;
using System.Collections.Generic;
using System.ServiceModel;
using WaterQuality.Contracts.DTOs;
using WaterQuality.Contracts.Services;

namespace WaterQuality.StatisticsApp.Services
{
    public class WaterQualityWcfClient
    {
        private readonly IWaterQualityDataService service;

        public WaterQualityWcfClient()
        {
            var binding = new NetNamedPipeBinding();

            var address = new EndpointAddress(
                "net.pipe://localhost/WaterQualityMonitoring/WaterQualityDataService");

            var factory = new ChannelFactory<IWaterQualityDataService>(binding, address);

            service = factory.CreateChannel();
        }
        public List<WaterSourceDto> GetAllSources()
        {
            return service.GetAllSources();
        }

        public List<WaterQualityReadingDto> GetReadingsBySourceAndYear(Guid sourceId, int year)
        {
            return service.GetReadingsBySourceAndYear(sourceId, year);
        }
    }
}