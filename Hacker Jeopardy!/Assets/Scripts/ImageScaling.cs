using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScaling : MonoBehaviour
{
    
    public void Scale_Image()
    {
        RawImage img = this.GetComponent<RawImage>();
        Texture s = img.texture;
        RectTransform r = this.GetComponent<RectTransform>();
        if (s.width > s.height) {
            float new_Height = r.rect.width * s.height / s.width;
            if (r.rect.height >= new_Height)
                r.sizeDelta = new Vector2(r.rect.width, new_Height);
            else
                r.sizeDelta = new Vector2(r.rect.height * s.width / s.height, r.rect.height);
        }
        else
        {
            r.sizeDelta = new Vector2( r.rect.height * s.width / s.height, r.rect.height);
        }
    }

    
}
