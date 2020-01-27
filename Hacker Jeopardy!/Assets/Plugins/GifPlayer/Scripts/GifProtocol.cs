/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GifPlayer
{
    /// <summary>
    /// GIF文件协议
    /// </summary>
    public static class GifProtocol
    {
        #region 文件结构 //结构顺序是有意义的 根据 重要程度、处理先后 排序

        /// <summary>
        /// GIF文件结构
        /// </summary>
        public class GraphicsInterchangeFormat
        {
            #region 全局属性

            /// <summary>
            /// GIF声明
            /// </summary>
            public string Signature { get; set; }

            /// <summary>
            /// 版本声明
            /// </summary>
            public string Version { get; set; }

            /// <summary>
            /// 画布宽
            /// </summary>
            public ushort Width { get; set; }

            /// <summary>
            /// 画布高
            /// </summary>
            public ushort Height { get; set; }

            #region Packet Fields(1 byte)

            /// <summary>
            /// 是否包含全局色表
            /// </summary>
            public bool FlagGlobalColorTable { get; set; }

            /// <summary>
            /// 颜色分辨率 （不处理）
            /// </summary>
            public int ColorResolution { get; set; }

            /// <summary>
            /// 重要颜色前排标识 一般为false （不处理）
            /// </summary>
            public bool Flagsort { get; set; }

            /// <summary>
            /// 全局色表的数量 范围 0-256 所占字节再*3
            /// </summary>
            public int GlobalColorTableSize { get; set; }

            #endregion

            /// <summary>
            /// 背景色色号
            /// </summary>
            public byte BgColorIndex { get; set; }

            /// <summary>
            /// 像素宽高比
            /// </summary>
            public byte PixelAspectRatio { get; set; }

            /// <summary>
            /// 全局色表 3个byte一组
            /// </summary>
            public List<byte[]> GlobalColorTable { get; set; }

            #endregion

            #region 局部属性（序列帧）集

            /// <summary>
            /// 绘图控制扩展集
            /// </summary>
            public List<GraphicControlExtension> GraphicControlExtensions { get; set; }

            /// <summary>
            /// 帧描述器集
            /// </summary>
            public List<ImageDescriptor> ImageDescriptors { get; set; }

            /// <summary>
            /// 应用扩展集
            /// </summary>
            public List<ApplicationExtension> ApplicationExtensions { get; set; }

            /// <summary>
            /// 注释扩展集
            /// </summary>
            public List<CommentExtension> CommentExtensions { get; set; }

            /// <summary>
            /// 文本扩展集
            /// </summary>
            public List<PlainTextExtension> PlainTextExtensions { get; set; }

            #endregion

            /// <summary>
            /// 结束符 0x3B
            /// </summary>
            public byte Trailer { get; set; }

            /// <summary>
            /// 初始化
            /// </summary>
            public GraphicsInterchangeFormat(byte[] bytes)
            {
                GraphicControlExtensions = new List<GraphicControlExtension>();
                ImageDescriptors = new List<ImageDescriptor>();
                CommentExtensions = new List<CommentExtension>();
                PlainTextExtensions = new List<PlainTextExtension>();
                ApplicationExtensions = new List<ApplicationExtension>();

                int byteIndex = 0;
                this.SetGlobalProperties(bytes, ref byteIndex);
                this.SetExtensionsAndDescriptors(bytes, ref byteIndex);
            }
        }

        /// <summary>
        /// 下一帧图像预处理方法
        /// </summary>
        public enum DisposalMethod
        {
            /// <summary>
            /// 正常处理
            /// <para>0 - No disposal specified. The decoder is not required to take any action. </para>
            /// </summary>
            Normal = 0,

            /// <summary>
            /// 保留当前帧
            /// <para>1 - Do not dispose. The graphic is to be left in place. </para>
            /// </summary>
            Last = 1,

            /// <summary>
            /// 还原背景色
            /// <para>2 - Restore to background color. The area used by the graphic must be restored to the background color. </para>
            /// </summary>
            Bg = 2,

            /// <summary>
            /// 还原上一帧
            /// <para>3 - Restore to previous. The decoder is required to restore the area overwritten by the graphic with what was there prior to rendering the graphic.</para>
            /// </summary>
            Previous = 3,
        }

        /// <summary>
        /// 绘图控制扩展（包含绘图信息,必须处理）
        /// </summary>
        public  struct GraphicControlExtension
        {
            /// <summary>
            /// 扩展引导标识 0x21
            /// </summary>
            public byte Introducer { get; set; }

            /// <summary>
            /// 绘图控制扩展标识 0xF9
            /// </summary>
            public byte Label { get; set; }

            /// <summary>
            /// 扩展字节长度 不算结尾字节 绘图控制拓展的字节长度基本是0x04
            /// </summary>
            public byte Length { get; set; }

            #region Packet Fields (1byte)

            /// <summary>
            /// 保留字段（3bit）
            /// </summary>
            public int Reserved { get; set; }

            /// <summary>
            /// 下一帧图像预处理方法（3bit）
            /// </summary>
            public DisposalMethod DisposalMethod { get; set; }

            /// <summary>
            /// 用户输入标志,为1 时表示处理完该图像域后等待用户的输入后才开始下一图像域的处理。(1bit)
            /// </summary>
            public bool FlagUserInput { get; set; }

            /// <summary>
            /// 透明颜色索引有效标志,该标志置位表示透明颜色索引有效,使用方法参照【透明颜色索引】注释。补充：同时表示背景透明(1bit)
            /// </summary>
            public bool FlagTransparentColor { get; set; }

            #endregion

            /// <summary>
            /// 帧延时 1/100 秒 （2byte）
            /// </summary>
            public ushort DelayTime { private get; set; }

            /// <summary>
            /// 帧延时(秒)
            /// </summary>
            public float DelaySecond
            {
                get
                {
                    float delaySecond = DelayTime / 100f;

                    if (delaySecond <= 0f)
                        delaySecond = 0.1f;

                    return delaySecond;
                }
            }

            /// <summary>
            /// 透明颜色索引,在透明颜色索引有效标识为真的情况下,解码所得颜色索引与该索引值相等时,数据将不作处理（不更新对应像素）。(1byte)
            /// </summary>
            public byte TransparentColorIndex { get; set; }

            /// <summary>
            /// 尾部标识 0x00 (1byte)
            /// </summary>
            public byte Terminator { get; set; }
        }

        /// <summary>
        /// 字节块
        /// </summary>
        public struct Block
        {
            /// <summary>
            /// 长度
            /// </summary>
            public byte Size { get; set; }

            /// <summary>
            /// 字节组
            /// </summary>
            public byte[] Bytes { get; set; }
        }

        /// <summary>
        /// GIF序列帧 描述器
        /// </summary>
        public struct ImageDescriptor
        {
            /// <summary>
            /// 描述器 标志 分隔符 0x2C
            /// </summary>
            public byte Separator { get; set; }

            /// <summary>
            /// 左边距离
            /// </summary>
            public ushort MarginLeft { get; set; }

            /// <summary>
            /// 顶部距离
            /// </summary>
            public ushort MarginTop { get; set; }

            /// <summary>
            /// 帧宽
            /// </summary>
            public ushort Width { get; set; }

            /// <summary>
            /// 帧高
            /// </summary>
            public ushort Height { get; set; }

            #region Packet Fields(1 byte)

            /// <summary>
            /// 是否包含局部色表（为真时使用局部色表,为假时使用全局色表）
            /// </summary>
            public bool FlagLocalColorTable { get; set; }

            /// <summary>
            /// 是否包含交错
            /// </summary>
            public bool FlagInterlace { get; set; }

            /// <summary>
            /// 重要颜色排序标志 一般为0 不作处理
            /// </summary>
            public bool FlagSort { get; set; }

            /// <summary>
            /// 保留字段
            /// </summary>
            public int Reserved { get; set; }

            /// <summary>
            /// 局部色表大小
            /// </summary>
            public int LocalColorTableSize { get; set; }

            #endregion

            /// <summary>
            /// 局部色表
            /// </summary>
            public List<byte[]> LocalColorTable { get; set; }

            /// <summary>
            /// 表示表示一个像素索引值所用的最少比特位数,如：该值为0x08时表示解码后的每个像素索引值为8 位（一个字节,可以表示256种颜色）。
            /// </summary>
            public byte LzwCodeSize { get; set; }

            /// <summary>
            /// lzw压缩后的色号字节块
            /// </summary>
            public List<Block> LzwPixelsBlocks { get; set; }
        }

        /// <summary>
        /// 应用扩展（解析文件时需处理,绘图时可忽略）
        /// </summary>
        public struct ApplicationExtension
        {
            /// <summary>
            /// 扩展引导标识 0x21
            /// </summary>
            public byte Introducer { get; set; }

            /// <summary>
            /// 应用扩展标识 0xFF
            /// </summary>
            public byte Label { get; set; }

            // Block Size
            public byte BlockSize { get; set; }

            // Block Size & Application Data List
            public List<Block> Blocks { get; set; }

            public string ApplicationIdentifier { get; set; }

            public string ApplicationAuthenticationCode { get; set; }
        }

        /// <summary>
        /// 注释扩展（解析文件时需处理,绘图时可忽略）
        /// </summary>
        public struct CommentExtension
        {
            /// <summary>
            /// 扩展引导标识 0x21
            /// </summary>
            public byte Introducer { get; set; }

            /// <summary>
            /// 注释扩展标识 0xFE
            /// </summary>
            public byte Label { get; set; }

            // Block Size & Comment Data List
            public List<Block> Blocks { get; set; }
        }

        /// <summary>
        /// 文本扩展（解析文件时需处理,绘图时可忽略）
        /// </summary>
        public struct PlainTextExtension
        {
            /// <summary>
            /// 扩展引导标识 定值0x21
            /// </summary>
            public byte Introducer { get; set; }

            /// <summary>
            /// 文本扩展标识 0x01
            /// </summary>
            public byte Label { get; set; }

            // Block Size
            public byte BlockSize { get; set; }

            // Block Size & Plain Text Data List
            public List<Block> Blocks { get; set; }
        }

        #endregion

        #region 解析方法 //方法顺序是有意义的 根据 重要程度、处理先后 排序

        /// <summary>
        /// 设置全局属性
        /// </summary>
        public static void SetGlobalProperties(this GraphicsInterchangeFormat gif, byte[] bytes, ref int byteIndex)
        {
            // GIF声明 (GIF) (3 byte)
            gif.Signature = Encoding.ASCII.GetString(bytes, 0, 3);

            // 版本声明 (87a) (3 byte)
            gif.Version = Encoding.ASCII.GetString(bytes, 3, 3);

            // 画布宽度(2 byte)
            gif.Width = BitConverter.ToUInt16(bytes, 6);

            // 画布高度(2 byte)
            gif.Height = BitConverter.ToUInt16(bytes, 8);

            #region Packet Fields (1 byte)

            // 是否包含全局色表(1 bit)
            // 为1 时表明Logical Screen Descriptor 后面跟的是全局颜色表。
            gif.FlagGlobalColorTable = bytes[10] >> 7 == 1;

            // 颜色分辨率(3 bit)
            //值加1 代表颜色表中每种基色用多少位表示,如为“111”时表示每种基色用8 位表示,则颜色表中每项为3Byte。由于该值有时可为0,一般在解码程序中,该3 位不作处理,而直接由Global Color Table Size 算出颜色表大小。
            gif.ColorResolution = (bytes[10] >> 4) % 8 + 1;

            // 重要颜色前排标识(1 bit)
            //表示重要颜色排序标志,标志为1 时,表示颜色表中重要的颜色排在前面,有利于颜色数较少的解码器选择最好的颜色。一般该标志为0,不作处理。
            gif.Flagsort = (bytes[10] >> 3) % 2 == 1;

            //色表长度（一个色表有3字节）(3 bit)
            //值加1 作为2 的幂,算得的数即为颜色表的项数,实际上颜色表每项由RGB 三基色构成,每种基色占一个字节,则颜色表占字节数为项数的3 倍。由于最大值为“111”,故颜色表的项数最多为256项,即256 种颜色,8 位每基色则颜色表大小为768 Bytes。
            int power = bytes[10] % 8 + 1;
            gif.GlobalColorTableSize = (int)Math.Pow(2, power);

            #endregion

            // 背景色色号(1 byte)
            //表示背景颜色索引值*。可以这样理解：在指定大小显示区,GIF 图像的大小可能小于显示区域大小,显示区中剩余的区域则一律用背景颜色索引值在全局颜色表中对应的颜色填充。在实际解码过程中,在显示图像之前可将显示区域全部用该颜色填充。
            gif.BgColorIndex = bytes[11];

            // 像素宽高比(1 byte)
            //表示像素宽高比,一般为0,不作处理,直接以Logical Screen 宽和高作处理。如该项不为0,则参照GIF89a 标准【1】计算。
            gif.PixelAspectRatio = bytes[12];

            byteIndex = 13;

            //判断是否包含全局色表
            if (gif.FlagGlobalColorTable)
            {
                //获取全局色表
                gif.GlobalColorTable = new List<byte[]>();

                //色表长度*3（字节数）
                for (int index = byteIndex; index < byteIndex + (gif.GlobalColorTableSize * 3); index += 3)
                    //3个字节一组
                    gif.GlobalColorTable.Add(new byte[] { bytes[index], bytes[index + 1], bytes[index + 2] });

                //指针下移
                byteIndex += (gif.GlobalColorTableSize * 3);
            }
        }

        /// <summary>
        /// 设置扩展属性和帧描述
        /// </summary>
        public static void SetExtensionsAndDescriptors(this GraphicsInterchangeFormat gif, byte[] bytes, ref int byteIndex)
        {
            //记录上次处理的序号
            int lastIndex = 0;

            //处理字节 序号相同就是没有字节能识别 循环结束
            while (lastIndex != byteIndex)
            {
                //同步序号
                lastIndex = byteIndex;

                //处理下一字节
                switch (bytes[byteIndex])
                {
                    //扩展 0x21
                    case 0x21:
                        switch (bytes[byteIndex + 1])
                        {
                            //绘图控制扩展 0xF9
                            case 0xF9:
                                gif.GetGraphicControlExtension(bytes, ref byteIndex);
                                break;

                            //应用扩展 0xFF
                            case 0xFF:
                                gif.GetApplicationExtension(bytes, ref byteIndex);
                                break;

                            //注释扩展 0xFE
                            case 0xFE:
                                gif.GetCommentExtension(bytes, ref byteIndex);
                                break;

                            //文本扩展 0x01
                            case 0x01:
                                gif.GetPlainTextExtension(bytes, ref byteIndex);
                                break;
                        }
                        break;

                    //帧描述器 0x2C
                    case 0x2C:
                        gif.GetDescriptor(bytes, ref byteIndex);
                        break;

                    //结尾 0x3B
                    case 0x3B:
                        gif.Trailer = bytes[byteIndex];
                        byteIndex++;
                        return;
                }
            }
        }

        /// <summary>
        /// 获取绘图控制扩展
        /// </summary>
        private static void GetGraphicControlExtension(this GraphicsInterchangeFormat gif, byte[] bytes, ref int byteIndex)
        {
            //初始化
            GraphicControlExtension graphicControlExtension = new GraphicControlExtension();

            //扩展标引导记 0x21 (1 byte)
            graphicControlExtension.Introducer = bytes[byteIndex];
            byteIndex++;

            //绘图控制扩展标记 0xF9 (1 byte)
            graphicControlExtension.Label = bytes[byteIndex];
            byteIndex++;

            //扩展字节长度 0x04 (1 byte)
            graphicControlExtension.Length = bytes[byteIndex];
            byteIndex++;

            #region Packet Fields (1byte)

            //保留字段（3bit）
            graphicControlExtension.Reserved = bytes[byteIndex] >> 5;

            //图像显示方法 (3 bit)
            graphicControlExtension.DisposalMethod = (DisposalMethod)((bytes[byteIndex] >> 2) % 8);

            //用户输入标志 (1 bit)
            graphicControlExtension.FlagUserInput = (bytes[byteIndex] >> 1) % 2 == 1;

            //透明颜色索引有效标志 (1 bit)
            graphicControlExtension.FlagTransparentColor = bytes[byteIndex] % 2 == 1;

            #endregion

            //指针下移
            byteIndex++;

            //帧延时 (2 byte)
            graphicControlExtension.DelayTime = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            //透明颜色索引 (1 byte)
            graphicControlExtension.TransparentColorIndex = bytes[byteIndex];
            byteIndex++;

            //结尾标识 (1 byte)
            graphicControlExtension.Terminator = bytes[byteIndex];
            byteIndex++;

            //添加到gif中
            gif.GraphicControlExtensions.Add(graphicControlExtension);
        }

        /// <summary>
        /// 获取帧描述器
        /// </summary>
        private static void GetDescriptor(this GraphicsInterchangeFormat gif, byte[] bytes, ref int byteIndex)
        {
            //初始化
            ImageDescriptor imageDescriptor = new ImageDescriptor();

            //描述器标识符 0x2c (1 byte)
            imageDescriptor.Separator = bytes[byteIndex];
            byteIndex++;

            //图像起始点 (2 byte)
            imageDescriptor.MarginLeft = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            //图像起始点 (2 byte)
            imageDescriptor.MarginTop = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            //图像宽度 (2 byte)
            imageDescriptor.Width = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            //图像高度 (2 byte)
            imageDescriptor.Height = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            #region Packet Fields (1 byte) //此处算法参照全局配置

            //是否使用局部色表 (1 bit)
            imageDescriptor.FlagLocalColorTable = bytes[byteIndex] >> 7 == 1;

            //是否包含交错 (1 bit)
            imageDescriptor.FlagInterlace = (bytes[byteIndex] >> 6) % 2 == 1;

            //重要颜色靠前标识 (1 bit)
            imageDescriptor.FlagSort = (bytes[byteIndex] >> 5) % 2 == 1;

            //保留字段 (2 bit)
            imageDescriptor.Reserved = (bytes[byteIndex] >> 3) % 4;

            //局部色表长度 (3 bit)
            int power = bytes[byteIndex] % 8 + 1;
            imageDescriptor.LocalColorTableSize = (int)Math.Pow(2, power);

            #endregion

            //指针下移
            byteIndex++;

            //是否包含局部色表,计算方法参照全局色表
            if (imageDescriptor.FlagLocalColorTable)
            {
                imageDescriptor.LocalColorTable = new List<byte[]>();
                for (int index = byteIndex; index < byteIndex + (imageDescriptor.LocalColorTableSize * 3); index += 3)
                    imageDescriptor.LocalColorTable.Add(new byte[] { bytes[index], bytes[index + 1], bytes[index + 2] });
                byteIndex += (imageDescriptor.LocalColorTableSize * 3);
            }

            //Lzw解码后 拆分成色号的单位bit长度(1 byte)
            imageDescriptor.LzwCodeSize = bytes[byteIndex];
            byteIndex++;

            //紧接着是表示色号的lzw压缩后的字节块 结构：长度+数据+长度+数据+...+结尾符
            while (true)
            {
                //字节块长度(1 byte)
                byte blockSize = bytes[byteIndex];
                byteIndex++;

                //判断是否为结尾符
                if (blockSize == 0x00)
                    break;

                //初始化字节块
                Block block = new Block();
                block.Size = blockSize;
                block.Bytes = new byte[block.Size];

                //字节块赋值
                for (int index = 0; index < block.Bytes.Length; index++)
                {
                    block.Bytes[index] = bytes[byteIndex];
                    byteIndex++;
                }

                //添加到描述器中
                if (imageDescriptor.LzwPixelsBlocks == null)
                    imageDescriptor.LzwPixelsBlocks = new List<Block>();
                imageDescriptor.LzwPixelsBlocks.Add(block);
            }

            //添加到Gif中
            gif.ImageDescriptors.Add(imageDescriptor);
        }

        /// <summary>
        /// 获取注释扩展
        /// </summary>
        private static void GetCommentExtension(this GraphicsInterchangeFormat gif, byte[] bytes, ref int byteIndex)
        {
            CommentExtension commentExtension = new CommentExtension();

            // Extension Introducer(1 byte)
            // 0x21
            commentExtension.Introducer = bytes[byteIndex];
            byteIndex++;

            // Comment Label(1 byte)
            // 0xFE
            commentExtension.Label = bytes[byteIndex];
            byteIndex++;

            // Block Size & Comment Data List
            while (true)
            {
                // Block Size(1 byte)
                byte blockSize = bytes[byteIndex];
                byteIndex++;

                if (blockSize == 0x00)
                {
                    // Block Terminator(1 byte)
                    break;
                }

                Block block = new Block();
                block.Size = blockSize;

                // Comment Data(n byte)
                block.Bytes = new byte[block.Size];
                for (int index = 0; index < block.Bytes.Length; index++)
                {
                    block.Bytes[index] = bytes[byteIndex];
                    byteIndex++;
                }

                if (commentExtension.Blocks == null)
                    commentExtension.Blocks = new List<Block>();

                commentExtension.Blocks.Add(block);
            }

            gif.CommentExtensions.Add(commentExtension);
        }

        /// <summary>
        /// 获取文字扩展
        /// </summary>
        private static void GetPlainTextExtension(this GraphicsInterchangeFormat gif, byte[] bytes, ref int byteIndex)
        {
            PlainTextExtension plainTextExtension = new PlainTextExtension();

            // Extension Introducer(1 byte)
            // 0x21
            plainTextExtension.Introducer = bytes[byteIndex];
            byteIndex++;

            // Plain Text Label(1 byte)
            // 0x01
            plainTextExtension.Label = bytes[byteIndex];
            byteIndex++;

            // Block Size(1 byte)
            // 0x0c
            plainTextExtension.BlockSize = bytes[byteIndex];
            byteIndex++;

            // Text Grid Left Position(2 byte)
            // Not supported
            byteIndex += 2;

            // Text Grid Top Position(2 byte)
            // Not supported
            byteIndex += 2;

            // Text Grid Width(2 byte)
            // Not supported
            byteIndex += 2;

            // Text Grid Height(2 byte)
            // Not supported
            byteIndex += 2;

            // Character Cell Width(1 byte)
            // Not supported
            byteIndex++;

            // Character Cell Height(1 byte)
            // Not supported
            byteIndex++;

            // Text Foreground Color Index(1 byte)
            // Not supported
            byteIndex++;

            // Text Background Color Index(1 byte)
            // Not supported
            byteIndex++;

            // Block Size & Plain Text Data List
            while (true)
            {
                // Block Size(1 byte)
                byte blockSize = bytes[byteIndex];
                byteIndex++;

                if (blockSize == 0x00)
                {
                    // Block Terminator(1 byte)
                    break;
                }

                Block block = new Block();
                block.Size = blockSize;

                // Plain Text Data(n byte)
                block.Bytes = new byte[block.Size];
                for (int index = 0; index < block.Bytes.Length; index++)
                {
                    block.Bytes[index] = bytes[byteIndex];
                    byteIndex++;
                }

                if (plainTextExtension.Blocks == null)
                    plainTextExtension.Blocks = new List<Block>();

                plainTextExtension.Blocks.Add(block);
            }

            gif.PlainTextExtensions.Add(plainTextExtension);
        }

        /// <summary>
        /// 获取程序扩展
        /// </summary>
        private static void GetApplicationExtension(this GraphicsInterchangeFormat gif, byte[] bytes, ref int byteIndex)
        {
            ApplicationExtension applicationExtension = new ApplicationExtension();

            // Extension Introducer(1 byte)
            // 0x21
            applicationExtension.Introducer = bytes[byteIndex];
            byteIndex++;

            // Extension Label(1 byte)
            // 0xFF
            applicationExtension.Label = bytes[byteIndex];
            byteIndex++;

            // Block Size(1 byte)
            // 0x0B
            applicationExtension.BlockSize = bytes[byteIndex];
            byteIndex++;

            // Application Identifier(8 byte)
            applicationExtension.ApplicationIdentifier = Encoding.ASCII.GetString(bytes, byteIndex, 8);
            byteIndex += 8;

            // Application Authentication Code(3 byte)
            applicationExtension.ApplicationAuthenticationCode = Encoding.ASCII.GetString(bytes, byteIndex, 3);
            byteIndex += 3;

            // Block Size & Application Data List
            while (true)
            {
                // Block Size (1 byte)
                byte blockSize = bytes[byteIndex];
                byteIndex++;

                if (blockSize == 0x00)
                {
                    // Block Terminator(1 byte)
                    break;
                }

                Block block = new Block();
                block.Size = blockSize;

                // Application Data(n byte)
                block.Bytes = new byte[block.Size];
                for (int index = 0; index < block.Bytes.Length; index++)
                {
                    block.Bytes[index] = bytes[byteIndex];
                    byteIndex++;
                }

                if (applicationExtension.Blocks == null)
                    applicationExtension.Blocks = new List<Block>();

                applicationExtension.Blocks.Add(block);
            }

            gif.ApplicationExtensions.Add(applicationExtension);
        }

        #endregion

        #region 解码方法 包含Lzw解码和GIF交错解码

        /// <summary>
        /// 初始化lzw字典
        /// </summary>
        private static Dictionary<int, string> InitLzwDictionary(int power)
        {
            //字典大小为2的power次方
            int dictLength = (int)Math.Pow(2, power);
            Dictionary<int, string> dict = new Dictionary<int, string>();
            for (int index = 0; index < dictLength + 2; index++)
                //ASCII (0,c0),(1,c1),(2,c2),(3,c3)....(length+2,c(length+2))
                dict.Add(index, ((char)index).ToString());
            return dict;
        }

        /// <summary>
        /// 转为整数
        /// </summary>
        private static int ToInt(this BitArray array, int startIndex, int bitLength)
        {
            BitArray newArray = new BitArray(bitLength);

            for (int index = 0; index < bitLength; index++)
                if (array.Length <= startIndex + index)
                    newArray[index] = false;
                else
                    newArray[index] = array.Get(startIndex + index);

            if (newArray.Length > 32)
                return 0;

            int[] intArray = new int[1];
            newArray.CopyTo(intArray, 0);
            return intArray[0];
        }

        /// <summary>
        /// Lwz解码为目标长度
        /// </summary>
        private static byte[] GetLzwDecodedBytes(List<byte> srcBytes, int dictPower, int destLength)
        {
            //初始化字典
            int dictPowerPlush = dictPower + 1;
            int dictLength = (int)Math.Pow(2, dictPower);
            int dictLengthPlush = dictLength + 1;
            Dictionary<int, string> dict = InitLzwDictionary(dictPower);

            //转为比特流处理
            BitArray srcBits = new BitArray(srcBytes.ToArray());

            byte[] destBytes = new byte[destLength];
            int outputAddIndex = 0;

            string prevEntry = null;

            bool dictInitFlag = false;

            int bitDataIndex = 0;

            // 循环处理Lzw数据
            while (bitDataIndex < srcBits.Length)
            {
                if (dictInitFlag)
                {
                    dictPowerPlush = dictPower + 1;
                    dictLength = (int)Math.Pow(2, dictPower);
                    dictLengthPlush = dictLength + 1;
                    dict = InitLzwDictionary(dictPower);
                    dictInitFlag = false;
                }

                int key = srcBits.ToInt(bitDataIndex, dictPowerPlush);

                string entry;

                if (key == dictLength)
                {
                    dictInitFlag = true;
                    bitDataIndex += dictPowerPlush;
                    prevEntry = null;
                    continue;
                }
                else if (key == dictLengthPlush)
                    break;
                else if (dict.ContainsKey(key))
                    entry = dict[key];
                else if (key >= dict.Count)
                {
                    if (prevEntry != null)
                        entry = prevEntry + prevEntry[0];
                    else
                    {
                        bitDataIndex += dictPowerPlush;
                        continue;
                    }
                }
                else
                {
                    bitDataIndex += dictPowerPlush;
                    continue;
                }

                byte[] temp = Encoding.Unicode.GetBytes(entry);
                for (int index = 0; index < temp.Length; index++)
                {
                    if (index % 2 == 0)
                    {
                        destBytes[outputAddIndex] = temp[index];
                        outputAddIndex++;
                    }
                }

                if (outputAddIndex >= destLength)
                    break;

                if (prevEntry != null)
                    dict.Add(dict.Count, prevEntry + entry[0]);

                prevEntry = entry;

                bitDataIndex += dictPowerPlush;

                if (dictPowerPlush == 3 && dict.Count >= 8)
                    dictPowerPlush = 4;
                else if (dictPowerPlush == 4 && dict.Count >= 16)
                    dictPowerPlush = 5;
                else if (dictPowerPlush == 5 && dict.Count >= 32)
                    dictPowerPlush = 6;
                else if (dictPowerPlush == 6 && dict.Count >= 64)
                    dictPowerPlush = 7;
                else if (dictPowerPlush == 7 && dict.Count >= 128)
                    dictPowerPlush = 8;
                else if (dictPowerPlush == 8 && dict.Count >= 256)
                    dictPowerPlush = 9;
                else if (dictPowerPlush == 9 && dict.Count >= 512)
                    dictPowerPlush = 10;
                else if (dictPowerPlush == 10 && dict.Count >= 1024)
                    dictPowerPlush = 11;
                else if (dictPowerPlush == 11 && dict.Count >= 2048)
                    dictPowerPlush = 12;
                else if (dictPowerPlush == 12 && dict.Count >= 4096)
                {
                    int nextKey = srcBits.ToInt(bitDataIndex, dictPowerPlush);
                    if (nextKey != dictLength)
                        dictInitFlag = true;
                }
            }

            return destBytes;
        }

        /// <summary>
        /// 整理交错
        /// </summary>
        private static byte[] GetInterlaceDecodedIndexs(byte[] indexs, int width)
        {
            int height = 0;
            int dataIndex = 0;
            byte[] newIndexs = new byte[indexs.Length];
            // Every 8th. row, starting with row 0.
            for (int index = 0; index < newIndexs.Length; index++)
            {
                if (height % 8 == 0)
                {
                    newIndexs[index] = indexs[dataIndex];
                    dataIndex++;
                }
                if (index != 0 && index % width == 0)
                {
                    height++;
                }
            }
            height = 0;
            // Every 8th. row, starting with row 4.
            for (int index = 0; index < newIndexs.Length; index++)
            {
                if (height % 8 == 4)
                {
                    newIndexs[index] = indexs[dataIndex];
                    dataIndex++;
                }
                if (index != 0 && index % width == 0)
                {
                    height++;
                }
            }
            height = 0;
            // Every 4th. row, starting with row 2.
            for (int index = 0; index < newIndexs.Length; index++)
            {
                if (height % 4 == 2)
                {
                    newIndexs[index] = indexs[dataIndex];
                    dataIndex++;
                }
                if (index != 0 && index % width == 0)
                {
                    height++;
                }
            }
            height = 0;
            // Every 2nd. row, starting with row 1.
            for (int index = 0; index < newIndexs.Length; index++)
            {
                if (height % 8 != 0 && height % 8 != 4 && height % 4 != 2)
                {
                    newIndexs[index] = indexs[dataIndex];
                    dataIndex++;
                }
                if (index != 0 && index % width == 0)
                {
                    height++;
                }
            }

            return newIndexs;
        }

        /// <summary>
        /// 获取像素的色号集
        /// </summary>
        private static byte[] GetColorIndexs(this ImageDescriptor imageDescriptor)
        {
            //合并Lzw数据
            List<byte> lzwedBytes = new List<byte>();
            foreach (Block block in imageDescriptor.LzwPixelsBlocks)
                lzwedBytes.AddRange(block.Bytes);

            // Lzw解码
            int indexsLength = imageDescriptor.Height * imageDescriptor.Width;
            byte[] indexs = GetLzwDecodedBytes(lzwedBytes, imageDescriptor.LzwCodeSize, indexsLength);

            // 整理交错
            if (imageDescriptor.FlagInterlace)
                indexs = GetInterlaceDecodedIndexs(indexs, imageDescriptor.Width);

            return indexs;
        }

        #endregion

        #region 转换为UnityFrame

        /// <summary>
        /// 透明色
        /// </summary>
        private static Color32 _colorTransparent = new Color32(0, 0, 0, 0);

        /// <summary>
        /// 全局色表中获取颜色
        /// </summary>
        private static Color32 GetColor(this GraphicsInterchangeFormat gif, int colorIndex)
        {
            //异常
            if (!gif.FlagGlobalColorTable || colorIndex >= gif.GlobalColorTableSize)
            {
                Debug.LogError("全局色表不存在或者超出全局色表长度");
                return _colorTransparent;
            }

            //全局色
            byte[] rgb = gif.GlobalColorTable[colorIndex];
            return new Color32(rgb[0], rgb[1], rgb[2], 255);
        }

        /// <summary>
        /// 局部色表中获取颜色
        /// </summary>
        private static Color32 GetColor(this ImageDescriptor descriptor, int colorIndex, GraphicControlExtension control, GraphicsInterchangeFormat gif)
        {
            //透明色
            if (control.FlagTransparentColor && colorIndex == control.TransparentColorIndex)
                return _colorTransparent;

            //全局色
            if (!descriptor.FlagLocalColorTable)
                return gif.GetColor(colorIndex);

            //异常
            if (colorIndex >= descriptor.LocalColorTableSize)
            {
                Debug.LogError("超出局部色表长度");
                return _colorTransparent;
            }

            //局部色
            byte[] rgb = descriptor.LocalColorTable[colorIndex];
            return new Color32(rgb[0], rgb[1], rgb[2], 255);
        }

        /// <summary>
        /// 中心点
        /// </summary>
        private static Vector2 _pivotCenter = new Vector2(0.5f, 0.5f);

        /// <summary>
        /// 获取序列帧集
        /// </summary>
        private static List<UnityFrame> GetFrames(GraphicsInterchangeFormat gif)
        {
            //序列帧集初始化
            var frames = new List<UnityFrame>();
            //背景色
            var colorBackground = gif.GetColor(gif.BgColorIndex);
            var textureBackground = new Texture2D(gif.Width, gif.Height);
            var textureTransparent = new Texture2D(gif.Width, gif.Height);
            for (int x = 0; x < gif.Width; x++)
            {
                for (int y = 0; y < gif.Height; y++)
                {
                    textureBackground.SetPixel(x, y, colorBackground);
                    textureTransparent.SetPixel(x, y, _colorTransparent);
                }
            }
            textureBackground.Apply();
            textureTransparent.Apply();

            //初始化Texture
            var textureFrame = new Texture2D(gif.Width, gif.Height);
            //绘图方式
            var disposalMethod = DisposalMethod.Normal;
            //是否保留原像素
            var reserve = false;

            //处理每个图块
            for (int index = 0; index < gif.GraphicControlExtensions.Count; index++)
            {
                LogUtil.Log(string.Format("gif analyze steps at frame {0}.", index));
                //命名
                textureFrame.name = "FrameOfIndex" + index;
                //图像描述器
                var imageDescriptor = gif.ImageDescriptors[index];
                //像素色号集
                var colorIndexs = imageDescriptor.GetColorIndexs();
                //绘图控制扩展
                var graphicControl = gif.GraphicControlExtensions[index];
                //检查背景是否透明
                if (graphicControl.FlagTransparentColor && disposalMethod == DisposalMethod.Bg)
                    textureFrame.SetPixels(textureTransparent.GetPixels());
                //像素指针
                int pixelIndex = 0;
                //gif的像素点顺序 左上到右下,unity的像素顺序是 左下到右上,所以y套x, y翻转一下
                for (int gifY = imageDescriptor.MarginTop; gifY < imageDescriptor.MarginTop + imageDescriptor.Height; gifY++)
                {
                    for (int gifX = imageDescriptor.MarginLeft; gifX < imageDescriptor.MarginLeft + imageDescriptor.Width; gifX++)
                    {
                        Color32 colorPixel = imageDescriptor.GetColor(colorIndexs[pixelIndex++], graphicControl, gif);
                        if (colorPixel.a == 0 && reserve)
                            continue;
                        textureFrame.SetPixel(gifX, gif.Height - gifY - 1, colorPixel);
                    }
                }

                //保存
                textureFrame.wrapMode = TextureWrapMode.Clamp;
                textureFrame.Apply();

                //添加序列帧,并兵初始化Texture
                var sprite = Sprite.Create(textureFrame, new Rect(0, 0, textureFrame.width, textureFrame.height), _pivotCenter);
                sprite.name = textureFrame.name;
                frames.Add(new UnityFrame(sprite, graphicControl.DelaySecond));
                textureFrame = new Texture2D(gif.Width, gif.Height);

                //预处理下一帧图像
                reserve = false;
                disposalMethod = graphicControl.DisposalMethod;
                switch (disposalMethod)
                {
                    //1 - Do not dispose. The graphic is to be left in place. //保留此帧
                    case DisposalMethod.Last:
                        textureFrame.SetPixels(frames[index].Texture.GetPixels());
                        reserve = true;
                        break;

                    //2 - Restore to background color. The area used by the graphic must be restored to the background color. //还原成背景色
                    case DisposalMethod.Bg:
                        textureFrame.SetPixels(textureBackground.GetPixels());
                        break;

                    //3 - Restore to previous. The decoder is required to restore the area overwritten by the graphic with what was there prior to rendering the graphic.//还原成上一帧
                    case DisposalMethod.Previous:
                        textureFrame.SetPixels(frames[index - 1].Texture.GetPixels());
                        reserve = true;
                        break;
                }
            }
            return frames;
        }
        #endregion

        #region 获取UnityFrames
        /// <summary>
        /// 缓存
        /// </summary>
        private static Dictionary<string, List<UnityFrame>> _mapKeyFrames = new Dictionary<string, List<UnityFrame>>();

        /// <summary>
        /// 获取序列帧集
        /// </summary>
        public static List<UnityFrame> GetFrames(byte[] bytes, string cacheKey = null)
        {
            //缓存 cache
            if (!string.IsNullOrEmpty(cacheKey) && _mapKeyFrames.ContainsKey(cacheKey))
                return _mapKeyFrames[cacheKey];

            //解析 analyze
            try
            {
                if (bytes == null || bytes.Length == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("GIF解析发生错误/Gif analysed error");
                    sb.Append("输入了空的文件流/the bytes is empty");
                    Debug.LogError(sb.ToString());
                    return null;
                }

                LogUtil.Log("gif analyze starts.");
                var start = DateTime.Now;

                //解析analyze
                List<UnityFrame> frames = GetFrames(new GraphicsInterchangeFormat(bytes));
                if (!string.IsNullOrEmpty(cacheKey))
                    _mapKeyFrames.Add(cacheKey, frames);

                var end = DateTime.Now;
                LogUtil.Log(string.Format("gif analyze has finished, frames count:{0},cost time:{1} seconds, each costs:{2} seconds.", frames.Count, (end - start).TotalSeconds, (end - start).TotalSeconds / frames.Count));

                return frames;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("GIF解析发生错误/Gif analysed error");
                sb.AppendLine("可能是版本不兼容导致，请用PhotoShop 存储为web所用格式GIF 后重试/Mybe the version invalid , try to convert it by photoshop web format gif");
                sb.Append(ex.Message);
                Debug.LogError(sb.ToString());
                return null;
            }
        }
        #endregion
    }
}