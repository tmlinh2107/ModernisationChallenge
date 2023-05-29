using System;
using System.Data.Linq.Mapping;

namespace ModernisationChallenge.DataAccess
{
    [Table(Name = "Tasks")]
    public class Task
    {
        [Column(CanBeNull = false, IsDbGenerated = true, IsPrimaryKey = true)]
        public int? Id { get; set; }

        [Column(CanBeNull = false)]
        public DateTime? DateCreated { get; set; }

        [Column(CanBeNull = false)]
        public DateTime? DateModified { get; set; }

        [Column(CanBeNull = true)]
        public DateTime? DateDeleted { get; set; }

        [Column(CanBeNull = false)]
        public bool? Completed { get; set; }

        [Column(CanBeNull = false)]
        public string Details { get; set; }
    }
}
