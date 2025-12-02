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
        private const string EXERCISE_SCREEN_HEADER = "Работа";
        private const string REST_AFTER_APPROACH_SCREEN_HEADER = "Отдых";
        private const string REST_AFTER_SET_SCREEN_HEADER = "Отдых после сета";
        private const string REST_AFTER_BLOCK_SCREEN_HEADER = "Отдых после блока";
        
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly IDataService<Equipment> _equipmentDataService;
        
        private readonly Color _prepScreenColor = new Color32(254, 247, 255, 255);
        private readonly Color _exerciseScreenColor = new Color32(255, 152, 184, 255);
        private readonly Color _restAfterApproachScreenColor = new Color32(234, 221, 255, 255);
        private readonly Color _restAfterSetScreenColor = new Color32(244, 228, 255, 255);
        private readonly Color _restAfterBlockScreenColor = new Color32(208, 188, 255, 255);

        public TimerScreenDataCreator(IDataService<Exercise> exerciseDataService, 
            IDataService<Equipment> equipmentDataService)
        {
            _exerciseDataService = exerciseDataService;
            _equipmentDataService = equipmentDataService;
        }

        public List<TimerScreenData> CreateTimeScreens(SimpleTrainingData trainingData)
        {
            List<TimerScreenData> timeScreens = new List<TimerScreenData>();
            int blockIndex = 1;

            string indexText = CreateIndexText(blockIndex, 1, 1, 1);
            timeScreens.Add(
                CreatePrepTimeScreen(trainingData.PrepTimeSeconds, indexText));

            for (int set = 0; set < trainingData.Sets; set++)
            {
                for (int approach = 0; approach < trainingData.Approaches; approach++)
                {
                    indexText = CreateIndexText(blockIndex, set, approach, 1);
                    timeScreens.Add(CreateExerciseTimeScreen(trainingData.ExerciseDurationSeconds, indexText));
                    if (approach != trainingData.Approaches - 1)
                    {
                        timeScreens.Add(
                            CreateRestAfterApproachTimeScreen(trainingData.RestAfterApproachSeconds, indexText));
                    }
                }
                if (set != trainingData.Sets - 1)
                {
                    timeScreens.Add(CreateRestAfterSetTimeScreen(trainingData.RestAfterSetSeconds, indexText));
                }
            }
            return timeScreens;
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
                    for (int approach = 0; approach < block.Approaches; approach++)
                    {
                        for (int exerciseIndex = 0; exerciseIndex < block.Exercises.Count; exerciseIndex++)
                        {
                            indexText = CreateIndexText(blockIndex, set, approach, exerciseIndex);
                            timeScreens.Add(CreateExerciseTimeScreen(block.Exercises[exerciseIndex], indexText));
                        }
                        if (approach != block.Approaches - 1)
                        {
                            timeScreens.Add(
                                CreateRestAfterApproachTimeScreen(block.RestAfterApproachSeconds, indexText));
                        }
                    }
                    if (set != block.Sets - 1)
                    {
                        timeScreens.Add(CreateRestAfterSetTimeScreen(block.RestAfterSetSeconds, indexText));
                    }
                }
                if (blockIndex != training.Blocks.Count - 1)
                {
                    timeScreens.Add(CreateRestAfterBlockTimeScreen(block.RestAfterBlockSeconds, indexText));
                }
            }
            return timeScreens;
        }

        private string CreateIndexText(int blockIndex, int set, int approach, int exerciseIndex)
        {
            return $"Блок {blockIndex + 1} Сет {set + 1} Подход {approach + 1} Упр {exerciseIndex + 1}";
        }

        private TimerScreenData CreatePrepTimeScreen(int prepTimeSeconds, string indexText)
        {
            return CreateScreen(_prepScreenColor, PREP_SCREEN_HEADER, prepTimeSeconds, TimeValueType.Seconds, 
                indexText);
        }
        
        private TimerScreenData CreateExerciseTimeScreen(int timeSeconds, string indexText)
        {
            return CreateScreen(_exerciseScreenColor, EXERCISE_SCREEN_HEADER, 
                timeSeconds, TimeValueType.Seconds, indexText);
        }

        private TimerScreenData CreateExerciseTimeScreen(ExerciseInBlock exerciseInBlock, string indexText)
        {
            Exercise exercise = _exerciseDataService.GetDataById(exerciseInBlock.ExerciseId);
            bool haveDuration = exerciseInBlock.DurationSeconds > 0;
            string header = CreateHeader(exerciseInBlock, exercise);
            int value = haveDuration ? exerciseInBlock.DurationSeconds : exerciseInBlock.Repetitions;
            TimeValueType valueType = haveDuration ? TimeValueType.Seconds : TimeValueType.Repetitions;
            return CreateScreen(_exerciseScreenColor, header, value, valueType, indexText);
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
            Equipment equipment = _equipmentDataService.GetDataById(equipmentRef.EquipmentId);
            if (equipment == null)
            {
                equipmentStringBuilder.Append("??");
                return equipmentStringBuilder.ToString();
            }
            if (equipment.HasQuantity)
            {
                equipmentStringBuilder.Append($"{equipmentRef.Quantity}x ");
            }
            equipmentStringBuilder.Append(equipment.Name);
            if (equipment.HasWeight)
            {
                equipmentStringBuilder.Append($" {equipmentInBlock.Weight} {equipmentInBlock.WeightType}");
            }

            equipmentStringBuilder.Insert(0, "c ");
            return equipmentStringBuilder.ToString();
        }

        private TimerScreenData CreateRestAfterApproachTimeScreen(int restAfterApproachTimeSeconds, string indexText)
        {
            return CreateScreen(_restAfterApproachScreenColor, REST_AFTER_APPROACH_SCREEN_HEADER, 
                restAfterApproachTimeSeconds, TimeValueType.Seconds, indexText);
        }

        private TimerScreenData CreateRestAfterSetTimeScreen(int restAfterSetTimeSeconds, string indexText)
        {
            return CreateScreen(_restAfterSetScreenColor, REST_AFTER_SET_SCREEN_HEADER, restAfterSetTimeSeconds,
                TimeValueType.Seconds, indexText);
        }

        private TimerScreenData CreateRestAfterBlockTimeScreen(int restAfterBlockTimeSeconds, string indexText)
        {
            return CreateScreen(_restAfterBlockScreenColor, REST_AFTER_BLOCK_SCREEN_HEADER, restAfterBlockTimeSeconds,
                TimeValueType.Seconds, indexText);
        }

        private TimerScreenData CreateScreen(Color color, string header, int value, TimeValueType valueType, string indexText)
        {
            string valueTypeText = CreateValueTypeText(valueType);
            return new TimerScreenData(color, header, value, valueType, valueTypeText, indexText);
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
    }
}
