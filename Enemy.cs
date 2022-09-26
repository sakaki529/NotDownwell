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
        public int endure = 2;//弾丸耐久力
        public float kb;//弾丸ヒット時のノックバックの強さ
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
            if (game.player.gameOver == 0 && (position.Y < game.player.position.Y - Main.ScreenSize.Y * 1.5f || position.Y > game.player.position.Y + Main.ScreenSize.Y * 1.5f))//ゲームに敵がたまり続けるのを防ぐ
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
                        direction = Math.Sign(velocity.X);
                    }
                    break;
                case EnemyID.RedEye:
                    {
                        float speed = 2f;
                        float dist = Vector2.Distance(Center, game.player.Center);
                        if (++ai > 0f && dist > 60f)
                        {
                            ai = 0f;
                            Vector2 Velocity = Vector2.Normalize(game.player.Center - Center) * speed;
                            velocity = Vector2.Lerp(velocity, Velocity, 0.0333333351f);
                        }
                        rotation = velocity.ToRotation() + MathHelper.PiOver2;
                    }
                    break;
                case EnemyID.Squid:
                    {
                        if (++ai <= 60)
                        {
                            if (velocity.Y < 1.5f)
                                velocity.Y += 0.05f;
                            if (ai == 60)
                                velocity.Y = -4;
                        }
                        else
                        {
                            velocity *= 0.95f;
                            if (Math.Abs(velocity.Y) < 0.08f)
                                ai = 0;
                        }
                        if (position.Y < game.player.position.Y - Main.ScreenSize.Y * 0.5f)
                        {
                            isActive = false;
                            SpawnEnemy(position, Vector2.UnitY * 4.5f, EnemyID.SquidDrop);
                        }
                        rotation = velocity.ToRotation() + MathHelper.PiOver2;
                    }
                    break;
                case EnemyID.SquidDrop:
                    {
                        velocity = Vector2.UnitY * 4.5f;
                        rotation = velocity.ToRotation() - MathHelper.PiOver2;
                    }
                    break;
                case EnemyID.ManOWar:
                    {
                        int dir = position.X < game.player.position.X ? 1 : -1;
                        if (ai == 0)
                        {
                            ai = -90;
                            velocity = new Vector2(dir * 8, -6f);
                        }
                        else
                        {
                            velocity *= 0.97f;
                        }
                        ai++;
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
                case EnemyID.ManOWar:
                    {
                        velocity.X = 0f;
                    }
                    break;
                default:
                    return;
            }
        }
        public override bool CheckCollision(Entity target)
        {
            if (target is Bullet)
            {
                Bullet bullet = target as Bullet;
                if (hitBox.Intersect(bullet.hitBox))
                {
                    bullet.Kill();
                    velocity += Vector2.UnitY * kb;
                    if (--endure <= 0)
                    {
                        game.player.combo++;
                        Kill();
                    }
                    else
                        Sound.Hurt.Play(0.1f, 0f, 0f);
                }
            }
            return base.CheckCollision(target);
        }
        public override void Kill()
        {
            Sound.Stamp.Play(0.1f, 0f, 0f);
            isActive = false;
            Main.gameScene.currentScore += score * ((Main.gameScene.player.combo % 5) + 1);
        }
        public override void FrameUpdate()
        {
            switch (type)
            {
                case EnemyID.Bat:
                    if (++frameCounter > 8)
                    {
                        frameCounter = 0;
                        if (++frame >= maxFrame)
                            frame = 0;
                    }
                    break;
                case EnemyID.Squid:
                case EnemyID.SquidDrop:
                    if (type == EnemyID.SquidDrop)
                        frameCounter += 1;
                    if (++frameCounter > 10)
                    {
                        frameCounter = 0;
                        if (++frame >= maxFrame)
                            frame = 0;
                    }
                    break;
                case EnemyID.ManOWar:
                    if (++frameCounter > 15)
                    {
                        frameCounter = 0;
                        if (++frame >= maxFrame)
                            frame = 0;
                    }
                    break;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            /*if (!CanStomp)
                color = Color.Red;*/
            Texture2D texture = AssetHelper.GetTexture("Enemy_" + type);
            int frameHeight = texture.Height / maxFrame;//フレーム毎の高さ
            Rectangle rectangle = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
            spriteBatch.Draw(texture, Center, rectangle, color, rotation, rectangle.Size.ToVector2() / 2f, scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
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
                    heigth = 42;
                    CanStomp = false;
                    score = 10;
                    velocity = -Vector2.UnitY * 3f;
                    break;
                case EnemyID.Jelly:
                    width = 36;
                    heigth = 24;
                    kb = 2f;
                    CanStomp = true;
                    score = 10;
                    velocity.Y = Main.rand.NextFloat() * -1.0f - 1.0f;
                    velocity.X = Main.rand.NextFloat() * (Main.rand.NextBool() ? 2f : -2f);
                    break;
                case EnemyID.Bat:
                    width = 32;
                    heigth = 24;
                    kb = 3f;
                    CanStomp = true;
                    score = 10;
                    maxFrame = 4;
                    break;
                case EnemyID.RedEye:
                    width = 36;
                    heigth = 36;
                    kb = 2f;
                    endure = 3;
                    CanStomp = false;
                    score = 20;
                    break;
                case EnemyID.Squid:
                case EnemyID.SquidDrop:
                    width = 24;
                    heigth = 40;
                    CanStomp = type == EnemyID.Squid;
                    score = 10;
                    maxFrame = 8;
                    if (type == EnemyID.Squid)
                        velocity = -Vector2.UnitY * 5f;
                    else
                        endure = 1;
                    break;
                case EnemyID.ManOWar:
                    scale = 1.2f;
                    width = (int)(24 * scale);
                    heigth = (int)(24 * scale);
                    CanStomp = false;
                    score = 20;
                    maxFrame = 3;
                    velocity = Vector2.One * 5f;
                    break;
                default:
                    isActive = false;
                    return;
            }
            hitBox = new HitBox(position, Size);
        }
    }
}
