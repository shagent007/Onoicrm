import moment, {Moment} from "moment/moment";
import {getYear} from "~/modules/booking/services/getYear";

export const getMonth = (month?:any, year?:any):Moment => {
  month = month ?? +moment().month();
  return getYear(year).month(month);
}


