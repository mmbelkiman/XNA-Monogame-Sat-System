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

        Rectangle rectangleNormal;
        Texture2D texture2DRectangleNormal;
        Vector2 rectangleNormalPosition = new Vector2(100, 300);

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
        }

        protected override void LoadContent() { spriteBatch = new SpriteBatch(GraphicsDevice); }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
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
            rotatedRectangle1.rotation -= 0.01f;
            rotatedRectangle2.rotation += 0.01f;

            //Check collisions
            if (rotatedRectangle1.Collide(rotatedRectangle2)
                || rotatedRectangle1.Collide(rectangleNormal))
                rotatedRectangle1.SetColor(new Color(100, 50, 50, 200));
            else
                rotatedRectangle1.SetColor(new Color(50, 100, 50, 200));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            {
                rotatedRectangle1.Draw(spriteBatch);
                rotatedRectangle2.Draw(spriteBatch);

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