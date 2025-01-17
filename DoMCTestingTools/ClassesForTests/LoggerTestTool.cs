using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoMCTestingTools.ClassesForTests
{
    public class LoggerTestTool
    {
        public static string GetLoggerTestModuleName()
        {
            var stackFrame = new StackFrame(1, true);
            var method = stackFrame.GetMethod();
            if (method == null) { return "Unknown"; }
            string fullMethodName = $"{method.DeclaringType?.Name}_{method.Name}";
            return fullMethodName;
        }

    }
}
