import moment, {Moment} from "moment";

export const getYear = (year?: any): Moment => {
  year = year ?? +moment().format("YYYY");
  return moment(year, "YYYY").locale("ru");
}
