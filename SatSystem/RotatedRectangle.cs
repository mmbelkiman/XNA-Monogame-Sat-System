/**
 * Class created to work together with Monogame, 
 * is a simple implementation of a rectangle with rotation and collision 
 * using SAT (Separating Axis Theorem))
 * 
 * Feel free to change what you need, if you have ideas for improvement, contact and share =]
 *
 * No license, do anything you want with this file ^-~!
 * 
 * Created by: 
 * Maurio Terra (@mauriciomta) mauriciomta@gmail.com
 * Marcelo Belkiman (@mmbelkiman) marcelobelkiman@gmail.com
 * */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SatSystem
{
    class RotatedRectangle
    {
        private int axesCount = 4;
        private Texture2D texture2D;
        private Vector2 _position;
        private Vector2 _origin;
        private Color color = new Color(100, 100, 200, 100);
        private Rectangle rectangle;
        private List<Vector2> rectangleABNormalize = new List<Vector2>();

        public List<Vector2> rectangleVertices = new List<Vector2>();
        public List<Vector2> rectangleNormalize = new List<Vector2>();
        public float rotation = 0f;
        public bool isCollide = true;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public RotatedRectangle(GraphicsDevice graphicsDevice,
            Vector2 position,
            Vector2 origin,
            int width, int height)
        {
            Position = position;
            Origin = origin;

            if (graphicsDevice != null)
            {
                texture2D = new Texture2D(graphicsDevice, 1, 1);

                Color[] data = new Color[1 * 1];
                data[0] = color;
                texture2D.SetData(data);
            }

            rectangle = new Rectangle(
                        (int)Position.X,
                        (int)Position.Y,
                         width,
                         height);
        }

        public void Update()
        {
            rectangleVertices = new List<Vector2>();
            rectangleNormalize = new List<Vector2>();
            rectangleABNormalize = new List<Vector2>();

            List<Vector2> pointsVertices = GetPointsVertices();
            UpdateVertices(pointsVertices);
            List<Vector2> subtractedVectors = SubtractVectors();
            List<Vector2> perpendicularRectangles = GetPerpendicularRectangles(subtractedVectors);
            Normalize(perpendicularRectangles);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                     texture2D,
                     new Vector2((int)Position.X + Origin.X,
                                 (int)Position.Y + Origin.Y),
                     rectangle,
                     Color.White,
                     rotation,
                     Origin,
                     1,
                     SpriteEffects.None,
                     1);
        }

        public void SetColor(Color color)
        {
            Color[] data = new Color[1 * 1];
            data[0] = color;
            texture2D.SetData(data);
        }

        private List<Vector2> GetPointsVertices()
        {
            List<Vector2> pointsVertices = new List<Vector2>
            {
                new Vector2(Position.X, Position.Y),
                new Vector2(Position.X + rectangle.Width, Position.Y),
                new Vector2(Position.X + rectangle.Width, Position.Y + rectangle.Height),
                new Vector2(Position.X, Position.Y + rectangle.Height)
            };
            return pointsVertices;
        }

        private void UpdateVertices(List<Vector2> pointsVertices)
        {
            foreach (Vector2 vec2 in pointsVertices)
            {
                float originX = Position.X + Origin.X;
                float originY = Position.Y + Origin.Y;
                rectangleVertices.Add(RotatePoint(vec2, new Vector2(originX, originY), rotation));
            }
        }

        private List<Vector2> SubtractVectors()
        {
            List<Vector2> subtractedVectors = new List<Vector2>();

            for (int i = 0; i < axesCount; i++)
            {
                if (i == axesCount - 1)
                {
                    subtractedVectors.Add(
                        Vector2.Subtract(rectangleVertices[0], rectangleVertices[i]));
                }
                else
                {
                    subtractedVectors.Add(
                        Vector2.Subtract(rectangleVertices[i + 1], rectangleVertices[i]));
                }
            }
            return subtractedVectors;
        }

        private List<Vector2> GetPerpendicularRectangles(List<Vector2> subtractedVectors)
        {
            List<Vector2> perpendicularRectangles = new List<Vector2>();

            for (int i = 0; i < axesCount; i++)
            {
                perpendicularRectangles.Add(new Vector2(subtractedVectors[i].Y,
                                                        subtractedVectors[i].X * -1));
            }

            return perpendicularRectangles;
        }

        private void Normalize(List<Vector2> perpendicularRectangles)
        {
            for (int i = 0; i < axesCount; i++)
            {
                rectangleNormalize.Add(Vector2.Normalize(perpendicularRectangles[i]));
            }
        }

        public bool Collide(Rectangle rectangle)
        {
            RotatedRectangle rotateRectangle = new RotatedRectangle(
                null,
                new Vector2(rectangle.X, rectangle.Y),
                new Vector2(0, 0),
                rectangle.Width,
                rectangle.Height);
            rotateRectangle.Update();
            return Collide(rotateRectangle);
        }

        public bool Collide(RotatedRectangle rotatedRectangle)
        {
            return Collide(rotatedRectangle.rectangleNormalize,
                 rotatedRectangle.rectangleVertices);
        }

        public bool Collide(
            List<Vector2> rectangleBNormalize,
            List<Vector2> rectangleBVertices)
        {
            foreach (Vector2 vec in rectangleNormalize)
            {
                rectangleABNormalize.Add(vec);
            }
            foreach (Vector2 vec in rectangleBNormalize)
            {
                rectangleABNormalize.Add(vec);
            }

            isCollide = true;
            CalculateDot(rectangleBVertices);

            return isCollide;
        }

        private void CalculateDot(List<Vector2> rectangleBVertices)
        {
            foreach (Vector2 vecNormalize in rectangleABNormalize)
            {
                float dot = Vector2.Dot(vecNormalize, rectangleVertices[0]);

                float rectangle1Menor = dot;
                float rectangle1Maior = dot;
                dot = Vector2.Dot(vecNormalize, rectangleBVertices[0]);

                float rectangle2Menor = dot;
                float rectangle2Maior = dot;

                foreach (Vector2 vecPositionOriginal in rectangleVertices)
                {
                    dot = 0;
                    dot = Vector2.Dot(vecNormalize, vecPositionOriginal);

                    if (dot < rectangle1Menor)
                        rectangle1Menor = dot;
                    if (dot > rectangle1Maior)
                        rectangle1Maior = dot;
                }
                foreach (Vector2 vecPositionOriginal in rectangleBVertices)
                {
                    dot = 0;
                    dot = Vector2.Dot(vecNormalize, vecPositionOriginal);

                    if (dot < rectangle2Menor)
                        rectangle2Menor = dot;
                    if (dot > rectangle2Maior)
                        rectangle2Maior = dot;
                }

                //have overlaps between Rectangles (i.e : MinB is less MaxA ?)
                //Then are detecting collision on this Axis
                if (
                    rectangle2Maior >= rectangle1Maior &&
                    rectangle1Maior < rectangle2Menor)
                {
                    isCollide = false;
                    break;
                }
                else if (
                    rectangle1Maior >= rectangle2Maior &&
                    rectangle2Maior < rectangle1Menor)
                {
                    isCollide = false;
                    break;
                }
            }
        }

        private Vector2 RotatePoint(Vector2 point, Vector2 origin, float rotation)
        {
            Vector2 aTranslatedPoint = new Vector2()
            {
                X = (float)(origin.X + (point.X - origin.X)
                * Math.Cos(rotation) - (point.Y - origin.Y)
                * Math.Sin(rotation)),

                Y = (float)(origin.Y + (point.Y - origin.Y)
                * Math.Cos(rotation) + (point.X - origin.X)
                * Math.Sin(rotation))
            };
            return aTranslatedPoint;
        }

        public static double RadianToDegree(double angle) { return angle * (180.0 / Math.PI); }
        public static float DegreeToRadian(double angle) { return Convert.ToSingle(Math.PI * angle / 180.0); }
    }
}
