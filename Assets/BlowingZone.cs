using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Windows.Speech;
using System.Linq;

public class BlowingZone : MonoBehaviour
{
    //Holders
    public Animation anim;
    //Our Bomb object
    public GameObject bomb;
    //Variables for the explosion
    public float powerOfExplosion = 10.0f;
    public float radiusOfExplosion = 5.0f;
    public float upForceOfExplosion = 2.0f;
    public GameObject bigExplosionPrefab;
    //VoiceRec
    private KeywordRecognizer keyword;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Start()
    {
        actions.Add("forward", Forward);
        actions.Add("backward", Backward);
        actions.Add("up", Up);
        actions.Add("down", Down);
        actions.Add("explode", Detonate);
        anim = GetComponent<Animation>();
        keyword = new KeywordRecognizer(actions.Keys.ToArray());
        keyword.OnPhraseRecognized += RecognizedSpeech;
        keyword.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Forward()
    {
        bomb.transform.Translate(0, 0, 1);
    }

    private void Backward()
    {
        bomb.transform.Translate(0, 0, -1);
    }

    private void Up()
    {
        bomb.transform.Translate(0, 1, 0);
    }

    private void Down()
    {
        bomb.transform.Translate(0, -1, 0);
    }

    private void Detonate()
    {
        if (bomb == enabled)
        {
            anim.Play();
            Invoke("Explode", 6);

        }

    }

    private void Explode()
    {
        Instantiate(bigExplosionPrefab, transform.position, transform.rotation);
        //The explosion will start where the bomb is
        Vector3 explosionPosition = bomb.transform.position;
        //Bringing colliders that are in range 
        Collider[] collidersOfObjects = Physics.OverlapSphere(explosionPosition, radiusOfExplosion);
        foreach (Collider hit in collidersOfObjects)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(powerOfExplosion, explosionPosition, radiusOfExplosion, upForceOfExplosion, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }

   

    

}
