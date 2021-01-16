using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugTowerBehaviour : MonoBehaviour
{
    public Dictionary<int, TowerTarget> keyValuePairs = new Dictionary<int, TowerTarget>();
    public List<TowerTarget> vs = new List<TowerTarget>();

    public TowerTarget actualTarget;

    private void FixedUpdate()
    {
        //foreach (var vs in vs)
        //{
        //    if (PlayerInZone(vs))
        //    {
        //        Debug.Log(vs.gameObject.name);
        //    }
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
       var tt=  other.gameObject.GetComponent<TowerTarget>();
        if(tt != null)
        AddTarget(tt);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");

        var tt = other.gameObject.GetComponent<TowerTarget>();
        if (tt != null)
            RemoveTarget(tt);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveTarget(actualTarget);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            getTraget();
        }
    }

    public void AddTarget(TowerTarget TowerTarget)
    {
        keyValuePairs.Add(TowerTarget.ViewID, TowerTarget);
    }
    public void RemoveTarget(TowerTarget TowerTarget)
    {
        keyValuePairs.Remove(TowerTarget.ViewID);
    }
    public void getTraget()
    {
        if(keyValuePairs.Count > 0)
        {
            actualTarget=  keyValuePairs.First().Value;
        }
        else
        {
            Debug.Log("No Target");
        }
    }

    public bool PlayerInZone(TowerTarget target)
    {
        var playerPos = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.x);
        return Vector2.Distance(playerPos, this.transform.position) < Mathf.Sqrt(5.8f);
    }
    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5.8f);
    }
   
}
