using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Barracuda
{
    public class Cacher
    {
        private Dictionary<Coord, Dictionary<Coord, double>> State;

        public Cacher()
        {
            State = new Dictionary<Coord, Dictionary<Coord, double>>();
        }

        public bool CheckPair(Coord a, Coord b)
        {
            return State.ContainsKey(a) && State[a].ContainsKey(b) || State.ContainsKey(b) && State[b].ContainsKey(a);
        }

        public void AddPair(Coord a, Coord b, double s)
        {
            if (State.ContainsKey(a))
            {
                State[a][b] = s;
                if (State.ContainsKey(b))
                {
                    State[b][a] = s;
                }
                else
                {
                    State[b] = new Dictionary<Coord, double>();
                    State[b][a] = s;
                }
            }
            else
            {
                State[a] = new Dictionary<Coord, double>();
                AddPair(a, b, s);
            }
        }

        public double GetDist(Coord a, Coord b)
        {
            if (!State.ContainsKey(a) || !State[a].ContainsKey(b))
            {
                throw new NullReferenceException("Attempted to get non-calculated distance");
            }

            return State[a][b];
        }
    }
}