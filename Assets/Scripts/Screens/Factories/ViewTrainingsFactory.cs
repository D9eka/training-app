using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewExercises;
using Screens.ViewTrainings;

namespace Screens.Factories
{
    public class ViewTrainingsFactory : IViewModelFactory<ViewTrainingsViewModel, IScreenParameter>
    {
        private readonly IDataService<Training> _trainingService;

        public ViewTrainingsFactory(IDataService<Training> trainingService)
        {
            _trainingService = trainingService;
        }
        
        public ViewTrainingsViewModel Create(IScreenParameter param)
        {
            return new ViewTrainingsViewModel(_trainingService);
        }
    }
}