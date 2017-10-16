using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SnakeGame
{
    public class SnakeGameController : Controller
    {
        Timer timer;

        public SnakeGameController()
        {
            // update the board every one second;
            timer = new Timer(SnakeGameModel.TIME_BASE / SnakeGameModel.Speed);
            timer.Enabled = false;
            timer.Elapsed += this.OnTimedEvent;
        }


        public void KeyUpHandled(KeyboardState ks)
        {
            int direction = -1;
            Keys[] keys = ks.GetPressedKeys();
           
            if (keys.Contains(Keys.Up))
            {
                direction = SnakeGameModel.MOVE_UP;
            }
            else if(keys.Contains(Keys.Down))
            {
                direction = SnakeGameModel.MOVE_DOWN;
            }
            else if(keys.Contains(Keys.Left))
            {
                direction = SnakeGameModel.MOVE_LEFT;
            }
            else if(keys.Contains(Keys.Right))
            {
                direction = SnakeGameModel.MOVE_RIGHT;
            }
            // Find all snakeboard model we know
            if (direction == -1) return;
            foreach (Model m in mList)
            {
                if (m is SnakeGameModel)
                {
                    // Tell the model to update
                    SnakeGameModel sbm = (SnakeGameModel)m;
                    sbm.SetDirection(direction);
                }
            }

        }


        public void Start()
        {
            timer.Enabled = true; 
        }

        public void Stop()
        {
            // Stop the game
            timer.Enabled = false;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Snake.Debug("TE");
            foreach (Model m in mList)
            {
                if (m is SnakeGameModel)
                {
                    SnakeGameModel sbm = (SnakeGameModel)m;
                    sbm.Move();
                    sbm.Update();
                }
            }
            timer.Interval = SnakeGameModel.TIME_BASE / SnakeGameModel.Speed;
        }

    }
}
