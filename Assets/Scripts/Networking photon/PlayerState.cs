using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    DISCONECTED,
    CONNECTED,
    IN_MAIN_PANEL,
    INLOBBY,
    IN_READY_PANEL,
    IN_HERO_PANEL,
    IN_LOADING_PANEL
}
public class PlayerState : MonoBehaviour
{
    public static PlayerState m_Instance;

    private State myState;

    public State m_State
    {
        get { return myState; }
        set
        {
            myState = value;
            UIManager.UIM.PanelManaging(myState);
        }
    }
    private void Awake()
    {
        #region singleton
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            if (m_Instance != this)
            {
                m_Instance = this;
            }
        }
        #endregion
    }

    public void OverrideState(State _state)
    {
        m_State = _state;
    }
}
