using System.ComponentModel.DataAnnotations;

namespace MarketPlace.DataLayer.DTOs.Contact
{
   public class AnswerTicketDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }
    }

   public enum AnswerTicketResult
   {
       NotFourUser,
       NotFound,
       Success
   }
}
