using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textTranslate : MonoBehaviour
{
    // Use this for initialization
    public float time;
    public RectTransform[] temp;
    public int i = 0;
    void Start()
    {
		GetComponent<Canvas> ().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (i != 8 /*&& temp[i].anchoredPosition.y <= 0*/)
        {
            temp[i].anchoredPosition =new Vector2 (temp[i].anchoredPosition.x,0);
            i++;
            time = 0;
        }
        if (i < 8)
        {
            time += Time.deltaTime;

            temp[i].position -= new Vector3(0, 0, time * 0.2f);
        }
        else
        {
            time += Time.deltaTime * 4;
            if ((int)(time) > 0)
            {
                if ((int)((time) / 1.5f) % 2 == 0)
                    temp[0].position += new Vector3(0, 0, Time.deltaTime * 0.1f);
                else
                    temp[0].position -= new Vector3(0, 0, Time.deltaTime * 0.1f);
            }
            if ((int)(time -0.5f) > 0)
            {
                if ((int)((time - 0.5f) / 1.5f) % 2 == 0)
                    temp[1].position += new Vector3(0, 0, Time.deltaTime * 0.1f);
                else
                    temp[1].position -= new Vector3(0, 0, Time.deltaTime * 0.1f);
            }
            if ((int)(time - 1f) > 0)
            {
                if ((int)((time - 1f) / 1.5f) % 2 == 0)
                    temp[2].position += new Vector3(0, 0, Time.deltaTime * 0.1f);
                else
                    temp[2].position -= new Vector3(0, 0, Time.deltaTime * 0.1f);
            }
            if ((int)(time - 1.5f) > 0)
            {
                if ((int)((time - 1.5f) / 1.5f) % 2 == 0)
                    temp[3].position += new Vector3(0, 0, Time.deltaTime * 0.1f);
                else
                    temp[3].position -= new Vector3(0, 0, Time.deltaTime * 0.1f);
            }
            if ((int)(time - 2f) > 0)
                if ((int)((time - 2f) / 1.5f) % 2 == 0)
                    temp[4].position += new Vector3(0, 0, Time.deltaTime * 0.1f);
                else
                    temp[4].position -= new Vector3(0, 0, Time.deltaTime * 0.1f);
            if ((int)(time - 2.5f) > 0)
            {
                if ((int)((time - 2.5f) / 1.5f) % 2 == 0)
                    temp[5].position += new Vector3(0, 0, Time.deltaTime * 0.1f);
                else
                    temp[5].position -= new Vector3(0, 0, Time.deltaTime * 0.1f);
            }
            if ((int)(time - 3f) > 0)
            {
                if ((int)((time - 3f) / 1.5f) % 2 == 0)
                    temp[6].position += new Vector3(0, 0, Time.deltaTime * 0.1f);
                else
                    temp[6].position -= new Vector3(0, 0, Time.deltaTime * 0.1f);
            }
            if ((int)(time - 3.5f) > 0)
            {
                if ((int)((time - 3.5f) / 1.5f) % 2 == 0)
                    temp[7].position += new Vector3(0, 0, Time.deltaTime * 0.1f);
                else
                    temp[7].position -= new Vector3(0, 0, Time.deltaTime * 0.1f);
            }
        }
    }
}
