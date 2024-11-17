import {IDoctorObjectDataSource} from "~/modules/doctor/interfaces/IDoctorObjectDataSource";
import {ObjectDataSourceConfig, useObjectDataSource} from "~/modules/data-sources";
import axios from "axios";
import {checkDuplicateAuthError} from "~/modules/data-sources/checkDuplicateAuthError";
import {ref} from "vue";

export const useDoctorObjectDataSource = (config:ObjectDataSourceConfig):IDoctorObjectDataSource => {
  const dataSource = useObjectDataSource({
    ...config,
    className:"userProfile"
  })

  const invoices = ref<any[]>([]);

  const  getBookingCount =async (dateTime:string) => {
    try {
      const {data} = await axios.get(`/api/v1/public/userProfile/${config.id}/booking-count?dateTime=${dateTime}`);
      return data;
    } catch (e) {
      await checkDuplicateAuthError(e);
    }
  }
  const  getInvoices =async (clinicId:number) => {
    try {
      const {data} = await axios.get(`/api/v1/public/userProfile/${config.id}/doctor/invoice/${clinicId}/service-percent`);
      invoices.value = data;
      console.log(invoices.value)
      return data;
    } catch (e) {
      await checkDuplicateAuthError(e);
    }
  }


  return  {
    ...dataSource,
    invoices,
    getInvoices,
    getBookingCount
  }

}
