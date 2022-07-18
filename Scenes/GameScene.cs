using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace NotDownwell.Scenes
{
    public class GameScene : Scene
    {
        public static float AreaSize => 416;
        public static float BorderL => Main.ScreenSize.X / 2f - AreaSize / 2f;
        public static float BorderR => Main.ScreenSize.X / 2f + AreaSize / 2f;
        public Camera gameCamera;
        public Player player;
        public List<Enemy> enemies;
        public float maxDepth;//最大深度
        public int currentScore;//現在のスコア
        public int gameTime;//ゲーム中のタイマー
        public override void DoInit()
        {
            gameTime = 0;
            player = new Player() { position = new Vector2(Main.ScreenSize.X / 2f, 0f) };//ゲームを通して使用するプレイヤーの宣言
            enemies = new List<Enemy>();
            gameCamera = new Camera();
            maxDepth = 0;
            currentScore = 0;
        }
        public override void DoExit()
        {
        }
        public override void Update()
        {
            if (player.gameOver > Player.gameOverMax)
            {
                Main.resultScene.DoInit();
                MoveScene(this, Main.resultScene);
                return;
            }
            gameTime++;
            if (Utils.KeyJustPressed(Keys.Escape))
                MoveScene(this, Main.menuScene);
            if (gameTime % 30 == 0 && gameTime > 120)
            {
                Enemy.SpawnEnemy(new Vector2(Main.rand.NextFloat(BorderL, BorderR), player.position.Y + Main.ScreenSize.Y + (Main.ScreenSize.Y * 0.2f * Main.rand.NextFloat())),Vector2.Zero, Main.rand.Next(1, 5));
            }
            player.Update();
            enemies = enemies.Where(e => e.isActive).ToList();//アクティブなものだけを残す
            enemies.ForEach(e => e.Update());
            gameCamera.Follow(player);
        }
        public override void Draw()
        {
            DrawGameEntity();
            DrawGameUI();
        }
        public void DrawGameEntity()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(transformMatrix: gameCamera.Transform);
            //spriteBatch.Draw(AssetHelper.GetTexture("Placeholder"), Vector2.Zero, Color.Red);
            player.Draw(Main.spriteBatch);
            enemies.ForEach(e => e.Draw(Main.spriteBatch));
            //コンボ数
            if (player.combo >= 5)
            {
                Vector2 pos = player.Center - Vector2.UnitY * player.heigth;
                Main.DrawShadowedString(Main.hudFont, player.combo.ToString(), pos + Vector2.UnitY * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.combo.ToString(), pos + Vector2.UnitX * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.combo.ToString(), pos + Vector2.UnitY * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.combo.ToString(), pos + Vector2.UnitX * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.combo.ToString(), pos, Color.White, 1.2f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }
        public void DrawGameUI()
        {
            Vector2 pos;
            Texture2D texture;
            //枠
            {
                texture = AssetHelper.GetTexture("Pixel");
                pos = new Vector2(BorderL - 8, 0);
                Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, 8, (int)Main.ScreenSize.Y), Color.White);
                pos = new Vector2(BorderR, 0);
                Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, 8, (int)Main.ScreenSize.Y), Color.White);
            }
            //体力
            {
                pos = Vector2.One * 16f;
                texture = AssetHelper.GetTexture("HealthBar");
                Main.spriteBatch.Draw(texture, pos, Color.White);
                texture = AssetHelper.GetTexture("HealthBar_fill");
                Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, 4 + (int)((texture.Width - 8) * ((float)player.statLife / player.maxLife)), texture.Height), Color.White);
                //黒のアウトラインが付いたような文字を描画
                Main.DrawShadowedString(Main.hudFont, player.statLife + " / " + player.maxLife, pos + texture.Size() / 2f + Vector2.UnitY * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.statLife + " / " + player.maxLife, pos + texture.Size() / 2f + Vector2.UnitX * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.statLife + " / " + player.maxLife, pos + texture.Size() / 2f, Color.White, 1.2f);
            }
            //電力
            {
                texture = AssetHelper.GetTexture("EnergyBar");
                Vector2 eBarPos = new Vector2(Main.ScreenSize.X * 0.85f, Main.ScreenSize.Y / 2f - texture.Height / 2f);//ゲージの左上の位置
                pos = eBarPos;
                Main.spriteBatch.Draw(texture, pos, Color.White);
                eBarPos += new Vector2(0, texture.Height) + Vector2.One * texture.Width / 2f;//文字を描画する位置を代入。テクスチャ依存の位置なのでここで。
                pos += new Vector2(6f, texture.Height - 6f);//残量の描画位置
                texture = AssetHelper.GetTexture("EnergyBar_fill");
                for (int i = 0; i < player.statEnergy; i++)
                {
                    pos.Y -= texture.Height;
                    Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);
                }
                pos = eBarPos;
                //赤と黒のアウトラインが付いたような文字を描画
                Main.DrawShadowedString(Main.hudFont, player.statEnergy.ToString(), pos + Vector2.UnitY * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.statEnergy.ToString(), pos + Vector2.UnitX * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.statEnergy.ToString(), pos + Vector2.UnitY * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.statEnergy.ToString(), pos + Vector2.UnitX * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, player.statEnergy.ToString(), pos, Color.White, 1.2f);
            }
            //スコア
            {
                texture = AssetHelper.GetTexture("Pixel");
                Vector2 barPos = new Vector2(Main.ScreenSize.X * 0.8f, Main.ScreenSize.Y * 0.1f);
                pos = barPos;
                Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, 64, 4), Color.White);
                pos.X += 32f;
                pos.Y -= 12f;
                //赤と黒のアウトラインが付いたような文字を描画
                Main.DrawShadowedString(Main.hudFont, currentScore.ToString(), pos + Vector2.UnitY * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, currentScore.ToString(), pos + Vector2.UnitX * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, currentScore.ToString(), pos + Vector2.UnitY * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, currentScore.ToString(), pos + Vector2.UnitX * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, currentScore.ToString(), pos, Color.White, 1.2f);
            }
            //フェード
            texture = AssetHelper.GetTexture("Pixel");
            Main.spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, (int)Main.ScreenSize.X, (int)Main.ScreenSize.Y), Color.Lerp(Color.Transparent, Color.Black, (player.gameOver - 60f) / (Player.gameOverMax - 60f)));
        }
    }
}
