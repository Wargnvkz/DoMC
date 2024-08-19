using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    // шаблон команды. Каждая команда сама выполняет код. Возможно будет нормальным создать в каждом модуле фабрику, чтобы команды не нужно было извращенно собирать
    //TODO: Сделать фабрику комманд, чтобы ее можно было в каждом модуле реализовать и получать нужные команды
    public abstract class ICommand
    {
        public void ExecuteCommand()
        {
            IsRunning = true;
            try
            {
                Execution();
            }
            finally
            {
                IsRunning = false;
            }
        }
        protected abstract void Execution();
        public bool IsRunning { get; private set; }
    }

    //TODO: Удалить тестовый регион после проверки на адекватность этого кода
    #region Test region
    public class ThisCommand : ICommand
    {
        protected override void Execution()
        {

        }
    }

    public class Test
    {
        public Test()
        {
            var CMD = new ThisCommand();
            CMD.ExecuteCommand();
        }
    }
    #endregion
}
