using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Basic card characteristics
public abstract class Card : MonoBehaviour {

	/// Called when a card is used. NOT when a card is played on the board.
    public abstract IEnumerator Use();

	/// Number of phases a card executes over.
	[SerializeField]
	protected int cost;
	public int GetCost() { return cost; }

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

	/// Position of the card on the board before it is moved for hover animation.
	Vector3 previousPosition;
	/// Rotation of the card on the board before it is moved by a hover animation.
	Quaternion previousRotation;

	/// The current movement coroutine	
	Coroutine currentMovementCoroutine;

	/// True when the card cannot be played or viewed.
	bool busy;

	/// True while the player is dragging a card
	bool grabbing = false;

	/// The phase index the card is placed into.
	public int phaseIndex = -1;

	/// Hovers the card so the player can see it better.
	private virtual IEnumerator Hover() {
		if (currentMovementCoroutine != null) {
			transform.position = previousPosition;
			transform.rotation = previousRotation;
		}
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			// deHovering = false;
		}
		Vector3 desiredPosition = transform.position + Vector3.up * 2f;
		hoverCoroutine = StartCoroutine(LerpTransform(desiredPosition, Quaternion.identity, 2f));
		yield return hoverCoroutine;
	}

	/// Dehovers the card.
	private virtual IEnumerator DeHover() {
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

	/// Slerps the card's transform to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Transform desiredTransform, bool interuptable = false, float speed = 1f) {
		yield return StartCoroutine(LerpTransform(desiredTransform.position, desiredTransform.rotation, interuptable, speed))
	}

	/// Slerps the card's transofrm to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Vector3 desiredPosition, Quaternion desiredRotation, bool interuptable, float speed = 1f) {
		busy = !interuptable;
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
		busy = false;
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
