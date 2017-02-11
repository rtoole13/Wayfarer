using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public GameObject hexPrefab;
    public int width;
    public int height;

    float xEvenOffset;
    float xOddOffset;

    float yEvenOffset;
    float yOddOffset;
    float skin = .3f;

    float xOffset;
    float yOffset;
    // Use this for initialization
    void Start ()
    {

        xEvenOffset = Mathf.Sqrt(3) / 2;
        xOddOffset = xEvenOffset;
        yEvenOffset = 3/4f;
        yOddOffset = yEvenOffset;
	    for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = x * xEvenOffset;
                float yPos = y * yEvenOffset;
                if (y % 2 == 1)
                {
                    xPos += xEvenOffset/2f;
                    //yPos = yOddOffset;
                }
                GameObject newHex = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
