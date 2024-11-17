export const genderCaption = (genderId: number) => {
  switch (genderId) {
    case 0: return "Женский";
    case 1: return "Мужской";
    case 2: return "Не определено";
  }
};
