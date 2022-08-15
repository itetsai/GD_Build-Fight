using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPEnemyorAlly : MonoBehaviour {
    List<GameObject> hpbars = new List<GameObject>();
	// Use this for initialization
	void Start () {
        StartCoroutine(getHpBar());
        
    }
    public IEnumerator  getHpBar()
    {
        hpbars.Clear();
        yield return new WaitForSeconds(1f);
        hpbars.AddRange(GameObject.FindGameObjectsWithTag("HpBar"));

    }
	// Update is called once per frame
	void Update () {
        if (Camera.current == GetComponent<Camera>())
        {
            Debug.Log(gameObject.tag);
            for (int i = 0; i < hpbars.Count; i++)
            {
                if (hpbars[i].transform.parent.gameObject.tag == gameObject.tag)
                {
                    hpbars[i].GetComponentsInChildren<Image>()[1].color = Color.green;
                }
                else
                {
                    hpbars[i].GetComponentsInChildren<Image>()[1].color = Color.red;
                }
            }
        }
	}
}
