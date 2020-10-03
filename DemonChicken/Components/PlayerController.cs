using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace DemonChicken
{
    internal enum PlayerState
    {
        Idle,
        Running
    }

    class PlayerController : Component, IUpdatable
    {
        public float MoveSpeed { get; set; } = 200;
        public float AttackCooldown { get; set; } = 0.2f;

        private PlayerState playerState;
        private float timeSinceAttack;
        private SubpixelVector2 subPixelVector = new SubpixelVector2();

        private Keys leftKey = Keys.A;
        private Keys rightKey = Keys.D;
        private Keys upKey = Keys.W;
        private Keys downKey = Keys.S;
        private VirtualButton attackInput;
        private VirtualIntegerAxis horizontalInput;
        private VirtualIntegerAxis verticalInput;
        private Mover mover;
        private SpriteAnimator animator;
        private Collider collider;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            mover = Entity.AddComponent(new Mover());
            animator = Entity.AddComponent(new SpriteAnimator());
            collider = Entity.AddComponent(new BoxCollider());

            SetupInput();
            SetupAnimations();
        }

        /// <summary>
        /// Add animations to SpriteAnimator.
        /// </summary>
        private void SetupAnimations()
        {
            var atlasTexture = Entity.Scene.Content.LoadTexture(@"Content\Textures\DemonChicken.png");
            var animationSprites = Sprite.SpritesFromAtlas(atlasTexture, 25, 31).ToArray();

            animator.AddAnimation(PlayerState.Idle.ToString(), new SpriteAnimation(new[]
            {
                animationSprites[0],
                animationSprites[1],
                animationSprites[2]
            }, 8f));

            animator.AddAnimation(PlayerState.Running.ToString(), new SpriteAnimation(new[]
            {
                animationSprites[3],
                animationSprites[4],
                animationSprites[5],
                animationSprites[6]
            }, 10f));
        }

        public override void OnRemovedFromEntity()
        {
            base.OnRemovedFromEntity();

            attackInput.Deregister();
            horizontalInput.Deregister();
            verticalInput.Deregister();
        }

        public void Update()
        {
            timeSinceAttack += Time.DeltaTime;

            Move();
            UpdateState();
            UpdateAnimation();
        }

        /// <summary>
        /// Configures input for both keyboard and gamepad. 
        /// </summary>
        private void SetupInput()
        {
            attackInput = new VirtualButton();
            attackInput.Nodes.Add(new VirtualButton.MouseLeftButton());
            attackInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

            horizontalInput = new VirtualIntegerAxis();
            horizontalInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            horizontalInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            horizontalInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, leftKey, rightKey));

            verticalInput = new VirtualIntegerAxis();
            verticalInput.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
            verticalInput.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
            verticalInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, upKey, downKey));
        }

        /// <summary>
        /// Execute movement-related logic on every update.
        /// </summary>
        private void Move()
        {
            Vector2 moveDir = new Vector2(horizontalInput.Value, verticalInput.Value);

            //Adjust for diagonal movement
            float moveMod = (moveDir.X != 0 && moveDir.Y != 0) ? 0.7071f : 1f;

            var movement = moveDir * MoveSpeed * moveMod * Time.DeltaTime;
            mover.CalculateMovement(ref movement, out CollisionResult res);
            subPixelVector.Update(ref movement);
            mover.ApplyMovement(movement);
        }

        private void PollAttacks()
        {
            if (attackInput.IsPressed && timeSinceAttack >= AttackCooldown)
            {
                //TODO: Add attack system
            }
        }

        /// <summary>
        /// Update the player's state depending on current input.
        /// </summary>
        private void UpdateState()
        {
            Vector2 moveDir = new Vector2(horizontalInput.Value, verticalInput.Value);

            if (moveDir != Vector2.Zero)
            {
                playerState = PlayerState.Running;
            }
            else
            {
                playerState = PlayerState.Idle;
            }
        }

        /// <summary>
        /// Play the proper animation according to the player's state.
        /// </summary>
        private void UpdateAnimation()
        {
            if (playerState == PlayerState.Idle && !animator.IsAnimationActive(PlayerState.Idle.ToString()))
            {
                animator.Play(PlayerState.Idle.ToString(), SpriteAnimator.LoopMode.Loop);
            }
            else if (playerState == PlayerState.Running && !animator.IsAnimationActive(PlayerState.Running.ToString()))
            {
                animator.Play(PlayerState.Running.ToString(), SpriteAnimator.LoopMode.Loop);
            }

            //Flip sprites.
            Vector2 moveDir = new Vector2(horizontalInput.Value, verticalInput.Value);

            if (moveDir.X < 0)
            {
                animator.FlipX = false;
            }
            else if (moveDir.X > 0)
            {
                animator.FlipX = true;
            }
        }
    }
}
