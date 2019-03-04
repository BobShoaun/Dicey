using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dice : MonoBehaviour
{

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
        rb.AddForce (Vector3.up * 1.1f, ForceMode.Impulse);
        rb.AddTorque (UnityEngine.Random.insideUnitSphere * 0.25f, ForceMode.Impulse);
    }

    public int Result () {
        foreach (var e in orientations) {
            if (Physics.Raycast (transform.position, transform.TransformDirection (e.Key))) {
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
		for (float percent = 0; percent <= 1; percent += Time.unscaledDeltaTime / duration) {
			result (Vector3.Lerp (initial, target, percent));
			yield return null;
		}
		result (target);
        if (callback != null)
		    callback ();
	}

}
