using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightButton : MonoBehaviour {

	void OnMouseOver() {
		if (Input.GetMouseButtonDown(0)) {
			GameController.currentBoard.FightButtonPressed();
		}
	}
}
