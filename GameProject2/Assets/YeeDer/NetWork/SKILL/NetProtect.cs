using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetProtect : NetworkBehaviour {

    [SyncVar]
    public int connectID;

    [SyncVar(hook = "Skill")]
    public bool useSkill = false;

    public SphereCollider temp;
    public ParticleSystem test;
    public ParticleSystem test2;
    // Use this for initialization

    float r1;
    float r2;
    float r3;
    void Start()
    {
        r1 = temp.radius;
        r2 = test.startSize;
        r3 = test2.startSize;
        GameObject[] robotlist = GameObject.FindGameObjectsWithTag("Robot");

        if (isServer)
        {
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);
        }

        for (int i = 0; i < robotlist.Length; i++)
            if (robotlist[i].GetComponent<RobotProperty>().connectID == connectID)
            {
                transform.parent = robotlist[i].transform.Find("MiddlePoint");
            }
    }

    public float WaitTime = 20;
    public float time = 20;
    public float SkillTime = 5;

    private void Update()
    {
        if (hasAuthority)
        {
            time += Time.deltaTime;
            if (time > WaitTime)
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    CmdUseSkill();
                    time = 0;
                }

            if (useSkill == true)
                if (time > SkillTime)
                {
                    CmdSkillEnd();
                }
        }
    }

    [Command]
    void CmdUseSkill()
    {
        useSkill = true;
    }

    [Command]
    void CmdSkillEnd()
    {
        useSkill = false;
    }



    public Transform Middle;

    public float multiple;
    // Use this for initialization
    public float maxX = -10000, maxY = -10000, maxZ = -10000, minX = 10000, minY = 10000, minZ = 10000;

    public GameObject Object; 
    void Skill(bool use)
    {
        maxX = -10000;
        maxY = -10000;
        maxZ = -10000;
        minX = 10000;
        minY = 10000;
        minZ = 10000;

        if (hasAuthority)
        {
            temp.gameObject.SetActive(false);
        }
        if (use == true)
        {
            Renderer[] renderer = transform.root.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderer.Length; i++)
            {
                if (renderer[i].tag == "Skill")
                    continue;

                if (renderer[i].transform.position.x > maxX)
                    maxX = renderer[i].transform.position.x;
                if (renderer[i].transform.position.y > maxY)
                    maxY = renderer[i].transform.position.y;
                if (renderer[i].transform.position.z > maxZ)
                    maxZ = renderer[i].transform.position.z;
                if (renderer[i].transform.position.x < minX)
                    minX = renderer[i].transform.position.x;
                if (renderer[i].transform.position.y < minY)
                    minY = renderer[i].transform.position.y;
                if (renderer[i].transform.position.z < minZ)
                    minZ = renderer[i].transform.position.z;
            }

            float radius;
            //this.transform.position = new Vector3(( maxX +  minX) / 2, ( maxY +  minY) / 2, ( maxZ +  minZ) / 2);
            radius = (maxX - minX) / 2;
            if ((maxY - minY) / 2 > radius)
                radius = (maxY - minY) / 2;
            if ((maxZ - minZ) / 2 > radius)
                radius = (maxZ - minZ) / 2;

            radius *= multiple;
            print(maxX);
            print(maxY);
            print(maxZ);
            print(minX);
            print(minY);
            print(minZ);




            Middle.transform.position = new Vector3((maxX + minX) / 2.0f, (maxY + minY) / 2.0f, (maxZ + minZ) / 2.0f);
            print(Middle.transform.position);
            print(radius);

            temp.radius = radius*r1;
            test.startSize = radius*r2;
            test2.startSize = radius*r3;

            Object.SetActive(true);
            useSkill = true;
        }

        if (use == false)
        {
            Object.SetActive(false);
            useSkill = false;
        }
    }

}
