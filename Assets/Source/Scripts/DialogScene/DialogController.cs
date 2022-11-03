using Assets.Source.Data;
using Assets.Source.Scripts;
using Data.Texts;
using Data.Texts.Schemas;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.Video;

public class DialogController : MonoBehaviour
{
    internal static Texts currentTexts;

    public static bool Available => LocalGame.cutsceneNumber >= 0 || TextLoader.LoadLanguage(DataIO.LangCode).dialog.Length - 1 >= LocalGame.dialogNumber;

    public TextMeshProUGUI dialogBoxText;
    public Image dialogSprite;
    public Image dialogBGImage;
    public TextMeshProUGUI dialogAuthor;
    public SceneLoader transition;
    public GameObject DialogParent;

    public VideoPlayer videoPlayer;
    public TextMeshProUGUI videoPlayerSub;
    public AudioSource audioSource;

    private Dialog dialog;
    private DialogText[] dialogs;
    private Cutscene cutscene;

    private bool cutSceneDone = false;

    private float animationSpeed = 0.05f;

    private bool skippable = false;
    private bool autoplay = false;
    private bool nosprites = false;
    private bool nobg = false;

    private bool _buffer;

    private const int autoPlayDelay = 3;

    private void Start()
    {
        currentTexts = TextLoader.LoadLanguage(DataIO.LangCode);

        if (LocalGame.cutsceneNumber >= 0)
        {
            cutscene = currentTexts.cutscenes[LocalGame.cutsceneNumber];
            var load = Addressables.LoadAssetAsync<VideoClip>(cutscene.name);
            videoPlayer.clip = load.WaitForCompletion();
            videoPlayer.prepareCompleted += e =>
            {
                e.Play();
                e.waitForFirstFrame = true;
                StartCoroutine(WaitForVideo());
            };
            videoPlayer.Prepare();
        }
        else
        {
            videoPlayer.gameObject.SetActive(false);
            DialogParent.SetActive(true);
            cutSceneDone = true;
        }

        if (cutSceneDone && LocalGame.dialogNumber > currentTexts.dialog.Length - 1)
        {
            goto SkipDialog;
        }

        dialog = currentTexts.dialog[LocalGame.dialogNumber];
        dialogs = dialog.dialogs;

        StartCoroutine(DisplayDialog());

        return;
        SkipDialog:
        StartCoroutine(DisplayDialog(true));
    }

    private void Update()
    {
        _buffer = _buffer || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }

    private IEnumerator WaitForVideo()
    {
        yield return null;
        while (videoPlayer.isPlaying && !Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return) && !Input.GetMouseButtonDown(0))
        {
            videoPlayerSub.text = GetCurrentSubtitle(videoPlayer.time);
            yield return null;
        }
        videoPlayer.gameObject.SetActive(false);
        DialogParent.SetActive(true);
        yield return null;
        cutSceneDone = true;
    }

    private string GetCurrentSubtitle(double time)
    {
        for (int i = 0; i < cutscene.subtitles.Length - 1; i++)
        {
            if (cutscene.subtitles[i].time <= time && cutscene.subtitles[i + 1].time > time)
            {
                return cutscene.subtitles[i].text;
            }
        }
        return cutscene.subtitles[^1].text;
    }

    private bool Skipping()
    {
        bool res = _buffer;
        _buffer = false;
        return res;
    }

    private IEnumerator DisplayDialog(bool skip = false)
    {
        while (!cutSceneDone)
        {
            yield return null;
        }
        Skipping();
        if (skip || dialogs.Length == 0)
        {
            goto SkipDialog;
        }

        skippable = (dialog.flags & 0x01) == 0x01;
        autoplay = (dialog.flags & 0x02) == 0x02;
        nosprites = (dialog.flags & 0x04) == 0x04;
        nobg = (dialog.flags & 0x08) == 0x08;
        if (!nobg)
        {
            dialogBGImage.sprite = Addressables.LoadAssetAsync<Sprite>(dialog.background).WaitForCompletion();
        }

        // Animate the dialog text character by character
        for (int idx = 0; idx < dialogs.Length; idx++)
        {
            DialogText text = dialogs[idx];
            dialogAuthor.text = text.author;
            if (!nosprites)
            {
                if (text.sprite is not null)
                {
                    dialogSprite.sprite = Addressables.LoadAssetAsync<Sprite>(text.sprite).WaitForCompletion();
                }
                else
                {
                    string spr = $"spr_{text.author}";
                    AsyncOperationHandle<Sprite> load = Addressables.LoadAssetAsync<Sprite>(spr);
                    load.WaitForCompletion();
                    dialogSprite.sprite = load.OperationException is null ? load.Result : null;
                }
            }
            dialogBoxText.color = new Color32((byte)(text.color >> 16), (byte)((text.color >> 8) & 0xFF), (byte)(text.color & 0xFF), 0xFF);

            animationSpeed = text.overrideAnimationSpeed is 0 ? 0.05f : text.overrideAnimationSpeed;

            float startTime = Time.time;

            // Play the voiceline
            if (text.voiceline is not null or "")
            {
                audioSource.clip = Addressables.LoadAssetAsync<AudioClip>(text.voiceline).WaitForCompletion();
                audioSource.Play();
            }
            else
            {
                string va = $"{DataIO.LangCode}_va_{LocalGame.dialogNumber + 1}_{idx + 1}";
                AsyncOperationHandle<AudioClip> load = Addressables.LoadAssetAsync<AudioClip>(va);
                load.WaitForCompletion();
                if (load.OperationException is null)
                {
                    audioSource.clip = load.Result;
                    audioSource.Play();
                }
            }

            if (text.type != DialogType.Pause)
            {
                for (int i = 0; i < text.text.Length; i++)
                {
                    dialogBoxText.text = text.text[..(i + 1)];
                    dialogBoxText.fontStyle = text.type == DialogType.Erikative ? FontStyles.Italic : FontStyles.Normal;
                    if (skippable && Skipping())
                    {
                        dialogBoxText.text = text.text;
                        break;
                    }
                    yield return new WaitForSeconds(animationSpeed);
                }
            }
            else
            {
                dialogBoxText.text = "";
            }

            // Wait until either space or return is pressed.
            // If autoplay is on, it will wait for a press OR for a certain time to pass from this point on.
            while (!Skipping())
            {
                if (dialogs[^1] != text && autoplay && Time.time - startTime >= autoPlayDelay)
                {
                    break;
                }
                yield return null;
            }
            audioSource.Stop();
            yield return new WaitForSeconds(0.2f);
        }

        SkipDialog:

        if (LocalGame.dialogNumber % 10 == 0)
        {
            StartCoroutine(transition.LoadScene(3 + (LocalGame.dialogNumber / 10)));
            yield break;
        }

        StartCoroutine(transition.LoadScene(3));
    }
}