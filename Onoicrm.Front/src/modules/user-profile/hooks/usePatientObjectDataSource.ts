import {
  ObjectDataSourceConfig,
  useObjectDataSource,
} from "~/modules/data-sources";

import axios from "axios";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import { IPatientObjectDataSource } from "~/modules/user-profile/interfaces/IPatientObjectDataSource";
import { Ref, ref } from "vue";
import { ToothStateModel } from "~/modules/tooth/models/ToothStateModel";

export const usePatientObjectDataSource = (
  config: Partial<ObjectDataSourceConfig> = new ObjectDataSourceConfig(),
): IPatientObjectDataSource => {
  const clinicStore = useClinicStore();
  const invoices = ref<any[]>([]);
  const dataSource = useObjectDataSource(
    new ObjectDataSourceConfig({
      ...config,
      className: "patient",
    }),
  );
  const toothStates: Ref<ToothStateModel[]> = ref<ToothStateModel[]>([]);
  const getToothStates = async (clinicId: number) => {
    const { data } = await axios.get(
      `/api/v1/public/patient/${dataSource.config.value.id}/tooth-states?clinicId=${clinicId}`,
    );
    toothStates.value = data;
  };

  const getInvoices = async () => {
    try {
      const {data} = await axios.get(`/api/v1/public/patient/${dataSource.config.value.id}/invoice/${clinicStore.clinic?.id}`);
      invoices.value = data;
      return data
    } catch (e) {
      throw e;
    }
  }

  const updateInvoices = async (groupId:number) => {
    try {
      const {data} = await axios.get(`/api/v1/public/patient/${dataSource.config.value.id}/invoice/${clinicStore.clinic?.id}/groupId/${groupId}`);
      invoices.value = data;
      return data
    } catch (e) {
      throw e;
    }
  }

  return {
    ...dataSource,
    getToothStates,
    updateInvoices,
    toothStates,
    getInvoices,
    invoices
  };
};
