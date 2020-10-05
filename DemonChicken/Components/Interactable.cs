using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace DemonChicken
{
    public class Interactable : Component
    {
        private SpriteOutlineRenderer spriteOutline;
        private SpriteRenderer renderer;
        public Interactable(Texture2D tex)
        {
            renderer = new SpriteRenderer(tex);
        }
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            spriteOutline = Entity.AddComponent(new SpriteOutlineRenderer(renderer));
            spriteOutline.Color = Color.Green;
            spriteOutline.RenderLayer = 0;
        }
    }
}