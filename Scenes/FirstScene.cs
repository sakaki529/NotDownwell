using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotDownwell.Scenes
{
    public class FirstScene : Scene
    {
        public static float BorderY => Main.ScreenSize.Y / 2f + 64f;
        public Player player;
        public int startTimer;
        public const int startTimerMax = 90;
        public override void DoInit()
        {
            player = new Player();
        }
        public override void DoExit()
        {
        }
        public override void Update()
        {
            player.Update();
            if (startTimer > 0)
                startTimer++;
            else if (player.position.Y > Main.ScreenSize.Y + 80f)
                startTimer++;
            if (startTimer >= startTimerMax)
            {
                MoveScene(this, Main.gameScene);
                DoExit();
            }
        }
        public override void Draw()
        {
            player.Draw(Main.spriteBatch);
            DrawGameUI();
        }
        public void DrawGameUI()
        {
            Vector2 pos;
            Texture2D texture;
            //枠
            {
                texture = AssetHelper.GetTexture("Pixel");
                pos = new Vector2(GameScene.BorderL - 8, BorderY);
                Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, 8, (int)Main.ScreenSize.Y), Color.White);
                pos = new Vector2(GameScene.BorderR, BorderY);
                Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, 8, (int)Main.ScreenSize.Y), Color.White);
                //x
                pos = new Vector2(0, BorderY);
                Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, (int)GameScene.BorderL - 8, 8), Color.White);
                pos = new Vector2(GameScene.BorderR, BorderY);
                Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, (int)GameScene.BorderL + 8, 8), Color.White);
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
            //メッセージ
            {
                pos = Main.ScreenSize / 2f;
                pos.Y -= Main.ScreenSize.Y * 0.1f;
                string msg = "Jump into the hole and start game";
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 4, Color.Black, 1.4f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 4, Color.Black, 1.4f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 2, Color.Red, 1.4f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 2, Color.Red, 1.4f);
                Main.DrawShadowedString(Main.hudFont, msg, pos, Color.White, 1.4f);
            }
            //フェード
            texture = AssetHelper.GetTexture("Pixel");
            Main.spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, (int)Main.ScreenSize.X, (int)Main.ScreenSize.Y), Color.Lerp(Color.Transparent, Color.Black, startTimer / (startTimerMax - 30f)));
        }
    }
}
