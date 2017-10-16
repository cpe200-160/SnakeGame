using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public interface View
    {
        void setController(Controller c);
        void Notify(Model m);
    }
}
