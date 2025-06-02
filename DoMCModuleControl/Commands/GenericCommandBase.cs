using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Commands
{
    public abstract class GenericCommandBase<TInput, TOutput> : AbstractCommandBase
    {
        public new TInput InputData { get; private set; } = default!;
        public new TOutput OutputData { get; protected set; } = default!;

        public GenericCommandBase(IMainController controller, AbstractModuleBase module)
            : base(controller, module, typeof(TInput), typeof(TOutput)) { }

        public override void SetInputData(object? inputData)
        {
            if (inputData is not TInput typedInput)
                throw new ArgumentException($"Ожидался тип {typeof(TInput).Name}, а передан {inputData?.GetType().Name ?? "null"}");
            InputData = typedInput;
            base.InputData = inputData;
        }

        protected void SetOutput(TOutput output)
        {
            OutputData = output;
            base.OutputData = output;
        }

        public TOutput GetOutput() => OutputData;
    }
    public abstract class GenericCommandBase<TOutput> : AbstractCommandBase
    {
        public new TOutput OutputData { get; protected set; } = default!;

        public GenericCommandBase(IMainController controller, AbstractModuleBase module)
            : base(controller, module, null, typeof(TOutput)) { }


        protected void SetOutput(TOutput output)
        {
            OutputData = output;
            base.OutputData = output;
        }

        public TOutput GetOutput() => OutputData;
    }

}
