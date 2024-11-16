using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests {
    //referencia a la clase InputTestFixure para el InputSystem
    public class TestSuite : InputTestFixture {
        //instanciacion de los atributos para el Mouse y el Teclado
        Mouse mouse;
        Keyboard keyboard;

        //Metodo para la implementacion del evento click para el sistema de matrices
        public void ClickUI (GameObject uiElement) {
            Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Vector3 screenPos = camera.WorldToScreenPoint(uiElement.transform.position);
            Set(mouse.position, screenPos);
            Click(mouse.leftButton);
        }

        // metodo Setup especifico para el InputTestFixure
        public override void Setup() {
            base.Setup();
            SceneManager.LoadScene("Match 3 Starter/Scenes/Game"); //instaciacion de la escena Game para las pruebas
            mouse = InputSystem.AddDevice<Mouse>();
            keyboard = InputSystem.AddDevice<Keyboard>();
        }

        //Prueba para el evento de emparejamiento de 3 caramelos iguales
        [UnityTest] public IEnumerator MatchTres() {
            //instanciacion del objeto scoreTxt
            GameObject scoreTexto = GameObject.Find("ScorePanel/ScoreTxt");

            //armado de la matriz de 3x3 para la prueba
            string[,] grid = new string[3,3] { {"Y","B","M"}, {"Y","R","G"}, {"P","Y","M"}};
            BoardManager.instance.InitializeBoard(grid); //intanciacion de la matriz dentro de la clase BoardManager
            yield return null;
            string score = scoreTexto.GetComponent<Text>().text;
            Assert.That(score, Is.EqualTo("0")); //para asegurarnos que la puntuacion inicial es de CERO

            //Uso del metodo ClickUp para mover 2 caramelos con la intencion de realizar el Match de 3
            yield return new WaitForSeconds(1f);
            ClickUI(BoardManager.instance.tiles[1,2]); 
            yield return new WaitForSeconds(1f);
            ClickUI(BoardManager.instance.tiles[0,2]);
            yield return new WaitForSeconds(2f);

            score = scoreTexto.GetComponent<Text>().text;
            Assert.That(score, Is.EqualTo("150")); //si la puntuacion actual es igual a 150 puntos estonces la prueba es correcta
            //cada caramelo equivale a 50 puntos
        }

        //Prueba para el evento de emparejamiento de 3 caramelos iguales
        [UnityTest] public IEnumerator MatchSpecialCuarto() {
            //instanciacion del objeto scoreTxt
            GameObject scoreTexto = GameObject.Find("ScorePanel/ScoreTxt");

            //armado de la matriz de 4x4 para la prueba
            string[,] grid = new string[4,4] { {"Y","B","M","G"}, {"Y","R","G","R"}, {"Y","P","M","B"}, {"R","Y","P","M"}};
            BoardManager.instance.InitializeBoard(grid);//intanciacion de la matriz dentro de la clase BoardManager
            yield return null;
            string score = scoreTexto.GetComponent<Text>().text;
            Assert.That(score, Is.EqualTo("0")); //para asegurarnos que la puntuacion inicial es de CERO

            //Uso del metodo ClickUp para mover 2 caramelos con la intencion de realizar el Match de 4
            yield return new WaitForSeconds(1f);
            ClickUI(BoardManager.instance.tiles[1,3]);
            yield return new WaitForSeconds(1f);
            ClickUI(BoardManager.instance.tiles[0,3]);
            yield return new WaitForSeconds(2f);

            score = scoreTexto.GetComponent<Text>().text;
            Assert.That(score, Is.EqualTo("200"));//si la puntuacion actual es igual a 200 puntos estonces la prueba es correcta
        }

        //SOLO FUNCIONA CORRECTAMENTE SI CORRE ESTA PRUEBA DE MANERA INDIVIDUAL
        //Prueba para el evento de salir del juego al presionar el boton Escape
        [UnityTest] public IEnumerator VolverAMenu() {
            string[,] grid = new string[3,3] { {"Y","B","M"}, {"G","R","P"}, {"M","Y","G"}};
            BoardManager.instance.InitializeBoard(grid);
            yield return new WaitForSeconds(1f);
            PressAndRelease(keyboard.escapeKey); // realiza o simula en evento de presionar boton Escape
            yield return new WaitForSeconds(5f);
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Menu")); //si el nombre de la escena activa es igual a MENU, entonces la prueba salio correctamente
            

        }
    }
}
