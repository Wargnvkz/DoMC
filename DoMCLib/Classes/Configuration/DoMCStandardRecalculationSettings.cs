namespace DoMCLib.Classes.Configuration
{
    public class DoMCStandardRecalculationSettings
    {

        public int NCycle;

        public double StandardPercent;
        public double Koefficient
        {
            get
            {
                return Math.Exp(Math.Log(StandardPercent / 100) / NCycle);
            }
        }
    }
}
