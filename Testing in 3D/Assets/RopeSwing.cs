using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwing : MonoBehaviour
{
    //Used https://www.habrador.com/tutorials/rope/1-realistic-rope/ for this.
    //Had to use Vector3 because of DisplayLine() a lineRenderer thing
    
    //Objects that will interact with the rope
    public Transform objectRopeIsConnectedTo;
    public Transform objectThatIsHangingFromTheRope;


    //Line renderer used to display the rope. A LineRender is used to draw free-floating lines in space
    LineRenderer lineRenderer;

    //A list with all rope sections
    public List<RopeSection> allRopeSections = new List<RopeSection>();

    //Rope data
    private float ropeSectionLength = 1f;

    //Data that we can change to change the rope properties
    //Spring Constant
    public float kRope = 40f;
    //Damping from rope friction constant (I do not yet understand how this works)
    public float dRope = 2f;
    //Damping from air resistance constant (I do not yet understand how this works)
    public float aRope = 0.05f;
    //Mass of one rope section (haha I understand this one though)
    public float mRopeSection = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        //Init the line renderer used to display the rope
        lineRenderer = GetComponent<LineRenderer>();

        //
        //Create the rope
        //
        //Build the rope from the top
        Vector3 pos = objectRopeIsConnectedTo.position;

        List<Vector3> ropePositions = new List<Vector3>();

        // ++ increments its operand by 1; i++ uses the current value and adds one; ++i adds one then uses that value
        for (int i = 0; i < 7; i++)
        {
            //Adding positions of the rope sections to the list
            ropePositions.Add(pos);

            //making the new position start at the end of the last rope section
            pos.y -= ropeSectionLength;
        }

        //But add the rope sections from bottom because it's easier to add more sections to it if we have a winch (WHY?)
        
        //Counts the elements in list ropePositions - 1
        //Runs through the ropePositions list backwards and makes a new RopeSection element for each one
        for (int i = ropePositions.Count - 1; i >= 0; i--)
        {
            allRopeSections.Add(new RopeSection(ropePositions[i]));
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Display the rope with the line renderer
        DisplayRope();

        //Compare the current length of the rope with the wanted length
        DebugRopeLength();

        //Move what is hanging from the rope to the end of the rope (End of the rope because the bottom most section is element 0)
        objectThatIsHangingFromTheRope.position = allRopeSections[0].pos;

        //Make what is hanging from the rope look at the second-to-bottom rope position to make it rotate with the rope
        //CHANGE THIS IF DON'T WANT CHARACTER TO ROTATE
        objectThatIsHangingFromTheRope.LookAt(allRopeSections[1].pos);
    }

    void FixedUpdate()
    {
        if (allRopeSections.Count > 0)
        {
            //Simulate the rope
            //How accurate should the simulation be?
            int iterations = 1;

            //Time step
            float timeStep = Time.fixedDeltaTime / (float)iterations;

            for (int i = 0; i < iterations; i++)
            {
                //Called function
                UpdateRopeSimulation(allRopeSections, timeStep);
            }

        }
    }

    //Display the rope with a line renderer
    private void DisplayRope()
    {
        float ropeWidth = 0.2f;

        //Rope Width
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        //An array with all rope section positions
        Vector3[] positions = new Vector3[allRopeSections.Count];

        //adds the rope section positions to the array
        for (int i = 0; i < allRopeSections.Count; i++)
        {
            positions[i] = allRopeSections[i].pos;
        }

        //Other guy used numPositions instead of positionCount but numPositions is OBSOLETE
        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);

    }

    private void UpdateRopeSimulation(List<RopeSection> allRopeSections, float timeStep)
    {
        //Move the last position (which is the top position) to what the rope is attached to
        RopeSection lastRopeSection = allRopeSections[allRopeSections.Count - 1];

        //
        //Calculate the next pos and vel with Forward Euler (If we know the start pos and start vel, and acceleration, we can find the pos and vel next update)
        //
        //Making lists for the next accelerations and positions
        List<Vector3> accelerations = CalculateAccelerations(allRopeSections);

        List<RopeSection> nextPosVelForwardEuler = new List<RopeSection>();

        //Loop through all line segments (except the last because it's always connected to something)
        for (int i = 0; i < allRopeSections.Count - 1; i++)
        {
            //I think this calls on RopeSection.cs
            RopeSection thisRopeSection = RopeSection.zero;

            //Forward Euler
            //vel = vel + acc * t
            thisRopeSection.vel = allRopeSections[i].vel + (accelerations[i] * timeStep);

            //pos = pos + vel * t
            thisRopeSection.pos = allRopeSections[i].pos + allRopeSections[i].vel * timeStep;

            //Save the new data in a temporary list
            nextPosVelForwardEuler.Add(thisRopeSection);

        }

        //Adds the last segment which is always the same b/c it's attached to something
        nextPosVelForwardEuler.Add(allRopeSections[allRopeSections.Count - 1]);

        //
        //Calculate hthe next pos with Heun's method (which is basically an improved Forward Euler)
        //
        //Calculate the acceleration in each rope section which is what is needed to get the next pos and vel
        List<Vector3> accelerationFromEuler = CalculateAccelerations(nextPosVelForwardEuler);

        List<RopeSection> nextPosVelHeunsMethod = new List<RopeSection>();

        //Loop through all line segments (except the last section because it's always connected to something)
        for (int i = 0; i < allRopeSections.Count - 1; i++)
        {
            RopeSection thisRopeSection = RopeSection.zero;

            //Heuns method
            //vel = vel + (acc + <acc from Forward Euler>) * .5 * t
            thisRopeSection.vel = allRopeSections[i].vel + (accelerations[i] + accelerationFromEuler[i]) * 0.5f * timeStep;

            //pos = pos + (vel + velFromForwardEuler) * 0.5f * t
            thisRopeSection.pos = allRopeSections[i].pos + (allRopeSections[i].vel + nextPosVelForwardEuler[i].vel) * 0.5f * timeStep;

            //Save the new data in a temporary list
            nextPosVelHeunsMethod.Add(thisRopeSection);

        }

        //Add the last section which is always the same because it's attached to something
        nextPosVelHeunsMethod.Add(allRopeSections[allRopeSections.Count - 1]);


        //From the temp list to the main list
        for (int i = 0; i < allRopeSections.Count; i++)
        {
            allRopeSections[i] = nextPosVelHeunsMethod[i];

        }

        //Implement maximum stretch to avoid numerical instabilities (NOT SURE WHAT THIS MEANS)
        //May need to run the algorithm several times
        int maximumStretchIterations = 2;

        for (int i = 0; i < maximumStretchIterations; i++)
        {
            ImplementMaximumStretch(allRopeSections);
        }


    }

    // \/\/\/ Calculates accelerations in each rope section which is what is needed to get the next pos and vel
    private List<Vector3> CalculateAccelerations(List<RopeSection> allRopeSections)
    {
        List<Vector3> accelerations = new List<Vector3>();

        //Spring constant
        float k = kRope;
        //Damping constant
        float d = dRope;
        //Damping constant from air resistance
        float a = aRope;
        //Mass of one rope section
        float m = mRopeSection;
        //How long should the rope section be
        float wantedLength = ropeSectionLength;

        //Calculate all forces once because some sections are using the same force but negative (Don't totally understand this but I get some of it)
        List<Vector3> allForces = new List<Vector3>();

        for (int i = 0; i < allRopeSections.Count - 1; i++)
        {
            //From Physics for game developers book
            //The force exerted on boy 1
            //pos1 (above) - pos2
            //This takes the difference of the positions of each section subtracted from the section above it
            Vector3 vectorBetween = allRopeSections[i + 1].pos - allRopeSections[i].pos;

            float distanceBetween = vectorBetween.magnitude;

            Vector3 dir = vectorBetween.normalized;

            //Spring Constant times the difference of the <length between a rope section and the section above it> and how long the rope section should be
            //Essentially just calculatting how far the rope section is from where it should be (connected to the section above it) and giving a force to pull it back
            float springForce = k * (distanceBetween - wantedLength);

            //Damping from rope friction
            //vel1 (above) - vel2
            float frictionForce = d * ((Vector3.Dot(allRopeSections[i + 1].vel - allRopeSections[i].vel, vectorBetween)) / distanceBetween);

            //The total force on the spring (uses the force of friction and the force of the spring)
            Vector3 springForceVec = -(springForce + frictionForce) * dir;

            //This is body 2 if we follow the book because we are looping from below, so negative
            springForceVec = -springForceVec;

            allForces.Add(springForceVec);

        }

        //Loop through all line segments (once again, except the last one)
        //and calculate the acceleration
        for (int i = 0; i < allRopeSections.Count - 1; i++)
        {
            Vector3 springForce = Vector3.zero;

            //Spring 1- above
            //allForces contain all the spring forces from thing above
            springForce += allForces[i];

            //Spring 2 - below
            //The first spring is at the bottom so it doesn't have a sectio below it
            if (i != 0)
            {
                springForce -= allForces[i - 1];
            }

            //Damping from air resistance, which depends on the square of the velocity
            float vel = allRopeSections[i].vel.magnitude;

            Vector3 dampingForce = a * vel * vel * allRopeSections[i].vel.normalized;

            //The mass attached to this spring
            float springMass = m;

            //end of the rope is attached to a box with a mass (0 is the end rope)
            if (i == 0)
            {
                springMass += objectThatIsHangingFromTheRope.GetComponent<Rigidbody>().mass;
            }

            //Force from gravity
            Vector3 gravityForce = springMass * new Vector3(0f, -9.81f, 0f);

            //The total force on this spring
            Vector3 totalForce = springForce + gravityForce - dampingForce;
            
            //Calculating acceleration a = F/m
            Vector3 acceleration = totalForce / springMass;

            accelerations.Add(acceleration);

        }

        //The last line segmen'ts acc is always 0 (once again, because it is attached to something)
        accelerations.Add(Vector3.zero);

            return accelerations;
    }

    //Implement max stretch to avoid numerical instabilities
    private void ImplementMaximumStretch(List<RopeSection> allRopeSections)
    {
        //Make sure each spring is not less compressed than 90% or more stretched than 110%
        float maxStretch = 1.1f;
        float minStretch = 0.9f;

        //Loop from the end because it's better to adjust the top section of the rope before the bottom
        //And the top if the rope is at the end of the list

        for (int i = allRopeSections.Count - 1; i > 0; i--)
        {
            RopeSection topSection = allRopeSections[i];

            RopeSection bottomSection = allRopeSections[i - 1];

            //The distance between the sections
            float dist = (topSection.pos - bottomSection.pos).magnitude;

            //What's the stretch/compression
            float stretch = dist / ropeSectionLength;

            if (stretch > maxStretch)
            {
                //How far do we need to compress the spring?
                float compressLength = dist - (ropeSectionLength * maxStretch);

                //In what direction should we compress the spring? Between the top and bottom section directions
                Vector3 compressDir = (topSection.pos - bottomSection.pos).normalized;

                Vector3 change = compressDir * compressLength;

                MoveSection(change, i - 1);
            }
            else if (stretch < minStretch)
            {
                //How far do we need to stretch the spring?
                float stretchLength = (ropeSectionLength * minStretch) - dist;

                //In what direction should we compress the spring?
                Vector3 stretchDir = (bottomSection.pos - topSection.pos).normalized;

                Vector3 change = stretchDir * stretchLength;

                MoveSection(change, i - 1);

            }

        }

    }

    //Moves a rope section based on the stretch/compression
    //Basically putting in the change Vector (based on stretch/compression) and it moves the position
    private void MoveSection(Vector3 finalChange, int listPos)
    {
        RopeSection bottomSection = allRopeSections[listPos];

        //Move the bottom section
        Vector3 pos = bottomSection.pos;

        pos += finalChange;

        bottomSection.pos = pos;

        allRopeSections[listPos] = bottomSection;

    }

    //Compare current length of the rope with wanted length
    private void DebugRopeLength()
    {
        float currentLength = 0f;

        for (int i = 1; i < allRopeSections.Count; i++)
        {
            //thisLength is the length between two sections
            float thisLength = (allRopeSections[i].pos - allRopeSections[i - 1].pos).magnitude;

            //currentLength is 0 += thisLength (which is just the same as thisLength)
            currentLength += thisLength;
        }

        float wantedLength = ropeSectionLength * (float)(allRopeSections.Count - 1);

        print("Wanted: " + wantedLength + " Actual: " + currentLength);
        
    }




}
