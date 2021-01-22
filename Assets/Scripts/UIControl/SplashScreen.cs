using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] RectTransform loadingBox = null;
    [SerializeField] float timeBetweenBoxMovement = 1f;
    [SerializeField] Vector2[] movePositions;

    private void Start()
    {
        StartCoroutine(MoveBox());
    }

    IEnumerator MoveBox()
    {
        for(int i = 0; i < 4; i++)
        {
            loadingBox.anchoredPosition = movePositions[i];
            yield return new WaitForSeconds(timeBetweenBoxMovement);
        }
        StartCoroutine(MoveBox());
    }

}
