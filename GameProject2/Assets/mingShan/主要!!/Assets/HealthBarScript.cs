using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {
    public float minValue;
    public float maxValue;
    public Image HealthBar;
    public float max_health = 100f;
    public float cur_health;
	public NetPlayerController localplayer;
    public Color32 StartColor;
    public Color32 EndColor;

	// Use this for initialization
	void Start () {
    }
	public void init()
	{
		max_health = localplayer.main.totalHp;
		cur_health =max_health ;
		setHealth(cur_health);
	}

	//void decreaseHealth()
 //   {
 //       if (cur_health <= 0)
 //       {
 //           cur_health = 0;
 //       }
 //       else
 //       {
 //           cur_health -= 5f;
 //           setHealth();
 //       }
 //   }
	public  void setHealth(float cur_health)
    {
        float calc_health = cur_health / max_health; //   88/100=0.88f
        float output_health = calc_health * (maxValue - minValue)+minValue;

        HealthBar.fillAmount = Mathf.Clamp(output_health, minValue, maxValue);
        HealthBar.color = Color.Lerp(EndColor, StartColor, calc_health);
    }

}
