using System;
using System.Collections.Generic;

namespace OpgaveLabo
{
    public class Knoop
    //Een knoop heeft een knoop-ID en verwijst naar een punt.
    {
        #region Properties

        public int knoopID { get; set; }
        public Punt punt { get; set; }

        #endregion Properties

        public Knoop(int i, Punt p)
        {
            this.knoopID = i;
            this.punt = p;
        }

        public override string ToString()
        {
            return ("Knoop met id " + knoopID + " , verwijst naar punt x,y: ( " + punt.x + " , " + punt.y + " )");
        }

        #region equals & getHashCode

        public override bool Equals(object obj)
        {
            return obj is Knoop knoop &&
                   knoopID == knoop.knoopID &&
                   EqualityComparer<Punt>.Default.Equals(punt, knoop.punt);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(knoopID, punt);
        }

        #endregion equals & getHashCode

        //
    }
}