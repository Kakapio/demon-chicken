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
            SetDesignResolution(Screen.Width, Screen.Height, Scene.SceneResolutionPolicy.None);

#if debug
            CreateEntity("demo imgui draw commands")
                .SetPosition(new Vector2(150, 150))
                .AddComponent<DemoComponent>()
                .AddComponent(new PrototypeSpriteRenderer(20, 20));
#endif 

            var logo = Content.LoadTexture(@"Content\Textures\nez-logo-black.png");
            CreateEntity("Player")
                .SetPosition(Screen.Center)
                .AddComponent(new PlayerController());
        }
    }
}

