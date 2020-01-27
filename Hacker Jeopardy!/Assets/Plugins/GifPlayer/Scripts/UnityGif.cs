/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace GifPlayer
{
    /// <summary>
    /// Gif播放器
    /// </summary>
    public class UnityGif : MonoBehaviour
    {
        /// <summary>
        /// 是否循环播放
        /// </summary>
        [SerializeField]
        [Header("If loop?")]
        bool Loop = true;

        /// <summary>
        /// GIF的文件流
        /// </summary>
        private RawImage _rawImage;

        private List<UnityFrame> _frames;

        void Awake()
        {
            _rawImage = GetComponent<RawImage>();
            
            
        }

        private bool _stop = true;
        private bool _scale = true;
        private int _frameIndex = 0;

        IEnumerator Play(GameObject guiObject)
        {
            //判断状态
            if (_stop)
            {
                _frameIndex = 0;
                
                _scale = true;
                yield break;
            }

            //帧序号
            _frameIndex %= _frames.Count;

            //绘图
            
            _rawImage.texture = _frames[_frameIndex].Texture;

            if (_scale)
            {
                guiObject.GetComponent<ImageScaling>().Scale_Image();
                _scale = false;
            }

            //帧延时
            yield return new WaitForSeconds(_frames[_frameIndex].DelaySecond);

            //序号++
            _frameIndex++;

            //播放一次
            if (!Loop && _frameIndex == _frames.Count)
            {
                
                yield break;
            }

            //递归播放下一帧
            StartCoroutine(Play(guiObject));
        }

        public void playGif(String path, GameObject guiObject)
        {

            if (path == null)
            {
                Debug.LogError(this.name + "\r\n请检查文件流\r\nCheck gif bytes");
                return;
            }

            Debug.Log(guiObject.name);

            _frames = GifProtocol.GetFrames(File.ReadAllBytes(path), "test");

            //播放序列帧
            if (_frames != null && _frames.Count > 0)
            {
                _stop = false;
                StartCoroutine(Play(guiObject));
            }

            
        }

        void OnDisable()
        {
            _stop = true;
            
        }
    }
}