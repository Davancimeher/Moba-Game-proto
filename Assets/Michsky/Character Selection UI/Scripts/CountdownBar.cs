using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Michsky.UI.Freebie
{
    public class CountdownBar : MonoBehaviour
    {
        // Content
        [HideInInspector]public float currentValue;
        [Range(0, 120)] public float maxValue = 30;

        // Resources
        public Image loadingBar;
        public TextMeshProUGUI textPercent;

        // Settings
        public bool isOn;
        public bool restart;
        public bool invert;
        public UnityEvent onCountEnd;

        void Start()
        {
            if (isOn == false)
            {
                loadingBar.fillAmount = currentValue / maxValue;
                textPercent.text = ((int)currentValue).ToString("F0") + "%";
            }
        }

        void Update()
        {
            if (isOn == true)
            {
                if (currentValue <= maxValue && invert == false)
                    currentValue += 1 * Time.deltaTime;

                else if (currentValue >= 0 && invert == true)
                    currentValue -= 1 * Time.deltaTime;

                if (currentValue >= maxValue && restart == true && invert == false)
                    currentValue = 0;

                else if (currentValue >= maxValue && restart == false && invert == false)
                    onCountEnd.Invoke();

                else if (currentValue <= 0 && restart == true && invert == true)
                    currentValue = maxValue;

                else if (currentValue <= 0 && restart == false && invert == true)
                    onCountEnd.Invoke();

                loadingBar.fillAmount = currentValue / maxValue;
                textPercent.text = ((int)currentValue).ToString("F0");
            }
        }

        public void UpdateUI()
        {
            loadingBar.fillAmount = currentValue / maxValue;
            textPercent.text = ((int)currentValue).ToString("F0");
        }

        public void StopCounting()
        {
            isOn = false;
        }
    }
}