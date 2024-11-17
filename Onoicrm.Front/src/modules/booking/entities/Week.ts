import {getGUID} from "~/shared/lib/getGUID";
import {Day} from "~/modules/booking/entities/Day";
export class Week {
    id:string = getGUID();
    days: Day[] = []

    constructor(init?:Partial<Week>) {
        Object.assign(this,init)
    }
}
