using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChargeJellyBatch : MonoBehaviour
{
    public GameObject stateObject;
    public GameObject chargeJellyPrefab;
    public float distanceThreshold = 20f; // 배치 간격

    [ContextMenu("Replace Jelly by X Distance")]
    public void ReplaceJellyByDistance()
    {
        if (stateObject == null || chargeJellyPrefab == null)
            return;

        // 1. 모든 jelly 컴포넌트를 가져와서 리스트로 변환
        List<Jelly> jellyList = stateObject.GetComponentsInChildren<Jelly>(true).ToList();

        // 2. X 좌표 기준으로 오름차순 정렬 (왼쪽 -> 오른쪽)
        jellyList = jellyList.OrderBy(j => j.transform.position.x).ToList();

        if (jellyList.Count == 0)
            return;

        int replaceCount = 0;

        // 첫 번째 젤리의 X좌표를 기준으로 시작
        float lastReplacedX = jellyList[0].transform.position.x;

        // 첫 번째 젤리는 유지할지, 혹은 첫 번째부터 바꿀지 결정 (여기서는 첫 번째는 유지하고 다음 50 거리부터 체크)
        for (int i = 0; i < jellyList.Count; i++)
        {
            GameObject currentObj = jellyList[i].gameObject;
            float currentX = currentObj.transform.position.x;

            // 3. 마지막으로 교체한 지점으로부터 설정한 거리(50)보다 멀리 있다면 교체
            // (첫 번째 젤리를 무조건 바꾸고 싶다면 i == 0 조건을 추가하면 됩니다)
            if (replaceCount == 0 || currentX >= lastReplacedX + distanceThreshold)
            {
                ReplaceObject(currentObj);

                // 마지막 교체 위치 업데이트
                lastReplacedX = currentX;
                replaceCount++;
            }
        }

        Debug.Log($"거리 기반 교체 완료! 총 {replaceCount}개의 젤리가 교체되었습니다.");
    }

    private void ReplaceObject(GameObject oldObj)
    {
        Vector3 pos = oldObj.transform.position;
        Quaternion rot = oldObj.transform.rotation;
        Transform parent = oldObj.transform.parent;

        GameObject newJelly = Instantiate(chargeJellyPrefab, pos, rot, parent);
        newJelly.name = oldObj.name + "_DistanceCharged";

        // 에디터 작업 시 Hierarchy에서 바로 확인 가능하도록 DestroyImmediate 사용
        DestroyImmediate(oldObj);
    }
}
