import {getMinutesFromTime} from "../../../shared";

export const mergeTimeIntervals = (timeIntervals: string[]): string => {
  if (timeIntervals.length === 0) {
    return "";
  }

  timeIntervals.sort();
  let mergedIntervals: string[] = [timeIntervals[0]];

  for (let i = 1; i < timeIntervals.length; i++) {
    const currentInterval: string = timeIntervals[i];
    const [prevStartTime, prevEndTime]: string[] =
      mergedIntervals[mergedIntervals.length - 1].split("-");
    const [currentStartTime, currentEndTime]: string[] =
      currentInterval.split("-");

    const prevEndMinutes: number = getMinutesFromTime(prevEndTime);
    const currentStartMinutes: number = getMinutesFromTime(currentStartTime);

    if (currentStartMinutes <= prevEndMinutes) {
      mergedIntervals[
      mergedIntervals.length - 1
        ] = `${prevStartTime}-${currentEndTime}`;
    } else {
      mergedIntervals.push(currentInterval);
    }
  }

  return mergedIntervals.join(", ");
};

