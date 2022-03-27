using System;

namespace ListaZaginionych.Data.Models
{
    public class LostPeopleModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string LastSeenLocation { get; set; }
        public DateTime LastSeenDate { get; set; }
        public string Image { get; set; }
    }
}
