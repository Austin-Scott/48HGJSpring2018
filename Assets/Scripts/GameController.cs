using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	Dictionary<System.Type, Card> cardDictionary = new Dictionary<System.Type, Card>();

	/// Prefab for a board
	Board boardPrefab;
	/// Prefab for a character
	Character characterPrefab;

	Board currentBoard;

	/// Load resources
	void Awake() {
		gameController = this;
		cardDictionary.Add(typeof(CardSlash), Resources.Load("Cards/CardSlash", typeof(CardSlash)) as CardSlash); 
		boardPrefab = Resources.Load("Board", typeof(Board)) as Board;
		characterPrefab = Resources.Load("Character", typeof(Character)) as Character;
		Shield.SetShieldPrefab(Resources.Load("Shield", typeof(Shield)) as Shield);
	}

	/// Initialize the game
	void Start() {
		currentBoard = Instantiate(boardPrefab);
		StartCoroutine(currentBoard.Initialize(Instantiate(characterPrefab), Instantiate(characterPrefab)));

        //Load all cards into game
        CreateCard(typeof(CardSlash));
        CreateCard(typeof(CardPsychUp));
        CreateCard(typeof(CardStab));
        CreateCard(typeof(CardPhysicallyFit));
        CreateCard(typeof(CardHalberdStrike));
        CreateCard(typeof(CardPunch));
        CreateCard(typeof(CardSteroids));
        CreateCard(typeof(CardTackle));
        CreateCard(typeof(CardPistolShot));
        CreateCard(typeof(CardBuckshot));
        CreateCard(typeof(CardLightningBolt));
        CreateCard(typeof(CardRifleShot));
        CreateCard(typeof(CardReload));
        CreateCard(typeof(CardShowOff));
        CreateCard(typeof(CardQuickFingers));
        CreateCard(typeof(CardThrownDagger));
        CreateCard(typeof(CardOilWeapon));
        CreateCard(typeof(CardAim));
        CreateCard(typeof(CardRest));
        CreateCard(typeof(CardTowerShield));
        CreateCard(typeof(CardBandage));
        CreateCard(typeof(CardDoctorsBag));
        CreateCard(typeof(CardPlan));
        CreateCard(typeof(CardPrayer));
        CreateCard(typeof(CardSeduce));
        CreateCard(typeof(CardIntellectualism));
        CreateCard(typeof(CardIntimidate));
        CreateCard(typeof(CardTerrify));
        CreateCard(typeof(CardSouless));
        CreateCard(typeof(CardFranticThinking));
        //TODO: Add the remaining cards when they are properly implemented


    }

	/// Creates a card of type
	public static Card CreateCard(System.Type cardType) {
		return Instantiate(gameController.cardDictionary[cardType]);
	}

	static GameController gameController;

	public static Coroutine ControllerCoroutine(IEnumerator routine) {
		return gameController.StartCoroutine(routine);
	}
}
