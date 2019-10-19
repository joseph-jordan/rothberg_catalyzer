using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    private string email;
    private string password;


    private void advance() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetEmail(string newEmail) {
        email = newEmail;
    }

    public void SetPassword(string newPassword) {
        password = newPassword;
    }

    public void OnSubmit() {
        advance();
    }
}
