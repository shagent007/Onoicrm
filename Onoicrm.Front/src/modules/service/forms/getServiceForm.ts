import {EditorDialogConfig} from "~/modules/editor/models/EditorDialogConfig";
import {Service} from "~/modules/service/entities/Service";
import {FieldAttribute, InputFieldAttribute, NumberField, StringField} from "~/modules/app-form";
import {required} from "~/shared/consts/consts";
import {CollectionField} from "~/modules/app-form/components/collections/models/CollectionField";
import {CollectionChipItem} from "~/modules/app-form/components/collections/items/models/CollectionChipItem";
import {ServicePriceType} from "~/modules/service/models/ServicePriceType";
import {IListDataSource, ITreeDataSource} from "~/modules/data-sources";
import {IServiceGroupLinkDataSource} from "~/modules/service-group-link/interfaces/IServiceGroupLinkDataSource";
import {getServiceLinkActions} from "~/modules/service-group-link/forms/getServiceLinkActions";

export const getServicePriceField = (type:ServicePriceType) => {
  switch (type) {
    case "general" : return  [
        new NumberField("price", {
          attrs: new InputFieldAttribute({
            caption: "Цена",
          }),
          grid:"col-12"
        }),
      ]
    case "preliminary": return [
      new NumberField("priceFrom", {
        attrs: new InputFieldAttribute({
          caption: "Цена от",
        }),
        grid:"col-12 md:col-6"
      }),
      new NumberField("priceTo", {
        attrs: new InputFieldAttribute({
          caption: "Цена до",
        }),
        grid:"col-12 md:col-6"
      }),
    ]
  }
}

export const getServiceCreateItemForm = (
  model:any,
  serviceGroupDataSource:ITreeDataSource,
  serviceGroupLinkDataSource:IServiceGroupLinkDataSource,
  priceType: ServicePriceType
) => {
  const priceFields = getServicePriceField(priceType);
  const actions = getServiceLinkActions(serviceGroupLinkDataSource, serviceGroupDataSource);
  return new EditorDialogConfig({
    title: "Добавить услугу",
    model,
    actions,
    fields:[
      new StringField("caption", {
       attrs: new InputFieldAttribute({
         caption: "Нименование",
       }),
       validations: [required()],
       grid:"col-12 md:col-6"
      }),
      new StringField("code", {
       attrs: new InputFieldAttribute({
         caption: "Код",
       }),
       validations: [required()],
       grid:"col-12 md:col-6"
     }),
      new StringField("labaratoryCaption", {
        attrs: new InputFieldAttribute({
          caption: "Лабораторная работа",
        }),
        grid:"col-12 md:col-6 flex-order-1"
      }),
      new NumberField("labaratoryPrice", {
        attrs: new InputFieldAttribute({
          caption: "Цена лабораторная работы",
        }),
        grid:"col-12 md:col-6 flex-order-2"
      }),
      ...priceFields
    ]
  })
}

export const getServiceUpdateItemForm = (
  model:any,
  dataSource:IListDataSource<any>,
  serviceGroupDataSource:ITreeDataSource,
  serviceGroupLinkDataSource:IServiceGroupLinkDataSource,
  priceType: ServicePriceType
) => {
  const priceFields = getServicePriceField(priceType);
  const actions = getServiceLinkActions(serviceGroupLinkDataSource, serviceGroupDataSource);
  return new EditorDialogConfig({
    title: "Обновить данные",
    model,
    fields:[
      new StringField("caption", {
        attrs: new InputFieldAttribute({
          caption: "Нименование",
        }),
        validations: [required()],
        grid:"col-12 md:col-6"
      }),
      new StringField("code", {
        attrs: new InputFieldAttribute({
          caption: "Код",
        }),
        validations: [required()],
        grid:"col-12 md:col-6"
      }),

      ...priceFields,
      new StringField("labaratoryCaption", {
        attrs: new InputFieldAttribute({
          caption: "Лабораторная работа",
        }),
        grid:"col-12 md:col-6"
      }),
      new NumberField("labaratoryPrice", {
        attrs: new InputFieldAttribute({
          caption: "Цена лабораторная работы",
        }),
        grid:"col-12 md:col-6"
      }),
      new CollectionField("links", {
        attrs: new FieldAttribute({
          caption: "Категории",
        }),
        getConfig: "getServiceLinkConfig",
        getItemConfig: "getServiceLinkItemConfig",
        customAdd: "addServiceLink",
        customDelete: "deleteServiceLink",
        selector: "tree",
        getCustomExcludeCallBack: "getServiceLinkFilterGroups",
        item: new CollectionChipItem({
          attrs: {
            class: "mr-2 mb-2",
          },
        }),
      })
    ],
    actions,
    updateFieldService: dataSource,
  })
}
