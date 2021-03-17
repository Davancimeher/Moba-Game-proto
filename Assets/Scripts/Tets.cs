using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tets : MonoBehaviour
{
    public bool m_ElecSabotage;
    public bool m_O2Sbatage;
    public bool m_ReactorSbatage;

    // lighting ........... 
    //
    //

    public void ElectricalSbatoge()
    {

    }
    public void O2Sbatoge()
    {

    }

    private void Update()
    {
        if (m_ElecSabotage)
        {
            ElectricalSbatoge();
        }
        if (m_O2Sbatage)
        {
            O2Sbatoge();
        }
    }

}
