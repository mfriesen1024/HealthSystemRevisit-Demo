using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health
{
    /// <summary>
    /// Contains info about an attack.
    /// </summary>
    public struct HitData
    {
        public int damage;
        public bool isTrueDamage;

        public HitData(int damage, bool isTrueDamage=false)
        {
            this.damage = damage;
            this.isTrueDamage = isTrueDamage;
        }
    }
}
