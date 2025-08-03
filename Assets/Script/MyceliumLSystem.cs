using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class MyceliumLSystem : MonoBehaviour
{
    [Header("Rule Settings")]
    public string axiom = "F";
    public int iterations = 5;
    public float segmentLength = 0.3f;
    public float turnAngle = 25f;

    [Header("Rendering")]
    public GameObject lineSegmentPrefab;  // A prefab with a LineRenderer (2 pts: 0 at (0,0), 1 at (0,1))

    // simple rule: F → F[+F]F[-F]F
    private Dictionary<char,string> rules = new Dictionary<char,string>
    {
        { 'F', "F[+F]F[-F]F" }
    };

    void Start()
    {
        string pattern = axiom;
        // generate the string
        for (int i = 0; i < iterations; i++)
        {
            var sb = new StringBuilder();
            foreach (char c in pattern)
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            pattern = sb.ToString();
        }
        Draw(pattern);
    }

    void Draw(string pattern)
    {
        var stack = new Stack<(Vector3 pos, Quaternion rot)>();
        Vector3 position = transform.position;
        Quaternion rotation = Quaternion.identity;

        foreach (char c in pattern)
        {
            if (c == 'F')
            {
                Vector3 newPos = position + rotation * Vector3.up * segmentLength;
                var seg = Instantiate(lineSegmentPrefab, transform);
                var lr = seg.GetComponent<LineRenderer>();
                lr.SetPosition(0, position);
                lr.SetPosition(1, newPos);
                position = newPos;
            }
            else if (c == '+')
                rotation *= Quaternion.Euler(0, 0, turnAngle);
            else if (c == '-')
                rotation *= Quaternion.Euler(0, 0, -turnAngle);
            else if (c == '[')
                stack.Push((position, rotation));
            else if (c == ']')
            {
                var state = stack.Pop();
                position = state.pos;
                rotation = state.rot;
            }
        }
    }
}

