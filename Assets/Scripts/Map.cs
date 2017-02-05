using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public GameObject hexPrefab;
    public int width;
    public int height;

    float xEvenOffset;
    float yEvenOffset;

    // Use this for initialization
    void Start ()
    {

        xEvenOffset = Mathf.Sqrt(3) / 2;
        yEvenOffset = 3/4f;

	    for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = x * xEvenOffset;
                float yPos = y * yEvenOffset;
                if (y % 2 == 1)
                {
                    xPos += xEvenOffset/2f;
                }
                GameObject newHex = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity);

                //rename hex
                newHex.name = "Hex_" + x + "_" + y;

                //set Map as parent
                newHex.transform.SetParent(this.transform);

            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
