namespace SehatNotebook.Entities.DBSet{
  public class User:BaseEntity{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
     }
}