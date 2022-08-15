using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetInvisible : NetworkBehaviour
{
    [SyncVar]
    public int connectID;

    [SyncVar(hook = "Skill")]
    public bool useSkill = false;

    // Use this for initialization
    void Start()
    {
        GameObject[] robotlist = GameObject.FindGameObjectsWithTag("Robot");

        if (isServer)
        {
            GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkServer.connections[connectID]);
        }

        for (int i = 0; i < robotlist.Length; i++)
            if (robotlist[i].GetComponent<RobotProperty>().connectID == connectID)
            {
                transform.parent = robotlist[i].transform;
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
                if (Input.GetKeyDown(KeyCode.F1))
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
        print("InvisibleOn");
    }

    [Command]
    void CmdSkillEnd()
    {
        useSkill = false;
        print("InvisibleClose");
    }


    void Skill(bool use)
    {
        print("Change");
        if (use == true)
        {
            Renderer[] meshs = transform.parent.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < meshs.Length; i++)//讓物件的所有material變透明
            {
                for (int j = 0; j < meshs[i].materials.Length; j++)
                {
                    if (meshs[i].tag == "Skill")
                        continue;
                    meshs[i].material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");//換成會變透明的shader
                    Color color = meshs[i].materials[j].color;
                    color = new Color(color.r, color.g, color.b, 0.2f);
                    meshs[i].materials[j].color = color;
                }
            }
            useSkill = true;
        }

        if (use == false)
        {
            Renderer[] meshs = transform.parent.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < meshs.Length; i++)//讓物件的所有material變透明
            {
                for (int j = 0; j < meshs[i].materials.Length; j++)
                {
                    meshs[i].material.shader = Shader.Find("Legacy Shaders/Bumped Specular");
                    Color color = meshs[i].materials[j].color;
                    color = new Color(color.r, color.g, color.b, 1f);
                    meshs[i].materials[j].color = color;
                }
            }
            useSkill = false;
        }
    }
}
