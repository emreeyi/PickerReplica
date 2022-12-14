using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class TopAlaniTeknikÝslemler
{
    public Animator TopAlaniAsansor;
    public TextMeshProUGUI SayiText;
    public int AtilmasiGerekenTop;
    public GameObject[] Toplar;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ToplayiciObje;
    [SerializeField] private GameObject TopKontrolObjesi;
    public bool ToplayiciHareketDurumu;
    int AtilanTopSayisi;
    int ToplamCheckPointSayisi;
    int MevcutCheckPointIndex;
    float ParmakPozX;
    [SerializeField] public List<TopAlaniTeknikÝslemler> _TopAlaniTeknikÝslemler = new List<TopAlaniTeknikÝslemler>();
    // Start is called before the first frame update
    void Start()
    {
        ToplayiciHareketDurumu = true;
        for (int i = 0; i < _TopAlaniTeknikÝslemler.Count; i++)
        {
            _TopAlaniTeknikÝslemler[i].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikÝslemler[i].AtilmasiGerekenTop;
        }
        ToplamCheckPointSayisi = _TopAlaniTeknikÝslemler.Count - 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (ToplayiciHareketDurumu)
        {
            ToplayiciObje.transform.position += 5 * Time.deltaTime * ToplayiciObje.transform.forward;
            if (Time.timeScale != 0)
            {
                if (Input.touchCount>0)
                {
                    Touch parmak = Input.GetTouch(0);
                    if (parmak.deltaPosition.x > 25f)
                    {
                        ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(ToplayiciObje.transform.position.x + .1f, ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), .3f);
                    }
                    if (parmak.deltaPosition.x < -25f)
                    {
                        ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(ToplayiciObje.transform.position.x - .1f, ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), .3f);
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(ToplayiciObje.transform.position.x - .1f, ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), 0.05f);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(ToplayiciObje.transform.position.x + .1f, ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), 0.05f);
                }
            }
        }
    }
    public void SiniraGelindi()
    {
        ToplayiciHareketDurumu = false;
        Invoke("AsamaKontrol", 2f);
        Collider[] HitColl = Physics.OverlapBox(TopKontrolObjesi.transform.position, TopKontrolObjesi.transform.localScale / 2, Quaternion.identity);
        int i = 0;
        while (i < HitColl.Length)
        {
            HitColl[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, .8f), ForceMode.Impulse);
            i++;
        }
    }
    public void ToplariSay()
    {
        AtilanTopSayisi++;
        _TopAlaniTeknikÝslemler[MevcutCheckPointIndex].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikÝslemler[MevcutCheckPointIndex].AtilmasiGerekenTop;
    }
    void AsamaKontrol()
    {
        if (AtilanTopSayisi >= _TopAlaniTeknikÝslemler[MevcutCheckPointIndex].AtilmasiGerekenTop)
        {
            Debug.Log("Kazandýn");
            _TopAlaniTeknikÝslemler[MevcutCheckPointIndex].TopAlaniAsansor.Play("Asansor");
            foreach (var item in _TopAlaniTeknikÝslemler[MevcutCheckPointIndex].Toplar)
            {
                item.SetActive(false);
            }
            if (MevcutCheckPointIndex == ToplamCheckPointSayisi)
            {
                Debug.Log("Oyun Bitti");
                Time.timeScale = 0;
            }
            else
            {
                MevcutCheckPointIndex++;
                AtilanTopSayisi = 0;
            }
        }
        else
        {
            Debug.Log("Kaybettin");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(TopKontrolObjesi.transform.position, TopKontrolObjesi.transform.localScale);
    }
}
