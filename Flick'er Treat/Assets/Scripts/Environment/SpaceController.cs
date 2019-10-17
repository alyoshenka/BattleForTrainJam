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

    private void Start()
    {
        borderTransform.localScale = new Vector3(maxScale, maxScale, maxScale);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //lerpGoal = 1;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //lerpGoal = 0;
        }

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
        borderTransform.localScale = new Vector3(lerpedValue, lerpedValue, lerpedValue);
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
