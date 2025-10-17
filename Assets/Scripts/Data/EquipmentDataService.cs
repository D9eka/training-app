using System.Collections.Generic;
using Models;

namespace Data
{
    public class EquipmentDataService : BaseDataService<Equipment>
    {
        public EquipmentDataService(List<Equipment> cache) : base(cache)
        {
        }
    }
}