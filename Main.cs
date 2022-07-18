using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NotDownwell.Scenes;
using System;

namespace NotDownwell
{
    public class Main : Game
    {
        public static Main instance;//アクセシビリティの向上
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice GraphicDeveice => graphics.GraphicsDevice;
        public static SpriteBatch spriteBatch;
        public static Random rand;
        //scene
        public static Scene currentScene;
        public static FirstScene firstScene;
        public static MenuScene menuScene;
        public static GameScene gameScene;
        public static ResultScene resultScene;
        //asset
        public static SpriteFont hudFont;
        //
        public static int GlobalTime;
        public static Vector2 ScreenSize => new Vector2(GraphicDeveice.Viewport.Width, GraphicDeveice.Viewport.Height);
        public static KeyboardState OldKeyState;//キーボードの状態を保存
        public Main()
        {
            instance = this;//起動時に自身をinstanceに格納することで外からアクセスしやすく
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/Assets";
            IsMouseVisible = false;
        }
        public void DoExit() => Exit();//ゲームの終了
        public void DoInit() => Initialize();//ゲームの初期化
        public void InitScenes()
        {
            firstScene = new FirstScene();
            menuScene = new MenuScene();
            gameScene = new GameScene();
            resultScene = new ResultScene();
        }
        protected override void Initialize()
        {
            InitScenes();//シーン初期化
            currentScene = firstScene;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hudFont = Content.Load<SpriteFont>("Fonts/Hud");
            Sound.Load(Content);
        }
        protected override void Update(GameTime gameTime)
        {
            rand = new Random();
            //Debug.WriteLine(ScreenSize.X);
            GlobalTime++;
            //現在のシーンをアップデート
            if (currentScene == firstScene)
                firstScene.Update();
            else if (currentScene == menuScene)
                menuScene.Update();
            else if (currentScene == gameScene)
                gameScene.Update();
            else if (currentScene == resultScene)
                resultScene.Update();
            OldKeyState = Keyboard.GetState();//ひとつ前のキーボードの状態を保存
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            //現在のシーンを描画
            if (currentScene == firstScene)
                firstScene.Draw();
            else if (currentScene == menuScene)
                menuScene.Draw();
            else if (currentScene == gameScene)
                gameScene.Draw();
            else if (currentScene == resultScene)
                resultScene.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public static void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color, float scale = 1.0f)
        {
            Vector2 size = font.MeasureString(value);
            Vector2 origin = size * 0.5f;
            spriteBatch.DrawString(font, value, position, Color.Black, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, value, position, color, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
