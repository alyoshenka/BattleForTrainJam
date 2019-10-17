using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceController : MonoBehaviour
{
    float lerpGoal = 1;
    float lerpFraction = 1;
    float lerpedValue;

    [SerializeField]
    float maxScale = 10;
    [SerializeField]
    float minScale = 3;
    [SerializeField]
    float GrowthSpeed = 1.5f;
    [SerializeField]
    Transform borderTransform = default;
    [SerializeField]
    Transform camTransform = default;
    [SerializeField]
    float maxZoom = 10f;
    Vector3 camStartPos;
    Vector3 camEndPos;

    private void Start()
    {
        borderTransform.localScale = new Vector3(maxScale, maxScale, maxScale);
        camStartPos = camTransform.position;
        camEndPos = camStartPos + camTransform.forward * maxZoom;
    }

    void Update()
    {
        if (lerpFraction < lerpGoal)
        {
            lerpFraction += GrowthSpeed * Time.deltaTime;
            lerpFraction = Mathf.Clamp01(lerpFraction);

            if ( Mathf.Abs(lerpFraction - lerpGoal) <= GrowthSpeed * Time.deltaTime)
            {
                lerpFraction = lerpGoal;
            }
        }
        else if (lerpFraction > lerpGoal)
        {
            lerpFraction -= GrowthSpeed * Time.deltaTime;
            lerpFraction = Mathf.Clamp01(lerpFraction);

            if (Mathf.Abs(lerpFraction - lerpGoal) <= GrowthSpeed * Time.deltaTime)
            {
                lerpFraction = lerpGoal;
            }
        }
        lerpedValue = Mathf.Lerp(minScale, maxScale, lerpFraction);
        borderTransform.localScale = new Vector3(lerpedValue, borderTransform.localScale.y, lerpedValue);
        camTransform.localPosition = Vector3.Lerp(camEndPos, camStartPos, lerpFraction);

        Debug.DrawLine(camStartPos, camEndPos);
    }

    public void SetBorderScale(float scale)
    {
        lerpGoal = scale;
    }

    public float GetLerpedValue()
    {
        return lerpedValue;
    }
}
