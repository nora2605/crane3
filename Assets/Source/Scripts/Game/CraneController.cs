using Assets.Source.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    public GameObject BoomAnchor;
    public GameObject RopeAnchor;
    public GameObject Tower;
    public Transform Hook;
    public IEnumerator Rotate(Direction dir)
    {
        float lerpDuration = 0.5f;
        float timeElapsed = 0f;
        bool left = ((int)Level.crane.direction - (int)dir) < 0;
        float destinationLength = (int)dir % 2 == 0 ? (Level.crane.extended ? 2f : 1f) : 1.414f;

        float currentLength = BoomAnchor.transform.localScale.z;
        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            if (left)
                Tower.transform.localEulerAngles += new Vector3(0, -45.0f * Time.deltaTime/lerpDuration, 0);
            else
                Tower.transform.localEulerAngles += new Vector3(0, 45.0f * Time.deltaTime / lerpDuration, 0);
            BoomAnchor.transform.localScale = new Vector3(1, 1, currentLength + (destinationLength - currentLength) * timeElapsed / lerpDuration);
            yield return null;
        }
        Tower.transform.localEulerAngles = new Vector3(0, 45.0f * (int)dir, 0);
        BoomAnchor.transform.localScale = new Vector3(1, 1, destinationLength);
        yield break;
    }

    public IEnumerator ChangeHeight(bool high)
    {
        float lerpDuration = 0.5f;
        float timeElapsed = 0f;
        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            if (high)
            {
                RopeAnchor.transform.localScale = new Vector3(1, 1 - 0.5f * timeElapsed / lerpDuration, 1);
            }
            else
            {
                RopeAnchor.transform.localScale = new Vector3(1, 0.5f + 0.5f * timeElapsed / lerpDuration, 1);
            }
            yield return null;
        }
        RopeAnchor.transform.localScale = new Vector3(1, high ? 0.5f : 1f, 1);
        yield return null;
    }

    public IEnumerator ChangeBoom(bool extended)
    {
        float lerpDuration = 0.5f;
        float timeElapsed = 0f;
        bool straight = (int)Level.crane.direction % 2 == 0;
        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            if (extended && straight)
            {
                BoomAnchor.transform.localScale = new Vector3(1, 1 + timeElapsed / lerpDuration, 1);
            }
            else
            {
                BoomAnchor.transform.localScale = new Vector3(1, 2 - timeElapsed / lerpDuration, 1);
            }
            yield return null;
        }
        BoomAnchor.transform.localScale = new Vector3(1, straight ? (extended ? 2f : 1f) : 1.414f, 1);
        yield return null;
    }

    public IEnumerator Tach(GameObject proto)
    {
        GameObject holding = Instantiate(proto, Hook);
        holding.transform.localPosition = new Vector3(0, 0, 0);
        holding.SetActive(true);
        yield return null;
    }
}
