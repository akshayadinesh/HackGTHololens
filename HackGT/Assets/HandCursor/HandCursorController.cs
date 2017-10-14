using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

[RequireComponent(typeof(Animator))]
public class HandCursorController : MonoBehaviour {
	Animator animator;

	public GameObject myo = null;
	private Pose _lastPose = Pose.Unknown;
	int idle, point, open, gesture, fist, pick, grab;

	void Awake () {
		animator = GetComponent<Animator>();
		point = Animator.StringToHash("Point");
		idle = Animator.StringToHash("Idle");
		gesture = Animator.StringToHash("Gesture");
		open = Animator.StringToHash("Open");
		fist = Animator.StringToHash("Fist");
		pick = Animator.StringToHash("Pick");
		grab = Animator.StringToHash("Grab");
	}

	void Update() {
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;

			// Vibrate the Myo armband when a fist is made.
			if (thalmicMyo.pose == Pose.Fist) {
				thalmicMyo.Vibrate (VibrationType.Medium);
				Fist ();
				ExtendUnlockAndNotifyUserAction (thalmicMyo);

				// Change material when wave in, wave out or double tap poses are made.
			} else if (thalmicMyo.pose == Pose.WaveIn) {
				
				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.WaveOut) {

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.DoubleTap) {
				Pick ();
				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.FingersSpread) {
				
				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.Rest) {
				Idle ();
				ExtendUnlockAndNotifyUserAction (thalmicMyo);

			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			Idle ();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			Point ();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			Gesture ();
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			Open ();
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			Fist ();
		}
		if(Input.GetKeyDown(KeyCode.Alpha6)) {
			Pick ();
		}
		if(Input.GetKeyDown(KeyCode.Alpha7)) {
			Grab ();
		}
	}

	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
	{
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}

		myo.NotifyUserAction ();
	}

	[ContextMenu("Idle")]
	public void Idle() {
		animator.SetTrigger(idle);
	}

	[ContextMenu("Point")]
	public void Point() {
		animator.SetTrigger(point);
	}

	[ContextMenu("Gesture")]
	public void Gesture() {
		animator.SetTrigger(gesture);
	}

	[ContextMenu("Open")]
	public void Open() {
		animator.SetTrigger(open);
	}

	[ContextMenu("Fist")]
	public void Fist() {
		animator.SetTrigger(fist);
	}

	[ContextMenu("Pick")]
	public void Pick() {
		animator.SetTrigger(pick);
	}

	[ContextMenu("Grab")]
	public void Grab() {
		animator.SetTrigger(grab);
	}

	public void WaveRight() {

	}

	public void WaveLeft() {

	}

}
