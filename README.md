Cabernas 

Cabernas é um joga de plataformas onde 

O objtivo de Cabernas é levar o Bernas até à escada que o leva para fora da caverna, tendo de desviar de inimigos e armadilhas para conseguir o morango que lhe concede o poder do duplo salto.

_________________________________________________________________________________

Grupo

34982 João Alves

34987 Gustavo Pontes

34979 Gonçalo Vasconcelos

_________________________________________________________________________________

Requisitos

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

_________________________________________________________________________________

Funcionalidades

Movimento em duas direções (esquerda e direta)

Salto e duplo salto

Desbloqueio de habilidades

Recomeço

Vitória

_________________________________________________________________________________

Como jogar

O jogador controla o personagem (Bernas) com as setas do tecaldo ou as teclas A e D para se mover para a esquerda e direita e clicando no espaço para saltar. Ao caminhar pela caverna terá inimigos e armadilhas. Se chocar com qualquer umdos inimigos ou armadilhas perderá de imediato. O jogador tem de saltar entre blocos para conseguir obter o slato duplo que o ajuda a chegar a outras partes da caverna e chegar à escada.

_________________________________________________________________________________

Estrutura do Código

O jogo está divido por classes, cada uma dedicada a um aspecto do jogo.

Algumas das mais importantes:

• Game1: Classe principal do jogo que gera o jogo. Contém a inicialização, carregamento de conteúdo, atualização e métodos de desenho, e carrega o primeiro ecrã do jogo.

•Player: Classe com as ações do jogador. Contém as mecânicas de movimento, colisão, animação.

•FollowCamera: Classe quepermite a camara ir com o jogador.

_________________________________________________________________________________

Análisa do Código

• Game1

A Classe Game1 carrega todas as texturas, sons, locais e fontes do jogo e outras funções.
  
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
        ...
        }
        
A função LoadLevel(string levelFile) cria, com base num ficheirode texto ja estruturado o mapa do jogo


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

        
A função RestartLevel, faz um som e reinvoca a função LoadLevel


        public void RestartLevel()
        {
            SF_explosion.Play();
            LoadLevel("part1.txt");
        }

A função Update(GameTime gameTime) verifica constantemente o teclado para verificar sealguma tecla foi precionada, e invoca as funçõesde Update de outras Classes

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
                
                jogador.Update(gameTime);

                camera.Follow(jogador.Position + new Vector2(tileSize / 2, tileSize / 2), new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
                base.Update(gameTime);
            }
        }

A função Draw(GameTime gameTime), implementa as imagens que são usadas no jogador, nos inimigos,paredes etc.

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
        
• Player

A Classe Player contem tudo o que o personagem faz, como reage às diferentes interações e as suas variàveis

    internal class Player
    {
        private Game1 game; //reference from Game1 to Player

        private float moveSpeed = 500f; // pixels per second
        private Vector2 velocity;
        private float gravity = 1100f;
        private float jumpForce = -450f;
        private bool isOnGround = false;

        private bool powerUp = false;
        private bool doubleJump = false;
        private bool hasJumped = true;

        private Rectangle Bounds =>
    new Rectangle(
        (int)position.X,
        (int)position.Y,
        64,
        64
    );

        // Current player position in the matrix (multiply by tileSize prior to drawing)

        private Vector2 position;
        public Vector2 Position => position;



        public Player(Game1 game, float x, float y) // constructor que dada a as posições guarda a sua posição
        {
            this.game = game;   
            position = new Vector2(x * 64, y * 64);
        }

        private bool CollidesWithWall(float x, float y)
        {
            Rectangle future = new Rectangle((int)x, (int)y, 64, 64);

            foreach (Wall wall in game.walls)
            {
                if (future.Intersects(wall.Bounds))
                    return true;
            }

            return false;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 direction = Vector2.Zero;

            if (kState.IsKeyDown(Keys.A) || (kState.IsKeyDown(Keys.Left))) {
                direction.X = -1;
                game.cont_sp++;
                if (game.cont_sp >= 9)
                {
                    if (game.sp_active == 0) game.sp_active = 5;
                    game.cont_sp = 0;
                    game.sp_active++;

                    if (game.sp_active >= game.correr.Length)
                    {
                        game.sp_active = 5;
                    }
                }
            }
            else if (kState.IsKeyDown(Keys.D) || (kState.IsKeyDown(Keys.Right))) {
                direction.X = 1;
                game.cont_sp++;
                if (game.cont_sp >= 9)
                {
                    game.cont_sp = 0;
                    game.sp_active++;

                    if (game.sp_active >= game.correr.Length / 2)
                    {
                        game.sp_active = 0;
                    }
                }
            }
            else { game.cont_sp = 0; if (game.sp_active >= 4) game.sp_active = 4; else game.sp_active = 0; }

            // Horizontal velocity
            velocity.X = direction.X * moveSpeed;

            // Jump
            if (kState.IsKeyDown(Keys.Space) && isOnGround)
            {
                game.SF_jump.Play();
                velocity.Y = jumpForce;
                isOnGround = false;
                hasJumped = true;
            }

            if (kState.IsKeyUp(Keys.Space) && hasJumped)
            {
                doubleJump = false;
            }
                //Double Jump

                if (powerUp)
            {
                if (kState.IsKeyDown(Keys.Space) && doubleJump == false && isOnGround == false)
                {
                    game.SF_doublejump.Play();
                    velocity.Y = jumpForce;
                    doubleJump = true;
                    hasJumped = false;
                }
            }

            // Gravity
            velocity.Y += gravity * dt;

            // ===== X AXIS COLLISION =====
            float newX = position.X + velocity.X * dt;

            if (!CollidesWithWall(newX, position.Y))
                position.X = newX;

            // ===== Y AXIS COLLISION =====
            float newY = position.Y + velocity.Y * dt;

            if (!CollidesWithWall(position.X, newY))
            {
                position.Y = newY;
                isOnGround = false;
            }
            else
            {
                // If moving downward and hit wall → on ground
                if (velocity.Y > 0)
                    isOnGround = true;
                    doubleJump = true;

                velocity.Y = 0;
            }

            foreach (Enemy enemy in game.enemies)
            {
                if (Bounds.Intersects(enemy.Bounds))
                {
                    game.RestartLevel();
                    return;
                }
            }

            foreach (Spike spike in game.spikes)
            {
                if (Bounds.Intersects(spike.Bounds))
                {
                    game.RestartLevel();
                    return;
                }
            }

            foreach (Power power in game.powers)
            {
                if (Bounds.Intersects(power.Bounds))
                {
                    game.SF_powerup.Play();
                    game.powers.Remove(power);
                    powerUp = true;
                    isOnGround = true;
                    return;
                }
            }

            foreach (Exit exit in game.exits)
            {
                if (Bounds.Intersects(exit.Bounds))
                {
                    game.SF_powerup.Play();
                    MediaPlayer.Stop();
                    MediaPlayer.Play(game.Mus_final);
                    game.currentState = Game1.GameState.Win;
                    return;
                }
            }

        }
    }

A função Player(Game1 game, float x, float y) cria o peronagem na posição pré-determinada


A função CollidesWithWall(float x, float y) verifica se há colisão com uma parede


A função Update(GameTime gameTime) verifica se alguma das teclas que mexeo peronagem foi pressionada e se houve alguma colisão com inimigos, power up ou a saída e faz a ação da mesma.

• Enemy

A Classe Enemy coloca os inimigos nas suas posições e dá-lhes a sua "hitbox", variáveis e movimento

    public class Enemy
    {
        private Vector2 position;

        private float speed = 100f;

        private float startY;
        private float range = 150f;

        private int direction = 1;

        public Rectangle Bounds =>
    new Rectangle(
        (int)position.X,
        (int)position.Y,
        64,
        64
    );

        public Vector2 Position => position;

        public Enemy(float x, float y)
        {
            position = new Vector2(x * 64, y * 64);

            startY = position.Y;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.Y += speed * direction * dt;

            if (position.Y > startY + range)
                direction = -1;

            if (position.Y < startY - range)
                direction = 1;
        }
    }

A função Update(GameTime gameTime) faz com que o inimigose mexa de acordo com as variáveis

• FollowCamera
A Classe FollowCamera move a camara segundo o moimento do jogador

    public class FollowCamera
    {
        public Vector2 Position;

        public FollowCamera(Vector2 position)
        {
            Position = position;
        }

        public void Follow(Vector2 targetPosition, Vector2 screenSize)
        {
            Position = new Vector2(
                -targetPosition.X + (screenSize.X / 2),
                Position.Y = 0
            );
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(
                new Vector3(Position, 0)
            );
        }
    }

A função Follow(Vector2 targetPosition, Vector2 screenSize) centra a camara e faz com que não se possa mover verticalmente
A função Matrix GetTransform()  é a função que faz mexer a camara

• Spike, Power, Wall e Exit

A Classe Spike, Power, Wall e Exit fazem o mesmo.
São usadas para a verifiação de colisão com o jogador e para se inserirem no sítio certo 


    public class Spike
    {
        private Vector2 position;

        public Rectangle Bounds =>
    new Rectangle(
        (int)position.X,
        (int)position.Y,
        64,
        64
    );

        public Vector2 Position => position;

        public Spike(float x, float y)
        {
            position = new Vector2(x * 64, y * 64);

        }

        public void Update(GameTime gameTime)
        {

        }
    }


    public class Power
    {
        private Vector2 position;

        public Rectangle Bounds =>
    new Rectangle(
        (int)position.X,
        (int)position.Y,
        64,
        64
    );

        public Vector2 Position => position;

        public Power(float x, float y)
        {
            position = new Vector2(x * 64, y * 64);

        }

        public void Update(GameTime gameTime)
        {

        }
    }

        public class Wall
    {
        public Vector2 Position;

        public Rectangle Bounds =>
            new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                64,
                64
            );

        public Wall(int x, int y)
        {
            Position = new Vector2(x * 64, y * 64);
        }
    }


    public class Exit
    {
        private Vector2 position;

        public Rectangle Bounds =>
    new Rectangle(
        (int)position.X,
        (int)position.Y,
        64,
        64
    );

        public Vector2 Position => position;

        public Exit(float x, float y)
        {
            position = new Vector2(x * 64, y * 64);

        }

        public void Update(GameTime gameTime)
        {

        }
    }
