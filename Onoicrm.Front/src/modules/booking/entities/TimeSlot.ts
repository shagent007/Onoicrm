import {Booking} from "~/modules/booking";

export class TimeSlot {
  public value!: string
  public booking!: Booking;
  public color!: string;

  public get hasBooking() {
    return !!this.booking
  }

  public setColor(services: any[]): void {
    if (!this.hasBooking) return;
    const service = services.find(s => this.booking.bookingServices.some(bs => bs.serviceId == s.id));
    if (!service) return;
    this.color = service.color;
  }

  constructor(init?: Partial<TimeSlot>) {
    Object.assign(this, init)
  }
}
