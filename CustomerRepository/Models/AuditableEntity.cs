using System;
namespace CustomerRepository.Models
{
    public class AuditableEntity
    {
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }

        protected AuditableEntity()
        {
        }
    }
}