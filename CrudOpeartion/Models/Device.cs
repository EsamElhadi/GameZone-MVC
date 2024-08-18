using System.ComponentModel.DataAnnotations;

namespace CrudOpeartion.Models
{
    public class Device
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Icon { get; set; } = string.Empty;
    }
}
