using UnityEngine;

namespace Screens.Timer
{
    public class TimerScreenData
    {
        public TimerScreenData(Color color, string currentExerciseHeader, int value, TimeValueType valueType, string valueTypeText, string indexText)
        {
            Color = color;
            CurrentExerciseHeader = currentExerciseHeader;
            Value = value;
            ValueType = valueType;
            ValueTypeText = valueTypeText;
            IndexText = indexText;
        }

        public Color Color { get; private set; }
        public string CurrentExerciseHeader {get; private set; }
        public int Value { get; private set; }
        public TimeValueType ValueType { get; private set; }
        public string ValueTypeText { get; private set; }
        public string IndexText { get; private set; }
    }
}