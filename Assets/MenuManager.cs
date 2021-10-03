using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    float t = 0;
    int index = 0;
    [SerializeField]
    Text[] texts;
    float fadeTime = 5f;
    [SerializeField]
    GameObject introPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            introPanel.SetActive(false);
        }
        if (index >= texts.Length)
        {
            
            return;
        }
        texts[index].color = Color.Lerp(Color.clear, Color.white, t/ fadeTime);
        t += Time.deltaTime;
        if (t > fadeTime)
        {
            t = 0;
            index++;
        }
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
