using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Werewolf.StatusIndicators.Components;

public enum ActualState
{
    PASSIVE,
    ATTACKING,
    REGEN_HEALTH,
    IN_RECALL,
    DEAD
}
public class ChampionManager : MonoBehaviour, IInRoomCallbacks
{
    [Header("Champion State")]
    public ActualState state;
    public int level = 1;
    public byte teamIndex;

    [Header("Generics")]
    public Animator m_Animator;
    public PhotonView m_MyPhotonView;



    [Header("Champion Scripts")]
    public HealthManager m_HealthManager;
    public PlayerMouvement m_PlayerMouvement;
    public SetPlayerInGameInfo m_SetPlayerInGameInfo;
    public SpellManager m_SpellManager; 

    [Header("collisions")]
    public GameObject m_HitObject;
    public GameObject m_HitBox;
    public BoxCollider m_HitCollider;
    public CapsuleCollider m_ChampionCollider;
    public GameObject m_ChampionCharacter;

    [Header("Champion Auto Attack")]
    public Attack m_AutoAttack;

    private Image m_AutoAttackImage;
    private Button m_AutoAttackButton;
    private TextMeshProUGUI m_AutoAttackTime;

    [Header("Champion Attack 1")]
    public Attack Attack1;
    private Image m_Attack1Image;
    private TextMeshProUGUI m_Attack1Time;

    [Header("Champion Attack 2")]
    public Attack Attack2;
    private Image m_Attack2Image;
    private TextMeshProUGUI m_Attack2Time;

    [Header("Champion Attack 3")]
    public Attack Attack3;
    private Image m_Attack3Image;
    private TextMeshProUGUI m_Attack3Time;

    [Header("Cancel Cast Button")]
    public GameObject m_CancelCastButton;

    [Header("Recall Button")]
    public Button m_RecallButton;
    public GameObject m_RecallFx;

    [Header("Champion manager variables")]
    public Attack m_ActualAttack;   

    private EventTrigger Attack1Event;
    private EventTrigger Attack2Event;
    private EventTrigger Attack3Event;

    private float autoAttackRate = 0;
    private float Attack1Rate = 0;
    private float Attack2Rate = 0;
    private float Attack3Rate = 0;

    private GameObject autoAttackIndicator;
    private GameObject attack1Indicator;
    private GameObject attack2Indicator;
    private GameObject attack3Indicator;
    
    private bool cancelCast = false;


    private float AutoAttackingTime = 0;
    [HideInInspector]
    public bool InAutoAttacking = false;

    public GameObject SplatsManager;

    [HideInInspector]
    public bool EnemyHitted;

    public float m_respowntime;

    public Tower m_Tower;
    public bool m_UnderTower;

    public bool canAttack;


    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void InitChampionManager(Hero _hero, GameObject autoAttackObject, GameObject attack1Object, GameObject attack2Object, GameObject attack3Object, GameObject cancelButton,Button RecallButton)
    {
        Application.targetFrameRate = 120;

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("I'm the master");

            InGameManager.IGM.InitTowers();
            InGameManager.IGM.m_MasterPhotonView = m_MyPhotonView;
        }

        teamIndex = RoomData.RD.m_PlayersTeams[PhotonNetwork.LocalPlayer.ActorNumber];

        m_AutoAttack = _hero.AutoAttack;
        Attack1 = _hero.Attack1;
        Attack2 = _hero.Attack2;
        Attack3 = _hero.Attack3;

        InitWithObject(autoAttackObject, ref m_AutoAttackImage, ref m_AutoAttackTime, m_AutoAttack);
        InitWithObject(attack1Object, ref m_Attack1Image, ref m_Attack1Time, Attack1);
        InitWithObject(attack2Object, ref m_Attack2Image, ref m_Attack2Time, Attack2);
        InitWithObject(attack3Object, ref m_Attack3Image, ref m_Attack3Time, Attack3);

        m_AutoAttackButton = autoAttackObject.GetComponent<Button>();
        m_HealthManager.Health = _hero.Health;
        m_HealthManager.MaxHealth = _hero.Health;
        m_HealthManager.teamIndex = RoomData.RD.m_PlayersTeams[PhotonNetwork.LocalPlayer.ActorNumber];
        m_PlayerMouvement.speed = _hero.Speed;

        m_CancelCastButton = cancelButton;
        m_RecallButton = RecallButton;
        InitUI();
        InitIndicators();
        canAttack = true;
    }
    private void InitWithObject(GameObject _initObject, ref Image attackImage, ref TextMeshProUGUI attackText, Attack attack)
    {
        attackImage = _initObject.GetComponent<Image>();
        attackText = _initObject.GetComponentInChildren<TextMeshProUGUI>();
        attackImage.sprite = attack.AttackSprite;
    }

    public void InitUI()
    {
        m_AutoAttackButton.onClick.AddListener(() => OnClickAutoAttack());
        m_RecallButton.onClick.AddListener(() => OnClickRecallButton());
        InitCastsButtons();
    }
    public void InitIndicators()
    {
        autoAttackIndicator = Instantiate(m_AutoAttack.Indicator, SplatsManager.gameObject.transform);
        attack1Indicator = Instantiate(Attack1.Indicator, SplatsManager.gameObject.transform);
        attack2Indicator = Instantiate(Attack2.Indicator, SplatsManager.gameObject.transform);
        attack3Indicator = Instantiate(Attack3.Indicator, SplatsManager.gameObject.transform);

        autoAttackIndicator.SetActive(false);
        attack1Indicator.SetActive(false);
        attack2Indicator.SetActive(false);
        attack3Indicator.SetActive(false);
    }
    public void OnClickAutoAttack()
    {
        if(state != ActualState.DEAD && canAttack)
        {
            AutoAttackingTime = 1.5f;
            autoAttackIndicator.SetActive(true);
            m_MyPhotonView.RPC("RPC_AutoAttackExecution", RpcTarget.AllViaServer);
        }
    }
    public void OnClickRecallButton()
    {
        if (state != ActualState.DEAD && canAttack)
        {
            m_SpellManager.ExecuteRecallSpell();
        }
    }
    public void OnClickAttack1()
    {
        if (state != ActualState.DEAD && canAttack)
        {
            m_MyPhotonView.RPC("RPC_Attack1Execution", RpcTarget.AllViaServer);
        }
    }
    public void OnClickAttack2()
    {
        if (state != ActualState.DEAD && canAttack)
        {
            m_MyPhotonView.RPC("RPC_Attack2Execution", RpcTarget.AllViaServer);
        }
    }
    public void OnClickAttack3()
    {
        if (state != ActualState.DEAD && canAttack)
        {
            m_MyPhotonView.RPC("RPC_Attack3Execution", RpcTarget.AllViaServer);
        }
    }

    public void UpdateRateValues()
    {
        UpdateAutoAttackTime();

        //if (autoAttackRate > 0)
        //{
        //    if (!m_AutoAttackTime.gameObject.activeSelf)
        //        m_AutoAttackTime.gameObject.SetActive(true);

        //    autoAttackRate -= Time.deltaTime;
        //    m_AutoAttackImage.fillAmount = (m_AutoAttack.AttackRate - autoAttackRate) / m_AutoAttack.AttackRate;

        //    m_AutoAttackTime.text = Mathf.RoundToInt(autoAttackRate).ToString();
        //}
        //else
        //{
        //    if (m_AutoAttackTime.gameObject.activeSelf) m_AutoAttackTime.gameObject.SetActive(false);
        //    if (!m_AutoAttackButton.interactable)
        //    {
        //        m_AutoAttackImage.fillAmount = 1f;
        //        m_AutoAttackButton.interactable = true;
        //    }

        //}

        if (Attack1Rate > 0)
        {
            if (!m_Attack1Time.gameObject.activeSelf)
                m_Attack1Time.gameObject.SetActive(true);
            if (Attack1Event.isActiveAndEnabled) Attack1Event.enabled = false;

            Attack1Rate -= Time.deltaTime;
            m_Attack1Image.fillAmount = (Attack1.AttackRate - Attack1Rate) / Attack1.AttackRate;

            m_Attack1Time.text = Mathf.RoundToInt(Attack1Rate).ToString();
        }
        else
        {
            if (m_Attack1Time.gameObject.activeSelf) m_Attack1Time.gameObject.SetActive(false);

            if (!Attack1Event.isActiveAndEnabled)
            {
                Attack1Event.enabled = true;
                m_Attack1Image.fillAmount = 1f;
            }
        }

        if (Attack2Rate > 0)
        {
            if (!m_Attack2Time.gameObject.activeSelf)
                m_Attack2Time.gameObject.SetActive(true);
            if (Attack2Event.isActiveAndEnabled) Attack2Event.enabled = false;

            Attack2Rate -= Time.deltaTime;
            m_Attack2Image.fillAmount = (Attack2.AttackRate - Attack2Rate) / Attack2.AttackRate;
            m_Attack2Time.text = Mathf.RoundToInt(Attack2Rate).ToString();
        }
        else
        {
            if (m_Attack2Time.gameObject.activeSelf) m_Attack2Time.gameObject.SetActive(false);

            if (!Attack2Event.isActiveAndEnabled)
            {
                Attack2Event.enabled = true;
                m_Attack2Image.fillAmount = 1f;
            }
        }

        if (Attack3Rate > 0)
        {
            if (!m_Attack3Time.gameObject.activeSelf)
                m_Attack3Time.gameObject.SetActive(true);
            if (Attack3Event.isActiveAndEnabled) Attack3Event.enabled = false;

            Attack3Rate -= Time.deltaTime;
            m_Attack3Image.fillAmount = (Attack3.AttackRate - Attack3Rate) / Attack3.AttackRate;
            m_Attack3Time.text = Mathf.RoundToInt(Attack3Rate).ToString();
        }
        else
        {
            if (m_Attack3Time.gameObject.activeSelf) m_Attack3Time.gameObject.SetActive(false);
            if (!Attack3Event.isActiveAndEnabled)
            {
                Attack3Event.enabled = true;
                m_Attack3Image.fillAmount = 1f;
            }
        }
    }


    private void Update()
    {
        if (m_MyPhotonView.IsMine)
        {
            UpdateReswpanTimeValue();
            UpdateRateValues();
        }
    }

    public void EnableHitObject()
    {
        if (m_MyPhotonView.IsMine)
        {
            m_HitObject.SetActive(true);
        }
    }
    public void DisableHitObject()
    {
        if (m_MyPhotonView.IsMine)
        {
            m_HitObject.SetActive(false);
        }
    }
    public void UpdateHitObject()
    {
        if (!m_MyPhotonView.IsMine) return;
        if (EnemyHitted) return;
        if (!m_HitObject.activeSelf) m_HitObject.SetActive(true);
    }

    public void UpdateAutoAttackTime()
    {
        if (canAttack)
        {
            if (AutoAttackingTime > 0)
            {
                AutoAttackingTime -= Time.deltaTime;
                if (!m_Animator.GetBool("AutoAttacking"))
                {
                    m_Animator.SetBool("AutoAttacking", true);
                }
            }
            else
            {
                if (m_Animator.GetBool("AutoAttacking"))
                {
                    autoAttackIndicator.SetActive(false);
                    ExecuteStopAutoAttacking();
                }

            }
        }
       
    }
    [PunRPC]
    public void RPC_AutoAttackExecution()
    {
        m_ActualAttack = m_AutoAttack;

        if (!InAutoAttacking)
        {
            InAutoAttacking = true;
            m_Animator.SetBool("AutoAttacking", true);
            m_Animator.SetTrigger("AutoAttack");

        }

        if (m_MyPhotonView.IsMine)
        {
            autoAttackRate = m_AutoAttack.AttackRate;
        }
    }
    public void ExecuteStopAutoAttackingFromSpell()
    {
        m_MyPhotonView.RPC("RPC_StopAutoAttackFromSpell", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPC_StopAutoAttackFromSpell()
    {
        InAutoAttacking = false;
        m_Animator.SetBool("AutoAttacking", false);
        Debug.LogError("RPC_StopAutoAttackFromSpell");
    }
    public void ExecuteStopAutoAttacking()
    {
        m_MyPhotonView.RPC("RPC_StopAutoAttack", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPC_StopAutoAttack()
    {
        InAutoAttacking = false;
        m_Animator.SetBool("AutoAttacking", true);
    }
    [PunRPC]
    public void RPC_Attack1Execution()
    {
        m_ActualAttack = Attack1;
        m_Animator.SetTrigger("Attack1");

        if (m_MyPhotonView.IsMine)
        {
            Attack1Rate = Attack1.AttackRate;
        }
    }
    [PunRPC]
    public void RPC_Attack2Execution()
    {
        m_ActualAttack = Attack2;
        m_Animator.SetTrigger("Attack2");

        if (m_MyPhotonView.IsMine)
        {
            Attack2Rate = Attack2.AttackRate;
        }
    }    
    [PunRPC]
    public void RPC_Attack3Execution()
    {
        m_ActualAttack = Attack3;
        m_Animator.SetTrigger("Attack3");

        if (m_MyPhotonView.IsMine)
        {
            Attack3Rate = Attack3.AttackRate;
        }
    }
    public void ExecuteSetDead()
    {
        m_MyPhotonView.RPC("RPC_SetDead", RpcTarget.AllViaServer);
    }
    public void ExecuteRespawn()
    {
        m_MyPhotonView.RPC("RPC_SetRespawn", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPC_SetDead()
    {
        m_Animator.SetTrigger("Dead");
        m_Animator.SetBool("isDead", true);
        state = ActualState.DEAD;
        m_ChampionCollider.enabled = false;
        m_HitCollider.enabled = false;
        m_SetPlayerInGameInfo.m_ChampionCanvas.SetActive(false);
    }
   
    public void SetDead()   
    {
        m_PlayerMouvement.m_isDead = true;
        SetRespawnTimeFromLevel(level);
        InGameManager.IGM.respawnCanvas.SetActive(true);

        InGameManager.IGM.JoystickCanvas.SetActive(false);
        InGameManager.IGM.PlayerHUDCanvas.SetActive(false);
    }
    [PunRPC]
    public void RPC_SetRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }
    public void SetRespawn()
    {
        m_HealthManager.ResetHealth();
        InGameManager.IGM.respawnCanvas.SetActive(false);

        InGameManager.IGM.JoystickCanvas.SetActive(true);
        InGameManager.IGM.PlayerHUDCanvas.SetActive(true);
        m_PlayerMouvement.m_isDead = false;
    }
    public IEnumerator RespawnCoroutine()
    {
        while (state == ActualState.DEAD)
        {
            m_ChampionCharacter.SetActive(false);
            this.transform.position = InGameManager.IGM.mySpawnPoint;
            m_Animator.SetBool("isDead", false);

            yield return new WaitForSeconds(1f);

            m_ChampionCollider.enabled = true;
            m_HitCollider.enabled = true;
            m_SetPlayerInGameInfo.m_ChampionCanvas.SetActive(true);
            m_ChampionCharacter.SetActive(true);
            state = ActualState.PASSIVE;
            StopCoroutine(RespawnCoroutine());
        }
    }
    public void ExecuteSetRecall()
    {
        m_MyPhotonView.RPC("RPC_SetRecall", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPC_SetRecall()
    {
        state = ActualState.IN_RECALL;
        m_RecallFx.SetActive(true);
    }   
    public void ExecuteEndRecall()  
    {
        m_MyPhotonView.RPC("RPC_EndRecall", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPC_EndRecall() 
    {
        StartCoroutine(RecallCoroutine());
    }
    public IEnumerator RecallCoroutine()
    {
        while (state == ActualState.IN_RECALL)
        {
            m_ChampionCharacter.SetActive(false);
            this.transform.position = InGameManager.IGM.mySpawnPoint;

            yield return new WaitForSeconds(1f);

            m_ChampionCollider.enabled = true;
            m_HitCollider.enabled = true;
            m_SetPlayerInGameInfo.m_ChampionCanvas.SetActive(true);
            m_ChampionCharacter.SetActive(true);
            state = ActualState.PASSIVE;
            m_RecallFx.SetActive(false);
            StopCoroutine(RecallCoroutine());
        }
    }
    #region Delegates
    //attack 1 Inits

    public void OnPointerUpAttack1(PointerEventData data)
    {
        if (!cancelCast)
        {
            //RPC
            OnClickAttack1();
        }
        else
        {
            cancelCast = false;
        }
        m_CancelCastButton.gameObject.SetActive(false);
        attack1Indicator.SetActive(false);
    }
    public void OnPointerUpAttack2(PointerEventData data)
    {
        if (!cancelCast)
        {
            //RPC
            OnClickAttack2();
        }
        else
        {
            cancelCast = false;
        }
        m_CancelCastButton.gameObject.SetActive(false);
        attack2Indicator.SetActive(false);

    }
    public void OnPointerUpAttack3(PointerEventData data)
    {
        if (!cancelCast)
        {
            //RPC
            OnClickAttack3();
        }
        else
        {
            cancelCast = false;
        }
        m_CancelCastButton.gameObject.SetActive(false);
        attack3Indicator.SetActive(false);
    }


    public void OnPointerDownAttack1(PointerEventData data)
    {
        m_CancelCastButton.gameObject.SetActive(true);
        attack1Indicator.SetActive(true);
    }
   
    public void OnPointerDownAttack2(PointerEventData data)
    {
        m_CancelCastButton.gameObject.SetActive(true);
        attack2Indicator.SetActive(true);
    }

    public void OnPointerDownAttack3(PointerEventData data)
    {
        m_CancelCastButton.gameObject.SetActive(true);
        attack3Indicator.SetActive(true);
    }

    public void OnPointerEnterCancelCast(PointerEventData data)
    {
        cancelCast = true;
    }
    public void OnPointerExitCancelCast(PointerEventData data)
    {
        cancelCast = false;
    }
    public void InitAttack1()
    {
        EventTrigger trigger = m_Attack1Image.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry PointerDown = new EventTrigger.Entry();
        PointerDown.eventID = EventTriggerType.PointerDown;
        PointerDown.callback.AddListener((data) => { OnPointerDownAttack1((PointerEventData)data); });
        trigger.triggers.Add(PointerDown);

        EventTrigger.Entry PointerUp = new EventTrigger.Entry();
        PointerUp.eventID = EventTriggerType.PointerUp;
        PointerUp.callback.AddListener((data) => { OnPointerUpAttack1((PointerEventData)data); });
        trigger.triggers.Add(PointerUp);

        Attack1Event = trigger;
    }

    //attack 2 Inits
    public void InitAttack2()
    {
        EventTrigger trigger = m_Attack2Image.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry PointerDown = new EventTrigger.Entry();
        PointerDown.eventID = EventTriggerType.PointerDown;
        PointerDown.callback.AddListener((data) => { OnPointerDownAttack2((PointerEventData)data); });
        trigger.triggers.Add(PointerDown);

        EventTrigger.Entry PointerUp = new EventTrigger.Entry();
        PointerUp.eventID = EventTriggerType.PointerUp;
        PointerUp.callback.AddListener((data) => { OnPointerUpAttack2((PointerEventData)data); });
        trigger.triggers.Add(PointerUp);

        Attack2Event = trigger;

    }
    //attack 2 Inits
    public void InitAttack3()
    {
        EventTrigger trigger = m_Attack3Image.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry PointerDown = new EventTrigger.Entry();
        PointerDown.eventID = EventTriggerType.PointerDown;
        PointerDown.callback.AddListener((data) => { OnPointerDownAttack3((PointerEventData)data); });
        trigger.triggers.Add(PointerDown);

        EventTrigger.Entry PointerUp = new EventTrigger.Entry();
        PointerUp.eventID = EventTriggerType.PointerUp;
        PointerUp.callback.AddListener((data) => { OnPointerUpAttack3((PointerEventData)data); });
        trigger.triggers.Add(PointerUp);

        Attack3Event = trigger;
    }
    //attack 2 Inits
    public void InitCancelButton()
    {
        EventTrigger trigger = m_CancelCastButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry PointerDown = new EventTrigger.Entry();
        PointerDown.eventID = EventTriggerType.PointerEnter;
        PointerDown.callback.AddListener((data) => { OnPointerEnterCancelCast((PointerEventData)data); });
        trigger.triggers.Add(PointerDown);

        EventTrigger.Entry PointerUp = new EventTrigger.Entry();
        PointerUp.eventID = EventTriggerType.PointerExit;
        PointerUp.callback.AddListener((data) => { OnPointerExitCancelCast((PointerEventData)data); });
        trigger.triggers.Add(PointerUp);
    }
    #endregion
    public void InitCastsButtons()
    {
        InitAttack1();
        InitAttack2();
        InitAttack3();
        InitCancelButton();
    }
    public void InitSpellsButtons()
    {

    }
    public void SetRespawnTimeFromLevel(int _level)
    {
        m_respowntime = 6;
    }
    public void UpdateReswpanTimeValue()
    {
        if (m_MyPhotonView.IsMine)
        {
            if (state == ActualState.DEAD)
            {
                if (m_respowntime > 1)
                {
                    m_respowntime -= Time.deltaTime;
                    InGameManager.IGM.respawnTimeUI.text =" Respawn in : " +Mathf.RoundToInt(m_respowntime).ToString()+" s";
                    if (!InGameManager.IGM.respawnCanvas.activeSelf)
                    InGameManager.IGM.respawnCanvas.SetActive(false);

                }
                else
                {
                    ExecuteRespawn();
                    SetRespawn();
                  
                }
            }
        }
        
       
    }


    public void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("I'm the master");
            InGameManager.IGM.m_MasterPhotonView = m_MyPhotonView;
            InGameManager.IGM.InitTowers();
        }
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {

    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
    }

   
}
