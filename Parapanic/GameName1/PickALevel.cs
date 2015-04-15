using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Parapanic
{
    class PickALevel : Level
    {
        Parapanic game;
        GameLevel l1;
        GameLevel l2;
        GameLevel l3;

        Rectangle b1;
        Rectangle b2;
        Rectangle b3;

        public PickALevel(Parapanic game)
        {
            this.game = game;
            l1 = new GameLevel(game.GraphicsDevice);
            l2 = new GameLevel(game.GraphicsDevice);
            l3 = new GameLevel(game.GraphicsDevice);

            l1.minimap.map.GetMapTexture(game, l1.world);
            Minimap.Map.DirtyFlag = true;
            l2.minimap.map.GetMapTexture(game, l2.world);
            Minimap.Map.DirtyFlag = true;
            l3.minimap.map.GetMapTexture(game, l3.world);

            b1 = new Rectangle(50, 300, 200, 200);
            b2 = new Rectangle(400, 300, 200, 200);
            b3 = new Rectangle(750, 300, 200, 200);
        }

        public override void Update()
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && Utilities.CheckCollision(mouse.Position, b1))
            {
                game.gameState = Parapanic.State.Game;
                game.Level = l1;
            }
            if (mouse.LeftButton == ButtonState.Pressed && Utilities.CheckCollision(mouse.Position, b2))
            {
                game.gameState = Parapanic.State.Game;
                game.Level = l2;
            }
            if (mouse.LeftButton == ButtonState.Pressed && Utilities.CheckCollision(mouse.Position, b2))
            {
                game.gameState = Parapanic.State.Game;
                game.Level = l3;
            }
            base.Update();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Parapanic game)
        {
            SpriteFont font = game.Content.Load<SpriteFont>("font");
            game.GraphicsDevice.Clear(Color.White);

            spriteBatch.DrawString(font, "Pick a Level!", new Vector2(200, 60), Color.Black,
                                   0, Vector2.Zero, 5, SpriteEffects.None, 0);

            spriteBatch.Draw(l1.minimap.map.GetMapTexture(game, l1.world), b1, Color.White);
            spriteBatch.Draw(l2.minimap.map.GetMapTexture(game, l2.world), b2, Color.White);
            spriteBatch.Draw(l3.minimap.map.GetMapTexture(game, l3.world), b3, Color.White);

            base.Draw(spriteBatch, game);
        }

    }
}
