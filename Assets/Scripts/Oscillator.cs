using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    // Variables
    private Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0f, 1f)] float movementFactor;
    [SerializeField] float period = 5f;


    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Protect against NaN (Not a Number) error.
        if (period <= Mathf.Epsilon) { return; }

        // Get amount of cycles that have happened after say
        // 10 seconds / 2, then 5 cycles have happened.
        // It'll continually grow over time.
        float cycles = Time.time / period;

        // As time goes, we are traveling from -1 to 1.
        const float tau = Mathf.PI * 2; // Constant value of 6.283
        float rawSinWave = Mathf.Sin(cycles * tau);

        // Have the movement factor slider move on its own.
        // Add the +1 so that instead of going from -1 to 1,
        // we're now going from 0 to 2. Another way to word
        // this is we've recalculated to go from 0 to 1.
        movementFactor = (rawSinWave + 1f) / 2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
