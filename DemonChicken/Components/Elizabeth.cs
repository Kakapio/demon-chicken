using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace DemonChicken
{
    public class Elizabeth : Component
    {
        private Texture2D tex;
        public Elizabeth(Texture2D tex)
        {
            this.tex = tex;
        }
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            SpriteAnimator animator = Entity.AddComponent(new SpriteAnimator());

            var animationSprites = Sprite.SpritesFromAtlas(tex, 64, 64).ToArray();

            animator.AddAnimation("dance", animationSprites);
            animator.Play("dance", SpriteAnimator.LoopMode.PingPong);
        }
    }
}