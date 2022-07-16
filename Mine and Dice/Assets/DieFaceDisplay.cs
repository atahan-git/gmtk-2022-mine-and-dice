using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DieFaceDisplay : MonoBehaviour {


    [SerializeField]
    private Image bgImg;
    [SerializeField]
    private Image faceImg;
    [SerializeField]
    private Image noPipsOverlayImg;
    [SerializeField]
    private Image pipsImg;

    [SerializeField]
    private SpriteRenderer bg;
    [SerializeField]
    private SpriteRenderer face;
    [SerializeField]
    private SpriteRenderer noPipsOverlay;
    [SerializeField]
    private SpriteRenderer pips;
    
    [SerializeField]
    private Sprite[] pipsSprites;

    [SerializeField]
    private Sprite emptyFace;

    [SerializeField]
    private Sprite emptySprite;


    private Color realBGColor;

    public DieFace myFace;

    public void SetDieFace(Die myDie, DieFace face) {
        myFace = face;
        SetBGColor(myDie.color);
        //if (myFace.isFilled) {
            SetFace(myDie.gfx);
            SetPips(myFace.clampedPips);
            SetNoPipsOverlay(myFace.clampedPips == 0);
        /*} else {
            SetFace(null);
            SetPips(0);
            SetNoPipsOverlay(false);
        }*/
    }


    public Color activeBGColor = Color.white;
    void SetBGColor(Color color) {
        realBGColor = color;
        
        if (bg != null) 
            bg.color = color;
        

        if (bgImg != null) 
            bgImg.color = color;
        
    }

    void SetFace(Sprite sprite) {
        if (sprite == null) 
            sprite = emptySprite;
        

        if (face != null) 
            face.sprite = sprite;

        if (faceImg != null)
            faceImg.sprite = sprite;
    }


    void SetPips(int count) {
        count = Mathf.Clamp(count, 0, pipsSprites.Length);

        Sprite sprite = null;
        if (count == 0) {
            sprite = emptySprite;
        } else if (count > 0 && count < pipsSprites.Length + 1) {
            sprite = pipsSprites[count - 1];
        } else {
            Debug.LogError($"Illegal pip count {count}. Must be more that -1 and less than {pipsSprites.Length + 1}");
        }
        
        
        
        if (pips != null) 
            pips.sprite = sprite;
            
        if (pipsImg != null) 
            pipsImg.sprite = sprite;
    }


    void SetNoPipsOverlay(bool isNoPips) {
        if (isNoPips) {
            if (noPipsOverlay != null) {
                noPipsOverlay.gameObject.SetActive(true);
            }
            
            if (noPipsOverlayImg != null) {
                noPipsOverlayImg.gameObject.SetActive(true);
            }

            if (face != null) {
                var color = face.color;
                color.a = 0.5f;
                face.color = color;
            }

            if (faceImg != null) {
                var color = faceImg.color;
                color.a = 0.5f;
                faceImg.color = color;
            }
        } else {
            if (noPipsOverlay != null) {
                noPipsOverlay.gameObject.SetActive(false);
            }
            
            if (noPipsOverlayImg != null) {
                noPipsOverlayImg.gameObject.SetActive(false);
            }

            if (face != null) {
                var color = face.color;
                color.a = 1;
                face.color = color;
            }

            if (faceImg != null) {
                var color = faceImg.color;
                color.a = 1;
                faceImg.color = color;
            }
        }
    }

    public void SetHighlightStatus(bool status) {
        if (status) {
            if (bg != null) 
                bg.color = activeBGColor;
        

            if (bgImg != null) 
                bgImg.color = activeBGColor;
        } else {
            if (bg != null) 
                bg.color = realBGColor;
        

            if (bgImg != null) 
                bgImg.color = realBGColor;
        }
    }

    public void SetCanBeActivated(bool status) {
        if (status) {
            if (bg != null)
                bg.color = realBGColor;


            if (bgImg != null)
                bgImg.color = realBGColor;
        } else {
            Color.RGBToHSV(realBGColor, out float h, out float s, out float v);
            var disabledColor = Color.HSVToRGB(h, s, 0.2f);
            if (bg != null)
                bg.color = disabledColor;

            if (bgImg != null)
                bgImg.color = disabledColor;
        }
    }
}
