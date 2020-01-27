/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GifPlayer
{
    /// <summary>
    /// Unity序列帧
    /// </summary>
    public class UnityFrame
    {
        /// <summary>
        /// 帧画面
        /// </summary>
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// 帧画面
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return Sprite.texture;
            }
        }

        /// <summary>
        /// 帧延时
        /// </summary>
        public float DelaySecond { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public UnityFrame(Sprite sprite, float delaySecond)
        {
            Sprite = sprite;
            DelaySecond = delaySecond;
        }
    }
}