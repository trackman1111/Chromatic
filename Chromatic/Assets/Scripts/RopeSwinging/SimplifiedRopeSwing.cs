using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The object the rope is hanging from must have a Spring Joint, a LineRenderer
//The object hanging from the rope must have a Rigidbody and enable gravity




//If there is a stiff rope, then it needs a simplified solution
//this is also an accurate solution because a metal wire is not swinging as much as a rope made of lighter material
public class SimplifiedRopeSwing : MonoBehaviour
{
    //Objects that will interact with thr eop
    public Transform objectTheRopeIsConnectedTo;
    public Transform objectThatIsHangingFromTheRope;

    //Line renderer used to display the rope
    private LineRenderer lineRenderer;

    //A list with all rope sections
    public List<Vector3> allRopeSections = new List<Vector3>();

    //Rope data
    private float ropeLength = 1f;
    private float minRopeLength = 1f;
    private float maxRopeLength = 20f;
    //Mass of what the rope is carrying
    private float loadMass = 100f;
    //How fast we can add more/less rope
    float winchSpeed = 2f;

    //The joint we use to approximate the rope
    SpringJoint springJoint;

    // Start is called before the first frame update
    void Start()
    {
        springJoint = objectTheRopeIsConnectedTo.GetComponent<SpringJoint>();

        //Init the line renderer we use to display the rope
        lineRenderer = GetComponent<LineRenderer>();

        //Init the spring we use to approximate the rope from point a to b
        UpdateSpring();

        //Add the weight to what the rope is carrying
        objectThatIsHangingFromTheRope.GetComponent<Rigidbody>().mass = loadMass;


    }

    // Update is called once per frame
    void Update()
    {
        //Add more/less rope
        UpdateWinch();

        //Display the rope with a line renderer
        DisplayRope();
    }

    //Update the spring constant and length of the spring
    private void UpdateSpring()
    {
        //Someone said setting this to infinity avoids bounce, but it didn't work for this guy
        //kRope = float.inf

        //
        //The mass of the rope
        //
        //Density of the wire (This is using stainless steel) kg/m3
        float density = 7750f;
        float radius = 0.02f;

        float volume = Mathf.PI * radius * radius * ropeLength;

        float ropeMass = volume * density;

        //
        //The spring constant (it has to recalculate if the rope length is changing)
        //
        //The force from the rope: F = rope_mass * g, which is how much the top rope segment will carry
        float ropeForce = ropeMass * 9.81f;

        //Use the spring equation to calculate F = k*x should balance the force, w
        //where x is how much the top rope segment should stretch (such as .01m)

        //Is about 146000
        float kRope = ropeForce / 0.01f;

        //Add the value to the spring
        springJoint.spring = kRope * 1.0f;
        springJoint.damper = kRope * 0.8f;

        //Update the length of the rope
        springJoint.maxDistance = ropeLength;

    }

    //Display the rope with a line renderer
    private void DisplayRope()
    {
        //This is not the actual width, but the width use so we can see the rope
        float ropeWidth = 0.2f;

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        //Update the list with rope sections by approximating the rope with a bezier curve
        //A Bezier curve needs 4 control points
        Vector3 A = objectTheRopeIsConnectedTo.position;
        Vector3 D = objectThatIsHangingFromTheRope.position;

        //Upper control point
        //To get a little curve at the top than at the bottom
        Vector3 B = A + objectTheRopeIsConnectedTo.up * (-(A - D).magnitude * 0.1f);
        //B = A;

        //Lower control point
        Vector3 C = D + objectThatIsHangingFromTheRope.up * ((A - D).magnitude * 0.5f);

        //Get the positions
        BezierforRopeSwing.GetBezierCurve(A, B, C, D, allRopeSections);

        //An array with all rope section positions
        Vector3[] positions = new Vector3[allRopeSections.Count];

        for (int i = 0; i < allRopeSections.Count; i++)
        {
            positions[i] = allRopeSections[i];
        }

        //Just add a line between the start and end position for testing purposes
        //Vector3[] positions = new Vector3[2];


        //Add the positions to the line renderer
        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);

    }

    //Add more/less rope
    private void UpdateWinch()
    {
        bool hasChangedRope = false;

        //More rope
        if (Input.GetKey(KeyCode.O) && ropeLength < maxRopeLength)
        {
            ropeLength += winchSpeed * Time.deltaTime;

            hasChangedRope = true;
        }
        else if (Input.GetKey(KeyCode.I) && ropeLength > minRopeLength)
        {
            ropeLength -= winchSpeed * Time.deltaTime;

            hasChangedRope = true;
        }

        if (hasChangedRope)
        {
            ropeLength = Mathf.Clamp(ropeLength, minRopeLength, maxRopeLength);

            //Need to recalculate the k-value because it depends on the length of the rope
            UpdateSpring();
        }

    }

}
