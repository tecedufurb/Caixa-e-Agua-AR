﻿//Classe responsável pelo gerenciamento dos elementos UI da Main Scene
//E do gerenciamento das perguntas apresentadas
//Está inserido no gameObject ThirdPersonCharacter

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Question : MonoBehaviour
{

    public AudioSource m_answerAudio;       //recebe o AudioSource que esta no objeto _gameManager
    public AudioClip m_rightAnswerAudio;    //armazena o AudioClip que é reproduzido quando a resposta estiver certa
    public AudioClip m_wrongAnswerAudio;    //armazena o AudioClip que é reproduzido quando a resposta estiver errada
    public AudioClip m_winAudio;            //armazena o AudioClip que é reproduzido quando o jogador finaliza o jogo (responde todas as perguntas)

    //inicio elementos do canvasMain
    public GameObject m_canvasMain;         //armazena o canvasMain
    public GameObject m_panelMessege;       //mensagem inicial que aparece ao iniciar o aplicativo
    public GameObject m_panelPressButton;   //mensagem que aparece ao entrar no collider Question dizendo para apertar o botao R
    public Text m_textRightHits;            //armazena a pontuação do jogador (calculado no metodo Resposta)
    public Button m_buttonShowQuestions;    //armazena o botão R que chama o canvasQuestions
    public Text m_textFinalAnswer;          //armazena a mensagem "Você acertou!" ou "Você errou!" dependendo da resposta
    //fim elementos do canvasMain

    //inicio elementos canvasQuestions
    public GameObject m_canvasQuestions;    //armazena o objeto Canvas para poder ativar ou desativar
    public Text m_textQuestion;             //armazena o texto da pergunta
    public Text m_textAnswerA;              //armazena o texto da alternativa A
    public Text m_textAnswerB;              //armazena o texto da alternativa B
    public Text m_textAnswerC;              //armazena o texto da alternativa C
    public Text m_textAnswerD;              //armazena o texto da alternativa D
    public string[] m_questions;            //armazena todas as perguntas
    public string[] m_answerA;              //armazena todas as alternativas A
    public string[] m_answerB;              //armazena todas as alternativas B
    public string[] m_answerC;              //armazena todas as alternativas C
    public string[] m_answerD;              //armazena todas as alternativas D
    public string[] m_rightAnswers;         //armazena todas as alternativas corretas
    //fim elementos canvasQuestions

    //inicio elementos do canvasFinal
    public GameObject m_canvasFinal;
    public Text m_textRightFinalScore;
    public Text m_textWrongFinalScore;
    //fim elementos do canvasFinal

    //armezena os colliders das perguntas
    public GameObject m_question1;
    public GameObject m_question2;
    public GameObject m_question3;
    public GameObject m_question4;
    public GameObject m_question5;
    public GameObject m_question6;

    private float m_rightHits;                  //conta a quantidade de respostas certas
    private float m_wrongHits;                  //conta a quantidade de respostas erradas
    private bool m_clicked = false;             //utilizada para ver se o botao foi pressionado ou nao
    private float m_time = 10f;                 //armazena o tempo maximo que o titulo vai permanecer na tela
    private float m_timeFinalAnswer = 5f;       //armazena o tempo maximo que a mensagem de acerto ou erro vai permanecer na tela
    private int m_currentQuestion;              //recebe o número da questão, que será referenciado no vetor respostas[]
    private int m_answeredQuestions;            //conta o numero de perguntas respondidas corretamente
    private bool m_boolRightAnswer = false;     //recebe true quando a resposta selecionada for a correta     

    void Update()
    {
        //faz com que a mensagem inicial desapareça após 10 segundos
        if (m_time > 0)
        {
            m_time -= Time.deltaTime;
            m_panelMessege.gameObject.SetActive(true);
        }
        else
        {
            m_panelMessege.gameObject.SetActive(false);
        }

        //faz com que a mensagem de acerto ou erro desapareça após 5 segundos
        if (m_clicked == true && m_timeFinalAnswer > 0)
        {
            m_timeFinalAnswer -= Time.deltaTime;
            if (m_timeFinalAnswer <= 0)
                m_textFinalAnswer.text = "";
        }

        m_textRightFinalScore.text = "" + m_rightHits;
        m_textWrongFinalScore.text = "" + m_wrongHits;
    }

    //ao entrar no colldier
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Question")          //só faz as acoes abaixo se o gameObject colidido ter a tag "Question"
        {
            m_boolRightAnswer = false;
            m_clicked = false;                              //garante que sempre que entrar do collider a m_clicou seja falsa para que assim as perguntas possam sempre aparecer ao entrar novamente
            m_timeFinalAnswer = 5;                          //o tempo que a resposta de acerto/erro aparece na tela volta a ser 5 para poder decrescer até 0 novamente
            m_textFinalAnswer.text = "";                    //garante que a resposta de Certo e Errado inicie em branco
            m_panelPressButton.gameObject.SetActive(true);
            m_buttonShowQuestions.interactable = true;      //torna o buttonPergunta interativo
        }
    }

    //enquanto está no collider
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Question")          //só faz as acoes abaixo se o gameObject colidido ter a tag "Question"
        {
            //Chama o método das perguntas quando o personagem esta em contato com algum collider
            Questions(collider);

            //destroi a pergunta se a resposta estiver certa
            DestroyQuestion(collider);
        }
    }

    //ao sair do collider
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Question")          //só faz as acoes abaixo se o gameObject colidido ter a tag "Question"
        {
            m_canvasQuestions.gameObject.SetActive(false);      //desliga novamente o canvas da pergunta e dos botões (caso o jogador saia do collider sem responder a pergunta)
            m_buttonShowQuestions.interactable = false;         //quando o avatar sai do collider o buttonPergunta deixa de ser interativo novamente
            m_panelPressButton.gameObject.SetActive(false);     //e o aviso para apertar o botao desaparece
        }
    }

    //chamado no OnClick() do buttonPergunta
    public void ButtonR()
    {
        m_panelPressButton.gameObject.SetActive(false);     //desativa o aviso para apertar o botao
        m_canvasQuestions.gameObject.SetActive(true);       //ativa o canvas das perguntas
        m_buttonShowQuestions.interactable = false;         //torna o buttonPergunta nao interativo (para nao poder recarregar a pergunta enquanto esta no collider)
        m_clicked = false;
    }

    //Método que gerencia as perguntas
    private void Questions(Collider collider)
    {
        //as perguntas e alternativas só aparecem se o jogador não tiver clicado em nenhuma alternativa 
        if (m_clicked == false)
        {
            //Se o nome do objeto com o qual o personagem colidiu for igual a um dos cases abaixo, cria uma determinada pergunta com suas alternativas
            switch (collider.gameObject.name)
            {
                case "Question1":
                    m_currentQuestion = 0;
                    QuestionAux(m_currentQuestion);
                    break;
                case "Question2":
                    m_currentQuestion = 1;
                    QuestionAux(m_currentQuestion);
                    break;
                case "Question3":
                    m_currentQuestion = 2;
                    QuestionAux(m_currentQuestion);
                    break;
                case "Question4":
                    m_currentQuestion = 3;
                    QuestionAux(m_currentQuestion);
                    break;
                case "Question5":
                    m_currentQuestion = 4;
                    QuestionAux(m_currentQuestion);
                    break;
                case "Question6":
                    m_currentQuestion = 5;
                    QuestionAux(m_currentQuestion);
                    break;
            }
        }
        else
        {
            m_canvasQuestions.gameObject.SetActive(false);
        }
    }

    //Método que gerencia as perguntas. Chamado no Questions()
    private void QuestionAux(int currentQuestion) {
        m_textQuestion.text = m_questions[currentQuestion];
        m_textAnswerA.text = m_answerA[currentQuestion];
        m_textAnswerB.text = m_answerB[currentQuestion];
        m_textAnswerC.text = m_answerC[currentQuestion];
        m_textAnswerD.text = m_answerD[currentQuestion];
    }

    //método chamado no OnClick() dos 4 botões de alternativas de resposta (ao chamar o método é preciso passar a string dos cases no unity)
    public void Answer(string answer)
    {

        switch (answer)
        {
            case "A":
                RightWrongAnswer(m_answerA[m_currentQuestion]);
                break;
            case "B":
                RightWrongAnswer(m_answerB[m_currentQuestion]);
                break;
            case "C":
                RightWrongAnswer(m_answerC[m_currentQuestion]);
                break;
            case "D":
                RightWrongAnswer(m_answerD[m_currentQuestion]);
                break;
        }
        m_answerAudio.Play();       //toca o som de resposta
    }

    //metodo que verifica se a resposta esta certa ou errada. Recebe o vetor da pergunta como parametro
    private void RightWrongAnswer(string answer) {
        //se a resposta selecionada estiver certa a variavel m_hits é implementada e a m_respostaFinal informa "Você acertou!" em verde
        if (answer == m_rightAnswers[m_currentQuestion])
        {
            m_clicked = true;                               //ao receber true faz com que as perguntas e alternativas desapareçam da tela
            m_rightHits++;
            m_textRightHits.text = "Acertos: " + m_rightHits;

            //só toca a musica de acerto e imprime a imagem na tela se o numero de acertos for menor do que o numero de perguntas
            if (m_rightHits < m_questions.Length)
            {
                m_textFinalAnswer.GetComponent<Text>().color = Color.green;
                m_textFinalAnswer.text = "Você acertou!";
                m_answerAudio.clip = m_rightAnswerAudio;        //faz a variavel m_answerAudio receber o audio clip da resposta correta
            }
            else    //se o numero de acertos for igual o numero de perguntas toca a musica de vitoria e ativa o canvasFinal
            {
                m_canvasMain.gameObject.SetActive(false);
                m_canvasFinal.gameObject.SetActive(true);
                m_answerAudio.clip = m_winAudio;
            }
            m_boolRightAnswer = true;
        }
        else
        { //e se estiver errada a m_respostaFinal informa "Você errou!" em vermelho
            m_clicked = true;
            m_wrongHits++;
            m_textFinalAnswer.GetComponent<Text>().color = Color.red;
            m_textFinalAnswer.text = "Você errou!";
            m_answerAudio.clip = m_wrongAnswerAudio;        //faz a variavel m_answerAudio receber o audio clip da resposta errada
            m_buttonShowQuestions.interactable = true;      //torna o buttonPergunta interativo
        }
    }

    //verifica em qual pergunta o personagem esta para depois destrui-la se a resposta estiver correta
    private void DestroyQuestion(Collider collider)
    {
        switch (collider.gameObject.name)
        {
            case "Question1":
                DestroyQuestionAux(m_question1);
                break;
            case "Question2":
                DestroyQuestionAux(m_question2);
                break;
            case "Question3":
                DestroyQuestionAux(m_question3);
                break;
            case "Question4":
                DestroyQuestionAux(m_question4);
                break;
            case "Question5":
                DestroyQuestionAux(m_question5);
                break;
            case "Question6":
                DestroyQuestionAux(m_question6);
                break;
        }
    }

    //metodo que verifica se a resposta esta correta e, se estiver, destroi a pergunta
    private void DestroyQuestionAux(GameObject question) {
        if ((m_answerA[m_currentQuestion] == m_rightAnswers[m_currentQuestion]) && (m_boolRightAnswer == true))
            Destroy(question);
        else if ((m_answerB[m_currentQuestion] == m_rightAnswers[m_currentQuestion]) && (m_boolRightAnswer == true))
            Destroy(question);
        else if ((m_answerC[m_currentQuestion] == m_rightAnswers[m_currentQuestion]) && (m_boolRightAnswer == true))
            Destroy(question);
        else if ((m_answerD[m_currentQuestion] == m_rightAnswers[m_currentQuestion]) && (m_boolRightAnswer == true))
            Destroy(question);
    }

    //chamado no OnClick() do botao do canvasFinal
    public void KeepPlaying() {
        m_rightHits = 0;
        m_canvasFinal.gameObject.SetActive(false);
        m_canvasMain.gameObject.SetActive(true);
    }
}