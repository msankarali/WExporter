using Core.DataAccess.EntityFramework;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WExporter.Entities.SampleConcrete;

namespace Infrastructure.Repositories
{
    public class SampleRepository : EfEntityRepositoryBase<Sample, int>, ISampleRepository
    {
        private readonly SampleDbContext _dbContext;

        public SampleRepository(SampleDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Sample GetSampleById(int id)
        {
            using (_dbContext)
            {
                
            }
            return GetFirst(x => x.Id == id);
        }
    }

    public interface ISampleRepository : IEntityRepository<Sample, int>
    {
        Sample GetSampleById(int id);
    }
}