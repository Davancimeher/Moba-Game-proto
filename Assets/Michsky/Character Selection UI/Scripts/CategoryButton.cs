using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Michsky.UI.Freebie
{
    public class CategoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("CONTENT")]
        public Sprite buttonIcon;
        public AudioClip hoverSound;
        public AudioClip clickSound;

        [Header("RESOURCES")]
        public Animator objectAnimator;
        public Image normalImage;
        public Image pressedImage;
        public AudioSource soundSource;

        [Header("SETTINGS")]
        public bool useCustomContent = false;
        public bool enableButtonSounds = false;
        public bool useHoverSound = true;
        public bool useClickSound = true;
        public bool useSelectSound = true;

        [Header("EVENTS")]
        public UnityEvent onButtonSelection;

        void Start()
        {
            if (useCustomContent == false)
                UpdateUI();
        }

        public void UpdateUI()
        {
            normalImage.sprite = buttonIcon;
            pressedImage.sprite = buttonIcon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableButtonSounds == true && useHoverSound == true)
                soundSource.PlayOneShot(hoverSound);

            if (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed"))
                objectAnimator.Play("Normal to Hover");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed"))
                objectAnimator.Play("Hover to Normal");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (enableButtonSounds == true && useClickSound == true)
                soundSource.PlayOneShot(clickSound);

            onButtonSelection.Invoke();
        }
    }
}