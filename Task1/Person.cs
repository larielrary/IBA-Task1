using System.ComponentModel.DataAnnotations;


namespace Task1
{
   
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        
        public Person(string date, string firstName, string lastName, string surname, string city, string country)
        {
            Date = date;
            FirstName = firstName;
            LastName = lastName;
            Surname = surname;
            City = city;
            Country = country;
        }
    }
}
