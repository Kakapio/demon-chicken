using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace DemonChicken
{
    internal enum PlayerState
    {
        Idle,
        Running
    }

    class PlayerController : Component, IUpdatable
    {
        public float MoveSpeed { get; set; } = 175f;

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
            Move();
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
    }
}
