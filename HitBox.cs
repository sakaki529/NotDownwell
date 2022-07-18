using SharpDX;

namespace NotDownwell
{
    public class HitBox
    {
        public float X
        {
            get => hitBoxRect.X;
            set => hitBoxRect.X = value;
        }
        public float Y
        {
            get => hitBoxRect.Y;
            set => hitBoxRect.Y = value;
        }
        public Vector2 Position
        {
            get => new Vector2(hitBoxRect.X, hitBoxRect.Y);
            set
            {
                X += value.X;
                Y += value.Y;
            }
        }
        public RectangleF hitBoxRect;
        public HitBox(float x, float y, float w, float h) => hitBoxRect = new RectangleF(x, y, w, h);
        public HitBox(Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 size) => hitBoxRect = new RectangleF(position.X, position.Y, size.X, size.Y);
        public bool Intersect(HitBox target) => hitBoxRect.Intersects(target.hitBoxRect);
    }
}
