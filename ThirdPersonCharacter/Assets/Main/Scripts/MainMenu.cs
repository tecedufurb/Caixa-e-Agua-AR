//Classe responsavel por gerenciar o menu principal da scene MainMenu e o buttonBackMenu da scene MainScene

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject m_canvasMainMenu;         //armazena o canvas do menu principal
    public GameObject m_canvasInstructions;     //armazena o canvas das instrucoes
    public GameObject m_canvasConfiguration;    //armazena o canvas das configuracoes
    public Image m_imageButtonVolume;           //armazena a imagem do buttonVolume
    public GCCamera m_camera;                   //armazena a camera que possui o Audio Listener

    void Awake()
    {
        if (!PlayerPrefs.HasKey("volume"))      //Senão existir nenhuma informação já salva
            PlayerPrefs.SetInt("volume", 1);    //Definimos o volume como ativo

        var volume = PlayerPrefs.GetInt("volume");
        var color = m_imageButtonVolume.color;
        if (volume == 1)
            color.a = 1f; //Alteramos o valor de alpha
        else
            color.a = 0.4f; //Alteramos o valor de alpha
        m_imageButtonVolume.color = color;
    }

    // Use this for initialization
    void Start () {
        //apenas garante que a primeira tela apresentada é a do menu principal
        m_canvasMainMenu.gameObject.SetActive(true);
        m_canvasInstructions.gameObject.SetActive(false);
        m_canvasConfiguration.gameObject.SetActive(false);
    }

    //chamado no metodo OnClick do buttonPlay. Inicializa a cena principal da caixa de areia
    public void Play() {
        SceneManager.LoadScene("MainScene");
    }

    //chamado no metodo Onclick do buttonInstruction. Desabilita o canvas do menu principal e habilita o canvas das instrucoes
    public void Instructions() {
        m_canvasMainMenu.gameObject.SetActive(false);
        m_canvasInstructions.gameObject.SetActive(true);
    }

    //chamado no metodo Onclick do buttonConfigurations. Desabilita o canvas do menu principal e habilita o canvas das configuracoes
    public void Configurations()
    {
        m_canvasMainMenu.gameObject.SetActive(false);
        m_canvasConfiguration.gameObject.SetActive(true);
    }

    //chamado no metodo OnClick do buttonQuit. Fecha a aplicação
    public void Quit() {
        Application.Quit();
    }

    //chamado no metodo OnClick do buttonBack do canvasInstructions, canvasConfigurations e no buttonBackMenu do canvasMain na scene MainScene
    public void Back() {
        SceneManager.LoadScene("MainMenu");
    }

    //chamado no metodo OnClick do buttonVolume do canvasConfigurations
    public void Volume()
    {
        var volume = PlayerPrefs.GetInt("volume");      //Recupera informação sobre o som
        var color = m_imageButtonVolume.color;          //Recupera a cor do botão
        if (volume == 0.5)
        {
            PlayerPrefs.SetInt("volume", 0);    //Salva a informação do volume
            color.a = 0.4f;                     //Torna o botão mais transparente
            m_camera.DisableSound();            //Desabilita o som
        }
        else
        {
            PlayerPrefs.SetInt("volume", 1);    //Salva a informação do volume
            color.a = 1f;                       //Torna o botão 100% visivel
            m_camera.EnableSound();             //Habilita o som
        }
        m_imageButtonVolume.color = color;
    }
}
