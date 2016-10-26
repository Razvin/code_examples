using UnityEngine;
using System.Collections;

public class FreezeEffect : MonoBehaviour {

    public float effectTime = 2.5f;
    public float tickTime = .2f;
    private float timeRemaining = 0;

    IEnumerator Freeze()
    {
        //Set move speed to zero
        PlayerMovement playerMove = this.GetComponent<PlayerMovement>();
        GameObject snowflake = Resources.Load("snowPrefab") as GameObject;
        for (int i = 0; i < 6; i++)
            Instantiate(snowflake,this.transform.position,Quaternion.identity); 
        float moveTemp = playerMove.moveSpeed;
        playerMove.moveSpeed = 0.0f;
        //Set rigid body to non-moving
        Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        Color temp = renderer.color;
        Debug.Log(temp);
        renderer.color = Color.blue;
        timeRemaining = effectTime;
        while (timeRemaining > 0)
        {
            timeRemaining -= tickTime;
            yield return new WaitForSeconds(tickTime);
            Instantiate(snowflake, this.transform.position, Quaternion.identity);
        }
        renderer.color = temp;
        Debug.Log(renderer.color);
        playerMove.moveSpeed = moveTemp;
        Destroy(this);
    }

    public void StartEffect()
    {
        StartCoroutine("Freeze");
    }

    public void Refresh()
    {
        timeRemaining = effectTime;
    }
}
