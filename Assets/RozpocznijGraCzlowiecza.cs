using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RozpocznijGraCzlowiecza : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void rozpocznijGraCzlowiecza()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("GraCzlowiecza").buildIndex);
    }
}
