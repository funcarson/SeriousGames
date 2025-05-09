// PhotoPopup.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotoPopup : MonoBehaviour
{
    public Image displayImage;
    public TMP_Text captionText;
    public Button closeButton;

    public void Show(Sprite image, string caption, int points)
    {
        displayImage.sprite = image;
        captionText.text = caption + $" (+{points} pts)";
        Time.timeScale = 0f;
        closeButton.onClick.AddListener(Close);
        gameObject.SetActive(true);
    }

    void Close()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}

