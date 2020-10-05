using System;
using System.Collections;
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
        Running,
        Dead,
    }

    class PlayerController : Component, IUpdatable
    {
        public float MoveSpeed { get; set; } = 200;
        public float AttackCooldown { get; set; } = 0.2f;

        public int Lifespan { get; set; } = 60;

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
        private Health health;
        private CollisionResult collisionResult;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            mover = Entity.AddComponent(new Mover());
            animator = Entity.AddComponent(new SpriteAnimator());
            collider = Entity.AddComponent(new BoxCollider(-7, 48, 12, 8));
            health = Entity.AddComponent(new Health(600));
            
            health.OnDeath += OnDeath;
            animator.RenderLayer = 0;
            Core.StartCoroutine(TimeDamage());
            
            SetupInput();
            SetupAnimations();
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
            CheckInteractables();
            
            //Console.WriteLine(collider.Bounds.y);
            if (Input.IsKeyPressed(Keys.X)) health.Damage(20);
        }
        
        private void OnDeath()
        {
            playerState = PlayerState.Dead;
        }

        /// <summary>
        /// Deals damage over time to the player.
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimeDamage()
        {
            while (playerState != PlayerState.Dead)
            {
                health.Damage(1);
                yield return Coroutine.WaitForSeconds(1f);
            }
        }
        
        /// <summary>
        /// Add animations to SpriteAnimator.
        /// </summary>
        private void SetupAnimations()
        {
            var atlasTexture = Entity.Scene.Content.LoadTexture(@"Content\Textures\DemonChicken.png");
            var animationSprites = Sprite.SpritesFromAtlas(atlasTexture, 120, 113).ToArray();

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
            
            animator.AddAnimation("Attack", new SpriteAnimation(new[]
            {
                animationSprites[7],
                animationSprites[8],
                animationSprites[9],
                animationSprites[10],
                animationSprites[11],
                animationSprites[12],
                animationSprites[13]
            }, 10f));
            
            animator.AddAnimation(PlayerState.Dead.ToString(), new SpriteAnimation(new[]
            {
                animationSprites[14],
                animationSprites[15],
                animationSprites[16],
                animationSprites[17],
                animationSprites[18],
                animationSprites[19],
                animationSprites[20],
                animationSprites[21],
                animationSprites[22],
                animationSprites[23],
                animationSprites[24],
                animationSprites[25],
                animationSprites[26],
                animationSprites[27],
                animationSprites[28],
                animationSprites[29],
                animationSprites[30],
                animationSprites[31],
            }, 10f));
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
            //No movement if you're dead.
            if (playerState == PlayerState.Dead) return;
            
            Vector2 moveDir = new Vector2(horizontalInput.Value, verticalInput.Value);

            //Adjust for diagonal movement
            float moveMod = (moveDir.X != 0 && moveDir.Y != 0) ? 0.7071f : 1f;

            var movement = moveDir * MoveSpeed * moveMod * Time.DeltaTime;
            
            //No movement if trying to walk into something.
            if (moveDir.X < 0 && collisionResult.Normal.X > 0
                 || moveDir.X > 0 && collisionResult.Normal.X < 0)
            {
                return;
            }
            if (moveDir.Y < 0 && collisionResult.Normal.Y > 0
                || moveDir.Y > 0 && collisionResult.Normal.Y < 0)
            {
                return;
            }
            
            mover.CalculateMovement(ref movement, out collisionResult);
            subPixelVector.Update(ref movement);
            mover.ApplyMovement(movement);
        }

        private void CheckInteractables()
        {
            //Physics.Linecast(new Vector2(collider.Bounds.Left), )
        }

        private void PollAttacks()
        {
            if (playerState == PlayerState.Dead) return;
            
            if (attackInput.IsPressed && timeSinceAttack >= AttackCooldown)
            {
            }
        }

        /// <summary>
        /// Update the player's state depending on current input.
        /// </summary>
        private void UpdateState()
        {
            //Nothing can break out of death.
            if (playerState == PlayerState.Dead) return;
            
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
            if (playerState == PlayerState.Dead)
            {
                if (!animator.IsAnimationActive(PlayerState.Dead.ToString()))
                {
                    animator.Play(PlayerState.Dead.ToString(), SpriteAnimator.LoopMode.ClampForever);
                }
                return;
            }
            
            Vector2 moveDir = new Vector2(horizontalInput.Value, verticalInput.Value);

            //Flip sprites.
            if (moveDir.X < 0)
            {
                animator.FlipX = false;
            }
            else if (moveDir.X > 0)
            {
                animator.FlipX = true;
            }
            
            //Console.WriteLine($"Movement {moveDir.X} : {moveDir.Y} and collision {collisionResult.Normal.ToString()}");
            //Play idle animation if colliding with walls
            if ((moveDir.X < 0 && collisionResult.Normal.X > 0 
                 || moveDir.X > 0 && collisionResult.Normal.X < 0
                 || moveDir.Y < 0 && collisionResult.Normal.Y > 0
                 || moveDir.Y > 0 && collisionResult.Normal.Y < 0) && playerState == PlayerState.Running)
            {
                if (!animator.IsAnimationActive(PlayerState.Idle.ToString()))
                {
                    animator.Play(PlayerState.Idle.ToString(), SpriteAnimator.LoopMode.Loop);
                }

                return;
            }

            if (playerState == PlayerState.Idle && !animator.IsAnimationActive(PlayerState.Idle.ToString()))
            {
                animator.Play(PlayerState.Idle.ToString(), SpriteAnimator.LoopMode.Loop);
            }
            else if (playerState == PlayerState.Running && !animator.IsAnimationActive(PlayerState.Running.ToString()))
            {
                animator.Play(PlayerState.Running.ToString(), SpriteAnimator.LoopMode.Loop);
            }
        }
    }
}
