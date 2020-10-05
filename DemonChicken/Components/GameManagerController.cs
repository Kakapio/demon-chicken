using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace DemonChicken
{
    public class GameManagerController : Component, IUpdatable
    {
        public ProgressBar Bar { get; set; }
        public void Update()
        {
            Bar.Value = GameManager.PlayerHealth;
        }
    }
}