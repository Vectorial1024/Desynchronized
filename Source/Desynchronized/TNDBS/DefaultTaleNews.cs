using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class DefaultTaleNews : TaleNews
    {
        public DefaultTaleNews()
        {

        }

        public override string GetNewsIdentifier()
        {
            return "Default TaleNews";
        }

        protected override void ConductSaveFileIO()
        {
            return;
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            DesynchronizedMain.LogError("Somebody tried to trigger a thought-giving process using a default TaleNews. Nothing was done." + Environment.StackTrace);
        }
    }
}
