﻿using AI.GOAP;
using UnityEngine;

public class TestStart : MonoBehaviour
{
    private void Awake()
    {
        GOAPReader.ReadAll();
    }
}