using Microsoft.Xna.Framework;
using System;

namespace NotDownwell
{
    public class Camera
    {
        public Vector2 Position => new Vector2(-Main.ScreenSize.X / 2f, -Math.Max(Main.gameScene.maxDepth, Main.gameScene.player.position.Y) - (Main.gameScene.player.heigth / 2) - 60f);
        public Matrix Transform { get; private set; }
        private int shakeTime;//振動の時間
        private float shakeIntencsity;//振動の強さ
        public void SetShake(int time = 10, float intensity = 3)
        {
            shakeTime = time;
            shakeIntencsity = intensity;
        }
        public void Follow(Player player)
        {
            if (player.gameOver > 0)
                return;
            Vector2 camPos = Position;
            if (shakeTime > 0)
            {
                shakeTime--;
                camPos += new Vector2(Main.rand.NextFloat(-shakeIntencsity, shakeIntencsity), Main.rand.NextFloat(-shakeIntencsity, shakeIntencsity));//カメラの位置を微妙にずらし、振動の表現
            }
            //var position = Matrix.CreateTranslation(-entity.position.X - (entity.width / 2), -entity.position.Y - (entity.heigth / 2), 0);
            var position = Matrix.CreateTranslation(camPos.X, camPos.Y, 0);
            var offset = Matrix.CreateTranslation(Main.ScreenSize.X / 2f, Math.Min(player.position.Y, Main.ScreenSize.Y / 2f), 0);
            Transform = position * offset;
        }
    }
}
