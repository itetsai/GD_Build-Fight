using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class RadarObject
{
    public Image icon { get; set; }
    public GameObject owner { get; set; }
}
public class RadarScript : NetworkBehaviour
{
    public static GameObject MyPlayer;

    public static Transform playerPos;
    float mapScale = 2.0f;

    public static List<RadarObject> radObjects = new List<RadarObject>();

    public static void RegisterRadarObject(GameObject o, Image i)
    {
        Image image = Instantiate(i);
        radObjects.Add(new RadarObject() { owner = o, icon = image });
    }

    public static void RemomveRadarObject(GameObject o)
    {
        List<RadarObject> newList = new List<RadarObject>();
        for (int i = 0; i < radObjects.Count; i++)
        {
            if (radObjects[i].owner == o)
            {
                Destroy(radObjects[i].icon);
                continue;
            }
            else
                newList.Add(radObjects[i]);
        }
        radObjects.RemoveRange(0, radObjects.Count);
        radObjects.AddRange(newList);
    }

    void DrawRadarDots()
    {
        if (!isServer)
        {
            foreach (RadarObject ro in radObjects)
            {
                Vector3 radarPos = (ro.owner.transform.position - playerPos.position);
                float disToObject = Vector3.Distance(playerPos.position, ro.owner.transform.position) * mapScale;
                float deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
                radarPos.x = disToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
                radarPos.z = disToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

                if (MyPlayer == null)
                    MyPlayer = Camera.current.gameObject;

                if (ro.owner.tag == MyPlayer.tag)
                {
                    ro.icon.color = Color.green;
                }
                else
                {
                    ro.icon.color = Color.red;

                }
                ro.icon.transform.SetParent(this.transform);
                ro.icon.transform.position = new Vector3(radarPos.x, radarPos.z, 0) + this.transform.position;
            }
        }
    }
    void Update()
    {
        DrawRadarDots();
    }
}
