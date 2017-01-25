using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoyoBot
{
    class PoyoStreamList : List<PoyoStream>
    {
        public PoyoStreamList()
            : base()
        {
        }

        public bool Contains(string value)
        {
            foreach (PoyoStream p in this)
            {
                if (value == p.Name)
                {
                    return true;
                }
            }
            return false;
        }

        public PoyoStream Find(string value)
        {
            foreach (PoyoStream pS in this)
            {
                if (pS.Name == value.TrimStart('#'))
                {
                    return pS;
                }
            }
            return null;
        }

        public void Remove(string value)
        {
            if (Contains(value))
            {
                foreach (PoyoStream p in this)
                {
                    if (value == p.Name)
                    {
                        PoyoStream streamToRemove = p;                        
                        this.Remove(p);
                        streamToRemove = null;
                        return;
                    }
                }
            }
        }
    }
}
