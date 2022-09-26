using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NotDownwell.Scenes;
using System.Diagnostics;

namespace NotDownwell
{
    public class Bullet : Entity
    {
        public bool isActive;
        public int timeLeft;
        public void Init()
        {
            width = heigth = 22;
            scale = 0.9f;
            timeLeft = 20;
        }
        public Bullet()
        {
            //初期化等
            Init();
        }
        public override void Update()
        {
            position += velocity;
            hitBox = new HitBox(position, Size);
            if (--timeLeft <= 0)
                Kill();
        }
        public override void Kill()
        {
            isActive = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            Texture2D texture = AssetHelper.GetTexture("Bullet");
            int frameHeight = texture.Height / maxFrame;//フレーム毎の高さ
            Rectangle rectangle = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
            spriteBatch.Draw(texture, Center, rectangle, color, rotation, rectangle.Size.ToVector2() / 2f, scale, SpriteEffects.None, 0f);
        }
        public static void SpawnBullet(Vector2 position, Vector2 velocity)
        {
            Bullet bullet = new Bullet();
            bullet.velocity = velocity;
            bullet.Center = position;
            bullet.isActive = true;
            if (Main.currentScene is GameScene)
                Main.gameScene.bullets.Add(bullet);
            if (Main.currentScene is FirstScene)
                Main.firstScene.bullets.Add(bullet);
        }
    }
}
