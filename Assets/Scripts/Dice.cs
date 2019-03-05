using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dice : MonoBehaviour
{

    public AudioClip[] diceSounds;
    private AudioSource audio;
    public Vector3 displayPosition;
    private Rigidbody rb;
    private Dictionary<Vector3, int> orientations;
    private bool rolling = false;
    private Action onDoneRolling;
    private void Awake () {
        orientations = new Dictionary<Vector3, int> {
            [Vector3.up] = 1,
            [Vector3.down] = 6,
            [Vector3.right] = 4,
            [Vector3.left] = 3,
            [Vector3.forward] = 5,
            [Vector3.back] = 2,
        };
        rb = GetComponent<Rigidbody> ();
        audio = GetComponent<AudioSource> ();
        rb.maxAngularVelocity = 10;
    }

    private void Update () {
        if (rolling && rb.IsSleeping ()) {
            rolling = false;
            onDoneRolling ();
        }
    }

    public void Roll (Action done) {
        onDoneRolling = done;
        rolling = true;
        rb.AddForce (Vector3.up * 0.1f, ForceMode.Impulse);
       // rb.AddTorque (new Vector3 (UnityEngine.Random.Range (0, 100f), UnityEngine.Random.Range (0, 100f), UnityEngine.Random.Range (0, 100f)), ForceMode.Impulse);
        rb.AddTorque (UnityEngine.Random.insideUnitSphere * 0.1f, ForceMode.Impulse);
       // print (rb.maxAngularVelocity = 100);
    }

    public int Result () {
        foreach (var e in orientations) {
            if (Physics.Raycast (transform.position, transform.TransformDirection (e.Key), 0.5f)) {
                //print (e.Value);
                return e.Value;
            }
        }
        return -1;
    }

    public void ResetPosition (Action callback) {
        rb.isKinematic = true;
        StartCoroutine (Transition (target => rb.position = target, 0.5f, rb.position, displayPosition, () => {
            rb.isKinematic = false;
            callback ();
            }));
    }

    private IEnumerator Transition (Action<Vector3> result, float duration, Vector3 initial, Vector3 target, Action callback = null) {
		for (float percent = 0; percent <= 1; percent += Time.deltaTime / duration) {
			result (Vector3.Lerp (initial, target, percent));
			yield return null;
		}
		result (target);
        if (callback != null)
		    callback ();
	}

    private void OnCollisionEnter (Collision col) {
        //audio.Play ();
        
        if (!rolling)
            return;

        audio.PlayOneShot (diceSounds [UnityEngine.Random.Range (0, diceSounds.Length)], col.relativeVelocity.sqrMagnitude / 10);
    }

}
