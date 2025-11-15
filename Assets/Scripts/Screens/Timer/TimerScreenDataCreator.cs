using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Models;
using UnityEngine;

namespace Screens.Timer
{
    public class TimerScreenDataCreator
    {
        private const string PREP_SCREEN_HEADER = "Подготовка";
        private const string REST_AFTER_APPROACH_SCREEN_HEADER = "Отдых";
        private const string REST_AFTER_SET_SCREEN_HEADER = "Отдых после сета";
        private const string REST_AFTER_BLOCK_SCREEN_HEADER = "Отдых после блока";
        
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly IDataService<Equipment> _equipmentDataService;
        
        private readonly Color _prepScreenColor = Color.green;
        private readonly Color _restAfterApproachScreenColor = Color.paleGreen;
        private readonly Color _restAfterSetScreenColor = Color.darkSeaGreen;
        private readonly Color _restAfterBlockScreenColor = Color.lightSeaGreen;
        private readonly Color _exerciseScreenColor = Color.softRed;

        public TimerScreenDataCreator(IDataService<Exercise> exerciseDataService, IDataService<Equipment> equipmentDataService)
        {
            _exerciseDataService = exerciseDataService;
            _equipmentDataService = equipmentDataService;
        }

        public List<TimerScreenData> CreateTimeScreens(Training training)
        {
            List<TimerScreenData> timeScreens = new List<TimerScreenData>();

            string indexText = CreateIndexText(1, 1, 1, 1);
            timeScreens.Add(
                CreatePrepTimeScreen(training.PrepTimeSeconds, indexText));
            
            for (int blockIndex = 0; blockIndex < training.Blocks.Count; blockIndex++)
            {
                TrainingBlock block = training.Blocks[blockIndex];
                for (int set = 0; set < block.Sets; set++)
                {
                    for (int approach = 0; approach < block.Approaches; set++)
                    {
                        for (int exerciseIndex = 0; exerciseIndex < block.Exercises.Count; exerciseIndex++)
                        {
                            indexText = CreateIndexText(blockIndex, set, approach, exerciseIndex);
                            timeScreens.Add(CreateExerciseTimeScreen(block.Exercises[exerciseIndex],indexText));
                        }
                        if (approach != block.Approaches - 1)
                        {
                            timeScreens.Add(
                                CreateRestAfterApproachTimeScreen(block.RestAfterApproachTimeSpan.Seconds, indexText));
                        }
                    }
                    if (set != block.Sets - 1)
                    {
                        timeScreens.Add(CreateRestAfterSetTimeScreen(block.RestAfterSetTimeSpan.Seconds, indexText));
                    }
                }
                if (blockIndex != training.Blocks.Count - 1)
                {
                    timeScreens.Add(CreateRestAfterBlockTimeScreen(block.RestAfterBlockTimeSpan.Seconds, indexText));
                }
            }
            return timeScreens;
        }

        private TimerScreenData CreatePrepTimeScreen(int prepTimeSeconds, string indexText)
        {
            int value = prepTimeSeconds;
            TimeValueType valueType = TimeValueType.Seconds;
            string valueTypeText = CreateValueTypeText(valueType);
            return new TimerScreenData(
                _prepScreenColor,
                PREP_SCREEN_HEADER,
                value,
                valueType,
                valueTypeText,
                indexText
            );
        }

        private string CreateIndexText(int blockIndex, int set, int approach, int exerciseIndex)
        {
            return $"Блок {blockIndex + 1} Сет {set + 1} Подход {approach + 1} Упр {exerciseIndex + 1}";
        }

        private TimerScreenData CreateExerciseTimeScreen(ExerciseInBlock exerciseInBlock, string indexText)
        {
            Exercise exercise = _exerciseDataService.GetDataById(exerciseInBlock.Id);
            bool haveDuration = exerciseInBlock.DurationTimeSpan.Seconds >= 0;
            string header = CreateHeader(exerciseInBlock, exercise);
            int value = haveDuration ? exerciseInBlock.DurationTimeSpan.Seconds : exerciseInBlock.Repetitions;
            TimeValueType valueType = haveDuration ? TimeValueType.Seconds : TimeValueType.Repetitions;
            string valueTypeText = CreateValueTypeText(valueType);
            return new TimerScreenData(
                _exerciseScreenColor,
                header,
                value,
                valueType,
                valueTypeText,
                indexText
            );
        }

        private string CreateHeader(ExerciseInBlock exerciseInBlock, Exercise exercise)
        {
            StringBuilder header = new StringBuilder();
            header.Append(exercise.Name);
            
            StringBuilder equipmentsStringBuilder = new StringBuilder();
            foreach (EquipmentInBlock equipmentInBlock in exerciseInBlock.EquipmentWeights)
            {
                equipmentsStringBuilder.AppendJoin(", ", CreateEquipmentText(equipmentInBlock));
            }

            return $"{header} {equipmentsStringBuilder}";
        }

        private string CreateEquipmentText(EquipmentInBlock equipmentInBlock)
        {
            StringBuilder equipmentStringBuilder = new StringBuilder();
            ExerciseEquipmentRef equipmentRef = equipmentInBlock.Equipment;
            Equipment equipment = equipmentRef.Equipment;
            if (equipment.HasQuantity)
            {
                equipmentStringBuilder.Append($"{equipmentRef.Quantity}x ");
            }
            equipmentStringBuilder.Append(equipment.Name); //TODO: Заменить на _equipmentDataService.GetDataById(...)
            if (equipment.HasWeight)
            {
                equipmentStringBuilder.Append($" {equipmentInBlock.Weight} {equipmentInBlock.WeightType}");
            }

            equipmentStringBuilder.Insert(0, "c ");
            return equipmentStringBuilder.ToString();
        }

        private string CreateValueTypeText(TimeValueType valueType)
        {
            return valueType switch
            {
                TimeValueType.Seconds => "сек",
                TimeValueType.Repetitions => "раз",
                _ => throw new NotImplementedException($"Value type {valueType} not implemented")
            };
        }

        private TimerScreenData CreateRestAfterApproachTimeScreen(int restAfterApproachTimeSeconds, string indexText)
        {
            int value = restAfterApproachTimeSeconds;
            TimeValueType valueType = TimeValueType.Seconds;
            string valueTypeText = CreateValueTypeText(valueType);
            return new TimerScreenData(
                _restAfterApproachScreenColor,
                REST_AFTER_APPROACH_SCREEN_HEADER,
                value,
                valueType,
                valueTypeText,
                indexText
            );
        }

        private TimerScreenData CreateRestAfterSetTimeScreen(int restAfterSetTimeSeconds, string indexText)
        {
            int value = restAfterSetTimeSeconds;
            TimeValueType valueType = TimeValueType.Seconds;
            string valueTypeText = CreateValueTypeText(valueType);
            return new TimerScreenData(
                _restAfterSetScreenColor,
                REST_AFTER_SET_SCREEN_HEADER,
                value,
                valueType,
                valueTypeText,
                indexText
            );
        }

        private TimerScreenData CreateRestAfterBlockTimeScreen(int restAfterBlockTimeSeconds, string indexText)
        {
            int value = restAfterBlockTimeSeconds;
            TimeValueType valueType = TimeValueType.Seconds;
            string valueTypeText = CreateValueTypeText(valueType);
            return new TimerScreenData(
                _restAfterBlockScreenColor,
                REST_AFTER_BLOCK_SCREEN_HEADER,
                value,
                valueType,
                valueTypeText,
                indexText
            );
        }
    }
}