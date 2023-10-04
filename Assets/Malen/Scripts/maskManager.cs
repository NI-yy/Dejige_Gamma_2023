using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maskManager : MonoBehaviour
{
    [SerializeField] bool isRedReleased;
    [SerializeField] bool isGreenReleased;
    [SerializeField] bool isBlueReleased;
    [SerializeField] bool isOrangeReleased;
    [SerializeField] bool isYellowReleased;
    [SerializeField] bool isPurpleReleased;

    [SerializeField] GameObject coloredImage;
    [SerializeField] Texture coloredTexture;
     
    // Start is called before the first frame update
    void Start()
    {
        Texture2D texture2D = new Texture2D(256, 256);

        //Texture2D����Sprite���쐬
        Sprite sprite = Sprite.Create(
          texture: texture2D,
          rect: new Rect(0, 0, texture2D.width, texture2D.height),
          pivot: new Vector2(0.5f, 0.5f)
        );

        //��������Sprite��SpriteRenderer�ɐݒ�
        coloredImage.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void setColorTileActive()
    {
        
    }
}
