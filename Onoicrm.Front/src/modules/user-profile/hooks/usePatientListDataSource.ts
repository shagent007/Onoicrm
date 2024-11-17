import {IPatientListDataSource} from "~/modules/user-profile/interfaces/IPatientListDataSource";
import {Filter, ListDataSourceConfig, useListDataSource} from "~/modules/data-sources";

import axios from "axios";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {CategoryMailingModel} from "~/modules/user-profile/models/CategoryMailingModel";
import {checkDuplicateAuthError} from "~/modules/data-sources/checkDuplicateAuthError";

export const usePatientListDataSource = (config:Partial<ListDataSourceConfig>=new ListDataSourceConfig()):IPatientListDataSource => {
  const clinicStore = useClinicStore();
  const dataSource = useListDataSource(
    new ListDataSourceConfig({
      ...config,
      className:"patient",
      orderFieldDirection:"DESC",
      orderFieldName:"LastUpdate",
      fields:"new(FullName, Id, WhatsAppNumber, BirthDate,GenderId)",
    })
  )

  dataSource.filter.value = [...dataSource.filter.value,
    new Filter("roleNames", "Patient"),
    new Filter("clinicId", clinicStore.clinic?.id),
  ]

  const mailingByCategory = async(model:CategoryMailingModel) => {
    try {
      const {data} = await axios.post(`/api/v1/public/userProfile/mailing/category/`,model)
      return data;
    } catch (error){
      await checkDuplicateAuthError(error);
    }

  }

  return {...dataSource,mailingByCategory}

}
