﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public ParticleSystem leftExhaust;
	public ParticleSystem rightExhaust;
	public Text speedometer;
	public float maxRoll = 5;
	public float maxPitch = 5;

	private Rigidbody body;
	private ConstantForce force;
	private Vector3 forceVector = Vector3.zero;
	private float throttle;

	public float Throttle {
		get 
		{
			return throttle;
		}
		set
		{
			throttle = value;
			if (leftExhaust) {
				leftExhaust.startSpeed = value / 6f;
				leftExhaust.emissionRate = value;
			}
			if (rightExhaust) {
				rightExhaust.startSpeed = value / 6f;
				rightExhaust.emissionRate = value;
			}
			forceVector.x = throttle;
			force.relativeForce = forceVector;
		}
	}
		
	void Start () {
		body = GetComponent<Rigidbody>();
		force = GetComponent<ConstantForce> ();
	}

	void FixedUpdate() {
		speedometer.text = "" + (int) body.velocity.magnitude + ", " + body.position;

		// Keyboard

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Throttle = 10;
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Throttle = 20;
		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Throttle = 30;
		} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Throttle = 40;
		} else if (Input.GetKeyDown(KeyCode.Alpha5)) {
			Throttle = 50;
		} else if (Input.GetKeyDown(KeyCode.Alpha6)) {
			Throttle = 60;
		} else if (Input.GetKeyDown(KeyCode.Alpha7)) {
			Throttle = 70;
		} else if (Input.GetKeyDown(KeyCode.Alpha8)) {
			Throttle = 80;
		} else if (Input.GetKeyDown(KeyCode.Alpha9)) {
			Throttle = 90;
		} else if (Input.GetKeyDown (KeyCode.Alpha0)) {
			Throttle = 100;
		}

		Quaternion rotationDelta = Quaternion.identity;
		if (Input.GetKey (KeyCode.S)) {
			rotationDelta = rotationDelta * Quaternion.Euler (0, 0, maxPitch);
		} else if (Input.GetKey (KeyCode.W)) {
			rotationDelta = rotationDelta * Quaternion.Euler (0, 0, -maxPitch);
		}
		if (Input.GetKey (KeyCode.A)) {
			rotationDelta = rotationDelta * Quaternion.Euler (maxRoll, 0, 0);
		} else if (Input.GetKey (KeyCode.D)) {
			rotationDelta = rotationDelta * Quaternion.Euler (-maxRoll, 0, 0);
		}

		// Touch

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Moved && touch.position.x < Screen.width / 8) {
				Throttle = 100f * ((float)touch.position.y) / ((float)Screen.height);
			}
		}

		// Accelerometer
		
		if (Input.acceleration.magnitude > 0f) {
			if (Mathf.Abs (Input.acceleration.x) > 0.1) {
				rotationDelta = rotationDelta * Quaternion.Euler (-maxRoll * Input.acceleration.x, 0, 0);
			}
			float adjustedPitch = Mathf.Clamp (Input.acceleration.z + 0.5f, -1f, 1f);
			if (Mathf.Abs (adjustedPitch) > 0.1) {
				rotationDelta = rotationDelta * Quaternion.Euler (0, 0, maxPitch * adjustedPitch);
			}
		}

		transform.rotation = transform.rotation * rotationDelta;



	}

}
