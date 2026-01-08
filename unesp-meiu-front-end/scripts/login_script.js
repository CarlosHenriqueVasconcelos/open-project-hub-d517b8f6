import * as auth from "./auth_status_handler.js";
import { saveAuthTokenToStorage } from "./token_handler.js";

document.getElementById("loginForm").addEventListener("submit", async function(event) {
    event.preventDefault(); // Evita o envio do formulário padrão

    // Obtém os valores dos campos de email e senha
    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;

    // Monta o objeto com os dados a serem enviados
    var data = {
        email: email,
        password: password
    };

    try {
        // Realiza a solicitação POST
        const response = await fetch("http://localhost:5058/v1/accounts/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        // Lida com a resposta http como um login
        const responseData = await auth.handleHttpResponse(response, "login");

        // Salva o token de autenticação de forma segura
        await saveAuthTokenToStorage(responseData.token);

        console.log(responseData); // Lidar com a resposta do servidor

    } catch (error) {
        console.error("Erro:", error);
    }
});

document.querySelector(".register__button").addEventListener("click", function() {
    window.location.href = "../pages/register.html";
});
