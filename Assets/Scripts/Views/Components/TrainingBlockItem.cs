using System;
using System.Collections.Generic;
using System.Text;
using Screens.CreateTraining;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components
{
    public class TrainingBlockItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _exercisesText;
        [SerializeField] private TMP_Text _approachesText;
        [SerializeField] private TMP_Text _setsText;
        [SerializeField] private TMP_Text _restText;
        [SerializeField] private Button _button;
        [SerializeField] private Button _changeButton;
        [SerializeField] private Button _deleteButton;

        private string _id;
        private Action<string> _onClickById;
        private Action<string> _onClickEditById;
        private Action<string> _onClickDeleteById;
        
        public void Setup(TrainingBlockViewData trainingBlockViewData, bool isEditMode,
            Action<string> onClick, Action<string> onClickEdit = null, Action<string> onClickDelete = null)
        {
            _id = trainingBlockViewData.Id;
            _onClickById = onClick;
            _onClickEditById = onClickEdit;
            _onClickDeleteById = onClickDelete;

            _nameText.text = $"Блок {transform.GetSiblingIndex()}";
            _exercisesText.text = GetExercisesText(trainingBlockViewData.ExercisesInBlockViewData);
            _approachesText.text = $"{trainingBlockViewData.Approaches} подходов по " +
                                   $"{trainingBlockViewData.ApproachesTimeSpan.ToRussianFormattedString()}," +
                                   $" отдых {trainingBlockViewData.RestAfterApproachTimeSpan.ToRussianFormattedString()}";
            _setsText.text = $"{trainingBlockViewData.Sets} сета, отдых после сета " +
                             $"{trainingBlockViewData.RestAfterSetsTimeSpan.ToRussianFormattedString()}";
            _restText.text = $"Отдых после блока " +
                             $"{trainingBlockViewData.RestAfterBlockTimeSpan.ToRussianFormattedString()}";
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => _onClickById?.Invoke(_id));
            
            _changeButton.gameObject.SetActive(isEditMode);
            _deleteButton.gameObject.SetActive(isEditMode);
            if (isEditMode)
            {
                _changeButton.onClick.RemoveAllListeners();
                _changeButton.onClick.AddListener(() => _onClickEditById?.Invoke(_id));
                
                _deleteButton.onClick.RemoveAllListeners();
                _deleteButton.onClick.AddListener(() => _onClickDeleteById?.Invoke(_id));
            }
        }

        private string GetExercisesText(List<ExerciseInBlockViewData> exercisesInBlockViewData)
        {
            StringBuilder exercisesText = new();
            for (int i = 0; i < exercisesInBlockViewData.Count; i++)
            {
                exercisesText.Append($"{exercisesInBlockViewData[i].Name} {exercisesInBlockViewData[i].Repetitions} раз");
                if (i < exercisesInBlockViewData.Count - 1)
                {
                    exercisesText.Append("+");
                }
            }
            return exercisesText.ToString();
        }
    }
}