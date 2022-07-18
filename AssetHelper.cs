using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NotDownwell
{
    /// <summary>
    /// アセット関連のあれこれを便利にするためのメソッドなどを格納
    /// </summary>
    class AssetHelper
    {
        /// <summary>
        /// テクスチャを簡単に取得するためのメソッド
        /// </summary>
        /// <param name="pass">Assets/Textures/以下のファイルパス</param>
        /// <returns>指定のTexture2Dクラスのテクスチャ</returns>
        /// <exception cref="ContentLoadException">指定テクスチャが発見できなかった場合</exception>
        public static Texture2D GetTexture(string pass)
        {
            //filestreamを用いた方法であればpngのまま読み込むことも可能
            //https://community.monogame.net/t/loading-png-jpg-etc-directly/7403/2
            /*FileStream fileStream = new FileStream("Content/Assets/Textures/" + pass + ".png", FileMode.Open);
            Texture2D texture = Texture2D.FromStream(Main.instance.GraphicsDevice, fileStream);
            fileStream.Dispose();
            return texture;*/
            //trycatchでエラー処理指定テクスチャが発見できなかった場合は空のテクスチャを返す
            try
            {
                return Main.instance.Content.Load<Texture2D>("Textures/" + pass);//xnbファイルを読み取る
            }
            catch (ContentLoadException error)
            {
                Debug.WriteLine(error.ToString());
                return Main.instance.Content.Load<Texture2D>("Textures/Blank");
            }
        }
        public static Texture2D GetBlankTex() => Main.instance.Content.Load<Texture2D>("Textures/Blank");
    }
}
