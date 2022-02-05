using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twoDAnimationScriptController : MonoBehaviour
{
    Animator animator;
    float velocityX = 0.0f;
    float velocityZ = 0.0f;

    public float acceleration = 2.0f;
    public float deceleration = 2.0f;

    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;

    // increase performance
    int VelocityZHash;
    int VelocityXHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // increase performance
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");

    }

    void changeVelocity(bool forwardPress, bool leftPress, bool rightPress, bool runPress, float currentMaxVelocity)
    {
        // if pressed w increase velocity in z direction
        if (forwardPress && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        // increase velocity in left direction
        if (leftPress && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        // increase velocity in right direction
        if (rightPress && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        // decrease velocityZ if no press
        if (!forwardPress && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        /////////////////////////////////

        // increase velocityX if left not pressed and velocityX < 0
        if (!leftPress && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        // decrease velocityX if right not pressed and velocityX > 0
        if (!rightPress && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

    }

    void lockOrResetVelocity(bool forwardPress, bool leftPress, bool rightPress, bool runPress, float currentMaxVelocity)
    {

        // reset velocityZ
        if (!forwardPress && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        // reset velocityX
        if (!leftPress && !rightPress && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        /////////////////////////////////////////////////////////
        // lock forward speed
        if (forwardPress && runPress && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        // deceleration to the maximum walk velocity
        else if (forwardPress && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            // round to the currentMaxVelocity if within offset
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        // round to the currentMaxVelocity if within offset
        else if (forwardPress && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }
        /////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////
        // lock left
        if (leftPress && runPress && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        // deceleration to the maximum walk velocity
        else if (leftPress && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            // round to the currentMaxVelocity if within offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity + 0.05))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        // round to the currentMaxVelocity if within offset
        else if (leftPress && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }
        ////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////
        // lock right
        if (rightPress && runPress && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        // deceleration to the maximum walk velocity
        else if (rightPress && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            // round to the currentMaxVelocity if within offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05))
            {
                velocityX = currentMaxVelocity;
            }
        }
        // round to the currentMaxVelocity if within offset
        else if (rightPress && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }
        ////////////////////////////////////////////////////////
  
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPress = Input.GetKey(KeyCode.W);
        bool leftPress = Input.GetKey(KeyCode.A);
        bool rightPress = Input.GetKey(KeyCode.D);
        bool runPress = Input.GetKey(KeyCode.LeftShift);

        // set current max velocity
        float currentMaxVelocity = runPress ? maximumRunVelocity : maximumWalkVelocity;

        // handle change in velocity
        changeVelocity(forwardPress, leftPress, rightPress, runPress, currentMaxVelocity);
        lockOrResetVelocity(forwardPress, leftPress, rightPress, runPress, currentMaxVelocity);


        // set parameters to local variable values
        animator.SetFloat(VelocityXHash, velocityX);
        animator.SetFloat(VelocityZHash, velocityZ);
    }
}
