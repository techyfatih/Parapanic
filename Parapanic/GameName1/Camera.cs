using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Parapanic
{
    class Camera
    {
        public static Vector2 position;
        static Rectangle view;

        static float scale = 1f;

        public static void Initialize(int width, int height)
        {
            position = new Vector2();
            view = new Rectangle(0, 0, width, height);
        }

        public static void Update(Ambulance ambulance, World world)
        {
            int newX = (int)ambulance.position.X - view.Width / 2;
            if (newX < 0) newX = 0;
            if (newX > world.Width - view.Width) newX = world.Width - view.Width;
            position.X = newX;
            view.X = newX;
            
            int newY = (int)ambulance.position.Y - view.Height / 2;
            if (newY < 0) newY = 0;
            if (newY > world.Height - view.Height) newY = world.Height - view.Height;
            position.Y = newY;
            view.Y = newY;
        }

        public static void Update(VectorAmbulance ambulance, World world)
        {
            int newX = (int)ambulance.position.X - view.Width / 2;
            if (newX < 0) newX = 0;
            if (newX > world.Width - view.Width) newX = world.Width - view.Width;
            position.X = newX;
            view.X = newX;

            int newY = (int)ambulance.position.Y - view.Height / 2;
            if (newY < 0) newY = 0;
            if (newY > world.Height - view.Height) newY = world.Height - view.Height;
            position.Y = newY;
            view.Y = newY;
        }

        struct RenderItem
        {
            public VertexPositionColorTexture[] Verts;
            public Texture2D Texture;
        }

        public static void DrawScreen(Parapanic game, Ambulance ambulance, World world)
        {
            //This whole method can probably be optimized a TON, so if we ever have speed problems this should be the first place to check.

            scale = Math.Abs((float)ambulance.speed) * -.25f / (float)ambulance.topSpeed + 1.25f;

            Vector2 TopLeft = new Vector2(0, 0);
            Vector2 BottomLeft = new Vector2(0, 1);
            Vector2 BottomRight = new Vector2(1, 1);
            Vector2 TopRight = new Vector2(1, 0);

            GraphicsDevice graphics = game.GraphicsDevice;

            
            int width = graphics.Viewport.Width;
            int height = graphics.Viewport.Height;
            Matrix view = Matrix.CreateLookAt(new Vector3(position.X + width/2, -position.Y - height/2, 100), 
                                              new Vector3(position.X + width/2, -position.Y - height/2, 0), 
                                              new Vector3(0, 1, 0));
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 1.26f/scale, (float)width/height, 0.01f, 1000);
                
            BasicEffect effect = new BasicEffect(graphics);

            effect.World = Matrix.CreateRotationX(MathHelper.Pi); 
            effect.View = view;
            effect.Projection = projection;
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = true;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            graphics.RasterizerState = rs;

            graphics.DepthStencilState = DepthStencilState.Default;

            List<RenderItem> renderItems = new List<RenderItem>();
            
            foreach (Block b in world.grid)
            {
                if (Math.Abs((b.position - position).LengthSquared()) < 2000000)
                {
                    Texture2D texture = Textures.ambulance;
                    Color c = Color.White;
                    if (b is WallBlock)
                        texture = Textures.wall;
                    else if (b is RoadBlock)
                        switch (((RoadBlock)b).Type)
                        {
                            case RoadTypes.Vertical:
                                texture = game.Content.Load<Texture2D>("vertical");
                                break;
                            case RoadTypes.Horizontal:
                                texture = game.Content.Load<Texture2D>("horizontal");
                                break;
                            case RoadTypes.FourWay:
                                texture = game.Content.Load<Texture2D>("FourWay");
                                break;
                            case RoadTypes.EndD:
                                texture = game.Content.Load<Texture2D>("EndD");
                                break;
                            case RoadTypes.EndL:
                                texture = game.Content.Load<Texture2D>("EndL");
                                break;
                            case RoadTypes.EndU:
                                texture = game.Content.Load<Texture2D>("EndU");
                                break;
                            case RoadTypes.EndR:
                                texture = game.Content.Load<Texture2D>("EndR");
                                break;
                            case RoadTypes.TwoWayLD:
                                texture = game.Content.Load<Texture2D>("TwoWayLD");
                                break;
                            case RoadTypes.TwoWayLU:
                                texture = game.Content.Load<Texture2D>("TwoWayLU");
                                break;
                            case RoadTypes.TwoWayRD:
                                texture = game.Content.Load<Texture2D>("TwoWayRD");
                                break;
                            case RoadTypes.TwoWayRU:
                                texture = game.Content.Load<Texture2D>("TwoWayRU");
                                break;
                            case RoadTypes.ThreeWayDLR:
                                texture = game.Content.Load<Texture2D>("ThreeWayDLR");
                                break;
                            case RoadTypes.ThreeWayUDL:
                                texture = game.Content.Load<Texture2D>("ThreeWayUDL");
                                break;
                            case RoadTypes.ThreeWayUDR:
                                texture = game.Content.Load<Texture2D>("ThreeWayUDR");
                                break;
                            case RoadTypes.ThreeWayULR:
                                texture = game.Content.Load<Texture2D>("ThreeWayULR");
                                break;

                            default:
                                texture = Textures.floor;
                                break;
                        }
                    else if (b is FloorBlock)
                        texture = Textures.floor;
                    else if (b is PatientBlock)
                        texture = Textures.patient;
                    else if (b is HospitalBlock)
                        texture = Textures.hospital;



                    Vector3 rearTopLeft = new Vector3(0, 0, 0) + new Vector3(b.position, 0);
                    Vector3 rearBottomLeft = new Vector3(0, Block.size, 0) + new Vector3(b.position, 0);
                    Vector3 rearBottomRight = new Vector3(Block.size, Block.size, 0) + new Vector3(b.position, 0);
                    Vector3 rearTopRight = new Vector3(Block.size, 0, 0) + new Vector3(b.position, 0);

                    Vector3 frontTopLeft = new Vector3(0, 0, -b.depth) + new Vector3(b.position, 0);
                    Vector3 frontBottomLeft = new Vector3(0, Block.size, -b.depth) + new Vector3(b.position, 0);
                    Vector3 frontBottomRight = new Vector3(Block.size, Block.size, -b.depth) + new Vector3(b.position, 0);
                    Vector3 frontTopRight = new Vector3(Block.size, 0, -b.depth) + new Vector3(b.position, 0);

                    VertexPositionColorTexture[] vs;

                    if (false)
                        //b.depth != 0)
                    {
                        vs = new VertexPositionColorTexture[36];
                        vs[0] = new VertexPositionColorTexture(rearBottomLeft, c, BottomLeft);
                        vs[1] = new VertexPositionColorTexture(rearTopLeft, c, TopLeft);
                        vs[2] = new VertexPositionColorTexture(rearBottomRight, c, BottomRight);
                        vs[3] = new VertexPositionColorTexture(rearTopRight, c, TopRight);
                        vs[4] = vs[2];
                        vs[5] = vs[1];

                        vs[6] = new VertexPositionColorTexture(rearBottomLeft, c, BottomRight);
                        vs[7] = new VertexPositionColorTexture(rearTopLeft, c, BottomLeft);
                        vs[8] = new VertexPositionColorTexture(frontTopLeft, c, TopLeft);
                        vs[9] = new VertexPositionColorTexture(frontBottomLeft, c, TopRight);
                        vs[10] = vs[8];
                        vs[11] = vs[6];

                        vs[12] = new VertexPositionColorTexture(rearBottomRight, c, BottomLeft);
                        vs[13] = new VertexPositionColorTexture(rearTopRight, c, BottomRight);
                        vs[14] = new VertexPositionColorTexture(frontTopRight, c, TopRight);
                        vs[15] = new VertexPositionColorTexture(frontBottomRight, c, TopLeft);
                        vs[16] = vs[14];
                        vs[17] = vs[12];

                        vs[18] = new VertexPositionColorTexture(rearBottomLeft, c, BottomLeft);
                        vs[19] = new VertexPositionColorTexture(frontBottomLeft, c, TopLeft);
                        vs[20] = new VertexPositionColorTexture(rearBottomRight, c, BottomRight);
                        vs[21] = new VertexPositionColorTexture(frontBottomRight, c, TopRight);
                        vs[22] = vs[20];
                        vs[23] = vs[19];

                        vs[24] = new VertexPositionColorTexture(rearTopRight, c, BottomLeft);
                        vs[25] = new VertexPositionColorTexture(frontTopRight, c, TopLeft);
                        vs[26] = new VertexPositionColorTexture(rearTopLeft, c, BottomRight);
                        vs[27] = new VertexPositionColorTexture(frontTopLeft, c, TopRight);
                        vs[28] = vs[26];
                        vs[29] = vs[25];

                        vs[30] = new VertexPositionColorTexture(frontBottomLeft, c, BottomLeft);
                        vs[31] = new VertexPositionColorTexture(frontTopLeft, c, TopLeft);
                        vs[32] = new VertexPositionColorTexture(frontBottomRight, c, BottomRight);
                        vs[33] = new VertexPositionColorTexture(frontTopRight, c, TopRight);
                        vs[34] = vs[32];
                        vs[35] = vs[31];
                    }
                    else
                    {
                        vs = new VertexPositionColorTexture[6];

                        vs[0] = new VertexPositionColorTexture(rearBottomLeft, c, BottomLeft);
                        vs[1] = new VertexPositionColorTexture(rearTopLeft, c, TopLeft);
                        vs[2] = new VertexPositionColorTexture(rearBottomRight, c, BottomRight);
                        vs[3] = new VertexPositionColorTexture(rearTopRight, c, TopRight);
                        vs[4] = vs[2];
                        vs[5] = vs[1];
                    }


                    RenderItem r = new RenderItem { Verts = vs, Texture = texture };
                    renderItems.Add(r);
                }
            }
            {
                Color c = Color.White;
                Texture2D texture = Textures.ambulance;
                Vector2 p1 = new Vector2(-texture.Width / 2, -texture.Height / 2);
                Vector2 p2 = new Vector2(-texture.Width / 2, texture.Height / 2);
                Vector2 p3 = new Vector2(texture.Width / 2, texture.Height / 2);
                Vector2 p4 = new Vector2(texture.Width / 2, -texture.Height / 2);

                Matrix itemWorld = Matrix.CreateTranslation(new Vector3(ambulance.position, 0));



                VertexPositionColorTexture[] vs = new VertexPositionColorTexture[6];
                Matrix rotationMatrix = Matrix.CreateRotationZ(ambulance.drawDirection);

                Vector3 v1 = (Matrix.Identity
                    * Matrix.CreateTranslation(new Vector3(p1, 0)) 
                    * Matrix.CreateScale(ambulance.scale)
                    * rotationMatrix
                    * itemWorld
                    ).Translation;
                Vector3 v2 = (Matrix.Identity
                    * Matrix.CreateTranslation(new Vector3(p2, 0))
                    * Matrix.CreateScale(ambulance.scale)
                    * rotationMatrix
                    * itemWorld
                    ).Translation;
                Vector3 v3 = (Matrix.Identity
                    * Matrix.CreateTranslation(new Vector3(p3, 0))
                    * Matrix.CreateScale(ambulance.scale)
                    * rotationMatrix
                    * itemWorld
                    ).Translation;
                Vector3 v4 = (Matrix.Identity
                    * Matrix.CreateTranslation(new Vector3(p4, 0))
                    * Matrix.CreateScale(ambulance.scale)
                    * rotationMatrix
                    * itemWorld
                    ).Translation;

                vs[0] = new VertexPositionColorTexture(v2 + new Vector3(0, 0, -.2f), c, BottomLeft);
                vs[1] = new VertexPositionColorTexture(v1 + new Vector3(0, 0, -.2f), c, TopLeft);
                vs[2] = new VertexPositionColorTexture(v3 + new Vector3(0, 0, -.2f), c, BottomRight);
                vs[3] = new VertexPositionColorTexture(v4 + new Vector3(0, 0, -.2f), c, TopRight);
                vs[4] = vs[2];
                vs[5] = vs[1];

                RenderItem r = new RenderItem() { Verts = vs, Texture = texture };
                renderItems.Add(r);
            }
            foreach(Car car in world.Cars)
            {
                if (Math.Abs((car.position - position).LengthSquared()) < 2000000)
                {
                    Color c = Color.White;
                    Texture2D texture = Textures.ambulance;
                    Vector2 p1 = new Vector2(-texture.Width / 2, -texture.Height / 2);
                    Vector2 p2 = new Vector2(-texture.Width / 2, texture.Height / 2);
                    Vector2 p3 = new Vector2(texture.Width / 2, texture.Height / 2);
                    Vector2 p4 = new Vector2(texture.Width / 2, -texture.Height / 2);

                    Matrix itemWorld = Matrix.CreateTranslation(new Vector3(car.position, 0));



                    VertexPositionColorTexture[] vs = new VertexPositionColorTexture[6];
                    Matrix rotationMatrix = Matrix.CreateRotationZ(car.direction);

                    Vector3 v1 = (Matrix.Identity
                        * Matrix.CreateTranslation(new Vector3(p1, 0))
                        * Matrix.CreateScale(car.scale)
                        * rotationMatrix
                        * itemWorld
                        ).Translation;
                    Vector3 v2 = (Matrix.Identity
                        * Matrix.CreateTranslation(new Vector3(p2, 0))
                        * Matrix.CreateScale(car.scale)
                        * rotationMatrix
                        * itemWorld
                        ).Translation;
                    Vector3 v3 = (Matrix.Identity
                        * Matrix.CreateTranslation(new Vector3(p3, 0))
                        * Matrix.CreateScale(car.scale)
                        * rotationMatrix
                        * itemWorld
                        ).Translation;
                    Vector3 v4 = (Matrix.Identity
                        * Matrix.CreateTranslation(new Vector3(p4, 0))
                        * Matrix.CreateScale(car.scale)
                        * rotationMatrix
                        * itemWorld
                        ).Translation;

                    vs[0] = new VertexPositionColorTexture(v2 + new Vector3(0, 0, -.1f), c, BottomLeft);
                    vs[1] = new VertexPositionColorTexture(v1 + new Vector3(0, 0, -.1f), c, TopLeft);
                    vs[2] = new VertexPositionColorTexture(v3 + new Vector3(0, 0, -.1f), c, BottomRight);
                    vs[3] = new VertexPositionColorTexture(v4 + new Vector3(0, 0, -.1f), c, TopRight);
                    vs[4] = vs[2];
                    vs[5] = vs[1];

                    RenderItem r = new RenderItem() { Verts = vs, Texture = texture };
                    renderItems.Add(r);
                }
            }
            

            foreach (RenderItem item in renderItems)
            {
                effect.Texture = item.Texture;

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphics.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, item.Verts, 0, item.Verts.Length / 3);
                }
            }
        }



    }
}
