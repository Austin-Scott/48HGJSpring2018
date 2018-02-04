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
        cardDictionary[typeof(CardSlash)] = new CardSlash();
        cardDictionary[typeof(CardPsychUp)] = new CardPsychUp();
        cardDictionary[typeof(CardStab)] = new CardStab();
        cardDictionary[typeof(CardPhysicallyFit)] = new CardPhysicallyFit();
        cardDictionary[typeof(CardHalberdStrike)] = new CardHalberdStrike();
        cardDictionary[typeof(CardPunch)] = new CardPunch();
        cardDictionary[typeof(CardSteroids)] = new CardSteroids();
        cardDictionary[typeof(CardTackle)] = new CardTackle();
        cardDictionary[typeof(CardPistolShot)] = new CardPistolShot();
        cardDictionary[typeof(CardBuckshot)] = new CardBuckshot();
        cardDictionary[typeof(CardLightningBolt)] = new CardLightningBolt();
        cardDictionary[typeof(CardRifleShot)] = new CardRifleShot();
        cardDictionary[typeof(CardReload)] = new CardReload();
        cardDictionary[typeof(CardShowOff)] = new CardShowOff();
        cardDictionary[typeof(CardQuickFingers)] = new CardQuickFingers();
        cardDictionary[typeof(CardThrownDagger)] = new CardThrownDagger();
        cardDictionary[typeof(CardOilWeapon)] = new CardOilWeapon();
        cardDictionary[typeof(CardAim)] = new CardAim();
        cardDictionary[typeof(CardRest)] = new CardRest();
        cardDictionary[typeof(CardTowerShield)] = new CardTowerShield();
        cardDictionary[typeof(CardBandage)] = new CardBandage();
        cardDictionary[typeof(CardDoctorsBag)] = new CardDoctorsBag();
        cardDictionary[typeof(CardPlan)] = new CardPlan();
        cardDictionary[typeof(CardPrayer)] = new CardPrayer();
        cardDictionary[typeof(CardSeduce)] = new CardSeduce();
        cardDictionary[typeof(CardIntellectualism)] = new CardIntellectualism();
        cardDictionary[typeof(CardIntimidate)] = new CardIntimidate();
        cardDictionary[typeof(CardTerrify)] = new CardTerrify();
        cardDictionary[typeof(CardSouless)] = new CardSouless();
        cardDictionary[typeof(CardFranticThinking)] = new CardFranticThinking();
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
