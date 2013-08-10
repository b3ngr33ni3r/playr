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
    class HandsTracker : Dictionary<int,PositionTracker>
    {
        
        private int historySize = -1;
        private Dictionary<int, long> stamps;
        public HandsTracker(int historySize,long timestamp,HandList list)
            : base()
        {
            stamps = new Dictionary<int, long>();
            this.historySize = historySize;
            if (list != null)
            {
                foreach (Hand h in list)
                {
                    InitByHand(h,timestamp);
                }
            }
        }

        public void AddOrUpdate(Hand item,long timestamp)
        {
            if (ContainsKey(item.Id))
                this[item.Id].Add(item.PalmPosition);
            else
                InitByHand(item,timestamp);
        }

        public void AddOrUpdate(HandList list,long timestamp)
        {
            foreach (Hand item in list)
            {
                if (ContainsKey(item.Id))
                {
                    this[item.Id].Add(item.PalmPosition);
                    stamps[item.Id] = timestamp;
                }
                else
                {
                    InitByHand(item,timestamp);
                }
            }
        }

        private void InitByHand(Hand h,long timestamp)
        {
            if (!ContainsKey(h.Id))
            {
                this.Add(h.Id, new PositionTracker(historySize));
                this[h.Id].Add(h.PalmPosition);
                stamps.Add(h.Id, timestamp);
            }
        }

        //any data from before expirationAmount gets removed
        public void Clean(long timestamp,long expirationAmount)
        {
            foreach (int h in this.Keys)
            {
                if (timestamp - expirationAmount > stamps[h])
                {
                    this[h].Clear();
                    this.Remove(h);
                }
            }
        }
    }
}
