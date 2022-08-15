using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class CubeSelect : MonoBehaviour {
    public Transform panel;
    public GameObject turrent,wheel,rocket,regular,grenade,foot,fly,track,cube;
    public Text[] test;
    void Start ()
    {
        panel.gameObject.SetActive(false);
    }
    //void Awake()
    //{

    //    CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

    //    float screenWidthScale = Screen.width / canvasScaler.referenceResolution.x;
    //    float screenHeightScale = Screen.height / canvasScaler.referenceResolution.y;

    //    canvasScaler.matchWidthOrHeight = screenWidthScale > screenHeightScale ? 1 : 0;
    //}

    // Update is called once per fram

    void Update ()
    {
        turrent.transform.Rotate(0, 50 * Time.deltaTime, 0);
        rocket.transform.Rotate(0, 50 * Time.deltaTime, 0);
        regular.transform.Rotate(0, 50 * Time.deltaTime, 0);
        grenade.transform.Rotate(0, 50 * Time.deltaTime, 0);
        wheel.transform.Rotate(0, 50 * Time.deltaTime, 0);
        foot.transform.Rotate(0, -50 * Time.deltaTime, 0);
        fly.transform.Rotate(0, 50 * Time.deltaTime, 0);
        track.transform.Rotate(0, 50 * Time.deltaTime, 0);
        cube.transform.Rotate(0, 50 * Time.deltaTime, 0);


        if (Input.GetKeyDown(KeyCode.P))
        {
            if(panel.gameObject.activeInHierarchy==false)
            {
                panel.gameObject.SetActive(true);
                GameObject g = GameObject.Find("Creater");
                if (g != null)//解鎖鎖定畫面
                {
                    GameObject.Find("Creater").GetComponent<NewFirstPersonControl>().enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

            }
            else
            {
                panel.gameObject.SetActive(false);
                GameObject g = GameObject.Find("Creater");
                if (g != null)//鎖定畫面
                {
                    GameObject.Find("Creater").GetComponent<NewFirstPersonControl>().enabled = true;
                }
            }
            /*test = panel.GetComponentsInChildren<Text>();
            for (int i = 0; i < test.GetLength(0); i++)
            {
                test[i].color = Color.clear;
            }*/

        }

	}

    public void ClosePanel()
    {
        panel.gameObject.SetActive(false);
    }
}
