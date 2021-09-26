using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private bool isFacingRight;

	private Rigidbody2D rb;
	private Animator animator;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		float moveInput = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

		if (moveInput < 0) TurnLeft();
		if (moveInput > 0) TurnRight();

		animator.SetBool("isWalking", moveInput != 0);
	}

	private void TurnLeft()
	{
		if (isFacingRight == true) Flip();
	}

	private void TurnRight()
	{
		if (isFacingRight == false) Flip();
	}

	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector2 Scaler = transform.localScale;
		Scaler.x *= -1;
		transform.localScale = Scaler;
	}
}
