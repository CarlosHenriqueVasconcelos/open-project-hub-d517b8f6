export function displayError(message) {
    var errorBox = document.createElement("div");
    errorBox.innerHTML = "<i class='fa-solid fa-triangle-exclamation'></i> " + message; // Adiciona o ícone junto com a mensagem
    errorBox.classList.add("error-box"); // Adiciona a classe CSS para a animação
    document.body.appendChild(errorBox);

    setTimeout(function() {
        errorBox.classList.add("slide-down"); // Adiciona a classe para a animação de slideDown
        setTimeout(function() {
            errorBox.remove(); // Remove o elemento após a animação de slideDown
        }, 500); // Tempo correspondente à duração da animação slideDown
    }, 4000); // Tempo em que a mensagem é exibida antes de desaparecer
}

export function displaySuccess(message) {
    var successBox = document.createElement("div");
    successBox.innerHTML = "<i class='fa-solid fa-circle-check'></i> " + message;
    successBox.classList.add("success-box");
    document.body.appendChild(successBox);

    setTimeout(function() {
        successBox.classList.add("slide-down");
        setTimeout(function() {
            successBox.remove();
        }, 500);
    }, 4000);
}

const loginStatusHandlers = {
    401: () => displayError("Credenciais inválidas!"),
    500: () => displayError("Erro interno do servidor!")
};

const registerStatusHandlers = {
    400: () => displayError("Email já cadastrado!"),
    500: () => displayError("Erro interno do servidor!")
};

export function handleHttpResponse(response, page) {
    if (response.ok) {
        if (page === "register") {
            displaySuccess("Cadastro realizado com sucesso!");
            setTimeout(function() {
                window.location.href = "../pages/login.html";
            }, 3000);
        } else if (page === "login") {
            displaySuccess("Login realizado com sucesso!");
        }
        return response.json(); // Retorna os dados da resposta em caso de sucesso
    } else {
        let handler;
        if (page === "register") {
            handler = registerStatusHandlers[response.status];
        } else if (page === "login") {
            handler = loginStatusHandlers[response.status];
        }
        if (handler) {
            handler();
        } else {
            displayError(`Erro: ${response.status}`);
        }
        throw new Error(response.status); // Lança um erro com o código de status HTTP
    }
}
