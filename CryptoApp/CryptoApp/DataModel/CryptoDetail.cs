using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoApp.DataModel
{
    public class CryptoDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Pair { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime UpdateDate { get; set; }
    }

}