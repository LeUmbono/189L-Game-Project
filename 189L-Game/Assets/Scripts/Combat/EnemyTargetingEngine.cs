using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public static class EnemyTargetingEngine
    {
        // Targeting probabilities engine based on the number of allies left in battle.
        // For instance the second list (representing the scenario with 2 allied units
        // remaining) in TargetingProbabilities has that the unit at the front of the player
        // formation has a 60% chance of being attacked whereas the unit in the back has a
        // 40% chance of being attacked.
        public static List<List<int>> TargetingProbabilities = new List<List<int>>()
        {
            new List<int>(){100, -1, -1, -1},
            new List<int>(){60, 100, -1, -1},
            new List<int>(){45, 80, 100, -1},
            new List<int>(){40, 70, 90, 100},    
        };
    }
}
