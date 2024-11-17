export const getInitials = (fullName: string | null): string => {
  if (!fullName) {
    return '';
  }
  const names = fullName.split(' ');

  if(['уулу',"кызы"].includes(names[1])){
    return `${names[0].charAt(0)} ${names[2].charAt(0)}`;
  }

  return `${names[0].charAt(0)} ${names[1].charAt(0)}`;
}
