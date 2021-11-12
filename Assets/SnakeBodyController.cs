using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wobblifier))]
public class SnakeBodyController : MonoBehaviour
{
    private Wobblifier wobble;
    // Start is called before the first frame update
    private void Awake()
    {
        wobble = GetComponent<Wobblifier>();
    }

    /// <summary>
    /// Initializes wobbling parameters and starts the wobbling process
    /// </summary>
    /// <param name="maxWobbleScale">The maximum potential scale the body segment will reach during a wobble.</param>
    /// <param name="minWobbleScale">The minimum potential scale the body segment will reach during a wobble.</param>
    /// <param name="wobbleInterval">The maximum duration (in seconds) the wobble oscillation between MinWobbleScale and MaxWobbleScale will take.</param>
    public void InitializeWobble(float minWobbleScale, float maxWobbleScale, float minWobbleDuration, float maxWobbleDuration)
    {
        float NORMAL_SCALE = transform.localScale.x;

        float SHRINK_SCALE = Random.Range(NORMAL_SCALE * minWobbleScale, NORMAL_SCALE);
        float GROW_SCALE = Random.Range(NORMAL_SCALE, NORMAL_SCALE * maxWobbleScale);
        
        wobble.MaxWobble = GROW_SCALE;
        wobble.MinWobble = SHRINK_SCALE;
        wobble.WobbleRate = Random.Range(minWobbleDuration, maxWobbleDuration);

        wobble.StartWobbling();
    }
}
