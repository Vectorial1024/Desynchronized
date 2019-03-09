using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS.Storage
{
    /// <summary>
    /// Implements IExposable.
    /// </summary>
    public class Pawn_TaleNewsRefArchive : IExposable
    {
        private List<TaleNewsReference> listTaleNewsRef;

        public Pawn_TaleNewsRefArchive()
        {

        }

        public void ExposeData()
        {
            throw new NotImplementedException();
        }
    }
}
