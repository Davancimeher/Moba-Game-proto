using Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{

    public ChampionManager ChampionManager;
    public Spell m_RecallSpell; 
    public Spell m_actualSpell;

    private float m_spellDuration;
    private float m_RecallDuration;
    public bool m_InRecall = false;

    public bool m_InSpellEffect = false;

    private Dictionary<int, Spell> m_SpellsDict = new Dictionary<int, Spell>();

    private SpellManager MySpellManager;

    public Image SpellIcon;

    private void Start()
    {
        LoadSpells();
    }

    private void LoadSpells()
    {
        var spellList = new List<Spell>(Resources.LoadAll<Spell>(GlobalVariables.m_SpellsPath));

        foreach (var spell in spellList)
        {
            if (!m_SpellsDict.ContainsKey(spell.SpellID))
            {
                m_SpellsDict.Add(spell.SpellID, spell);
            }
        }
    }

    public void ExecuteSpellRPC(PhotonView senderPhotonView, Player playerDamaged, int spellId)
    {
        senderPhotonView.RPC("SetNewSpell", playerDamaged, spellId);
    }

    [PunRPC]
    public void SetNewSpell(int spellId)
    {
        if (MySpellManager == null)
        {
            if (RoomData.RD.PlayersChampions.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
            {
                MySpellManager = RoomData.RD.PlayersChampions[PhotonNetwork.LocalPlayer.ActorNumber].GetComponent<SpellManager>();
            }
        }
        MySpellManager.RunNewSpell(spellId);
    }
    public void RunNewSpell(int spellId)
    {
        if (!m_InSpellEffect)
        {
            if (!m_SpellsDict.ContainsKey(spellId)) return;

            m_actualSpell = m_SpellsDict[spellId];
            ExecuteSpell();
            ExecuteRPCSpell(m_actualSpell.SpellID);
        }
    }
    public void RunNewSpell(Spell spell)
    {
        m_actualSpell = spell;
        ExecuteSpell();
        ExecuteRPCSpell(m_actualSpell.SpellID);
    }
    public void ExecuteSpell()
    {
        m_spellDuration = m_actualSpell.SpellDuration;
        m_actualSpell.SetSpell(this);
        m_InSpellEffect = true;
        Debug.LogError("ExecuteSpell : " + m_actualSpell.name);

        if (!m_actualSpell.CanAttack)
            ChampionManager.ExecuteStopAutoAttackingFromSpell();
    }
    public void ResetSpellValues()
    {
        if (m_InSpellEffect)
        {
            m_actualSpell.ResetSpell(this);
            m_InSpellEffect = false;
            SpellIcon.gameObject.SetActive(false);
            ExecuteHideSpellIcon();
        }
    }
    public void UpdateSpellValue()
    {
        if (m_InSpellEffect)
        {
            if (m_spellDuration > 0)
            {
                m_spellDuration -= Time.deltaTime;
            }
            else
            {
                ResetSpellValues();
            }
        }
        if (m_InRecall)
        {
            if (m_RecallDuration > 0)
            {
                m_RecallDuration -= Time.deltaTime;
            }
            else
            {
                ExecuteEndRecallSpell();
            }
        }
    }

    private void Update()
    {
        UpdateSpellValue();
    }

    public void ExecuteRPCSpell(int spellId)
    {
        ChampionManager.m_MyPhotonView.RPC("RPC_SetSpell", RpcTarget.AllViaServer, spellId);
    }
    public void ExecuteHideSpellIcon()
    {
        ChampionManager.m_MyPhotonView.RPC("RPC_ResetSpell", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPC_SetSpell(int spellId)
    {
        m_actualSpell = m_SpellsDict[spellId];

        if (m_actualSpell.HasRpcCalls)
            m_actualSpell.SetRpcVaribales(this);

        SpellIcon.sprite = m_actualSpell.SpellSprite;
        SpellIcon.gameObject.SetActive(true);
    }

    [PunRPC]
    public void RPC_ResetSpell()
    {
        if (m_actualSpell.HasRpcCalls)
            m_actualSpell.ResetRpcVaribales(this);

        SpellIcon.gameObject.SetActive(false);
    }

    public void ExecutePassiveSpell(Spell _spell)
    {
        if(_spell.SpellType == SpellType.PASSIVE)
        {

        }
    }
    public void RPC_SetPassiveSpell(int _spellId)
    {

    }

    public void ExecuteRecallSpell()
    {
        ChampionManager.m_MyPhotonView.RPC("RPC_RunRecallSpell", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPC_RunRecallSpell()
    {
        m_RecallSpell.SetRpcVaribales(this);
        m_InRecall = true;
        m_RecallDuration = m_RecallSpell.SpellDuration;
    }
    public void ExecuteEndRecallSpell()
    {
        ChampionManager.m_MyPhotonView.RPC("RPC_RunEndRecallSpell", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPC_RunEndRecallSpell()
    {
        m_RecallSpell.ResetRpcVaribales(this);
        m_InRecall = false;
    }
}
