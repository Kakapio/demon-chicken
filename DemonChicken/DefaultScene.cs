using DemonChicken;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
                player.SetPosition(100, 100)
                    .AddComponent(new PlayerController());
            player.UpdateOrder = 0;

            Camera.AddComponent(new FollowCamera(player));
            Camera.GetComponent<FollowCamera>().FollowLerp = 0.02f;
            Camera.UpdateOrder = 1;

            var tilemap = CreateEntity("tilemap");
            var mapData = Content.LoadTiledMap(@"Content\Tilemaps\SexChamber.tmx");
            var tiledMapRenderer = tilemap.AddComponent(new TiledMapRenderer(mapData, "Walls", true));
            tiledMapRenderer.SetLayersToRender(new[] {"Walls", "Floor"});

            tiledMapRenderer.RenderLayer = 10;
        }
    }
}

