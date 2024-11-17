export const getQuarterCaption = (quarter: number) => {
  switch (quarter) {
    case 1: return "Верхней правой челюсти";
    case 2: return "Верхней левой челюсти";
    case 3: return "Нижней левой челюсти";
    case 4: return "Нижней правой челюсти";
  }
};

export const getToothCaption = (tooth: any) => {
  let toothSide = "";
  switch (tooth.quarter) {
    case 1: toothSide = "верхней правой челюсти";
      break;
    case 2: toothSide = "верхней левой челюсти";
      break;
    case 3: toothSide = "нижней правой челюсти";
      break;
    case 4: toothSide = "нижней левой челюсти";
      break;
    default: toothSide = "неизвестной челюсти";
  }

  return `${tooth.id}-${tooth.caption.toLowerCase()} ${toothSide} №${
    tooth.position
  }`;
};

