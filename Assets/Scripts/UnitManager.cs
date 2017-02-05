using UnityEngine;
using System.Collections;

public class UnitManager : MonoBehaviour {

    public Vector3 destination;

    float speed = 2;
	// Use this for initialization
	void Start () {
        destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = destination - transform.position;
        Vector3 translateDist = direction.normalized * speed * Time.deltaTime;

        translateDist = Vector3.ClampMagnitude(translateDist, direction.magnitude);

        transform.Translate(translateDist);

	}
}
