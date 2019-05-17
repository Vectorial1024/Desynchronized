﻿using System;
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

        public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
        {
            throw new NotImplementedException();
        }

        public override string GetNewsIdentifier()
        {
            return "Default TaleNews";
        }

        public override bool IsValid()
        {
            return true;
        }

        internal override void SelfVerify()
        {
            // No need to self-verify; this is always valid.
            return;
        }

        public override bool PawnIsInvolved(Pawn pawn)
        {
            return false;
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