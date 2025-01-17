using DoMCLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMC.Classes
{
    public interface IDoMCSettingsUpdatedProvider
    {
        DoMCLib.Classes.DoMCApplicationContext GetContext();
        event EventHandler SettingsUpdated;
    }
}
