# XNA Monogame Sat System

This class is designed to work with Monogame and provides a simple implementation of a rectangle with rotation and collision detection using the Separating Axis Theorem (SAT).

Feel free to make any modifications you need. If you have ideas for improvement, please contact and share them! =]

![demo](https://github.com/mmbelkiman/XNA-Monogame-Sat-System/assets/6968452/f8a472be-ed85-4cf0-a403-46d391ae57ff)

# Created by: 
 * Mauricio Terra (@mauriciomta) mauriciomta@gmail.com
 * Marcelo Belkiman (@mmbelkiman) marcelobelkiman@gmail.com

# Installation

Copy file RotatedRectangle.cs, Circle.cs, CollideObj.cs to your project. Yeah, thats all. (need Monogame/XNA)

# Example
You can see Game1.cs to see how to use

### How to use
Create a new rotateionRectangle

```
 rotatedRectangleA = new RotatedRectangle(
                GraphicsDevice,
                position,
                originPoint,
                width, 
                heigth);
```

At Update do a update

```
rotatedRectangleA.Update();
```

If you want to rotate, do:
```
rotatedRectangleA.rotation = newRotationValue;
```

You can use a static function DegreeToRadian to help you a rotate

```
rotatedRectangleA = RotateRectangle.DegreeToRadian(80);
```

To check a collision do
```
rotatedRectangleA.Collide(rotatedRectangleB)
```

You can change collor if has collision
```
if (rotatedRectangleA.Collide(rotatedRectangleB))
    rotatedRectangleA.SetColor(new Color(255, 0, 0));
else
    rotatedRectangleA.SetColor(new Color(0, 255, 0, 200));
```

Do Draw at Draw
```
spriteBatch.Begin();
    rotatedRectangleA.Draw(spriteBatch);
spriteBatch.End();
```

# License
Under MIT license
