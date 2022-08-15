using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetShellExplosion : NetworkBehaviour
{
	public NetPlayerController fromPlayer;
	public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
	public GameObject m_ExplosionParticles;         // Reference to the particles that will play on explosion.
	//public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.                // The amount of damage done if the explosion is centred on a tank.
	public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
	public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
	public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
	public int maxDamage =80;
	bool burn=false;
	private void Start ()
	{
		// If it isn't destroyed by then, destroy the shell after it's lifetime.
		Destroy (gameObject, m_MaxLifeTime);
	}


    private void OnTriggerEnter(Collider other)
    {
		print (other.gameObject.name + " layer= " + other.gameObject.layer);
		NetobjStateControl findobjStateControl;
		if (findobjStateControl = other.gameObject.GetComponentInParent<NetobjStateControl> ())
		if ((findobjStateControl.main.player == fromPlayer)&&(findobjStateControl.gameObject.GetComponent<ObjectName>().kind=="gun"))
			return;
		if (other.gameObject.layer != 11) {
			Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius);

			for (int i = 0; i < colliders.Length; i++) {
				if (findobjStateControl = colliders [i].gameObject.GetComponentInParent<NetobjStateControl> ()) {
					if (findobjStateControl.team != fromPlayer.Team) {
						float damage = CalculateDamage (findobjStateControl.transform.position);
						findobjStateControl.OnDamage (damage);
						if (burn)
							findobjStateControl.startBurn ();
					}
				}
			}

			GameObject ex = Instantiate (m_ExplosionParticles, transform.position, transform.rotation);
			Destroy (ex, 1);
			NetworkServer.Spawn (ex);

			Destroy (gameObject);
		}
    }
	private float CalculateDamage (Vector3 targetPosition)
	{
		// Create a vector from the shell to the target.
		Vector3 explosionToTarget = targetPosition - transform.position;

		// Calculate the distance from the shell to the target.
		float explosionDistance = explosionToTarget.magnitude;

		// Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
		float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

		// Calculate damage as this proportion of the maximum possible damage.
		float damage = relativeDistance * maxDamage;

		// Make sure that the minimum damage is always 0.
		damage = Mathf.Max (0f, damage);

		return damage;
	}
}

