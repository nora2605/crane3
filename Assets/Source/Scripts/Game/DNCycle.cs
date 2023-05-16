using Assets.Source.Scripts;
using UnityEngine;

public class DNCycle : MonoBehaviour
{
    public Material skb10;
    public Material skb20;
    public Material skb30;
    public Material skb40;

    public AudioSource ads;
    public AudioClip ads10;
    public AudioClip ads20;
    public AudioClip ads30;
    public AudioClip ads40;

    public ReflectionProbe refProbe;

    // Start is called before the first frame update
    void Start()
    {
        if (LocalGame.levelNumber <= 10)
        {
            RenderSettings.skybox = skb10;
            ads.clip = ads10;
        }
        else if (LocalGame.levelNumber <= 20)
        {
            RenderSettings.skybox = skb20;
            ads.clip = ads20;
        }
        else if (LocalGame.levelNumber <= 30)
        {
            RenderSettings.skybox = skb30;
            ads.clip = ads30;
        }
        else
        {
            RenderSettings.skybox = skb40;
            ads.clip = ads40;
        }
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
        ads.Play();
        refProbe.RenderProbe();
    }
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
    }
}
