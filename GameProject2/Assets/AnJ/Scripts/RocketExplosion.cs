using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace rocket
{
public class RocketExplosion : MonoBehaviour {
		public LayerMask m_TankMask; 
	public ParticleSystem m_ExplosionParticles;
		public AudioSource m_ExplosionAudio;
		public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
		public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
		public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
		int maxDamage =60;
	public float autodestroytime=5;
	bool burn=false;
	void Start(){
		Destroy (gameObject, autodestroytime);
	}
	private void OnTriggerEnter(Collider other) 
	{

			Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);

			for (int i = 0; i < colliders.Length; i++)
			{
				objStateControl findobjStateControl;
				if (findobjStateControl= colliders[i].gameObject.GetComponentInParent<objStateControl>())
				{
					float damage = CalculateDamage (findobjStateControl.transform.position);
					findobjStateControl.OnDamage(damage);
					if(burn)
						findobjStateControl.startBurn ();
				}
			}

		// Play the particle system.
			ParticleSystem exp = ParticleSystem.Instantiate (m_ExplosionParticles, transform.position, transform.rotation);
			exp.Play ();
			ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
			Destroy (exp.gameObject, mainModule.duration/4);

		// Play the explosion sound effect.
			AudioSource expa=AudioSource.Instantiate(m_ExplosionAudio,transform.position, transform.rotation);
			expa.PlayOneShot (expa.clip);
			Destroy (expa.gameObject, 1f);
		// Once the particles have finished, destroy the gameobject they are on.

		// Destroy the shell.
		Destroy (gameObject);
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
}