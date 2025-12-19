using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.CreateTraining;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.Components;
using Zenject;

namespace Screens.ViewTraining
{
    public class ViewTrainingScreen : ScreenWithUpdatableViewModel<ViewTrainingViewModel, TrainingIdParameter>
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _prepTimeText;
        [Space]
        [SerializeField] private Transform _blockListParent;
        [SerializeField] private TrainingBlockItem _trainingBlockPrefab;
        [Space]
        [SerializeField] private Button _editButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _deleteButton;
        
        private ItemsGroup<TrainingBlockItem> _trainingBlockItemsGroup;
        
        [Inject]
        public void Construct(ViewTrainingViewModel viewModel, UiController uiController)
        {
            InitializeAsync(viewModel, uiController);
        }

        public override async Task InitializeAsync(ViewTrainingViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);
            
            _trainingBlockItemsGroup = new ItemsGroup<TrainingBlockItem>(_blockListParent, _trainingBlockPrefab);

            Vm.TrainingChanged += MarkDirtyOrRefresh;
            Subscribe(() => Vm.TrainingChanged -= MarkDirtyOrRefresh);

            _editButton.onClick.AddListener(() => 
                UIController.OpenScreen(ScreenType.CreateTraining, new TrainingIdParameter(Vm.TrainingId)));
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
            _deleteButton.onClick.AddListener(DeleteTraining);
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                _nameText.text = Vm.TrainingName;
                _descriptionText.text = Vm.TrainingDescription;
                _prepTimeText.text = Vm.PrepTimeText;

                IReadOnlyList<TrainingBlockViewData> trainingBlocks = Vm.GetTrainingBlocks();
                List<TrainingBlockItem> items = _trainingBlockItemsGroup.Refresh(trainingBlocks.Count);
                for (int i = 0; i < trainingBlocks.Count; i++)
                {
                    items[i].Setup(trainingBlocks[i], false);
                }
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void DeleteTraining()
        {
            UIController.CloseScreen();
            Vm.DeleteTraining();
        }
    }
}
