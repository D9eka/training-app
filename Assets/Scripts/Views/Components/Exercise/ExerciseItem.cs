using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components.Exercise
{
    /// <summary>
    /// UI-компонент для отображения упражнения в списке.
    /// </summary>
    public class ExerciseItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _equipmentsText;
        [SerializeField] private Button _button;

        private Models.Exercise _exercise;
        private Action<Models.Exercise> _onClick;

        private void Awake()
        {
            if (_nameText == null) throw new MissingComponentException("NameText is not assigned");
            if (_equipmentsText == null) throw new MissingComponentException("EquipmentsText is not assigned");
            if (_button == null) throw new MissingComponentException("Button is not assigned");
        }

        /// <summary>
        /// Настраивает элемент.
        /// </summary>
        public void Setup(Models.Exercise exercise, IReadOnlyList<(string Id, string Name, int Quantity)> equipmentData, Action<Models.Exercise> onClick)
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