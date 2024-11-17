import {
  EmptyField,
  InputFieldAttribute,
  NumberField,
  SelectField,
  SelectFieldConfig, SelectMode,
  StringField,
  Watcher
} from "~/modules/app-form";
import {EditorDialogConfig} from "~/modules/editor/models/EditorDialogConfig";

const colors = [
  {
    id:0,
    bgColor:"#8EF9E3",
    textColor:"#1CA085"
  },
  {
    id:1,
    bgColor:"#33FDBD",
    textColor:"#134938"
  },
  {
    id:2,
    bgColor:"#E7BAEA",
    textColor:"#5F3C61"
  },
  {
    id:3,
    bgColor:"#B2E0FF",
    textColor:"#3398DB"
  },
  {
    id:4,
    bgColor:"#FFA0B1",
    textColor:"#F24463"
  },
  {
    id:5,
    bgColor:"#C484DE",
    textColor:"#8E43AD"
  },
  {
    id:6,
    bgColor:"#FAD17D",
    textColor:"#9C7627"
  },
  {
    id:7,
    bgColor:"#93ACC6",
    textColor:"#3D556E"
  },
  {
    id:8,
    bgColor:"#FFB9E4",
    textColor:"#F340AF"
  },
  {
    id:9,
    bgColor:"#FFEDA4",
    textColor:"#F2C511"
  },
  {
    id:10,
    bgColor:"#D4E8A5",
    textColor:"#AEBE89"
  }
];
const actions = {
  getColors:()=> colors.map(c => c.textColor),
  watchTextColor:(model: any) => {
    model.bgColor = colors.find(c => c.textColor == model.textColor)?.bgColor
  },
}

const fields = [
  new EmptyField("bgColor",{
    watchers:[
      new Watcher({
        fields: ["textColor"],
        callOnMounted: true,
        changeModel: "watchTextColor",
      })
    ]
  }),
  new StringField("caption",{
    attrs:new InputFieldAttribute({
      caption:"Нименование"
    }),
    grid:"col-8"
  }),
  new NumberField("priority",{
    attrs:new InputFieldAttribute({
      caption:"Приоритет"
    }),
    grid:"col-2"
  }),
  new SelectField("textColor",{
    attrs: new InputFieldAttribute({
      caption:"Условный цвет"
    }),
    config: new SelectFieldConfig({
      getItems:"getColors",
      mode:SelectMode.Color,
    })
  }),
]

export const getServiceGroupConfig = (model:any, title:string) =>{
  return new EditorDialogConfig({
    title,
    fields:fields,
    model,
    actions:actions
  })
}
