using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AutoDestroy : MonoBehaviour {

	private System.Action callback;

	public void Init(float timeToLive, System.Action callback=null) {
		Invoke ("Destroy",timeToLive);

		this.callback = callback;

	}

	void Destroy() {
		GameObject.Destroy(gameObject);

		if(callback != null) {
			callback();
		}
		
	}
}

public class EffectSpawner : AutoDestroy {

	private static EffectSpawner instance = null;

	private EffectSpawner() {
	}

	public static EffectSpawner Instance {
		get{
			if (instance == null)
				instance = Game.FindObjectOfType(typeof(EffectSpawner)) as EffectSpawner;
			
			return instance;

		}
	}

	public GameObject ExplosionEffect;



	private List<GameObject> effects;
	// Use this for initialization
	void Start () {
	}


	public void SpawnExplosion(Vector2 position) {
		GameObject animator = GameObject.Instantiate(ExplosionEffect) as GameObject;
		animator.transform.position = position;
		animator.GetComponent<Animator>().Play("ExplosionIdle");
		animator.gameObject.AddComponent<AutoDestroy>().Init (0.2f);
	}

	void Update () {
	
	}
}
