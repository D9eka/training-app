using System;
using System.Text;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components.Exercise
{
    public class ExerciseItem : MonoBehaviour
    {
        [SerializeField] private Button _itemButton;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _requiredEqText;

        private Models.Exercise _exercise;

        public void Setup(Models.Exercise exercise, Action<Models.Exercise> onClick)
        {
            _exercise = exercise;

            _nameText.text = exercise.Name;

            StringBuilder sb = new StringBuilder("Нужно: ");
            foreach (ExerciseEquipment req in exercise.RequiredEquipment)
            {
                sb.Append(req.Equipment.Name);
                if (req.Equipment.HasQuantity) sb.Append($" ({req.Quantity})");
                sb.Append(", ");
            }
            if (exercise.RequiredEquipment.Count > 0) sb.Length -= 2; // Убрать последнюю запятую
            _requiredEqText.text = sb.ToString();

            _itemButton.onClick.AddListener(() => onClick?.Invoke(_exercise));
        }
    }
}