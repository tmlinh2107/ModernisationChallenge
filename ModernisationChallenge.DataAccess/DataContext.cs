using System;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ModernisationChallenge.DataAccess
{
    public class DataContext : System.Data.Linq.DataContext
    {
        private readonly static MappingSource mappingSource = new AttributeMappingSource();

        public DataContext()
            : base(ConfigurationManager.ConnectionStrings["ModernisationChallenge"].ConnectionString, DataContext.mappingSource)
        {
        }

        public Table<Task> Tasks
        {
            get
            {
                return GetTable<Task>();
            }
        }
    }
}
