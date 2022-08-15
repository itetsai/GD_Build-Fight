using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBeamCollision : MonoBehaviour {
    public bool Reflect = false;
	public NetPlayerController fromPlayer;
	public int team=0;
    private BeamLine BL;
    int damage = 100;
    public GameObject HitEffect = null;
	bool burn=false;
    private bool bHit = false;

    private BeamParam BP;

    // Use this for initialization
    void Start()
    {

        BL = (BeamLine)this.gameObject.transform.Find("BeamLine").GetComponent<BeamLine>();
        BP = this.transform.root.gameObject.GetComponent<BeamParam>();
    }

    // Update is called once per frame
    void Update()
    {
        //RayCollision
        RaycastHit hit;
        int layerMask = ~(1 << LayerMask.NameToLayer("NoBeamHit") | 1 << 2);
        if (HitEffect != null && !bHit && Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            GameObject hitobj = hit.collider.gameObject;
            if (hit.distance < BL.GetNowLength())
            {
                BL.StopLength(hit.distance);
                bHit = true;

                Quaternion Angle;
                //Reflect to Normal
                if (Reflect)
                {
                    Angle = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal));
                }
                else
                {
                    Angle = Quaternion.AngleAxis(180.0f, transform.up) * this.transform.rotation;
                }
                GameObject obj = (GameObject)Instantiate(HitEffect, this.transform.position + this.transform.forward * hit.distance, Angle);
                obj.GetComponent<BeamParam>().SetBeamParam(BP);
                obj.transform.localScale = this.transform.localScale;
            }


			NetobjStateControl findobjStateControl;
			if ((findobjStateControl= hitobj.GetComponentInParent<NetobjStateControl>())!=null)
			{
				if (findobjStateControl.team != team) {
					findobjStateControl.OnDamage (damage);
					if (burn)
						findobjStateControl.startBurn ();
				}

			}



            //print("find" + hit.collider.gameObject.name);
        }
    }
}
