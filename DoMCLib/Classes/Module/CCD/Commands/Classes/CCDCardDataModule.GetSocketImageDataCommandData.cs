/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class GetSocketImageDataCommandData
        {
            public ApplicationContext Context;
            public int EqipmentSocket;
        }

    }

}
