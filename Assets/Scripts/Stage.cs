using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level
{
    [Range(1, 11)]
    public int partCount = 11;

    [Range(0, 11)]
    public int deathPartCount = 1;
}

[CreateAssetMenu(fileName = "New Stage", menuName = "ScriptableObjects/Stage", order = 1)]
public class Stage : ScriptableObject
{
    public Color stageBackgroundColor = Color.white;
    public Color stageLevelPartColor = Color.white;
    public Color stageBallColor = Color.white;
    public List<Level> levels = new List<Level>();
}
//ScriptableObject의 주요 사용 사례.
// 에디터 세션 동안 데이터 저장 및 보관
// 데이터를 프로젝트의 에셋으로 저장하여 사용

// ScriptableObject의 사용 방법
// ScriptableObject를 상속받는다.