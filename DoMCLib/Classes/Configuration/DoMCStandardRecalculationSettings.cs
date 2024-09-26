namespace DoMCLib.Classes.Configuration
{
    public class DoMCStandardRecalculationSettings
    {

        public int NCycle = 10;

        public double StandardPercent = 10;
        public double Koefficient
        {
            get
            {
                return Math.Exp(Math.Log(StandardPercent / 100) / NCycle);
            }
        }
    }
}
