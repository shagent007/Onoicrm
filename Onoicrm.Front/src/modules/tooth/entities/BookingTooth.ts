import {BookingToothFile} from "~/modules/booking-tooth/entities/BookingToothFile";
import {Entity} from "~/shared/entities/Entity";

export class BookingTooth extends Entity{
  public caption: string = "";
  public description: string = "";
  public bookingId!: number
  public toothStateId!: number
  public toothId!: number
  public files:BookingToothFile[] = []
  public createDate!:string
  constructor(init?: Partial<BookingTooth>) {
    super();
    Object.assign(this, init)
  }
}
