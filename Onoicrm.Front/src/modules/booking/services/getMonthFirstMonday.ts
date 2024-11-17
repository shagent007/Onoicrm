import moment, {Moment} from "moment";
import 'moment/dist/locale/ru'
import {getMonth} from "~/modules/booking/services/getMonth";

export const getMonthFirstMonday = (month?:any, year?:any):Moment => {
  month = getMonth(month, year)
  return month
    .startOf('month')
    .subtract((month.startOf('month').day() || 7) - 1, 'd');
}

