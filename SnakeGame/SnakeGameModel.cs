using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class SnakeBodyLoc
    {
        private int _x;
        private int _y;
        public int X { get { return _x; } } // these two properties are read-only 
        public int Y { get { return _y; } }
        public SnakeBodyLoc(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }

    public class SnakeGameModel : Model
    {
        public const int MOVE_LEFT = 0;
        public const int MOVE_UP = 1;
        public const int MOVE_RIGHT = 2;
        public const int MOVE_DOWN = 3;

        public const int BOARD_EMPTY = 0;
        public const int BOARD_SNAKE = 1;
        public const int BOARD_FOOD = 2;
        public const int BOARD_WALL = 3;

        public const int SNAKE_INIT_SIZE = 5;

        protected int[,] _board;
        protected Random rand;
        protected int boardWidth;
        protected int boardHeight;
        protected int direction;
        protected int curHeadX;
        protected int curHeadY;

        protected ArrayList snakeBody;
        protected Boolean _isHit;
        protected Boolean _isEating;

        public const int TIME_BASE = 250;
        public const int MAX_SPEED = 250;
        protected static int _speed = 1;

        public static int Speed
        {
            set
            {
                if (value > 0 && value < SnakeGameModel.MAX_SPEED)
                {
                    _speed = value;
                }
            }
            get { return _speed; }
        }
        
        // Accessor for outsider
        public Boolean isHit { get{ return _isHit; } }  // this property is also readonly for outsider
        public Boolean isEating { get { return _isEating;  } }
        public int[,] Board { get { return _board; } }

        public SnakeGameModel(int w, int h)
        {
            boardWidth = w; // width = w = X = col
            boardHeight = h;// height = h = Y = row
            _board = new int[w, h]; // order is X,Y
            // clear the array, just in case
            foreach (int j in Enumerable.Range(1, h - 1))
            {
                // Draw play area
                foreach (int i in Enumerable.Range(1, w - 1))
                {
                    _board[i, j] = BOARD_EMPTY;
                }
                // draw left&right wall
                _board[0, j] = BOARD_WALL;
                _board[w - 1, j] = BOARD_WALL;
            }
            // Draw top&bottom wall
            foreach (int j in Enumerable.Range(0, w))
            {
                _board[j, 0] = BOARD_WALL;
                _board[j, h - 1] = BOARD_WALL;
            }
            rand = new Random();
            snakeBody = new ArrayList();
            
         // random the first body sequence, set the head location on the bottom part of the screen
            curHeadY = rand.Next(h/2) + 1;
            curHeadX = rand.Next(w - 1) + 1;
            for (int i = 0; i != SNAKE_INIT_SIZE; i++)
            {
                curHeadY++;
                //Snake.Debug("new snake body at [" + curHeadX + ", " + curHeadY + "]");
                snakeBody.Add(new SnakeBodyLoc(curHeadX, curHeadY));
                _board[curHeadX, curHeadY] = BOARD_SNAKE;
            }
            // always start going down
            direction = MOVE_DOWN;

            // random new food;
            RandomFood();

            _isHit = false;
            NotifyAll();
        }

        protected void RandomFood()
        {

            // random new food
            int x, y;
            do
            {
                x = rand.Next(boardWidth);
                y = rand.Next(boardHeight);
            } while (isSnakeBody(x, y));
            _board[x, y] = BOARD_FOOD;
        }

        protected bool isSnakeBody(int x, int y)
        {
            foreach (SnakeBodyLoc sbl in snakeBody)
            {
                if (sbl.X == x && sbl.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetDirection(int d)
        {
            direction = d;
        }

        public void Move()
        {
            if (direction == MOVE_DOWN)
            {
                curHeadY++;
            }
            else if (direction == MOVE_UP)
            {
                curHeadY--;
            }
            else if (direction == MOVE_LEFT)
            {
                curHeadX--;
            }
            else if (direction == MOVE_RIGHT)
            {
                curHeadX++;
            }

            // hit myself?
            if (isSnakeBody(curHeadX, curHeadY))
            {
                _isHit = true;
                return;
            }
            // hit wall?
            if (_board[curHeadX, curHeadY] == BOARD_WALL)
            {
                _isHit = true;
                return;
            }
            // hit food?
            if (_board[curHeadX, curHeadY] == BOARD_FOOD)
            {
                _isEating = true;
                Speed += 1;
            }
            
            // add new head
            //Snake.Debug("new snake body at [" + curHeadX + ", " + curHeadY + "]");
            snakeBody.Add(new SnakeBodyLoc(curHeadX, curHeadY));
            _board[curHeadX, curHeadY] = BOARD_SNAKE;

            // remove tail;
            if (!_isEating)
            {
                try
                {
                    SnakeBodyLoc sbl = (SnakeBodyLoc)snakeBody[0];
                    snakeBody.RemoveAt(0);
                    _board[sbl.X, sbl.Y] = BOARD_EMPTY;
                }
                catch
                {
                    // We must not be here
                }
            }
            else
            {
                RandomFood();
            }

            // Don't call the Notify here!!!!!, let's Update handle that
        }

        public void Update()
        {
            NotifyAll();
            _isEating = false;
        }

        public int SnakeLength()
        {
            return snakeBody.Count;
        }
    }
}
