using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace DemonChicken
{
    public class Keyman : Component
    {
        private Texture2D tex;
        public Keyman(Texture2D tex)
        {
            this.tex = tex;
        }
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            SpriteAnimator animator = Entity.AddComponent(new SpriteAnimator());

            var animationSprites = Sprite.SpritesFromAtlas(tex, 33, 47).ToArray();

            animator.AddAnimation("dance", animationSprites);
            animator.Play("dance", SpriteAnimator.LoopMode.Loop);
        }
    }
}