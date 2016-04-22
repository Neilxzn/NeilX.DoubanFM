using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.Common
{
    public interface IVMNavigate<T>
    {
        void OnNavigatedTo(T t);
        void OnNavigatedFrom(T t);
    }
}
