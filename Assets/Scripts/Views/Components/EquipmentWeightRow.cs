using System;
using System.Globalization;
using Models;
using Screens.CreateBlock;
using TMPro;
using UnityEngine;

namespace Views.Components
{
    public class EquipmentWeightRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _equipmentText;
        [SerializeField] private TMP_InputField _equipmentWeightInputField;
        [SerializeField] private TMP_Dropdown _equipmentWeightTypeDropdown;
        
        private EquipmentInBlockViewData  _equipmentInBlockViewData;

        private Action<float> _onWeightChanged;
        private Action<WeightType> _onTypeChanged;
        
        //TODO: Заменить Input веса на Dropdown для целых кг и десятых кг: |72| , |25|

        public void Setup(EquipmentInBlockViewData equipmentInBlockViewData, 
            Action<(string Id, float Weight, WeightType WeightType)> onWeightChanged)
        {
            _equipmentInBlockViewData = equipmentInBlockViewData;
            
            _equipmentText.text = $"{equipmentInBlockViewData.Quantity}x {equipmentInBlockViewData.Name}";
            
            _equipmentWeightInputField.onValueChanged.RemoveAllListeners();
            _equipmentWeightInputField.text = equipmentInBlockViewData.Weight.ToString(CultureInfo.CurrentCulture);
            _equipmentWeightInputField.onValueChanged.AddListener((_) => onWeightChanged?.Invoke(GetWeightData()));
            
            _equipmentWeightTypeDropdown.onValueChanged.RemoveAllListeners();
            _equipmentWeightTypeDropdown.SetValueWithoutNotify((int)equipmentInBlockViewData.WeightType);
            _equipmentWeightTypeDropdown.onValueChanged.AddListener((_) => onWeightChanged?.Invoke(GetWeightData()));
            
        }

        private (string Id, float Weight, WeightType WeightType) GetWeightData()
        {
            return 
            (
                _equipmentInBlockViewData.Id, 
                float.Parse(_equipmentWeightInputField.text, 
                CultureInfo.CurrentCulture), (WeightType)_equipmentWeightTypeDropdown.value
            );
        }
    }
}