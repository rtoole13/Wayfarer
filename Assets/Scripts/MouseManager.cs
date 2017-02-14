using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	// Update is called once per frame
	void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.collider.transform.parent.gameObject;
            MeshRenderer mr = hitObject.GetComponentInChildren<MeshRenderer>();
            HexManager hm = hitObject.GetComponent<HexManager>();

            if (Input.GetMouseButtonDown(1)) 
            {
                if (hm.walkable)
                {
                    mr.material.color = Color.cyan;
                } 
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (hm.walkable)
                {
                    mr.material.color = Color.white;
                }
            }
        }
	}
}
