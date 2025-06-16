using Console.Entities.CustomerEntities;
using System;

namespace Console.Entities.ModelEntities
{
    public class Instrument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InstallDate { get; set; }
        public string SerialNumber { get; set; }
        public DateTime LastPmDate { get; set; }
        public DateTime NextPmDate { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Model Model { get; set; }
        public int ModelId { get; set; }
        public InstrumentStatus Status { get; set; }
        public int StatusId { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        // Được tạo bởi
        public int UserId { get; set; }

    }
}
