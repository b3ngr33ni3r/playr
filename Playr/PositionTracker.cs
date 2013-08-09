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

        override public void Add(Vector item)
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
        public HandsTracker(int historySize,HandList list = null)
            : base()
        {
            this.historySize = historySize;
            if (list != null)
            {
                foreach (Hand h in list)
                {
                    InitByIdAndPosition(h.Id,h.PalmPosition);
                }
            }
        }

        public void AddOrUpdate(Hand item)
        {
            if (ContainsKey(item.Id))
                this[item.Id].Add(item.PalmPosition);
            else
                InitByIdAndPosition(item.Id, item.PalmPosition);
        }

        public void AddOrUpdate(HandList list)
        {
            foreach (Hand item in list)
            {
                if (ContainsKey(item.Id))
                    this[item.Id].Add(item.PalmPosition);
                else
                    InitByIdAndPosition(item.Id, item.PalmPosition);
            }
        }

        private void InitByIdAndPosition(int id, Vector position)
        {
            if (!ContainsKey(id))
            {
                this.Add(id, new PositionTracker(historySize));
                this[id].Add(position);
            }
        }
    }
}
