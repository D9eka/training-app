using Data;
using Models;

namespace Core.Installers
{
    public class DataServiceInstaller: IInstaller
    {
        public void Install(DiContainer diContainer)
        {
            ISaveService saveService = new SaveService();
            IDataService<Equipment> equipmentDataService = new EquipmentDataService(saveService.EquipmentsCache);
            IDataService<Exercise> exerciseDataService = new ExerciseDataService(saveService.ExercisesCache);
            TrainingDataService trainingDataService = new TrainingDataService(saveService.TrainingsCache);
            DataService data = new DataService(saveService, equipmentDataService, exerciseDataService, trainingDataService);
            
            diContainer.Register(saveService);
            diContainer.Register(equipmentDataService);
            diContainer.Register(exerciseDataService);
            diContainer.Register<IDataService<Training>>(trainingDataService);
            diContainer.Register(trainingDataService);
            diContainer.Register(data);
        }
    }
}