export class CategoryMailingModel {
  public categories:number[] = [];
  public text:string = "";
  public clinicId:number;

  constructor(clinicId:number) {
    this.clinicId = clinicId;
  }
}
