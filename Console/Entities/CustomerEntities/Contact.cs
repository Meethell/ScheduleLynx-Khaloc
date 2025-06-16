namespace Console.Entities.CustomerEntities
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Notes { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
