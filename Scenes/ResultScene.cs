using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NotDownwell.Scenes
{
    public class ResultScene : Scene
    {
        public int selectedMenuItem;
        public const int menuItems = 2;
        public int timer;
        public override void DoInit()
        {
            selectedMenuItem = 0;
            timer = 0;
        }
        public override void DoExit()
        {
            DoInit();
            Main.gameScene.DoInit();
            MoveScene(this, Main.gameScene);
            selectedMenuItem = 0;
        }
        public override void Update()
        {
            timer++;
            if (Utils.KeyJustPressed(Keys.Space))
            {
                if (timer < 60)
                    timer = 60;
                else if (timer < 120)
                    timer = 120;
                else if (timer < 140)
                    timer = 140;
                else if (timer < 160)
                    timer = 160;
                else if (timer < 220)
                    timer = 220;
                else if (timer < 340)
                    timer = 340;
            }
            if (timer == 60)
                Sound.Stamp.Play(0.5f, 0f, 0f);
            if (timer == 120 || timer == 160)
                Sound.Hurt.Play(0.5f, 0f, 0f);
            if (timer == 220)
                Sound.Jump.Play(0.5f, 0f, 0f);
            if (timer > 340)
            {
                if (Utils.KeyJustPressed(Keys.Escape))
                {
                    DoExit();
                }
                if (Utils.KeyJustPressed(Keys.Space))
                {
                    switch (selectedMenuItem)
                    {
                        case 0:
                            Main.instance.DoExit();
                            break;
                        case 1://retry
                            Sound.Sel.Play(0.5f, 0f, 0f);
                            Main.gameScene.DoInit();
                            DoExit();
                            break;
                    }
                }
                if (Utils.KeyJustPressed(Keys.W))
                {
                    selectedMenuItem--;
                    if (selectedMenuItem < 0)
                        selectedMenuItem = menuItems - 1;
                }
                if (Utils.KeyJustPressed(Keys.S))
                {
                    selectedMenuItem++;
                    if (selectedMenuItem >= menuItems)
                        selectedMenuItem = 0;
                }
            }
        }
        public override void Draw()
        {
            Vector2 pos = Vector2.Zero;
            string msg;
            float score = Main.gameScene.currentScore;
            float depth = Main.gameScene.maxDepth / 100f;
            //時間経過に応じて結果の描画
            if (timer >= 60)//gameover
            {
                pos = Main.ScreenSize / 2f - Vector2.UnitY * Main.ScreenSize.Y * 0.4f;
                msg = "RESULT";
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 4, Color.Black, 1.6f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 4, Color.Black, 1.6f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 2, Color.Red, 1.6f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 2, Color.Red, 1.6f);
                Main.DrawShadowedString(Main.hudFont, msg, pos, Color.White, 1.6f);
            }
            if (timer >= 120)//score
            {
                pos.Y += Main.ScreenSize.Y * 0.2f;
                msg = "SCORE: " + score;
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, msg, pos, Color.White, 1.2f);
            }
            if (timer >= 140)//*
            {
                pos.Y += Main.ScreenSize.Y * 0.075f;
                msg = "*";
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 2, Color.Black, 1.0f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 2, Color.Black, 1.0f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 1, Color.Red, 1.0f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 1, Color.Red, 1.0f);
                Main.DrawShadowedString(Main.hudFont, msg, pos, Color.White, 1.0f);
            }
            if (timer >= 160)//depth
            {
                pos.Y += Main.ScreenSize.Y * 0.05f;
                msg = "DEPTH: " + (int)depth + " M";
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 2, Color.Black, 1.2f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 1, Color.Red, 1.2f);
                Main.DrawShadowedString(Main.hudFont, msg, pos, Color.White, 1.2f);
            }
            if (timer >= 220)//res
            {
                pos.Y += Main.ScreenSize.Y * 0.1f;
                msg = "FINAL SCORE: " + (int)(depth * (score / 10f));
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 2, Color.Black, 1.3f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 2, Color.Black, 1.3f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitY * 1, Color.Red, 1.3f);
                Main.DrawShadowedString(Main.hudFont, msg, pos + Vector2.UnitX * 1, Color.Red, 1.3f);
                Main.DrawShadowedString(Main.hudFont, msg, pos, Color.White, 1.3f);
                int xOff = 240;
                Main.spriteBatch.Draw(AssetHelper.GetTexture("Pixel"), pos + Vector2.UnitY * 16f - Vector2.UnitX * xOff, new Rectangle(0, 0, xOff * 2, 2), Color.White);
            }
            if (timer >= 340)//menu
            {
                Main.DrawShadowedString(Main.hudFont, "Exit ", Main.ScreenSize / 2f + Vector2.UnitY * 80f, selectedMenuItem == 0 ? Color.White : Color.Gray, selectedMenuItem == 0 ? 1.2f : 1.0f);
                Main.DrawShadowedString(Main.hudFont, "Retry ", Main.ScreenSize / 2f + Vector2.UnitY * 128f, selectedMenuItem == 1 ? Color.White : Color.Gray, selectedMenuItem == 1 ? 1.2f : 1.0f);
            }
        }
    }
}
