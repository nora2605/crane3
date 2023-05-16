using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Source.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        public Animator transition;

        public IEnumerator LoadScene(int index)
        {
            index = index != 2 ? index : DialogController.Available ? 2 : 3;
            // Play animation
            transition.SetTrigger("Transition");
            yield return new WaitForSeconds(1);

            AsyncOperation op = SceneManager.LoadSceneAsync(index);
            while (!op.isDone)
            {
                yield return null;
            }
        }
    }
}
