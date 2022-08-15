using UnityEngine;

namespace Complete
{
    public class ShellExplosion : MonoBehaviour
    {
        public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
        public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.                // The amount of damage done if the explosion is centred on a tank.
        public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
        public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
        public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
		int maxDamage =80;
		bool burn=false;


        private void Start ()
        {
            // If it isn't destroyed by then, destroy the shell after it's lifetime.
            Destroy (gameObject, m_MaxLifeTime);
        }


        private void OnTriggerEnter (Collider other)
        {
			Debug.Log("hit");
            // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
            
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

             
           /* GameObject findobjStateControl= other.GetComponent<Collider>().gameObject;
            while (!findobjStateControl.gameObject.GetComponent<objStateControl>())//���W�M�䪽�즳objStateControl
            {      
                if (findobjStateControl.transform.parent == null)//��ܳo�Ӫ���S����q�o���ݩ�
                    break;
                findobjStateControl = findobjStateControl.transform.parent.gameObject;
            }

            if (findobjStateControl.GetComponent<objStateControl>())
            {
                findobjStateControl.GetComponent<objStateControl>().OnDamage(damage);
				if(burn)
				findobjStateControl.GetComponent<objStateControl> ().startBurn ();
            }*/



            // Unparent the particles from the shell.
            // m_ExplosionParticles.transform.parent = null;

            ParticleSystem exp = ParticleSystem.Instantiate (m_ExplosionParticles, transform.position, transform.rotation);
			exp.Play ();
			ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
			Destroy (exp.gameObject, mainModule.duration/4);
            // Play the particle system.
			AudioSource expa=AudioSource.Instantiate(m_ExplosionAudio,transform.position, transform.rotation);
			expa.PlayOneShot (expa.clip);
			Destroy (expa.gameObject, 1f);
            // Play the explosion sound effect.
           // m_ExplosionAudio.Play();

            // Once the particles have finished, destroy the gameobject they are on.
            //ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
			//Destroy (m_ExplosionParticles.gameObject, mainModule.duration);

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