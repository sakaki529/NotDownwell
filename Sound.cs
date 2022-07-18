using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace NotDownwell
{
    static class Sound
	{
		public static SoundEffect Hurt;
		public static SoundEffect Stamp;
		public static SoundEffect Jump;
		public static SoundEffect Sel;
		public static void Load(ContentManager content)
		{
			Hurt = Main.instance.Content.Load<SoundEffect>("Sounds/hurt");
			Stamp = Main.instance.Content.Load<SoundEffect>("Sounds/stamp");
			Jump = Main.instance.Content.Load<SoundEffect>("Sounds/jump");
			Sel = Main.instance.Content.Load<SoundEffect>("Sounds/sel");
		}
	}
}
