import moment, {Moment} from "moment";
import 'moment/dist/locale/ru'
import {getMonth} from "~/modules/booking/services/getMonth";

export const getMonthLastSunday = (month?:any, year?:any):Moment => {
  month = getMonth(month, year)
  return month
    .endOf('month')
    .add(7 - (month.endOf('month').day() || 7), 'days')
};
