using System;
using System.Collections.Generic;
using System.Linq;
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

        private string _id;
        private Action<string> _onClickById;

        public void Setup(string id, string exerciseName, IReadOnlyList<(string EquipmentName, int Quantity)> equipmentData, Action<string> onClickById)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
            _onClickById = onClickById;

            _nameText.text = exerciseName ?? string.Empty;
            if (equipmentData != null && equipmentData.Count > 0)
            {
                var parts = equipmentData.Select(r => $"{r.EquipmentName} x{r.Quantity}");
                _equipmentsText.text = $"Нужно: {string.Join(", ", parts)}";
            }
            else
            {
                _equipmentsText.text = "Без оборудования";
            }

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => _onClickById?.Invoke(_id));
        }
    }
}
