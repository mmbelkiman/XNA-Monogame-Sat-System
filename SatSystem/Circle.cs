#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace SatSystem
{
    class Circle : CollideObj
    {
        public int Radius { get; private set; }

        public Circle(int x, int y, int radius, GraphicsDevice graphicsDevice)
        {
            Position = new Vector2(x, y);
            Radius = radius;
            CreateCircle(graphicsDevice);
        }

        private void CreateCircle(GraphicsDevice graphicsDevice)
        {
            int outerRadius = Radius * 2 + 2;
            _texture2D = new Texture2D(graphicsDevice, outerRadius, outerRadius);

            SetColor(Color.White);
        }

        public override bool Collide(CollideObj collideObj)
        {
            //TODO verify type of OBJ
            //GET Circle or Rectangle
            //Verify Collision

            if (collideObj.GetType() == typeof(Circle))
            {
                CollideFlag = Intersects((Circle)collideObj);
            }
            else if (collideObj.GetType() == typeof(RotatedRectangle))
            {
                //TODO
                //CollideFlag = 
            }

            return CollideFlag;
        }

        public override void SetColor(Color color)
        {
            if (_texture2D == null) return;

            int outerRadius = Radius * 2 + 2;
            Color[] data = new Color[outerRadius * outerRadius];
            _colorSize = outerRadius * outerRadius;

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.TransparentBlack;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / Radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                int x = (int)Math.Round(Radius + Radius * Math.Cos(angle));
                int y = (int)Math.Round(Radius + Radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = color;
            }

            _texture2D.SetData(data);
        }

        public bool Intersects(Rectangle rectangle)
        {
            // the first thing we want to know is if any of the corners intersect
            var corners = new[]
            {
                    new Point(rectangle.Top, rectangle.Left),
                    new Point(rectangle.Top, rectangle.Right),
                    new Point(rectangle.Bottom, rectangle.Right),
                    new Point(rectangle.Bottom, rectangle.Left)
                };

            foreach (var corner in corners)
                if (ContainsPoint(corner)) return true;

            // next we want to know if the left, top, right or bottom edges overlap
            if (Position.X - Radius > rectangle.Right || Position.X + Radius < rectangle.Left)
                return false;

            if (Position.Y - Radius > rectangle.Bottom || Position.Y + Radius < rectangle.Top)
                return false;

            return true;
        }

        public bool Intersects(Circle circle)
        {
            // put simply, if the distance between the circle centre's is less than
            // their combined radius
            var center0 = new Vector2(circle.Position.X, circle.Position.Y);
            var center1 = new Vector2(Position.X, Position.Y);
            return Vector2.Distance(center0, center1) < Radius + circle.Radius;
        }

        public bool ContainsPoint(Point point)
        {
            var vector2 = new Vector2(point.X - Position.X, point.Y - Position.Y);
            return vector2.Length() <= Radius;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture2D, new Vector2(Position.X, Position.Y), _color);
        }
    }
}
