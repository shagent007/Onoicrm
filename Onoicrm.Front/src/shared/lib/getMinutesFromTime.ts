export const getMinutesFromTime = (time: string): number => {
  const [hours, minutes]: string[] = time.split(":");
  return parseInt(hours) * 60 + parseInt(minutes);
};
