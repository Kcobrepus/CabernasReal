using System.Collections.Generic;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using CabernasReal.Classes;


namespace CabernasReal
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int largura = 1280;
        private int altura = 720;
        private KeyboardState tecladoAnterior;

        private int nrLinhas = 0;
        private int nrColunas = 0;
        private SpriteFont font;

        public char[,] level;
        int tileSize = 64;

        public List<Enemy> enemies = new();
        public List<Spike> spikes = new();
        public List<Power> powers = new();
        public List<Exit> exits = new();
        public List<Wall> walls = new();

        private Player jogador;

        private FollowCamera camera;
        public enum GameState
        {
            Menu,
            Playing,
            Win
        }

        public GameState currentState = GameState.Menu;


        private Texture2D player, enemy, spike, wall, power, exit;
        public Texture2D[] correr;
        public int cont_sp, sp_active;

        public Song Mus_fundo, Mus_final, Mus_intro;

        public SoundEffect SF_jump, SF_doublejump, SF_explosion, SF_powerup;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = largura;
            _graphics.PreferredBackBufferHeight = altura;

            //_graphics.IsFullScreen = true; //Causa problemas por alguma razão

            Window.IsBorderless = true;

            _graphics.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            _graphics.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            _graphics.ApplyChanges();

            camera = new(Vector2.Zero);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            LoadLevel("part1.txt");

            _graphics.PreferredBackBufferHeight = 720; // definição da altura
            _graphics.PreferredBackBufferWidth = 1280; // definição da largura
            _graphics.ApplyChanges(); //aplica a atualização da janela

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("File");

            player = Content.Load<Texture2D>("player");
            enemy = Content.Load<Texture2D>("enemy");
            spike = Content.Load<Texture2D>("spike");
            wall = Content.Load<Texture2D>("parede");
            power = Content.Load<Texture2D>("power");
            exit = Content.Load<Texture2D>("door");

            correr = new Texture2D[8];
            correr[0] = Content.Load<Texture2D>("pl_sp0");
            correr[1] = Content.Load<Texture2D>("pl_sp1");
            correr[2] = Content.Load<Texture2D>("pl_sp2");
            correr[3] = Content.Load<Texture2D>("pl_sp3");
            correr[4] = Content.Load<Texture2D>("pl_sp4");
            correr[5] = Content.Load<Texture2D>("pl_sp5");
            correr[6] = Content.Load<Texture2D>("pl_sp6");
            correr[7] = Content.Load<Texture2D>("pl_sp7");



            // Sons
            Mus_intro = Content.Load<Song>("MUS_Intro");
            Mus_fundo = Content.Load<Song>("MUS_Cavern");
            Mus_final = Content.Load<Song>("MUS_Teal");
            MediaPlayer.Play(Mus_intro);

            SF_jump = Content.Load<SoundEffect>("SFX_Jump");
            SF_doublejump = Content.Load<SoundEffect>("SFX_Double_Jump");
            SF_explosion = Content.Load<SoundEffect>("SFX_Explosion");
            SF_powerup = Content.Load<SoundEffect>("SFX_Item_Get");

            // TODO: use this.Content to load your game content here
        }

        void LoadLevel(string levelFile)
        {
            enemies.Clear();
            spikes.Clear();
            powers.Clear();
            exits.Clear();
            walls.Clear();

            string[] linhas = File.ReadAllLines($"Content/{levelFile}"); // " Content /" + level
            nrLinhas = linhas.Length;
            nrColunas = linhas[0].Length;

            level = new char[nrColunas, nrLinhas];
            for (int x = 0; x < nrColunas; x++)
            {
                for (int y = 0; y < nrLinhas; y++)
                {
                    if (linhas[y][x] == 'p')
                    {
                        jogador = new Player(this, x, y);
                    }
                    else if (linhas[y][x] == 'E')
                    {
                        enemies.Add(new Enemy(x, y));
                        level[x, y] = ' ';
                    }
                    else if (linhas[y][x] == 'S')
                    {
                        spikes.Add(new Spike(x, y));
                        level[x, y] = ' ';
                    }
                    else if (linhas[y][x] == 'P')
                    {
                        powers.Add(new Power(x, y));
                        level[x, y] = ' ';
                    }
                    else if (linhas[y][x] == 'n')
                    {
                        exits.Add(new Exit(x, y));
                        level[x, y] = ' ';
                    }
                    else if (linhas[y][x] == 'X')
                    {
                        walls.Add(new Wall(x, y));
                    }
                    else
                    {
                        level[x, y] = ' ';
                    }

                }

            }
        }

        public void RestartLevel()
        {
            SF_explosion.Play();
            LoadLevel("part1.txt");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.F11) &&
                tecladoAnterior.IsKeyUp(Keys.F11))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();

            if (currentState == GameState.Menu)
            {
                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(Mus_fundo);
                    LoadLevel("part1.txt");
                    currentState = GameState.Playing;
                }
            }
            else if (currentState == GameState.Win)
            {
                if (keyboard.IsKeyDown(Keys.Space))
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(Mus_intro);
                    currentState = GameState.Menu;
                }
            }
            else if (currentState == GameState.Playing)
            {


                foreach (Enemy enemy in enemies)
                {
                    enemy.Update(gameTime);
                }

                tecladoAnterior = keyboard;

                // TODO: Add your update logic here

                jogador.Update(gameTime);

                camera.Follow(jogador.Position + new Vector2(tileSize / 2, tileSize / 2), new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            _spriteBatch.Begin(transformMatrix:
        currentState == GameState.Playing ? camera.GetTransform() : null);

            if (currentState == GameState.Menu)
            {
                _spriteBatch.DrawString(font, "CABERNAS", new Vector2(570, 150), Color.White);
                _spriteBatch.DrawString(font, "O Bernas caiu numa gruta e agora precisa de consumir o morango para poder saltar no ar e conseguir chegar na escada que o leva para o exterior!", new Vector2(100, 250), Color.White);
                _spriteBatch.DrawString(font, "Use as setas de direita e esquerda ou as teclas D e A para direcionar o Bernas para forna da Caverna e use a tecla de Espaco para o fazer saltar. ", new Vector2(100, 300), Color.White);
                _spriteBatch.DrawString(font, "Press ENTER to Start", new Vector2(530, 400), Color.White);
            }
            else if (currentState == GameState.Win)
            {
                _spriteBatch.DrawString(font, "You won", new Vector2(300, 150), Color.White);
                _spriteBatch.DrawString(font, "Press SPACE to go back to the Main Menu", new Vector2(250, 300), Color.White);
            }
            else if (currentState == GameState.Playing)
            {

                Rectangle position = new Rectangle(0, 0, tileSize, tileSize);

                foreach (Wall w in walls)
                {
                    Rectangle rect = new Rectangle(
                        (int)w.Position.X,
                        (int)w.Position.Y,
                        tileSize,
                        tileSize
                    );

                    _spriteBatch.Draw(wall, rect, Color.White);
                }

                foreach (Enemy e in enemies)
                {
                    Rectangle enemyRect = new Rectangle(
                        (int)e.Position.X,
                        (int)e.Position.Y,
                        tileSize,
                        tileSize
                    );

                    _spriteBatch.Draw(enemy, enemyRect, Color.White);
                }

                foreach (Spike s in spikes)
                {
                    Rectangle spikeRect = new Rectangle(
                        (int)s.Position.X,
                        (int)s.Position.Y,
                        tileSize,
                        tileSize
                    );

                    _spriteBatch.Draw(spike, spikeRect, Color.White);
                }

                foreach (Power p in powers)
                {
                    Rectangle powerRect = new Rectangle(
                        (int)p.Position.X,
                        (int)p.Position.Y,
                        tileSize,
                        tileSize
                    );

                    _spriteBatch.Draw(power, powerRect, Color.White);
                }

                foreach (Exit e in exits)
                {
                    Rectangle exitRect = new Rectangle(
                        (int)e.Position.X,
                        (int)e.Position.Y,
                        tileSize,
                        tileSize
                    );

                    _spriteBatch.Draw(exit, exitRect, Color.White);
                }

                position.X = (int)jogador.Position.X;
                position.Y = (int)jogador.Position.Y;

                //_spriteBatch.Draw(player, position, Color.White);
                _spriteBatch.Draw(correr[sp_active], position, Color.White);

            }

            _spriteBatch.End();

                base.Draw(gameTime);
            
        }
    }
}
