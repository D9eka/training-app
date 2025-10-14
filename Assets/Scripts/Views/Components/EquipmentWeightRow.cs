using System.Globalization;
using Models;
using TMPro;
using UnityEngine;

namespace Views.Components
{
    public class EquipmentWeightRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _equipmentText;
        [SerializeField] private TMP_InputField _equipmentWeightInputField;
        [SerializeField] private TMP_Dropdown _equipmentWeightTypeDropdown;
        
        private ExerciseEquipmentRef _currentEquipment;

        public void Setup(ExerciseEquipmentRef equipment, string equipmentName, float weight = 0f, WeightType weightType = WeightType.Kg)
        {
            _currentEquipment  = equipment;
            
            _equipmentText.text = $"{_currentEquipment.Quantity}x {equipmentName}";
            _equipmentWeightInputField.text = weight.ToString(CultureInfo.InvariantCulture);
            _equipmentWeightTypeDropdown.SetValueWithoutNotify((int)weightType);
        }

        public EquipmentInBlock GetEquipmentInBlock()
        {
            int equipmentWeight = int.Parse(_equipmentWeightInputField.text);
            return new EquipmentInBlock(_currentEquipment, equipmentWeight, (WeightType)_equipmentWeightTypeDropdown.value);
        }
    }
}