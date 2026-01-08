import * as auth from "./auth_status_handler.js";

document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("loginForm").addEventListener("submit", function(event) {
        event.preventDefault(); // Evita o envio do formulário padrão

        // Obtém os valores dos campos de nome e email
        var name = document.getElementById("name").value;
        var email = document.getElementById("email").value;

        // Monta o objeto com os dados a serem enviados
        var data = {
            name: name,
            email: email
        };

        // Realiza a solicitação POST
        fetch("http://localhost:5058/v1/accounts", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        })
        .then(response => auth.handleHttpResponse(response, "register")) // Lida com a resposta http como um registro
        .then(data => {
            console.log(data); // Lidar com a resposta do servidor
        })
        .catch(error => {
            console.error("Erro:", error);
        });
    });

    document.querySelector(".register__button").addEventListener("click", function() {
        window.location.href = "../pages/login.html";
    });
});
