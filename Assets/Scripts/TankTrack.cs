using UnityEngine;

public class TankTrack : MonoBehaviour
{
    [SerializeField] float activeTime = 3.00f;

    // Update is called once per frame

    private void Start()
    {
        activeTime = 1.00f;
    }

    void Update()
    {
        activeTime -= Time.deltaTime;

        if (activeTime <= 0)
        {
            print("RETURNING TO POOL");
            Destroy(gameObject);
        }
    }
}
