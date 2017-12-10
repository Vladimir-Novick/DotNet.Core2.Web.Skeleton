

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DotNet.Core2.Web.Skeleton.Code
{
    public class DataCache
    {


        public static ConcurrentBag<UserDataItem> UserDataCache = new ConcurrentBag<UserDataItem>();
    }
}
