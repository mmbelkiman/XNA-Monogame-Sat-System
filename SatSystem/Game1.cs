using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SatSystem
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RotatedRectangle rotatedRectangle1;
        RotatedRectangle rotatedRectangle2;
        Circle circle1;
        Circle circle2;

        bool circle2MoveUp = false;

        Rectangle rectangleNormal;
        Texture2D texture2DRectangleNormal;
        Vector2 rectangleNormalPosition = new Vector2(400, 300);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            int widthRec1 = 50;
            int heightRec1 = 80;
            int widthRec2 = 40;
            int heightRec2 = 200;

            rotatedRectangle1 = new RotatedRectangle(
                GraphicsDevice,
                new Vector2(100, 100),
                new Vector2(widthRec1 / 2, heightRec1 / 2),
                widthRec1, heightRec1);

            rotatedRectangle2 = new RotatedRectangle(
                GraphicsDevice,
                new Vector2(300, 100),
                new Vector2(widthRec2 / 2, heightRec2 / 2),
                widthRec2, heightRec2);

            //Normal rectangle to test with a rotated rectangle
            texture2DRectangleNormal = new Texture2D(GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            data[0] = new Color(120, 100, 50);
            texture2DRectangleNormal.SetData(data);

            rectangleNormal = new Rectangle(
                  (int)rectangleNormalPosition.X,
                  (int)rectangleNormalPosition.Y,
                  100,
                  100);

            //Circles
            circle1 = new Circle(
                80,
                300,
                50, GraphicsDevice);

            circle2 = new Circle(
                   80,
                   300,
                   50, GraphicsDevice);

        }

        protected override void LoadContent() { spriteBatch = new SpriteBatch(GraphicsDevice); }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            Window.Title = "Monogame Sat System @mmbelkiman";

            //Move rectangle 1 with keyboard
            KeyboardState state = Keyboard.GetState();
            Vector2 rotate1Position = rotatedRectangle1.Position;
            if (state.IsKeyDown(Keys.Left)) rotate1Position.X -= 2.5f;
            if (state.IsKeyDown(Keys.Right)) rotate1Position.X += 2.5f;
            if (state.IsKeyDown(Keys.Up)) rotate1Position.Y -= 2.5f;
            if (state.IsKeyDown(Keys.Down)) rotate1Position.Y += 2.5f;
            rotatedRectangle1.Position = rotate1Position;

            rotatedRectangle1.Update();
            rotatedRectangle2.Update();
            rotatedRectangle1.Rotation -= 0.01f;
            rotatedRectangle2.Rotation += 0.01f;

            //Check collisions
            if (rotatedRectangle1.Collide(rotatedRectangle2)
                || rotatedRectangle1.Collide(rectangleNormal))
                rotatedRectangle1.SetColor(new Color(100, 50, 50, 200));
            else
                rotatedRectangle1.SetColor(new Color(50, 100, 50, 200));

            if (circle1.Collide(circle2))
            {
                circle1.SetColor(new Color(100, 50, 50, 200));
                circle2.SetColor(new Color(100, 50, 50, 200));
            }
            else
            {
                circle1.SetColor(new Color(150, 255, 180, 255));
                circle2.SetColor(new Color(150, 255, 180, 255));
            }



            if (circle2MoveUp)
                circle2.Position = new Vector2(circle2.Position.X, circle2.Y-=2);
            else
                circle2.Position = new Vector2(circle2.Position.X, circle2.Y+=2);

            if (circle2.Position.Y > 430) circle2MoveUp = true;
            else if (circle2.Position.Y < 180) circle2MoveUp = false;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            {
                rotatedRectangle1.Draw(spriteBatch);
                rotatedRectangle2.Draw(spriteBatch);
                circle1.Draw(spriteBatch);
                circle2.Draw(spriteBatch);

                spriteBatch.Draw(
                         texture2DRectangleNormal,
                         rectangleNormalPosition,
                         rectangleNormal,
                         Color.White,
                         0,
                         new Vector2(0, 0),
                         1,
                         SpriteEffects.None,
                         1);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}