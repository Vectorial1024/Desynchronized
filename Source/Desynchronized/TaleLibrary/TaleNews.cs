using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.TaleLibrary
{
    public class TaleNews
    {
        Tale tale;
        
        public int NewsID
        {
            get
            {
                return tale.id;
            }
        }

        public TaleNews(Tale tale)
        {
            this.tale = tale;
        }

        public override string ToString()
        {
            return tale + ", no further parameters.";
        }
    }
}
