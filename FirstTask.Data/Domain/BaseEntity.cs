using System;

namespace FirstTask.Data.Domain
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
