using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTexture : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer SpriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        Texture2D groundTex = new Texture2D(256,256);

        for (int y = 0; y < groundTex.height; y++)
        {
            for (int x = 0; x < groundTex.width; x++)
            {
                Color color = x%2 != 0 ? Color.white : Color.gray;
                groundTex.SetPixel(x, y, color);
            }
        }
        groundTex.Apply();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
