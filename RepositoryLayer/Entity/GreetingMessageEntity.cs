using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RepositoryLayer.Entity
{
    public class GreetingMessageEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        //Navigation Property
        [ForeignKey("UserId")]
        public UserEntity Users { get; set; }
    }
}
