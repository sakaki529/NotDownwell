using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NotDownwell.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NotDownwell
{
    public class Enemy : Entity
    {
        public GameScene game;
        public int type;
        public int score;
        public bool isActive;
        public bool CanStomp;
        public float ai;
        public void Init()
        {
        }
        public void ResetPerFrame()
        {
        }
        public Enemy()
        {
            //初期化等
            Init();
        }
        public void PlayerCollision()
        {
            if (game.player.CheckCollision(this))
            {

            }
        }
        public override void Update()
        {
            game = Main.gameScene;
            if (!isActive)
                return;
            ResetPerFrame();
            oldVelocity = velocity;
            UpdateByID();
            oldPosition = position;
            position += velocity;
            CheckAreaCollide();
            hitBox = new HitBox(position, Size);
            PlayerCollision();
            if (game.player.gameOver == 0 && position.Y < game.player.position.Y - Main.ScreenSize.Y * 1.5f)//ゲームに敵がたまり続けるのを防ぐ
                isActive = false;
        }
        public void UpdateByID()
        {
            switch (type)
            {
                case EnemyID.Mono:
                    {
                    }
                    break;
                case EnemyID.Jelly:
                    {
                    }
                    break;
                case EnemyID.Bat:
                    {
                        float speed = 2f;
                        velocity.X = (float)Math.Cos((float)Math.Atan2(Center.Y - game.player.Center.Y, Center.X - game.player.Center.X)) * -speed;
                        velocity.Y = (float)Math.Sin((float)Math.Atan2(Center.Y - game.player.Center.Y, Center.X - game.player.Center.X)) * -speed;
                    }
                    break;
                case EnemyID.RedEye:
                    {
                        float speed = 2f;
                        ai += 1f;
                        float dist = Vector2.Distance(Center, game.player.Center);
                        if (ai > 0f && dist > 60f)
                        {
                            ai = 0f;
                            Vector2 Velocity = Vector2.Normalize(game.player.Center - Center) * speed;
                            velocity = Vector2.Lerp(velocity, Velocity, 0.0333333351f);
                        }
                        rotation = velocity.ToRotation() + MathHelper.PiOver2;
                    }
                    break;
                default:
                    isActive = false;
                    return;
            }
        }
        public override void CheckAreaCollide()
        {
            base.CheckAreaCollide();
            switch (type)
            {
                case EnemyID.Jelly:
                case EnemyID.Bat:
                    {
                        if (collideX && velocity.X != oldVelocity.X)
                            velocity.X = oldVelocity.X * -1;
                    }
                    break;
                case EnemyID.RedEye:
                    {
                        if (collideX && velocity.X != oldVelocity.X)
                            velocity.X = oldVelocity.X * -2;
                    }
                    break;
                default:
                    return;
            }
        }
        public override void Kill()
        {
            Sound.Stamp.Play(0.5f, 0f, 0f);
            isActive = false;
            Main.gameScene.currentScore += score * ((Main.gameScene.player.combo % 5) + 1);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            /*if (!CanStomp)
                color = Color.Red;*/
            Texture2D texture = AssetHelper.GetTexture("Enemy_" + type);
            spriteBatch.Draw(texture, Center, new Rectangle(0, 0, texture.Width, texture.Height), color, rotation, texture.Size() / 2f, scale, SpriteEffects.None, 0f);
        }
        public static void SpawnEnemy(Vector2 position, Vector2 velocity, int type)
        {
            Enemy enemy = new Enemy();
            enemy.velocity = velocity;
            enemy.position = position;
            enemy.type = type;
            enemy.isActive = true;
            enemy.SetDefaults();
            Main.gameScene.enemies.Add(enemy);
        }
        public void SetDefaults()
        {
            switch (type)
            {
                case EnemyID.Mono:
                    width = 26;
                    heigth = 46;
                    CanStomp = false;
                    score = 10;
                    velocity = -Vector2.UnitY * 3f;
                    break;
                case EnemyID.Jelly:
                    width = 36;
                    heigth = 24;
                    CanStomp = true;
                    score = 10;
                    velocity.Y = Main.rand.NextFloat() * -1.0f - 1.0f;
                    velocity.X = Main.rand.NextFloat() * (Main.rand.NextBool() ? 2f : -2f);
                    break;
                case EnemyID.Bat:
                    width = 24;
                    heigth = 16;
                    CanStomp = true;
                    score = 10;
                    break;
                case EnemyID.RedEye:
                    width = 36;
                    heigth = 36;
                    CanStomp = false;
                    score = 10;
                    break;
                default:
                    isActive = false;
                    return;
            }
            hitBox = new HitBox(position, Size);
        }
    }
}
