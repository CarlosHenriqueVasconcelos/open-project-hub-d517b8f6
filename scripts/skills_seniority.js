const skills = document.querySelectorAll('.skill');

skills.forEach(skill => {
  const starIcons = skill.querySelectorAll('i.fa-star');

  starIcons.forEach((starIcon, index) => {
    starIcon.addEventListener('click', () => {
      // Remove a classe solid para todas as estrelas
      starIcons.forEach(icon => {
        icon.classList.remove('fa-solid');
      });

      // Adiciona a classe solid para as estrelas até a estrela clicada
      for (let i = 0; i <= index; i++) {
        starIcons[i].classList.add('fa-solid');
      }

      // Remove a classe solid para as estrelas após a estrela clicada
      for (let i = index + 1; i < starIcons.length; i++) {
        starIcons[i].classList.remove('fa-solid');
      }
    });
  });
});
