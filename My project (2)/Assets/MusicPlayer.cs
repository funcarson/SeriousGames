using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Awake()
    {
        // If another MusicPlayer already exists, destroy this duplicate
        if (FindObjectsOfType<MusicPlayer>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        // Otherwise, keep me alive through scene loads
        DontDestroyOnLoad(gameObject);
    }
}

