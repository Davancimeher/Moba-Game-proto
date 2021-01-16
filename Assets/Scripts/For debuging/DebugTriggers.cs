using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugTriggers : MonoBehaviour
{
    public GameObject buttonTest;
    public GameObject buttonCancel;
    public GameObject Indicator;
    public GameObject rotation;
    public bool Canceled = false;
    public bool InAttack = false;
    private Vector3 postion;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        InitDownTrigger();
        InitUpTrigger();
        InitCancelTrigger();
    }


    public void InitDownTrigger()
    {
        EventTrigger trigger = buttonTest.GetComponent<EventTrigger>();
        EventTrigger.Entry PointerDown = new EventTrigger.Entry();
        PointerDown.eventID = EventTriggerType.PointerDown;
        PointerDown.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(PointerDown);
    }
    public void OnPointerDownDelegate(PointerEventData data)
    {
        buttonCancel.SetActive(true);
        Indicator.SetActive(true);
    }

    public void InitUpTrigger()
    {
        EventTrigger trigger = buttonTest.GetComponent<EventTrigger>();
        EventTrigger.Entry PointerUp = new EventTrigger.Entry();
        PointerUp.eventID = EventTriggerType.PointerUp;
        PointerUp.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });
        trigger.triggers.Add(PointerUp);

    }
    public void OnPointerUpDelegate(PointerEventData data)
    {
        if (!Canceled)
        {
            Debug.Log("Attack Called");
            Vector3 targetPostion = new Vector3(rotation.transform.position.x, player.transform.position.y, rotation.transform.position.z);
            player.transform.LookAt(targetPostion);
        }
        else
        {
            Debug.Log("Attack canceled");

            Canceled = false;
        }
        buttonCancel.SetActive(false);

        Indicator.SetActive(false);
        InAttack = false;
    }

    public void InitCancelTrigger()
    {
        EventTrigger trigger = buttonCancel.GetComponent<EventTrigger>();
        EventTrigger.Entry PointerEnter = new EventTrigger.Entry();
        PointerEnter.eventID = EventTriggerType.PointerEnter;
        PointerEnter.callback.AddListener((data) => { OnCancelPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(PointerEnter);

        EventTrigger.Entry PointerExit = new EventTrigger.Entry();
        PointerExit.eventID = EventTriggerType.PointerExit;
        PointerExit.callback.AddListener((data) => { OnCancelPointerExit((PointerEventData)data); });
        trigger.triggers.Add(PointerExit);
    }
    public void OnCancelPointerEnter(PointerEventData data)
    {
        Canceled = true;
    }
    public void OnCancelPointerExit(PointerEventData data)
    {
        Canceled = false;
    }

    private void Update()
    {
       
        
    }
}
