export class SelectedArea {
  public dates!:string[]
  public times!:string[]


  constructor(init?:Partial<SelectedArea>) {
    if (init){
      Object.assign(this,init)
    }
  }
}
