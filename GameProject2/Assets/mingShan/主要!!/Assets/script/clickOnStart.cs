using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class clickOnStart : MonoBehaviour {


    public void loadOnIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
  
}
