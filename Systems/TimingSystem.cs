using RLNETConsoleGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Systems
{
    public class TimingSystem
    {
        private int _time;
        private readonly SortedDictionary<int, List<ITiming>> _scheduleables;
        public TimingSystem()
        {
            _time = 0;
            _scheduleables = new SortedDictionary<int, List<ITiming>>();
        }

        // Add a new object to the schedule starting at the current time plus the object's Time property.
        public void Add(ITiming scheduleable)
        {
            int key = _time + scheduleable.Time;
            if (!_scheduleables.ContainsKey(key))
            {
                _scheduleables.Add(key, new List<ITiming>());
            }
            _scheduleables[key].Add(scheduleable);
        }
        // Remove a specific object from the schedule.
        // Useful for when an monster is killed to remove it before it's action comes up again.
        public void Remove(ITiming scheduleable)
        {
            KeyValuePair<int, List<ITiming>> scheduleableListFound = new KeyValuePair<int, List<ITiming>>(-1, null);
            foreach (var scheduleablesList in _scheduleables)
            {
                if (scheduleablesList.Value.Contains(scheduleable))
                {
                    scheduleableListFound = scheduleablesList;
                    break;
                }
            }
            if (scheduleableListFound.Value != null)
            {
                scheduleableListFound.Value.Remove(scheduleable);
                if (scheduleableListFound.Value.Count <= 0)
                {
                    _scheduleables.Remove(scheduleableListFound.Key);
                }
            }
        }
        // Get the next object whose turn it is from the schedule. Advance time if necessary
        public ITiming Get()
        {
            var firstScheduleableGroup = _scheduleables.First();
            var firstScheduleable = firstScheduleableGroup.Value.First();
            Remove(firstScheduleable);
            _time = firstScheduleableGroup.Key;
            return firstScheduleable;
        }
        // Get the current time (turn) for the schedule
        public int GetTime()
        {
            return _time;
        }
        // Reset the time and clear out the schedule
        public void Clear()
        {
            _time = 0;
            _scheduleables.Clear();
        }
    }
}

