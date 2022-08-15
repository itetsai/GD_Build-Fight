using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpDisplay : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    public GameObject enemyhpbar, allyhpbar;
    // Use this for initialization
    void Start()
    {
        Debug.Log(gameObject.tag);
        players.AddRange(GameObject.FindGameObjectsWithTag("BlueTeam"));
        players.AddRange(GameObject.FindGameObjectsWithTag("RedTeam"));

        for (int i = 0; i < players.Count; i++)
        {
            GameObject hpbar;
            /* if (players[i].tag != gameObject.tag)
             {
                 hpbar = GameObject.Instantiate(enemyhpbar, players[i].transform.position, players[i].transform.rotation);
                 hpbar.transform.parent = players[i].transform;
             }
             else*/

            hpbar = GameObject.Instantiate(allyhpbar, players[i].transform.position, players[i].transform.rotation);
            hpbar.transform.parent = players[i].transform;
            hpbar.GetComponent<RectTransform>().localPosition = new Vector3(0, 2, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
