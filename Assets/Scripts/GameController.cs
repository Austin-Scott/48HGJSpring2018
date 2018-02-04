using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	Dictionary<System.Type, Card> cardDictionary = new Dictionary<System.Type, Card>();

	/// Prefab for a board
	Board boardPrefab;
	/// Prefab for a character
	Character characterPrefab;

	public static Board currentBoard;

    /// The current index of the current enemy
    public static int currentEnemyIndex = 0;

    [SerializeField]
    Font cardFont;
    [SerializeField]
    Material fontMaterial;
    [SerializeField]
    int titleSize;
    [SerializeField]
    int costSize;
    [SerializeField]
    int textSize;

	/// Load resources
	void Awake() {
        gameController = this;

        //Load all cards into game
        cardDictionary.Add(typeof(CardSlash), Resources.Load("Cards/CardSlash", typeof(CardSlash)) as CardSlash);
        cardDictionary.Add(typeof(CardPsychUp), Resources.Load("Cards/CardPsychUp", typeof(CardPsychUp)) as CardPsychUp);
        cardDictionary.Add(typeof(CardStab), Resources.Load("Cards/CardStab", typeof(CardStab)) as CardStab);
        cardDictionary.Add(typeof(CardPhysicallyFit), Resources.Load("Cards/CardPhysicallyFit", typeof(CardPhysicallyFit)) as CardPhysicallyFit);
        cardDictionary.Add(typeof(CardHalberdStrike), Resources.Load("Cards/CardHalberdStrike", typeof(CardHalberdStrike)) as CardHalberdStrike);
        cardDictionary.Add(typeof(CardPunch), Resources.Load("Cards/CardPunch", typeof(CardPunch)) as CardPunch);
        cardDictionary.Add(typeof(CardSteroids), Resources.Load("Cards/CardSteroids", typeof(CardSteroids)) as CardSteroids);
        cardDictionary.Add(typeof(CardTackle), Resources.Load("Cards/CardTackle", typeof(CardTackle)) as CardTackle);
        cardDictionary.Add(typeof(CardPistolShot), Resources.Load("Cards/CardPistolShot", typeof(CardPistolShot)) as CardPistolShot);
        cardDictionary.Add(typeof(CardBuckshot), Resources.Load("Cards/CardBuckshot", typeof(CardBuckshot)) as CardBuckshot);
        cardDictionary.Add(typeof(CardLightningBolt), Resources.Load("Cards/CardLightningBolt", typeof(CardLightningBolt)) as CardLightningBolt);
        cardDictionary.Add(typeof(CardRifleShot), Resources.Load("Cards/CardRifleShot", typeof(CardRifleShot)) as CardRifleShot);
        cardDictionary.Add(typeof(CardReload), Resources.Load("Cards/CardReload", typeof(CardReload)) as CardReload);
        cardDictionary.Add(typeof(CardShowOff), Resources.Load("Cards/CardShowOff", typeof(CardShowOff)) as CardShowOff);
        cardDictionary.Add(typeof(CardQuickFingers), Resources.Load("Cards/CardQuickFingers", typeof(CardQuickFingers)) as CardQuickFingers);
        cardDictionary.Add(typeof(CardThrownDagger), Resources.Load("Cards/CardThrownDagger", typeof(CardThrownDagger)) as CardThrownDagger);
        cardDictionary.Add(typeof(CardOilWeapon), Resources.Load("Cards/CardOilWeapon", typeof(CardOilWeapon)) as CardOilWeapon);
        cardDictionary.Add(typeof(CardAim), Resources.Load("Cards/CardAim", typeof(CardAim)) as CardAim);
        cardDictionary.Add(typeof(CardRest), Resources.Load("Cards/CardRest", typeof(CardRest)) as CardRest);
        cardDictionary.Add(typeof(CardTowerShield), Resources.Load("Cards/CardTowerShield", typeof(CardTowerShield)) as CardTowerShield);
        cardDictionary.Add(typeof(CardBandage), Resources.Load("Cards/CardBandage", typeof(CardBandage)) as CardBandage);
        cardDictionary.Add(typeof(CardDoctorsBag), Resources.Load("Cards/CardDoctorsBag", typeof(CardDoctorsBag)) as CardDoctorsBag);
        cardDictionary.Add(typeof(CardPlan), Resources.Load("Cards/CardPlan", typeof(CardPlan)) as CardPlan);
        cardDictionary.Add(typeof(CardPrayer), Resources.Load("Cards/CardPrayer", typeof(CardPrayer)) as CardPrayer);
        cardDictionary.Add(typeof(CardSeduce), Resources.Load("Cards/CardSeduce", typeof(CardSeduce)) as CardSeduce);
        cardDictionary.Add(typeof(CardIntellectualism), Resources.Load("Cards/CardIntellectualism", typeof(CardIntellectualism)) as CardIntellectualism);
        cardDictionary.Add(typeof(CardIntimidate), Resources.Load("Cards/CardIntimidate", typeof(CardIntimidate)) as CardIntimidate);
        cardDictionary.Add(typeof(CardTerrify), Resources.Load("Cards/CardTerrify", typeof(CardTerrify)) as CardTerrify);
        cardDictionary.Add(typeof(CardFranticThinking), Resources.Load("Cards/CardFranticThinking", typeof(CardFranticThinking)) as CardFranticThinking);
        cardDictionary.Add(typeof(CardFeign), Resources.Load("Cards/CardFeign", typeof(CardFeign)) as CardFeign);
        //TODO: Add the remaining cards when they are properly implemented

        foreach (KeyValuePair<System.Type, Card> card in cardDictionary) {
            card.Value.GetAllComponents();
            card.Value.titleText.font = cardFont;
            card.Value.costText.font = cardFont;
            card.Value.text.font = cardFont;
            card.Value.titleText.fontSize = titleSize;
            card.Value.costText.fontSize = costSize;
            card.Value.text.fontSize = textSize;
            card.Value.titleText.GetComponent<MeshRenderer>().sharedMaterial = fontMaterial;
            card.Value.costText.GetComponent<MeshRenderer>().sharedMaterial = fontMaterial;
            card.Value.text.GetComponent<MeshRenderer>().sharedMaterial = fontMaterial;
            Vector3 scale = new Vector3(0.05f, 0.05f, 1f);
            card.Value.titleText.transform.localScale = scale;
            card.Value.costText.transform.localScale = scale;
            card.Value.text.transform.localScale = scale;
            card.Value.costText.text = card.Value.GetCost().ToString();
        }

        boardPrefab = Resources.Load("Board", typeof(Board)) as Board;
		characterPrefab = Resources.Load("Character", typeof(Character)) as Character;
		Shield.SetShieldPrefab(Resources.Load("Shield", typeof(Shield)) as Shield);
	}

	/// Initialize the game
	void Start() {
        StartCoroutine(StartGame());
    }

	/// Creates a card of type
	public static Card CreateCard(System.Type cardType) {
		return Instantiate(gameController.cardDictionary[cardType]);
	}

    static GameController gameController;

	public static Coroutine ControllerCoroutine(IEnumerator routine) {
		return gameController.StartCoroutine(routine);
	}

    public IEnumerator StartGame() {
        currentBoard = Instantiate(boardPrefab);
        // Give player basic deck

        yield return StartCoroutine(TextboxController.ShowText("Welcom to the game"));

        //Stacks all the cards into their character's deck
		yield return StartCoroutine(currentBoard.Initialize(Instantiate(characterPrefab), CreateEnemy()));
    }

    public void RestartGame() {
        StopAllCoroutines();
        currentBoard.DeleteAll();
        StartCoroutine(StartGame());
    }

    public static void RestartGameStaticMethod() {
        gameController.RestartGame();
    }

    public static Character CreateEnemy() {
        Character enemy = Instantiate(gameController.characterPrefab);
        // enemy.CreateEnemyDeck(currentEnemyIndex);
        return enemy;
    }
}
