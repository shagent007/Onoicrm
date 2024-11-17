import {Moment} from "moment/moment";
import moment from "moment";

export const orderBy={
  asc:(format:string, args:any[])=> args.map((i: any) => moment(i, format))
    .sort((a, b) => a.isBefore(b) ? -1 : 1),
  desc:(format:string, args:any[]) => args
    .map((i: any) => moment(i, format))
    .sort((a, b) => a.isAfter(b) ? -1 : 1)
}
