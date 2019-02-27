# XNA Monogame Sat System

Class created to work together with Monogame, is a simple implementation of a rectangle with rotation and collision using SAT (Separating Axis Theorem))
  
Feel free to change what you need, if you have ideas for improvement, contact and share =]

![](https://media.giphy.com/media/3oFzmr4vs1VLIhfKko/giphy.gif)

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

[![kofi](https://az743702.vo.msecnd.net/cdn/kofi2.png)](https://ko-fi.com/B0B2KE8I)

# License
Under MIT license
