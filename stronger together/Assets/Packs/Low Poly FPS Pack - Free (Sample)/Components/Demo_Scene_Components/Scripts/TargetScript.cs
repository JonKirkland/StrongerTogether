using UnityEngine;
using System.Collections;

// ----- Low Poly FPS Pack Free Version -----
public class TargetScript : MonoBehaviour {



	//Used to check if the target has been hit
	public bool isHit = false;

	[Header("Customizable Options")]
	//Minimum time before the target goes back up
	public float minTime;
	//Maximum time before the target goes back up
	public float maxTime;

	[Header("Audio")]
	public AudioClip upSound;
	public AudioClip downSound;

	public AudioSource audioSource;
	
	private void Update () {
		
		//Generate random time based on min and max time values
		

		//If the target is hit
		//if (isHit == true) 
		//{
			//STEnemyHealth enemyHealth = gameObject.GetComponent<STEnemyHealth>();
			//if (enemyHealth != null)
			//{
				//enemyHealth.TakeDamage(5f);
				//Animate the target hit
				//gameObject.GetComponent<Animation> ().Play("target_down");

				//Set the downSound as current sound, and play it
				//audioSource.GetComponent<AudioSource>().clip = downSound;
				//audioSource.Play();

				//Start the timer

			//} 
		//}
	}
    public void EnemyHit()
    {
		STEnemyHealth enemyHealth = gameObject.GetComponent<STEnemyHealth>();
		enemyHealth.TakeDamage(5f);

	}
	public void HeadHit()
	{
		STEnemyHealth enemyHealth = gameObject.GetComponent<STEnemyHealth>();
		enemyHealth.TakeDamage(15f);

	}
	public void PlayerHit()
    {
		STPlayerInfo playerHealth = gameObject.GetComponent<STPlayerInfo>();
		playerHealth.Hurt(5);
	}
	
}
// ----- Low Poly FPS Pack Free Version -----