using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pixel : MonoBehaviour
{
    // Start is called before the first frame update
    public bool connected;
    public bool isCordinater;
    public bool change;
    public int number;
    public Vector2 coordinate;

    private GameObject cd;
    private GameObject gcd;

    private GameManager GM;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        cd = transform.GetChild(0).gameObject;
        gcd = cd.transform.GetChild(0).gameObject;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(isCordinater)
        {
            gcd.GetComponent<SpriteRenderer>().color = Color.red;
            connected = true;
        }
        else if (connected)
        {
            gcd.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            gcd.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pixel"))
        {

            var heading = this.transform.position - collision.gameObject.transform.position;
            Pixel pixel = collision.gameObject.GetComponent<Pixel>();
            
            if (!pixel.isCordinater && !pixel.connected && this.isCordinater)
            {
                GM.CordinaterToRouter(this.number, pixel.number);
                // if(this.change)
                // {
                //     if(!this.isCordinater)
                //     {
                //         this.connected = false;
                //         pixel.connected = false;
                //         pixel.change = true;
                //     }
                //     this.change = false;
                //     return;
                // }

                //pixel.connected = true;
                if (Math.Abs(heading.x) < Math.Abs(heading.y))
                {
                    if (heading.y > 0)
                    {
                        pixel.coordinate = this.coordinate + new Vector2(0, 1);
                    }
                    else
                    {
                        pixel.coordinate = this.coordinate + new Vector2(0, -1);
                    }
                }
                else
                {
                    if (heading.x > 0)
                    {
                        pixel.coordinate = this.coordinate + new Vector2(1, 0);
                    }
                    else
                    {
                        pixel.coordinate = this.coordinate + new Vector2(-1, 0);
                    }
                }
            }
            else if(!pixel.isCordinater && !pixel.connected && this.connected)
            {
                GM.RouterToRouter(this.number, pixel.number);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pixel"))
        {
            GM.ConnectionOff(this.number, collision.gameObject.GetComponent<Pixel>().number);
        }
    }

    public void DisableCollider()
    {
        boxCollider2D.enabled = false;
    }
    public void EnableCollider()
    {
        boxCollider2D.enabled = true;
    }
}
