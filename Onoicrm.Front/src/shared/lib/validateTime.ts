export function validateTime(str:string) {
  // Регулярное выражение для проверки времени в формате "часы:минуты"
  const timeRegex = /^([01]\d|2[0-3]):[0-5]\d$/;

  // Проверяем, соответствует ли строка регулярному выражению
  return timeRegex.test(str);
}
