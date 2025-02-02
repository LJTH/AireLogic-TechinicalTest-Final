using System.ComponentModel.DataAnnotations;

namespace PANDA.Repository.Model
{
    public abstract class EntityModelBase
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedDateTime { get; set; }
    }
}
