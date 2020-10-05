using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace DemonChicken
{
    public class Death : Component
    {
        private Texture2D tex;
        public Death(Texture2D tex)
        {
            this.tex = tex;
        }
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            SpriteAnimator animator = Entity.AddComponent(new SpriteAnimator());

            var animationSprites = Sprite.SpritesFromAtlas(tex, 64, 64).ToArray();

            animator.AddAnimation("idle", new []{animationSprites[0], animationSprites[1]}, 1);
            animator.AddAnimation("dead", new []{animationSprites[2]});
            animator.Play("idle", SpriteAnimator.LoopMode.Loop);
        }
    }
}