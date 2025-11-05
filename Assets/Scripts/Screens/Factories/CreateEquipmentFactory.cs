using Data;
using Models;
using Screens.CreateEquipment;
using Screens.Factories.Parameters;

namespace Screens.Factories
{
    public class CreateEquipmentFactory : IViewModelFactory<CreateEquipmentViewModel, IScreenParameter>
    {
        private readonly IDataService<Equipment> _exerciseService;

        public CreateEquipmentFactory(IDataService<Equipment> exerciseService)
        {
            _exerciseService = exerciseService;
        }
        
        public CreateEquipmentViewModel Create(IScreenParameter param)
        {
            return new CreateEquipmentViewModel(_exerciseService);
        }
    }
}