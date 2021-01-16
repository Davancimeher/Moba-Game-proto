using UnityEngine;
using System.Collections;

public class C_ky_matControl : MonoBehaviour {
		public	Material thisMat;
		private float prpValue = 0;
		private float prpValueBck = 0;
		public float prpValueDynSpd = 1;
		public float prpValueMax = 1;
		public string prpName;
		private ParticleSystem thisPS;
		//public ParticleSystem.Particle[] thisP;
		private float particleTimeBck = 0;
		private bool resetFlg = false;

	void Awake () {
				thisMat = this.GetComponent<Renderer>().material;
				prpValueBck = prpValue = thisMat.GetFloat(prpName);
				thisPS = this.GetComponent<ParticleSystem>();
				//thisPS.GetParticles;
				//Debug.Log(thisPS.GetParticles[0].lifetime);
				//thisPS.startLifetime;
				//thisP[0].lifetime
		//Debug.Log("tst");
	}

	void Start () {
				thisMat.SetFloat(prpName,prpValue);

	}
	
	// Update is called once per frame
	void Update () {
				if( thisPS.time > particleTimeBck){
					particleTimeBck = thisPS.time;
				}else{
					resetFlg = true;
				}
						
				if(prpValue <= prpValueMax ){
						prpValue += prpValueDynSpd * Time.deltaTime;
						thisMat.SetFloat(prpName,prpValue);
				}

				if( !thisPS.isPlaying || resetFlg){
						prpValue = prpValueBck;
						thisMat.SetFloat(prpName,prpValueBck);
						resetFlg = false;
						particleTimeBck = 0;
				}

	}
}
