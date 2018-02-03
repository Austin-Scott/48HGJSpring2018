using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSlot : MonoBehaviour {

	[SerializeField]
	Board board;

	[SerializeField]
	int globalPhaseIndex;

	void OnTriggerStay(Collider other) {
		if (globalPhaseIndex > 2) {
			return;
		}
		if (Input.GetMouseButtonUp(0)) {
			Card card = other.GetComponent<Card>();
			if (card != null) {
				if (!card.onBoard) {
					StartCoroutine(card.holder.PlaceCard(card, globalPhaseIndex));
				}
			}
		}
	}
}
