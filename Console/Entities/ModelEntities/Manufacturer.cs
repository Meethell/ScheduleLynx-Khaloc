
namespace Console.Entities.ModelEntities
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set;}
        public int CountryId { get; set; }
        public string Descriptions { get; set; }

    }
}
