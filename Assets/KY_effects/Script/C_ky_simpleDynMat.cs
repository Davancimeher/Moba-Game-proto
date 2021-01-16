using UnityEngine;
using System.Collections;

public class C_ky_simpleDynMat : MonoBehaviour {
		public	Material thisMat;
		private float prpValue = 0;
		private float prpValueBck = 0;
		public float prpValueDynSpd = 1;
		public float prpValueMax = 1;
		public string prpName;
		private ParticleSystem thisPS;
		private float particleTimeBck = 0;
		private bool resetFlg = false;

		public bool useFmod = false;
		public int fmodValue = 16;


		public bool isMatOffset = false;
		public Vector2 offsetSpd;
		private Vector2 tmpVec2;
		void Awake () {
				thisMat = this.GetComponent<Renderer>().material;
				if(prpName != "")prpValueBck = prpValue = thisMat.GetFloat(prpName);
				thisPS = this.GetComponent<ParticleSystem>();
				//tmpVec2 = new Vector2(0,0);
				//Debug.Log("tst");
		}

		void Start () {
				thisMat.SetFloat(prpName,prpValue);

		}


		void Update () {
				//reset control
				if( thisPS.time > particleTimeBck){
						particleTimeBck = thisPS.time;
						matOffset();
				}else{
						resetFlg = true;
				}



				if(prpValue <= prpValueMax ){
						prpValue += prpValueDynSpd * Time.deltaTime;
						if( useFmod )prpValue = prpValue % fmodValue;//Debug.Log(prpValue % 16);
						thisMat.SetFloat(prpName,prpValue);
				}

				if( !thisPS.isPlaying || resetFlg){
						prpValue = prpValueBck;
						thisMat.SetFloat(prpName,prpValueBck);
						resetFlg = false;
						particleTimeBck = 0;

						if( isMatOffset )thisMat.mainTextureOffset = Vector2.zero;
				}

		}

		void matOffset(){
				if( isMatOffset ){
						tmpVec2 = Time.deltaTime * offsetSpd;
						thisMat.mainTextureOffset += tmpVec2;//;Time.deltaTime * offsetYSpd;

								if( thisMat.mainTextureOffset.y > 1){
										tmpVec2 = Vector2.zero;
										thisMat.mainTextureOffset = tmpVec2;
								}
					//	thisMat.SetTextureOffset ("_MainTex", offsetYSpd);
				}
		}
}
