/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GifPlayer
{
    /// <summary>
    /// 文件读取
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 读取
        /// </summary>
        public static byte[] GetBytes(string path)
        {
            WWW www = new WWW(path);
            while (!www.isDone) { }

            byte[] bytes = null;

            if (string.IsNullOrEmpty(www.error))
                bytes = www.bytes;

            www.Dispose();
            return bytes;
        }

        /// <summary>
        /// 读取,必须及时Dispose()
        /// </summary>
        public static MemoryStream GetStream(string path)
        {
            byte[] bytes = GetBytes(path);
            if (bytes != null && bytes.Length > 0)
                return new MemoryStream(bytes);
            return null;
        }

        /// <summary>
        /// 大小
        /// </summary>
        public static int GetLength(string path)
        {
            byte[] bytes = GetBytes(path);
            if (bytes != null)
                return bytes.Length;
            return 0;
        }

        /// <summary>
        /// 存在
        /// </summary>
        public static bool DoesExist(string path)
        {
            byte[] bytes = GetBytes(path);
            return bytes != null && bytes.Length > 0;
        }
    }
}