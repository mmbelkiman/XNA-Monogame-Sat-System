/**
 * Class created to work together with Monogame, 
 * is a simple implementation of a rectangle with rotation and collision 
 * using SAT (Separating Axis Theorem))
 * 
 * Feel free to change what you need, if you have ideas for improvement, contact and share =]
 *
 * 
 * Created by: 
 * Mauricio Terra (@mauriciomta) mauriciomta@gmail.com
 * Marcelo Belkiman (@mmbelkiman) marcelobelkiman@gmail.com
 * */

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
#endregion

namespace SatSystem
{
    class RotatedRectangle : CollideObj
    {
        private int _axesCount = 4;
        private List<Vector2> _rectangleABNormalize = new List<Vector2>();

        public List<Vector2> RectangleVertices = new List<Vector2>();
        public List<Vector2> rectangleNormalize = new List<Vector2>();

        public RotatedRectangle(GraphicsDevice graphicsDevice,
                                Vector2 position,
                                Vector2 origin,
                                int width, int height)
        {
            Position = position;
            Origin = origin;

            if (graphicsDevice != null)
            {
                _texture2D = new Texture2D(graphicsDevice, 1, 1);

                Color[] data = new Color[1 * 1];
                data[0] = _color;
                _texture2D.SetData(data);
            }

            Rectangle = new Rectangle(
                        (int)Position.X,
                        (int)Position.Y,
                         width,
                         height);
        }

        public void Update()
        {
            RectangleVertices = new List<Vector2>();
            rectangleNormalize = new List<Vector2>();
            _rectangleABNormalize = new List<Vector2>();

            List<Vector2> pointsVertices = GetPointsVertices();
            UpdateVertices(pointsVertices);
            List<Vector2> subtractedVectors = SubtractVectors();
            List<Vector2> perpendicularRectangles = GetPerpendicularRectangles(subtractedVectors);
            Normalize(perpendicularRectangles);
        }
       
        private List<Vector2> GetPointsVertices()
        {
            List<Vector2> pointsVertices = new List<Vector2>
            {
                new Vector2(Position.X, Position.Y),
                new Vector2(Position.X + Rectangle.Width, Position.Y),
                new Vector2(Position.X + Rectangle.Width, Position.Y + Rectangle.Height),
                new Vector2(Position.X, Position.Y + Rectangle.Height)
            };
            return pointsVertices;
        }

        private void UpdateVertices(List<Vector2> pointsVertices)
        {
            foreach (Vector2 vec2 in pointsVertices)
            {
                float originX = Position.X + Origin.X;
                float originY = Position.Y + Origin.Y;
                RectangleVertices.Add(RotatePoint(vec2, new Vector2(originX, originY), Rotation));
            }
        }

        private List<Vector2> SubtractVectors()
        {
            List<Vector2> subtractedVectors = new List<Vector2>();

            for (int i = 0; i < _axesCount; i++)
            {
                if (i == _axesCount - 1)
                {
                    subtractedVectors.Add(
                        Vector2.Subtract(RectangleVertices[0], RectangleVertices[i]));
                }
                else
                {
                    subtractedVectors.Add(
                        Vector2.Subtract(RectangleVertices[i + 1], RectangleVertices[i]));
                }
            }
            return subtractedVectors;
        }

        private List<Vector2> GetPerpendicularRectangles(List<Vector2> subtractedVectors)
        {
            List<Vector2> perpendicularRectangles = new List<Vector2>();

            for (int i = 0; i < _axesCount; i++)
            {
                perpendicularRectangles.Add(new Vector2(subtractedVectors[i].Y,
                                                        subtractedVectors[i].X * -1));
            }

            return perpendicularRectangles;
        }

        private void Normalize(List<Vector2> perpendicularRectangles)
        {
            for (int i = 0; i < _axesCount; i++)
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
                 rotatedRectangle.RectangleVertices);
        }

        public bool Collide(
            List<Vector2> rectangleBNormalize,
            List<Vector2> rectangleBVertices)
        {
            foreach (Vector2 vec in rectangleNormalize)
            {
                _rectangleABNormalize.Add(vec);
            }
            foreach (Vector2 vec in rectangleBNormalize)
            {
                _rectangleABNormalize.Add(vec);
            }

            CollideFlag = true;
            CalculateDot(rectangleBVertices);

            return CollideFlag;
        }

        private void CalculateDot(List<Vector2> rectangleBVertices)
        {
            foreach (Vector2 vecNormalize in _rectangleABNormalize)
            {
                float dot = Vector2.Dot(vecNormalize, RectangleVertices[0]);
                float rectangle1Min = dot;
                float rectangle1Max = dot;

                dot = Vector2.Dot(vecNormalize, rectangleBVertices[0]);
                float rectangle2Min = dot;
                float rectangle2Max = dot;

                foreach (Vector2 vecPositionOriginal in RectangleVertices)
                {
                    dot = 0;
                    dot = Vector2.Dot(vecNormalize, vecPositionOriginal);

                    if (dot < rectangle1Min) rectangle1Min = dot;
                    if (dot > rectangle1Max) rectangle1Max = dot;
                }
                foreach (Vector2 vecPositionOriginal in rectangleBVertices)
                {
                    dot = 0;
                    dot = Vector2.Dot(vecNormalize, vecPositionOriginal);

                    if (dot < rectangle2Min) rectangle2Min = dot;
                    if (dot > rectangle2Max) rectangle2Max = dot;
                }

                //have overlaps between Rectangles (i.e : MinB is less MaxA ?)
                //Then are detecting collision on this Axis
                if (rectangle2Max >= rectangle1Max
                    && rectangle1Max < rectangle2Min)
                {
                    CollideFlag = false;
                    break;
                }
                else if (rectangle1Max >= rectangle2Max
                    && rectangle2Max < rectangle1Min)
                {
                    CollideFlag = false;
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
