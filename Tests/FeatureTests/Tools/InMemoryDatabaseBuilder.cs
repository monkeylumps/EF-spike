using System;
using EF_Spike.DatabaseContext;

namespace FeatureTests.Tools
{
    public class InMemoryDatabaseBuilder
    {
        public void AddEntityToDb<T>(T entity, RegistryContext context) where T : class
        {
            context.Add(entity);
            context.SaveChanges();

        }
    }
}