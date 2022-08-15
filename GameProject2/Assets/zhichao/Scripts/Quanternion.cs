using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
public class Quanternion : MonoBehaviour {
    public float x, y;
    public float xSpeed = 100;
    public float ySpeed = 100;
    public Quaternion rotationEuler;
    public GameObject trackRot;
    // Use this for initialization
    [DllImport("user32")]
    static extern bool SetCursorPos(int x, int y);
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
       // UnityEngine.Cursor.visible = false;
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.L))
            SetCursorPos(700,350);
        if (x > 360)
            x -= 360;
        else if (x < 0)
        {
            x += 360;
        }
        rotationEuler = Quaternion.Euler(y, x, 0);
        transform.rotation = rotationEuler;
    }
}
