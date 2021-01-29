using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Michsky.UI.Freebie
{
    public class WindowManager : MonoBehaviour
    {
        [Header("LIST")]
        public List<WindowItem> windows = new List<WindowItem>();

        [Header("SETTINGS")]
        public int currentWindowIndex = 0;
        private int currentButtonIndex = 0;
        private int newWindowIndex;
        public string windowFadeIn = "Window In";
        public string windowFadeOut = "Window Out";
        public string buttonFadeIn = "Hover to Pressed";
        public string buttonFadeOut = "Pressed to Normal";

        private GameObject currentWindow;
        private GameObject nextWindow;
        private GameObject currentButton;
        private GameObject nextButton;

        private Animator currentWindowAnimator;
        private Animator nextWindowAnimator;
        private Animator currentButtonAnimator;
        private Animator nextButtonAnimator;

        [System.Serializable]
        public class WindowItem
        {
            public string windowName = "My Window";
            public TextMeshProUGUI titleObject;
            public GameObject windowObject;
            public GameObject buttonObject;
        }

        void Start()
        {
            currentButton = windows[currentWindowIndex].buttonObject;
            currentButtonAnimator = currentButton.GetComponent<Animator>();
            currentButtonAnimator.Play(buttonFadeIn);

            currentWindow = windows[currentWindowIndex].windowObject;
            currentWindowAnimator = currentWindow.GetComponent<Animator>();
            currentWindowAnimator.Play(windowFadeIn);

            if (windows[currentWindowIndex].titleObject != null)
                windows[currentWindowIndex].titleObject.text = windows[currentWindowIndex].windowName;
        }

        public void OpenFirstTab()
        {
            if (currentWindowIndex != 0)
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeOut);

                currentButton = windows[currentWindowIndex].buttonObject;
                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play(buttonFadeOut);

                currentWindowIndex = 0;
                currentButtonIndex = 0;

                currentWindow = windows[currentWindowIndex].windowObject;
                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeIn);

                currentButton = windows[currentButtonIndex].buttonObject;
                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play(buttonFadeIn);

                if (windows[currentWindowIndex].titleObject != null)
                    windows[currentWindowIndex].titleObject.text = windows[currentWindowIndex].windowName;
            }

            else if (currentWindowIndex == 0)
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeIn);

                currentButton = windows[currentButtonIndex].buttonObject;
                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play(buttonFadeIn);

                if (windows[currentWindowIndex].titleObject != null)
                    windows[currentWindowIndex].titleObject.text = windows[currentWindowIndex].windowName;
            }
        }

        public void OpenPanel(string newPanel)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowName == newPanel)
                    newWindowIndex = i;
            }

            if (newWindowIndex != currentWindowIndex)
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                currentButton = windows[currentWindowIndex].buttonObject;
                currentWindowIndex = newWindowIndex;
                nextWindow = windows[currentWindowIndex].windowObject;

                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                nextWindowAnimator = nextWindow.GetComponent<Animator>();

                currentWindowAnimator.Play(windowFadeOut);
                nextWindowAnimator.Play(windowFadeIn);

                currentButtonIndex = newWindowIndex;
                nextButton = windows[currentButtonIndex].buttonObject;

                currentButtonAnimator = currentButton.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeOut);
                nextButtonAnimator.Play(buttonFadeIn);

                if (windows[currentWindowIndex].titleObject != null)
                    windows[currentWindowIndex].titleObject.text = windows[currentWindowIndex].windowName;
            }
        }

        public void NextPage()
        {
            if (currentWindowIndex <= windows.Count - 2)
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                currentButton = windows[currentButtonIndex].buttonObject;
                nextButton = windows[currentButtonIndex + 1].buttonObject;

                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentButtonAnimator = currentButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeOut);
                currentWindowAnimator.Play(windowFadeOut);

                currentWindowIndex += 1;
                currentButtonIndex += 1;
                nextWindow = windows[currentWindowIndex].windowObject;

                nextWindowAnimator = nextWindow.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextWindowAnimator.Play(windowFadeIn);
                nextButtonAnimator.Play(buttonFadeIn);

                if (windows[currentWindowIndex].titleObject != null)
                    windows[currentWindowIndex].titleObject.text = windows[currentWindowIndex].windowName;
            }
        }

        public void PrevPage()
        {
            if (currentWindowIndex >= 1)
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                currentButton = windows[currentButtonIndex].buttonObject;
                nextButton = windows[currentButtonIndex - 1].buttonObject;

                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentButtonAnimator = currentButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeOut);
                currentWindowAnimator.Play(windowFadeOut);

                currentWindowIndex -= 1;
                currentButtonIndex -= 1;
                nextWindow = windows[currentWindowIndex].windowObject;

                nextWindowAnimator = nextWindow.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextWindowAnimator.Play(windowFadeIn);
                nextButtonAnimator.Play(buttonFadeIn);

                if (windows[currentWindowIndex].titleObject != null)
                    windows[currentWindowIndex].titleObject.text = windows[currentWindowIndex].windowName;
            }
        }

        public void AddNewItem()
        {
            WindowItem window = new WindowItem();
            windows.Add(window);
        }
    }
}