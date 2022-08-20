using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour
{

    private Vector3 startRotation;
    private Vector2 lastTapPos;
    private float helixDistance;
    public Transform topTransform;
    public Transform goalTransform;
    public GameObject helixLevelPrefab;
    public List<Stage> allStages = new List<Stage>();
    private List<GameObject> spawnedLevels = new List<GameObject>();

    void Awake()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);
    }

    void Start()
    {
        LoadStage();
        GameManager.Instance.ResetGame += LoadStage;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Vector2 curTapPos = Input.mousePosition;

            if (lastTapPos.Equals(Vector2.zero))
                lastTapPos = curTapPos;

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage()
    {
        int stageNumber = GameManager.Instance.currentStage;

        // 스테이지 갖고와
        Stage stage = allStages[stageNumber];

        if (stage == null)
        {
            return;
        }

        // 스테이지에 따라 뒷배경 색 조정
        Camera.main.backgroundColor = stage.stageBackgroundColor;
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = stage.stageBallColor;

        // 위치 초기화
        transform.localEulerAngles = startRotation;

        // 레벨 초기화
        foreach (GameObject go in spawnedLevels)
            Destroy(go);

        // 헬릭스 타워와 레벨 수에 따라 거리를 구함
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            // 위치를 한 단계씩 내려줌
            spawnPosY -= levelDistance;
            GameObject level = Instantiate(helixLevelPrefab, transform);

            // localposition과 position의 차이
            // position은 월드 기준
            // localposition은 부모의 위치 기준
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            // 360에서 30도씩이니까 최대 12개라 12 - partcount를 해주는 것
            int partsToDisable = 12 - stage.levels[i].partCount;

            // 구멍 몇개 내줘야 하니까 
            List<GameObject> disabledParts = new List<GameObject>();

            while (disabledParts.Count < partsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (disabledParts.Contains(randomPart) == false)
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }

            
            // 살아있는 것들 색바꿔주고 살아있는것들 리스트에 추가
            List<GameObject> leftParts = new List<GameObject>();

            foreach (Transform t in level.transform)
            {
                print(t.name);
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;

                if (t.gameObject.activeInHierarchy)
                    leftParts.Add(t.gameObject);
            }

            List<GameObject> deathParts = new List<GameObject>();

            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[Random.Range(0, leftParts.Count)];

                if (deathParts.Contains(randomPart) == false)
                {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }
            }


        }
    }
}
