type FrontTooth = 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18;
type UpperMolar = 21 | 22 | 23 | 24 | 25 | 26 | 27 | 28;
type LowerMolar = 31 | 32 | 33 | 34 | 35 | 36 | 37 | 38;
type FrontLowerTooth = 41 | 42 | 43 | 44 | 45 | 46 | 47 | 48;

export type ToothPosition = FrontTooth | UpperMolar | LowerMolar | FrontLowerTooth;

export class ToothStateModel {
  public position!:ToothPosition
  public color!:string

  constructor(init?:Partial<ToothStateModel>) {
    Object.assign(this, init)
  }
}