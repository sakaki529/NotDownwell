using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NotDownwell.Scenes;
using System;

namespace NotDownwell
{
    public class Player : Entity
    {
        public int gameOver;
        public const int gameOverMax = 180;
        public int gems;
        public int combo;
        public int immune;
        public const int immuneMax = 60;
        //体力
        public const int initLife = 4;
        public int maxLife;
        public int statLife;
        //電力
        public const int initEnergy = 8;
        public int maxEnergy;
        public int statEnergy;
        public bool justShot;
        //ctrl
        public bool controlR;//右方向へのコントロールの確認
        public bool controlL;//左方向へのコントロールの確認
        public bool controlJump;//ジャンプ確認
        public bool isJumping;//ジャンプ中か リセットはしない
        public int jumpTimeMax;
        public int jumpTime;
        public bool justJump;//ジャンプしたフレームか
        public const float defGrav = 0.38f;//0.5
        public float gravity = defGrav;//重力
        public void InitPlayer()
        {
            ResetPlayerPerFrame();
            gameOver = 0;
            gems = 0;
            combo = 0;
            width = 22;
            heigth = 28;
            hitBox = new HitBox(position, Size);
            maxLife = statLife = initLife;
            maxEnergy = statEnergy = initEnergy;
            justShot = false;
            controlR = false;
            controlL = false;
            controlJump = false;
            isJumping = false;
            jumpTimeMax = 5;
            jumpTime = 0;
            gravity = defGrav;
            maxFrame = 4;
        }
        public void ResetPlayerPerFrame()
        {
            collideX = false;
            collideY = false;
            justShot = false;
            controlR = false;
            controlL = false;
            controlJump = false;
            justJump = false;
            gravity = defGrav;
        }
        public Player()
        {
            //初期化等
            InitPlayer();
        }
        public void Control()
        {
            if (gameOver > 0)
                return;
            float accH = 0.6f;
            float accV = 0.4f;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                velocity.X += accH;
                controlR = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity.X -= accH;
                controlL = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && isJumping)//ジャンプ長押し中
            {
                jumpTime++;
                if (jumpTime > jumpTimeMax)
                    isJumping = false;
                velocity.Y -= accV;
            }
            else if (Utils.KeyJustPressed(Keys.Space))
            {
                controlJump = true;
                if (velocity.Y != 0f)
                {
                    if (statEnergy > 0)
                    {
                        Bullet.SpawnBullet(Center + Vector2.UnitY * (heigth / 2), Vector2.UnitY * 8f);
                        Sound.Jump.Play(0.1f, 0f, 0f);
                        velocity.Y = -2f;
                        statEnergy--;
                        justShot = true;
                    }
                    else
                    {
                        //電力が無い場合
                    }
                }
                else
                {
                    velocity.Y = -2f;
                    justJump = true;
                    isJumping = true;
                }
            }
            else//ジャンプ操作をしていない状態
            {
                //jumpTime = 0;
                isJumping = false;
            }
            /*if (!isJumping)
                jumpTime = MathHelper.Clamp(jumpTime - 1, 0, jumpTimeMax);*/
        }
        public override void Update()
        {
            if (gameOver > 0)
                gameOver++;
            else
            {
                //到達深度
                if (position.Y > Main.gameScene.maxDepth)
                    Main.gameScene.maxDepth = position.Y;
            }
            ResetPlayerPerFrame();
            //stat
            if (immune > 0)//無敵時間の減少
                immune--;
            //playerUpdate
            oldVelocity = velocity;
            Control();
            oldPosition = position;
            position += velocity;
            CheckAreaCollide();
            //velocityUpdate
            //重力
            if (!controlJump && !isJumping && !justShot)
                velocity.Y = MathHelper.Min(velocity.Y + gravity, 9f);//落下速度のキャップ 12
            //減速
            if (!controlR && velocity.X > 0f)
                velocity.X *= 0.94f;
            if (!controlL && velocity.X < 0f)
                velocity.X *= 0.94f;
            if (velocity.X < 0.5f && velocity.X > -0.5f)
                velocity.X = 0f;
            hitBox = new HitBox(position, Size);
            /*if (position.Y > Main.ScreenSize.Y)
            {
                statLife = 0;
                gameOver++;
            }*/
            if (collideY)
            {
                velocity.Y = 0f;
                jumpTime = 0;
                isJumping = false;
                statEnergy = maxEnergy;
            }
        }
        public override void CheckAreaCollide()
        {
            if (Main.currentScene == Main.firstScene)//スタート画面のみ特殊処理
            {
                if (position.Y + heigth >= FirstScene.BorderY)
                {
                    if ((position.X < GameScene.BorderL || position.X + width > GameScene.BorderR) && oldPosition.Y + heigth <= FirstScene.BorderY)
                    {
                        collideY = true;
                        velocity.Y = 0f;
                        position.Y = FirstScene.BorderY - heigth;
                    }
                    else
                    {
                        if (position.X < GameScene.BorderL)
                        {
                            collideX = true;
                            velocity.X = 0f;
                            position.X = GameScene.BorderL;
                        }
                        if (position.X + width > GameScene.BorderR)
                        {
                            collideX = true;
                            velocity.X = 0f;
                            position.X = GameScene.BorderR - width;
                        }
                    }
                }
                //画面端
                if (position.X <= 0f)
                {
                    collideX = true;
                    velocity.X = 0f;
                    position.X = 0f;
                }
                if (position.X + width >= Main.ScreenSize.X)
                {
                    collideX = true;
                    velocity.X = 0f;
                    position.X = Main.ScreenSize.X - width;
                }
                return;
            }
            base.CheckAreaCollide();
        }
        public override bool CheckCollision(Entity target)
        {
            if (gameOver > 0)
                return false;
            if (target is Enemy)
            {
                bool damaged = false;
                Enemy enemy = target as Enemy;
                //TODO: 踏みつけ直前にジャンプし、下方向の敵から衝突されるとダメージを受ける不具合の修正
                //if (oldPosition.Y < position.Y)//落下中の場合
                {
                    float dif = position.Y - oldPosition.Y;
                    HitBox hb = new HitBox(oldPosition, Size);
                    //すり抜け防止のため、移動した区間の判定を補完
                    for (float j = oldPosition.Y; j <= position.Y; j++)
                    {
                        hb.X = MathHelper.Lerp(position.X, oldPosition.X, (position.Y - j) / dif);//xの補完
                        hb.Y++;//判定のy座標変更
                        if (hb.Intersect(target.hitBox))
                        {
                            //Debug.WriteLine(hb.Y + heigth + "/"+ enemy.position.Y);
                            if (enemy.CanStomp && hb.Y <= enemy.Center.Y)//踏みつけ
                            {
                                Main.gameScene.gameCamera.SetShake();
                                statEnergy = maxEnergy;
                                combo++;
                                velocity.Y = -4f;
                                enemy.Kill();
                                return false;
                            }
                            else if (immune <= 0)
                                damaged = true;
                        }
                    }
                }
                /*if (hitBox.Intersect(target.hitBox))
                {
                    if (immune <= 0)
                        damaged = true;
                }*/
                if (damaged)
                {
                    Main.gameScene.gameCamera.SetShake(20, 4);
                    Sound.Hurt.Play(0.1f, 0f, 0f);
                    combo = 0;
                    immune = immuneMax;
                    if (--statLife <= 0)
                    {
                        Main.gameScene.gameCamera.SetShake(60, 12);
                        statLife = 0;
                        //ゲームオーバー
                        gameOver++;
                    }
                    return true;
                }
            }
            return false;
        }
        public override void FrameUpdate()
        {
            if (velocity != Vector2.Zero)
            {
                //移動中
                if (velocity.Y == 0f)
                {
                    //地上平行移動
                    if (++frameCounter > 4)
                    {
                        frameCounter = 0;
                        if (++frame >= maxFrame)
                            frame = 0;
                    }
                }
                else
                {
                    frame = 1;//落下中のフレームは固定
                    if (velocity.Y < -0.5f)//ジャンプ中
                        frame = 2;
                }
            }
            else
            {
                //棒立ち状態
                if (++frameCounter > 8)
                {
                    frameCounter = 0;
                    if (++frame >= maxFrame)
                        frame = 0;
                }
            }
            if (Math.Abs(velocity.X) >= 1)
                direction = Math.Sign(velocity.X);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = immune > 0 ? Color.LightYellow : Color.White;
            Texture2D texture = velocity != Vector2.Zero ? AssetHelper.GetTexture("Player_move") : AssetHelper.GetTexture("Player_idle");
            int frameHeight = texture.Height / maxFrame;//フレーム毎の高さ
            Rectangle rectangle = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
            spriteBatch.Draw(texture, Center, rectangle, color, rotation, rectangle.Size.ToVector2() / 2f, scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
