using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld
{
    public class SceneTransitioner : MonoBehaviour
    {
        [SerializeField] private string combatSceneName;
        [SerializeField] private float timeToWait;
        [SerializeField][Range(0, 1)] private float slowdownPercent;

        public IEnumerator LoadCombatScene(PartyData playerData, PartyData enemyData)
        {
            Debug.Log("Play Transition");
            Time.timeScale = 0.3f;
            yield return new WaitForSeconds(timeToWait * slowdownPercent);
            SceneManager.LoadScene(combatSceneName, LoadSceneMode.Single);
        }
    }
}
