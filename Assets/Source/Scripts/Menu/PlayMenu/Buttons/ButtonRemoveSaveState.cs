using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonRemoveSaveState : MonoBehaviour
{
    private Button removeSaveStateButton;

    void Start()
    {
        removeSaveStateButton = GetComponent<Button>();
        removeSaveStateButton.onClick.AddListener(Remove);
    }

    private void Update()
    {
        removeSaveStateButton.enabled = Indexer.selected;
    }

    void Remove()
    {
        if (Indexer.selected)
        {
            File.Delete(Indexer.currentSelected);
            Indexer.entry = Camera.main.GetComponent<AudioSource>().time;
            Indexer.selected = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Currently intermediate way to reset the content view
        }
    }
}
