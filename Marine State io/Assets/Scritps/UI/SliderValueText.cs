using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Extensions;

namespace Scripts.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class SliderValueText : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private TMP_Text _valueText;

        private void Start()
        {
            Asserts();

            _valueText = GetComponent<TMP_Text>();

            Subscribe();
        }

        private void Asserts()
        {
            _slider.LogErrorIfComponentNull();
        }

        private void Subscribe()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
            UpdateValueText(_slider.value);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        public void OnSliderValueChanged(float value)
        {
            UpdateValueText(value);
        }

        private void UpdateValueText(float value)
        {
            int intValue = Mathf.RoundToInt(value);
            _valueText.text = intValue.ToString();
        }
    }
}