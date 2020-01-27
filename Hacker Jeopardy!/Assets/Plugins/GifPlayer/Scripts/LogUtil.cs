/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GifPlayer
{
    /// <summary>
    /// LogUtil
    /// </summary>
    public static class LogUtil
    {
        /// <summary>
        /// log
        /// </summary>
        public static void Log(string msg)
        {
            Debug.Log(string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss.fff"), msg));
        }

    }
}