using Microsoft.Xna.Framework;
using System;

namespace NotDownwell
{
    public class Camera
    {
        public Vector2 Position => new Vector2(-Main.ScreenSize.X / 2f, -Math.Max(Main.gameScene.maxDepth, Main.gameScene.player.position.Y) - (Main.gameScene.player.heigth / 2));
        public Matrix Transform { get; private set; }
        public void Follow(Player player)
        {
            if (player.gameOver > 0)
                return;
            //var position = Matrix.CreateTranslation(-entity.position.X - (entity.width / 2), -entity.position.Y - (entity.heigth / 2), 0);
            var position = Matrix.CreateTranslation(Position.X, Position.Y, 0);
            var offset = Matrix.CreateTranslation(Main.ScreenSize.X / 2f, Math.Min(player.position.Y, Main.ScreenSize.Y / 2f), 0);
            Transform = position * offset;
        }
    }
}
