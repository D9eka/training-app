using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Screens.CreateBlock;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components
{
    public class ExerciseInBlockItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _exerciseNameText;
        [SerializeField] private TMP_Text _equipmentsText;
        [Space] 
        [SerializeField] private Transform _weightRowParent;
        [SerializeField] private EquipmentWeightRow _weightRowPrefab;
        [Space]
        [SerializeField] private TMP_InputField _repetitionsInputField;
        [SerializeField] private TMP_InputField _durationSecondsInputField;
        [Space]
        [SerializeField] private Button _changeExerciseButton;
        [SerializeField] private Button _deleteExerciseButton;

        private ExerciseInBlockViewData _exerciseInBlockViewData;
        private string _id;
        private ItemsGroup<EquipmentWeightRow> _equipmentWeightRowItemsGroup;
        
        private Action<string> _onClickEditById;
        private Action<string> _onClickDeleteById;
        private Action<string, int> _onChangeRepetitions;
        private Action<string, int> _onChangeDuration;
        private Action<string, (string Id, float Weight, WeightType WeightType)> _onChangeEquipmentWeight;

        private void Awake()
        {
            _equipmentWeightRowItemsGroup = new ItemsGroup<EquipmentWeightRow>(_weightRowParent, _weightRowPrefab);
        }

        public void Setup(ExerciseInBlockViewData exerciseInBlockViewData, Action<string> onClickEdit, 
            Action<string> onClickDelete, Action<string, int> onChangeRepetitions, Action<string, int> onChangeDuration, 
            Action<string, (string Id, float Weight, WeightType WeightType)> onChangeEquipmentWeight)
        {
            _exerciseInBlockViewData = exerciseInBlockViewData;
            _id = _exerciseInBlockViewData.Id;
            _onClickEditById = onClickEdit;
            _onClickDeleteById = onClickDelete;
            _onChangeRepetitions = onChangeRepetitions;
            _onChangeDuration = onChangeDuration;
            _onChangeEquipmentWeight = onChangeEquipmentWeight;
            
            _exerciseNameText.text = exerciseInBlockViewData.Name;
            
            string equipmentsText = GetEquipmentsText(exerciseInBlockViewData.Equipments);
            _equipmentsText.gameObject.SetActive(!string.IsNullOrEmpty(equipmentsText));
            _equipmentsText.text = equipmentsText;
            
            SetupWeights(exerciseInBlockViewData.Equipments);
            
            _repetitionsInputField.onValueChanged.RemoveAllListeners();
            _repetitionsInputField.text = exerciseInBlockViewData.Repetitions.ToString();
            _repetitionsInputField.onValueChanged.AddListener((text) => _onChangeRepetitions?.Invoke(_id, int.Parse(text)));
            
            _durationSecondsInputField.onValueChanged.RemoveAllListeners();
            _durationSecondsInputField.text = exerciseInBlockViewData.DurationSeconds.ToString();
            _durationSecondsInputField.onValueChanged.AddListener((text) => _onChangeDuration?.Invoke(_id, int.Parse(text)));
            
            _changeExerciseButton.onClick.RemoveAllListeners();
            _changeExerciseButton.onClick.AddListener(() => _onClickEditById?.Invoke(_id));
            
            _deleteExerciseButton.onClick.RemoveAllListeners();
            _deleteExerciseButton.onClick.AddListener(() => _onClickDeleteById?.Invoke(_id));
        }

        private string GetEquipmentsText(List<EquipmentInBlockViewData> equipmentsInBlockViewData)
        {
            StringBuilder equipmentsText = new StringBuilder();
            for (int i = 0; i < equipmentsInBlockViewData.Count; i++)
            {
                equipmentsText.Append($"{equipmentsInBlockViewData[i].Quantity}x {equipmentsInBlockViewData[i].Name}");
                if (i < equipmentsInBlockViewData.Count - 1)
                {
                    equipmentsText.Append(", ");
                }
            }
            return equipmentsText.ToString();
        }

        private void SetupWeights(List<EquipmentInBlockViewData> equipmentsInExerciseInBlockViewData)
        {
            List<EquipmentInBlockViewData> eqsWithWeights = 
                equipmentsInExerciseInBlockViewData.Where(eq => eq.NeedWeight).ToList();
            List<EquipmentWeightRow> items = _equipmentWeightRowItemsGroup.Refresh(eqsWithWeights.Count);
            for (int i = 0; i < eqsWithWeights.Count; i++)
            {
                items[i].Setup(eqsWithWeights[i], OnChangeEquipmentWeight);
            }
            _weightRowParent.gameObject.SetActive(_weightRowParent.transform.childCount > 0);
        }

        private void OnChangeEquipmentWeight((string Id, float Weight, WeightType WeightType) weightData)
        {
            _onChangeEquipmentWeight?.Invoke(_id, weightData);
        }
    }
}