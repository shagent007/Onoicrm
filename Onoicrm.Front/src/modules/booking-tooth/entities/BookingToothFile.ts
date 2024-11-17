import {Entity} from "~/shared/entities/Entity";

export class BookingToothFile extends Entity{
  name: string | null = '';
  description: string | null = '';
  fileSize: number | null = null;
  extension: string | null = '';
  base64: string | null = '';

  constructor(data: Partial<BookingToothFile>) {
    super();
    Object.assign(this, data);
  }
}
