using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonFX : MonoBehaviour {
    
	public AudioSource myFx;
	public AudioClip hoverFx;
	public AudioClip clickFx;
	public AudioClip moveFx;

	public void HoverSOund() {
		myFx.PlayOneShot(hoverFx);
	}

	public void ClickSound() {
		myFx.PlayOneShot(clickFx);
	}

	public void MoveSound() {
		myFx.PlayOneShot(moveFx);
	}

}
