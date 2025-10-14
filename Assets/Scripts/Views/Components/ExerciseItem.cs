using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components
{
    public class ExerciseItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _equipmentsText;
        [SerializeField] private Button _button;

        private Exercise _exercise;
        private Action<Exercise> _onClick;
        
        public void Setup(Exercise exercise, IReadOnlyList<(string Id, string Name, int Quantity)> equipmentData, Action<Exercise> onClick)
        {
            _exercise = exercise ?? throw new ArgumentNullException(nameof(exercise));
            _onClick = onClick;

            _nameText.text = exercise.Name;
            if (equipmentData != null && equipmentData.Count > 0)
            {
                IEnumerable<string> parts = equipmentData.Select(r => $"{r.Name} x{r.Quantity}");
                _equipmentsText.text = $"Нужно: {string.Join(", ", parts)}";
            }
            else
            {
                _equipmentsText.text = "Без оборудования";
            }

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => _onClick?.Invoke(_exercise));
        }
    }
}