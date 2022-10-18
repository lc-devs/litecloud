using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Inventory_Common
    {
        public Inventory_Common() { }

        public enum Type : ushort
        {
            Categories = 1,
            Ratios = 2,
            Sizes = 3,
            ThreadPatterns = 4,
            ValveTypes = 5,
            VehiclePartNumbers = 6,
            VehicleParts =7
        }

    }
}
