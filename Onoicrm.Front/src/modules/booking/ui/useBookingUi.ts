import moment from "moment";
export const isCompact = ({event}:any) => {
  const start = moment(event.start);
  const end = moment(event.end);
  const diff = end.diff(start, "minutes")
  return diff <= 30;
}
