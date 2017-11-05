using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wordpop
{
    public class WordpopView : Game, View
    {
        private int _w;
        private int _h;
        private WordpopController controller = null;
        Texture2D snakeTile = null;
        Texture2D foodTile = null;
        Texture2D wallTile = null;
        private const int TILE_SIZE = 10;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        WordpopModel sbm = null;

        public WordpopView(int w, int h)
        {
            _w = w;
            _h = h;
            try
            {
                graphics = new GraphicsDeviceManager(this);
                graphics.PreferredBackBufferWidth = _w * TILE_SIZE;
                graphics.PreferredBackBufferHeight = _h * TILE_SIZE;
                Content.RootDirectory = "Content";
            }
            catch (Exception e)
            {
                MessageBox.Show("Can't initilize game engine, error is " + e.Message);
                throw (e);
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            try
            {
                using (var stream = TitleContainer.OpenStream("Content/black5b5.png"))
                {
                    snakeTile = Texture2D.FromStream(this.GraphicsDevice, stream);
                }
                using (var stream = TitleContainer.OpenStream("Content/gray5b5.png"))
                {
                    foodTile = Texture2D.FromStream(this.GraphicsDevice, stream);
                }
                using (var stream = TitleContainer.OpenStream("Content/white5b5.png"))
                {
                    wallTile = Texture2D.FromStream(this.GraphicsDevice, stream);
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Can't load asset " + ex.ToString());
                Exit();
            }
            base.LoadContent();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();
            controller.KeyUpHandled(Keyboard.GetState());
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (sbm != null)
            {
                spriteBatch.Begin();
                Vector2 position;
                foreach (int i in Enumerable.Range(0, _w))
                {
                    foreach (int j in Enumerable.Range(0, _h))
                    {
                        position = new Vector2(i * TILE_SIZE, j * TILE_SIZE);
                        if (sbm.Board[i, j] == WordpopModel.BOARD_SNAKE)
                        {
                            spriteBatch.Draw(snakeTile, position, Color.White);
                        }
                        else if (sbm.Board[i, j] == WordpopModel.BOARD_FOOD)
                        {
                            spriteBatch.Draw(foodTile, position, Color.White);
                        }
                        else if (sbm.Board[i, j] == WordpopModel.BOARD_WALL)
                        {
                            spriteBatch.Draw(wallTile, position, Color.White);
                        }
                    }
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public void setController(Controller c)
        {
            controller = (WordpopController)c;
        }

        public void Notify(Model m)
        {
            // if it's a SnakeBoardModel, then we know how to handle it
            if (m is WordpopModel)
            {
                sbm = (WordpopModel)m;

                if (sbm.isHit)
                {
                    controller.Stop();
                    MessageBox.Show("Game over!!, your score is " + (sbm.WordpopLength() - WordpopModel.SNAKE_INIT_SIZE));
                }

                if (sbm.isEating)
                {
                    Wordpop.Debug("Eating");
                }

            }
        }
    }
}
