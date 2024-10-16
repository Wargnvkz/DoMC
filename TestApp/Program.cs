namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var list = Commands.GetCommandList(typeof(DoMCLib.Classes.Module.LCB.LCBModule));
            Console.WriteLine(string.Join("\r\n\r\n", list));
        }
    }

    public class Commands
    {
        public static string CreateCommandText(Type moduleType, string MethodName, Type? InputType = null, Type? OutputType = null)
        {
            var text =
            $"            public class {MethodName}Command : AbstractCommandBase\r\n" +
            $"            {{\r\n" +
            $"                public {MethodName}Command(IMainController mainController, AbstractModuleBase module) : base(mainController, module, {(InputType == null ? "null" : "typeof(" + InputType.Name + ")")}, null) {{ }}\r\n";
            string OutputDataString = String.Empty;
            if (OutputType != null)
            {
                OutputDataString = "OutputData=";
            }
            string CallMethodString;
            if (InputType == null)
            {
                CallMethodString = $"(({moduleType.Name})Module).{MethodName}()";
            }
            else
            {
                CallMethodString = $"(({moduleType.Name})Module).{MethodName}(({InputType.Name}) InputData)";
            }
            text += $"                protected override void Executing() => {OutputDataString}{CallMethodString};\r\n";
            text += $"            }}\r\n";
            return text;
        }

        public static List<string> GetCommandList(Type type)
        {
            var list = new List<string>();
            var methods = type.GetMethods();// System.Reflection.BindingFlags.Public);
            foreach (var method in methods)
            {
                if (method.IsStatic || !method.IsPublic || method.IsGenericMethod) continue;
                //if (method.ReturnParameter.ParameterType.Name != "Void") continue;
                var parameters = method.GetParameters();
                if (parameters.Length <= 1)
                {
                    var text = CreateCommandText(type, method.Name, parameters.Length == 0 ? null : parameters[0].ParameterType);
                    list.Add(text);
                }
            }
            return list;
        }
    }
}
