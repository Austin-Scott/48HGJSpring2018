using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTextController : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        string health;
        health = "Player health: " + Board.player.health + "\nEnemy health: " + GameController.currentBoard.enemy.health + "\n";
        text.text = health;
	}
}
