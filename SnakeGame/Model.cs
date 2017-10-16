using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SnakeGame
{
    public class Model
    {
        protected ArrayList oList;

        public Model()
        {
            oList = new ArrayList();
        }
        public void NotifyAll()
        {
            foreach (View m in oList)
            {
                m.Notify(this);
            }
        }

        public void AttachObserver(View m)
        {
            oList.Add(m);
        }

    }
}
