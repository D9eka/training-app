using System;
using System.Collections.Generic;
using System.Linq;
using Screens.ViewExercises;
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

        public void Setup(ExerciseViewData exerciseViewData, Action<string> onClickById)
        {
            _id = exerciseViewData.Id ?? throw new ArgumentNullException(nameof(exerciseViewData.Id));
            _onClickById = onClickById;

            _nameText.text = exerciseViewData.Name;
            if (exerciseViewData.Equipments != null && exerciseViewData.Equipments.Count > 0)
            {
                var parts = exerciseViewData.Equipments.Select(
                    r => $"{r.EquipmentName} x{r.Quantity}");
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
