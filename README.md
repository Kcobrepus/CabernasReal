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
        ...
        }

