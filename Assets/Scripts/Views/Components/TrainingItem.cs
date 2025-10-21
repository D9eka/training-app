using System;
using Screens.ViewExercises;
using Screens.ViewTrainings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components
{
    public class TrainingItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _exerciseCountText;
        [SerializeField] private TMP_Text _timeDurationText;
        [SerializeField] private TMP_Text _lastTimeStartText;
        [SerializeField] private Button _button;
        
        private string _id;
        private Action<string> _onClickById;

        public void Setup(TrainingViewData trainingViewData, Action<string> onClickById)
        {
            _id = trainingViewData.Id ?? throw new ArgumentNullException(nameof(trainingViewData.Id));
            _onClickById = onClickById;
            
            _nameText.text = trainingViewData.Name;
            _exerciseCountText.text = $"{trainingViewData.ExerciseCount} упражнений";
            _timeDurationText.text = $"{trainingViewData.Duration.ToRussianFormattedString()}";
            _lastTimeStartText.text = $"Последний раз: {trainingViewData.LastTime.Date.ToShortDateString()}";
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => _onClickById?.Invoke(_id));
        }
    }
}