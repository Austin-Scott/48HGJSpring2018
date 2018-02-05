using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum CardType {
// 	Melee,
// 	Ranged,
// 	Defense,
// 	White,
// 	Black
// };

/// Basic card characteristics
public abstract class Card : MonoBehaviour {

    //Particle effects
    ParticleSystemController ParticleController;

    public IEnumerator showParticles()
    {
        if(ParticleController!=null)
        {
             yield return ParticleController.Explode();
        }
        yield break;
    }

	/// Called when a card is used. NOT when a card is played on the board.
    public abstract IEnumerator Use();

	/// Number of phases a card executes over.
	[SerializeField]
	protected int cost;
	public int GetCost() { return cost; }

	/// True if the card is on the board
	public bool onBoard = false;

	/// True if the card is in the deck
	public bool inDeck = true;

	/// Type of the card (not implemented)
	CardType cardType;

	/// Text mesh of the title.
	public TextMesh titleText;
	/// Text mesh of the cost display.
	public TextMesh costText;
	/// Text mesh of the text.
	public TextMesh text;

	/// Fetches all neccessary components for the card to use.
	public void GetAllComponents() {
		TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();
		titleText = textMeshes[0];
		costText = textMeshes[1];
		text = textMeshes[2];
	}

	/// The character using the card
	public Character holder { get; private set; }

	/// The character opposite the holder. Target
	public Character target { get; private set; }

	/// Position of the card on the board before it is moved for hover animation.
	Vector3 positionBeforeHover;
	/// Rotation of the card on the board before it is moved by a hover animation.
	Quaternion rotationBeforeHover;

	/// The current hover or dehover routine.	
	Coroutine hoverCoroutine;

	/// True if the card is currently moving.
	bool moving = false;

	/// Although hovering is considered moving, it can be interrupted be re-hovering, so it needs to be stored.
	bool hovering = false;

	/// Although dehovering is considered moving, it can be interrupted be re-hovering, so it needs to be stored.
	bool dehovering = false;

	/// True while the player is dragging a card
	bool grabbing = false;

	/// The phase index the card is placed into.
	public int phaseIndex = -1;

	/// Hovers the card so the player can see it better.
	public virtual IEnumerator Hover() {
		if (moving && !hovering) {
			yield break;
		}
		hovering = true;
		dehovering = false;
		if (hoverCoroutine != null) {
			transform.position = positionBeforeHover;
			transform.rotation = rotationBeforeHover;
		} else {
			positionBeforeHover = transform.position;
			rotationBeforeHover = transform.rotation;
		}
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			// deHovering = false;
		}
		Vector3 desiredPosition;
		if (onBoard) {
			desiredPosition = transform.position + Vector3.up * 2f;
		} else {
			desiredPosition = transform.position + new Vector3(0f, 5f, 4f);
		}
		hoverCoroutine = StartCoroutine(LerpTransform(desiredPosition, Quaternion.identity, 2f));
		yield return hoverCoroutine;
	}

	/// Dehovers the card.
	public virtual IEnumerator DeHover() {
		if (moving && !hovering) {
			yield break;
		}
		hovering = true;
		dehovering = true;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
		}
		hoverCoroutine = StartCoroutine(LerpTransform(positionBeforeHover, rotationBeforeHover, 2f));
		yield return hoverCoroutine;
		hovering = false;
		dehovering = false;
		hoverCoroutine = null;
	}

	/// Used for position a card on the board.
	public IEnumerator PositionOnBoard() {
		// Shift the card slightly based on how many other cards are in the same phase slot.
		Vector3 desiredPosition = GameController.currentBoard.phasePositions[phaseIndex].transform.position + new Vector3(0f, 0.3f, -0.8f) * (GameController.currentBoard.GetCardCount(holder, phaseIndex)-1);
		Quaternion desiredRotation = GameController.currentBoard.phasePositions[phaseIndex].transform.rotation;
		yield return StartCoroutine(SmoothTransform(desiredPosition, desiredRotation));
	}

	/// Marks a card to be destroyed at the end of the turn.
	public void DestroyAtEndOfTurn() {
		Board.endTurn += StartDestroyAnimation;
	}

	/// Start the destruction process of the card.
	public virtual void StartDestroyAnimation() {
		Board.endTurn -= StartDestroyAnimation;
		StartCoroutine(Destroy());
	}

	/// If a player card, add it to Board.destroyed cards so it can be put back into the player's deck at the end of the game. 
	/// Else, destroy it.
	public virtual IEnumerator Destroy() {
		GameController.currentBoard.RemoveCard(this, phaseIndex);
		if (holder.player) {
			Board.destroyedCards.Add(this);
			gameObject.SetActive(false);
		} else {
			Destroy(gameObject);
		}
		yield break; //TODO destruction animation	
	}

	/// Called when an character dies only.
	public void ForceDestroy() {
		Destroy(gameObject);
	}

	/// Slerps the card's transform to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Transform desiredTransform, float speed = 1f) {
		moving = true;
		while (transform.rotation != desiredTransform.rotation || transform.position != desiredTransform.position) {
			float deltaTime = Time.deltaTime * 5f * speed;
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredTransform.rotation, deltaTime);
			transform.position = Vector3.Slerp(transform.position, desiredTransform.position, deltaTime);
			transform.localScale = Vector3.Slerp(transform.localScale, desiredTransform.localScale, deltaTime);
			if (Quaternion.Angle(transform.rotation, desiredTransform.rotation) < 10f) {
				transform.rotation = desiredTransform.rotation;
			}
			if (Vector3.Distance(transform.position, desiredTransform.position) < 1f) {
				transform.position = desiredTransform.position;
			}
			// transform.localScale = desiredTransform.localScale;//TODO scale almost finished scaling check (if we decide to use it)
			yield return null;
		}
		moving = false;
	}

	/// Slerps the card's transofrm to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Vector3 desiredPosition, Quaternion desiredRotation, float speed = 1f) {
		moving = true;
		while (transform.rotation != desiredRotation || transform.position != desiredPosition) {
			float deltaTime = Time.deltaTime * 5f * speed;
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, deltaTime);
			transform.position = Vector3.Slerp(transform.position, desiredPosition, deltaTime);
			if (Quaternion.Angle(transform.rotation, desiredRotation) < 10f) {
				transform.rotation = desiredRotation;
			}
			if (Vector3.Distance(transform.position, desiredPosition) < 1f) {
				transform.position = desiredPosition;
			}
			// transform.localScale = desiredTransform.localScale;//TODO scale almost finished scaling check (if we decide to use it)
			yield return null;
		}
		moving = false;
	}

	/// Moves the card to a certain position over time. Speed of 1 is default.
	private IEnumerator LerpPosition (Vector3 desiredPosition, float speed = 1f) {
		moving = true;
		while (transform.position != desiredPosition) {
			float deltaTime = Time.deltaTime * 5f * speed;
			transform.position = Vector3.Slerp(transform.position, desiredPosition, deltaTime);
			if (Vector3.Distance(transform.position, desiredPosition) < 1f) {
				transform.position = desiredPosition;
			}
			yield return null;
		}
		moving = false;
	}

	/// Smoothly move the card to desired position.
	public IEnumerator SmoothMove (Vector3 desiredPosition, float speed = 1f) {
		hovering = false;
		dehovering = false;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			transform.position = positionBeforeHover;
		}
		yield return StartCoroutine(LerpPosition(desiredPosition, speed));
	}

	/// Smoothly transform the card to the desired position.
	public IEnumerator SmoothTransform (Transform desiredTransform, float speed = 1f) {
		hovering = false;
		dehovering = false;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			transform.position = positionBeforeHover;
			transform.rotation = rotationBeforeHover;
		}
		yield return StartCoroutine(LerpTransform(desiredTransform, speed));
	}

	/// Smoothly transform the card to the desired position.
	public IEnumerator SmoothTransform (Vector3 desiredPosition, Quaternion desiredRotation, float speed = 1f) {
		hovering = false;
		dehovering = false;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			transform.position = positionBeforeHover;
			transform.rotation = rotationBeforeHover;
		}
		yield return StartCoroutine(LerpTransform(desiredPosition, desiredRotation, speed));
	}

	protected virtual void Awake() {
        ParticleController = GetComponent<ParticleSystemController>();
	}

	protected virtual void Start() {

	}

	protected virtual void Update() {
		/// Only player's cards can be interacted with.
		if (holder != Board.player) {
			return;
		}
		/// Player lets go of card.
		if (grabbing && Input.GetMouseButtonUp(0)) {
			grabbing = false;
			GameController.currentBoard.SetPhaseCollider(false);
			//if not placed, put back in hand
			if (!onBoard) {
				StartCoroutine(holder.PositionHand());
			}
		}
		/// Player is currently holding card.
		if (grabbing) {
			// this creates a horizontal plane passing through this object's center
			Plane plane = new Plane(new Vector3(0f, 0.3f, 0f), Vector3.up);
			// create a ray from the mousePosition
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// plane.Raycast returns the distance from the ray start to the hit point
			float distance;
			// I don't think this should ever return false.
			if (plane.Raycast(ray, out distance)){
				// some point of the plane was hit - get its coordinates
				Vector3 hitPoint = ray.GetPoint(distance);
				// use the hitPoint to position the card.
				// Move card to cursor's position on a 3d plane of the board.
				transform.position = hitPoint;
				positionBeforeHover = transform.position;
				rotationBeforeHover = transform.rotation;
			}
		}
	}

	/// Called when player is mousing over a card
	protected virtual void OnMouseOver() {
		/// If card is in hand and not busy, hover it so player can see it better.
		if ((hoverCoroutine == null || dehovering) && !grabbing && !onBoard && !inDeck && holder.player == true) {
			StartCoroutine(Hover());
		}
		/// Clicked on card and card is not busy
		if (Input.GetMouseButtonDown(0) && (!moving || hovering) && !inDeck && holder.player == true && !GameController.currentBoard.running) {
			if (hoverCoroutine != null) {
				StopCoroutine(hoverCoroutine);
			}
			// Remove card from board
			if (onBoard) {
				StartCoroutine(holder.RemoveCardFromBoard(this, phaseIndex));
			// Grab card if not busy.
			} else {
				// Make sure no cards are in the middle of moving
				if (!holder.GetHandBusy()) {
					grabbing = true;
					GameController.currentBoard.SetPhaseCollider(true);
				}
			}
		}
	}

	// protected virtual void OnMouseEnter() {
	// 	if (!grabbing && !onBoard) {
	// 		StartCoroutine(Hover());
	// 	}
	// }

	/// If hovering, dehover.
	protected virtual void OnMouseExit() {
		if (!grabbing && !onBoard && !inDeck && holder.player == true) {
			StartCoroutine(DeHover());
		}
	}

	/// Sets the cards friendy and target properties.
	public virtual void Initialize(Character holder, Character target) {
		this.holder = holder;
		this.target = target;
		gameObject.SetActive(true);
	}

	/// Returns if the card is currently in an important move state that is not a hover animation.
	public bool GetBusy() {
		return (moving && !hovering && !dehovering);
	}
}
