using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonChangeColor : MonoBehaviour {
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;


    
	// Update is called once per frame
	void Update ()
    {
        //如果按下熱鍵
        if (Input.GetKey(KeyCode.Q))
        {
            //設定按鈕圖片的顏色為灰色
             image1.color= new Color32(200, 200, 200, 140);
        }

        //如果放開熱鍵
        if (Input.GetKeyUp(KeyCode.Q))
        {
            image1.color = Color.white;
        }
        //如果按下熱鍵
        if (Input.GetKey(KeyCode.E))
        {
            //設定按鈕圖片的顏色為灰色
            image2.color = new Color32(200, 200, 200, 140);
        }

        //如果放開熱鍵
        if (Input.GetKeyUp(KeyCode.E))
        {
            image2.color = Color.white;
        }
        //如果按下熱鍵
        if (Input.GetKey(KeyCode.Alpha1))
        {
            //設定按鈕圖片的顏色為灰色
            image3.color = new Color32(200, 200, 200, 140);
        }

        //如果放開熱鍵
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            image3.color = Color.white;
        }
        //如果按下熱鍵
        if (Input.GetKey(KeyCode.Alpha2))
        {
            //設定按鈕圖片的顏色為灰色
            image4.color = new Color32(200, 200, 200, 140);
        }

        //如果放開熱鍵
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            image4.color = Color.white;
        }



    }
}
