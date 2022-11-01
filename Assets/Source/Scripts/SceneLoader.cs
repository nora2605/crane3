using System;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Source.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        public Animator transition;

        public IEnumerator LoadScene(int index)
        {
            // Play animation
            transition.SetTrigger("Transition");
            yield return new WaitForSeconds(1);

            AsyncOperation op = SceneManager.LoadSceneAsync(index);
            while (!op.isDone)
            {
                Debug.Log(op.progress);
                yield return null;
            }
        }
    }
}
