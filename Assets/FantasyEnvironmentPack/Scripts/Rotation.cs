using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

    public float speed = 0;
    public bool y = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!y)
        transform.Rotate(0,0,speed*Time.deltaTime);
        if (y)
            transform.Rotate(0, speed * Time.deltaTime, 0);

    }
}
