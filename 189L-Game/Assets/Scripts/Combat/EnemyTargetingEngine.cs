using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public static class EnemyTargetingEngine
    {
        public static List<List<int>> TargetingProbabilities = new List<List<int>>()
        {
            new List<int>(){100, -1, -1, -1},
            new List<int>(){60, 100, -1, -1},
            new List<int>(){45, 80, 100, -1},
            new List<int>(){40, 70, 90, 100},    
        };
    }
}

