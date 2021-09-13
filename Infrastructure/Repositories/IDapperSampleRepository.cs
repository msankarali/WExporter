using Core.DataAccess.Dapper;
using WExporter.Entities.SampleConcrete;

namespace Infrastructure.Repositories
{
    public interface IDapperSampleRepository : IDapperRepository<Sample, int>
    {

    }
}