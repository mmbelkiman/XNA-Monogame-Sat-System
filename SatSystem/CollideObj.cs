#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
#endregion


namespace SatSystem
{
    class CollideObj
    {
        protected Texture2D _texture2D;
        protected Color _color = new Color(100, 100, 200, 100);
        protected int _colorSize = 1;

        public Rectangle Rectangle;
        public float Rotation = 0f;
        public Vector2 Origin;
        public Vector2 Position;
        public bool CollideFlag = true;

        public virtual bool Collide(CollideObj calangoCollide) {
            return CollideFlag;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                     _texture2D,
                     new Vector2((int)Position.X + Origin.X,
                                 (int)Position.Y + Origin.Y),
                     Rectangle,
                     Color.White,
                     Rotation,
                     Origin,
                     1,
                     SpriteEffects.None,
                     1);
        }

        public virtual void SetColor(Color color)
        {
            if (_texture2D == null) return;
            Color[] data = new Color[_colorSize];

            for (int i = 0; i < data.Length; i++)
                data[i] = color;

            _texture2D.SetData(data);
        }
    }
}
