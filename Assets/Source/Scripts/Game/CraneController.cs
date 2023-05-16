using Assets.Source.Scripts.Game;
using System.Collections;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    public GameObject BoomAnchor;
    public GameObject RopeAnchor;
    public GameObject Tower;
    public Transform Hook;

    private GameObject holding;
    public IEnumerator Rotate(bool left, float lerpDuration)
    {
        float timeElapsed = 0f;
        Direction dir = Level.crane.direction;
        float destinationLength = (int)dir % 2 == 0 ? (Level.crane.extended ? 2f : 1f) : 1.414f;

        float currentLength = BoomAnchor.transform.localScale.z;
        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            if (left)
            {
                Tower.transform.localEulerAngles += new Vector3(0, -45.0f * Time.deltaTime / lerpDuration, 0);
            }
            else
            {
                Tower.transform.localEulerAngles += new Vector3(0, 45.0f * Time.deltaTime / lerpDuration, 0);
            }

            BoomAnchor.transform.localScale = new Vector3(1, 1, currentLength + ((destinationLength - currentLength) * timeElapsed / lerpDuration));
            yield return null;
        }
        Tower.transform.localEulerAngles = new Vector3(0, 45.0f * (int)dir, 0);
        BoomAnchor.transform.localScale = new Vector3(1, 1, destinationLength);
        yield break;
    }

    public IEnumerator ChangeHeight(bool high, float lerpDuration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            RopeAnchor.transform.localScale = high
                ? new Vector3(1, 1 - (0.5f * timeElapsed / lerpDuration), 1)
                : new Vector3(1, 0.5f + (0.5f * timeElapsed / lerpDuration), 1);
            yield return null;
        }
        RopeAnchor.transform.localScale = new Vector3(1, high ? 0.5f : 1f, 1);
        yield return null;
    }

    public IEnumerator ChangeBoom(bool extended, float lerpDuration)
    {
        float timeElapsed = 0f;
        bool straight = (int)Level.crane.direction % 2 == 0;
        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            if (extended && straight)
            {
                BoomAnchor.transform.localScale = new Vector3(1, 1, 1 + (timeElapsed / lerpDuration));
            }
            else if (straight)
            {
                BoomAnchor.transform.localScale = new Vector3(1, 1, 2 - (timeElapsed / lerpDuration));
            }
            yield return null;
        }
        BoomAnchor.transform.localScale = new Vector3(1, 1, straight ? (extended ? 2f : 1f) : 1.414f);
        yield return null;
    }

    private void Update()
    {
        if (holding != null)
        {
            holding.transform.localPosition = MultiplyComponents(new Vector3(-3.5f, 0, -3.5f), VectorReciprocal(Hook.lossyScale));
            holding.transform.localScale = MultiplyComponents(0.7f * Vector3.one, VectorReciprocal(Hook.lossyScale));
        }
    }

    public IEnumerator Tach(bool attach, GameObject proto = null)
    {
        if (attach)
        {
            holding = Instantiate(proto, Hook);
            holding.transform.localPosition = MultiplyComponents(new Vector3(-3.5f, 0, -3.5f), VectorReciprocal(Hook.lossyScale));
            holding.transform.localScale = MultiplyComponents(0.7f * Vector3.one, VectorReciprocal(Hook.lossyScale));
            holding.SetActive(true);
            yield return null;
        }
        else
        {
            Destroy(holding);
            yield return null;
        }
    }

    public Vector3 MultiplyComponents(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public Vector3 VectorReciprocal(Vector3 a)
    {
        return new Vector3(1.0f / a.x, 1.0f / a.y, 1.0f / a.z);
    }
}
