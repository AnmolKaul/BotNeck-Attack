using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    public static Server instance;

    [SerializeField] private Image healthFillImg;
    [SerializeField] private Image antivirusFillImg;


    private int virusCount = 0;
    private int goodPacketCount = 0;

    [SerializeField] private Text virusTerminated;
    [SerializeField] private Text goodPackets;

    [SerializeField] private GameObject shieldImg;

    private bool canTakeDamage = true;

    private void Start()
    {
        instance = this;

        healthFillImg.fillAmount = 1;
        antivirusFillImg.fillAmount = 0;

        virusTerminated.text = "Virus Terimanted : " + virusCount;
        goodPackets.text = "Good Packet : " + goodPacketCount;
    }

    public void ActivateShieldImage()
    {
        shieldImg.SetActive(true);
    }

    public void DeactivateShieldImage()
    {
        shieldImg.SetActive(false);
    }

    public void ModifyGoodPackets(int val)
    {
        goodPacketCount += val;
        goodPackets.text = "Good Packet : " + goodPacketCount;
    }

    public void ModifyVirusPackets(int val)
    {
        virusCount += val;
        virusTerminated.text = "Virus Terimanted : " + virusCount;
    }

    public void TakeDamage(float dmg)
    {
        if (healthFillImg.fillAmount <= 0)
        {
            // Game over 
            // Restart the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            if (canTakeDamage)
            {
                healthFillImg.fillAmount -= dmg;
            }
        }
    }

    public void Antivirus(float val)
    {
        if (antivirusFillImg.fillAmount >= 1)
        {
            // Shielding for some time
            StartCoroutine(TimeCounter());
        }
        else
        {
            antivirusFillImg.fillAmount += val;
        }
    }

    IEnumerator TimeCounter()
    {
        canTakeDamage = false;
        ActivateShieldImage();
        yield return new WaitForSeconds(3f);
        DeactivateShieldImage();
        canTakeDamage = true;

        // Reset antivirus shield bar
        antivirusFillImg.fillAmount = 0;
    }
}
