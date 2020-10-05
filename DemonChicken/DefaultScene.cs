using System;
using DemonChicken;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.Sprites;
using Nez.UI;

namespace DemonChicken
{
	public class DefaultScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();
            
            //Set internal game resolution and then set window resolution
            SetDesignResolution(480, 270, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.SetSize(480 * 4, 270 * 4);
#if debug
            CreateEntity("demo imgui draw commands")
                .SetPosition(new Vector2(150, 150))
                .AddComponent<DemoComponent>();
#endif 

            var player = CreateEntity("Player");
                player.SetPosition(1606, 1650)
                    .AddComponent(new PlayerController());
            player.UpdateOrder = 1;

            var playerShadow = CreateEntity("Player_Shadow");
            var playerShadowTex = Content.LoadTexture(@"Content\Textures\ChickenShadow.png");
            playerShadow.Transform.Position = playerShadow.Transform.Position;
            playerShadow.Transform.LocalPosition = new Vector2(0, 56);
            playerShadow.AddComponent(new SpriteRenderer(playerShadowTex))
                .Transform.SetParent(player.Transform);

            Camera.AddComponent(new FollowCamera(player));
            Camera.GetComponent<FollowCamera>().FollowLerp = 0.04f;
            Camera.UpdateOrder = 2;

            var tilemap = CreateEntity("Tilemap");
            var mapData = Content.LoadTiledMap(@"Content\Tilemaps\Chicken_Map.tmx");
            var tiledMapRenderer = tilemap.AddComponent(new TiledMapRenderer(mapData, "Wall", true));
            tiledMapRenderer.SetLayersToRender(new[] {"Ground", "Lava", "Shade", "Building", "BuildingC", "BuildingTop", "Wall", "WallTop"});

            tiledMapRenderer.RenderLayer = 10;

            /*var scythe = CreateEntity("Scythe");
            var scytheTex = Content.LoadTexture(@"Content\Textures\Deaths_Scythe.png");
            scythe.AddComponent(new Interactable(scytheTex));*/

            var elizabeth = CreateEntity("Elizabeth");
            elizabeth.AddComponent(new Elizabeth(Content.LoadTexture(@"Content\Textures\Elizabeth.png")));
            elizabeth.Position = new Vector2(1695, 1675);
            
            var airwrecka = CreateEntity("Airwrecka");
            airwrecka.AddComponent(new Elizabeth(Content.LoadTexture(@"Content\Textures\Airwrecka.png")));
            airwrecka.Position = new Vector2(1273, 335);
            
            var titya = CreateEntity("Titya");
            titya.AddComponent(new Elizabeth(Content.LoadTexture(@"Content\Textures\Titya.png")));
            titya.Position = new Vector2(1126, 328);
            
            var keyman = CreateEntity("Keyman");
            keyman.AddComponent(new Keyman(Content.LoadTexture(@"Content\Textures\KeyMan.png")));
            keyman.Position = new Vector2(1816, 1186);
            
            var death = CreateEntity("Death");
            death.AddComponent(new Death(Content.LoadTexture(@"Content\Textures\Death.png")));
            death.Position = new Vector2(1669, 1539);

            var music = Content.LoadSoundEffect(@"Content\Audio\helljazz.wav");
            var musicInst = music.CreateInstance();
            musicInst.Play();
            
            SetupGUI(player);
        }

        private void SetupGUI(Entity player)
        {
            //Setup GUI
            var uiCanvas = CreateEntity("UICanvas").AddComponent(new UICanvas());
            uiCanvas.Entity.Transform.LocalPosition = new Vector2(-45, -45);
            uiCanvas.Entity.SetParent(player.Transform);

            var table = uiCanvas.Stage.AddElement(new Table());
            table.SetFillParent(true);

            var bar = new ProgressBar(0, 60, 0.1f, false, ProgressBarStyle.Create(Color.Orange, Color.Gray));
            bar.Value = 20;
            table.Add(bar);
            table.Top();
            table.PadTop(10f);
            table.Left();

            var gameManager = CreateEntity("GameManager")
                .AddComponent(new GameManagerController());
            gameManager.UpdateOrder = 0;
            gameManager.Bar = bar;
        }
    }
}

