import {EditorDialogConfig} from "~/modules/editor/models/EditorDialogConfig";
import {
  DateFieldAttribute, InputFieldAttribute,
  MaskField,
  MaskFieldConfig, SelectField, SelectFieldConfig, SelectMode,
  StringField, TitleField
} from "~/modules/app-form";
import {IListDataSource} from "~/modules/data-sources";
import {getPatientFormActions} from "~/modules/patient/forms/getPatientFormActions";
import {UserProfile} from "~/modules/user-profile/UserProfile";
import {IUpdateFieldService} from "~/modules/app-form/services/IUpdateFieldService";

export const getPatientUpdateItemForm = (model:UserProfile,dataSource:IUpdateFieldService, informationSourceDataSource:IListDataSource<any>) => {
  const actions = getPatientFormActions(informationSourceDataSource);
  return new EditorDialogConfig({
    title: 'Обновить данные',
    model,
    fields: [
      new TitleField("fullName", {
        attrs: new InputFieldAttribute({
          caption: model.fullName,
          cssClass:"mb-3",
        }),
      }),
      new MaskField("birthDate", {
        attrs: new DateFieldAttribute({
          placeholder: "Введите дату рождения",
          caption: "Дата рождения",
        }),
        get:"getDate",
        set:"setDate",
        config: new MaskFieldConfig({
          mask: "99.99.9999",
        }),
        grid: "col-12 md:col-6",
      }),
      new MaskField("whatsAppNumber", {
        attrs: new InputFieldAttribute({
          caption: "WhatsApp номер",
          placeholder: "Введите WhatsApp номер",
        }),
        config: new MaskFieldConfig({
          mask: "+996 (999) 99-99-99",
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
      }),
    ],
    actions
  });
}
