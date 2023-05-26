using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TomatesBuenos.Models
{
    public class TopTVshow
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TVshow")]
        public int TVshowId { get; set; }
        public TVshow TVshow { get; set; }
    }
}
