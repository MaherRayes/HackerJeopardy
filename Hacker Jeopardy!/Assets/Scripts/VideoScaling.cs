using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


public class VideoScaling : MonoBehaviour
{
    

    public void Scale_Video()
    {
        RectTransform r = this.GetComponent<RectTransform>();
        VideoPlayer vid = this.GetComponent<VideoPlayer>();
        Texture vid_Texture = vid.texture;
        
        if (vid_Texture.width > vid_Texture.height)
        {
            float new_Height = r.rect.width * vid_Texture.height / vid_Texture.width;

            if (r.rect.height >= new_Height)
                r.sizeDelta = new Vector2(r.rect.width, new_Height);
            else
                r.sizeDelta = new Vector2(r.rect.height * vid_Texture.width / vid_Texture.height, r.rect.height);
        }
        else
        {
            r.sizeDelta = new Vector2(r.rect.height * vid_Texture.width / vid_Texture.height, r.rect.height);
        }

           
    }

    
}
