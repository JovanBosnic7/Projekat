namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class VpAlarmTip
    {

        public int Id { get; set; }
        public string NazivAlarma { get; set; }
        public int GrupaId { get; set; }
        public int KmDoIsteka { get; set; }
        public int DanaDoIsteka { get; set; }
        public int DanaDoAlarma { get; set; }
        public int Sort { get; set; }


        public virtual VpAlarmGrupa VpAlarmGrupa { get; set; }
    }
}
