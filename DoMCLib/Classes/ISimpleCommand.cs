using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMC.Classes
{
    public interface ISimpleCommand<T1, T2>
    {
        T2 Execute(T1 data);
    }
}
