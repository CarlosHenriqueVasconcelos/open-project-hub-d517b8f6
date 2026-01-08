async function saveAuthTokenToStorage(token) {
    const encoder = new TextEncoder();
    const data = encoder.encode(token);

    // Cria uma chave de criptografia
    const key = await window.crypto.subtle.generateKey(
        { name: "AES-GCM", length: 256 },
        true,
        ["encrypt", "decrypt"]
    );

    // Criptografa os dados usando a chave
    const encryptedData = await window.crypto.subtle.encrypt(
        { name: "AES-GCM", iv: window.crypto.getRandomValues(new Uint8Array(12)) },
        key,
        data
    );

    // Converte os dados criptografados em uma string base64
    const encryptedToken = btoa(String.fromCharCode.apply(null, new Uint8Array(encryptedData)));

    // Armazena o token criptografado no sessionStorage
    sessionStorage.setItem("authToken", encryptedToken);

}

async function getAuthTokenFromStorage() {
    // Recupera o token criptografado do sessionStorage
    const encryptedToken = sessionStorage.getItem("authToken");

    if (!encryptedToken) return null;

    // Decodifica a string base64 de volta para um ArrayBuffer
    const encryptedData = new Uint8Array(atob(encryptedToken).split("").map(char => char.charCodeAt(0)));

    // Recupera a chave de criptografia
    const key = await window.crypto.subtle.generateKey(
        { name: "AES-GCM", length: 256 },
        true,
        ["encrypt", "decrypt"]
    );

    // Descriptografa os dados usando a chave
    const decryptedData = await window.crypto.subtle.decrypt(
        { name: "AES-GCM", iv: new Uint8Array(12) },
        key,
        encryptedData
    );

    // Decodifica os dados descriptografados de volta para uma string
    const decoder = new TextDecoder();
    const token = decoder.decode(decryptedData);

    return token;
}

export { saveAuthTokenToStorage, getAuthTokenFromStorage };
