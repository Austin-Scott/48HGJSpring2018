using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {

	public abstract IEnumerator Play();
	public abstract IEnumerator Use();

	public virtual IEnumerator Hover() {
		//TODO move card closer to player infront of all other game elements.
	}

	public virtual IEnumerator Destroy() {
		
	}
	
}
