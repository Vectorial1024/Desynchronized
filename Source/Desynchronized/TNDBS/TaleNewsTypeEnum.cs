using System;

namespace Desynchronized.TNDBS
{
    public enum TaleNewsTypeEnum
    {
        Default = 0,
        PawnDied,
        PawnBanished,
        PawnKidnapped,
        PawnHarvested,
        PawnSold
    }

    public static class TaleNewsTypeMapperUtility
    {
        public static Type GetTypeForEnum(this TaleNewsTypeEnum typeEnum)
        {
            switch (typeEnum)
            {
                case TaleNewsTypeEnum.PawnDied:
                    return typeof(TaleNewsPawnDied);
                default:
                    return typeof(Object);
            }
        }
    }
}
