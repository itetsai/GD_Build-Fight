using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shield : MonoBehaviour
{
    public SphereCollider temp;
    public ParticleSystem test;
    public ParticleSystem test2;
    public Transform Middle;

    public float multiple;
    // Use this for initialization
    public float maxX = 0, maxY = 0, maxZ = 0, minX = 0, minY = 0, minZ = 0;
    void Start()
    {
        Renderer[] renderer = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderer.Length; i++)
        {
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

        Middle.transform.position = new Vector3((maxX + minX) / 2.0f, (maxY + minY) / 2.0f, (maxZ + minZ) / 2.0f);
        print(Middle.transform.position);
        print(radius);

        temp.radius *= radius;
        test.startSize *= radius;
        test2.startSize *= radius;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
