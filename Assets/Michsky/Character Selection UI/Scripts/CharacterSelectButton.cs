using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

namespace Michsky.UI.Freebie
{
    public class CharacterSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("CONTENT")]
        public Sprite previewIcon;
        public Sprite characterIcon;
        public string characterName = "Character";
        public string characterType = "Support";
        [TextArea] public string characterInfo = "Character info here.";
        [TextArea] public string firstAbility = "First ability description here.";
        [TextArea] public string secondAbility = "Second ability description here.";
        [TextArea] public string thirdAbility = "Third ability description here.";

        [Header("SOUND")]
        public bool enableButtonSounds = false;
        public bool useHoverSound = true;
        public bool useClickSound = true;
        public bool useSelectSound = true;
        public AudioClip hoverSound;
        public AudioClip clickSound;
        public AudioClip selectSound;
        public AudioSource soundSource;

        [Header("RESOURCES")]
        public Animator objectAnimator;
        public CharacterSelectManager characterManager;
        public Image previewImage;
        public TextMeshProUGUI characterText;

        [Header("SETTINGS")]
        public bool useCustomContent = false;

        [Header("EVENTS")]
        public UnityEvent onCharacterClick;
        public UnityEvent onCharacterSelection;

        void Start()
        {
            if (useCustomContent == false)
                UpdateUI();
        }

        public void UpdateUI()
        {
            characterText.text = characterName;
            previewImage.sprite = previewIcon;
        }

        public void SelectCharacter()
        {
            objectAnimator.Play("Pressed to Selected");
            onCharacterSelection.Invoke();

            if (enableButtonSounds == true && useSelectSound == true)
                soundSource.PlayOneShot(selectSound);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableButtonSounds == true && useHoverSound == true)
                soundSource.PlayOneShot(hoverSound);

            if (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed") &&
                !objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed to Selected"))
                objectAnimator.Play("Normal to Hover");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed") &&
                !objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed to Selected"))
                objectAnimator.Play("Hover to Normal");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (enableButtonSounds == true && useClickSound == true)
                soundSource.PlayOneShot(clickSound);

            if (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed to Selected") &&
                characterManager.enableSelecting == true)
            {
                onCharacterClick.Invoke();
                objectAnimator.Play("Hover to Pressed");
            }

            if (characterManager != null)
            {
                if (characterManager.currentObjectAnimator != null)
                    if (characterManager.currentObjectAnimator != objectAnimator)
                        characterManager.UpdateCharacter();

                characterManager.currentObjectAnimator = objectAnimator;
                characterManager.UpdateInfo();
            }
        }
    }
}