using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelSpecific.MobBakery.Npc
{
    public class SitController : MonoBehaviour,IEnumerator<SitArea>
    {
        [SerializeField] private List<SitArea> seats;
        private int _index;
        public bool MoveNext()
        {
            var isFinished = _index == seats.Count - 1;
            if (isFinished) return false;
            
            _index++;
            return true;
        }

        public void Reset()
        {
            _index = 0;
        }

        public SitArea Current => seats[_index];
        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool TryGetSeat(out SitArea sit)
        {
            if (seats.Count == 0)
            {
                sit = null;
                return false;
            }

            if (!Current.IsOccupied)
            {
                sit = Current;
                return true;
            }

            if (MoveNext())
            {
                sit = Current;
                return true;
            }
            else
            {
                sit = null;
                return false;
            }


        }
    }
}