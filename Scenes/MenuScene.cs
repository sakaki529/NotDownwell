using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NotDownwell.Scenes
{
    public class MenuScene : Scene
    {
        public int selectedMenuItem;
        public const int menuItems = 3;
        public override void DoInit()
        {
            selectedMenuItem = 0;
        }
        public override void DoExit()
        {
            Sound.Sel.Play(0.5f, 0f, 0f);
            MoveScene(this, Main.gameScene);
            selectedMenuItem = 0;
        }
        public override void Update()
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
                    case 1://return
                        DoExit();
                        break;
                    case 2://retry
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
        public override void Draw()
        {
            Main.DrawShadowedString(Main.hudFont, "Exit ", Main.ScreenSize / 2 - Vector2.UnitY * 48f, selectedMenuItem == 0 ? Color.White : Color.Gray, selectedMenuItem == 0 ? 1.2f : 1.0f);
            Main.DrawShadowedString(Main.hudFont, "Return Game ", Main.ScreenSize / 2, selectedMenuItem == 1 ? Color.White : Color.Gray, selectedMenuItem == 1 ? 1.2f : 1.0f);
            Main.DrawShadowedString(Main.hudFont, "Retry ", Main.ScreenSize / 2 + Vector2.UnitY * 48f, selectedMenuItem == 2 ? Color.White : Color.Gray, selectedMenuItem == 2 ? 1.2f : 1.0f);
            //spriteBatch.Draw(AssetHelper.GetTexture("Placeholder"), ScreenSize / 2f - Vector2.UnitY * 64f, null, Color.Red, 0f, AssetHelper.GetTexture("Placeholder").Size() / 2f, 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(AssetHelper.GetTexture("Placeholder"), ScreenSize / 2f + Vector2.UnitY * 64f, null, Color.Blue, 0f, AssetHelper.GetTexture("Placeholder").Size() / 2f, 1f, SpriteEffects.None, 0f);
        }
    }
}
