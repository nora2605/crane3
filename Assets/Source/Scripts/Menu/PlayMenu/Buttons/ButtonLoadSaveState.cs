using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadSaveState : MonoBehaviour
{
    private Button loadSaveStateButton;
    void Start()
    {
        loadSaveStateButton = GetComponent<Button>();
        loadSaveStateButton.onClick.AddListener(Load);
    }

    private void Update()
    {
        loadSaveStateButton.enabled = Indexer.selected;
    }

    void Load()
    {
        // TODO
    }
}
