import {getGUID} from "~/shared/lib/getGUID";
import {TimeSlot} from "~/modules/booking/entities/TimeSlot";
import {BookingState, DisplayService} from "~/modules/booking/services/DisplayerService";
import {Booking} from "~/modules/booking";

export class Day {
    public id:string = getGUID();
    public caption!:string;
    public date!:string
    public value!:string
    public timeSlots:TimeSlot[] = []
    public displayService!:DisplayService
    private bookingCache?:Booking
    public displayData(timeSlot:string){
        const booking = this.getTimeSlot(timeSlot)?.booking;
        if(!booking) return "";
        const state:BookingState = this.bookingCache?.id == booking.id
            ? BookingState.Same : BookingState.New;

        const result = this.displayService.displayDoctorData(booking, state);
        this.bookingCache = booking;
        return result;
    }

    public hasBooking(timeSlot:string){
        const ts = this.getTimeSlot(timeSlot);
        return !!ts?.booking;
    }

    public getTimeSlot(timeSlot:string): TimeSlot | undefined {
        return this.timeSlots.find(ts => ts.value == timeSlot);
    }

    public color(timeSlot:string){
        const ts = this.getTimeSlot(timeSlot);
        if(!ts?.booking) return;
        return ts.color;
    }

    get hasData(){
        return !!this.caption && !!this.date
    }
    constructor(init?:Partial<Day>) {
        Object.assign(this,init)
    }
}


