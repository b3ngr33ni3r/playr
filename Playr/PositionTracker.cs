using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

namespace Playr
{
    enum Axis { X, Y, Z }
    class PositionTracker : List<Vector>
    {

        private int limit;

        public PositionTracker(int limit = -1)
            : base()
        {
            this.limit = limit;
        }

        public int Limit
        {
            get { return limit; }
            set { limit = value; if (Count > limit) base.RemoveRange(0,limit); }
        }

        public bool ReachedLimit
        {
            get { return (Count == limit); }
        }

        public void Add(Vector item)
        {
            if (limit == -1 || Count < limit)
                base.Add(item);
        }

        public float DistanceOnAxis(Axis axis)
        {
            float total = 0;
            foreach (Vector v in this)
            {
                if (axis == Axis.X)
                    total += v.x;
                else if (axis == Axis.Y)
                    total += v.y;
                else if (axis == Axis.Z)
                    total += v.z;
            }
            return total;
        }

        public bool ContainsDistanceOnAxis(float distance, Axis axis)
        {
            return (DistanceOnAxis(axis) >= distance);
        }

    }
    class HandsTracker : Dictionary<Hand,PositionTracker>
    {
        
        private int historySize = -1;
        public HandsTracker(int historySize,HandList list = null)
            : base()
        {
            this.historySize = historySize;
            if (list != null)
            {
                foreach (Hand h in list)
                {
                    InitByHand(h);
                }
            }
        }

        public void AddOrUpdate(Hand item)
        {
            if (ContainsKey(item))
                this[item].Add(item.PalmPosition);
            else
                InitByHand(item);
        }

        public void AddOrUpdate(HandList list)
        {
            foreach (Hand item in list)
            {
                if (ContainsKey(item))
                    this[item].Add(item.PalmPosition);
                else
                    InitByHand(item);
            }
        }

        private void InitByHand(Hand h)
        {
            if (!ContainsKey(h))
            {
                this.Add(h, new PositionTracker(historySize));
                this[h].Add(h.PalmPosition);
            }
        }

        //any data from before expirationAmount gets removed
        public void Clean(long timestamp,long expirationAmount)
        {
            foreach (Hand h in this.Keys)
            {
                if (timestamp - expirationAmount > h.Frame.Timestamp)
                {
                    this[h].Clear();
                    this.Remove(h);
                }
            }
        }
    }
}
