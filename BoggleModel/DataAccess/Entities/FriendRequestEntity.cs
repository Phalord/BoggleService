using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoggleModel.DataAccess.Entities
{
    [Table("FriendRequests")]
    public class FriendRequestEntity
    {
        [Key]
        public int FriendRequestID { get; set; }

        public string IsAccepted { get; set; }

        public virtual PlayerEntity Sender { get; set; }

        public virtual PlayerEntity Receiver { get; set; }
    }
}