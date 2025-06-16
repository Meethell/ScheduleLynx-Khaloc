using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entities.ModelEntities
{
    public class SpareParts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public int QuantityInStock { get; set; }


        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

    }
}
