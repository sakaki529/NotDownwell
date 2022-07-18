using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NotDownwell.Scenes;

namespace NotDownwell
{
    public class Player : Entity
    {
        public int gameOver;
        public const int gameOverMax = 180;
        public int gems;
        public int combo;
        public int immune;
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
            width = 24;
            heigth = 32;
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
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                velocity.X += 0.6f;
                controlR = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity.X -= 0.6f;
                controlL = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && isJumping)//ジャンプ長押し中
            {
                jumpTime++;
                if (jumpTime > jumpTimeMax)
                    isJumping = false;
                velocity.Y -= 0.4f;
            }
            else if (Utils.KeyJustPressed(Keys.Space))
            {
                controlJump = true;
                if (velocity.Y != 0f)
                {
                    if (statEnergy > 0)
                    {
                        Sound.Jump.Play(0.5f, 0f, 0f);
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
                if (oldPosition.Y < position.Y)//落下中の場合
                {
                    float dif = position.Y - oldPosition.Y;
                    HitBox hb = new HitBox(oldPosition, Size);
                    //すり抜け防止のため、移動した区間の判定を補完
                    for (float j = oldPosition.Y; j <= position.Y; j++)
                    {
                        hb.X = MathHelper.Lerp(position.X, oldPosition.X, (position.Y - j) / dif);
                        hb.Y++;//判定のy座標のみ変更
                        if (hb.Intersect(target.hitBox))
                        {
                            //Debug.WriteLine(hb.Y + heigth + "/"+ enemy.position.Y);
                            if (enemy.CanStomp && hb.Y <= enemy.position.Y)//踏みつけ
                            {
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
                else if (hitBox.Intersect(target.hitBox))
                {
                    if (immune <= 0)
                        damaged = true;
                }
                if (damaged)
                {
                    Sound.Hurt.Play(0.5f, 0f, 0f);
                    combo = 0;
                    immune = 60;
                    if (--statLife <= 0)
                    {
                        statLife = 0;
                        //ゲームオーバー
                        gameOver++;
                    }
                    return true;
                }
            }
            return false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = immune > 0 ? Color.LightYellow : Color.White;
            Texture2D texture = AssetHelper.GetTexture("Placeholder");
            spriteBatch.Draw(texture, Center, new Rectangle(0, 0, width, heigth), color, rotation, Size / 2f, scale, SpriteEffects.None, 0f);
        }
    }
}
