using UnityEngine;
using System.Collections;


public class MouseManager : MonoBehaviour
{

    UnitManager selectedUnit;

    void Start()
    {

    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.transform.parent.gameObject;
            
            
            if (hitObject.GetComponent<HexManager>() != null){
                //got em
                MouseOver_Hex(hitObject);
            }
            else if(hitObject.GetComponent<UnitManager>() != null){
                MouseOver_Unit(hitObject);
            }
            
            
        }
    }
    void MouseOver_Hex(GameObject hitObject)
    {
        //Clicking on a hex
        if (Input.GetMouseButtonDown(0))
        {
            MeshRenderer mr = hitObject.GetComponentInChildren<MeshRenderer>();

            if (mr.material.color == Color.red)
            {
                mr.material.color = Color.white;
            }
            else
            {
                mr.material.color = Color.red;
            }

            if (selectedUnit != null)
            {
                selectedUnit.destination = hitObject.transform.position;
            }
        }
    }
    void MouseOver_Unit(GameObject hitObject)
    {
        //Clicking on a hex
        if (Input.GetMouseButtonDown(0))
        {
            selectedUnit = hitObject.GetComponent<UnitManager>();
        }
    }
}
