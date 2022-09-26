using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NotDownwell
{
    static class Extensions
    {
        public static Vector2 Size(this Texture2D texture) => new Vector2(texture.Width, texture.Height);
        public static float ToRotation(this Vector2 v) => (float)Math.Atan2(v.Y, v.X);
        /// <summary>
        /// 特定のベクトルをrad分回転させる
        /// </summary>
        public static Vector2 RotatedBy(this Vector2 vector, double rad)
        {
            float cos = (float)Math.Cos(rad), sin = (float)Math.Sin(rad);
            return new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
        }
        public static bool NextBool(this Random random, int denominator = 2) => random.Next(denominator) == 0;
        public static float NextFloat(this Random random, float maxValue = 1) => (float)random.NextDouble() * maxValue;
        public static float NextFloat(this Random random, float minValue, float maxValue) => minValue + (float)random.NextDouble() * (maxValue - minValue);
    }
    class Utils
    {
        /// <summary>
        /// 指定のキーが押された瞬間であればtrueを返します
        /// </summary>
        /// <param name="key">該当のキーボード</param>
        /// <returns>指定のキーが押された瞬間かどうか</returns>
        public static bool KeyJustPressed(Keys key) => Keyboard.GetState().IsKeyDown(key) && Main.OldKeyState.IsKeyUp(key);
        /// <summary>
        /// 指定のキーが離された瞬間であればtrueを返します
        /// </summary>
        /// <param name="key">該当のキーボード</param>
        /// <returns>指定のキーが離された瞬間かどうか</returns>
        public static bool KeyJustReleased(Keys key) => Keyboard.GetState().IsKeyUp(key) && Main.OldKeyState.IsKeyDown(key);
    }
}
