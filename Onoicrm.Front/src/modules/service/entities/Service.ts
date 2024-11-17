import {Entity} from "~/shared/entities/Entity";

export class Service extends Entity{
  caption: string = '';
  description: string = '';
  cssClass: string = '';
  color: string = '';
  price: number = 0;
  currency: string = '';
  clinicId: number = 0;
  parentId:number | null = null
  links:any[] = []
  constructor(data?: Partial<Service>) {
    super();
    if (data) {
      Object.assign(this, data);
    }
  }

}



