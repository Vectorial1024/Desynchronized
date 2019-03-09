using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS.Storage
{
    public class TaleNewsDatabase: IExposable
    {
        private int nextID = 0;

        /// <summary>
        /// Please cast the result yourselves.
        /// Can return null if you gave in invalid newsTypeEnum.
        /// </summary>
        /// <param name="newsTypeEnum"></param>
        /// <returns></returns>
        public static TaleNews CreateTaleNews(TaleNewsTypeEnum newsTypeEnum)
        {
            TaleNews result = Activator.CreateInstance(TaleNewsTypeMapper.GetTypeForEnum(newsTypeEnum)) as TaleNews;
            if (result != null)
            {
                DesynchronizedMain.TaleNewsDatabase.InitializeTaleNewsID(result);
            }
            return result;
        }

        public void ExposeData()
        {
            throw new NotImplementedException();
        }

        public void InitializeTaleNewsID(TaleNews taleNews)
        {
            if (!taleNews.IsRegistered)
            {
                taleNews.UniqueID = nextID;
                nextID++;
            }
        }
    }
}
