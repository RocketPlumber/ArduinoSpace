using UnityEngine;
using System.Collections;

public class Destroy_by_boundary : MonoBehaviour {

    void OnTriggerExit(Collider other) {
        Destroy(other.gameObject);
    }
}
