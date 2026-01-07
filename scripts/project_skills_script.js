// Seleciona a seção disciplinas
var coursesButton = document.querySelector('.coursesButton');
coursesButton.style.backgroundColor = '#E1E3FF';
var coursesLink = document.querySelector('.coursesLink'); // Seleciona o link dentro de coursesButton
coursesLink.style.color = '#692DFF'; // Define a cor do texto do link


document.addEventListener("DOMContentLoaded", function() {
    const skillsBox = document.querySelector('.skillsBox');
    const addedSkills = document.querySelector('.addedSkills');

    // Função para mover uma habilidade para addedSkills
    function moveSkill(skillElement) {
        const skillName = skillElement.querySelector('.skillName').innerText;
        const newSkill = document.createElement('div');
        newSkill.classList.add('skill');
        newSkill.innerHTML = `<span class="skillName">${skillName} <i class="fa-solid fa-x"></i></span>`;
        addedSkills.appendChild(newSkill);
        skillElement.remove();
    }

    // Função para remover uma habilidade de addedSkills
    function removeSkill(event) {
        const skillElement = event.target.closest('.skill');
        if (skillElement) {
            skillElement.remove();
        }
    }

    // Adicionando evento de clique para cada skill no skillsBox
    const skills = skillsBox.querySelectorAll('.skill');
    skills.forEach(skill => {
        skill.addEventListener('click', function() {
            moveSkill(skill);
        });
    });

    // Adicionando evento de clique usando delegação de eventos para addedSkills
    addedSkills.addEventListener('click', function(event) {
        removeSkill(event);
    });

    // Adicionando evento para a caixa de entrada de habilidades
    const skillsInput = document.querySelector('.skillsInput');
    skillsInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            const newSkillName = skillsInput.value.trim();
            if (newSkillName !== '') {
                const newSkill = document.createElement('div');
                newSkill.classList.add('skill');
                newSkill.innerHTML = `<span class="skillName">${newSkillName} <i class="fa-solid fa-x"></i></span>`;
                addedSkills.appendChild(newSkill);
                skillsInput.value = '';
            }
        }
    });
});

