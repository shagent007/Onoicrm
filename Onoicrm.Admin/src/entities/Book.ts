import { Entity } from './Entity';

export class Book extends Entity {
    public title: string;
    public description: string;
    public dateTime: string;
    public factory: string;
    public author: string;
    public text: string;
}
