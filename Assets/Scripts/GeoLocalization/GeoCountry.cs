using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// The Geo data for a user.
/// 
/// http://ip-api.com/docs/api:json
/// 
/// <code>
/// {
/// 	"status": "success",
/// 	"country": "COUNTRY",
/// 	"countryCode": "COUNTRY CODE",
/// 	"region": "REGION CODE",
/// 	"regionName": "REGION NAME",
/// 	"city": "CITY",
/// 	"zip": "ZIP CODE",
/// 	"lat": LATITUDE,
/// 	"lon": LONGITUDE,
/// 	"timezone": "TIME ZONE",
/// 	"isp": "ISP NAME",
/// 	"org": "ORGANIZATION NAME",
/// 	"as": "AS NUMBER / NAME",
/// 	"query": "IP ADDRESS USED FOR QUERY"
/// }
/// </code>
/// 
/// </summary>
public class GeoData
{
	/// <summary>
	/// The status that is returned if the response was successful.
	/// </summary>
	public const string SuccessResult = "success";

	[JsonProperty("status")]
	public string Status { get; set; }

	[JsonProperty("country")]
	public string Country { get; set; }

	[JsonProperty("countryCode")]
	public string countryCode { get; set; }

	[JsonProperty("city")]
	public string city { get; set; }

	[JsonProperty("timezone")]
	public string timezone { get; set; }

	[JsonProperty("isp")]
	public string isp { get; set; }

	[JsonProperty("query")]
	public string IpAddress { get; set; }
}

public class GeoCountry : MonoBehaviour
{
	public GeoData m_GeoData;
	public SpriteRenderer SpriteRenderer;

	public Image image;

    private void Start()
    {
		SendGetCountry();

	}
    public void SendGetCountry()
	{
		string link = "http://ip-api.com/json";

		var request = (HttpWebRequest)WebRequest.Create(link);
		request.Method = "GET";

		var httpResponse = (HttpWebResponse)request.GetResponse();

		using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
		{
			var result = streamReader.ReadToEnd();
			GeoData data = JsonConvert.DeserializeObject<GeoData>(result);
			m_GeoData = data;
			if(m_GeoData.countryCode == "IL")// I DON'T KNOW SOMETHING NAMED IL or ISRAEL ...
            {
				m_GeoData.Country = "PALASTINE";
				m_GeoData.countryCode = "PS";
			}
			StartGetTexture();

		}
	}
	void StartGetTexture()
	{
		StartCoroutine(GetTexture());
	}

	IEnumerator GetTexture()
	{
		UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.countryflags.io/"+m_GeoData.countryCode.ToLower()+ "/shiny/64.png");
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			Sprite blankSprite = Sprite.Create(myTexture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
			//SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
			//spriteRenderer.sprite = blankSprite;
			image.sprite = blankSprite;
		}

		

	}
}