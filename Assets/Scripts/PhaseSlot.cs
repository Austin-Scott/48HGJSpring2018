using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSlot : MonoBehaviour {

	/// Board the phase slot is on
	[SerializeField]
	Board board;

	/// The phase index of the phase slot (0-5). Not relative to a character.
	[SerializeField]
	int globalPhaseIndex;

	/// Called when a card is dragged into the phase.
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
