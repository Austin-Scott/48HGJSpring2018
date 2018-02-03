using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CardType {
	Melee,
	Ranged,
	Defense,
	White,
	Black
};

public abstract class Card : MonoBehaviour {

	public abstract IEnumerator Use();

	/// Number of phases a card executes over.
	protected int cost;

	/// Type of the card (not implemented)
	CardType cardType;

	/// Text mesh of the title.
	[SerializeField]
	protected TextMesh titleText;
	/// Text mesh of the cost display.
	[SerializeField]
	protected TextMesh costText;
	/// Text mesh of the text.
	[SerializeField]
	protected TextMesh text;

	/// The character using the card
	protected Character holder { get; private set; }

	/// The character opposite the holder. Target
	protected Character target { get; private set; }

	/// Position of the card on the board before it is moved for hover animation.
	Vector3 positionBeforeHover;

	/// The current hover or dehover routine.	
	Coroutine hoverCoroutine;

	/// True if the card is currently moving.
	bool moving = false;

	/// Although dehovering is considered moving, it can be interrupted be re-hovering, so it needs to be stored.
	bool deHovering = false;

	/// True while the player is dragging a card
	bool grabbing = false;

	public virtual IEnumerator Hover() {
		if (moving && !deHovering) {
			yield break;
		}
		if (hoverCoroutine != null) {
			transform.position = positionBeforeHover;
		} else {
			positionBeforeHover = transform.position;
		}
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			deHovering = false;
		}
		hoverCoroutine = StartCoroutine(LerpPosition(transform.position + Vector3.up * 5f, 2f));
		yield return hoverCoroutine;
	}

	public virtual IEnumerator DeHover() {
		deHovering = true;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
		}
		hoverCoroutine = StartCoroutine(LerpPosition(positionBeforeHover, 2f));
		yield return hoverCoroutine;
		deHovering = false;
	}

	public void DestroyAtEndOfTurn() {
		Board.endTurn += StartDestroyAnimation;
	}

	public virtual void StartDestroyAnimation() {
		StartCoroutine(Destroy());
	}

	public virtual IEnumerator Destroy() {
		Destroy(gameObject);
		yield break; //TODO destruction animation	
	}

	/// Slerps the card's transofrm to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Transform desiredTransform, float speed = 1f) {
		moving = true;
		while (transform != desiredTransform) {
			float deltaTime = Time.deltaTime * 5f * speed;
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredTransform.rotation, deltaTime);
			transform.position = Vector3.Slerp(transform.position, desiredTransform.position, deltaTime);
			transform.localScale = Vector3.Slerp(transform.localScale, desiredTransform.localScale, deltaTime);
			if (Quaternion.Angle(transform.rotation, desiredTransform.rotation) < 5f) {
				transform.rotation = desiredTransform.rotation;
			}
			if (Vector3.Distance(transform.position, desiredTransform.position) < 0.1f) {
				transform.position = desiredTransform.position;
			}
			//TODO scale almost finished scaling check (if we decide to use it)
			yield return null;
		}
		moving = false;
	}

	/// Moves the card to a certain position over time. Speed of 1 is default.
	private IEnumerator LerpPosition (Vector3 desiredPosition, float speed = 1f) {
		moving = true;
		while (transform.position != desiredPosition) {
			float deltaTime = Time.deltaTime * 5 * speed;
			transform.position = Vector3.Slerp(transform.position, desiredPosition, deltaTime);
			if (Vector3.Distance(transform.position, desiredPosition) < 0.1f) {
				transform.position = desiredPosition;
			}
			yield return null;
		}
		moving = false;
	}

	/// Smoothly move the card to desired position.
	public IEnumerator SmoothMove (Vector3 desiredPosition, float speed = 1f) {
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			transform.position = positionBeforeHover;
		}
		yield return StartCoroutine(LerpPosition(desiredPosition, speed));
	}

	/// Smoothly transform the card to the desired position.
	public IEnumerator SmoothTransform (Transform desiredTransform, float speed = 1f) {
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			transform.position = positionBeforeHover;
		}
		yield return StartCoroutine(LerpTransform(desiredTransform, speed));
	}

	protected virtual void Awake() {

	}

	protected virtual void Start() {

	}

	protected virtual void Update() {
		if (grabbing && Input.GetMouseButtonUp(0)){
			grabbing = false;
		}
		if (grabbing) {
			// this creates a horizontal plane passing through this object's center
			Plane plane = new Plane(new Vector3(0f, 1f, 0f), Vector3.up);
			// create a ray from the mousePosition
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// plane.Raycast returns the distance from the ray start to the hit point
			float distance;
			if (plane.Raycast(ray, out distance)){
				// some point of the plane was hit - get its coordinates
				Vector3 hitPoint = ray.GetPoint(distance);
				// use the hitPoint to position the card.
				transform.position = hitPoint;
			}
		}
	}

	protected virtual void OnMouseOver() {
		if (Input.GetMouseButtonDown(0)) {
			if (hoverCoroutine != null) {
				StopCoroutine(hoverCoroutine);
			}
			grabbing = true;
		}
	}

	protected virtual void OnMouseEnter() {
		if (!grabbing) {
			StartCoroutine(Hover());
		}
	}

	protected virtual void OnMouseExit() {
		if (!grabbing) {
			StartCoroutine(DeHover());
		}
	}

	/// Sets the cards friendy and target properties.
	public virtual void Initialize(Character holder, Character target) {
		this.holder = holder;
		this.target = target;
	}
}
