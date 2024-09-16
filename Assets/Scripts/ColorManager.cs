using System;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    public event Action<string> OnColorChanged;

    private string currentColor;

    public string CurrentColor
    {
        get { return currentColor; }
        set
        {
            if (currentColor != value)
            {
                currentColor = value;
                OnColorChanged?.Invoke(currentColor);
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}