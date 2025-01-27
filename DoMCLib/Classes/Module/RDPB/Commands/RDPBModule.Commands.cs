using DoMCLib.Classes.Configuration;
using DoMCLib.Classes.Module.RDPB.Classes;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {
        public class SendSetIsOkCommand : WaitingCommandBase// AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public SendSetIsOkCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(RDPBStatus)) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetIsOK);

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.SetIsOK.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }

            protected override void PrepareOutputData()
            {
                OutputData = status;
            }
        }
        public class LoadConfigurationCommand : AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public LoadConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(RemoveDefectedPreformBlockConfig), typeof(RDPBStatus)) { }
            protected override void Executing() => ((RDPBModule)Module).config = (DoMCLib.Classes.Configuration.RemoveDefectedPreformBlockConfig)InputData;
           /* protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.<>.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }*/
        }
        public class MakeBlockSendWorkingStateCommand : WaitingCommandBase //AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public MakeBlockSendWorkingStateCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(RDPBStatus)) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.GetParameters);

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.GetParameters.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }
        }

        public class SendManualCommand : WaitingCommandBase //AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public SendManualCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(string), typeof(RDPBStatus)) { }

            protected override void Executing() => ((RDPBModule)Module).SendManualCommandProc((string)(InputData ?? String.Empty));

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    status = (RDPBStatus)data!;
                    requestGot = true;
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }
        }

        public class SendSetIsBadCommand : WaitingCommandBase //AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public SendSetIsBadCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(RDPBStatus)) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetIsBad);
            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.SetIsBad.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }
        }
        public class SetCoolingBlockQuantityCommand : WaitingCommandBase //AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public SetCoolingBlockQuantityCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(int), typeof(RDPBStatus)) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetCoolingBlocks, (int)(InputData ?? 4));
            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.SetCoolingBlocks.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }
        }
        public class StartCommand : AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(RDPBStatus)) { }
            protected override void Executing() => ((RDPBModule)Module).Start();
            /*protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.<>.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }*/
        }
        public class StopCommand : AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(RDPBStatus)) { }

            protected override void Executing() => ((RDPBModule)Module).Stop();
            /*protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.<>.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }*/
        }
        public class TurnOffCommand : WaitingCommandBase //AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public TurnOffCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(RDPBStatus)) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.Off);
            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.Off.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }
        }
        public class TurnOnCommand : WaitingCommandBase //AbstractCommandBase
        {
            bool requestGot = false;
            RDPBStatus status = null;
            public TurnOnCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(RDPBStatus)) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.On);
            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return requestGot;
            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                var parts = NotificationName.Split('.');
                if (parts[0] == nameof(RDPBModule))
                {
                    if (parts[1] == RDPBCommandType.On.ToString())
                    {
                        if (parts[2] == StatusStringProccessResult.OK.ToString())
                        {
                            status = (RDPBStatus)data!;
                            requestGot = true;
                        }
                    }
                }
            }
            protected override void PrepareOutputData()
            {
                OutputData = status;
            }
        }
    }
}
