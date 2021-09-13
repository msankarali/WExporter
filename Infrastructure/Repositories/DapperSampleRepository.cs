using Core.DataAccess.Dapper;
using Microsoft.Extensions.Configuration;
using WExporter.Entities.SampleConcrete;

namespace Infrastructure.Repositories
{
    public class DapperSampleRepository : BaseDapperRepository<Sample, int>, IDapperSampleRepository
    {
        public DapperSampleRepository(IConfiguration configuration) : base(configuration, "Samples")
        {
        }
    }
}