using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour {

    public float delay = 3f;

    ParticleSystem emitter;

	// Use this for initialization
	void Start () {
        emitter = GetComponent<ParticleSystem>();
	}

    public IEnumerable Explode()
    {
        emitter.Play();
        yield return new WaitForSeconds(delay);
        emitter.Stop();
        yield break;
    }
	
	// Update is called once per frame
	void Update () {

	}
}
