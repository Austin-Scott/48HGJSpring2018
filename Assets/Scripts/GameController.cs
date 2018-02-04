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
        //cardDictionary[typeof(CardSlash)] = Instantiate(CardSlash);
        InitializeCard(typeof(CardSlash));
        InitializeCard(typeof(CardPsychUp));
        InitializeCard(typeof(CardStab));
        InitializeCard(typeof(CardPhysicallyFit));
        InitializeCard(typeof(CardHalberdStrike));
        InitializeCard(typeof(CardPunch));
        InitializeCard(typeof(CardSteroids));
        InitializeCard(typeof(CardTackle));
        InitializeCard(typeof(CardPistolShot));
        InitializeCard(typeof(CardBuckshot));
        InitializeCard(typeof(CardLightningBolt));
        InitializeCard(typeof(CardRifleShot));
        InitializeCard(typeof(CardReload));
        InitializeCard(typeof(CardShowOff));
        InitializeCard(typeof(CardQuickFingers));
        InitializeCard(typeof(CardThrownDagger));
        InitializeCard(typeof(CardOilWeapon));
        InitializeCard(typeof(CardAim));
        InitializeCard(typeof(CardRest));
        InitializeCard(typeof(CardTowerShield));
        InitializeCard(typeof(CardBandage));
        InitializeCard(typeof(CardDoctorsBag));
        InitializeCard(typeof(CardPlan));
        InitializeCard(typeof(CardPrayer));
        InitializeCard(typeof(CardSeduce));
        InitializeCard(typeof(CardIntellectualism));
        InitializeCard(typeof(CardIntimidate));
        InitializeCard(typeof(CardTerrify));
        InitializeCard(typeof(CardSouless));
        InitializeCard(typeof(CardFranticThinking));
        //TODO: Add the remaining cards when they are properly implemented


    }

	/// Creates a card of type
	public static Card CreateCard(System.Type cardType) {
		return Instantiate(gameController.cardDictionary[cardType]);
	}

    public static void InitializeCard(System.Type cardType)
    {
        //TODO: fix bug here
        gameController.cardDictionary[cardType] = new typeof(cardType)();
    }

	static GameController gameController;

	public static Coroutine ControllerCoroutine(IEnumerator routine) {
		return gameController.StartCoroutine(routine);
	}
}
