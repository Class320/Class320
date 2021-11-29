using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject Target;

    [SerializeField]
    private GameObject Location;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Target)
        {
            Location.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Target)
        {
            Location.SetActive(false);
        }
    }
}
