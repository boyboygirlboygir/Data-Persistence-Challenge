using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneSwapper : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public static SceneSwapper instance;
    public GameObject disableThis;
    public MainManager mainManager;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void OnClickLoad()
    {
        SceneManager.LoadScene(1);
        disableThis.SetActive(false);
    }
    
}
