using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetMakeRadarObject : MonoBehaviour
{
    public Image image;
    // Use this for initialization
    void Start()
    {
        if (transform.parent.GetComponent<NetPlayerController>().local)
            RadarScript.playerPos = transform.parent;
        else
            RadarScript.RegisterRadarObject(this.gameObject, image);
    }

    private void OnDestroy()
    {
        RadarScript.RemomveRadarObject(this.gameObject);
    }

}
