using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Screens.Factories.Parameters;
using Screens.ViewExercise;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.ViewTraining
{
    public class ViewTrainingScreen : ScreenWithViewModel<ViewTrainingViewModel>
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

        public override async Task InitializeAsync(ViewTrainingViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

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
                
                foreach (Transform child in _blockListParent)
                    if (child.TryGetComponent(out TrainingBlockItem _))
                        SimplePool.Return(child.gameObject, _trainingBlockPrefab.gameObject);

                foreach (var block in Vm.GetTrainingBlocks())
                {
                    var go = SimplePool.Get(_trainingBlockPrefab.gameObject, _blockListParent);
                    var item = go.GetComponent<TrainingBlockItem>();
                    item.Setup(block, false);
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