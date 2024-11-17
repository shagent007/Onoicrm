import {Booking} from "~/modules/booking";

export enum DisplayCounter {
  Patient,
  Doctor,
  Armchair,
  Service,
  End
}

export enum BookingState{
  Same,
  New,
  Null
}

export class DisplayService{
  public doctorCounter:DisplayCounter = DisplayCounter.Patient;
  public armchairCounter:DisplayCounter = DisplayCounter.Patient;
  public armchairs:any[] = []


  constructor(armchairs:any[]) {
    this.armchairs = armchairs;
  }
  public displayDoctorData(booking:Booking, state:BookingState){
    if(state == BookingState.New){
      this.doctorCounter = DisplayCounter.Patient;
      return booking.patient?.fullName
    }

    switch (this.doctorCounter) {
      case DisplayCounter.Patient:{
        this.doctorCounter = DisplayCounter.End;
        return this.armchairs.find(a => a.id == booking.armchairId)?.caption
      }
    }
  }

  public displayArmchairData(booking:Booking, state:BookingState){
    if([BookingState.Null,BookingState.New].includes(state)){
      this.armchairCounter = DisplayCounter.Patient;
      return ""
    }

    switch (this.armchairCounter) {
      case DisplayCounter.Patient:{
        this.armchairCounter = DisplayCounter.Doctor;
        return booking.patient?.fullName
      }

      case DisplayCounter.Armchair:{
        this.armchairCounter = DisplayCounter.Patient;
        return booking.doctor?.fullName
      }
    }
  }
}
