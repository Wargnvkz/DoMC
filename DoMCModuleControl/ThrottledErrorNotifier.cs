using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DoMCModuleControl
{
    public class ThrottledErrorNotifier
    {
        Dictionary<(string Name, string Message), (DateTime, int)> errors = new Dictionary<(string Name, string Message), (DateTime, int)>();
        Observer Observer;
        int IgnoreErrorsSeconds;
        int MaxErrorCounter;
        private readonly object _lock = new object();

        public ThrottledErrorNotifier(Observer observer, int ignoreErrorsBeforeSeconds, int maxErrorCounter)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            Observer = observer;
            IgnoreErrorsSeconds = ignoreErrorsBeforeSeconds;
            MaxErrorCounter = maxErrorCounter;
        }
        public void SendError(string Name, Exception error)
        {
            lock (_lock)
            {
                CleanupOldErrors();
                if (!errors.ContainsKey((Name, error.Message)))
                {
                    NotifyObserver(Name, error);
                }
                else
                {
                    var ErrParameters = errors[(Name, error.Message)];
                    if ((DateTime.Now - ErrParameters.Item1).TotalSeconds > IgnoreErrorsSeconds || MaxErrorCounter < ErrParameters.Item2)
                    {
                        NotifyObserver(Name, error);
                    }
                    else
                    {
                        errors[(Name, error.Message)] = new(ErrParameters.Item1, ErrParameters.Item2 + 1);

                    }
                }
            }
        }

        private void NotifyObserver(string name, Exception error)
        {
            Observer.Notify(name, error);
            errors[(name, error.Message)] = (DateTime.Now, 1);
        }

        private void CleanupOldErrors()
        {
            var now = DateTime.Now;
            var keysToRemove = new List<(string, string)>();

            foreach (var kvp in errors)
            {
                if ((now - kvp.Value.Item1).TotalSeconds > IgnoreErrorsSeconds)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                errors.Remove(key);
            }
        }
    }
}
