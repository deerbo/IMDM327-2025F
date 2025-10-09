// 3-body Starter Code
// Fall 2025. IMDM 327
// Instructor. Myungin Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.Random;

public class ThreeBody : MonoBehaviour
{
    private const float G = 500f; // Gravity constant https://en.wikipedia.org/wiki/Gravitational_constant
    GameObject[] body;
    BodyProperty[] bp;
    private int numberOfSphere = 100;
    TrailRenderer trailRenderer;
    struct BodyProperty // why struct?
    {                   // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct
        public float mass;
        public Vector3 velocity;
        public Vector3 acceleration;
    }


    void Start()
    {
        // Just like GO, computer should know how many room for struct is required:
        bp = new BodyProperty[numberOfSphere];
        body = new GameObject[numberOfSphere];

        // Loop generating the gameobject and assign initial conditions (type, position, (mass/velocity/acceleration)
        for (int i = 0; i < numberOfSphere; i++)
        {
            // Our gameobjects are created here:
            body[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); // why sphere? try different options.
            // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html

            // initial conditions
            //float r = 100f;
            float r = Random.Range(10, 100);
            // position is (x,y,z). In this case, I want to plot them on the circle with r

            // ******** Fill in this part ********
            float angle = i * Mathf.PI * 2 / numberOfSphere;
            body[i].transform.position = new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 180);
            // z = 180 to see this happen in front of me. Try something else (randomize) too.

            bp[i].velocity = new Vector3(-Mathf.Sin(angle), 0, Mathf.Cos(angle)); // Try different initial condition
            bp[i].mass = 5; // Simplified. Try different initial condition


            // + This is just pretty trails
            trailRenderer = body[i].AddComponent<TrailRenderer>();
            // Configure the TrailRenderer's properties
            trailRenderer.time = 100.0f;  // Duration of the trail
            trailRenderer.startWidth = 0.5f;  // Width of the trail at the start
            trailRenderer.endWidth = 0.1f;    // Width of the trail at the end
            // a material to the trail
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
            // Set the trail color over time
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(Mathf.Cos(Mathf.PI * 2 / numberOfSphere * i), Mathf.Sin(Mathf.PI * 2 / numberOfSphere * i), Mathf.Tan(Mathf.PI * 2 / numberOfSphere * i)), 0.80f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            trailRenderer.colorGradient = gradient;

        }
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // Reset accelerations before calculating new forces
        for (int i = 0; i < numberOfSphere; i++)
        {
            bp[i].acceleration = Vector3.zero;
        }
        
        // Compute pairwise gravity
        for (int i = 0; i < numberOfSphere; i++)
        {
            for (int j = i + 1; j < numberOfSphere; j++)
            {
                Vector3 distanceVector = body[j].transform.position - body[i].transform.position;

                Vector3 force = CalculateGravity(distanceVector, bp[i].mass, bp[j].mass);

                // Apply accelerations (F = ma → a = F/m)
                bp[i].acceleration += force / bp[i].mass;
                bp[j].acceleration -= force / bp[j].mass; // equal & opposite
            }
        }

        // Integrate motion
        for (int i = 0; i < numberOfSphere; i++)
        {
            bp[i].velocity += bp[i].acceleration * dt;
            body[i].transform.position += bp[i].velocity * dt;
        }
    }

    // Gravity Fuction to finish
    private Vector3 CalculateGravity(Vector3 distanceVector, float m1, float m2)
    {
        Vector3 gravity = new Vector3(0f,0f,0f); // note this is also Vector3
        // **** Fill in the function below. 
        float G = 50f;
        float r = distanceVector.magnitude;
        gravity = G * m1 * m2 / (r * r * r) * distanceVector;

        return gravity;
    }
}

