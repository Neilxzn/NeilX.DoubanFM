using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.Core.Common
{
    public class SingletonProvider<T> where T:new()
    {
        public SingletonProvider() { }

        public static T Instance
        {
            get
            {
                return SingletonCreator.Instance;
            }
        }


        class SingletonCreator
        {
            static SingletonCreator() { }

            internal static readonly T Instance = new T();
        }
    }

    
}
