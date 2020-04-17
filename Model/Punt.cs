using System;

namespace OpgaveLabo
{
    public class Punt
    //Een punt is gekenmerkt door zijn x-en y-coördinaat.
    {
        #region Properties

        public double x { get; set; }
        public double y { get; set; }

        #endregion Properties

        //constructor
        public Punt(double d1, double d2)
        {
            this.x = d1;
            this.y = d2;
        }

        //gebruikt voor testen en debug
        public override string ToString()
        {
            return ("Punt met x,y: ({0},{1})", x, y).ToString();
        }

        #region equals & getHashCode

        public override bool Equals(object obj)
        {
            return obj is Punt punt &&
                   x == punt.x &&
                   y == punt.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        #endregion equals & getHashCode
    }
}