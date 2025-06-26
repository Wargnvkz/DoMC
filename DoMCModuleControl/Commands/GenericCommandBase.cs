using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Commands
{
    public abstract class GenericCommandBase<TInput, TOutput> : AbstractCommandBase, IExecuteCommandAsync<TInput, TOutput>
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

        public async Task<TOutput> ExecuteCommandAsync(TInput input)
        {
            return await base.ExecuteCommandAsync<TOutput>(input);
        }
    }
    public abstract class GenericCommandBase<TOutput> : AbstractCommandBase, IExecuteCommandAsync<TOutput>
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
        public new async Task<TOutput> ExecuteCommandAsync()
        {
            return await base.ExecuteCommandAsync<TOutput>();

        }

    }

    public abstract class GenericCommandBase : AbstractCommandBase, IExecuteCommandAsync
    {

        public GenericCommandBase(IMainController controller, AbstractModuleBase module)
            : base(controller, module, null, null) { }
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }

    public interface IExecuteCommandAsync<TInput, TOutput>
    {
        public Task<TOutput> ExecuteCommandAsync(TInput input);
    }
    public interface IExecuteCommandAsync<TOutput>
    {
        public Task<TOutput> ExecuteCommandAsync();
    }
    public interface IExecuteCommandAsync
    {
        public Task ExecuteCommandAsync();
    }
}
