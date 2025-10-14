using System;
using System.Text;
using Models;
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

        private TrainingBlock _trainingBlock;
        
        public void Setup(TrainingBlock trainingBlock, Action<TrainingBlock> onClick)
        {
            _trainingBlock = trainingBlock;

            _nameText.text = $"Блок {transform.GetSiblingIndex()}";
            _exercisesText.text = GetExercisesText();
            _approachesText.text = $"{_trainingBlock.Approaches} подходов по {_trainingBlock.ApproachesTimeSeconds}," +
                                   $" отдых {_trainingBlock.RestAfterApproachSeconds} секунд";
            _setsText.text = $"{_trainingBlock.Sets} сета, отдых после сета {_trainingBlock.RestAfterSetSeconds} секунд";
            _restText.text = $"Отдых после блока {_trainingBlock.RestAfterBlockSeconds} секунд";
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => onClick?.Invoke(_trainingBlock));
        }

        private string GetExercisesText()
        {
            StringBuilder exercisesText = new();
            for (int i = 0; i < _trainingBlock.Exercises.Count; i++)
            {
                Models.ExerciseInBlock trainingBlockExercise = _trainingBlock.Exercises[i];
                exercisesText.Append(trainingBlockExercise.Exercise.Name);
                exercisesText.Append($"{trainingBlockExercise.Repetitions} раз");
                if (i < _trainingBlock.Exercises.Count - 1)
                {
                    exercisesText.Append("+");
                }
            }
            return exercisesText.ToString();
        }
    }
}