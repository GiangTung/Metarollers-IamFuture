using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GestureRecognizer;
using System.Linq;

public class ExampleGestureHandler : MonoBehaviour {

	public Text textResult;

	public Transform referenceRoot;

	GesturePatternDraw[] references;

	void Start () {
		references = referenceRoot.GetComponentsInChildren<GesturePatternDraw> ();
	}

	void ShowAll(){
		for (int i = 0; i < references.Length; i++) {
			references [i].gameObject.SetActive (true);
		}
	}

	public void OnRecognize(RecognitionResult result){
		StartCoroutine(ClearString());
		StopAllCoroutines();
		ShowAll ();
		if (result != RecognitionResult.Empty && result.gesture.airTrick)
		{
			StartCoroutine(ClearString());

			if (PlayerControl.instance.isJumping)
			{
				PlayerControl.instance.TriggerTrick(result.gesture.id);
				StartCoroutine(ClearString());
				return;
			}
			else StartCoroutine(ClearString());

			textResult.text = result.gesture.id;
			StartCoroutine(Blink(result.gesture.id));
			StartCoroutine(ClearString());
		}
		else if (result != RecognitionResult.Empty && !result.gesture.airTrick)
		{
			StartCoroutine(ClearString());

			if (!PlayerControl.instance.isJumping)
			{
				PlayerControl.instance.TriggerTrick(result.gesture.id);
				StartCoroutine(ClearString());
				return;
			}
			else StartCoroutine(ClearString());

			textResult.text = result.gesture.id;
			StartCoroutine(Blink(result.gesture.id));
			StartCoroutine(ClearString());
		}

		else
		{
			textResult.text = "?";
			StartCoroutine(ClearString());
		}
		StartCoroutine(ClearString());
	
	}

	IEnumerator Blink(string id){
		var draw = references.Where (e => e.pattern.id == id).FirstOrDefault ();
		if (draw != null) {
			var seconds = new WaitForSeconds (0.1f);
			for (int i = 0; i <= 20; i++) {
				draw.gameObject.SetActive (i % 2 == 0);
				yield return seconds;
			}
			draw.gameObject.SetActive (true);
		}
	}
	
	IEnumerator ClearString()
    {
		yield return new WaitForSeconds(0.2f);
		textResult.text = "";
	}
}
