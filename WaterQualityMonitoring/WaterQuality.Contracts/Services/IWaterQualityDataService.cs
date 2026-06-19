using System;
using System.Collections.Generic;
using System.ServiceModel;
using WaterQuality.Contracts.DTOs;

namespace WaterQuality.Contracts.Services
{
    [ServiceContract]
    public interface IWaterQualityDataService
    {
        [OperationContract]
        List<WaterSourceDto> GetAllSources();

        [OperationContract]
        List<WaterQualityReadingDto> GetReadingsBySourceAndYear(Guid sourceId, int year);
    }
}