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
            l1 = new GameLevel(game.GraphicsDevice, game);
            l2 = new GameLevel(game.GraphicsDevice, game);
            l3 = new GameLevel(game.GraphicsDevice, game);

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
            if (mouse.LeftButton == ButtonState.Pressed && Utilities.CheckCollision(mouse.Position, b3))
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
            spriteBatch.DrawString(font, "Score: " + game.Score, new Vector2(200, 250), Color.Black,
                                   0, Vector2.Zero, 1, SpriteEffects.None, 0);
            Color multiplierColor;
            switch(game.ScoreMultiplier)
            {
                case 1: //make invisible
                        multiplierColor = Color.White; break;
                case 2:
                case 3: multiplierColor = Color.Black; break;
                case 4: multiplierColor = Color.DarkRed; break;
                case 5: multiplierColor = Color.Red; break;
                case 6: multiplierColor = Color.OrangeRed; break;
                case 7: multiplierColor = Color.Orange; break;
                case 8: multiplierColor = Color.DarkGoldenrod; break;
                case 9: multiplierColor = Color.Yellow; break;
                
                default: multiplierColor = Color.Green; break;

            }
            spriteBatch.DrawString(font, "X" + game.ScoreMultiplier, new Vector2(400, 250), multiplierColor,
                                   0, Vector2.Zero, 1, SpriteEffects.None, 0);


            spriteBatch.DrawString(font, l1.Name, new Vector2(50, 500), Color.Black);
            spriteBatch.Draw(l1.minimap.map.GetMapTexture(game, l1.world), b1, Color.White);
            foreach (PointOfInterest poi in l1.world.pointsOfInterest)
            {
                Texture2D texture;
                switch (poi.Type)
                {
                    case PointOfInterest.Types.Hospital:
                    texture = game.Content.Load<Texture2D>("Hospital_Icon");
                    break;
                    
                    default:
                    texture = game.Content.Load<Texture2D>("alert");
                    break;
                }
                Vector2 posToDraw = new Vector2();
                posToDraw.X = poi.Position.X * b1.Width / l1.world.Width;
                posToDraw.Y = poi.Position.Y * b1.Height / l1.world.Height;
                posToDraw += b1.Location.ToVector2();

                Vector2 origin = texture.Bounds.Center.ToVector2();

                spriteBatch.Draw(texture, posToDraw, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);

            }


            spriteBatch.DrawString(font, l2.Name, new Vector2(400, 500), Color.Black);
            spriteBatch.Draw(l2.minimap.map.GetMapTexture(game, l2.world), b2, Color.White);
            foreach (PointOfInterest poi in l2.world.pointsOfInterest)
            {
                Texture2D texture;
                switch (poi.Type)
                {
                    case PointOfInterest.Types.Hospital:
                    texture = game.Content.Load<Texture2D>("Hospital_Icon");
                    break;

                    default:
                    texture = game.Content.Load<Texture2D>("alert");
                    break;
                }
                Vector2 posToDraw = new Vector2();
                posToDraw.X = poi.Position.X * b2.Width / l2.world.Width;
                posToDraw.Y = poi.Position.Y * b2.Height / l2.world.Height;
                posToDraw += b2.Location.ToVector2();

                Vector2 origin = texture.Bounds.Center.ToVector2();

                spriteBatch.Draw(texture, posToDraw, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);

            }

            spriteBatch.DrawString(font, l3.Name, new Vector2(750, 500), Color.Black);
            spriteBatch.Draw(l3.minimap.map.GetMapTexture(game, l3.world), b3, Color.White);
            foreach (PointOfInterest poi in l3.world.pointsOfInterest)
            {
                Texture2D texture;
                switch (poi.Type)
                {
                    case PointOfInterest.Types.Hospital:
                    texture = game.Content.Load<Texture2D>("Hospital_Icon");
                    break;

                    default:
                    texture = game.Content.Load<Texture2D>("alert");
                    break;
                }
                Vector2 posToDraw = new Vector2();
                posToDraw.X = poi.Position.X * b3.Width / l3.world.Width;
                posToDraw.Y = poi.Position.Y * b3.Height / l3.world.Height;
                posToDraw += b3.Location.ToVector2();

                Vector2 origin = texture.Bounds.Center.ToVector2();

                spriteBatch.Draw(texture, posToDraw, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);

            }

            base.Draw(spriteBatch, game);
        }

    }
}
