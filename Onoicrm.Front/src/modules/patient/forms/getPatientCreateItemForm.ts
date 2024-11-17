import {EditorDialogConfig} from "~/modules/editor/models/EditorDialogConfig";
import {
  DateFieldAttribute,
  InputFieldAttribute,
  MaskField,
  MaskFieldConfig, SelectField, SelectFieldConfig, SelectMode,
  StringField,
  Watcher
} from "~/modules/app-form";
import {required} from "~/shared/consts/consts";
import {PhoneField} from "~/modules/app-form/components/editors/phone/models/PhoneField";
import {IListDataSource} from "~/modules/data-sources";
import {getPatientFormActions} from "~/modules/patient/forms/getPatientFormActions";

export const getPatientCreateItemForm = (model:any, informationSourceDataSource:IListDataSource<any>) => {
  const actions = getPatientFormActions(informationSourceDataSource);
  return new EditorDialogConfig({
    title: "Добавить нового пациента",
    model,
    fields: [
      new StringField("fullName", {
        attrs: new InputFieldAttribute({
          placeholder: "Введите ФИО",
          caption: "ФИО",
        }),
        watchers: [
          new Watcher({
            changeModel: "searchPatient",
            fields: ["fullName"],
          }),
        ],
        validations: [required()],
        grid: "col-12 md:col-6",
      }),
      new MaskField("birthDate", {
        attrs: new DateFieldAttribute({
          placeholder: "Дата рождения",
          caption: "Введите дату рождения",
        }),
        get:"getDate",
        set:"setRegisterDate",
        config: new MaskFieldConfig({
          mask: "99.99.9999",
        }),
        grid: "col-12 md:col-6",
      }),
      new PhoneField("whatsAppNumber", {
        attrs: new InputFieldAttribute({
          caption: "Номер телефона",
          placeholder: "Напишите номер телефона",
        }),
        grid: "col-12 md:col-6",
      }),
      new SelectField("genderId", {
        attrs: new InputFieldAttribute({
          caption: "Пол",
          cssClass: "mb-3",
        }),
        config: new SelectFieldConfig({
          getItems: "getGenders",
          labelKeyName: "caption",
          valueKeyName: "value",
          mode: SelectMode.BtnGroup,
        }),
        grid: "col-12 md:col-6",
      }),
      new StringField("address", {
        attrs: new InputFieldAttribute({
          caption: "Адрес",
          placeholder: "Введите адрес",
        }),
        grid: "col-12 md:col-6",
      }),
      new SelectField("informationSourceId", {
        attrs: new InputFieldAttribute({
          caption: "От куда вы о нас узнали?",
          placeholder: "Напишите о том откуда вы о нас узнали",
        }),
        config: new SelectFieldConfig({
          getItems: "getInformationSources",
          labelKeyName: "caption",
          valueKeyName: "id",
        }),
        grid: "col-12 md:col-6",
      }),
    ],
    actions,
  });
}
