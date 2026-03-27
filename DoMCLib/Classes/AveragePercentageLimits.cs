using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes
{
    [DataContract]
    public class AveragePercentageLimits
    {
        [DataMember]
        public double WarningPercentage;
        [DataMember]
        public double DefectPercentage;
        public AveragePercentageLimits Clone()
        {
            return new AveragePercentageLimits()
            {
                DefectPercentage = DefectPercentage,
                WarningPercentage = WarningPercentage,
            };
        }
    }
}
