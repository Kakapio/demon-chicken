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
            SetDesignResolution(384, 216, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
#if debug
            CreateEntity("demo imgui draw commands")
                .SetPosition(new Vector2(150, 150))
                .AddComponent<DemoComponent>()
                .AddComponent(new PrototypeSpriteRenderer(20, 20));
#endif 

            CreateEntity("Player")
                .SetPosition(Vector2.Zero)
                .AddComponent(new PlayerController());
        }
    }
}

