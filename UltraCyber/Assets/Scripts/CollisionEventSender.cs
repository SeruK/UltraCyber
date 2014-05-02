using UnityEngine;
using System.Collections;

public class CollisionEventSender : MonoBehaviour
{
	public delegate void TriggerEventHandler(CollisionEventSender in_trigger, Collider in_other);
	public delegate void TriggerEventHandler2D(CollisionEventSender in_trigger, Collider2D in_other);
	public delegate void CollisionEventHandler(CollisionEventSender in_collider, Collision in_collision);
	public delegate void CollisionEventHandler2D(CollisionEventSender in_collider, Collision2D in_collision);
	public delegate void CharacterControllerEventHandler(CollisionEventSender in_characterController,
	                                                     ControllerColliderHit in_hit);

	public event TriggerEventHandler   TriggerEnter;
	public event TriggerEventHandler2D TriggerEnter2D;
	public event TriggerEventHandler   TriggerStay;
	public event TriggerEventHandler2D TriggerStay2D;
	public event TriggerEventHandler   TriggerExit;
	public event TriggerEventHandler2D TriggerExit2D;

	public event CollisionEventHandler   CollisionEnter;
	public event CollisionEventHandler2D CollisionEnter2D;
	public event CollisionEventHandler   CollisionStay;
	public event CollisionEventHandler2D CollisionStay2D;
	public event CollisionEventHandler   CollisionExit;
	public event CollisionEventHandler2D CollisionExit2D;

	public event CharacterControllerEventHandler ControllerHit;

	/*********************************************/

	private void OnTriggerEnter(Collider in_other)
	{
		if (TriggerEnter != null) TriggerEnter(this, in_other);
	}

	private void OnTriggerEnter2D(Collider2D in_other)
	{
		if (TriggerEnter2D != null) TriggerEnter2D(this, in_other);
	}

	private void OnTriggerStay(Collider in_other)
	{
		if (TriggerStay != null) TriggerStay(this, in_other);
	}

	private void OnTriggerStay2D(Collider2D in_other)
	{
		if (TriggerStay2D != null) TriggerStay2D(this, in_other);
	}

	private void OnTriggerExit(Collider in_other)
	{
		if (TriggerExit != null) TriggerExit(this, in_other);
	}

	private void OnTriggerExit2D(Collider2D in_other)
	{
		if (TriggerExit2D != null) TriggerExit2D(this, in_other);
	}

	/*********************************************/

	private void OnCollisionEnter(Collision in_collision)
	{
		if (CollisionEnter != null) CollisionEnter(this, in_collision);
	}

	private void OnCollisionEnter(Collision2D in_collision)
	{
		if (CollisionEnter2D != null) CollisionEnter2D(this, in_collision);
	}

	private void OnCollisionStay(Collision in_collision)
	{
		if (CollisionStay != null) CollisionStay(this, in_collision);
	}

	private void OnCollisionStay2D(Collision2D in_collision)
	{
		if (CollisionStay2D != null) CollisionStay2D(this, in_collision);
	}

	private void OnCollisionExit(Collision in_collision)
	{
		if (CollisionExit != null) CollisionExit(this, in_collision);
	}

	private void OnCollisionExit2D(Collision2D in_collision)
	{
		if (CollisionExit2D != null) CollisionExit2D(this, in_collision);
	}

	/*********************************************/

	private void OnControllerColliderHit(ControllerColliderHit in_hit)
	{
		if (ControllerHit != null) ControllerHit(this, in_hit);
	}
}
