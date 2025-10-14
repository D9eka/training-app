using System;
using System.Collections.Generic;
using System.Text;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Views.Components
{
    public class ExerciseInBlock : MonoBehaviour
    {
        [SerializeField] private TMP_Text _header;
        
        [Space]
        [SerializeField] private TMP_Text _exerciseNameText;
        [SerializeField] private TMP_Text _equipmentsText;
        
        [Space] 
        [SerializeField] private Transform _weightRowParent;
        [SerializeField] private EquipmentWeightRow _weightRowPrefab;
        
        [Space]
        [SerializeField] private TMP_InputField _repetitionsInputField;
        [SerializeField] private TMP_InputField _durationSecondsInputField;
        [SerializeField] private Button _changeExerciseButton;

        private Exercise _currentExercise;
        private List<EquipmentWeightRow> _weights;

        public void Setup(Exercise exercise, List<(string Name, int Quantity)> equipments,
            List<(ExerciseEquipmentRef Ref, string Name)> equipmentsWithWeightData, 
            Action onChangeExerciseClick, int repetitions = 0, int durationSeconds = 0)
        {
            _currentExercise = exercise;
            _changeExerciseButton.onClick.RemoveAllListeners();
            _changeExerciseButton.onClick.AddListener(onChangeExerciseClick.Invoke);
            
            _exerciseNameText.text = exercise.Name;
            _equipmentsText.text = GetEquipmentsText(equipments);
            SetupWeights(equipmentsWithWeightData);
            _repetitionsInputField.text = repetitions.ToString();
            _durationSecondsInputField.text = durationSeconds.ToString();
        }

        private string GetEquipmentsText(List<(string Name, int Quantity)> equipments)
        {
            StringBuilder equipmentsText = new StringBuilder();
            for (int i = 0; i < equipments.Count; i++)
            {
                equipmentsText.Append($"{equipments[i].Quantity}x {equipments[i].Name}");
                if (i < equipments.Count - 1)
                {
                    equipmentsText.Append(", ");
                }
            }
            return equipmentsText.ToString();
        }

        private void SetupWeights(List<(ExerciseEquipmentRef Ref, string Name)> equipmentsWithWeightData)
        {
            _weights.Clear();
            foreach (Transform t in _weightRowParent)
            {
                SimplePool.Return(t.gameObject, _weightRowPrefab.gameObject);
            }

            _weightRowParent.gameObject.SetActive(equipmentsWithWeightData.Count > 0);
            foreach ((ExerciseEquipmentRef Ref, string Name) equipmentWithWeightData in equipmentsWithWeightData)
            {
                EquipmentWeightRow equipmentWeightRow = 
                    SimplePool.Get(_weightRowPrefab.gameObject, _weightRowParent).GetComponent<EquipmentWeightRow>();
                equipmentWeightRow.Setup(equipmentWithWeightData.Ref, equipmentWithWeightData.Name);
                _weights.Add(equipmentWeightRow);
            }
        }

        public Models.ExerciseInBlock GetExerciseInBlock()
        {
            int repetitions = int.Parse(_repetitionsInputField.text);
            int durationSeconds = int.Parse(_durationSecondsInputField.text);
            List<EquipmentInBlock> equipmentWeights = new List<EquipmentInBlock>();
            foreach (EquipmentWeightRow equipmentWeightRow in _weights)
            {
                equipmentWeights.Add(equipmentWeightRow.GetEquipmentInBlock());
            }
            
            return new Models.ExerciseInBlock(_currentExercise, equipmentWeights, repetitions, durationSeconds);
        }
    }
}