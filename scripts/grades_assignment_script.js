// Seleciona a seção alunos
var studentsButton = document.querySelector('.studentsButton');
studentsButton.style.backgroundColor = '#E1E3FF';
var studentsLink = document.querySelector('.studentsLink'); // Seleciona o link dentro de studentsButton
studentsLink.style.color = '#692DFF'; // Define a cor do texto do link

// Função para calcular a média das notas selecionadas
function calculateAverage() {
    // Inicialize variáveis para somar as notas e contar quantas foram selecionadas
    let total = 0;
    let contador = 0;

    // Iterar sobre todas as checkboxes
    checkboxes.forEach(checkbox => {
        // Se o checkbox estiver marcado
        if (checkbox.checked) {
            // Adicione o valor do checkbox à soma total
            total += parseInt(checkbox.value);
            // Aumente o contador
            contador++;
        }
    });

    // Calcular a média
    const media = contador > 0 ? total / contador : 0;

    // Exibir a média na seção finalGrade
    const finalGrade = document.querySelector('.finalGrade');
    finalGrade.textContent = `Média parcial do aluno: ${media.toFixed(2)}`; // Arredonda para 2 casas decimais
}

// Pegando todas as divs de notas
const notasDivs = document.querySelectorAll('.grades');

// Inicialize uma array para armazenar todas as checkboxes
const checkboxes = [];

// Adicionando evento de clique a cada div de notas
notasDivs.forEach(div => {
    // Pegando todos os checkboxes dentro da div atual
    const checkboxesInDiv = div.querySelectorAll('input[type="checkbox"]');
    checkboxes.push(...checkboxesInDiv); // Adiciona checkboxesInDiv à array checkboxes

    // Adicionando evento de clique a cada checkbox
    checkboxesInDiv.forEach(checkbox => {
        checkbox.addEventListener('click', function() {
            // Desmarca todos os outros checkboxes quando um é marcado
            checkboxesInDiv.forEach(cb => {
                if (cb !== this) {
                    cb.checked = false;
                }
            });
            calculateAverage();
        });
    });
});
