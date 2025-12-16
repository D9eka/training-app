using System.Globalization;
using Screens.WeightTracker;
using TMPro;
using UnityEngine;

namespace Views.Components
{
    public class WeightItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dateText;
        [SerializeField] private TextMeshProUGUI _weightText;
        [SerializeField] private TextMeshProUGUI _weightDifferenceText;
        [Space]
        [SerializeField] private Color _negativeDifferenceColor;
        [SerializeField] private Color _positiveDifferenceColor;
        
        private Color _zeroDifferenceColor;

        private void Awake()
        {
            _zeroDifferenceColor = _weightDifferenceText.color;
        }

        public void Setup(WeightItemViewData weightItemData)
        {
            _dateText.text = weightItemData.Date;
            _weightText.text = weightItemData.Weight.ToString(CultureInfo.CurrentCulture);
            _weightDifferenceText.text = GetWeightDifferenceText(weightItemData.WeightDifference, weightItemData.ShowDifference);
            if (weightItemData.ShowDifference)
                SetDifferenceColor(weightItemData.WeightDifference);
        }

        private string GetWeightDifferenceText(float weightDifference, bool showDifference)
        {
            if (!showDifference)
            {
                return string.Empty;
            }

            return $"{(weightDifference > 0 ? "+" : "")}{weightDifference.ToString(CultureInfo.CurrentCulture)}";
        }

        private void SetDifferenceColor(float weightDifference)
        {
            bool isZero = weightDifference == 0;
            if (isZero)
            {
                _weightDifferenceText.color = _zeroDifferenceColor;
                return;
            }
            _weightDifferenceText.color = weightDifference > 0 ? _positiveDifferenceColor : _negativeDifferenceColor;
        }
    }
}