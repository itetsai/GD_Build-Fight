using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MakeRadarObject : MonoBehaviour {
    public Image image;
    // Use this for initialization
    void Start()
    {
        RadarScript.RegisterRadarObject(this.gameObject, image);
    }

    private void OnDestroy()
    {
        RadarScript.RemomveRadarObject(this.gameObject);
    }

}
	
