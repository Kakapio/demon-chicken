using System;
using DemonChicken;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

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
            player.UpdateOrder = 0;

            var playerShadow = CreateEntity("Player_Shadow");
            var playerShadowTex = Content.LoadTexture(@"Content\Textures\ChickenShadow.png");
            playerShadow.Transform.Position = playerShadow.Transform.Position;
            playerShadow.Transform.LocalPosition = new Vector2(0, 56);
            playerShadow.AddComponent(new SpriteRenderer(playerShadowTex))
                .Transform.SetParent(player.Transform);

            Camera.AddComponent(new FollowCamera(player));
            Camera.GetComponent<FollowCamera>().FollowLerp = 0.04f;
            Camera.UpdateOrder = 1;

            var tilemap = CreateEntity("tilemap");
            var mapData = Content.LoadTiledMap(@"Content\Tilemaps\Chicken_Map.tmx");
            var tiledMapRenderer = tilemap.AddComponent(new TiledMapRenderer(mapData, "Building", true));
            tiledMapRenderer.SetLayersToRender(new[] {"Ground2", "Lava", "Shade", "Building", "Building Top", "Wall", "Wall Tops"});

            tiledMapRenderer.RenderLayer = 10;
        }
    }
}

