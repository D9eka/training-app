using Models;

namespace Data
{
    public class EquipmentDataService : BaseDataService<Equipment>
    {
        public EquipmentDataService(ISaveService saveService) : base(saveService.EquipmentsCache)
        {
        }
    }
}