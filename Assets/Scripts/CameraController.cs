using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Camera mCamera;

    public float scrollSpeed;
    public float dragSpeed;

    private bool drag = false;
    private Vector3 mouseOrigin;
    private Vector3 moveTo;
    private Vector3 viewCenter;

	// Use this for initialization
	void Awake () {
        mCamera = Camera.main;
        viewCenter = Vector3.one;
        viewCenter = 0.5f * viewCenter;
        viewCenter.z = 0;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        //--- Mouse Controls ---//
        // Pan
        if (Input.GetMouseButtonDown(1))
        {
            mouseOrigin = mCamera.ScreenToViewportPoint(Input.mousePosition) - viewCenter;
            drag = true;
        }
        if (Input.GetMouseButton(1))
        {
            moveTo = mCamera.ScreenToViewportPoint(Input.mousePosition) - viewCenter;
            Vector3 newPos = new Vector3(mCamera.transform.position.x + dragSpeed * (mouseOrigin.x - moveTo.x),
                mCamera.transform.position.y, mCamera.transform.position.z + dragSpeed * (mouseOrigin.y - moveTo.y));
            mCamera.transform.position = newPos;
            mouseOrigin = moveTo;
        }
        if (Input.GetMouseButtonUp(1))
        {
            drag = false;
        }

        // Zoom
        // FIXME: Won't work in a 3d setting. Need to implement screen to ray casting. Have a 2d plane that is only checked
        // by camera raycasting. Can get a vector to camera's center point and move along this vector to zoom/unzoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            mCamera.orthographicSize -= scrollSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            mCamera.orthographicSize += scrollSpeed;
        }

        // keyboard controls

    }
}
